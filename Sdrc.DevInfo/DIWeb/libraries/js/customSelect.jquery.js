(function ($) {
    $.fn.extend({

        customSelect: function (options) {
            if (!$.browser.msie || ($.browser.msie && $.browser.version > 6)) {
                var RetVal = null;
                RetVal = this.each(function () {

                    var currentSelected = $(this).find(':selected');
                    var html = currentSelected.html();
                    if (!html) { html = '&nbsp;'; }
                     $(this).after('<span class="customStyleSelectBox"><span class="customStyleSelectBoxInner">' + html + '</span></span>').css({ position: 'absolute', opacity: 0, fontSize: $(this).next().css('font-size') });
                    var selectBoxSpan = $(this).next();
                    var selectBoxWidth = parseInt($(this).width()) - parseInt(selectBoxSpan.css('padding-left')) - parseInt(selectBoxSpan.css('padding-right'));

                    /* Modification/Correction for Safari ...starts*/
                    if ($.browser.safari) {
                        var selectBoxWidth = parseInt($(this).width() + 23) - parseInt(selectBoxSpan.css('padding-left')) - parseInt(selectBoxSpan.css('padding-right'));
                    }
                    /* Modification/Correction for Safari ...ends*/

                    var selectBoxSpanInner = selectBoxSpan.find(':first-child');
                    selectBoxSpan.css({ display: 'inline-block' });
                    selectBoxSpanInner.css({ width: selectBoxWidth, display: 'inline-block' });
                    var selectBoxHeight = parseInt(selectBoxSpan.height()) + parseInt(selectBoxSpan.css('padding-top')) + parseInt(selectBoxSpan.css('padding-bottom'));
                    $(this).height(selectBoxHeight).change(function () {
                        // selectBoxSpanInner.text($(this).val()).parent().addClass('changed');   This was not ideal
                        selectBoxSpanInner.text($(this).find(':selected').text()).parent().addClass('changed');
                        // Thanks to Juarez Filho & PaddyMurphy
                    });

                });
                return RetVal;
            }
        }

    });
})(jQuery);
