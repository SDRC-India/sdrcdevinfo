<%@ Page Title="" Language="C#" MasterPageFile="~/libraries/aspx/Admin/AdminMaster.master"
    AutoEventWireup="true" CodeFile="RegHeaderDetail.aspx.cs" Inherits="libraries_aspx_Admin_RegHeaderDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript" src="../../js/sdmx/Header.js"></script>
    <h1 id="lang_HeaderSettings">
        <!-- Header Settings-->
    </h1>
    <h4 id="lang_HeaderSet_Desc">
        <!--Modify Header details here-->
    </h4>
    <input type="hidden" id="langHeaderUpdateSuccess" value="" />
    <input type="hidden" id="langHeaderUpdateFailure" value="" />
    <input type="hidden" id="hLoggedInUserNId" value="" />
 
    <div id="DivHeaderContainer">
    </div>
    <script type="text/javascript">

        di_jq(document).ready(function () {
            var IsHeaderCreated = '<%=IsSDMXHeaderCreated %>'
            if (IsHeaderCreated == "true") {
                z('hLoggedInUserNId').value = '<%=hLoggedInUserNId%>';
                var DbNid = getDefaultDbId();
                var SelectedDbDSDName = getDefaultDSDName(DbNid);
                SelectLeftMenuItem("Header");
                CreateSenderRecieverHeader(true, DbNid, SelectedDbDSDName);
            }
        });
    </script>
</asp:Content>
