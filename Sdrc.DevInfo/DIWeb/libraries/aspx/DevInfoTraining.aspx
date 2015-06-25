<%@ Page Title="" Language="C#" MasterPageFile="~/libraries/aspx/HomeMaster.master" AutoEventWireup="true" CodeFile="DevInfoTraining.aspx.cs" Inherits="libraries_aspx_DevInfoTraining" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" Runat="Server">

<script type="text/javascript">
            // This function will be called by child window
            parentFunction = function (height) {
                document.getElementById('cphMainContent_iframeContent').style.height = height + "px";
            }
 
</script>

<div class="content_containers">
<%--    <div id="reg_content_containers">
        <h2>DevInfo Training</h2>
        <h5></h5>--%>
   
    <!-- Left Links Section ...starts-->
    <div id="lft_sec_adm" class="lft_sec_adm_pos">
        <ul>
            <li><a href="DevInfoTraining.aspx?T=TCW&PN=diorg/di_courseware.html" id="aTrainingCourses">Training Courses</a></li>
            <li><a href="DevInfoTraining.aspx?T=EL&PN=diorg/di_elearning.html" id="aElearningCourses">e-learning Courses</a></li>
            <li><a href="DevInfoTraining.aspx?T=AR&PN=diorg/di_additional_resources.html" id="aAdditionalCourses">Additional Resources</a></li>
            <li><a href="DevInfoTraining.aspx?T=TS&PN=diorg/di_training_schedule.php" id="aTrainingSchedule">Training Schedule</a></li>
        </ul>
    </div>
    <!-- Left Links Section ...ends--> 

    <!-- Right Config Data Section ...starts-->
    <div id="rgt_sec_adm" class="rgt_sec_adm_pos">    
        <h1>DevInfo Training</h1>
        <h4></h4>

        <!-- Main Contact Page Content Area ...starts-->
        <div class="desc_pg_main_sec" id="div_content" runat="server">
<%--        <iframe style="width:850px" frameborder="0" allowfullscreen runat="server" id="iframeContent" scrolling="no" src="http://www.devinfo.org/libraries/aspx/diorg/TrainingSchedule/di_training_schedule.php"></iframe>--%>
      <iframe style="width:850px" frameborder="0" allowfullscreen runat="server" id="iframeContent" scrolling="no" src="diorg/TrainingSchedule/di_training_schedule.php"></iframe>
        </div>
        <!-- Main Contact Page Content Area ...ends-->            
    </div>
    <!-- Right Config Data Section ...ends-->     
    <div class="clear"></div>
</div>

<!-- DEVELOPER CODE -->
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
     di_jq(document).ready(function () {
         SelectTrainingMenuOption();
     });	
    </script>
</asp:Content>

