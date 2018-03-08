<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="GroupAdminPermission.aspx.vb" Inherits="SystemManager_GroupAdminPermission"
    Title="Untitled Page" %>

<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table cellspacing="0" cellpadding="5" width="100%" border="0">
        <tr>
            <td id="tr_Edit" runat="server">
                <table class="Dg" id="Table1" cellspacing="1" cellpadding="5" border="0">
                    <tr class="DgHeader">
                        <td>
                            後台選單</td>
                        <td>
                            前台選單</td>
                        <td>
                            操作說明</td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <ComponentArt:TreeView ID="TreeView1" runat="server" CssClass="TreeView" EnableViewState="true"
                                        AutoPostBackOnNodeMove="false" Height="400px" Width="400px" DragAndDropEnabled="false"
                                        NodeEditingEnabled="false" NodeCssClass="TreeNode" SelectedNodeCssClass="SelectedTreeNode"
                                        HoverNodeCssClass="HoverTreeNode" NodeEditCssClass="NodeEdit" LineImageWidth="19"
                                        LineImageHeight="20" ItemSpacing="0" NodeLabelPadding="3" ShowLines="true" LineImagesFolderUrl="~/common/imagesTree/lines/"
                                        DropSiblingEnabled="True" ImagesBaseUrl="~/common/imagesTree/" __designer:wfdid="w90">
                                    </ComponentArt:TreeView>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btn_clearreload" EventName="Click"></asp:AsyncPostBackTrigger>
                                    <asp:AsyncPostBackTrigger ControlID="btn_Update" EventName="Click"></asp:AsyncPostBackTrigger>
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                        <td valign="top">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <ComponentArt:TreeView ID="TreeView2" runat="server" CssClass="TreeView" EnableViewState="true"
                                        AutoPostBackOnNodeMove="false" Height="400px" Width="400px" DragAndDropEnabled="false"
                                        NodeEditingEnabled="false" NodeCssClass="TreeNode" SelectedNodeCssClass="SelectedTreeNode"
                                        HoverNodeCssClass="HoverTreeNode" NodeEditCssClass="NodeEdit" LineImageWidth="19"
                                        LineImageHeight="20" ItemSpacing="0" NodeLabelPadding="3" ShowLines="true" LineImagesFolderUrl="~/common/imagesTree/lines/"
                                        DropSiblingEnabled="True" ImagesBaseUrl="~/common/imagesTree/" __designer:wfdid="w89">
                                    </ComponentArt:TreeView>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btn_clearreload" EventName="Click"></asp:AsyncPostBackTrigger>
                                    <asp:AsyncPostBackTrigger ControlID="btn_Update" EventName="Click"></asp:AsyncPostBackTrigger>
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                        <td valign="top">
                            <table class="Fn" id="Table_Fn" cellspacing="1" cellpadding="5" border="0">
                                <tr>
                                    <td style="width: 297px">
                                        <p>
                                            <asp:Button ID="btn_clearreload" runat="server" Text="重新讀取" CssClass="Btn"></asp:Button>
                                            &nbsp;
                                            <asp:Button ID="btn_Update" runat="server" Text="儲存權限資料" CssClass="Btn" />
                                            &nbsp;<asp:Label ID="lblMessage" runat="server" EnableViewState="False" ForeColor="Red"></asp:Label></p>
                                        <p>
                                            目前設定的群組為：<asp:Label ID="LabelSettingGroupName" runat="server"></asp:Label></p>
                                    </td>
                                </tr>
                            </table>
                            <p>
                                操作前請詳讀以下說明：</p>
                            <ol>
                                <li>請勾選/取消單元來設定本群組所擁有的權限。</li>
                                <li>當儲存權限資料後，原有的設定會被覆蓋。</li>
                                <li>出現的單元數量，是所您所屬群組擁有的單元權限。(沒有權限的單元不會出現)</li>
                            </ol>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
    </table>
</asp:Content>
