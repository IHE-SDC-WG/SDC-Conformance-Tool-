<%@ Page Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Submissions.aspx.cs" Inherits="SDC.Submissions" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2 id="title" runat="server">Form Receiver</h2>
   
    <div>
    <asp:GridView ID="GridView1" runat="server" Width="100%" AllowPaging="True" AutoGenerateColumns="False" 
        OnSelectedIndexChanged="GridView1_SelectedIndexChanged" OnPageIndexChanging="GridView1_PageIndexChanging">
        <Columns>
            <asp:BoundField DataField="transaction_id" HeaderText="Transaction Id" HeaderStyle-Width="100" />
            <asp:BoundField DataField="submit_time" HeaderText="Time Submitted" HeaderStyle-Width="100" />
            <asp:BoundField DataField="ip_address" HeaderText="Source IP" />
            <asp:BoundField DataField="Form_Id" HeaderText="Form Id" />
            <asp:BoundField DataField="Form_Name" HeaderText="Form Name" />
            <asp:BoundField DataField="validated" HeaderText="Validated" />
            <asp:BoundField DataField="Message" HeaderText="Message" />
             <asp:TemplateField ShowHeader="False">
                    <ItemTemplate>
                        
                      </ItemTemplate>
                 </asp:TemplateField>
             <asp:HyperLinkField DataNavigateUrlFields="transaction_id" DataNavigateUrlFormatString="~/SubmissionGetXML.aspx?transaction_id={0}" Target="_blank" Text="XML" />
             <asp:HyperLinkField DataNavigateUrlFields="transaction_id" DataNavigateUrlFormatString="~/SubmissionGetHTML.aspx?transaction_id={0}" Target="_blank" Text="HTML" />
        </Columns>
        <HeaderStyle BackColor="SteelBlue" ForeColor="White" />
        </asp:GridView>
    </div>
    
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SDCDB %>" 
            SelectCommand="SELECT [TRANSACTION_ID], [SUBMIT_TIME], [IP_ADDRESS], validated, message FROM [SDC_SUBMITS] order by transaction_id desc" 
            >

    </asp:SqlDataSource>
</asp:Content>