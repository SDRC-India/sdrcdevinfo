<%@ Page Language="C#" MasterPageFile="~/libraries/aspx/Admin/AdminMaster.master"
    AutoEventWireup="true" CodeFile="Language.aspx.cs" Inherits="libraries_aspx_Admin_Language"
    Title="Untitled Page" EnableSessionState="True" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h1 id="langLanguageSettings">
        <!--Language Settings-->
    </h1>
    <h4 id="langConfigure_Site_MLang">
        <!--Configure your site in multiple languages-->
    </h4>
    <div>
        <div class="flt_lft">
            <h2 id="langLanguageTranslation">
                <!--Language Translation-->
            </h2>
        </div>
        <div class="flt_rgt pddn_tp_fftn">   
            <asp:FileUpload ID="FileUpdXLS" runat="server" style="display:none;position:static;" />
            <asp:Button ID="BtnUploadXLS" runat="server" OnClick="btnUploadLangXLS_Click" class="di_gui_button" style="display:none;" />
            <input type="button" id="lang_lm_ImportXML" value="" onclick="ShowUploadDiv();"
                    class="di_gui_button adm_genpg_bttn_sz"/>  
            <input type="button" id="lang_lm_ExportXML" disabled="disabled" value="" onclick="CLickExportLanguage();"
                class="di_gui_button adm_genpg_bttn_sz" />
           
                        
            <div id="DivFileUpload" >
                <%--<span id="Spn_Import_Lang_XLS_FileName"></span>--%>
                
            </div>
        </div>
        <div class="clear">
        </div>
    </div>
    <!-- Input Fields Area ...starts-->
    <div class="confg_Adm_box">
        <p class="confg_frm_txt" id="langSourceLanguage">
            <!--Source Language-->
            <span class="Mandatory">*</span></p>
        <p class="confg_frm_inp_bx">
            <asp:DropDownList ID="ddlSrcLng" runat="server" AutoPostBack="false" onchange="ChangeLanguage();"
                class="confg_frm_inp_bx_txt_dd">
            </asp:DropDownList>
        </p>
        <div class="clear">
        </div>
        <p class="confg_frm_txt" id="langTargetLanguage">
            <!--Target Language-->
            <span class="Mandatory">*</span></p>
        <p class="confg_frm_inp_bx_big">
            <asp:DropDownList ID="ddlTrgLng" runat="server" AutoPostBack="false" onchange="ChangeLanguage();"
                class="confg_frm_inp_bx_txt_dd">
            </asp:DropDownList>
            <input type="button" value="..." onclick="ShowNewLanguageDiv();" class="adm_inp_trg_lng_pos"
                style="display: none;" /></p>
        <div class="clear">
        </div>
    </div>
    <!-- Input Fields Area ...ends-->
    <!-- Src, Trg Language grid for editing ...starts-->
    <div id="divLanguageGrid" style="width: 99%; overflow: auto; overflow-x: hidden;
        border: 1px Silver solid;">
    </div>
    <!-- Src, Trg Language grid for editing ...ends-->
    <!-- Database Connections Buttons ...starts-->
    <div class="adm_upd_bttn">
        <input type="button" id="langSave" value="" class="di_gui_button_disabled" disabled="disabled"
            onclick="SaveLanguageChanges();" /><input type="button" id="langCancel" value=""
                class="di_gui_button_disabled" disabled="disabled" onclick="ChangeLanguage();" /><input
                    type="button" id="btnlangBack" value="" onclick="window.location = 'LanguageMaster.aspx'"
                    class="di_gui_button" />
    </div>
    <script type="text/javascript">
        function ShowUploadDiv() {
            document.getElementById('<%= BtnUploadXLS.ClientID %>').style.display = '';
            document.getElementById('<%= FileUpdXLS.ClientID %>').style.display = ''; 

           z('lang_lm_ImportXML').style.display = 'none';
        }

        var RowDelimiter = "<%=RowDelimiter%>";
        var ColumnDelimiter = "<%=ColumnDelimiter%>";
        z('divLanguageGrid').style.height = PageHeight() - 330 + 'px';
        SelectLeftMenuItem("LngSettings");
        ChangeLanguage();
        RemoveLoaderNMask();
    </script>
</asp:Content>
