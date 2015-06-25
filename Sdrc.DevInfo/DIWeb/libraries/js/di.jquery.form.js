/*!
 * jQuery Form Plugin
 * version: 2.82 (15-JUN-2011)
 * @requires jQuery v1.3.2 or later
 *
 * Examples and documentation at: http://malsup.com/jquery/form/
 * Dual licensed under the MIT and GPL licenses:
 *   http://www.opensource.org/licenses/mit-license.php
 *   http://www.gnu.org/licenses/gpl.html
 */
;(function(jq) {

/*
	Usage Note:
	-----------
	Do not use both ajaxSubmit and ajaxForm on the same form.  These
	functions are intended to be exclusive.  Use ajaxSubmit if you want
	to bind your own submit handler to the form.  For example,

	jq(document).ready(function() {
		jq('#myForm').bind('submit', function(e) {
			e.preventDefault(); // <-- important
			jq(this).ajaxSubmit({
				target: '#output'
			});
		});
	});

	Use ajaxForm when you want the plugin to manage all the event binding
	for you.  For example,

	jq(document).ready(function() {
		jq('#myForm').ajaxForm({
			target: '#output'
		});
	});

	When using ajaxForm, the ajaxSubmit function will be invoked for you
	at the appropriate time.
*/

/**
 * ajaxSubmit() provides a mechanism for immediately submitting
 * an HTML form using AJAX.
 */
jq.fn.ajaxSubmit = function(options) {
	// fast fail if nothing selected (http://dev.jquery.com/ticket/2752)
	if (!this.length) {
		log('ajaxSubmit: skipping submit process - no element selected');
		return this;
	}
	
	var method, action, url, jqform = this;;

	if (typeof options == 'function') {
		options = { success: options };
	}

	method = this.attr('method');
	action = this.attr('action');
	url = (typeof action === 'string') ? jq.trim(action) : '';
	url = url || window.location.href || '';
	if (url) {
		// clean url (don't include hash vaue)
		url = (url.match(/^([^#]+)/)||[])[1];
	}

	options = jq.extend(true, {
		url:  url,
		success: jq.ajaxSettings.success,
		type: method || 'GET',
		iframeSrc: /^https/i.test(window.location.href || '') ? 'javascript:false' : 'about:blank'
	}, options);

	// hook for manipulating the form data before it is extracted;
	// convenient for use with rich editors like tinyMCE or FCKEditor
	var veto = {};
	this.trigger('form-pre-serialize', [this, options, veto]);
	if (veto.veto) {
		log('ajaxSubmit: submit vetoed via form-pre-serialize trigger');
		return this;
	}

	// provide opportunity to alter form data before it is serialized
	if (options.beforeSerialize && options.beforeSerialize(this, options) === false) {
		log('ajaxSubmit: submit aborted via beforeSerialize callback');
		return this;
	}

	var n,v,a = this.formToArray(options.semantic);
	if (options.data) {
		options.extraData = options.data;
		for (n in options.data) {
			if(options.data[n] instanceof Array) {
				for (var k in options.data[n]) {
					a.push( { name: n, value: options.data[n][k] } );
				}
			}
			else {
				v = options.data[n];
				v = jq.isFunction(v) ? v() : v; // if value is fn, invoke it
				a.push( { name: n, value: v } );
			}
		}
	}

	// give pre-submit callback an opportunity to abort the submit
	if (options.beforeSubmit && options.beforeSubmit(a, this, options) === false) {
		log('ajaxSubmit: submit aborted via beforeSubmit callback');
		return this;
	}

	// fire vetoable 'validate' event
	this.trigger('form-submit-validate', [a, this, options, veto]);
	if (veto.veto) {
		log('ajaxSubmit: submit vetoed via form-submit-validate trigger');
		return this;
	}

	var q = jq.param(a);

	if (options.type.toUpperCase() == 'GET') {
		options.url += (options.url.indexOf('?') >= 0 ? '&' : '?') + q;
		options.data = null;  // data is null for 'get'
	}
	else {
		options.data = q; // data is the query string for 'post'
	}

	var callbacks = [];
	if (options.resetForm) {
		callbacks.push(function() { jqform.resetForm(); });
	}
	if (options.clearForm) {
		callbacks.push(function() { jqform.clearForm(); });
	}

	// perform a load on the target only if dataType is not provided
	if (!options.dataType && options.target) {
		var oldSuccess = options.success || function(){};
		callbacks.push(function(data) {
			var fn = options.replaceTarget ? 'replaceWith' : 'html';
			jq(options.target)[fn](data).each(oldSuccess, arguments);
		});
	}
	else if (options.success) {
		callbacks.push(options.success);
	}

	options.success = function(data, status, xhr) { // jQuery 1.4+ passes xhr as 3rd arg
		var context = options.context || options;   // jQuery 1.4+ supports scope context 
		for (var i=0, max=callbacks.length; i < max; i++) {
			callbacks[i].apply(context, [data, status, xhr || jqform, jqform]);
		}
	};

	// are there files to upload?
	var fileInputs = jq('input:file', this).length > 0;
	var mp = 'multipart/form-data';
	var multipart = (jqform.attr('enctype') == mp || jqform.attr('encoding') == mp);

	// options.iframe allows user to force iframe mode
	// 06-NOV-09: now defaulting to iframe mode if file input is detected
   if (options.iframe !== false && (fileInputs || options.iframe || multipart)) {
	   // hack to fix Safari hang (thanks to Tim Molendijk for this)
	   // see:  http://groups.google.com/group/jquery-dev/browse_thread/thread/36395b7ab510dd5d
	   if (options.closeKeepAlive) {
		   jq.get(options.closeKeepAlive, function() { fileUpload(a); });
		}
	   else {
		   fileUpload(a);
		}
   }
   else {
		// IE7 massage (see issue 57)
		if (jq.browser.msie && method == 'get') { 
			var ieMeth = jqform[0].getAttribute('method');
			if (typeof ieMeth === 'string')
				options.type = ieMeth;
		}
		jq.ajax(options);
   }

	// fire 'notify' event
	this.trigger('form-submit-notify', [this, options]);
	return this;


	// private function for handling file uploads (hat tip to YAHOO!)
	function fileUpload(a) {
		var form = jqform[0], i, s, g, id, jqio, io, xhr, sub, n, timedOut, timeoutHandle;

        if (a) {
        	// ensure that every serialized input is still enabled
          	for (i=0; i < a.length; i++) {
            	jq(form[a[i].name]).attr('disabled', false);
          	}
        }

		if (jq(':input[name=submit],:input[id=submit]', form).length) {
			// if there is an input with a name or id of 'submit' then we won't be
			// able to invoke the submit fn on the form (at least not x-browser)
			alert('Error: Form elements must not have name or id of "submit".');
			return;
		}
		
		s = jq.extend(true, {}, jq.ajaxSettings, options);
		s.context = s.context || s;
		id = 'jqFormIO' + (new Date().getTime());
		if (s.iframeTarget) {
			jqio = jq(s.iframeTarget);
			n = jqio.attr('name');
			if (n == null)
			 	jqio.attr('name', id);
			else
				id = n;
		}
		else {
			jqio = jq('<iframe name="' + id + '" src="'+ s.iframeSrc +'" />');
			jqio.css({ position: 'absolute', top: '-1000px', left: '-1000px' });
		}
		io = jqio[0];


		xhr = { // mock object
			aborted: 0,
			responseText: null,
			responseXML: null,
			status: 0,
			statusText: 'n/a',
			getAllResponseHeaders: function() {},
			getResponseHeader: function() {},
			setRequestHeader: function() {},
			abort: function(status) {
				var e = (status === 'timeout' ? 'timeout' : 'aborted');
				log('aborting upload... ' + e);
				this.aborted = 1;
				jqio.attr('src', s.iframeSrc); // abort op in progress
				xhr.error = e;
				s.error && s.error.call(s.context, xhr, e, status);
				g && jq.event.trigger("ajaxError", [xhr, s, e]);
				s.complete && s.complete.call(s.context, xhr, e);
			}
		};

		g = s.global;
		// trigger ajax global events so that activity/block indicators work like normal
		if (g && ! jq.active++) {
			jq.event.trigger("ajaxStart");
		}
		if (g) {
			jq.event.trigger("ajaxSend", [xhr, s]);
		}

		if (s.beforeSend && s.beforeSend.call(s.context, xhr, s) === false) {
			if (s.global) {
				jq.active--;
			}
			return;
		}
		if (xhr.aborted) {
			return;
		}

		// add submitting element to data if we know it
		sub = form.clk;
		if (sub) {
			n = sub.name;
			if (n && !sub.disabled) {
				s.extraData = s.extraData || {};
				s.extraData[n] = sub.value;
				if (sub.type == "image") {
					s.extraData[n+'.x'] = form.clk_x;
					s.extraData[n+'.y'] = form.clk_y;
				}
			}
		}
		
		var CLIENT_TIMEOUT_ABORT = 1;
		var SERVER_ABORT = 2;

		function getDoc(frame) {
			var doc = frame.contentWindow ? frame.contentWindow.document : frame.contentDocument ? frame.contentDocument : frame.document;
			return doc;
		}
		
		// take a breath so that pending repaints get some cpu time before the upload starts
		function doSubmit() {
			// make sure form attrs are set
			var t = jqform.attr('target'), a = jqform.attr('action');

			// update form attrs in IE friendly way
			form.setAttribute('target',id);
			if (!method) {
				form.setAttribute('method', 'POST');
			}
			if (a != s.url) {
				form.setAttribute('action', s.url);
			}

			// ie borks in some cases when setting encoding
			if (! s.skipEncodingOverride && (!method || /post/i.test(method))) {
				jqform.attr({
					encoding: 'multipart/form-data',
					enctype:  'multipart/form-data'
				});
			}

			// support timout
			if (s.timeout) {
				timeoutHandle = setTimeout(function() { timedOut = true; cb(CLIENT_TIMEOUT_ABORT); }, s.timeout);
			}
			
			// look for server aborts
			function checkState() {
				try {
					var state = getDoc(io).readyState;
					log('state = ' + state);
					if (state.toLowerCase() == 'uninitialized')
						setTimeout(checkState,50);
				}
				catch(e) {
					log('Server abort: ' , e, ' (', e.name, ')');
					cb(SERVER_ABORT);
					timeoutHandle && clearTimeout(timeoutHandle);
					timeoutHandle = undefined;
				}
			}

			// add "extra" data to form if provided in options
			var extraInputs = [];
			try {
				if (s.extraData) {
					for (var n in s.extraData) {
						extraInputs.push(
							jq('<input type="hidden" name="'+n+'" />').attr('value',s.extraData[n])
								.appendTo(form)[0]);
					}
				}

				if (!s.iframeTarget) {
					// add iframe to doc and submit the form
					jqio.appendTo('body');
	                io.attachEvent ? io.attachEvent('onload', cb) : io.addEventListener('load', cb, false);
				}
				setTimeout(checkState,15);
				form.submit();
			}
			finally {
				// reset attrs and remove "extra" input elements
				form.setAttribute('action',a);
				if(t) {
					form.setAttribute('target', t);
				} else {
					jqform.removeAttr('target');
				}
				jq(extraInputs).remove();
			}
		}

		if (s.forceSync) {
			doSubmit();
		}
		else {
			setTimeout(doSubmit, 10); // this lets dom updates render
		}

		var data, doc, domCheckCount = 50, callbackProcessed;

		function cb(e) {
			if (xhr.aborted || callbackProcessed) {
				return;
			}
			try {
				doc = getDoc(io);
			}
			catch(ex) {
				log('cannot access response document: ', ex);
				e = SERVER_ABORT;
			}
			if (e === CLIENT_TIMEOUT_ABORT && xhr) {
				xhr.abort('timeout');
				return;
			}
			else if (e == SERVER_ABORT && xhr) {
				xhr.abort('server abort');
				return;
			}

			if (!doc || doc.location.href == s.iframeSrc) {
				// response not received yet
				if (!timedOut)
					return;
			}
            io.detachEvent ? io.detachEvent('onload', cb) : io.removeEventListener('load', cb, false);

			var status = 'success', errMsg;
			try {
				if (timedOut) {
					throw 'timeout';
				}

				var isXml = s.dataType == 'xml' || doc.XMLDocument || jq.isXMLDoc(doc);
				log('isXml='+isXml);
				if (!isXml && window.opera && (doc.body == null || doc.body.innerHTML == '')) {
					if (--domCheckCount) {
						// in some browsers (Opera) the iframe DOM is not always traversable when
						// the onload callback fires, so we loop a bit to accommodate
						log('requeing onLoad callback, DOM not available');
						setTimeout(cb, 250);
						return;
					}
					// let this fall through because server response could be an empty document
					//log('Could not access iframe DOM after mutiple tries.');
					//throw 'DOMException: not available';
				}

				//log('response detected');
                var docRoot = doc.body ? doc.body : doc.documentElement;
                xhr.responseText = docRoot ? docRoot.innerHTML : null;
				xhr.responseXML = doc.XMLDocument ? doc.XMLDocument : doc;
				if (isXml)
					s.dataType = 'xml';
				xhr.getResponseHeader = function(header){
					var headers = {'content-type': s.dataType};
					return headers[header];
				};
                // support for XHR 'status' & 'statusText' emulation :
                if (docRoot) {
                    xhr.status = Number( docRoot.getAttribute('status') ) || xhr.status;
                    xhr.statusText = docRoot.getAttribute('statusText') || xhr.statusText;
                }

				var dt = s.dataType || '';
				var scr = /(json|script|text)/.test(dt.toLowerCase());
				if (scr || s.textarea) {
					// see if user embedded response in textarea
					var ta = doc.getElementsByTagName('textarea')[0];
					if (ta) {
						xhr.responseText = ta.value;
                        // support for XHR 'status' & 'statusText' emulation :
                        xhr.status = Number( ta.getAttribute('status') ) || xhr.status;
                        xhr.statusText = ta.getAttribute('statusText') || xhr.statusText;
					}
					else if (scr) {
						// account for browsers injecting pre around json response
						var pre = doc.getElementsByTagName('pre')[0];
						var b = doc.getElementsByTagName('body')[0];
						if (pre) {
							xhr.responseText = pre.textContent ? pre.textContent : pre.innerHTML;
						}
						else if (b) {
							xhr.responseText = b.innerHTML;
						}
					}
				}
				else if (s.dataType == 'xml' && !xhr.responseXML && xhr.responseText != null) {
					xhr.responseXML = toXml(xhr.responseText);
				}

                try {
                    data = httpData(xhr, s.dataType, s);
                }
                catch (e) {
                    status = 'parsererror';
                    xhr.error = errMsg = (e || status);
                }
			}
			catch (e) {
				log('error caught: ',e);
				status = 'error';
                xhr.error = errMsg = (e || status);
			}

			if (xhr.aborted) {
				log('upload aborted');
				status = null;
			}

            if (xhr.status) { // we've set xhr.status
                status = (xhr.status >= 200 && xhr.status < 300 || xhr.status === 304) ? 'success' : 'error';
            }

			// ordering of these callbacks/triggers is odd, but that's how jq.ajax does it
			if (status === 'success') {
				s.success && s.success.call(s.context, data, 'success', xhr);
				g && jq.event.trigger("ajaxSuccess", [xhr, s]);
			}
            else if (status) {
				if (errMsg == undefined)
					errMsg = xhr.statusText;
				s.error && s.error.call(s.context, xhr, status, errMsg);
				g && jq.event.trigger("ajaxError", [xhr, s, errMsg]);
            }

			g && jq.event.trigger("ajaxComplete", [xhr, s]);

			if (g && ! --jq.active) {
				jq.event.trigger("ajaxStop");
			}

			s.complete && s.complete.call(s.context, xhr, status);

			callbackProcessed = true;
			if (s.timeout)
				clearTimeout(timeoutHandle);

			// clean up
			setTimeout(function() {
				if (!s.iframeTarget)
					jqio.remove();
				xhr.responseXML = null;
			}, 100);
		}

		var toXml = jq.parseXML || function(s, doc) { // use parseXML if available (jQuery 1.5+)
			if (window.ActiveXObject) {
				doc = new ActiveXObject('Microsoft.XMLDOM');
				doc.async = 'false';
				doc.loadXML(s);
			}
			else {
				doc = (new DOMParser()).parseFromString(s, 'text/xml');
			}
			return (doc && doc.documentElement && doc.documentElement.nodeName != 'parsererror') ? doc : null;
		};
		var parseJSON = jq.parseJSON || function(s) {
			return window['eval']('(' + s + ')');
		};

		var httpData = function( xhr, type, s ) { // mostly lifted from jq1.4.4

			var ct = xhr.getResponseHeader('content-type') || '',
				xml = type === 'xml' || !type && ct.indexOf('xml') >= 0,
				data = xml ? xhr.responseXML : xhr.responseText;

			if (xml && data.documentElement.nodeName === 'parsererror') {
				jq.error && jq.error('parsererror');
			}
			if (s && s.dataFilter) {
				data = s.dataFilter(data, type);
			}
			if (typeof data === 'string') {
				if (type === 'json' || !type && ct.indexOf('json') >= 0) {
					data = parseJSON(data);
				} else if (type === "script" || !type && ct.indexOf("javascript") >= 0) {
					jq.globalEval(data);
				}
			}
			return data;
		};
	}
};

/**
 * ajaxForm() provides a mechanism for fully automating form submission.
 *
 * The advantages of using this method instead of ajaxSubmit() are:
 *
 * 1: This method will include coordinates for <input type="image" /> elements (if the element
 *	is used to submit the form).
 * 2. This method will include the submit element's name/value data (for the element that was
 *	used to submit the form).
 * 3. This method binds the submit() method to the form for you.
 *
 * The options argument for ajaxForm works exactly as it does for ajaxSubmit.  ajaxForm merely
 * passes the options argument along after properly binding events for submit elements and
 * the form itself.
 */
jq.fn.ajaxForm = function(options) {
	// in jQuery 1.3+ we can fix mistakes with the ready state
	if (this.length === 0) {
		var o = { s: this.selector, c: this.context };
		if (!jq.isReady && o.s) {
			log('DOM not ready, queuing ajaxForm');
			jq(function() {
				jq(o.s,o.c).ajaxForm(options);
			});
			return this;
		}
		// is your DOM ready?  http://docs.jquery.com/Tutorials:Introducing_jq(document).ready()
		log('terminating; zero elements found by selector' + (jq.isReady ? '' : ' (DOM not ready)'));
		return this;
	}

	return this.ajaxFormUnbind().bind('submit.form-plugin', function(e) {
		if (!e.isDefaultPrevented()) { // if event has been canceled, don't proceed
			e.preventDefault();
			jq(this).ajaxSubmit(options);
		}
	}).bind('click.form-plugin', function(e) {
		var target = e.target;
		var jqel = jq(target);
		if (!(jqel.is(":submit,input:image"))) {
			// is this a child element of the submit el?  (ex: a span within a button)
			var t = jqel.closest(':submit');
			if (t.length == 0) {
				return;
			}
			target = t[0];
		}
		var form = this;
		form.clk = target;
		if (target.type == 'image') {
			if (e.offsetX != undefined) {
				form.clk_x = e.offsetX;
				form.clk_y = e.offsetY;
			} else if (typeof jq.fn.offset == 'function') { // try to use dimensions plugin
				var offset = jqel.offset();
				form.clk_x = e.pageX - offset.left;
				form.clk_y = e.pageY - offset.top;
			} else {
				form.clk_x = e.pageX - target.offsetLeft;
				form.clk_y = e.pageY - target.offsetTop;
			}
		}
		// clear form vars
		setTimeout(function() { form.clk = form.clk_x = form.clk_y = null; }, 100);
	});
};

// ajaxFormUnbind unbinds the event handlers that were bound by ajaxForm
jq.fn.ajaxFormUnbind = function() {
	return this.unbind('submit.form-plugin click.form-plugin');
};

/**
 * formToArray() gathers form element data into an array of objects that can
 * be passed to any of the following ajax functions: jq.get, jq.post, or load.
 * Each object in the array has both a 'name' and 'value' property.  An example of
 * an array for a simple login form might be:
 *
 * [ { name: 'username', value: 'jresig' }, { name: 'password', value: 'secret' } ]
 *
 * It is this array that is passed to pre-submit callback functions provided to the
 * ajaxSubmit() and ajaxForm() methods.
 */
jq.fn.formToArray = function(semantic) {
	var a = [];
	if (this.length === 0) {
		return a;
	}

	var form = this[0];
	var els = semantic ? form.getElementsByTagName('*') : form.elements;
	if (!els) {
		return a;
	}

	var i,j,n,v,el,max,jmax;
	for(i=0, max=els.length; i < max; i++) {
		el = els[i];
		n = el.name;
		if (!n) {
			continue;
		}

		if (semantic && form.clk && el.type == "image") {
			// handle image inputs on the fly when semantic == true
			if(!el.disabled && form.clk == el) {
				a.push({name: n, value: jq(el).val()});
				a.push({name: n+'.x', value: form.clk_x}, {name: n+'.y', value: form.clk_y});
			}
			continue;
		}

		v = jq.fieldValue(el, true);
		if (v && v.constructor == Array) {
			for(j=0, jmax=v.length; j < jmax; j++) {
				a.push({name: n, value: v[j]});
			}
		}
		else if (v !== null && typeof v != 'undefined') {
			a.push({name: n, value: v});
		}
	}

	if (!semantic && form.clk) {
		// input type=='image' are not found in elements array! handle it here
		var jqinput = jq(form.clk), input = jqinput[0];
		n = input.name;
		if (n && !input.disabled && input.type == 'image') {
			a.push({name: n, value: jqinput.val()});
			a.push({name: n+'.x', value: form.clk_x}, {name: n+'.y', value: form.clk_y});
		}
	}
	return a;
};

/**
 * Serializes form data into a 'submittable' string. This method will return a string
 * in the format: name1=value1&amp;name2=value2
 */
jq.fn.formSerialize = function(semantic) {
	//hand off to jQuery.param for proper encoding
	return jq.param(this.formToArray(semantic));
};

/**
 * Serializes all field elements in the jQuery object into a query string.
 * This method will return a string in the format: name1=value1&amp;name2=value2
 */
jq.fn.fieldSerialize = function(successful) {
	var a = [];
	this.each(function() {
		var n = this.name;
		if (!n) {
			return;
		}
		var v = jq.fieldValue(this, successful);
		if (v && v.constructor == Array) {
			for (var i=0,max=v.length; i < max; i++) {
				a.push({name: n, value: v[i]});
			}
		}
		else if (v !== null && typeof v != 'undefined') {
			a.push({name: this.name, value: v});
		}
	});
	//hand off to jQuery.param for proper encoding
	return jq.param(a);
};

/**
 * Returns the value(s) of the element in the matched set.  For example, consider the following form:
 *
 *  <form><fieldset>
 *	  <input name="A" type="text" />
 *	  <input name="A" type="text" />
 *	  <input name="B" type="checkbox" value="B1" />
 *	  <input name="B" type="checkbox" value="B2"/>
 *	  <input name="C" type="radio" value="C1" />
 *	  <input name="C" type="radio" value="C2" />
 *  </fieldset></form>
 *
 *  var v = jq(':text').fieldValue();
 *  // if no values are entered into the text inputs
 *  v == ['','']
 *  // if values entered into the text inputs are 'foo' and 'bar'
 *  v == ['foo','bar']
 *
 *  var v = jq(':checkbox').fieldValue();
 *  // if neither checkbox is checked
 *  v === undefined
 *  // if both checkboxes are checked
 *  v == ['B1', 'B2']
 *
 *  var v = jq(':radio').fieldValue();
 *  // if neither radio is checked
 *  v === undefined
 *  // if first radio is checked
 *  v == ['C1']
 *
 * The successful argument controls whether or not the field element must be 'successful'
 * (per http://www.w3.org/TR/html4/interact/forms.html#successful-controls).
 * The default value of the successful argument is true.  If this value is false the value(s)
 * for each element is returned.
 *
 * Note: This method *always* returns an array.  If no valid value can be determined the
 *	   array will be empty, otherwise it will contain one or more values.
 */
jq.fn.fieldValue = function(successful) {
	for (var val=[], i=0, max=this.length; i < max; i++) {
		var el = this[i];
		var v = jq.fieldValue(el, successful);
		if (v === null || typeof v == 'undefined' || (v.constructor == Array && !v.length)) {
			continue;
		}
		v.constructor == Array ? jq.merge(val, v) : val.push(v);
	}
	return val;
};

/**
 * Returns the value of the field element.
 */
jq.fieldValue = function(el, successful) {
	var n = el.name, t = el.type, tag = el.tagName.toLowerCase();
	if (successful === undefined) {
		successful = true;
	}

	if (successful && (!n || el.disabled || t == 'reset' || t == 'button' ||
		(t == 'checkbox' || t == 'radio') && !el.checked ||
		(t == 'submit' || t == 'image') && el.form && el.form.clk != el ||
		tag == 'select' && el.selectedIndex == -1)) {
			return null;
	}

	if (tag == 'select') {
		var index = el.selectedIndex;
		if (index < 0) {
			return null;
		}
		var a = [], ops = el.options;
		var one = (t == 'select-one');
		var max = (one ? index+1 : ops.length);
		for(var i=(one ? index : 0); i < max; i++) {
			var op = ops[i];
			if (op.selected) {
				var v = op.value;
				if (!v) { // extra pain for IE...
					v = (op.attributes && op.attributes['value'] && !(op.attributes['value'].specified)) ? op.text : op.value;
				}
				if (one) {
					return v;
				}
				a.push(v);
			}
		}
		return a;
	}
	return jq(el).val();
};

/**
 * Clears the form data.  Takes the following actions on the form's input fields:
 *  - input text fields will have their 'value' property set to the empty string
 *  - select elements will have their 'selectedIndex' property set to -1
 *  - checkbox and radio inputs will have their 'checked' property set to false
 *  - inputs of type submit, button, reset, and hidden will *not* be effected
 *  - button elements will *not* be effected
 */
jq.fn.clearForm = function() {
	return this.each(function() {
		jq('input,select,textarea', this).clearFields();
	});
};

/**
 * Clears the selected form elements.
 */
jq.fn.clearFields = jq.fn.clearInputs = function() {
	var re = /^(?:color|date|datetime|email|month|number|password|range|search|tel|text|time|url|week)jq/i; // 'hidden' is not in this list
	return this.each(function() {
		var t = this.type, tag = this.tagName.toLowerCase();
		if (re.test(t) || tag == 'textarea') {
			this.value = '';
		}
		else if (t == 'checkbox' || t == 'radio') {
            if(t == 'radio')
            {
		      this.checked = false;
            }
            else if (t == 'checkbox')
            {
               this.defaultChecked = false;
            }
		}
		else if (tag == 'select') {
			this.selectedIndex = -1;
		}
	});
};

/**
 * Resets the form data.  Causes all form elements to be reset to their original value.
 */
jq.fn.resetForm = function() {
	return this.each(function() {
		// guard against an input with the name of 'reset'
		// note that IE reports the reset function as an 'object'
		if (typeof this.reset == 'function' || (typeof this.reset == 'object' && !this.reset.nodeType)) {
			this.reset();
		}
	});
};

/**
 * Enables or disables any matching elements.
 */
jq.fn.enable = function(b) {
	if (b === undefined) {
		b = true;
	}
	return this.each(function() {
		this.disabled = !b;
	});
};

/**
 * Checks/unchecks any matching checkboxes or radio buttons and
 * selects/deselects and matching option elements.
 */
jq.fn.selected = function(select) {
	if (select === undefined) {
		select = true;
	}
	return this.each(function() {
		var t = this.type;
		if (t == 'checkbox' || t == 'radio') {
        if (t == 'checkbox')
			{this.defaultChecked = select;}
        else if(t == 'radio')
        {this.checked = select;}
		}
		else if (this.tagName.toLowerCase() == 'option') {
			var jqsel = jq(this).parent('select');
			if (select && jqsel[0] && jqsel[0].type == 'select-one') {
				// deselect all other options
				jqsel.find('option').selected(false);
			}
			this.selected = select;
		}
	});
};

// helper fn for console logging
function log() {
	var msg = '[jquery.form] ' + Array.prototype.join.call(arguments,'');
	if (window.console && window.console.log) {
		window.console.log(msg);
	}
	else if (window.opera && window.opera.postError) {
		window.opera.postError(msg);
	}
};

})(jQuery);
