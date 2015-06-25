<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FileUpload.aspx.cs" Inherits="libraries_aspx_FileUpload" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>File Uploading - </title>
    <script type="text/javascript" src="../js/di.jquery-1.4.4.js"></script>
    <script type="text/javascript" language="javascript">
        var fileHTMLText;
        var MsgErrorInUploadingFile = "<%=MsgErrorInUploadingFile %>";
        var MsgFileTypeAllowed = "<%=MsgFileTypeAllowed %>";
        var MsgIValidFileName = "<%=MsgInvalidFileName %>";

        function UploadFile() {
            try {
                return UploadUsingSourceLinkWindow();
            }
            catch (ex) {
                alert(MsgErrorInUploadingFile);
                return false;
            }
        }



        function UploadUsingSourceLinkWindow() {

            //var parentfunObjet = getParentFunction("parentFunction");
            var fileUpload = document.getElementById("fileUploadPDF");
            if (fileUpload && fileUpload.value != "") {
                var fileName = fileUpload.value.substring(fileUpload.value.lastIndexOf("\\") + 1);
                /***Check Valid file name***/

                if (CheckSpChars(fileName)) {
                    fileName = fileName.replaceAll("&", "&amp;");
                    var fileExtension = fileName.substring(fileName.lastIndexOf(".")).toLowerCase();
                    if (IsAllowedFileType(fileExtension)) {
                        //ShowLoader();
                        document.getElementById("myform").submit();
                    }
                    else {

                        //parentfunObjet("Invalid File Type. Only Word, PowerPoint, Excel, PDF, Photos are allowed.");                        
                        alert("Invalid File Type. Only images are allowed.");
                        clearFileInputField();
                        return false;
                    }
                }
                else {
                    alert("Invalid File Name.");
                    clearFileInputField();
                    return false;
                }

            }
            else {
                return false;
            }
            return true;
        }

        function ShowLoader() {
            var version = 4;
            var rv = CheckIEBrowserVersion();
            if (rv != -1) {
                version = 9;
            }
            if (this.name == "frame1") {
                window.parent.document.getElementById(window.parent.CurrenDiv.replace("#", "")).childNodes[0].children[version].children[4].children[3].children[1].children[1].style.visibility = "visible";
            }
            else if (this.name == "frame2") {
                window.parent.document.getElementById(window.parent.CurrenDiv.replace("#", "")).childNodes[0].children[version].children[4].children[6].children[1].children[1].style.visibility = "visible";
            }
            else if (this.name == "frame3") {
                window.parent.document.getElementById(window.parent.CurrenDiv.replace("#", "")).childNodes[0].children[version].children[4].children[9].children[1].children[1].style.visibility = "visible";
            }
            else if (this.name == "frame4") {
                window.parent.document.getElementById(window.parent.CurrenDiv.replace("#", "")).childNodes[0].children[version].children[4].children[12].children[1].children[1].style.visibility = "visible";
            }
            else if (this.name == "frame5") {
                window.parent.document.getElementById(window.parent.CurrenDiv.replace("#", "")).childNodes[0].children[version].children[4].children[15].children[1].children[1].style.visibility = "visible";
            }
        }

        String.prototype.replaceAll = function (str1, str2, ignore) {
            return this.replace(new RegExp(str1.replace(/([\/\,\!\\\^\$\{\}\[\]\(\)\.\*\+\?\|\<\>\-\&])/g, "\\$&"), (ignore ? "gi" : "g")), (typeof (str2) == "string") ? str2.replace(/\$/g, "$$$$") : str2);
        }

        function clearFileInputField() {
            document.getElementById("divfileUploadContainer").innerHTML = fileHTMLText;
        }

        function UpdateParent(fileName, filePath) {
            return UpdateSourceLinkWindow(fileName, filePath)
        }

        function CheckIEBrowserVersion() {
            var rv = -1
            if (navigator.appName == 'Microsoft Internet Explorer') {
                var ua = navigator.userAgent;
                var re = new RegExp("MSIE ([0-9]{1,}[\.0-9]{0,})");
                if (re.exec(ua) != null)
                    rv = parseFloat(RegExp.$1);
                //                if (rv >= 9.0) {
                //                    rv = -9;
                //                }
                rv = 9;
            }
            return rv;
        }

        function UpdateControlForFrames(secLevel, thirdLevel, fouthLevel1, fouthLevel2, displayFileName, filePath, fileName) {

            window.parent.document.getElementById(window.parent.CurrenDiv.replace("#", "")).childNodes[0].children[secLevel].children[thirdLevel].children[fouthLevel1].children[0].style.display = "";
            window.parent.document.getElementById(window.parent.CurrenDiv.replace("#", "")).childNodes[0].children[secLevel].children[thirdLevel].children[fouthLevel1].children[1].style.display = "none";
            window.parent.document.getElementById(window.parent.CurrenDiv.replace("#", "")).childNodes[0].children[secLevel].children[thirdLevel].children[fouthLevel1].children[0].children[0].innerHTML = displayFileName;
            window.parent.document.getElementById(window.parent.CurrenDiv.replace("#", "")).childNodes[0].children[secLevel].children[thirdLevel].children[fouthLevel1].children[0].children[0].href = filePath;
            window.parent.document.getElementById(window.parent.CurrenDiv.replace("#", "")).childNodes[0].children[secLevel].children[thirdLevel].children[fouthLevel1].children[0].children[1].value = fileName;
        }

        function UpdateSourceLinkWindow(fileName, filePath) {
            var displayFileName = fileName;
            var arrdisplayFileName = displayFileName.split("__");
            for (var i = 0; i < arrdisplayFileName.length - 1; i++) {
                displayFileName = arrdisplayFileName[i];
            }
            arrdisplayFileName = fileName.split(".");
            displayFileName = displayFileName + "." + arrdisplayFileName[arrdisplayFileName.length - 1].toString();
            sec = 4;
            var rv = CheckIEBrowserVersion();
            if (rv != -1) {
                sec = 9;
            }
            window.parent.document.getElementById("hidImgPath").value = filePath;
        }


        function IsAllowedFileType(fileExtension) {
            var RetVal = false;
            var FileTypesAllowed = [".png", ".jpg", ".jpeg", ".gif"];

            if (fileExtension) {
                fileExtension = fileExtension.toLowerCase();
                for (var i = 0; i < FileTypesAllowed.length; i++) {
                    if (fileExtension == FileTypesAllowed[i]) {
                        RetVal = true;
                    }
                }
            }
            return RetVal;
        }


        function CheckSpChars(ValidFilename) {
            var flagfileName = true;
            var iChars = "!@#$^*%<>'";

            for (var i = 0; i < iChars.length; i++) {
                if (ValidFilename.indexOf(iChars.charAt(i)) != -1) {
                    flagfileName = false;
                }
            }

            return flagfileName;
        }

        function GetAngleBracketsReplacement(strInput) {
            if (strInput) {
                strInput = strInput.replace(new RegExp(['<'], "g"), "&lt;");
                strInput = strInput.replace(new RegExp(['>'], "g"), "&gt;");
                //strInput = EscapeQuotes(strInput);
            }
            return strInput;
        }


        function getParentFunction(funcName) {
            var func = null;
            // Child opened in new window e.g. target="blank"
            if (top.window.opener && !top.window.opener.closed) {
                try { func = eval("top.window.opener." + funcName); } catch (error) { }
            }
            if (!(func)) {
                // Child opened in IFRAME
                try { func = eval("top." + funcName); } catch (error) { }
            }
            //            if (!(func)) {
            //                throw new Error("function \"" + funcName + "\" is not in parent window.");
            //            }
            return func;
        }

      
    </script>
</head>
<body>
    <div>
        <form id="myform" name="myform" runat="Server" action="FileUpload.aspx" method="POST"
        target="_self" enctype="multipart/form-data">
        <table width="100%" cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td>
                    <div id="divfileUploadContainer">
                        <input type="file" name="fileUploadPDF" id="fileUploadPDF" runat="server" class="spanFileUpload"
                            onkeypress="return false;" onchange="JavaScript:UploadFile();" />
                    </div>
                    <div>
                        <br />
                        <img id="imgNews" runat="server" src="diorg/images/news_images/template.jpg" alt="News Image" />
                    </div>
                    <div id="divFileValue" style="display: none">
                        <span id="spanFileValue"></span>
                    </div>
                </td>
                <td class="filesTd">
                    <div id="divExtension" class="divExtension">
                    </div>
                </td>
            </tr>
        </table>
        <asp:hiddenfield id="hidSource" runat="server" value="" />
        <asp:hiddenfield id="hidSourceNId" runat="server" value="" />
        <%--   <asp:HiddenField ID="hdnSourceDeletionMsgConf" runat="server"/>--%>
        <script type="text/javascript" language="javascript">
            fileHTMLText = document.getElementById("divfileUploadContainer").innerHTML;
        </script>
        <script type="text/javascript">    
        </script>
        </form>
    </div>
</body>
</html>
