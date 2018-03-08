<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="AdminMenuSetting.aspx.vb" Inherits="SystemManager_AdminMenuSetting" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table cellspacing="0" cellpadding="5" width="100%" border="0">
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                       
                        <asp:GridView ID="GridView1" runat="server" CssClass="Dg" PageSize="15" DataKeyNames="NodeId" AutoGenerateColumns="False" DataSourceID="SqlDataSource_AdminMenu">
                            <Columns>
                                <asp:CommandField ButtonType="Button" ShowEditButton="True" HeaderText="資料編輯" >
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
                                <asp:CheckBoxField DataField="SuperAdmin" HeaderText="系統管理員單元" SortExpression="SuperAdmin" />
                                <asp:TemplateField HeaderText="標籤顯示" SortExpression="TabType" >
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlTabType" runat="server" >
                                            <asp:ListItem Text="預設" Value="0" Selected="True" ></asp:ListItem>
                                            <asp:ListItem Text="選取" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="禁用" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="隱藏" Value="3"></asp:ListItem>
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblTabType" runat="server" ></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="NavigateUrl" HeaderText="單元模組路徑" SortExpression="NavigateUrl" />
                                <asp:TemplateField HeaderText="上稿種類" SortExpression="PublishType">
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="SqlDataSource_PublishType"
                                            DataTextField="PublishTypeName" DataValueField="PublishTypeID">
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label2" runat="server"></asp:Label>
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
                        
                       
                        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" OldValuesParameterFormatString="original_{0}"
                            SelectMethod="GetDataTable" TypeName="AdminMenuDataSetTableAdapters.AdminMenuTableAdapter"
                            UpdateMethod="UpdateQuery">
                            <UpdateParameters>
                                <asp:Parameter Name="Text" Type="String" />
                                <asp:Parameter Name="SuperAdmin" Type="Boolean" />
                                <asp:Parameter Name="NavigateUrl" Type="String" />
                                <asp:Parameter Name="PublishType" Type="Int32" />
                                <asp:Parameter Name="Original_NodeID" Type="Int32" />
                                
                            </UpdateParameters>
                        </asp:ObjectDataSource>
                        <asp:SqlDataSource ID="SqlDataSource_AdminMenu" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                            SelectCommand="SELECT AdminMenu.NodeId, AdminMenu.ParentNodeId, AdminMenu.Text, AdminMenu.SuperAdmin, AdminMenu.NavigateUrl, AdminMenu.PublishType, AdminMenu.NodeLevel, PublishType.PublishTypeName, AdminMenu.ImageUrl, AdminMenu.TabType, Case AdminMenu.TabType when null then '預設' when 0 then '預設' when 1 then '選擇' when 2 then '禁用' when 3 then '隱藏' end as TabTypeName FROM AdminMenu INNER JOIN PublishType ON AdminMenu.PublishType = PublishType.PublishTypeID where NodeID < 1000 ORDER BY AdminMenu.AllNodeOrder"
                            UpdateCommand="UPDATE AdminMenu SET Text = @Text, SuperAdmin = @SuperAdmin, NavigateUrl = @NavigateUrl, PublishType = @PublishType, TabType = @TabType WHERE (NodeId = @NodeID)">
                            <UpdateParameters>
                                <asp:Parameter Name="Text" />
                                <asp:Parameter Name="SuperAdmin" />
                                <asp:Parameter Name="NavigateUrl" />
                                <asp:Parameter Name="PublishType" />
                                <asp:Parameter Name="NodeID" Type="Int32" />
                                <asp:Parameter Name="TabType" Type="Int32" />
                            </UpdateParameters>
                        </asp:SqlDataSource>
                        <asp:SqlDataSource ID="SqlDataSource_PublishType" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                            SelectCommand="SELECT [PublishTypeID], [PublishTypeName] FROM [PublishType]"></asp:SqlDataSource>
                        
                        
                    </ContentTemplate>
                    
                </asp:UpdatePanel>
                
                
                
                <br />
                &nbsp;
            </td>
        </tr>
    </table>
</asp:Content>
