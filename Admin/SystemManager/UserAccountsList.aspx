<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="UserAccountsList.aspx.vb" Inherits="SystemManager_UserAccountsList" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
            <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                <asp:View ID="View1" runat="server">
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 65%">
                                <asp:Button ID="btn_NewUser" runat="server" Text="新增人員" />
                                <asp:Label ID="lbMessage" runat="server" ForeColor="Red"></asp:Label>
                                &nbsp;&nbsp;
                            </td>
                            <td align="right" style="width: 35%">
                                搜尋：<asp:TextBox ID="txt_Search" runat="server"></asp:TextBox>
                                <asp:DropDownList ID="ddl_Department2" runat="server" AppendDataBoundItems="True"
                                    AutoPostBack="True" DataSourceID="sds_department" DataTextField="DepartmentName"
                                    DataValueField="DepartmentID">
                                    <asp:ListItem Value="%">全部部門</asp:ListItem>
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="sds_department" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                    ProviderName="<%$ ConnectionStrings:AdminModuleConnectionString.ProviderName %>"
                                    SelectCommand="SELECT DepartmentID, DepartmentName FROM Department ORDER BY ItemOrder">
                                </asp:SqlDataSource>
                                <asp:Button ID="btn_Search" runat="server" CausesValidation="False" Text="搜尋" />
                                <asp:Button ID="btn_Clear" runat="server" CausesValidation="False" Text="清除" /></td>
                        </tr>
                    </table>
                    <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
                        AutoGenerateColumns="False" CssClass="Dg" DataKeyNames="UserID" DataSourceID="SqlDS_UserAccounts"
                        PageSize="12">
                        <Columns>
                            <asp:TemplateField ShowHeader="False">
                             
                                <ItemTemplate>
                                    <asp:Button ID="BtnEdit" runat="server" CausesValidation="False" CommandName="EditUser"
                                        Text="編輯" Visible="False" />
                                    <asp:Button ID="BtnDel" runat="server" CausesValidation="False" 
                                        CommandName="SetDel" OnClientClick="return confirm('您確定要刪除這筆資料？資料刪除無法回復！');"
                                        Text="刪除" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="UserID" HeaderText="編號" InsertVisible="False" ReadOnly="True"
                                SortExpression="UserID" />
                            <asp:TemplateField HeaderText="姓名/英文姓名" SortExpression="Name">
                           
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbtn_FullName" runat="server" CausesValidation="False"
                                        CommandName="EditUser" ></asp:LinkButton>
                                    (<asp:Label ID="lbEngName" runat="server"></asp:Label>)
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="登入帳號/密碼" SortExpression="UserName">
                             
                                <ItemTemplate>
                                    <asp:Label ID="lbUserName" runat="server" ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="部門/職稱" SortExpression="DepartmentID">
                              
                                <ItemTemplate>
                                    <asp:Label ID="lbDepartmentName" runat="server"></asp:Label>
                                    (<asp:Label ID="lbTitle" runat="server" ></asp:Label>)
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
                            <asp:TemplateField HeaderText="電話/傳真" SortExpression="TelNo">
                             
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server"></asp:Label>,
                                    <asp:Label ID="Label2" runat="server" ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:CheckBoxField DataField="isOnline" HeaderText="帳號啟用" SortExpression="isOnline" />
                            <asp:TemplateField HeaderText="權限群組">
                                <ItemTemplate>
                                    <asp:CheckBoxList ID="CheckBoxList1" runat="server" Enabled="False">
                                    </asp:CheckBoxList>
                                </ItemTemplate>
                             
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="民意信箱權限" Visible="False">
                             
                                <ItemTemplate>
                                    <asp:Label ID="Label6" runat="server" ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <RowStyle CssClass="DgItem" />
                        <AlternatingRowStyle CssClass="DgAltItem" />
                        <PagerStyle CssClass="DgPager" />
                        <HeaderStyle CssClass="DgHeader" />
                        <FooterStyle CssClass="DgFooter" />
                        <EditRowStyle CssClass="DgItem_edit" />
                    </asp:GridView>
                    <asp:SqlDataSource ID="SqlDS_DepartmentList" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                        SelectCommand="SELECT DepartmentID, DepartmentName FROM Department where (@DepartmentID = 0 or (DepartmentID in (SELECT CreateDepID FROM DepartmentRelation WHERE (OwnerDepID = @DepartmentID))) or DepartmentID = @DepartmentID ) ORDER BY ItemOrder">
                        <SelectParameters>
                            <asp:Parameter Name="DepartmentID"  Type="Int32" DefaultValue ="0" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                    <asp:SqlDataSource ID="SDS_MailBoxPermissions_List" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                        SelectCommand="SELECT [MailBoxPermissionsID], [PermissionName] FROM [MailBoxPermissions] WHERE ([MailBoxPermissionsID] <= @MailBoxPermissionsID)">
                        <SelectParameters>
                            <asp:SessionParameter Name="MailBoxPermissionsID" SessionField="MailBoxPermissions"
                                Type="Int32" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                    <asp:SqlDataSource ID="SqlDS_UserAccounts" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                        DeleteCommand="DELETE FROM [Accounts_Users] WHERE [UserID] = @UserID" InsertCommand="INSERT INTO [Accounts_Users] ([DepartmentID], [Title], [UserName], [Password], [Name], [EngName], [Email], [TelNo], [FaxNo], [isOnline], [UpdateUser], [UpdateDateTime]) VALUES (@DepartmentID, @Title, @UserName, @Password, @Name, @EngName, @Email, @Address, @TelNo, @FaxNo, @isOnline, @UpdateUser, @UpdateDateTime)"
                        SelectCommand="Net2_Accounts_Users_List" SelectCommandType="StoredProcedure"
                        UpdateCommand="Net2_Accounts_Users_Update" UpdateCommandType="StoredProcedure">
                        <DeleteParameters>
                            <asp:Parameter Name="UserID" Type="Int32" />
                        </DeleteParameters>
                        <UpdateParameters>
                            <asp:Parameter Name="UserID" Type="Int32" />
                            <asp:Parameter Name="DepartmentID" Type="Int32" />
                            <asp:Parameter Name="Title" Type="String" />
                            <asp:Parameter Name="UserName" Type="String" />
                            <asp:Parameter Name="Name" Type="String" />
                            <asp:Parameter Name="EngName" Type="String" />
                            <asp:Parameter Name="Email" Type="String" />
                            <asp:Parameter DefaultValue="0" Name="MailBoxPermissions" Type="Int32" />
                            <asp:Parameter Name="TelNo" Type="String" />
                            <asp:Parameter Name="FaxNo" Type="String" />
                            <asp:Parameter Name="isOnline" Type="Boolean" />
                            <asp:SessionParameter Name="UpdateUser" SessionField="UserID" />
                        </UpdateParameters>
                        <InsertParameters>
                            <asp:Parameter Name="DepartmentID" Type="Int32" />
                            <asp:Parameter Name="Title" Type="String" />
                            <asp:Parameter Name="UserName" Type="String" />
                            <asp:Parameter Name="Password" Type="String" />
                            <asp:Parameter Name="Name" Type="String" />
                            <asp:Parameter Name="EngName" Type="String" />
                            <asp:Parameter Name="Email" Type="String" />
                            <asp:Parameter Name="Address" />
                            <asp:Parameter Name="TelNo" Type="String" />
                            <asp:Parameter Name="FaxNo" Type="String" />
                            <asp:Parameter Name="isOnline" Type="Boolean" />
                            <asp:Parameter Name="UpdateUser" Type="Int32" />
                            <asp:Parameter Name="UpdateDateTime" Type="DateTime" />
                        </InsertParameters>
                        <SelectParameters>
                            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32" />
                            <asp:Parameter Name="UserID" Type="Int32" />
                            <asp:ControlParameter ControlID="txt_Search" DefaultValue="%" Name="Keyword" PropertyName="Text"
                                Type="String" />
                            <asp:ControlParameter ControlID="ddl_Department2" DefaultValue="0" Name="DepartmentID"
                                PropertyName="SelectedValue" Type="String" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </asp:View>
                <asp:View ID="View2" runat="server">
                <asp:Label ID="Label8" runat="server" ForeColor="Red"></asp:Label>
                    <asp:DetailsView ID="DetailsView1" runat="server" AutoGenerateRows="False" CssClass="Dg"
                        DataKeyNames="UserID" DataSourceID="sds_UserDetail" Width="100%">
                        <Fields>
                            <asp:TemplateField HeaderText="所屬單位" SortExpression="DepartmentID">
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddl_Dep_List" runat="server" AppendDataBoundItems="True" AutoPostBack="True"
                                        DataSourceID="" DataTextField="DepartmentName" DataValueField="DepartmentID"
                                        OnSelectedIndexChanged="ddl_Dep_List_SelectedIndexChanged" >
                                        <asp:ListItem Selected="True" Value="0">選擇部門</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="ddl_Dep_List"
                                        Display="Dynamic" ErrorMessage="請選擇所屬部門！" MaximumValue="1000" MinimumValue="1"
                                        Type="Integer"></asp:RangeValidator>
                                </EditItemTemplate>
                                <InsertItemTemplate>
                                    <asp:DropDownList ID="ddl_Dep_List" runat="server" AppendDataBoundItems="True" AutoPostBack="True"
                                        DataSourceID="" DataTextField="DepartmentName" DataValueField="DepartmentID"
                                        OnSelectedIndexChanged="ddl_Dep_List_SelectedIndexChanged" >
                                        <asp:ListItem Selected="True" Value="0">選擇部門</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="ddl_Dep_List"
                                        Display="Dynamic" ErrorMessage="請選擇所屬部門！" MaximumValue="1000" MinimumValue="1"
                                        Type="Integer"></asp:RangeValidator>
                                </InsertItemTemplate>
                                <ItemStyle Width="90%" />
                                <HeaderStyle Width="80px" />
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server" ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="所屬科別" SortExpression="SectionID" Visible ="false" >
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddl_Section" runat="server" AppendDataBoundItems="True" DataSourceID=""
                                        DataTextField="SectionName" DataValueField="SectionID" OnDataBinding="ddl_Section_DataBinding">
                                        <asp:ListItem Selected="True" Value="0">選擇科室</asp:ListItem>
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <InsertItemTemplate>
                                    <asp:DropDownList ID="ddl_Section" runat="server" AppendDataBoundItems="True" DataSourceID=""
                                        DataTextField="SectionName" DataValueField="SectionID" OnDataBinding="ddl_Section_DataBinding">
                                        <asp:ListItem Selected="True" Value="0">選擇科室</asp:ListItem>
                                    </asp:DropDownList>
                                </InsertItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label4" runat="server" Text=''></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Title" HeaderText="職稱" SortExpression="Title" />
                            <asp:BoundField DataField="UserName" HeaderText="登入帳號" SortExpression="UserName" />
                            <asp:TemplateField HeaderText="密碼">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txt_P" runat="server" TextMode="Password"></asp:TextBox>
                                    <span style="color: #ff0066">※如不更新請留空白！</span>
                                </EditItemTemplate>
                                <InsertItemTemplate>
                                    <asp:TextBox ID="txt_P" runat="server" TextMode="Password"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="dv1_rfv_password" runat="server" ControlToValidate="txt_P"
                                        Display="Dynamic" ErrorMessage="請輸入密碼！"></asp:RequiredFieldValidator>
                                </InsertItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Name" HeaderText="姓名" SortExpression="Name" />
                            <asp:BoundField DataField="EngName" HeaderText="英文姓名" SortExpression="EngName" />
                            <asp:BoundField DataField="IDNumber" HeaderText="身份證字號" SortExpression="IDNumber" />
                            <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
                            <asp:BoundField DataField="Address" HeaderText="地址" SortExpression="Address" />
                           
                            <asp:TemplateField HeaderText="電話" SortExpression="TelNo">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                    分機：<asp:TextBox ID="TextBox4" runat="server"  Width="50px"></asp:TextBox>
                                </EditItemTemplate>
                                <InsertItemTemplate>
                                    <asp:TextBox ID="TextBox1" runat="server" ></asp:TextBox>
                                    分機：<asp:TextBox ID="TextBox4" runat="server" Width="50px"></asp:TextBox>
                                </InsertItemTemplate>
                                <ItemTemplate>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="FaxNo" HeaderText="傳真" SortExpression="FaxNo" />
                            <asp:CheckBoxField DataField="isOnline" HeaderText="帳號有效" SortExpression="isOnline"
                                Text="帳號有效" />
                            <asp:TemplateField HeaderText="帳號權限">
                                <EditItemTemplate>
                                    <asp:CheckBoxList ID="cbl_Permissions" runat="server" RepeatColumns="3" RepeatDirection="Horizontal">
                                    </asp:CheckBoxList>&nbsp;
                                </EditItemTemplate>
                                <InsertItemTemplate>
                                    <asp:CheckBoxList ID="cbl_Permissions" runat="server" RepeatColumns="3" RepeatDirection="Horizontal">
                                    </asp:CheckBoxList>
                                </InsertItemTemplate>
                                <ItemTemplate>
                                    <asp:CheckBoxList ID="cbl_Permissions" runat="server">
                                    </asp:CheckBoxList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False">
                                <EditItemTemplate>
                                    <asp:Button ID="Button1" runat="server" CausesValidation="True" CommandName="Update"
                                        Text="確定更新" />
                                    <asp:Button ID="Button2" runat="server" CausesValidation="False" CommandName="CancelEdit"
                                        Text="取消返回" />
                                </EditItemTemplate>
                                <InsertItemTemplate>
                                    <asp:Button ID="Button1" runat="server" CausesValidation="True" CommandName="Insert"
                                        Text="新增人員" />
                                    <asp:Button ID="Button2" runat="server" CausesValidation="False" CommandName="Cancel"
                                        Text="取消返回" />
                                </InsertItemTemplate>
                                <ItemTemplate>
                                  
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Fields>
                        <RowStyle CssClass="DgItem" />
                        <AlternatingRowStyle CssClass="DgAltItem" />
                    </asp:DetailsView>
                    <asp:Label ID="Label7" runat="server" EnableViewState="False"></asp:Label>
                    <asp:HiddenField ID="HiddenField1" runat="server" />
                    <asp:SqlDataSource ID="sds_UserDetail" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                        DeleteCommand="DELETE FROM [Accounts_Users] WHERE [UserID] = @UserID" 
                        InsertCommand="INSERT INTO Accounts_Users(DepartmentID, SectionID, Title, UserName, Name, IDNumber, EngName, Email, Address, TelNo, TelNo_Ext, FaxNo, isOnline, Password) VALUES (@DepartmentID, @SectionID, @Title, @UserName, @Name, @IDNumber, @EngName, @Email, @Address, @TelNo, @TelNo_Ext, @FaxNo, @isOnline, @Password)"
                        ProviderName="<%$ ConnectionStrings:AdminModuleConnectionString.ProviderName %>"
                        SelectCommand="SELECT Accounts_Users.UserID, Accounts_Users.DepartmentID, Accounts_Users.SectionID, Accounts_Users.Title, Accounts_Users.UserName, Accounts_Users.Name, Accounts_Users.IDNumber, Accounts_Users.EngName, Accounts_Users.Email, Accounts_Users.Address, Accounts_Users.TelNo, Accounts_Users.TelNo_Ext, Accounts_Users.FaxNo, Accounts_Users.isOnline, Department.DepartmentName, Section.SectionName, Accounts_Users.Password FROM Accounts_Users LEFT OUTER JOIN Department ON Accounts_Users.DepartmentID = Department.DepartmentID LEFT OUTER JOIN Section ON Accounts_Users.SectionID = Section.SectionID WHERE (Accounts_Users.UserID = @UserID)"
                        UpdateCommand="UPDATE Accounts_Users SET DepartmentID = @DepartmentID, SectionID = @SectionID, Title = @Title, UserName = @UserName, Name = @Name, IDNumber = @IDNumber, EngName = @EngName, Email = @Email, Address = @Address, TelNo = @TelNo, TelNo_Ext = @TelNo_Ext, FaxNo = @FaxNo, isOnline = @isOnline, Password = @Password WHERE (UserID = @UserID)">
                        <DeleteParameters>
                            <asp:Parameter Name="UserID" Type="Int32" />
                        </DeleteParameters>
                        <UpdateParameters>
                            <asp:Parameter Name="DepartmentID" Type="Int32" />
                            <asp:Parameter Name="SectionID" Type="Int32" DefaultValue="0" />
                            <asp:Parameter Name="Title" Type="String" />
                            <asp:Parameter Name="UserName" Type="String" />
                            <asp:Parameter Name="Name" Type="String" />
                            <asp:Parameter Name="IDNumber" Type="String" />
                            <asp:Parameter Name="EngName" Type="String" />
                            <asp:Parameter Name="Email" Type="String" />
                            <asp:Parameter Name="Address" Type="String" />
                            <asp:Parameter Name="TelNo" Type="String" />
                            <asp:Parameter Name="TelNo_Ext" Type="String" />
                            <asp:Parameter Name="FaxNo" Type="String" />
                            <asp:Parameter Name="isOnline" Type="Boolean" />
                            <asp:Parameter Name="UserID" Type="Int32" />
                            <asp:Parameter DefaultValue="" Name="Password" />
                        </UpdateParameters>
                        <SelectParameters>
                            <asp:Parameter Name="UserID" Type="Int32" />
                        </SelectParameters>
                        <InsertParameters>
                            <asp:Parameter Name="DepartmentID" Type="Int32" />
                            <asp:Parameter Name="SectionID" Type="Int32" DefaultValue="0" />
                            <asp:Parameter Name="Title" Type="String" />
                            <asp:Parameter Name="UserName" Type="String" />
                            <asp:Parameter Name="Name" Type="String" />
                            <asp:Parameter Name="IDNumber" Type="String" />
                            <asp:Parameter Name="EngName" Type="String" />
                            <asp:Parameter Name="Email" Type="String" />
                            <asp:Parameter Name="Address" Type="String" />
                            <asp:Parameter Name="TelNo" Type="String" />
                            <asp:Parameter Name="TelNo_Ext" Type="String" />
                            <asp:Parameter Name="FaxNo" Type="String" />
                            <asp:Parameter Name="isOnline" Type="Boolean" />
                            <asp:Parameter Name="Password" />
                        </InsertParameters>
                    </asp:SqlDataSource>
                    <asp:SqlDataSource ID="sds_Section" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                        ProviderName="<%$ ConnectionStrings:AdminModuleConnectionString.ProviderName %>"
                        SelectCommand="SELECT SectionID, SectionName, DepartmentID FROM Section WHERE (DepartmentID = @DepartmentID)">
                        <SelectParameters>
                            <asp:Parameter Name="DepartmentID" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                    <asp:SqlDataSource ID="sds_GroupUser" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                        DeleteCommand="DELETE FROM Accounts_GroupUsers&#13;&#10;WHERE          (UserID = @UserID)"
                        InsertCommand="INSERT INTO Accounts_GroupUsers&#13;&#10;                            (GroupID, UserID, UpdateUser, UpdateDateTime)&#13;&#10;VALUES          (@GroupID,@UserID,@UpdateUser, GETDATE())"
                        ProviderName="<%$ ConnectionStrings:AdminModuleConnectionString.ProviderName %>">
                        <DeleteParameters>
                            <asp:Parameter Name="UserID" />
                        </DeleteParameters>
                        <InsertParameters>
                            <asp:Parameter Name="GroupID" />
                            <asp:Parameter Name="UserID" />
                            <asp:SessionParameter Name="UpdateUser" SessionField="UserID" />
                        </InsertParameters>
                    </asp:SqlDataSource>
                    <asp:SqlDataSource ID="sds_HomePageBlock" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                        DeleteCommand="DELETE FROM PersonalizedGroup WHERE (UserID = @UserID)"
                        InsertCommand="Intra_HomePageBlockDefault" InsertCommandType="StoredProcedure"
                        ProviderName="<%$ ConnectionStrings:AdminModuleConnectionString.ProviderName %>">
                        <DeleteParameters>
                            <asp:Parameter Name="UserID" />
                        </DeleteParameters>
                        <InsertParameters>
                            <asp:Parameter Name="SiteID" />
                            <asp:Parameter Name="UserID" />
                        </InsertParameters>
                    </asp:SqlDataSource>
                        <asp:SqlDataSource ID="SDS_UsersRelation_Insert" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                            InsertCommand="INSERT INTO Accounts_UsersRelation (OwnerID, CreateID, UpdateUser, UpdateDateTime) VALUES (@OwnerID,@CreateID,@UpdateUser, GETDATE())"
                            SelectCommand="SELECT [OwnerDepID], [CreateDepID], [UpdateUser], [UpdateDateTime] FROM [DepartmentRelation]">
                            <InsertParameters>
                                <asp:Parameter Name="OwnerID" />
                                <asp:Parameter Name="CreateID" />
                                <asp:Parameter Name="UpdateUser" />
                            </InsertParameters>
                        </asp:SqlDataSource>
                </asp:View>
            </asp:MultiView>

</asp:Content>

