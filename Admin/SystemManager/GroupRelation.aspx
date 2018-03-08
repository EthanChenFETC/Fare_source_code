<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="GroupRelation.aspx.vb" Inherits="SystemManager_GroupRelation" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table>
                <tr>
                    <td width="500">
                        設定單位&nbsp;</td>
                    <td>
                    </td>
                    <td>
                        所屬單位</td>
                </tr>
                <tr>
                    <td valign="top" width="500">
                        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
                            AutoGenerateColumns="False" CssClass="Dg" DataKeyNames="GroupID" DataSourceID="SDS_Accounts_Group_List"
                            PageSize="12">
                            <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
                            <FooterStyle CssClass="DgFooter" />
                            <Columns>
                                <asp:BoundField DataField="GroupID" HeaderText="單位編號" InsertVisible="False" ReadOnly="True"
                                    SortExpression="Accounts_GroupID" />
                                <asp:BoundField DataField="GroupName" HeaderText="單位名稱" SortExpression="GroupName" />
                                <asp:TemplateField HeaderText="所屬單位資訊">
                                    <ItemTemplate>
                                        <asp:Label ID="lbInfo" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:ButtonField ButtonType="Button" CommandName="ViewItem" HeaderText="檢視投票項目" ShowHeader="True"
                                    Text="檢視所屬單位" />
                            </Columns>
                            <RowStyle CssClass="DgItem" />
                            <EditRowStyle CssClass="DgItem_edit" />
                            <PagerStyle CssClass="DgPager" />
                            <HeaderStyle CssClass="DgHeader" />
                            <AlternatingRowStyle CssClass="DgAltItem" />
                            <SelectedRowStyle CssClass="DgItem_edit" />
                        </asp:GridView>
                    </td>
                    <td>
                    </td>
                    <td valign="top">
                        <asp:Button ID="btbSubmit" runat="server" Text="更新設定" />
                        <asp:Label ID="lbMessage" runat="server" ForeColor="Red"></asp:Label>
                        <asp:CheckBoxList ID="cblMyOwnDep" runat="server" DataSourceID="SDS_Accounts_Group_List"
                            DataTextField="GroupName" DataValueField="GroupID" RepeatColumns="3" RepeatDirection="Horizontal"
                            Width="450px">
                        </asp:CheckBoxList></td>
                </tr>
            </table>
            <asp:SqlDataSource ID="SDS_Accounts_Group_List" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                SelectCommand="SELECT [GroupID], [GroupName] FROM [Accounts_Group]"></asp:SqlDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

