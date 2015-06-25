<%@ Page Title="" Language="C#" MasterPageFile="~/libraries/aspx/HomeMaster.master"
    AutoEventWireup="true" CodeFile="AddCMSContent.aspx.cs" Inherits="libraries_aspx_AddCMSContent" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="Server">
    <script type="text/javascript" src="ckeditor/ckeditor.js"></script>
    <script type="text/javascript" src="../js/di.jquery-1.4.4.js"></script>
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.9.2/themes/base/jquery-ui.css" />
    <script type="text/javascript">
        function deletePDF() {
            window.parent.document.getElementById("divSpan").style.display = "none";
            window.parent.document.getElementById("divDocument").style.display = "";
            window.parent.document.getElementById("hidPDFFileName").value = "";
        }
    </script>
    <div class="content_containers">
        <div id="reg_content_containers">
            <h2>
                <span id="SpnAddContentHeading" runat="server">Add Content</span></h2>
            <h5>
            </h5>
        </div>
        <div class="wdth_nnty_fv">
            <div style="float: left; margin-right: 10px; width: 440px">
                <table>
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td class="fnt_bld fnt_sz_frten">
                                        Date
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <input type="text" id="datepicker" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>                          
                        </td>
                    </tr>                                        
                    <tr>
                        <td colspan="2" class="fnt_bld fnt_sz_frten">
                            Title
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <input type="text" id="txtTitle" style="width: 430px" MaxLength="200" />
                        </td>
                    </tr>
                    <tr>
                        <td class="fnt_bld fnt_sz_frten" colspan="2">
                            Summary
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="width:100%">
                            <textarea id="txtSummary" rows="5" style="width: 100%"></textarea>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="float: right; padding-left: 10px; width: 46%">
                <table>
                    <tr>
                        <td class="fnt_bld fnt_sz_frten">
                            Upload Image<br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div id='tdFileUpload1' name="tdFileUpload1" runat='server' style='height: 150px;
                                vertical-align: top;'>
                                <iframe id='frame1' name='frame1' src="FileUpload.aspx" frameborder='0' scrolling='no'
                                    height='150px' marginwidth='0px' marginheight='0px' style="width: 440px"></iframe>
                                <input type="hidden" id="hidImgPath" value="" />
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="clear">
            </div>
        </div>
        <table class="wdth_nnty_fv" cellpadding="0" cellspacing="0" style="width:100%">
            <tr>
                <td colspan="2" style="width:100%">
                    <%--<table class="wdth_ful">--%>
                    <table style="width:100%">
                        <tr>
                            <td colspan="2" class="fnt_bld fnt_sz_frten">
                                Detailed Content
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2"  style="width:100%">
                                <textarea name="editorCMS" style="width:100%"></textarea>
                                <script type="text/javascript">
                                    CKEDITOR.replace('editorCMS',
                                {
                                    //filebrowserBrowseUrl: '/browser/browse.aspx',
                                    filebrowserUploadUrl: 'uploader/upload.aspx',
                                    toolbar:
		                            [
			                                { name: 'document', items: ['Source'] },
                                            { name: 'clipboard', items: ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Undo', 'Redo'] },
                                            { name: 'editing', items: ['Find', 'Replace', '-', 'SelectAll', '-', 'SpellChecker', 'Scayt'] },
                                            { name: 'forms', items: ['Form', 'Checkbox', 'Radio', 'TextField', 'Textarea', 'Select', 'Button', 'ImageButton', 'HiddenField'] },
                                            { name: 'basicstyles', items: ['Bold', 'Italic', 'Underline', 'Strike', 'Subscript', 'Superscript', '-', 'RemoveFormat'] }, { name: 'links', items: ['Link', 'Unlink', 'Anchor'] },
                                            { name: 'paragraph', items: ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', '-', 'Blockquote', 'CreateDiv', '-', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock', '-', 'BidiLtr', 'BidiRtl'] },

                                            { name: 'insert', items: ['Image', 'Flash', 'Table', 'HorizontalRule', 'Smiley', 'SpecialChar'] }, 
                                            { name: 'styles', items: ['Styles', 'Format', 'Font', 'FontSize'] },
                                            { name: 'colors', items: ['TextColor', 'BGColor'] },
                                            { name: 'tools', items: ['Maximize', 'ShowBlocks'] }
		                            ]
                                });                                  
                                </script>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="height: 50px">
                                <div id="divSpan" style="display: none">
                                    <a id="anchorPDF" href="#" target="_blank" style="cursor: pointer"></a>
                                    <img id="imgDel" src="../../stock/themes/default/images/cross.gif" alt="Delete" style="cursor: pointer"
                                        onclick="deletePDF();" />
                                </div>
                                <div id="divDocument">
                                    <table class="wdth_ful">
                                        <tr>
                                            <td colspan="2" class="fnt_bld fnt_sz_frten">
                                                PDF Upload
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div id='tdFileUpload2' name="tdFileUpload2" runat='server' style='height: 30px;
                                                    vertical-align: top;'>
                                                    <iframe id='frame2' name='frame2' src="PDFUpload.aspx" frameborder='0' scrolling='no'
                                                        height='23px' marginwidth='0px' marginheight='0px' style="width: 440px"></iframe>
                                                    <input type="hidden" id="hidPDFFileName" value="" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                            <td align="right" valign="top">
                                <input id="BtnOK" type="submit" value="OK" class="di_gui_button" />&nbsp;&nbsp<input
                                    id="BtnCancel" type="submit" value="Cancel" class="di_gui_button" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>    
    <script type="text/javascript">
        CookiePostfixStr = '_' + '<%=Global.CookiePostfixStr%>';    // use in hosting app

        var di_components = "Language";
        var di_component_version = '<%=Global.diuilib_version%>';
        var di_theme_css = '<%=Global.diuilib_theme_css%>';
        var di_diuilib_url = '<%=Global.diuilib_url%>';
        document.write("<script type='text/javascript' src='" + "<%=Global.diuilib_url%>" + "diuilibincludes.js'" + "><\/script>");
        document.write("<script type='text/javascript' src='" + "<%=Global.diuilib_url%>" + "diuilibcommon.js'" + "><\/script>");

    </script>
    <script type="text/javascript">
//                di_jq(function () {
//                    di_jq("#datepicker").datepicker();
//                });
        function GetRequestingPagePath() {
            return '<%=RequestingPagePath %>';
        }

        var CallbackPageName = "Callback.aspx";
        var count=1;
        di_jq(document).ready(function () {
            di_jq('#BtnOK').click(function () {
                var LinkUrl = "";
                var Title = txtTitle.value;
                var ArrayTitle = Title.split(" ");
                for (var i = 0; i < ArrayTitle.length; i++) {
                    if (count <= 5) {
                        if (ArrayTitle[i] != "") {
                            if (LinkUrl == "") {
                                LinkUrl = ArrayTitle[i];
                            }
                            else {
                                LinkUrl = LinkUrl + "-" + ArrayTitle[i];
                            }
                        }
                        i = i + 1;
                    }
                }
                if (LinkUrl != "") {
                    LinkUrl = RemoveSpecialChars(LinkUrl);
                    //                    LinkUrl = LinkUrl + ".html";
                }
                var Title = txtTitle.value;
                var Date = datepicker.value;
                var Summary = txtSummary.value;
                var ImageSrc = hidImgPath.value;
                var DetailedContent = CKEDITOR.instances.editorCMS.getData();
                var PdfSrc = hidPDFFileName.value;
                var RequestingPagePath = GetRequestingPagePath();                
                //var PCategory = PageCategory.value;
                //alert("PageCategory:" + PCategory);

                if (Title == "") {
                    alert("Title is blank");
                }
                else if (Date == "") {
                    alert("Date Selection is blank");
                }
                else if (Summary == "") {
                    alert("Summary is blank");
                }
                else if (ImageSrc == "") {
                    alert("Image is not selected");
                }
                else if (DetailedContent == "") {
                    alert("DetailedContent is blank");
                }
                else {
                    InputParam = LinkUrl;
                    InputParam += ParamDelimiter + Title;
                    InputParam += ParamDelimiter + Date;
                    InputParam += ParamDelimiter + Summary;
                    InputParam += ParamDelimiter + DetailedContent;
                    InputParam += ParamDelimiter + PdfSrc;
                    InputParam += ParamDelimiter + RequestingPagePath;
                    InputParam += ParamDelimiter + ImageSrc;
                    di_jq.ajax({
                        type: "POST",
                        url: CallbackPageName,
                        data: { 'callback': '1019', 'param1': InputParam },
                        async: true,
                        beforeSend: function () {
                            ApplyMaskingDiv();
                        },
                        success: function (data) {
                            try {
                                if (data == "true") {
                                    alert("Content added sucessfully.");
                                    window.location.replace(document.referrer);
                                }
                                else {
                                    alert("Error while adding content.");
                                }
                            }
                            catch (ex) {
                                alert("Error : " + ex.message);
                            }
                        },
                        error: function () {
                            alert("Error occured");
                        },
                        complete: function () {
                            RemoveMaskingDiv();
                        },
                        cache: false
                    });
                }
            });
            di_jq('#BtnCancel').click(function () {
                window.location.replace(document.referrer);
            });
        });

    
    </script>
    <script type="text/javascript">
        function RemoveSpecialChars(InputString) {
            var Reult = InputString.replace(/[^a-zA-Z0-9_-]/g, '');
            return Reult;
        }
    </script>
    <script type="text/javascript">
        createFormHiddenInputs("frmAddCMSContent", "POST");
        SetCommonLinksHref("frmAddCMSContent", "POST");
        setPostedData('<%=hdsby%>', '<%=hdbnid%>', '<%=hselarea%>', '<%=hselind%>', '<%=hlngcode%>', '<%=hlngcodedb%>', '<%=hselindo%>', '<%=hselareao%>', 10, '<%=hLoggedInUserNId%>', '<%=hLoggedInUserName%>');
        if (GetLanguageCounts() > 1) {
            z("LanguagePipeLi").style.display = "";
            ShowLanguageComponent(getAbsURL('stock'), 'lngcode_div', '<%=hlngcode%>');
        }
    </script>
</asp:Content>
