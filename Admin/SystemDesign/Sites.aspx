<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Sites.aspx.vb" Inherits="SystemDesign_Sites" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table border="0" cellpadding="5" cellspacing="0" width="100%">
        <tr>
            <td class="LeftCol" valign="top">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                       
                                <table>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            網站名稱：<br />
                                            <asp:TextBox ID="SiteNameTextBox" runat="server" Text='' Width="250px"></asp:TextBox></td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            網站簡稱：<br />
                                            <asp:TextBox ID="SiteShortNameTxt" runat="server" Text=''
                                                Width="250px"></asp:TextBox><br />
                                            網站根節點ID：<br />
                                            (自動產生)</td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            網站簡介：<br />
                                            <asp:TextBox ID="DescriptionTextBox" runat="server" Rows="6" Text=''
                                                TextMode="MultiLine" Width="250px"></asp:TextBox></td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 25px">
                                        </td>
                                        <td style="height: 25px">
                                            <asp:Button ID="Button1" runat="server" Text="新增網站" />
                                            <asp:Button ID="Button2" runat="server" Text="取消" style="height: 21px" /></td>
                                        <td style="height: 25px">
                                        </td>
                                    </tr>
                                </table>
                       
                        <asp:SqlDataSource ID="SqlDS_SitesAdd" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                            
                            InsertCommand="INSERT INTO [Sites] ([SiteName], [SiteRootNodeID], [Description], SiteshortName) VALUES (@SiteName, @SiteRootNodeID, @Description, @SiteshortName)"
                            >
                            <InsertParameters>
                                <asp:ControlParameter ControlID="SiteNameTextBox" Name="SiteName" Type="String" />
                                <asp:ControlParameter ControlID="SiteShortNameTxt" Name="SiteShortName" Type="String" />
                                <asp:ControlParameter ControlID="DescriptionTextBox" Name="Description" Type="String" />
                            </InsertParameters>
                        </asp:SqlDataSource>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td class="RightCol" valign="top">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="SearchArea">
                            <asp:Label ID="lblMessage" runat="server" EnableViewState="False" ForeColor="Red"></asp:Label>&nbsp;</div>
                        <br />
                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="Dg"
                            DataKeyNames="SiteID" DataSourceID="SqlDS_Sites" PageSize="12">
                            <RowStyle CssClass="DgItem" />
                            <AlternatingRowStyle CssClass="DgAltItem" />
                            <PagerStyle CssClass="DgPager" />
                            <HeaderStyle CssClass="DgHeader" />
                            <FooterStyle CssClass="DgFooter" />
                            <EditRowStyle CssClass="DgItem_edit" />
                            <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
                            <Columns>
                                <asp:TemplateField HeaderText="編輯" ShowHeader="False">
                                    <EditItemTemplate>
                                        <asp:Button ID="Button1" runat="server" CausesValidation="True" CommandName="Update"
                                            Text="更新" />&nbsp;<asp:Button ID="Button2" runat="server" CausesValidation="False"
                                                CommandName="Cancel" Text="取消" />
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Button ID="Button1" runat="server" CausesValidation="False" CommandName="Edit"
                                            Text="編輯" />&nbsp;<asp:Button ID="Button2" runat="server" CausesValidation="False"
                                                CommandName="Delete" OnClientClick="return confirm('您確定要刪除這筆資料？資料刪除無法回復！');"
                                                Text="刪除" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="SiteID" HeaderText="網站ID" InsertVisible="False" ReadOnly="True"
                                    SortExpression="SiteID" />
                                <asp:BoundField DataField="SiteName" HeaderText="網站名稱" SortExpression="SiteName" />
                                <asp:BoundField DataField="SiteShortName" HeaderText="網站簡稱" SortExpression="SiteShortName" />
                                <asp:BoundField DataField="SiteRootNodeID" HeaderText="網站節點編號" SortExpression="SiteRootNodeID" ReadOnly ="true"  />
                                <asp:BoundField DataField="Description" HeaderText="網站簡介" SortExpression="Description" />
                                <asp:CheckBoxField DataField="IsOnline" HeaderText="啟用" SortExpression="IsOnline" />
                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource ID="SqlDS_Sites" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                            DeleteCommand="Net2_Sites_Del" DeleteCommandType="StoredProcedure" SelectCommand="SELECT SiteID, SiteName, SiteRootNodeID, Description, SiteShortName, IsOnline FROM Sites"
                            UpdateCommand="UPDATE Sites SET SiteName = @SiteName, Description = @Description, SiteShortName = @SiteShortName, IsOnline = @IsOnline WHERE (SiteID = @SiteID)">
                            <UpdateParameters>
                                <asp:Parameter Name="SiteName" />
                                <asp:Parameter Name="Description" />
                                <asp:Parameter Name="SiteID" />
                                <asp:Parameter Name="SiteShortName" />
                                <asp:Parameter Name="IsOnline" />
                            </UpdateParameters>
                            <DeleteParameters>
                                <asp:Parameter Name="SiteID" />
                            </DeleteParameters>
                        </asp:SqlDataSource>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
                <br />
            </td>
        </tr>
    </table>
</asp:Content>

