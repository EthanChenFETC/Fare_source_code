<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="SiteMapTitlePic.aspx.vb" Inherits="SystemDesign_SiteMapTitlePic" title="Untitled Page" %>
<%@ Register Src="~/common/Uploader.ascx" TagName="Uploader" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                <asp:View ID="View1" runat="server">
<table cellspacing="0" cellpadding="5" width="100%" border="0">
        <tr>
            <td>
                <asp:Literal ID="ltlMessage" runat="server" EnableViewState="false"></asp:Literal>
                <asp:DropDownList ID="ddl_Site_List" runat="server" AppendDataBoundItems="True" DataSourceID="sdsSites_List"
                    DataTextField="SiteName" DataValueField="SiteID">
                    <asp:ListItem Value="0">請選擇網站</asp:ListItem>
                </asp:DropDownList><asp:Button ID="btnSelectSite" runat="server" Text="選擇網站" />
                <asp:Label ID="lbMessage" runat="server" ForeColor="Red"></asp:Label>
                <asp:SqlDataSource ID="sdsSites_List" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                    ProviderName="<%$ ConnectionStrings:AdminModuleConnectionString.ProviderName %>"
                    SelectCommand="SELECT SiteID, SiteName FROM Sites where IsOnline > 0 "></asp:SqlDataSource>
                        <asp:GridView ID="GridView1" runat="server" CssClass="Dg" PageSize="15" DataKeyNames="NodeId, Text, TitlePic,TitlePic2, TitlePic3" AutoGenerateColumns="False" DataSourceID="SqlDataSource_SiteMap">
                            <Columns>
                                <asp:BoundField DataField="NodeId" HeaderText="節點編號" ReadOnly="True" SortExpression="NodeId" />
                                <asp:TemplateField HeaderText="單元名稱" SortExpression="Text">
                                    <ItemTemplate>
                                        <asp:Image ID="Image1" runat="server" />
                                        <asp:Label ID="Label1" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="TitlePic" HeaderText="選單標頭圖檔" ReadOnly="True" SortExpression="TitlePic" />
                                <asp:BoundField DataField="TitlePic2" HeaderText="選單明細圖檔" ReadOnly="True" SortExpression="TitlePic2" />
                                <asp:BoundField DataField="TitlePic3" HeaderText="內容標頭圖檔" ReadOnly="True" SortExpression="TitlePic3" />
                                <asp:TemplateField HeaderText="編輯">
                                    <ItemTemplate>
                                        <asp:Button ID="btnTitlePicUpload" runat="server" Text="編輯" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <RowStyle CssClass="DgItem" />
                            <AlternatingRowStyle CssClass="DgAltItem" />
                            <PagerStyle CssClass="DgPager" />
                            <HeaderStyle CssClass="DgHeader" />
                            <FooterStyle CssClass="DgFooter" />
                            <EditRowStyle CssClass="DgItem_edit" />
                            <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" />
                        </asp:GridView>
                        <asp:SqlDataSource ID="SqlDataSource_SiteMap" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                            SelectCommand="SELECT SiteMap.NodeId, SiteMap.ParentNodeId, SiteMap.Text, SiteMap.NodeLevel, SiteMap.HaveChildNode, PublishTypeSitemap.PublishTypeName, SiteMap.TitlePic, SiteMap.TitlePic2, SiteMap.TitlePic3, SiteMap.ImageUrl FROM SiteMap INNER JOIN PublishTypeSitemap ON SiteMap.PublishType = PublishTypeSitemap.PublishTypeID WHERE (SiteMap.SiteID = @SiteID) ORDER BY SiteMap.AllNodeOrder">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="ddl_Site_List" Name="SiteID" PropertyName="SelectedValue" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                        <asp:SqlDataSource ID="SqlDataSource_PublishType" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                            SelectCommand="SELECT [PublishTypeID], [PublishTypeName] FROM [PublishTypeSitemap]"></asp:SqlDataSource>
                &nbsp;<br />
            </td>
        </tr>
    </table>
    </asp:View>
    <asp:View ID="View2" runat="server">
                    <table border="0" cellpadding="0" cellspacing="0" class="intranet" style="width: 100%">
                        <tr>
                            <td class="TopTitle">
                                標頭圖檔上架</td>
                        </tr>
                        <tr>
                            <td class="MidContent">
                                <table>
                                    
                                    <tr>
                                        <td width="250">
                                            節點編號：</td>
                                        <td width="90%">
                                            <asp:Label ID="lblNodeID" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td width="250">
                                            單元名稱：</td>
                                        <td width="90%">
                                            <asp:Label ID="lblNodeText" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td width="250">
                                            選單標頭上傳：</td>
                                        <td width="90%">
                                            &nbsp;<uc1:Uploader id="Uploader1A" runat="server" SavePath="TitlePic" SingleUpload="true"></uc1:Uploader>
                                            <asp:Label ID="lbMessage2" runat="server" ForeColor="Red"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td width="250">
                                            選單明細上傳：</td>
                                        <td width="90%">
                                            &nbsp;<uc1:Uploader id="Uploader1B" runat="server" SavePath="TitlePic" SingleUpload="true"></uc1:Uploader>
                                            <asp:Label ID="lbMessage3" runat="server" ForeColor="Red"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td width="250">
                                            內容標頭背景上傳：</td>
                                        <td width="90%">
                                            &nbsp;<uc1:Uploader id="Uploader1C" runat="server" SavePath="TitlePic" SingleUpload="true"></uc1:Uploader>
                                            <asp:Label ID="lbMessage4" runat="server" ForeColor="Red"></asp:Label></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="ButtomTitle" style="height: 23px">
                                <asp:Button ID="btn_Update" runat="server" Text="確定更新" />&nbsp;<asp:Button ID="btn_Cancel"
                                    runat="server" CausesValidation="False" CommandName="Cancel" Text="取消返回" />&nbsp;
                            </td>
                        </tr>
                    </table>
                    <asp:SqlDataSource ID="sds_detail" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                        ProviderName="<%$ ConnectionStrings:AdminModuleConnectionString.ProviderName %>"
                        SelectCommand="SELECT * FROM Sitemap WHERE (NodeID = @NodeID)">
                        <SelectParameters>
                            <asp:Parameter Name="NodeID" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                <asp:SqlDataSource ID="sds_TitlePic" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                    ProviderName="<%$ ConnectionStrings:AdminModuleConnectionString.ProviderName %>"
                    UpdateCommand="UPDATE SiteMap SET TitlePic = @TitlePic,TitlePic2 = @TitlePic2,TitlePic3 = @TitlePic3 WHERE (NodeId = @NodeID)">
                    <UpdateParameters>
                        <asp:Parameter Name="TitlePic" />
                        <asp:Parameter Name="TitlePic2" />
                        <asp:Parameter Name="TitlePic3" />
                        <asp:Parameter Name="NodeID" />
                    </UpdateParameters>
                </asp:SqlDataSource>

                </asp:View>
            </asp:MultiView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

