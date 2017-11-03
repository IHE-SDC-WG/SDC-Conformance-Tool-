<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="NewIndex.aspx.cs" Inherits="SDC.NewIndex" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
        
    <div >
        <h2>SDC Packages</h2>
        <div id="packages" runat="server">

        </div>
        
    <hr />
        <h2>SDC Forms</h2>
            <asp:Panel ID="pnlforms" runat="server">
            <asp:gridview Width="100%" ID="forms" runat="server" AllowPaging="True" AutoGenerateColumns="False" 
                DataKeyNames="FORM_ID" DataSourceID="SqlDataSource1" OnRowDeleted="Unnamed1_RowDeleted" 
                OnRowDeleting="Unnamed1_RowDeleting">
                <Columns>
                    <asp:BoundField DataField="FORM_ID" HeaderText="Form Id" ReadOnly="True" SortExpression="FORM_ID" />
                    <asp:BoundField DataField="FORM_NAME" HeaderText="Form Name" SortExpression="FORM_NAME" />
                    <asp:BoundField DataField="FORM_TYPE" HeaderText="Form Type" SortExpression="FORM_TYPE" />
                    <asp:BoundField DataField="DATE_UPDATED" HeaderText="Date Uploaded" SortExpression="DATE_UPDATED" />
                    <asp:BoundField DataField="LOADED_BY" HeaderText="Uploaded By" SortExpression="LOADED_BY" />
                    <asp:HyperLinkField DataNavigateUrlFields="FORM_ID" DataNavigateUrlFormatString="~/GetXML.aspx?formid={0}" Target="_blank" Text="XML" />
                    <asp:HyperLinkField DataNavigateUrlFields="FORM_ID" DataNavigateUrlFormatString="~/GetHTML.aspx?formid={0}" Target="_blank" Text="HTML" />
                    <asp:TemplateField ShowHeader="False">
                    <ItemTemplate>
                        <asp:ImageButton ID="DeleteButton" runat="server" Width="8px" Height="8px" ImageUrl="~/Images/delete.png"
                            CommandName="Delete" OnClientClick="return confirm('Are you sure you want to delete this event?');"
                            AlternateText="Delete" />               
                    </ItemTemplate>
                </asp:TemplateField> 
                </Columns>
                <HeaderStyle BackColor="SteelBlue" ForeColor="White" />
            </asp:gridview>
        </asp:Panel>
         <div style="background-color:lightgray; margin-top:10px; color:black; width:100%;padding:4px;border:groove solid thin"> </div>
    </div>
      
      
       <%-- <p style="font-weight:bold">Endpoints</p>
        <table>
            <tr><td>Form Manager</td><td><input type="text" id="manager" style="width:400px" /></td></tr>
            <tr><td>Form Receiver</td><td><input type="text" id="receiver" style="width:400px" /></td></tr>
        </table>
        <div class="row">
      <asp:Button ID="update" runat="server" OnClick="update_Click" Text="Update" />--%>
   
      <div style="background-color:lightgray; margin-top:2px; color:black; width:100%;padding:4px;border:groove solid thin">          
      <p style="font-weight:bold">Upload New SDC Form</p>            
      <input type="file" name="fileToUpload" style="background-color:lightgray"  id="fileToUpload" onchange="fileSelected();"/><br />
      <div id="fileName"></div>
      <div id="fileSize"></div>
      <div id="fileType"></div>
          <table>
              <tr style="height:14px">
                  <td>Form ID</td><td><input style="height:14px" type="text" name="formid" id="formid" /></td>
              </tr>
              <tr>
                  <td>Form Name</td><td><input style="height:14px" type="text" name ="formname" id="formname" /></td>
              </tr>
          </table>
       <div class="row">
      <%--<input type="button" onclick="uploadFile();return false;" value="Upload" />--%>
           <asp:Button runat="server"  ID="upload" OnClientClick="uploadFile();return false;" Text="Upload" />
    </div>
    </div>
   
   
    <div id="progressNumber"></div>

    
    
     
    
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:sdcdb %>" 
            SelectCommand="SELECT [FORM_ID], [FORM_NAME], [FORM_TYPE], [DATE_UPDATED], [LOADED_BY] FROM [SDC_FORMS] ORDER BY [FORM_NAME]" 
            DeleteCommand="DELETE FROM [SDC_FORMS] WHERE [FORM_ID] = @original_FORM_ID" 
           >
            <DeleteParameters>
                <asp:Parameter Name="original_FORM_ID" Type="String" />
            </DeleteParameters>
            
        </asp:SqlDataSource>
        
    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:sdcdb %>" 
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
            
        </asp:SqlDataSource>

    <asp:SqlDataSource ID="FormList" runat="server" ConnectionString="<%$ ConnectionStrings:sdcdb %>"
         SelectCommand="select form_id, form_name from sdc_forms where form_type='SDC' UNION ALL select null AS form_id, null AS form_name order by form_name" >

    </asp:SqlDataSource>

    <asp:SqlDataSource ID="DemogList" runat="server" ConnectionString="<%$ ConnectionStrings:sdcdb %>"
         SelectCommand="select form_id, form_name from sdc_forms where form_type='DEMOG' UNION ALL select null AS form_id, null AS form_name order by form_name" >

    </asp:SqlDataSource>
</asp:Content>
    

