<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="MyAccount.aspx.vb" Inherits="SystemManager_MyAccount" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            &nbsp;<asp:Literal ID="ltScript" runat="server" Visible="False"></asp:Literal>
            <asp:Label ID="lbMessage" runat="server" ForeColor="Red"></asp:Label>
         
                    <table>
                        <tr>
                            <td>
                                使用者名稱：</td>
                            <td>
                                <asp:TextBox ID="NameTextBox" runat="server"  Width="350px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>
                                使用者名稱(英文)：</td>
                            <td>
                                <asp:TextBox ID="EngNameTextBox" runat="server" Width="350px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>
                                職稱：</td>
                            <td>
                                <asp:TextBox ID="TitleTextBox" runat="server" Width="350px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>
                                單位：</td>
                            <td>
                                <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="SDS_Department_List"
                                    DataTextField="DepartmentName" DataValueField="DepartmentID"
                                    >
                                    <asp:ListItem Selected="True" Value="0">請選擇單位</asp:ListItem>
                                </asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td>
                                聯絡電話：</td>
                            <td>
                                <asp:TextBox ID="TelNoTextBox" runat="server" Width="350px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>
                                地址：</td>
                            <td>
                                <asp:TextBox ID="AddressTextBox" runat="server"  Width="350px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>
                                Email：</td>
                            <td>
                                <asp:TextBox ID="EmailTextBox" runat="server" Width="350px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>
                                身份證字號：</td>
                            <td>
                                <asp:TextBox ID="IDNumberTextBox" runat="server" Width="350px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>
                                登入帳號：</td>
                            <td>
                                <asp:Label ID="Label1" runat="server" ></asp:Label></td>
                        </tr>
                        <tr>
                            <td>
                                登入密碼：</td>
                            <td>
                                <asp:TextBox ID="PasswordTextBox" runat="server" TextMode="Password" Width="350px"></asp:TextBox>
                                <asp:Label ID="lbPasswordWarning" runat="server" Text="※ 如不更新密碼，請留空白。"></asp:Label>
                                </td>
                        </tr>
                    </table>
                    &nbsp;<br />
                    <asp:Button ID="ButtonUpdate" runat="server" CssClass="Btn"
                        Text="確定更新" />

            <asp:SqlDataSource ID="SDS_MyAccount" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                DeleteCommand="DELETE FROM [Accounts_Users] WHERE [UserID] = @UserID" InsertCommand="INSERT INTO [Accounts_Users] ([DepartmentID], [Title], [UserName], [Password], [Name], [EngName], [Email], [Address], [TelNo], [TelNo_Ext], [UpdateUser], [UpdateDateTime]) VALUES (@DepartmentID, @Title, @UserName, @Password, @Name, @EngName, @Email, @Address, @TelNo, @TelNo_Ext, @UpdateUser, @UpdateDateTime)"
                SelectCommand="SELECT UserID, DepartmentID, Title, UserName, Password, Name, EngName, Email, Address, TelNo, UpdateUser, UpdateDateTime, IDNumber FROM Accounts_Users WHERE (UserID = @UserID)"
                UpdateCommand="UPDATE Accounts_Users SET Title = @Title, Password = @Password, EngName = @EngName, Email = @Email, Address = @Address, TelNo = @TelNo, UpdateUser = @UpdateUser, UpdateDateTime = GETDATE(), IDNumber = @IDNumber, LastTimePasswordChanged = GETDATE(), NeedToChangePassword = 0 WHERE (UserID = @UserID)">
                <DeleteParameters>
                    <asp:Parameter Name="UserID" Type="Int32" />
                </DeleteParameters>
                <UpdateParameters>
                    <asp:Parameter Name="DepartmentID" Type="Int32" />
                    <asp:Parameter  Name="Title" Type="String" />
                    <asp:Parameter Name="Password" Type="String" />
                    <asp:Parameter Name="Name" Type="String"  />
                    <asp:Parameter Name="EngName" Type="String" />
                    <asp:Parameter Name="Email" Type="String" />
                    <asp:Parameter Name="Address" Type="String" />
                    <asp:Parameter Name="TelNo" Type="String"  />
                    <asp:SessionParameter Name="UpdateUser" SessionField="UserID" Type="Int32" />
                    <asp:SessionParameter Name="UserID" SessionField="UserID" Type="Int32" />
                    <asp:Parameter  Name="IDNumber"  />
                </UpdateParameters>
                <SelectParameters>
                    <asp:SessionParameter Name="UserID" SessionField="UserID" Type="Int32" />
                </SelectParameters>
                <InsertParameters>
                    <asp:Parameter Name="DepartmentID" Type="Int32" />
                    <asp:Parameter Name="Title" Type="String" />
                    <asp:Parameter Name="UserName" Type="String" />
                    <asp:Parameter Name="Password" Type="String" />
                    <asp:Parameter Name="Name" Type="String" />
                    <asp:Parameter Name="EngName" Type="String" />
                    <asp:Parameter Name="Email" Type="String" />
                    <asp:Parameter Name="Address" Type="String" />
                    <asp:Parameter Name="TelNo" Type="String" />
                    <asp:Parameter Name="TelNo_Ext" Type="String" />
                    <asp:Parameter Name="UpdateUser" Type="Int32" />
                    <asp:Parameter Name="UpdateDateTime" Type="DateTime" />
                </InsertParameters>
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="SDS_Department_List" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                SelectCommand="SELECT [DepartmentID], [DepartmentName] FROM [Department]"></asp:SqlDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

