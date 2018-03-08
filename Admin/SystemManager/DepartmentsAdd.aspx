<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="DepartmentsAdd.aspx.vb" Inherits="SystemManager_DepartmentsAdd" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                <tr>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="lbl_Message" runat="server" ForeColor="Red"></asp:Label><br />
                                <asp:FormView ID="FormView1" runat="server" DataKeyNames="DepartmentID" DataSourceID="SDS_Department_Insert"
                                    DefaultMode="Insert">
                                    <InsertItemTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    單位名稱：</td>
                                                <td>
                                                    <asp:TextBox ID="DepartmentNameTextBox" runat="server"></asp:TextBox></td>
                                                <td>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="DepartmentNameTextBox"
                                                        ErrorMessage="請輸入單位名稱！"></asp:RequiredFieldValidator></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    單位英文名稱：</td>
                                                <td>
                                                    <asp:TextBox ID="DepartmentEngNameTextBox" runat="server" ></asp:TextBox></td>
                                                <td>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="DepartmentEngNameTextBox"
                                                        ErrorMessage="請輸入單位英文名稱！"></asp:RequiredFieldValidator></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <br />
                                                    <asp:Button ID="Button1" runat="server" CommandName="Insert" Text="新增資料" />&nbsp;
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                        </table>
                                    </InsertItemTemplate>
                                    
                                </asp:FormView>
                                <asp:SqlDataSource ID="SDS_Department_Insert" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                    DeleteCommand="DELETE FROM [Department] WHERE [DepartmentID] = @DepartmentID"
                                    InsertCommand="Net2_Department_Add" InsertCommandType="StoredProcedure" SelectCommand="SELECT [ParentDepartmentID], [DepartmentName], [DepartmentEngName], [MailBoxID], [MailBoxPermissions], [UpdateUser], [UpdateDateTime], [DepartmentID] FROM [Department]"
                                    UpdateCommand="UPDATE [Department] SET [ParentDepartmentID] = @ParentDepartmentID, [DepartmentName] = @DepartmentName, [DepartmentEngName] = @DepartmentEngName, [MailBoxID] = @MailBoxID, [MailBoxPermissions] = @MailBoxPermissions, [UpdateUser] = @UpdateUser, [UpdateDateTime] = @UpdateDateTime WHERE [DepartmentID] = @DepartmentID">
                                    <DeleteParameters>
                                        <asp:Parameter Name="DepartmentID" Type="Int32" />
                                    </DeleteParameters>
                                    <UpdateParameters>
                                        <asp:Parameter Name="ParentDepartmentID" Type="Int32" />
                                        <asp:Parameter Name="DepartmentName" Type="String" />
                                        <asp:Parameter Name="DepartmentEngName" Type="String" />
                                        <asp:Parameter Name="MailBoxID" Type="Int32" />
                                        <asp:Parameter Name="MailBoxPermissions" Type="Int32" />
                                        <asp:SessionParameter Name="UpdateUser" SessionField="UserID" />
                                        <asp:Parameter Name="UpdateDateTime" Type="DateTime" />
                                        <asp:Parameter Name="DepartmentID" Type="Int32" />
                                    </UpdateParameters>
                                    <InsertParameters>
                                        <asp:Parameter Name="DepartmentName" Type="String" />
                                        <asp:Parameter Name="DepartmentEngName" Type="String" />
                                        <asp:Parameter DefaultValue="0" Name="MailBoxID" Type="Int32" />
                                        <asp:SessionParameter Name="UpdateUser" SessionField="UserID" />
                                        <asp:Parameter Direction="Output" Name="retVal" Type="Int32" />
                                    </InsertParameters>
                                </asp:SqlDataSource>
                                <asp:SqlDataSource ID="SqlDS_MailBox" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                    SelectCommand="SELECT [MailBoxID], [MailBoxName] FROM [MailBox] where IsOnline > 0 order by ItemOrder"></asp:SqlDataSource>
                                <asp:SqlDataSource ID="SDS_DepartmentRelation_Insert" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                    InsertCommand="INSERT INTO DepartmentRelation(OwnerDepID, CreateDepID, UpdateUser, UpdateDateTime) VALUES (@OwnerDepID, @CreateDepID, @UpdateUser, GETDATE())"
                                    SelectCommand="SELECT [OwnerDepID], [CreateDepID], [UpdateUser], [UpdateDateTime] FROM [DepartmentRelation]">
                                    <InsertParameters>
                                        <asp:Parameter Name="OwnerDepID" />
                                        <asp:Parameter Name="CreateDepID" />
                                        <asp:Parameter Name="UpdateUser" />
                                    </InsertParameters>
                                </asp:SqlDataSource>
                                &nbsp;
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
</asp:Content>

