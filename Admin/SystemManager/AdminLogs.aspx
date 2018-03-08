<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="AdminLogs.aspx.vb" Inherits="SystemManager_AdminLogs" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
            &nbsp;<table border="0" cellpadding="5" cellspacing="0" width="100%">
                <tr>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <div class="SearchArea">
                                    資料搜尋：<asp:TextBox ID="txtSearch" runat="server" Columns="20" CssClass="form_txt"></asp:TextBox>&nbsp;<asp:Button
                                        ID="btnSearch" runat="server" CssClass="Btn" Text="搜尋" />&nbsp;<asp:Button ID="btnShowAll"
                                            runat="server" CssClass="Btn" Text="清除" />
                                    &nbsp;<asp:Label ID="lblMessage" runat="server" EnableViewState="False" ForeColor="Red"></asp:Label></div>
                                <br />
                                <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                    CssClass="Dg" DataKeyNames="LogID" DataSourceID="SqlDS_AdminLog" PageSize="12">
                                    <Columns>
                                        <asp:BoundField DataField="LogID" HeaderText="LogID" InsertVisible="False" ReadOnly="True"
                                            SortExpression="LogID" />
                                        <asp:BoundField DataField="Actions" HeaderText="動作" SortExpression="Actions">
                                            <HeaderStyle Width="25%" Wrap="true" />
                                        </asp:BoundField>
                                        <asp:CheckBoxField DataField="ActionsResult" HeaderText="執行結果" SortExpression="ActionsResult" />
                                        <asp:BoundField DataField="AdminMenu" HeaderText="管理單元" SortExpression="AdminMenu" />
                                        <asp:BoundField DataField="SiteMenu" HeaderText="前台單元" SortExpression="SiteMenu" />
                                        <asp:BoundField DataField="IP" HeaderText="IP" SortExpression="IP" />
                                        <asp:BoundField DataField="UpdateUser" HeaderText="操作人員" SortExpression="UpdateUser" />
                                        <asp:BoundField DataField="UpdateDateTime" HeaderText="操作時間" SortExpression="UpdateDateTime" />
                                    </Columns>
                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlDS_AdminLog" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                    SelectCommand="SELECT TOP 500 LogID, Actions, ActionsResult, AdminMenu, SiteMenu, IP, UpdateUser, UpdateDateTime FROM AdminLog WHERE (Actions LIKE '%' + @Actions + '%') OR (AdminMenu LIKE '%' + @AdminMenu + '%') ORDER BY LogID DESC">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="txtSearch" DefaultValue="%" Name="Actions" PropertyName="Text"
                                            Type="String" />
                                        <asp:ControlParameter ControlID="txtSearch" DefaultValue="%" Name="AdminMenu" PropertyName="Text"
                                            Type="String" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
</asp:Content>

