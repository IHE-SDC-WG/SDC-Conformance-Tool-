<%@ Page MasterPageFile="~/Site1.Master" EnableEventValidation="false" ValidateRequest="false" Language="C#" AutoEventWireup="true" CodeBehind="GetForm.aspx.cs" Inherits="SDC.GetForm" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Form Filler</h2>  
    <h3 style="background-color:steelblue;color:white">Select a form manager</h3>
    <div id="formmanagers" runat="server" style="display:inline">
         <asp:RadioButtonList ClientIDMode="Static"  ID="lstManagers" RepeatDirection="Vertical" runat="server">
            
        </asp:RadioButtonList>
      
    </div>
       
    <h3 style="background-color:steelblue;color:white">Select one or more form receivers</h3>
    <div id="formreceivers" runat="server">
           <asp:CheckBoxList ID="lstReceivers" ClientIDMode="Static" RepeatDirection="Vertical" runat="server">

           </asp:CheckBoxList>

    </div>
    
    
    <span><b>Select Content Format</b></span>
    
       <asp:RadioButtonList RepeatDirection="Vertical" ClientIDMode="Static" ID="contentformat" runat="server" >
        <asp:ListItem Text = "XML" Value = "xml"></asp:ListItem>
        <asp:ListItem Text = "HTML" Value = "html"></asp:ListItem>        
        <asp:ListItem Text = "URN" Value = "urn"></asp:ListItem>   
      </asp:RadioButtonList> 
       
    
    
    <div id="packages" style="display:block" runat="server">
        
         <div id="getpackage"  runat="server" style="display:block">
             <%--<p>REST API not found for this form manager to get list of available packages. Please enter the package id to retrieve and click Retrieve</p>--%>
             
             <asp:Button runat="server" ID="retrievelist" Text="Get Package List" OnClick="retrievelist_Click" />  

            <div runat="server" id="divlist" style="display:none">
                <p>Select a package:</p>
                <asp:ListBox ClientIDMode="Static" ID="packagelist"  runat="server"></asp:ListBox>
            </div>   

             <p>Enter the package id below and click Retrieve. </p>
             <asp:TextBox ID="txtPackageid" ClientIDMode="Static" Width="200px" runat="server"></asp:TextBox> 
             <asp:Button ID="btnRetrierve" runat="server" Text="Retrieve" OnClientClick="retrieve_form('txtPackageid','contentformat')" />
             
             
             <%--<asp:Button runat="server" ID="btnRetrieveSelected" Visible="false" Text="Retrieve Selected" OnClick="btnRetrieveSelected_Click" />  --%>

             <asp:HiddenField ClientIDMode="Static" ID="formmanager" runat="server" />
             <asp:HiddenField ClientIDMode="Static" ID="receivers" runat="server" />            
             
             
         </div>
     </div>  
</asp:Content>

<%--<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <script type="text/javascript">
         function SelectForm()
         {
             var list = document.getElementById("configs");
             window.location = "GetForm.aspx?config_id=" + list.options[list.selectedIndex].value;
             
         }
     </script>
</head>
<body>
    
   
    
   <textarea cols="200" runat="server" id="rawxml" style="display:none;width:80%;margin-left:80px;margin-right:80px;background-color:wheat" rows="10"></textarea>

  
       <div id="parameter" runat="server" style="display:none">
          
           
       </div>
       <div id="forms" runat="server" style="display:none">

       </div>
    
        <div id="content" runat ="server">
        </div>
    
  
    
    
</body>
</html>--%>
