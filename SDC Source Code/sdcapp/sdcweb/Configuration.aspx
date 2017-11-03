<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Configuration.aspx.cs" MasterPageFile="~/Site1.Master" Inherits="SDC.Configuration" %>
    <asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
        <h2>Parameter Configuration</h2>
    <div>

        <asp:UpdatePanel ID="update1" runat="server">
            <ContentTemplate>                
                <%--AlternatingRowStyle-BackColor = "#C2D69B"--%>
                <asp:GridView ID="parameters" runat="server"  
                    RowStyle-Wrap="true" 
                    AutoGenerateColumns = "false"
                    FooterStyle-BorderStyle="Solid"
                    FooterStyle-BorderWidth="1px" 
                    FooterStyle-BackColor="Silver"
                    HeaderStyle-BackColor = "SteelBlue" HeaderStyle-ForeColor="White" AllowPaging ="true"  ShowFooter = "true" 
                    OnPageIndexChanging = "OnPaging" onrowediting="EditParameter"
                    onrowupdating="UpdateParameter"  onrowcancelingedit="parameters_RowCancelingEdit"
                     OnRowDeleting="parameters_RowDeleting"
                     CellSpacing="0"
                     CellPadding="0"
                    PageSize = "10">
                    <Columns>
                        <asp:TemplateField ItemStyle-Wrap="true" ItemStyle-Width = "80px"  HeaderText = "Config ID">
                            <ItemTemplate>
                                <asp:Label ID="lblConfigID" runat="server"
                                Text='<%# Eval("CONFIG_ID") %>'></asp:Label>
                            </ItemTemplate>
                             
                        <%--    <FooterTemplate>
                                <asp:TextBox ID="txtConfigID" Width = "20px"
                                  runat="server"></asp:TextBox>
                            </FooterTemplate>--%>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width = "300px" HeaderText = "Form List API">
                            <ItemTemplate>
                                <asp:Label style="word-wrap:break-word"  ID="lblFormList" runat="server" Width="300px" 
                                    Text='<%# Eval("FORMLIST_ENDPOINT") %>'/>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtFormList" TextMode="MultiLine" runat="server" Width = "300px"
                                    Text='<%# Eval("FORMLIST_ENDPOINT")%>'></asp:TextBox>
                            </EditItemTemplate> 
                            <FooterTemplate>
                                <asp:TextBox ID="txtFormList" Width = "300px"
                                  runat="server"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width = "300px" HeaderText = "Retrieve Endpoint">
                            <ItemTemplate>
                                
                               <asp:Label style="word-wrap:break-word"  ID="lblRetrieve" runat="server" Width="300px" 
                                   Text='<%# Eval("RETRIEVE_ENDPOINT") %>'/>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtRetrieve" TextMode="MultiLine" runat="server" Width = "300px"
                                    Text='<%# Eval("RETRIEVE_ENDPOINT")%>'></asp:TextBox>
                            </EditItemTemplate> 
                            <FooterTemplate>
                                <asp:TextBox ID="txtRetrieve" Width="300px" runat="server"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField ItemStyle-Width = "300px" ItemStyle-Wrap="true" HeaderText = "Submit Endpoint">
                            <ItemTemplate>                                
                                <asp:Label style="word-wrap:break-word"  ID="lblSubmit" runat="server" Width="300px" 
                                    Text='<%# Eval("SUBMIT_ENDPOINT") %>'/>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtSubmit" runat="server" Width = "300px"
                                    Text='<%# Eval("SUBMIT_ENDPOINT")%>'></asp:TextBox>
                            </EditItemTemplate> 
                            <FooterTemplate>
                                <asp:TextBox ID="txtSubmit" Width="300px" runat="server"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField ItemStyle-Width = "100px"  HeaderText = "XSLT Path">
                            <ItemTemplate>                                
                                <asp:Label ID="lblXSLT" runat="server"
                                    Text='<%# Eval("TRANSFORM_PATH")%>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>                                
                                <asp:TextBox ID="txtXSLT" runat="server" Width = "100px"
                                    Text='<%# Eval("TRANSFORM_PATH")%>'></asp:TextBox>
                            </EditItemTemplate> 
                            <FooterTemplate>
                                <asp:TextBox ID="txtXSLT" Width="100px" runat="server"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        
                                          
                        <%--<asp:TemplateField ItemStyle-Width = "100px"  HeaderText = "*Submit Namespace">
                            <ItemTemplate>                                
                                <asp:Label style="word-wrap:break-word"  ID="lblSubmitNamespace" runat="server" Width="100px" 
                                    Text='<%# Eval("SUBMIT_FORM_NAMESPACE") %>'/>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtSubmitNamespace" runat="server" Width = "100px"
                                    Text='<%# Eval("SUBMIT_FORM_NAMESPACE")%>'></asp:TextBox>
                            </EditItemTemplate> 
                            <FooterTemplate>
                                <asp:TextBox ID="txtSubmitNamespace" Width="100px" runat="server"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>--%>

                        <%--<asp:TemplateField ItemStyle-Width = "150px"  HeaderText = "**Submit Action">
                            <ItemTemplate>                                
                                <asp:Label style="word-wrap:break-word"  ID="lblSubmitAction" runat="server" Width="150px" 
                                    Text='<%# Eval("SUBMIT_FORM_ACTION") %>'/>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtSubmitAction" runat="server" Width = "150px"
                                    Text='<%# Eval("SUBMIT_FORM_ACTION")%>'></asp:TextBox>
                            </EditItemTemplate> 
                            <FooterTemplate>
                                <asp:TextBox ID="txtSubmitAction" Width="150px" runat="server"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>--%>

                        <asp:TemplateField>
                            <ItemTemplate >
                                <%--<asp:LinkButton ID="lnkRemove" runat="server"
                                    CommandArgument = '<%# Eval("CONFIG_ID")%>'
                                 OnClientClick = "return confirm('Do you want to delete?')"
                                Text = "Delete" OnClick = "DeleteParameter"></asp:LinkButton>--%>
                                <asp:ImageButton ID="DeleteButton" runat="server" Width="8px" Height="8px" ImageUrl="~/Images/delete.png"
                            CommandName="Delete" OnClientClick="return confirm('Are you sure you want to delete this event?');"
                            AlternateText="Delete" /> 
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Button ID="btnAdd" runat="server" Text="Add"
                                    OnClick = "AddNewParameter" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:CommandField  ShowEditButton="True"  ItemStyle-Width="20px" />
                    </Columns>
                    <%--<AlternatingRowStyle BackColor="#C2D69B"  />--%>
                </asp:GridView>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="parameters" />
            </Triggers>
        </asp:UpdatePanel>
        

        <%--<p>* Enter SubmitForm Namespace only if Form Receiver expects namespace other than urn:ihe:iti:rfd:2007</p>
        <p>** Enter SubmitForm Action only if Form Receiver expects action other than SubmitFormRequest</p>--%>
    </div>
        
    </asp:Content>

