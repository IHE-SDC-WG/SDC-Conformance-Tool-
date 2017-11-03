<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="SDC.GetForms" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
        
    <div >       
    
        <h2>Form Manager</h2>
        <h3>Available Packages</h3>
            <asp:Panel ID="pnlforms" runat="server">
            <asp:gridview Width="100%" ID="forms" runat="server" AllowPaging="True" AutoGenerateColumns="False" 
                DataKeyNames="PACKAGE_ID"          
                OnRowDataBound="forms_RowDataBound" 
                OnPageIndexChanging="forms_PageIndexChanging" >
                <Columns>
                    <asp:BoundField DataField="PACKAGE_ID" HeaderText="Package Id" ReadOnly="True" SortExpression="PACKAGE_ID" />
                    <asp:BoundField DataField="Encoded_ID" HeaderText="Package Id" Visible="false" ReadOnly="True" SortExpression="Encoded_ID" />
                    <asp:BoundField DataField="PACKAGE_NAME" HeaderText="Package" SortExpression="PACKAGE_NAME" />
                    <asp:BoundField DataField="FORM_ID" HeaderText="Form ID" SortExpression="FORM_ID" />
                    <asp:BoundField DataField="FORM_NAME" HeaderText="Form Name" SortExpression="FORM_NAME" />
                    <asp:BoundField DataField="DATE_UPDATED" HeaderText="Date Uploaded" SortExpression="DATE_UPDATED" />
                    <%--<asp:HyperLinkField DataNavigateUrlFields="PACKAGE_ID" DataNavigateUrlFormatString="~/GetXML.aspx?packageid={0}" Target="_blank" Text="XML" />
                    <asp:HyperLinkField DataNavigateUrlFields="PACKAGE_ID" DataNavigateUrlFormatString="~/GetHTML.aspx?packageid={0}" Target="_blank" Text="HTML" />--%>
                    <asp:HyperLinkField DataNavigateUrlFields="Encoded_ID" DataNavigateUrlFormatString="~/GetXML.aspx?packageid={0}" Target="_blank" Text="XML" />
                    <asp:HyperLinkField DataNavigateUrlFields="Encoded_ID" DataNavigateUrlFormatString="~/GetHTML.aspx?packageid={0}" Target="_blank" Text="HTML" />
                    <asp:TemplateField ShowHeader="False">
                    
                        <ItemTemplate>
                        <asp:ImageButton ID="DeleteButton" runat="server" Width="8px" Height="8px" ImageUrl="~/Images/delete.png"
                            CommandName="Delete" CommandArgument='<%#Eval("Package_Id") %>' CausesValidation="true" OnClick="DeleteButton_Click" OnClientClick="return confirm('Are you sure you want to delete this package?');"
                            AlternateText="Delete" />               
                    </ItemTemplate>
                </asp:TemplateField> 
                </Columns>
                <HeaderStyle BackColor="SteelBlue" ForeColor="White" />
            </asp:gridview>
        </asp:Panel>
         <%--<div style="background-color:lightgray; margin-top:10px; color:black; width:100%;padding:4px;border:groove solid thin"> </div>--%>
    </div>
      
        
      <div style="background-color:lightgray; margin-top:2px; color:black; width:100%;padding:4px;border:groove solid thin">          
      <p style="font-weight:bold">Upload New SDC Package</p>            
      <%--<input type="file"  name="fileToUpload"  multiple="multiple" style="background-color:lightgray"  id="fileToUpload" onchange="fileSelected();"/><br />--%>
          <input type="file"  name="fileToUpload"  style="background-color:lightgray"  id="fileToUpload" onchange="fileSelected();"/><br />
      <div id="fileName"></div>
      <div id="fileSize"></div>
      <div id="fileType"></div>
          <table style="border-spacing:0px;">
              <tr style="height:11px; padding:2px;">
                  <td>Package ID</td><td><input style="height:10px" type="text" name="packageid" id="packageid" /></td>
              </tr>
              <tr style="height:11px;padding:2px;">
                  <td>Package Name</td><td><input style="height:10px" type="text" name ="packagename" id="packagename" /></td>
              </tr>
          </table>
       <div class="row">      
           <asp:Button runat="server"  ID="upload" OnClientClick="uploadFile();return false;" Text="Upload" />
    </div>
    </div>
   
   
    <div id="progressNumber"></div>
     
    
     
    
          <%--  <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:sdcdb %>" 
            SelectCommand="SELECT [PACKAGE_ID], [PACKAGE_NAME], [DATE_UPDATED] FROM [SDC_PACKAGES] ORDER BY [PACKAGE_NAME]" 
            DeleteCommand="DELETE FROM [SDC_PACKAGES] WHERE [PACKAGE_ID] = @original_PACKAGE_ID" 
           >
            <DeleteParameters>
                <asp:Parameter Name="original_PACKAGE_ID" Type="String" />
            </DeleteParameters>
            
        </asp:SqlDataSource>--%>
        
   <%-- <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:sdcdb %>" 
            SelectCommand="SELECT D.PACKAGE_ID, D.FORM_ID, D.DEMOG_FORM_ID, PACKAGE_NAME, FORM_NAME, DEMOG_FORM_NAME  FROM
                            (
                            SELECT PACKAGE_ID, A.FORM_ID, DEMOG_FORM_ID, PACKAGE_NAME, FORM_NAME FROM SDC_PACKAGES A left outer JOIN 
                            (SELECT FORM_ID, form_name FROM SDC_FORMS WHERE FORM_TYPE='SDC')  B
                            ON  A.FORM_ID = B.FORM_ID ) AS D
                            LEFT OUTER JOIN 
                            (SELECT FORM_ID, FORM_NAME as DEMOG_FORM_NAME FROM SDC_FORMS WHERE FORM_TYPE='DEMOG')  E
                            ON  D.DEMOG_FORM_ID = E.FORM_ID
                            union select null, null, null, null, null, null order by form_id" 
            
           >
            <DeleteParameters>
                <asp:Parameter Name="original_FORM_ID" Type="String" />
            </DeleteParameters>
            
        </asp:SqlDataSource>--%>

    <asp:SqlDataSource ID="FormList" runat="server" ConnectionString="<%$ ConnectionStrings:sdcdb %>"
         SelectCommand="select form_id, form_name from sdc_forms where form_type='SDC' UNION ALL select null AS form_id, null AS form_name order by form_name" >

    </asp:SqlDataSource>

    <asp:SqlDataSource ID="DemogList" runat="server" ConnectionString="<%$ ConnectionStrings:sdcdb %>"
         SelectCommand="select form_id, form_name from sdc_forms where form_type='DEMOG' UNION ALL select null AS form_id, null AS form_name order by form_name" >

    </asp:SqlDataSource>
</asp:Content>
    


