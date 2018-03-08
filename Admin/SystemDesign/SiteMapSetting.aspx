<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="SiteMapSetting.aspx.vb" Inherits="SystemDesign_SiteMapSetting" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                <tr>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:SqlDataSource ID="SqlDataSource_PublishType" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                    SelectCommand="SELECT [PublishTypeID], [PublishTypeName] FROM [PublishTypeSitemap]">
                                </asp:SqlDataSource>
                                <asp:SqlDataSource ID="SqlDataSource_AdminMenu" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                    SelectCommand="SELECT SiteMap.NodeId, SiteMap.ParentNodeId, SiteMap.Text, SiteMap.RefPath, SiteMap.PublishType, SiteMap.NodeLevel, SiteMap.ImageUrl, SiteMap.HaveChildNode, SiteMap.Target, PublishTypeSitemap.PublishTypeName, SiteMap.TextEn, SiteMap.NodeKeyword FROM SiteMap INNER JOIN PublishTypeSitemap ON SiteMap.PublishType = PublishTypeSitemap.PublishTypeID WHERE (SiteMap.SiteID = @SiteID) ORDER BY SiteMap.AllNodeOrder"
                                    UpdateCommand="UPDATE SiteMap SET Text = @Text, RefPath = @RefPath, PublishType = @PublishType, Target = @Target, TextEn = @TextEn, NodeKeyword = @NodeKeyword WHERE (NodeId = @NodeID)">
                                    <UpdateParameters>
                                        <asp:Parameter Name="Text" />
                                        <asp:Parameter Name="RefPath" />
                                        <asp:Parameter Name="PublishType" />
                                        <asp:Parameter Name="Target" />
                                        <asp:Parameter Name="TextEn" />
                                        <asp:Parameter Name="NodeID" Type="Int32" />
                                        <asp:Parameter Name="NodeKeyword" />
                                    </UpdateParameters>
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="DropDownList1" Name="SiteID" PropertyName="SelectedValue" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                                <asp:SqlDataSource ID="sdsSites_List" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                    ProviderName="<%$ ConnectionStrings:AdminModuleConnectionString.ProviderName %>"
                                    SelectCommand="SELECT SiteID, SiteName FROM Sites where IsOnline > 0 and SiteID > 0 "></asp:SqlDataSource>
                                <asp:DropDownList ID="DropDownList1" runat="server" AppendDataBoundItems="True" DataSourceID="sdsSites_List"
                                    DataTextField="SiteName" DataValueField="SiteID">
                                    <asp:ListItem Value="0">請選擇網站</asp:ListItem>
                                </asp:DropDownList><asp:Button ID="btnSelectSite" runat="server" Text="選擇網站" /><asp:GridView
                                    ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="Dg" DataKeyNames="NodeId"
                                    DataSourceID="SqlDataSource_AdminMenu" PageSize="15">
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
                                                <asp:Label ID="Label1" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="TextEn" HeaderText="單元附註說明" SortExpression="TextEn" />
                                        
                                        <asp:TemplateField HeaderText="上稿種類" SortExpression="PublishType">
                                            <EditItemTemplate>
                                                <asp:RadioButtonList ID="rblPublishType" runat="server" DataSourceID="SqlDataSource_PublishType"
                                                    DataTextField="PublishTypeName" DataValueField="PublishTypeID" RepeatDirection="Horizontal"
                                                    RepeatLayout="Flow" >
                                                </asp:RadioButtonList>&nbsp;
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label2" runat="server" ></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="前台路徑" SortExpression="RefPath">
                                            <EditItemTemplate>
                                                URL:<asp:TextBox ID="txtURL" runat="server" Width="300px"></asp:TextBox>
                                                視窗：<asp:RadioButtonList ID="rbltxtURL" runat="server" RepeatDirection="Horizontal"
                                                    RepeatLayout="Flow">
                                                    <asp:ListItem Value="_parent">原視窗</asp:ListItem>
                                                    <asp:ListItem Value="_blank">新視窗</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label3" runat="server" ></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="NodeKeyword" HeaderText="特殊連結關鍵字" SortExpression="NodeKeyword" />
                                       
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
                        &nbsp; &nbsp; &nbsp;&nbsp;<br />
                        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" Visible="False" /></td>
                </tr>
            </table>
</asp:Content>

