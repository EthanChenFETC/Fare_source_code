<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Group.aspx.vb" Inherits="SystemManager_Group" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                <asp:View ID="View1" runat="server">
                    <table style="width: 100%">
                        <tr>
                            <td>
                                群組名稱：<asp:TextBox ID="txt_GroupName" runat="server" CssClass="SinglLineText" Width="200px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txt_GroupName"
                                    Display="Dynamic" ErrorMessage="請輸入一個群組名稱！"></asp:RequiredFieldValidator>
                                <br />
                                群組說明：<asp:TextBox ID="txt_Description" runat="server" CssClass="SinglLineText" Width="200px"></asp:TextBox>
                                <asp:Button ID="btnAddNewGroup" runat="server" Text="新增群組" />
                                <asp:Label ID="lbMessage" runat="server" EnableViewState="False" ForeColor="Red"></asp:Label>
                                <asp:SqlDataSource ID="SqlDataSource_GroupAdd" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                    DeleteCommand="DELETE FROM [Accounts_Group] WHERE [GroupID] = @GroupID" InsertCommand="Net2_Accounts_Group_Add"
                                    InsertCommandType="StoredProcedure" SelectCommand="SELECT [GroupName], [Description], [isOnline], [UpdateUser], [UpdateDateTime], [GroupID] FROM [Accounts_Group]"
                                    UpdateCommand="UPDATE [Accounts_Group] SET [GroupName] = @GroupName, [Description] = @Description, [isOnline] = @isOnline, [UpdateUser] = @UpdateUser, [UpdateDateTime] = @UpdateDateTime WHERE [GroupID] = @GroupID">
                                    <DeleteParameters>
                                        <asp:Parameter Name="GroupID" Type="Int32" />
                                    </DeleteParameters>
                                    <UpdateParameters>
                                        <asp:Parameter Name="GroupName" Type="String" />
                                        <asp:Parameter Name="Description" Type="String" />
                                        <asp:Parameter Name="isOnline" Type="Boolean" />
                                        <asp:Parameter Name="UpdateUser" Type="Int32" />
                                        <asp:Parameter Name="UpdateDateTime" Type="DateTime" />
                                        <asp:Parameter Name="GroupID" Type="Int32" />
                                    </UpdateParameters>
                                    <InsertParameters>
                                        <asp:Parameter Name="GroupName" Type="String" />
                                        <asp:Parameter Name="Description" Type="String" />
                                        <asp:Parameter Name="isOnline" Type="Boolean" />
                                        <asp:Parameter Name="UpdateUser" Type="Int32" />
                                        <asp:Parameter Direction="InputOutput" Name="retVal" Type="Int32" />
                                    </InsertParameters>
                                </asp:SqlDataSource>
                                <asp:SqlDataSource ID="SqlDataSource_GroupRelationAdd" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                    InsertCommand="INSERT INTO Accounts_GroupsRelation(OwnerGroupID, CreateGroupID, UpdateUser, UpdateDateTime) VALUES (@OwnerGroupID, @CreateGroupID, @UpdateUser, GetDate())"
                                    SelectCommand="SELECT Accounts_GroupsRelation.* FROM Accounts_GroupsRelation">
                                    <InsertParameters>
                                        <asp:Parameter Name="OwnerGroupID" />
                                        <asp:Parameter Name="CreateGroupID" />
                                        <asp:Parameter Name="UpdateUser" />
                                    </InsertParameters>
                                </asp:SqlDataSource>
                            </td>
                            <td style="text-align: right">
                                <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                                <asp:TextBox ID="txtSearch" runat="server" Columns="20" CssClass="form_txt"></asp:TextBox>&nbsp;<asp:Button
                                    ID="btnSearch" runat="server" CausesValidation="False" CssClass="Btn" Text="搜尋" />&nbsp;<asp:Button
                                        ID="btnShowAll" runat="server" CausesValidation="False" CssClass="Btn" Text="清除" />
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                    <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                        CssClass="Dg" DataKeyNames="GroupID" DataSourceID="SqlDS_Group" PageSize="12">
                        <Columns>
                            <asp:TemplateField HeaderText="編輯" ShowHeader="False">
                                <ItemTemplate>
                                    <asp:Button ID="btn_EditItem" runat="server" CausesValidation="false" CommandName="EditItem"
                                        Text="編輯" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="刪除" ShowHeader="False">
                                <ItemTemplate>
                                    <asp:Button ID="BtnDel" runat="server" CausesValidation="False" 
                                        CommandName="GroupDelete" OnClientClick="return confirm('您確定要刪除這筆資料？群組資料刪除將導致所有本群組相關的權限設定一併刪除！');"
                                        Text="刪除" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="GroupID" HeaderText="編號" InsertVisible="False" ReadOnly="True"
                                SortExpression="GroupID" />
                            <asp:BoundField DataField="GroupName" HeaderText="權限群組名稱" SortExpression="GroupName" />
                            <asp:TemplateField HeaderText="權限群組說明" SortExpression="Description">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox1" runat="server" Rows="4"
                                        TextMode="MultiLine"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:CheckBoxField DataField="isOnline" HeaderText="群組狀態" SortExpression="isOnline"
                                Text="啟用" />
                            <asp:TemplateField HeaderText="群組網站上稿權限">
                                <ItemTemplate>
                                    &nbsp;<asp:Image ID="ImagePublishSites" runat="server" ImageUrl="~/SystemManager/images/notes.gif" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    &nbsp;
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="群組網站/前台權限">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnSetGroupPermission" runat="server" CausesValidation="False"
                                        CommandArgument="3" CommandName="SetGroupPermission" Text="網站/後端權限設定">
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:SqlDataSource ID="SqlDS_Group" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                        DeleteCommand="DELETE FROM Accounts_Group WHERE (GroupID = @GroupID)" 
                        InsertCommand="INSERT INTO [Accounts_Group] ([GroupName], [Description], [isOnline], [UpdateUser], [UpdateDateTime]) VALUES (@GroupName, @Description, @isOnline, @UpdateUser, @UpdateDateTime)"
                        UpdateCommand="UPDATE [Accounts_Group] SET [GroupName] = @GroupName, [Description] = @Description, [isOnline] = @isOnline, [UpdateUser] = @UpdateUser, [UpdateDateTime] = GETDATE() WHERE [GroupID] = @GroupID">
                        <DeleteParameters>
                            <asp:Parameter Name="GroupID" Type="Int32" />
                        </DeleteParameters>
                        <UpdateParameters>
                            <asp:ControlParameter ControlID="txt_GroupName2" Name="GroupName" PropertyName="Text"
                                Type="String" />
                            <asp:ControlParameter ControlID="txt_Description2" Name="Description" PropertyName="Text"
                                Type="String" />
                            <asp:ControlParameter ControlID="cbIsOnline" Name="isOnline" PropertyName="Checked"
                                Type="Boolean" />
                            <asp:SessionParameter Name="UpdateUser" SessionField="UserID" />
                            <asp:Parameter Name="GroupID" Type="Int32" />
                        </UpdateParameters>
                        <InsertParameters>
                            <asp:Parameter Name="GroupName" Type="String" />
                            <asp:Parameter Name="Description" Type="String" />
                            <asp:Parameter Name="isOnline" Type="Boolean" />
                            <asp:Parameter Name="UpdateUser" Type="Int32" />
                            <asp:Parameter Name="UpdateDateTime" Type="DateTime" />
                        </InsertParameters>
                    </asp:SqlDataSource>
                    <asp:SqlDataSource ID="SqlDs_MailBox" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                        SelectCommand="SELECT [MailBoxID], [MailBoxName] FROM [MailBox] where IsOnline > 0 order by ItemOrder"></asp:SqlDataSource>
                    <asp:SqlDataSource ID="SqlDs_MailBoxPermission" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                        SelectCommand="SELECT * FROM [MailBoxPermissions]"></asp:SqlDataSource>
                    <asp:SqlDataSource ID="SqlDataSource_Accounts_GroupSitePermission" runat="server"
                        ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>" SelectCommand="SELECT [GroupID], [SiteID] FROM [Accounts_GroupSitePermission] WHERE ([GroupID] = @GroupID)">
                        <SelectParameters>
                            <asp:Parameter Name="GroupID" Type="Int32" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </asp:View>
                <asp:View ID="View2" runat="server">
                    <table>
                        <tr>
                            <td style="vertical-align: top; width: 110px">
                                群組名稱：</td>
                            <td style="width: 700px">
                                <asp:TextBox ID="txt_GroupName2" runat="server" CssClass="SinglLineText" Width="300px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txt_GroupName2"
                                    Display="Dynamic" ErrorMessage="請輸入一個群組名稱！"></asp:RequiredFieldValidator></td>
                        </tr>
                        <tr>
                            <td style="vertical-align: top; width: 110px">
                                群組說明：</td>
                            <td style="width: 700px">
                                <asp:TextBox ID="txt_Description2" runat="server" CssClass="SinglLineText" Rows="5"
                                    TextMode="MultiLine" Width="300px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="vertical-align: top; width: 110px">
                                群組狀態：</td>
                            <td style="width: 700px">
                                <asp:CheckBox ID="cbIsOnline" runat="server" Text="上線" /></td>
                        </tr>
                        <tr>
                            <td style="vertical-align: top; width: 110px">
                                網站權限：</td>
                            <td style="width: 700px">
                                <asp:CheckBoxList ID="cbl_SiteList" runat="server" CellPadding="0" CellSpacing="0"
                                    RepeatColumns="3" Width="700px">
                                </asp:CheckBoxList></td>
                        </tr>
                        <tr>
                            <td style="width: 110px">
                            </td>
                            <td style="width: 700px">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 110px">
                            </td>
                            <td style="width: 700px">
                                <asp:Button ID="btn_Update" runat="server" Text="更新群組" />
                                <asp:Button ID="btn_Cancel" runat="server" CausesValidation="False" Text="取消返回" />
                                <asp:Label ID="lbMessage_Edit" runat="server" EnableViewState="False" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    &nbsp;<br />
                    <br />
                    <br />
                </asp:View>
            </asp:MultiView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

