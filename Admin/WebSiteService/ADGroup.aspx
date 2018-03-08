<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="ADGroup.aspx.vb" Inherits="WebSiteService_ADGroup" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Button ID="btnAddNew" runat="server" OnClick="btnAddNew_Click" Text="新增分類代碼" />&nbsp;
            資料搜尋：<asp:TextBox ID="txtSearch" runat="server" Columns="20" CssClass="form_txt"></asp:TextBox>&nbsp;<asp:Button
                ID="btnSearch" runat="server" CssClass="Btn" Text="搜尋" />&nbsp;<asp:Button ID="btnShowAll"
                    runat="server" CssClass="Btn" Text="清除" />
            <asp:Label ID="lbMessage" runat="server" EnableViewState="False" ForeColor="Red"></asp:Label><br />
            <br />
            <asp:GridView ID="GridView1" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                DataKeyNames="GroupID" DataSourceID="sds_Categry">
                <Columns>
                    <asp:BoundField DataField="GroupID" HeaderText="編號" InsertVisible="False" ReadOnly="True"
                        SortExpression="GroupID" />
                    <asp:BoundField DataField="GroupName" HeaderText="分類名稱" InsertVisible="False"
                        SortExpression="GroupName" />
                    <asp:BoundField DataField="GroupCatgry" HeaderText="分類代碼" InsertVisible="False"
                        SortExpression="GroupCatgry" />
                    <asp:TemplateField HeaderText="網站名稱" ShowHeader="true" SortExpression="SiteID">
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddl_Site_List" runat="server" DataSourceID="sdsSites_List"
                                DataTextField="SiteName" DataValueField="SiteID"  >
                                <asp:ListItem Value="0">請選擇網站</asp:ListItem>
                            </asp:DropDownList>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="SiteName" runat="server" ></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ADCount" HeaderText="廣告顯示筆數" InsertVisible="False" 
                        SortExpression="ADCount" />
                    <asp:BoundField DataField="UpdateDateTime" HeaderText="最後更新" ReadOnly="True" SortExpression="UpdateDateTime" />
                    <asp:BoundField DataField="UpdateUser" HeaderText="更新人員" ReadOnly="True" SortExpression="UpdateUser" />
                    <asp:CheckBoxField DataField="IsOnline" HeaderText="是否上架" SortExpression="IsOnline" />
                    <asp:TemplateField HeaderText="管理" ShowHeader="False">
                        <EditItemTemplate>
                            <asp:Button ID="Button1" runat="server" CausesValidation="False" CommandName="UpdateItem"
                                Text="更新" />
                            <asp:Button ID="Button2" runat="server" CausesValidation="False" CommandName="Cancel"
                                Text="取消" />
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Button ID="Button1" runat="server" CausesValidation="False" CommandName="Edit"
                                Text="編輯" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="刪除" ShowHeader="False">
                        <ItemTemplate>
                            <asp:Button ID="Button12" runat="server" CausesValidation="False" CommandName="Delete"
                                OnClientClick="return confirm('您確定要刪除這筆分類資料？資料刪除無法回復！');" Text="刪除分類" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
                <asp:SqlDataSource ID="sdsSites_List" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                    ProviderName="<%$ ConnectionStrings:AdminModuleConnectionString.ProviderName %>"
                    SelectCommand="SELECT SiteID, SiteName FROM Sites"></asp:SqlDataSource>
                <asp:SqlDataSource ID="sds_Categry" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                DeleteCommand="DELETE FROM [AdvertisementsGroup] WHERE [GroupID] = @GroupID"
                InsertCommand="INSERT INTO AdvertisementsGroup(GroupName, GroupCatgry, SiteID, UpdateDateTime,UpdateUser, IsOnline, ADCount) VALUES (@GroupName, @GroupCatgry, @SiteID, GETDATE(), @UpdateUser, @IsOnline, @ADCount)"
                ProviderName="<%$ ConnectionStrings:AdminModuleConnectionString.ProviderName %>"
                SelectCommand="SELECT AdvertisementsGroup.GroupID, AdvertisementsGroup.GroupName, AdvertisementsGroup.GroupCatgry, AdvertisementsGroup.SiteID, AdvertisementsGroup.UpdateDateTime, AdvertisementsGroup.UpdateUser, AdvertisementsGroup.IsOnline, AdvertisementsGroup.ADCount, Sites.SiteName  FROM AdvertisementsGroup INNER JOIN Sites ON AdvertisementsGroup.SiteID = Sites.SiteID WHERE (GroupName LIKE N'%' + @Keyword + N'%') or (GroupCatgry LIKE N'%' + @Keyword + N'%')"
                UpdateCommand="UPDATE AdvertisementsGroup SET GroupName = @GroupName, GroupCatgry = @GroupCatgry, SiteID = @SiteID, UpdateDateTime = GETDATE(), UpdateUser = @UpdateUser, IsOnline = @IsOnline, ADCount = @ADCount WHERE (GroupID = @GroupID)">
                <DeleteParameters>
                    <asp:Parameter Name="GroupID" Type="Int32" />
                </DeleteParameters>
                <UpdateParameters>
                    <asp:Parameter Name="GroupName" Type="String" />
                    <asp:Parameter Name="GroupCatgry" Type="String" />
                    <asp:Parameter Name="SiteID" Type="Int32" />
                    <asp:SessionParameter Name="UpdateUser" SessionField="UserID" Type="Int32" />
                    <asp:Parameter Name="IsOnline" Type="Boolean" />
                    <asp:Parameter Name="GroupID" Type="Int32" />
                    <asp:Parameter Name="ADCount" Type="Int32" />
                </UpdateParameters>
                <SelectParameters>
                    <asp:ControlParameter ControlID="txtSearch" DefaultValue="%" Name="Keyword" PropertyName="Text" />
                </SelectParameters>
                <InsertParameters>
                    <asp:ControlParameter ControlID="txtGroupName" Name="GroupName" PropertyName="Text"
                        Type="String" />
                    <asp:ControlParameter ControlID="txtGroupCatgry" Name="GroupCatgry" PropertyName="Text"
                        Type="String" />
                    <asp:SessionParameter Name="UpdateUser" SessionField="UserID" Type="Int32" />
                    <asp:ControlParameter ControlID="ddl_Site_List2" Name="SiteID"  DefaultValue =''  />
                    <asp:ControlParameter ControlID="cbIsOnline" Name="IsOnline" PropertyName="Checked" />
                    <asp:ControlParameter ControlID="txtADCount1" Name="ADCount" PropertyName="Text" Type="Int32"  />
                </InsertParameters>
            </asp:SqlDataSource>
            <asp:MultiView ID="MultiView1" runat="server">
                <asp:View ID="View1" runat="server">
                    <table border="0" cellpadding="0" cellspacing="0" class="intranet" style="width: 100%">
                        <tr>
                            <td class="TopTitle">
                                新增/編輯分類</td>
                        </tr>
                        <tr>
                            <td class="MidContent">
                                <table>
                                    <tr>
                                        <td width="250">
                                            分類名稱：</td>
                                        <td width="90%">
                                            <asp:TextBox ID="txtGroupName" runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td width="250">
                                            分類代碼：</td>
                                        <td width="90%">
                                            <asp:TextBox ID="txtGroupCatgry" runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td width="250">
                                            網站名稱：</td>
                                        <td width="90%">
                            <asp:DropDownList ID="ddl_Site_List2" runat="server" AppendDataBoundItems="True" DataSourceID="sdsSites_List"
                                DataTextField="SiteName" DataValueField="SiteID">
                                <asp:ListItem Value="0">請選擇網站</asp:ListItem>
                            </asp:DropDownList>
                                    </tr>
                                    <tr>
                                        <td width="250">
                                            廣告顯示筆數：</td>
                                        <td width="90%">
                                            <asp:TextBox ID="txtADCount1" runat="server" ValidationGroup ="888"></asp:TextBox></td>
                            <asp:RequiredFieldValidator ID="rfv111" runat ="server" ErrorMessage = "請輸入數字" ValidationGroup ="888" ControlToValidate="txtADCount1" ></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator id="rev111" runat="server" ErrorMessage="請輸入正確數字" ValidationExpression="[0-9]"
							Display="Dynamic"  ControlToValidate="txtADCount1" ValidationGroup ="888" ></asp:RegularExpressionValidator>
                                    </tr>
                                    <tr>
                                        <td width="250">
                                            是否上線：</td>
                                        <td width="90%">
                                            <asp:CheckBox ID="cbIsOnline" runat="server" Checked="True" Text="上線(作用)" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="ButtomTitle">
                                <asp:Button ID="btn_Insert" runat="server" Text="確定新增" />
                                &nbsp;<asp:Button ID="btn_Cancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                    Text="取消返回" />&nbsp;
                            </td>
                        </tr>
                    </table>
                </asp:View>
            </asp:MultiView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

