<%@ Page Title="" Language="C#"  AutoEventWireup="true" CodeBehind="SubmissionGetHTML.aspx.cs" Inherits="SDC.SubmissionGetHTML" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
     
     <!--hide the submit button-->
    <input type="hidden" runat="server" id="allowsubmit" value="no"/>
    <textarea runat="server" id="rawxml" style="display:none;width:80%;margin-left:80px;margin-right:80px;background-color:wheat" rows="10"></textarea>
    <div id="content" runat="server">
    
    </div>
   
</body>
</html>