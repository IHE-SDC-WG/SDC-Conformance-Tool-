<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SDCForm.aspx.cs" Inherits="SDC.SDCForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">

        //function serversubmit()
        //{
        //    var xml = document.getElementById("rawxml").value;
        //    var submiturls = document.getElementById("submiturl").value
        //    openMessageData(0);
        //    var responsetext = PageMethods.submitform(xml, submiturls, OnSucceed, OnError);
        //    document.getElementById("btnServerSubmit").style.visibility = "hidden";
            
        //}
        //function OnSucceed(result)
        //{
           
        //    result = formatXml(result);
        //    xml_escaped = result.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/ /g, '&nbsp;').replace(/\n/g, '<br />');

        //    document.getElementById('FormData').style.display = 'block';

        //    document.getElementById('response').innerHTML = "<PRE>" + xml_escaped + "</PRE>";
        //    document.getElementById("response").style.backgroundColor = "yellow";
        //    document.getElementById('response').style.display = "block";

        //    document.getElementById('MessageData').style.display = 'block';
        //    document.getElementById('FormData').style.display = 'none';
        //}

        //function OnError(result)
        //{
        //    alert(result);
        //    document.getElementById("response").innerText = result;
        //    document.getElementById("response").style.backgroundColor = "yellow";
        //    document.getElementById("response").style.color = "red";
        //}

        function toggleviewxml()
        {
            var f = document.getElementById("rawxml");
            if (f.style.display == "none") {
                f.style.display = "block";
                document.getElementById("linkxml").innerText = "Hide Retrieve Response";
            }
            else {
                f.style.display = "none";
                document.getElementById("linkxml").innerText = "Show Retrieve Response";
            }
        }

        function togglesoap() {
            var f = document.getElementById("submitsoap");
            if (f.style.display == "none") {
                f.style.display = "block";
                document.getElementById("lnksoap").innerText = "Hide Soap";
            }
            else {
                f.style.display = "none";
                document.getElementById("lnksoap").innerText = "Show Soap";
            }
        }

        function toggleParameters()
        {
            var f = document.getElementById("parameters");
            if (f.style.display == "none") {
                f.style.display = "block";
                document.getElementById("linkParameters").innerText = "Hide Package Parameters";
            }
            else {
                f.style.display = "none";
                document.getElementById("linkParameters").innerText = "Show Package Parameters";
            }
        }

       
    </script>
</head>
<body>
    <div id="header" runat="server">
    <a id="linkParameters" style="display:block;margin-left:100px" href="#" onclick="toggleParameters()">Show Package Parameters</a> 
    <div id="parameters" style="display:none;width:80%;padding-left:20px;background-color:lightgray;border:solid thin;margin-left:200px" >
        <table >
            <tr>
                <td>Package ID</td><td><asp:Label runat="server" ID="packageid"></asp:Label></td>
            </tr>
           <%-- <tr>
                <td>Package Name</td><td><asp:Label runat="server" ID="packagename"></asp:Label></td>
            </tr>--%>
            <tr>
                <td>Form (s)</td><td><asp:Label runat="server" ID="forms"></asp:Label></td>
            </tr>
            <tr>
                <td>Form Submit Endpoint</td><td style="text-align:left;"><input type="text" runat="server" id="submiturl" style="width:80%"/></td>
            </tr>            
            <tr>
                <td>Form Manager Endpoint</td><td><input type="text" runat="server" id="retrieveurl" style="width:80%;"/></td>
            </tr>
            
          
<%--            <tr>
                <td>Submit Namespace</td><td><input type="text" runat="server" id="submitnamespace" style="width:80%;background-color:wheat"/></td>
            </tr>
            <tr>
                <td>Submit Action</td><td><input type="text" runat="server" id="submitaction" style="width:80%;background-color:wheat"/></td>
            </tr>
            <tr>
                <td>Local XSLT Path</td><td><input type="text" runat="server" id="xsltpath" style="width:80%;background-color:wheat"/></td>
            </tr>--%>
            <tr>
                <td>Submit Soap</td><td> <textarea cols="200" runat="server" id="submitsoap" style="width:80%;" rows="6">Soap Request is generated when you submit. Hit the back button after you submit.</textarea></td>
            </tr>
            <tr>
                <td>Submit w Script</td><td><input runat="server" type="checkbox" id="scriptsubmit" /></td>
            </tr>
        </table>
    </div>

    <a id="linkxml" style="display:block;margin-left:100px;margin-bottom:20px" href="#" onclick="toggleviewxml()">Show Retrieve Response</a>
    <textarea cols="200" runat="server" id="rawxml" 
        style="display:none;width:80%;padding-left:20px;background-color:lightgray;border:solid thin;margin-left:200px;margin-bottom:40px" rows="15"></textarea>
   </div>

    <div id="content" runat ="server">
        <!--place holder for form-->
    </div>
    <div id="submitmsg" runat="server"></div>

    <form runat="server">
         <asp:ScriptManager ID="ScriptManager1" EnablePageMethods="true" runat="server"></asp:ScriptManager>
       <%-- <div style="text-align:center;width:100%">
            
            <asp:Button runat="server"  ID="btnServerSubmit" Text="Server Submit" OnClientClick="serversubmit();return false;" />
        </div>--%>
        
    </form>
   
    
    <%--<div id="hiddenresponse" runat="server" style="display:none;white-space:pre"></div>--%>
   
</body>
</html>
