<%@ Page EnableEventValidation="false" ValidateRequest="false" Language="C#" AutoEventWireup="true" CodeBehind="GetXML.aspx.cs" Inherits="SDC.GetXML" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        body{
            margin-left:40px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <textarea id="rawxml" rows="40" style="width:90%;align-content:center" runat="server"></textarea>
    </div>
        <asp:Button runat="server" Text="Update" OnClick="Unnamed_Click" />
        <asp:Button ID="close" runat="server" Text="Close" OnClientClick="window.close();return false;" />
        <asp:Label ID="msg" runat="server"></asp:Label>
    </form>
</body>
</html>
