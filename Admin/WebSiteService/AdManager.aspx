<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="AdManager.aspx.vb" Inherits="WebSiteService_AdManager" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table cellspacing="0" cellpadding="5" width="100%" border="0">
        <tr>
            <td valign="top" runat="server" width="200">
                <table border="0" cellspacing="0" cellpadding="3">
                    <tr>
                        <td colspan="2" nowrap="nowrap">
                            新增廣告<asp:Label ID="lblAddMessage" runat="server" EnableViewState="False" ForeColor="Red"></asp:Label></td>
                    </tr>
                    <tr>
                        <td nowrap="nowrap">
                            廣告網站：</td>
                        <td>
                            <asp:DropDownList ID="ddlSite_List" runat="server" DataSourceID="sds_Sites_List"
                                DataTextField="SiteName" DataValueField="SiteID" AutoPostBack="true" OnSelectedIndexChanged="ddlSite_List_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="sds_Sites_List" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                ProviderName="<%$ ConnectionStrings:AdminModuleConnectionString.ProviderName %>"
                                SelectCommand="SELECT SiteID, SiteName FROM Sites where IsOnline > 0 order by SiteID"></asp:SqlDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td nowrap="nowrap">
                            廣告版位：</td>
                        <td>
                            <asp:DropDownList ID="ddlGroup_List" runat="server" DataSourceID="sds_Group_List"
                                DataTextField="GroupName" DataValueField="GroupID">
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="sds_Group_List" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                ProviderName="<%$ ConnectionStrings:AdminModuleConnectionString.ProviderName %>"
                                SelectCommand="SELECT GroupID, GroupName FROM AdvertisementsGroup where SiteID = @SiteID and IsOnline=1">
                            <SelectParameters>
                                <asp:Parameter Name="SiteID" Type="Int32" DefaultValue=0 />
                            </SelectParameters>
                            </asp:SqlDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td nowrap>
                            廣告名稱：</td>
                        <td>
                            <asp:TextBox ID="txtCaption" runat="server" Width="200" CssClass="form_txt"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td nowrap>
                            廣告註解：
                        </td>
                        <td>
                            <asp:TextBox ID="txtAlternateText" runat="server" Width="200" CssClass="form_txt" Rows="3" TextMode="MultiLine"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td nowrap>
                            廣告圖檔：</td>
                        <td>
                            <asp:FileUpload ID="FileUpload1" runat="server" CssClass="form_txt" /></td>
                    </tr>
                    <tr>
                        <td nowrap>
                            連結網址：</td>
                        <td>
                            <asp:TextBox ID="txtNavigateUrl" runat="server" Width="200px" CssClass="form_txt"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td nowrap>
                            關鍵字詞：</td>
                        <td>
                            <asp:TextBox ID="txtKeyword" runat="server" Width="200" CssClass="form_txt"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td nowrap>
                            出現機率：</td>
                        <td>
                            <asp:TextBox ID="txtImpressions" runat="server" Width="200" CssClass="form_txt">1</asp:TextBox></td>
                    </tr>
                    <tr>
                        <td nowrap>
                            &nbsp;</td>
                        <td>
                            <asp:Button ID="btnAddNew" runat="server" Width="100" Text="新增廣告" CausesValidation="False"
                                CssClass="Btn"></asp:Button></td>
                    </tr>
                </table>
          
            </td>
            <td align="left" valign="top" runat="server" width="85%">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                            廣告搜尋(S)：
                            <asp:TextBox ID="txtSearch" AccessKey="s" runat="server" Columns="20" CssClass="form_txt"></asp:TextBox>&nbsp;
                            <asp:Button ID="btnSearch" runat="server" Text="廣告搜尋" CausesValidation="False" CssClass="Btn">
                            </asp:Button>&nbsp;
                            <asp:Button ID="btnShowAll" runat="server" Text="全部列出" CssClass="Btn"></asp:Button>&nbsp;
                        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True" OnRowUpdating="GridView1_RowUpdating"
                            AutoGenerateColumns="False" CssClass="Dg" DataKeyNames="AdID" DataSourceID="SDS_AD_List" CaptionAlign="Top" EmptyDataText="目前沒有資料！">
                            <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
                            <FooterStyle CssClass="DgFooter" />
                            <Columns>
                                <asp:TemplateField ShowHeader="False">
                                    <EditItemTemplate>
                                        <asp:Button ID="btnUpdate" runat="server" CausesValidation="True" CommandName="Update"
                                            Text="更新" />
                                        <asp:Button ID="btnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                            Text="取消" />
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Button ID="btnEdit" runat="server" CausesValidation="False" CommandName="Edit"
                                            Text="編輯" />
                                        <asp:Button ID="btnDel" runat="server" CausesValidation="False" CommandName="Delete"
                                            OnClientClick="return confirm('您確定要刪除這筆資料？資料刪除無法回復！');" Text="刪除" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="AdID" HeaderText="編號" InsertVisible="False" ReadOnly="True"
                                    SortExpression="AdID" />
                                <asp:TemplateField HeaderText="圖檔/註解" SortExpression="Title">
                                    <EditItemTemplate>
                                        註解：<br />
                                        <asp:TextBox ID="TextBox1" runat="server"  Rows="3" TextMode="MultiLine"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Image ID="Image1" runat="server"  />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="網站／版位／廣告名稱／連結" SortExpression="Caption">
                                    <EditItemTemplate>
                                        網站：<asp:DropDownList ID="ddlSite_List2" runat="server" DataSourceID="sds_Site_List2"
                                            DataTextField="SiteName" DataValueField="SiteID"  AutoPostBack="true"  OnDataBound="ddlSite_List2_DataBound" OnSelectedIndexChanged="ddlSite_List2_SelectedIndexChanged" >
                                        </asp:DropDownList>
                                        <br />
                                        版位：<asp:DropDownList ID="ddlGroup_List2" runat="server" DataSourceID="sds_Group_List2"
                                            DataTextField="GroupName" DataValueField="GroupID"  OnDataBinding="ddlGroup_List2_DataBinding" OnDataBound ="ddlGroup_List2_DataBound">
                                        </asp:DropDownList>
                                        <br />
                                        名稱：<asp:TextBox ID="TextBox2" runat="server"  Width="200px"></asp:TextBox>
                                        <br />
                                        連結：<asp:TextBox ID="TextBox3" runat="server" Width="200px"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        網站：<asp:Label ID="Label1" runat="server" ></asp:Label><br />
                                        版位：<asp:Label ID="Label4" runat="server" ></asp:Label><br />
                                        名稱：<asp:Label ID="Label2" runat="server" ></asp:Label><br />
                                        連結：<asp:Label ID="Label3" runat="server" ></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Keyword" HeaderText="關鍵字" SortExpression="Keyword" />
                                <asp:BoundField DataField="impressions" HeaderText="機率" SortExpression="impressions" />
                                <asp:CheckBoxField DataField="IsOnline" HeaderText="上線狀態" SortExpression="IsOnline" />
                            </Columns>
                            <RowStyle CssClass="DgItem" />
                            <EditRowStyle CssClass="DgItem_edit" />
                            <PagerStyle CssClass="DgPager" />
                            <HeaderStyle CssClass="DgHeader" />
                            <AlternatingRowStyle CssClass="DgAltItem" />
                        </asp:GridView>
                        <asp:SqlDataSource ID="SDS_AD_List" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                            DeleteCommand="DELETE FROM [Advertisements] WHERE [AdID] = @AdID" 
                            InsertCommand="INSERT INTO [Advertisements] ([ImageUrl], [Caption], [AlternateText], [NavigateUrl], [Keyword], [impressions], [ShowCount], [ClickCount], [IsOnline], [DepartmentID], [UserID], [UpdateUser], [UpdateDateTime], GroupID) VALUES (@ImageUrl, @Caption, @AlternateText, @NavigateUrl, @Keyword, @impressions, @ShowCount, @ClickCount, @IsOnline, @DepartmentID, @UserID, @UpdateUser, @UpdateDateTime, @GroupID)"
                            SelectCommand="SELECT Advertisements.AdID, Advertisements.ImageUrl, Advertisements.Caption, Advertisements.AlternateText, Advertisements.NavigateUrl, Advertisements.Keyword, Advertisements.impressions, Advertisements.ShowCount, Advertisements.ClickCount, Advertisements.IsOnline, Advertisements.SiteID, Sites.SiteName, Advertisements.GroupID, AdvertisementsGroup.GroupName FROM Advertisements INNER JOIN Sites ON Advertisements.SiteID = Sites.SiteID Left JOIN AdvertisementsGroup ON Advertisements.GroupID = AdvertisementsGroup.GroupID ORDER BY Advertisements.SiteID, Advertisements.impressions DESC, Advertisements.AdID DESC"
                            UpdateCommand="UPDATE Advertisements SET Caption = @Caption, AlternateText = @AlternateText, NavigateUrl = @NavigateUrl, Keyword = @Keyword, impressions = @impressions, IsOnline = @IsOnline, UpdateUser = @UpdateUser, UpdateDateTime = GETDATE(), SiteID = @SiteID, GroupID = @GroupID WHERE (AdID = @AdID)"
                            FilterExpression="Caption like '%{0}%' OR ImageUrl like '%{1}%' OR GroupName like '%{2}%' OR Keyword like '%{3}%'">
                            <FilterParameters>
                            <asp:ControlParameter Name="Caption" ControlID="txtSearch" PropertyName="Text" />
                            <asp:ControlParameter Name="ImageUrl" ControlID="txtSearch" PropertyName="Text" />
                            <asp:ControlParameter Name="GroupName" ControlID="txtSearch" PropertyName="Text" />
                            <asp:ControlParameter Name="Keyword" ControlID="txtSearch" PropertyName="Text" />
                        </FilterParameters>
                            <DeleteParameters>
                                <asp:Parameter Name="AdID" Type="Int32" />
                            </DeleteParameters>
                            <UpdateParameters>
                                <asp:Parameter Name="Caption" />
                                <asp:Parameter Name="AlternateText" />
                                <asp:Parameter Name="NavigateUrl" />
                                <asp:Parameter Name="Keyword" />
                                <asp:Parameter Name="impressions" />
                                <asp:Parameter Name="IsOnline" />
                                <asp:SessionParameter Name="UpdateUser" SessionField="UserID" />
                                <asp:Parameter Name="AdID" />
                                <asp:Parameter Name="SiteID" />
                                <asp:Parameter Name="GroupID"/>
                            </UpdateParameters>
                            <InsertParameters>
                                <asp:Parameter Name="ImageUrl" Type="String" />
                                <asp:Parameter Name="Caption" Type="String" />
                                <asp:Parameter Name="AlternateText" Type="String" />
                                <asp:Parameter Name="NavigateUrl" Type="String" />
                                <asp:Parameter Name="Keyword" Type="String" />
                                <asp:Parameter Name="impressions" Type="Int16" />
                                <asp:Parameter Name="ShowCount" Type="Int32" />
                                <asp:Parameter Name="ClickCount" Type="Int32" />
                                <asp:Parameter Name="IsOnline" Type="Boolean" />
                                <asp:Parameter Name="DepartmentID" Type="Int32" />
                                <asp:Parameter Name="UserID" Type="Int32" />
                                <asp:Parameter Name="UpdateUser" Type="Int32" />
                                <asp:Parameter Name="UpdateDateTime" Type="DateTime" />
                                <asp:Parameter Name="GroupID" Type="Int32" />
                            </InsertParameters>
                        </asp:SqlDataSource>
                        <asp:SqlDataSource ID="sds_Site_List2" runat="server"  ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                ProviderName="<%$ ConnectionStrings:AdminModuleConnectionString.ProviderName %>"
                                SelectCommand="SELECT SiteID, SiteName FROM Sites"></asp:SqlDataSource>
                        <asp:SqlDataSource ID="sds_Group_List2" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                ProviderName="<%$ ConnectionStrings:AdminModuleConnectionString.ProviderName %>"
                                SelectCommand="SELECT GroupID, GroupName FROM AdvertisementsGroup where SiteID = @SiteID and IsOnline=1">
                            <SelectParameters>
                                <asp:Parameter Name="SiteID" Type="Int32" DefaultValue=0 />
                            </SelectParameters>
                            </asp:SqlDataSource>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
