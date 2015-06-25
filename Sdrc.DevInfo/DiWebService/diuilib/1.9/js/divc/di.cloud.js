/*!
 * jQCloud Plugin for jQuery
 *
 * Version 0.2.4
 *
 * Copyright 2011, Luca Ongaro
 * Licensed under the MIT license.
 *
 * Date: Sun Aug 14 00:09:07 +0200 2011
 */ 
 
(function( di_jq ){
  di_jq.fn.jQCloud = function(word_array, options) {
    // Reference to the container element
    var di_jqthis = this;
    // Reference to the ID of the container element
    var container_id = di_jqthis.attr('id');

    var divCloud = document.getElementById(container_id);
    // Default options value
    var default_options = {
      width: di_jqthis.width(),
      height: di_jqthis.height(),
      center: {
        x: di_jqthis.width() / 2,
        y: di_jqthis.height() / 2
      },
      delayedMode: word_array.length > 50,
      randomClasses: 0
    };

    // Maintain backward compatibility with old API (pre 0.2.0), where the second argument of jQCloud was a callback function
    if (typeof options === 'function') {
      options = { callback: options }
    }

    options = di_jq.extend(default_options, options || {});

    // Add the "jqcloud" class to the container for easy CSS styling
    di_jqthis.addClass("jqcloud");

    var drawWordCloud = function() {
      // Helper function to test if an element overlaps others
      var hitTest = function(elem, other_elems){
        // Pairwise overlap detection
        var overlapping = function(a, b){
          if (Math.abs(2.0*a.offsetLeft + a.offsetWidth - 2.0*b.offsetLeft - b.offsetWidth) < a.offsetWidth + b.offsetWidth) {
            if (Math.abs(2.0*a.offsetTop + a.offsetHeight - 2.0*b.offsetTop - b.offsetHeight) < a.offsetHeight + b.offsetHeight) {
              return true;
            }
          }
          return false;
        };
        var i = 0;
        // Check elements for overlap one by one, stop and return false as soon as an overlap is found
        for(i = 0; i < other_elems.length; i++) {
          if (overlapping(elem, other_elems[i])) {
            return true;
          }
        }
        return false;
      };

      // Make sure every weight is a number before sorting
      for (i = 0; i < word_array.length; i++) {
        word_array[i].weight = parseFloat(word_array[i].weight, 10);
      }
      
      // Sort word_array from the word with the highest weight to the one with the lowest
      word_array.sort(function(a, b) { if (a.weight < b.weight) {return 1;} else if (a.weight > b.weight) {return -1;} else {return 0;} });

      var step = 2.0;
      var already_placed_words = [];
      var aspect_ratio = options.width / options.height;

      // Function to draw a word, by moving it in spiral until it finds a suitable empty place. This will be iterated on each word.
      var drawOneWord = function(index, word) {
        // Define the ID attribute of the span that will wrap the word, and the associated jQuery selector string
        var word_id = container_id + "_word_" + index;
        var word_selector = "#" + word_id;
        
        // If the option randomClasses is a number, and higher than 0, assign this word randomly to a class
        // of the kind 'r1', 'r2', 'rN' with N = randomClasses
        // If option randomClasses is an array, assign this word randomly to one of the classes in the array
        var random_class = (typeof options.randomClasses === "number" && options.randomClasses > 0)
          ? " r" + Math.ceil(Math.random()*options.randomClasses)
          : ((di_jq.isArray(options.randomClasses) && options.randomClasses.length > 0)
            ? " " + options.randomClasses[ Math.floor(Math.random()*options.randomClasses.length) ]
            : "");

        var angle = 6.28 * Math.random();
        var radius = 0.0;

        // Linearly map the original weight to a discrete scale from 1 to 10
        var weight = Math.round((word.weight - word_array[word_array.length - 1].weight)/(word_array[0].weight - word_array[word_array.length - 1].weight) * 9.0) + 1;

        var inner_html = word.url !== undefined ? "<a href='" + encodeURI(word.url).replace(/'/g, "%27") + "'>" + word.text + "</a>" : word.text;
        di_jqthis.append("<span id='" + word_id + "' class='w" + weight + random_class + "' title='" + (word.title || "") + "'>" + inner_html + "</span>&nbsp;&nbsp;");

        // Search for the word span only once, and within the context of the container, for better performance
        var word_span = di_jq(word_selector, di_jqthis);

        var width = word_span.width();
        var height = word_span.height();
        var left = options.center.x - width / 2.0;
        var top = options.center.y - height / 2.0;

        // Save a reference to the style property, for better performance
        var word_style = word_span[0].style;
        word_style.position = "absolute";
        word_style.left = left + "px";
        word_style.top = top + "px";

        while(hitTest(document.getElementById(word_id), already_placed_words)) {
          radius += step;
          angle += (index % 2 === 0 ? 1 : -1)*step;

          left = options.center.x - (width / 2.0) + (radius*Math.cos(angle)) * aspect_ratio;
          top = options.center.y + radius*Math.sin(angle) - (height / 2.0);

          word_style.left = left + "px";
          word_style.top = top + "px";
        }
        already_placed_words.push(document.getElementById(word_id));
      }

      var drawOneWordDelayed = function(index) {
        index = index || 0;
        if (index < word_array.length) {
          drawOneWord(index, word_array[index]);
          setTimeout(function(){drawOneWordDelayed(index + 1);}, 10);
        } else {
          if (typeof options.callback === 'function') {
            options.callback.call(this);
          }
        }
      }

      // Iterate drawOneWord on every word. The way the iteration is done depends on the drawing mode (delayedMode is true or false)
      if (options.delayedMode || options.delayed_mode){
        drawOneWordDelayed();
      }
      else {
        di_jq.each(word_array, drawOneWord);
        if (typeof options.callback === 'function') {
          options.callback.call(this);
        }
      }
    };

    // Delay execution so that the browser can render the page before the computatively intensive word cloud drawing
    setTimeout(function(){drawWordCloud();}, 10);
    return this;
  };
})(jQuery);

/*Create treemap*/
function di_create_cloud_chart(title,subtitle,sourceText,uid, div_id, cloudRawData, chartType) {
	var cloudData = makeCollectionForCloud(cloudRawData);
	di_jq("#"+div_id).jQCloud(cloudData);	
  // Check for dublicate object
	var isUidExist=false;
	if(chartCollection.length>0)
	{
		for(var i=0;i<chartCollection.length;i++)
		{
			if(chartCollection[i].id==uid)
			{
				isUidExist = true;
				chartCollection[i].chartInstance = cloudData;
				break;
			}
		}		
	}
	if(!isUidExist)
	{
		var item = new Object();
		item.id = uid;
		item.chartInstance = cloudData;
		item.theme = "randam";
		item.title = {text:title,style:{font:'normal 15px arial',fontSize:'15px',fontWeight:'normal',color:'#000000',textDecoration:'none'}};
		item.subtitle={text:subtitle,style:{font:'normal 12px arial',fontSize:'12px',fontWeight:'normal',color:'#000000',textDecoration:'none'}};
		item.source={text:sourceText,style:{font:'normal 9px arial',fontSize:'12px',fontWeight:'normal',color:'#000000',textDecoration:'none'}};
		chartCollection.push(item);
	}
}

function di_redrawCloud(uid,div_id,cloudRawData,title)
{	
	di_create_cloud_chart(title,"","",uid, div_id, cloudRawData, "cloud");
}
function makeCollectionForCloud(data)
{
	var cloudData=new Array();
	if(data!=null)
	{
		if(data.categoryCollection.length>0 && data.seriesCollection.length>0)
		{
			var textList = data.categoryCollection;
			var datalist = data.seriesCollection[0].data;
			for(var i=0;i<textList.length;i++)
			{
				var cloudItem = new Object();
				cloudItem.text = textList[i];
				cloudItem.weight = datalist[i];
				cloudItem.title = datalist[i];
				cloudData.push(cloudItem);
			}
		}
	}
	return cloudData;
}
/* Return Cloud title obj
		Parameter : uid - chart unique id
					type - 0(title), 1(subtitle), 2(source)*/

function di_getCloudTitleObj(uid,type)
{
	var title;
	if(chartCollection.length>0)
	{
		for(var i=0;i<chartCollection.length;i++)
		{
			if(chartCollection[i].id==uid)
			{
				if(type==0)
				{
					title = chartCollection[i].title;
				}
				else if(type==1)
				{
					title = chartCollection[i].subtitle;
				}
				else if(type==2)
				{
					title = chartCollection[i].source;
				}
				break;
			}
		}
	}
	return title;
}

/* Set Cloud title obj
		Parameter : uid - chart unique id
		title - titlt object
		type - 0(title), 1(subtitle), 2(source)*/
function di_setCloudTitleObj(uid,title,type)
{
	if(chartCollection.length>0)
	{
		for(var i=0;i<chartCollection.length;i++)
		{
			if(chartCollection[i].id==uid)
			{				
				if(type==0)
				{
					chartCollection[i].title = title;
				}
				else if(type==1)
				{
					chartCollection[i].subtitle = title;
				}
				else if(type==2)
				{
					chartCollection[i].source = title;
				}
				break;
			}
		}
	}	
}