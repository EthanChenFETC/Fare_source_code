<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="SiteMapGroupSetting.aspx.vb" Inherits="SystemDesign_SiteMapGroupSetting" title="Untitled Page" %>
<%@ Register Src="../common/SiteTree.ascx" TagName="SiteTree" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width="100%">
    <tr>
        <td width="400" valign="top">
            &nbsp;<asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>
            <asp:Button ID="btnUpdate" runat="server" Text="確定更新" Height="40px" />
            <asp:Label ID="lbMessage" runat="server" ForeColor="Red"></asp:Label>
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
        <td width="75%" valign="top">
            </td>
    </tr>
    <tr>
        <td style="height: 21px" width="400" valign="top">
            <br />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
            <uc1:SiteTree ID="SiteTree1" runat="server" TreeTitle="勾選適用群組" TreeViewHeight="500"
                TreeViewWidth="280" IsShowCheckbox="true" IsNeedValidatorCheck="false" IsShowAllCheckbox="true"  />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="GridView1" EventName="SelectedIndexChanged" />
                </Triggers>
            </asp:UpdatePanel>
        </td>
        <td style="height: 21px" width="75%" valign="top">
            <asp:SqlDataSource ID="SDS_Sites_List" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                SelectCommand="SELECT SiteID, SiteName, (SELECT COUNT(GroupID) AS Expr1 FROM SiteMap_Group WHERE (SiteID = Sites.SiteID)) AS CheckCount FROM Sites where IsOnline > 0 "></asp:SqlDataSource>
<!--            <asp:SqlDataSource ID="sds_Group_List" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                ProviderName="<%$ ConnectionStrings:AdminModuleConnectionString.ProviderName %>"
                SelectCommand="SELECT GroupID, GroupName, SiteMapGroupID FROM SiteMapGroupCatgry where SiteID = @SiteID and IsOnline=1">
                <SelectParameters>
                    <asp:Parameter Name="SiteID" Type="Int32" />
                </SelectParameters>
            </asp:SqlDataSource>
-->
            <asp:RadioButtonList  ID="GroupIDSelected" runat="server" Visible="false">
                <asp:ListItem id="GroupID" text="GroupID" Value="0" Selected="true"></asp:ListItem>
            </asp:RadioButtonList>
            <br />
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="SiteID"
                DataSourceID="SDS_Sites_List" Width="100%">
                <Columns>
                    <asp:CommandField ButtonType="Button" ShowSelectButton="True" />
                    <asp:BoundField DataField="SiteID" HeaderText="網站編號" InsertVisible="False" ReadOnly="True"
                        SortExpression="SiteID" />
                    <asp:BoundField DataField="SiteName" HeaderText="網站名稱" SortExpression="SiteName" />
                    <asp:TemplateField HeaderText="網站版位" SortExpression="SiteID">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlGroup_List" runat="server" 
                                DataTextField="GroupName" DataValueField="GroupID" >
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="CheckCount" HeaderText="已設定選單數" SortExpression="CheckCount" />
                </Columns>
            </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnUpdate" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
            <asp:SqlDataSource ID="sds_SiteMap_Group_Insert" runat="server" 
            ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>" 
            ProviderName="<%$ ConnectionStrings:AdminModuleConnectionString.ProviderName %>" 
            DeleteCommand="DELETE FROM SiteMap_Group WHERE (GroupID = @GroupID) AND (SiteID = @SiteID)" 
            InsertCommand="INSERT INTO SiteMap_Group(NodeID, GroupID, SiteID) VALUES (@NodeID, @GroupID, @SiteID)" 
            SelectCommand="SELECT NodeID, GroupID, SiteID FROM SiteMap_Group WHERE (SiteID = @SiteID) AND (GroupID = @GroupID)">
                <DeleteParameters>
                    <asp:ControlParameter ControlID="GroupIDSelected" Name="GroupID" PropertyName="SelectedValue" />
                    <asp:ControlParameter ControlID="GridView1" Name="SiteID" PropertyName="SelectedValue" />
                </DeleteParameters>
                <SelectParameters>
                    <asp:ControlParameter ControlID="GridView1" Name="SiteID" PropertyName="SelectedValue" />
                    <asp:ControlParameter ControlID="GroupIDSelected" Name="GroupID" PropertyName="SelectedValue" />
                </SelectParameters>
                <InsertParameters>
                    <asp:Parameter Name="NodeID" />
                    <asp:Parameter Name="GroupID" />
                    <asp:Parameter Name="SiteID" />
                </InsertParameters>
            </asp:SqlDataSource>
            &nbsp;<br />
        </td>
    </tr>
    <tr>
        <td width="400" valign="top">
        </td>
        <td width="75%" valign="top">
        </td>
    </tr>
</table>

</asp:Content>

