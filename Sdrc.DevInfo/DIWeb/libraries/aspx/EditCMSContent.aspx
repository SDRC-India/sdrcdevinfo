<%@ Page Language="C#" MasterPageFile="~/libraries/aspx/HomeMaster.master" AutoEventWireup="true"
    CodeFile="EditCMSContent.aspx.cs" Inherits="libraries_aspx_EditCMSContent" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="Server">
    <script language="javascript" type="text/javascript" src="../js/di.jquery-1.4.4.js"></script>
    <script type="text/javascript" src="ckeditor/ckeditor.js"></script>
    <div class="content_containers">
        <div id="reg_content_containers" class="flt_lft">
            <h2>
                <span id="SpnEditContentHeading" runat="server">Edit Content</span></h2>
            <div class="clear">
            </div>
            <h5>
            </h5>
        </div>
        <div class="clear">
        </div>
        <div class="wdth_nnty_fv">
            <table style="width: 100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <textarea name="editorCMS"></textarea>
                        <script type="text/javascript">
                            CKEDITOR.replace('editorCMS',
                            {
                                filebrowserUploadUrl: 'uploader/upload.aspx',
                                height: 400,
                                toolbar:
		                            [
			                             { name: 'document', items: ['Source'] },
                                            { name: 'clipboard', items: ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Undo', 'Redo'] },
                                            { name: 'editing', items: ['Find', 'Replace', '-', 'SelectAll', '-', 'SpellChecker', 'Scayt'] },
                                            { name: 'forms', items: ['Form', 'Checkbox', 'Radio', 'TextField', 'Textarea', 'Select', 'Button', 'ImageButton', 'HiddenField'] },
                                            { name: 'basicstyles', items: ['Bold', 'Italic', 'Underline', 'Strike', 'Subscript', 'Superscript', '-', 'RemoveFormat'] }, '/', { name: 'links', items: ['Link', 'Unlink', 'Anchor'] },
                                            { name: 'paragraph', items: ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', '-', 'Blockquote', 'CreateDiv', '-', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock', '-', 'BidiLtr', 'BidiRtl'] },

                                            { name: 'insert', items: ['Image', 'Flash', 'Table', 'HorizontalRule', 'Smiley', 'SpecialChar'] }, '/',
                                            { name: 'styles', items: ['Styles', 'Format', 'Font', 'FontSize'] },
                                            { name: 'colors', items: ['TextColor', 'BGColor'] },
                                            { name: 'tools', items: ['Maximize', 'ShowBlocks'] }
		                            ]
                            });           
                        </script>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <br />
                        <input id="BtnSaveCMSdata" type="submit" value="Update" class="di_gui_button" />&nbsp;&nbsp<input
                            id="BtnCancel" type="submit" value="Cancel" class="di_gui_button" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <script type="text/javascript">
        var CallbackPageName = "Callback.aspx";
        di_jq(document).ready(function () {
            di_jq('#BtnSaveCMSdata').click(function () {
                var data = CKEDITOR.instances.editorCMS.getData();
                InputParam = SrcHtmlPath;
                InputParam += ParamDelimiter + data;
                di_jq.ajax({
                    type: "POST",
                    url: CallbackPageName,
                    data: { 'callback': '1018', 'param1': InputParam },
                    async: true,
                    beforeSend: function () {
                        ApplyMaskingDiv();
                    },
                    success: function (data) {
                        try {
                            if (data == "true") {
                                alert("Content updated sucessfully.");
                                window.location.replace(document.referrer);
                            }
                            else {
                                alert("Error while updating content.");
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
            });

            di_jq('#BtnCancel').click(function () {
                window.location.replace(document.referrer);
            });
        });

        var SrcHtmlPath = GetRelativePath();
        ShowLoadingDiv();
        di_jq.ajax({
            type: "GET",
            url: SrcHtmlPath,
            success: function (data) {
                CKEDITOR.instances.editorCMS.setData(data);
                HideLoadingDiv();
            },
            error: function (ex) {
                alert(ex.message);
                HideLoadingDiv();
            },
            cache: false
        });

        function GetRelativePath() {
            return '<%=Relativepath %>';
        }
           
    </script>
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
        createFormHiddenInputs("frmEditCMSContent", "POST");
        SetCommonLinksHref("frmEditCMSContent", "POST");
        setPostedData('<%=hdsby%>', '<%=hdbnid%>', '<%=hselarea%>', '<%=hselind%>', '<%=hlngcode%>', '<%=hlngcodedb%>', '<%=hselindo%>', '<%=hselareao%>', 10, '<%=hLoggedInUserNId%>', '<%=hLoggedInUserName%>');
        if (GetLanguageCounts() > 1) {
            z("LanguagePipeLi").style.display = "";
            ShowLanguageComponent(getAbsURL('stock'), 'lngcode_div', '<%=hlngcode%>');
        }
    </script>
</asp:Content>
