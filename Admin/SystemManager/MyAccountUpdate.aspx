<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MyAccountUpdate.aspx.vb" Inherits="SystemManager_MyAccountUpdate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>未命名頁面</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager> 
    <div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            &nbsp;<asp:Literal ID="ltScript" runat="server" Visible="False"></asp:Literal>
            <asp:Label ID="lbMessage" runat="server" ForeColor="Red"></asp:Label>
            <asp:FormView ID="FormView1" runat="server" DataKeyNames="UserID" DataSourceID="SDS_MyAccount"
                DefaultMode="Edit">
                <EditItemTemplate>
                    <table>
                        <tr>
                            <td>
                                使用者名稱：</td>
                            <td>
                                &nbsp;<asp:Label ID="Label2" runat="server" ></asp:Label>
                                &nbsp; &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                使用者名稱(英文)：</td>
                            <td>
                                <asp:TextBox ID="EngNameTextBox" runat="server"  Width="350px"></asp:TextBox></td>
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
                                <asp:DropDownList ID="DropDownList1" runat="server" AppendDataBoundItems="True" DataSourceID="SDS_Department_List"
                                    DataTextField="DepartmentName" DataValueField="DepartmentID" Enabled="False"
                                   >
                                    <asp:ListItem Selected="True" Value="0">請選擇單位</asp:ListItem>
                                </asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td>
                                聯絡電話：</td>
                            <td>
                                <asp:TextBox ID="TelNoTextBox" runat="server"  Width="350px"></asp:TextBox></td>
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
                                <asp:TextBox ID="TextBox1" runat="server" Width="350px"></asp:TextBox>※若要註冊自然人登入請填寫。</td>
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
                                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="PasswordTextBox"
                                    Display="Dynamic" Enabled="False" ErrorMessage="您沒有輸入新密碼！"></asp:RequiredFieldValidator></td>
                        </tr>
                    </table>
                    &nbsp;<br />
                    <asp:Button ID="ButtonUpdate" runat="server" CommandName="Update" CssClass="Btn"
                        Text="確定更新" />
                </EditItemTemplate>
                
            </asp:FormView>
            <asp:SqlDataSource ID="SDS_MyAccount" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                DeleteCommand="DELETE FROM [Accounts_Users] WHERE [UserID] = @UserID" 
                InsertCommand="INSERT INTO [Accounts_Users] ([DepartmentID], [Title], [UserName], [Password], [Name], [EngName], [Email], [Address], [TelNo], [TelNo_Ext], [UpdateUser], [UpdateDateTime]) VALUES (@DepartmentID, @Title, @UserName, @Password, @Name, @EngName, @Email, @Address, @TelNo, @TelNo_Ext, @UpdateUser, @UpdateDateTime)"
                SelectCommand="SELECT UserID, DepartmentID, Title, UserName, Password, Name, EngName, Email, Address, TelNo, UpdateUser, UpdateDateTime, IDNumber FROM Accounts_Users WHERE (UserID = @UserID)"
                UpdateCommand="UPDATE Accounts_Users SET Title = @Title, Password = @Password, EngName = @EngName, Email = @Email, Address = @Address, TelNo = @TelNo, UpdateUser = @UpdateUser, UpdateDateTime = GETDATE(), IDNumber = @IDNumber, LastTimePasswordChanged = GETDATE(), NeedToChangePassword = 0 WHERE (UserID = @UserID)">
                <DeleteParameters>
                    <asp:Parameter Name="UserID" Type="Int32" />
                </DeleteParameters>
                <UpdateParameters>
                    <asp:Parameter  Name="Title" Type="String" />
                    <asp:Parameter Name="Password" Type="String" />
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
    </div>
    </form>
</body>
</html>
