<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="FaqCatgry.aspx.vb" Inherits="Publish_FaqCatgry" Title="Untitled Page" %>

<%@ Register Src="../common/ViewTextboxUser.ascx" TagName="ViewTextboxUser" TagPrefix="uc2" %>
<%@ Register Src="../common/ViewTextboxDep.ascx" TagName="ViewTextboxDep" TagPrefix="uc3" %>
<%@ Register Src="../common/SiteCheckBoxList.ascx" TagName="SiteCheckBoxList" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="width: 100%">
                <tr>
                    <td style="width: 50%">
            <asp:Button ID="BtnAddNew" runat="server" Text="新增分類代碼" /></td>
                    <td align="right" style="width: 50%; text-align: right">
                        &nbsp; &nbsp;
            &nbsp; &nbsp;&nbsp; 網站：<asp:RadioButtonList ID="rbl_Sites_List" runat="server" AppendDataBoundItems="True"
                AutoPostBack="True" DataSourceID="sds_SiteList" DataTextField="SiteName" DataValueField="SiteID"
                RepeatDirection="Horizontal" RepeatLayout="Flow">
                <asp:ListItem Value="%">所有網站</asp:ListItem>
            </asp:RadioButtonList>
            <asp:SqlDataSource ID="sds_SiteList" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                ProviderName="<%$ ConnectionStrings:AdminModuleConnectionString.ProviderName %>"
                SelectCommand="SELECT SiteID, SiteShortName AS SiteName FROM Sites WHERE (SiteID > 0)"></asp:SqlDataSource>
            資料搜尋：<asp:TextBox ID="txtSearch" runat="server" Columns="20" CssClass="form_txt"></asp:TextBox>&nbsp;<asp:Button
                ID="btnSearch" runat="server" CssClass="Btn" Text="搜尋" />&nbsp;<asp:Button ID="btnShowAll"
                    runat="server" CssClass="Btn" Text="清除" />
            <asp:Label ID="lbMessage" runat="server" EnableViewState="False" ForeColor="Red"></asp:Label><br />
                        &nbsp;</td>
                </tr>
            </table>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="CateGoryID"
                DataSourceID="sds_FaqCatgry_List" AllowPaging="True" AllowSorting="True">
                <Columns>
                    <asp:BoundField DataField="CateGoryID" HeaderText="編號" InsertVisible="False"
                        ReadOnly="True" SortExpression="CateGoryID" />
                    <asp:BoundField DataField="CateGoryName" HeaderText="分類名稱" SortExpression="CateGoryName" />
                    <asp:CheckBoxField DataField="IsOnline" HeaderText="是否上線" SortExpression="IsOnline" />
                    <asp:BoundField DataField="UpdateTime" HeaderText="最後更新日" ReadOnly ="true"  SortExpression="UpdateTime" />
                    <asp:BoundField DataField="SiteName" HeaderText="發佈網站" ReadOnly ="true"  SortExpression="SiteName" />
                    <asp:CommandField ButtonType="Button" ShowSelectButton="True" EditText="編輯分類" HeaderText="編輯分類" SelectText="編輯分類" />
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:Button ID="Button2" runat="server" Text="刪除分類" CausesValidation="False" CommandName="Delete"
                                OnClientClick="return confirm('您確定要刪除這筆分類資料？資料刪除無法回復！');"></asp:Button>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:SqlDataSource ID="sds_FaqCatgry_List" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                DeleteCommand="DELETE FROM [FaqCatgry] WHERE [CateGoryID] = @CateGoryID"
                ProviderName="<%$ ConnectionStrings:AdminModuleConnectionString.ProviderName %>"
                SelectCommand="SELECT FaqCatgry.CateGoryID, FaqCatgry.CateGoryName, FaqCatgry.IsOnline, FaqCatgry.UpdateTime, SitesApRelation.SiteID, Sites.SiteName FROM Sites INNER JOIN SitesApRelation ON Sites.SiteID = SitesApRelation.SiteID INNER JOIN FaqCatgry ON SitesApRelation.ApUID = FaqCatgry.CateGoryID WHERE (SitesApRelation.ApKeyword = @ApKeyword) AND (FaqCatgry.CateGoryName LIKE N'%' + @Keyword + N'%') AND (CONVERT (varchar(5), SitesApRelation.SiteID) LIKE @SiteID) ORDER BY FaqCatgry.CateGoryID"
                UpdateCommand="UPDATE [FaqCatgry] SET [CateGoryName] = @CateGoryName, [IsOnline] = @IsOnline, [UpdateTime] = getdate() WHERE [CateGoryID] = @CateGoryID">
                <DeleteParameters>
                    <asp:Parameter Name="CateGoryID" Type="Int32" />
                </DeleteParameters>
                <UpdateParameters>
                    <asp:Parameter Name="CateGoryName" Type="String" />
                    <asp:Parameter Name="IsOnline" Type="Boolean" />
                    <asp:Parameter Name="CateGoryID" Type="Int32" />
                </UpdateParameters>
                <SelectParameters>
                    <asp:Parameter Name="ApKeyword" />
                    <asp:ControlParameter ControlID="txtSearch" DefaultValue="%" Name="Keyword" PropertyName="Text" />
                    <asp:ControlParameter ControlID="rbl_Sites_List" DefaultValue="%" Name="SiteID" PropertyName="SelectedValue" />
                </SelectParameters>
            </asp:SqlDataSource>
            <asp:MultiView ID="MultiView1" runat="server">
                <asp:View ID="View1" runat="server">
                    <br />
                    <table border="0" cellpadding="0" cellspacing="0" class="intranet" style="width: 100%">
                        <tr>
                            <td class="TopTitle">
                                新增分類</td>
                        </tr>
                        <tr>
                            <td class="MidContent">
                                上稿網站：&nbsp;<uc1:SiteCheckBoxList ID="SiteCheckBoxList1" runat="server" />
                    <br />
                    分類名稱：<asp:TextBox ID="txtSubject" runat="server" CssClass="txt40"></asp:TextBox><br />
                    是否上線：<asp:CheckBox ID="cbIsonline" runat="server" Checked="True" Text="是否上線" /><br />
                    資料提供人員：<uc2:ViewTextboxUser ID="ViewTextboxUser2" runat="server" />
                    <br />
                    資料提供單位：<uc3:ViewTextboxDep ID="ViewTextboxDep2" runat="server" />
                    &nbsp;&nbsp;&nbsp;<br />
                                &nbsp; &nbsp;<br />
                    <asp:SqlDataSource ID="sds_Catgry_Detail" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                        DeleteCommand="DELETE FROM [FaqCatgry] WHERE [CateGoryID] = @CateGoryID" InsertCommand="Net2_FaqCatgry_Insert"
                        InsertCommandType="StoredProcedure" ProviderName="<%$ ConnectionStrings:AdminModuleConnectionString.ProviderName %>"
                        SelectCommand="SELECT CateGoryID, CateGoryName, IsOnline, UpdateTime, ResponUser, ResponDepartment FROM FaqCatgry WHERE (CateGoryID = @CateGoryID)"
                        UpdateCommand="UPDATE [FaqCatgry] SET [CateGoryName] = @CateGoryName, [IsOnline] = @IsOnline, [UpdateTime] = GetDate() WHERE [CateGoryID] = @CateGoryID">
                        <DeleteParameters>
                            <asp:Parameter Name="CateGoryID" Type="Int32" />
                        </DeleteParameters>
                        <UpdateParameters>
                            <asp:ControlParameter ControlID="txtSubject_Edit" Name="CateGoryName" PropertyName="Text"
                                Type="String" />
                            <asp:ControlParameter ControlID="cbIsonline_Edit" Name="IsOnline" PropertyName="Checked"
                                Type="Boolean" />
                            <asp:ControlParameter ControlID="GridView1" Name="CateGoryID" PropertyName="SelectedValue"
                                Type="Int32" />
                        </UpdateParameters>
                        <SelectParameters>
                            <asp:ControlParameter ControlID="GridView1" Name="CateGoryID" PropertyName="SelectedValue"
                                Type="Int32" />
                        </SelectParameters>
                        <InsertParameters>
                            <asp:ControlParameter ControlID="txtSubject" Name="CateGoryName" PropertyName="Text"
                                Type="String" />
                            <asp:ControlParameter ControlID="cbIsonline" Name="IsOnline" PropertyName="Checked"
                                Type="Boolean" />
                            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32" />
                            <asp:Parameter Direction="InputOutput" Name="retVal" Type="Int32" />
                            <asp:SessionParameter Name="ResponUser" SessionField="UserID" Type="Int32" />
                            <asp:SessionParameter Name="ResponDepartment" SessionField="DepartmentID" Type="Int32" />
                        </InsertParameters>
                    </asp:SqlDataSource></td>
                        </tr>
                        <tr>
                            <td class="ButtomTitle">
                                <asp:Button ID="btn_Insert" runat="server" Text="確定新增" />
                                <asp:Button ID="btn_Insert_temp" runat="server" CommandName="Insert_Tmp"
                                            Text="資料暫存" />
                                    <asp:Button ID="btn_CancelBack" runat="server" CausesValidation="False" CommandName="Cancel"
                                        Text="取消返回" /></td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="View2" runat="server">
                    <br />
                    <table border="0" cellpadding="0" cellspacing="0" class="intranet" style="width: 100%">
                        <tr>
                            <td class="TopTitle">
                                新增分類</td>
                        </tr>
                        <tr>
                            <td class="MidContent">
                                上稿網站：&nbsp;<uc1:SiteCheckBoxList ID="SiteCheckBoxList2" runat="server" />
                                <br />
                                分類名稱：<asp:TextBox ID="txtSubject_Edit" runat="server" CssClass="txt40"></asp:TextBox><br />
                                是否上線：<asp:CheckBox ID="cbIsonline_Edit" runat="server" Checked="True" Text="是否上線" /><br />
                                資料提供人員：<uc2:ViewTextboxUser ID="ViewTextboxUser1" runat="server" />
                                <br />
                                資料提供單位：<uc3:ViewTextboxDep ID="ViewTextboxDep1" runat="server" />
                                &nbsp; &nbsp;<br />
                                &nbsp; &nbsp;<br />
                            </td>
                        </tr>
                        <tr>
                            <td class="ButtomTitle">
                                <asp:Button ID="btn_Update" runat="server" Text="確定更新" />&nbsp;
                                <asp:Button ID="btn_Cancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                        Text="取消返回" /></td>
                        </tr>
                    </table>
                   
                </asp:View>
            </asp:MultiView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
