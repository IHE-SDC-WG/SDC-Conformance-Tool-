<%@ Page Language="C#" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="UriRequestHandler.aspx.cs" Inherits="SDC.UriRequestHandler" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">

        function toggleviewxml() {
            var f = document.getElementById("rawxml");
            if (f.style.display == "none") {
                f.style.display = "block";
                document.getElementById("linkxml").innerText = "Hide Xml";
            }
            else {
                f.style.display = "none";
                document.getElementById("linkxml").innerText = "Show Xml";
            }
        }

        


    </script>
</head>
<body>
    <input runat="server" style="display:none" type="checkbox"  checked="checked" id="scriptsubmit" />
     <a id="linkxml" style="display:block;margin-left:100px;margin-bottom:20px" href="#" onclick="toggleviewxml()">Show Xml</a>
    <textarea cols="200" runat="server" id="rawxml" 
        style="display:none;width:80%;padding-left:20px;background-color:lightgray;border:solid thin;margin-left:200px;margin-bottom:40px" rows="15"></textarea>
    
    <div id="formcontent" runat ="server">
        <!--place holder for form-->
    </div>
    <div id="submitmsg" runat="server"></div>

    <form id="form1" runat="server">
        
          
    
    </form>
</body>
</html>
