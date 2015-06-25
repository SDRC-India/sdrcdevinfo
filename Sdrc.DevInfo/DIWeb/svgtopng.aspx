<%@ Page Language="C#" AutoEventWireup="true" CodeFile="svgtopng.aspx.cs" Inherits="svgtopng" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
<%--    <script src=http://code.jquery.com/jquery-latest.js></script>--%>
<script src="libraries/SvgTopng/jquery-latest.js"></script>
<script src="libraries/SvgTopng/saveSvgAsPng.js"></script>
<%--<script src=saveSvgAsPng.js></script>--%>
<script>
    function handleFileSelect(evt) {
       
        var $el = $('#filereader');
        var files = evt.target.files;
        for (var i = 0, f; f = files[i]; i++) {
            var reader = new FileReader();
            reader.onload = (function (file) {
                return function (e) {
                    $el.find('.load-target').html(e.target.result);
                    svgAsDataUri($el.find('.load-target svg')[0], null, function (uri) {
                        $el.find('input').hide()
                        $el.find('.preview').html('<img src="' + uri + '" />');
                    });
                    $el.find('.save').click(function () {
                        saveSvgAsPng($el.find('.load-target svg')[0], 'test.png');
                    });
                }
            })(f);
            reader.readAsText(f);
        }
    }

    if (window.File && window.FileReader && window.FileList && window.Blob) {
        document.getElementById('file').addEventListener('change', handleFileSelect, false);
    }

    function inlineTest(title, $el, saveOptions, testOptions) {
        var svg = $el.html();
        var template = $('#inline-template').html();
        var row = $el.html(template);
        row.find('h2').text(title);
        row.find('.canvas').html(svg);

        var canvas = row.find(testOptions && testOptions.selector || 'svg')[0];
        svgAsDataUri(canvas, saveOptions, function (uri) {
            row.find('.preview').html('<img src="' + uri + '" />');
        });

        row.find('.save').click(function () {
            saveSvgAsPng(canvas, 'test.png', saveOptions);
        });
    }

    inlineTest('Directly in the HTML', $('#inline'));
    inlineTest('With linked PNG image', $('#embedded-png'));
    inlineTest('With linked SVG image', $('#embedded-svg'));
    inlineTest('Sized with pixels', $('#sized-with-pixels'));
    inlineTest('Sized with style', $('#sized-with-style'));
    inlineTest('Sized with CSS', $('#sized-with-css'));
    inlineTest('At a higher resolution', $('#scaling'), { scale: 2 });
    inlineTest('When CSS styling selectors are prefixed', $('#selectors-prefixed'), {
        selectorRemap: function (s) { return s.replace('#selectors-prefixed ', '') }
    });
    inlineTest('Exporting a group within an SVG', $('#group'), null, {
        selector: '#sub-group'
    });
    inlineTest('With custom fonts', $('#custom-font'));
    inlineTest('Percentage Height and Width', $('#percentage-size'));

    var $sandbox = $('#sandbox');
    $sandbox.find('.render').click(function () {
        $sandbox.find('.error').hide().text('');
        $sandbox.find('.load-target').html($('#sandbox textarea').val());
        var canvas = $sandbox.find('.load-target svg')[0];
        try {
            svgAsDataUri(canvas, null, function (uri) {
                $sandbox.find('.preview').html('<img src="' + uri + '" />');
            });
            $sandbox.find('.save').unbind('click').click(function () {
                saveSvgAsPng(canvas, 'test.png');
            });
        } catch (err) {
            $sandbox.find('.error').show().text(err.message);
            $sandbox.find('.preview').html('');
        }
    });
</script>



</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>
