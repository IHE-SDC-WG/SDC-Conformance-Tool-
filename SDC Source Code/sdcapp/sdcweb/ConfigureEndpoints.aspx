<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ConfigureEndpoints.aspx.cs" Inherits="SDC.ConfigureEndpoints" %>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
  <%--  onrowcancelingedit="formreceivers_RowCancelingEdit"--%>
    <div>
        <h2>Form Filler Configuration</h2>
        <asp:UpdatePanel ID="update1" runat="server">
            <ContentTemplate>  
                <h3>Form Managers</h3>                              
                <asp:GridView ID="formmanager" runat="server"  
                    RowStyle-Wrap="true" 
                    AutoGenerateColumns = "false"
                    FooterStyle-BorderStyle="Solid"
                    FooterStyle-BorderWidth="1px" 
                    FooterStyle-BackColor="Silver"
                    HeaderStyle-BackColor = "SteelBlue" HeaderStyle-ForeColor="White" AllowPaging ="false"  ShowFooter = "true" 
                    onrowediting="formmanager_RowEditing"
                    onrowupdating="formmanager_RowUpdating"  onrowcancelingedit="formmanager_RowCancelingEdit"
                    OnRowDeleting="formmanager_RowDeleting"
                    OnRowDataBound="formmanager_RowDataBound"
                    CellSpacing="0"
                    CellPadding="0"
                    PageSize = "10">
                    <Columns>
                        <asp:TemplateField ItemStyle-Wrap="true" Visible="false" ItemStyle-Width = "80px"  HeaderText = "Config ID">
                            <ItemTemplate>
                                <asp:Label ID="lblID" runat="server"
                                Text='<%# Eval("ID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField ItemStyle-Width = "150px" HeaderText = "Form Manager">
                            <ItemTemplate>
                                <asp:Label style="word-wrap:break-word"  ID="lblFormManager" runat="server" Width="150px" 
                                    Text='<%# Eval("NAME") %>'/>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtFormManager"  runat="server" Width = "150px"
                                    Text='<%# Eval("NAME")%>'></asp:TextBox>
                            </EditItemTemplate> 
                            <FooterTemplate>
                                <asp:TextBox ID="txtFormManager" Width = "150px"
                                  runat="server"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField ItemStyle-Width = "400px" HeaderText = "Form List REST API Endpoint">
                            <ItemTemplate>
                                <asp:Label style="word-wrap:break-word"  ID="lblFormList" runat="server" Width="400px" 
                                    Text='<%# Eval("FORMLIST_ENDPOINT") %>'/>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtFormList" TextMode="MultiLine" runat="server" Width = "400px"
                                    Text='<%# Eval("FORMLIST_ENDPOINT")%>'></asp:TextBox>
                            </EditItemTemplate> 
                            <FooterTemplate>
                                <asp:TextBox ID="txtFormList" Width = "400px"
                                  runat="server"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width = "400px" HeaderText = "Retrieve Endpoint">
                            <ItemTemplate>
                                
                               <asp:Label style="word-wrap:break-word"  ID="lblRetrieve" runat="server" Width="400px" 
                                   Text='<%# Eval("RETRIEVE_ENDPOINT") %>'/>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtRetrieve" TextMode="MultiLine" runat="server" Width = "400px"
                                    Text='<%# Eval("RETRIEVE_ENDPOINT")%>'></asp:TextBox>
                            </EditItemTemplate> 
                            <FooterTemplate>
                                <asp:TextBox ID="txtRetrieve" Width="400px" runat="server"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField>
                            <ItemTemplate >
                                <asp:ImageButton ID="DeleteButton" runat="server" Width="8px" Height="8px" ImageUrl="~/Images/delete.png"
                            CommandName="Delete" OnClientClick="return confirm('Are you sure you want to delete this event?');"
                            AlternateText="Delete" /> 
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Button ID="btnAdd" runat="server" Text="Add"
                                    OnClick = "AddNewManager" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:CommandField  ShowEditButton="True"  ItemStyle-Width="20px" />
                    </Columns>
                    
                </asp:GridView>

                <h3>Form Receivers</h3>
                <asp:GridView ID="formreceivers" runat="server"  
                    RowStyle-Wrap="true" 
                    AutoGenerateColumns = "false"
                    FooterStyle-BorderStyle="Solid"
                    FooterStyle-BorderWidth="1px" 
                    FooterStyle-BackColor="Silver"
                    HeaderStyle-BackColor = "SteelBlue" HeaderStyle-ForeColor="White" AllowPaging ="false"  ShowFooter = "true" 
                    onrowediting="formreceivers_RowEditing"
                    onrowupdating="formreceivers_RowUpdating"                      
                    OnRowDeleting="formreceivers_RowDeleting"
                    OnRowDataBound="formreceivers_RowDataBound"
                    onrowcancelingedit="formreceivers_RowCancelingEdit"
                    CellSpacing="0"
                    CellPadding="0"
                    PageSize = "10">
                    <Columns>
                        <asp:TemplateField ItemStyle-Wrap="true" Visible="false" ItemStyle-Width = "80px"  HeaderText = "Config ID">
                            <ItemTemplate>
                                <asp:Label ID="lblID" runat="server"
                                Text='<%# Eval("ID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField ItemStyle-Width = "150px" HeaderText = "Form Receiver">
                            <ItemTemplate>
                                <asp:Label style="word-wrap:break-word"  ID="lblFormReceiver" runat="server" Width="150px" 
                                    Text='<%# Eval("NAME") %>'/>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtFormReceiver"  runat="server" Width = "150px"
                                    Text='<%# Eval("NAME")%>'></asp:TextBox>
                            </EditItemTemplate> 
                            <FooterTemplate>
                                <asp:TextBox ID="txtFormReceiver" Width = "150px"
                                  runat="server"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField ItemStyle-Width = "400px" HeaderText = "Submit Endpoint">
                            <ItemTemplate>
                                <asp:Label style="word-wrap:break-word"  ID="lblSubmit" runat="server" Width="400px" 
                                    Text='<%# Eval("SUBMIT_ENDPOINT") %>'/>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtSubmit" TextMode="MultiLine" runat="server" Width = "400px"
                                    Text='<%# Eval("SUBMIT_ENDPOINT")%>'></asp:TextBox>
                            </EditItemTemplate> 
                            <FooterTemplate>
                                <asp:TextBox ID="txtSubmit" Width = "400px"
                                  runat="server"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                       
                        <asp:TemplateField ItemStyle-Width = "40px" HeaderText = "Supports CORS">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkCORS" Enabled="false" runat="server" 
                                   Checked='<%#Convert.ToBoolean(Eval("SCRIPT_SUBMIT")) %>'/>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="chkCORS" runat="server" 
                                    Checked='<%#Convert.ToBoolean(Eval("SCRIPT_SUBMIT")) %>'></asp:CheckBox>
                            </EditItemTemplate> 
                            <FooterTemplate>
                                <asp:CheckBox ID="chkCORS" 
                                  runat="server"></asp:CheckBox>
                            </FooterTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField>
                            <ItemTemplate >
                                <asp:ImageButton ID="DeleteButton" runat="server" Width="8px" Height="8px" ImageUrl="~/Images/delete.png"
                            CommandName="Delete" OnClientClick="return confirm('Are you sure you want to delete this event?');"
                            AlternateText="Delete" /> 
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Button ID="btnAdd" runat="server" Text="Add"
                                    OnClick = "AddNewReceiver" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:CommandField  ShowEditButton="True"  ItemStyle-Width="20px" />
                    </Columns>
                    
                </asp:GridView>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="formmanager" />
            </Triggers>
        </asp:UpdatePanel>
        

        <%--<p>* Enter SubmitForm Namespace only if Form Receiver expects namespace other than urn:ihe:iti:rfd:2007</p>
        <p>** Enter SubmitForm Action only if Form Receiver expects action other than SubmitFormRequest</p>--%>
    </div>
</asp:Content>
