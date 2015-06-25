<%@ Page Language="C#" MasterPageFile="~/libraries/aspx/HomeMaster.master" AutoEventWireup="true" CodeFile="newdataview.aspx.cs" Inherits="libraries_aspx_newdataview" Title="Untitled Page" EnableSessionState="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" Runat="Server">

<!-- ***************** - DATA VIEW -- DataView CSS Files Starts- ***************** -->

<link href="../../stock/themes/default/css/NewDataView_standalone.css" rel="stylesheet" type="text/css" />
<link href="../../stock/themes/default/css/NewDataView_tabs.css" rel="stylesheet" type="text/css" />

<!-- ***************** - DATA VIEW -- DataView CSS Files Ends- ***************** -->



<!-- ***************** - DATA VIEW -- DataView JS Files Starts- ***************** -->
<script type="text/javascript" src="../js/NewDataView_jquery.js"></script>

<!-- This JavaScript snippet activates those tabs -->
<script>

// perform JavaScript after the document is scriptable.
$(function() {
	// setup ul.tabs to work as tabs for each div directly under div.panes
	$("ul.tabs").tabs("div.panes > div");
});
</script>
<!-- ***************** - DATA VIEW -- DataView JS Files Ends- ***************** -->





<!-- ***************** - DATA VIEW -- DataView Navigation Starts- ***************** -->
<div class="content_containers_dview">

		<!-- The Tabs Starts -->
<ul class="tabs">
	<li><a class="current" href="#">File</a></li>
	<li><a class="" href="#">Visualization</a></li>
	<li><a class="" href="#">Settings</a></li>
    <li><a class="" href="#">Options</a></li>
    <li><a class="" href="#">View & Sort</a></li>
</ul>
		<!-- The Tabs Ends -->
        
        
        
        
        <!-- The Tabs "Data" -->        
<div class="panes">
	<div style="display: block;">First tab content. Tab contents are called "panes"</div>
	<div style="display: none;">Second tab content</div>
	<div style="display: none;">Third tab content</div>
    <div style="display: none;">Fourth tab content</div>
    <div style="display: none;">Fifth tab content</div>
</div>
		<!-- The Tabs "Data" -->   


</div>


        

<!-- ***************** - DATA VIEW -- DataView Navigation Ends- ***************** -->
</asp:Content>

