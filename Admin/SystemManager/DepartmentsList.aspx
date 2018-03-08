<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="DepartmentsList.aspx.vb" Inherits="SystemManager_DepartmentsList" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                <tr>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <div class="SearchArea">
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="width: 50%">
                                                <asp:CheckBox ID="cbDoItemOrder" runat="server" AutoPostBack="True" OnCheckedChanged="cbDoItemOrder_CheckedChanged"
                                                    Text="進行單位排序" /></td>
                                            <td style="width: 50%; text-align: right">
                                                資料搜尋：<asp:TextBox ID="txtSearch" runat="server" Columns="20" CssClass="form_txt"></asp:TextBox>&nbsp;<asp:Button
                                                    ID="btnSearch" runat="server" CssClass="Btn" Text="搜尋" />&nbsp;<asp:Button ID="btnShowAll"
                                                        runat="server" CssClass="Btn" Text="清除" />
                                                <asp:Label ID="lblMessage" runat="server" EnableViewState="False" ForeColor="Red"></asp:Label></td>
                                        </tr>
                                    </table>
                                </div>
                                <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                    CssClass="Dg" DataKeyNames="DepartmentID" DataSourceID="SqlDS_Departments" PageSize="12">
                                    <Columns>
                                        <asp:TemplateField ShowHeader="False">
                                            <EditItemTemplate>
                                                <asp:Button ID="Button1" runat="server" CausesValidation="True" CommandName="Update"
                                                    Text="更新" />&nbsp;<asp:Button ID="Button2" runat="server" CausesValidation="False"
                                                        CommandName="Cancel" Text="取消" />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Button ID="BtnEdit" runat="server" CausesValidation="False" CommandName="Edit"
                                                    Text="編輯" />&nbsp;<asp:Button ID="BtnDel" runat="server" CausesValidation="False"
                                                        CommandName="Delete" OnClientClick="return confirm('您確定要刪除這筆資料？資料刪除無法回復！');"
                                                        Text="刪除" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="DepartmentID" HeaderText="部門(單位)ID" InsertVisible="False"
                                            ReadOnly="True" SortExpression="DepartmentID" />
                                        <asp:TemplateField HeaderText="排序" ShowHeader="False" Visible="False">
                                            <ItemTemplate>
                                                <asp:Button ID="btnOrderUp" runat="server" CausesValidation="False" 
                                                    CommandName="OrderUp" Text=" ↑ " />
                                                <asp:Button ID="btnOrderDn" runat="server" CausesValidation="False" 
                                                    CommandName="OrderDn" Text=" ↓ " />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="DepartmentName" HeaderText="單位名稱" SortExpression="DepartmentName" />
                                        <asp:BoundField DataField="DepartmentEngName" HeaderText="單位英文名稱" SortExpression="DepartmentEngName" />
                                    </Columns>
                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlDS_Departments" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                    DeleteCommand="DELETE FROM [Department] WHERE [DepartmentID] = @DepartmentID"
                                    FilterExpression="DepartmentName LIKE '%{0}%' OR DepartmentEngName LIKE '%{1}%'"
                                    InsertCommand="INSERT INTO [Department] ([DepartmentName], [DepartmentEngName], [MailBoxID], [MailBoxPermissions], [UpdateUser], [UpdateDateTime]) VALUES (@DepartmentName, @DepartmentEngName, @MailBoxID, @MailBoxPermissions, @UpdateUser, @UpdateDateTime)"
                                    SelectCommand="Net2_Department_List" SelectCommandType="StoredProcedure" UpdateCommand="UPDATE Department SET DepartmentName = @DepartmentName, DepartmentEngName = @DepartmentEngName, UpdateUser = @UpdateUser, UpdateDateTime = GETDATE() WHERE (DepartmentID = @DepartmentID)">
                                    <FilterParameters>
                                        <asp:ControlParameter ControlID="txtSearch" Name="DepartmentName" PropertyName="Text" />
                                        <asp:ControlParameter ControlID="txtSearch" Name="DepartmentEngName" PropertyName="Text" />
                                    </FilterParameters>
                                    <DeleteParameters>
                                        <asp:Parameter Name="DepartmentID" Type="Int32" />
                                    </DeleteParameters>
                                    <UpdateParameters>
                                        <asp:Parameter Name="DepartmentName" Type="String" />
                                        <asp:Parameter Name="DepartmentEngName" Type="String" />
                                        <asp:SessionParameter Name="UpdateUser" SessionField="UserID" />
                                        <asp:Parameter Name="DepartmentID" Type="Int32" />
                                    </UpdateParameters>
                                    <SelectParameters>
                                        <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32" />
                                        <asp:Parameter Name="DepartmentID" Type="Int32" />
                                    </SelectParameters>
                                    <InsertParameters>
                                        <asp:Parameter Name="DepartmentName" Type="String" />
                                        <asp:Parameter Name="DepartmentEngName" Type="String" />
                                        <asp:Parameter Name="MailBoxID" Type="Int32" />
                                        <asp:Parameter Name="MailBoxPermissions" Type="Int32" />
                                        <asp:Parameter Name="UpdateUser" Type="Int32" />
                                        <asp:Parameter Name="UpdateDateTime" Type="DateTime" />
                                    </InsertParameters>
                                </asp:SqlDataSource>
                                <asp:SqlDataSource ID="sds_Order_Up" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                    ProviderName="<%$ ConnectionStrings:AdminModuleConnectionString.ProviderName %>"
                                    UpdateCommand="UPDATE Department SET ItemOrder = ItemOrder + 1 WHERE (DepartmentID = @UID)">
                                    <UpdateParameters>
                                        <asp:Parameter Name="UID" />
                                    </UpdateParameters>
                                </asp:SqlDataSource>
                                <asp:SqlDataSource ID="sds_Order_Dn" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                    ProviderName="<%$ ConnectionStrings:AdminModuleConnectionString.ProviderName %>"
                                    UpdateCommand="UPDATE Department SET ItemOrder = ItemOrder - 1 WHERE (DepartmentID = @UID)">
                                    <UpdateParameters>
                                        <asp:Parameter Name="UID" />
                                    </UpdateParameters>
                                </asp:SqlDataSource>
                                <asp:SqlDataSource ID="sds_Order_Update" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                    ProviderName="<%$ ConnectionStrings:AdminModuleConnectionString.ProviderName %>"
                                    UpdateCommand="UPDATE Department SET ItemOrder = @ItemOrder WHERE (DepartmentID = @DepartmentID)">
                                    <UpdateParameters>
                                        <asp:Parameter Name="ItemOrder" />
                                        <asp:Parameter Name="DepartmentID" />
                                    </UpdateParameters>
                                </asp:SqlDataSource>
                                &nbsp; &nbsp; &nbsp;&nbsp;
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <br />
                        &nbsp;
                    </td>
                </tr>
            </table>
</asp:Content>

