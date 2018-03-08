
<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="SiteMapUpdateTimeSetting.aspx.vb" Inherits="SystemDesign_SiteMapUpdateTimeSetting" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table border="0" cellpadding="5" cellspacing="0" width="100%">
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="sdsSites_List"
                            DataTextField="SiteName" DataValueField="SiteID">
                        </asp:DropDownList>
                        <asp:Button ID="btnSelectSite" runat="server" Text="選擇網站" />
                        <asp:Label ID="lbMessage" runat="server" ForeColor="Red"></asp:Label>
                        <asp:SqlDataSource ID="sdsSites_List" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                            ProviderName="<%$ ConnectionStrings:AdminModuleConnectionString.ProviderName %>"
                            SelectCommand="SELECT SiteID, SiteName FROM Sites where IsOnline > 0 "></asp:SqlDataSource>
                        <asp:SqlDataSource ID="SqlDataSource_AdminMenu" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                            SelectCommand="SELECT NodeId, ParentNodeId, Text, RefPath, PublishType, NodeLevel, HaveChildNode, NodeKeyword, DeadlineDays, ImageUrl FROM SiteMap WHERE (SiteID = @SiteID) ORDER BY AllNodeOrder"
                            UpdateCommand="UPDATE SiteMap SET DeadlineDays = @DeadlineDays WHERE (NodeId = @NodeID)">
                            <UpdateParameters>
                                <asp:Parameter Name="DeadlineDays" />
                                <asp:Parameter Name="NodeID" />
                            </UpdateParameters>
                            <SelectParameters>
                                <asp:ControlParameter ControlID="DropDownList1" Name="SiteID" PropertyName="SelectedValue" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="Dg"
                            DataKeyNames="NodeId" DataSourceID="SqlDataSource_AdminMenu" PageSize="15">
                            <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
                            <FooterStyle CssClass="DgFooter" />
                            <Columns>
                                <asp:CommandField ButtonType="Button" HeaderText="資料編輯" ShowEditButton="True">
                                    <ItemStyle Width="60px" />
                                </asp:CommandField>
                                <asp:BoundField DataField="NodeId" HeaderText="節點編號" ReadOnly="True" SortExpression="NodeId" />
                                <asp:TemplateField HeaderText="單元名稱" SortExpression="Text">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox1" runat="server" ></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Image ID="Image1" runat="server" />
                                        <asp:Label ID="Label1" runat="server" 
                                            Text=''></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="上稿稽催天數" SortExpression="PublishType">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDeadline" runat="server" Width="60px"></asp:TextBox>
                                        <asp:Label ID="lbDayWord" runat="server" Text="天 "></asp:Label><asp:Button ID="btnUpdateDeadline"
                                            runat="server" CommandName="UpdateDeadline" Text="更新稽催工作天數" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <RowStyle CssClass="DgItem" />
                            <EditRowStyle CssClass="DgItem_edit" />
                            <PagerStyle CssClass="DgPager" />
                            <HeaderStyle CssClass="DgHeader" />
                            <AlternatingRowStyle CssClass="DgAltItem" />
                        </asp:GridView>
                        &nbsp; &nbsp;
                    </ContentTemplate>
                </asp:UpdatePanel>
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;</td>
        </tr>
    </table>
</asp:Content>

