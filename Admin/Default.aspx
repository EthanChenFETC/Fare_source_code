<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="_Default" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div id="float_bg" visible="false" runat="server" style="height:0px;">
</div>
<div id="float_function" visible="false" runat="server">
    <span class="float_title">公告訊息</span>
  
    <p>
      <asp:Label ID="lbSubect" runat="server" Text="Label"></asp:Label>
      <br />
    <asp:Label ID="lbContent" runat="server" Text="Label"></asp:Label>
        <asp:Button ID="btnCloseMessage" runat="server" Text="關閉訊息" cssclass="Btn" /></p>
</div>

<table class="HomeInfoTable">
        <tr>
            <td class="tdHead">
                網站基本資訊</td>
            <td class="tdHead">
                即時訊息公告</td>
        </tr>
        <tr>
            <td>
                <asp:DataList ID="DataList_SiteInfo" runat="server" DataSourceID="SDS_SiteInfo_List"
                    CssClass="HomeInfoSubTable">
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server"></asp:Label><br />
                        網站瀏覽總數：<asp:Label ID="CounterLabel" runat="server" ></asp:Label><br />
                        今日瀏覽總數：<asp:Label ID="CounterTodayLabel" runat="server" ></asp:Label><br />
                        昨日瀏覽總數：<asp:Label ID="CounterYesterdayLabel" runat="server" ></asp:Label><br />
                        網站文章總數：<asp:Label ID="DocCountLabel" runat="server" ></asp:Label><br />
                        網站文章異動：<asp:Label ID="LastUpdateDateLabel" runat="server" ></asp:Label><br />
                    </ItemTemplate>
                </asp:DataList>
                <asp:SqlDataSource ID="SDS_SiteInfo_List" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                    SelectCommand="SELECT Counter, CounterToday, CounterYesterday, DocCount, SiteName, SiteLastUpdate FROM Sites where IsOnline > 0 ">
                </asp:SqlDataSource>
            </td>
            <td rowspan="5">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="PublicID"
                    DataSourceID="SDS_AdminAnnounce_List" CssClass="Dg" AllowPaging="True" PageSize="20"
                    ShowHeader="False">
                    <Columns>
                        <asp:ButtonField DataTextField="Subject" HeaderText="公告主題" CommandName="EditPublish" />
                        <asp:TemplateField HeaderText="發佈者" SortExpression="Name">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" ></asp:Label>
                                <asp:Label ID="Label2" runat="server" ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="UpdateDateTime" HeaderText="發佈日期" SortExpression="UpdateDateTime" />
                    </Columns>
                    <RowStyle CssClass="DgItem" />
                    <AlternatingRowStyle CssClass="DgAltItem" />
                    <PagerStyle CssClass="DgPager" />
                    <HeaderStyle CssClass="DgHeader" />
                    <FooterStyle CssClass="DgFooter" />
                    <EditRowStyle CssClass="DgItem_edit" />
                    <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" />
                    <EmptyDataTemplate>
                        目前無系統內部公告或訊息！
                    </EmptyDataTemplate>
                </asp:GridView>
                <asp:SqlDataSource ID="SDS_AdminAnnounce_List" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                    SelectCommand="SELECT AdminAnnounce.PublicID, AdminAnnounce.Subject, AdminAnnounce.UpdateDateTime, AdminAnnounce.DepartmentID, AdminAnnounce.UpdateUser, Accounts_Users.Name, Department.DepartmentName, AdminAnnounce.PostDateTime FROM AdminAnnounce LEFT OUTER JOIN Accounts_Users ON AdminAnnounce.UpdateUser = Accounts_Users.UserID LEFT OUTER JOIN Department ON AdminAnnounce.DepartmentID = Department.DepartmentID ORDER BY AdminAnnounce.UpdateDateTime DESC, AdminAnnounce.PostDateTime DESC">
                </asp:SqlDataSource>
            </td>
        </tr>
        <tr>
            <td class="tdHead">
                登入資訊</td>
        </tr>
        <tr>
            <td>
                <asp:DataList ID="DataList_UserInfo" runat="server" DataSourceID="SDS_UserInfo_List"
                    RepeatLayout="Flow" CssClass="HomeInfoSubTable">
                    <ItemTemplate>
                        <asp:Label ID="NameLabel" runat="server"></asp:Label><br />
                        <ul>
                            <li>您前次登入：<asp:Label ID="LastLoginTimeLabel" runat="server"></asp:Label></li>
                            <li>您登入次數：<asp:Label ID="TotalLoginCountLabel" runat="server"></asp:Label></li>
                            <li>您的 IP位址：<asp:Label ID="LoginIPLabel" runat="server"></asp:Label><br />
                                <br />
                            </li>
                        </ul>
                    </ItemTemplate>
                </asp:DataList>
                <asp:SqlDataSource ID="SDS_UserInfo_List" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                    SelectCommand="SELECT [Name], [LastLoginTime], [TotalLoginCount] FROM [Accounts_Users] WHERE ([UserID] = @UserID)">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserID" SessionField="UserID" Type="Int32" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </td>
        </tr>
    </table>
<asp:Literal ID="ltScript" runat="server"></asp:Literal>
</asp:Content>

