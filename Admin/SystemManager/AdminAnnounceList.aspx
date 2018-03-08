<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="AdminAnnounceList.aspx.vb" Inherits="SystemManager_AdminAnnounceList" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                <tr>
                    <td>
                        資料搜尋：<asp:TextBox ID="txtSearch" runat="server" Columns="20" CssClass="form_txt"></asp:TextBox>&nbsp;<asp:Button
                            ID="btnSearch" runat="server" CssClass="Btn" Text="搜尋" />&nbsp;<asp:Button ID="btnShowAll"
                                runat="server" CssClass="Btn" Text="清除" />
                        <asp:Label ID="lblMessage" runat="server" EnableViewState="False" ForeColor="Red"></asp:Label>
                        &nbsp;
                        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                            CssClass="Dg" DataKeyNames="PublicID" DataSourceID="SDS_AdminAnnoce_List" PageSize="12">
                            <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
                            <FooterStyle CssClass="DgFooter" />
                            <RowStyle CssClass="DgItem" />
                            <EmptyDataTemplate>
                                目前沒有任何已發佈的資料！
                            </EmptyDataTemplate>
                            <EditRowStyle CssClass="DgItem_edit" />
                            <PagerStyle CssClass="DgPager" />
                            <HeaderStyle CssClass="DgHeader" />
                            <AlternatingRowStyle CssClass="DgAltItem" />
                            <Columns>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:Button ID="Button2" runat="server" CausesValidation="False" CommandName="Delete"
                                            OnClientClick="return confirm('您確定要刪除這筆資料？資料刪除無法回復！');" Text="刪除公告" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="PublicID" HeaderText="公告編號" InsertVisible="False" ReadOnly="True"
                                    SortExpression="PublicID" />
                                <asp:ButtonField CommandName="EditPublish" DataTextField="Subject" HeaderText="公告主題" />
                                <asp:BoundField DataField="Name" HeaderText="公告者" ReadOnly="True" SortExpression="Name" />
                                <asp:BoundField DataField="DepartmentName" HeaderText="公告單位" ReadOnly="True" SortExpression="DepartmentName" />
                                <asp:BoundField DataField="PostDateTime" HeaderText="公告時間" ReadOnly="True" SortExpression="PostDateTime" />
                                <asp:BoundField DataField="UpdateDateTime" HeaderText="最後更新時間" ReadOnly="True" SortExpression="UpdateDateTime" />
                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource ID="SDS_AdminAnnoce_List" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                            DeleteCommand="DELETE FROM [AdminAnnounce] WHERE [PublicID] = @PublicID" InsertCommand="INSERT INTO AdminAnnounce(Subject, DepartmentID, UpdateUser, UpdateDateTime) VALUES (@Subject, @DepartmentID, @UpdateUser, @UpdateDateTime)"
                            SelectCommand="SELECT AdminAnnounce.PublicID, AdminAnnounce.Subject, AdminAnnounce.UpdateDateTime, Department.DepartmentName, Accounts_Users.Name, AdminAnnounce.PostUser, AdminAnnounce.PostDateTime FROM AdminAnnounce INNER JOIN Department ON AdminAnnounce.PostDepartmentID = Department.DepartmentID INNER JOIN Accounts_Users ON AdminAnnounce.PostUser = Accounts_Users.UserID ORDER BY AdminAnnounce.PublicID DESC"
                            UpdateCommand="UPDATE AdminAnnounce SET Subject = @Subject, DepartmentID = @DepartmentID, UpdateUser = @UpdateUser, UpdateDateTime = @UpdateDateTime WHERE (PublicID = @PublicID)">
                            <DeleteParameters>
                                <asp:Parameter Name="PublicID" Type="Int32" />
                            </DeleteParameters>
                            <UpdateParameters>
                                <asp:Parameter Name="Subject" Type="String" />
                                <asp:Parameter Name="DepartmentID" Type="Int32" />
                                <asp:Parameter Name="UpdateUser" Type="Int32" />
                                <asp:Parameter Name="UpdateDateTime" Type="DateTime" />
                                <asp:Parameter Name="PublicID" Type="Int32" />
                            </UpdateParameters>
                            <InsertParameters>
                                <asp:Parameter Name="Subject" Type="String" />
                                <asp:Parameter Name="Content" Type="String" />
                                <asp:Parameter Name="DepartmentID" Type="Int32" />
                                <asp:Parameter Name="UpdateUser" Type="Int32" />
                                <asp:Parameter Name="UpdateDateTime" Type="DateTime" />
                            </InsertParameters>
                        </asp:SqlDataSource>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

