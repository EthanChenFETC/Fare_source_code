<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="AdminMenuBuilder.aspx.vb" Inherits="SystemDesign_AdminMenuBuilder"
    Title="Untitled Page" %>

<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table cellspacing="0" cellpadding="5" width="100%" border="0">
        <tr>
            <td id="tr_Edit" runat="server">
                <table class="Dg" id="Table1" cellspacing="1" cellpadding="5" border="0">
                    <tr class="DgHeader">
                        <td valign="top">
                            網站選單管理</td>
                        <td>
                            操作說明</td>
                    </tr>
                    <tr>
                        <td valign="top">
                            &nbsp;
                            <asp:DropDownList ID="ddlSites" runat="server" AppendDataBoundItems="True" AutoPostBack="True">
                                <asp:ListItem Value="0">請選擇網站</asp:ListItem>
                                <asp:ListItem Value="1" Selected ="True" >後端管理系統</asp:ListItem>
                                
                            </asp:DropDownList><asp:RadioButtonList ID="RadioButtonList1" runat="server" AutoPostBack="True" RepeatDirection="Horizontal">
                                <asp:ListItem Value="1" Selected="True">移動與更名模式</asp:ListItem>
                                <asp:ListItem Value="2">新增項目模式</asp:ListItem>
                                <asp:ListItem Value="3">刪除項目模式</asp:ListItem>
                            </asp:RadioButtonList>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <ComponentArt:TreeView ID="TreeView1" runat="server" AutoPostBackOnSelect="true"
                                        CssClass="TreeView" EnableViewState="true" AutoPostBackOnNodeMove="false" Height="580px"
                                        Width="320" DragAndDropEnabled="false" NodeEditingEnabled="false" NodeCssClass="TreeNode"
                                        SelectedNodeCssClass="SelectedTreeNode" HoverNodeCssClass="HoverTreeNode" NodeEditCssClass="NodeEdit"
                                        LineImageWidth="19" LineImageHeight="20" ItemSpacing="0" NodeLabelPadding="3"
                                        ShowLines="true" LineImagesFolderUrl="~/common/imagesTree/lines/" DropSiblingEnabled="True"
                                        ImagesBaseUrl="~/common/imagesTree/">
                                    </ComponentArt:TreeView>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="RadioButtonList1" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="btn_clearreload" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="BtnVirturl" EventName="ServerClick" />
                                    <asp:AsyncPostBackTrigger ControlID="ddlSites" EventName="SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                        <td valign="top">
                            <table class="Fn" id="Table_Fn" cellspacing="1" cellpadding="5" border="0">
                                <tr>
                                    <td style="width: 297px">
                                        <p>
                                            <input onclick="javascript:startTheLoop();" type="button" value="儲存變更" class="Btn"
                                                id="Button1" />&nbsp;
                                            <asp:Button ID="btn_clearreload" runat="server" Text="重新讀取" CssClass="Btn"></asp:Button>
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                <ContentTemplate>
                                            <asp:Label ID="lblMessage_Add" runat="server" EnableViewState="False" ForeColor="Red"></asp:Label>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            &nbsp;
                                        </p>
                                    </td>
                                </tr>
                            </table>
                            <p>
                                本管理單元為視覺化工具，提供前台選單結構管理，可以新增選單項目、修改選單名稱、調整選單順序以及刪除選單等功能 ，任何動作之後必需按下儲存變更才會儲存到資料庫，前台的選單會即時的作變更。</p>
                            <p>
                                操作前請詳讀以下說明：</p>
                            <ol>
                                <li>模式：樹狀選單上方核選鈕，可變換3種操作模式：移動與更名模式 新增項目模式 刪除項目模式</li>
                                <li>移動與更名模式：此為預設模式，允許移動與更名選單項目
                                    <ul>
                                        <li>移動：以拖移放式移動個別選項順序，您可以把個別選項移動到任何位置，系統會記錄下最後的順位。</li>
                                        <li>更名：以滑鼠左鍵點選項目，變換為編輯模式即可輸入文字，在樹狀選單空白處按一下左鍵，或按下鍵盤上的Enter鍵，即可完成更名。</li>
                                    </ul>
                                </li>
                                <li>新增項目模式：切換至此模式後，在您需要增加選單項目的同一層級任選一個項目，即可複製它的所有屬性，然後您可再切換回更名模式進行名稱變更。</li>
                                <li>刪除項目模式 ： 切換至此模式後，點選任何一個選單項目，該項目會被刪除。</li>
                                <li>儲存變更：以上各模式操作完成後，或您可以在作了任何小變更後，按下此鍵即可將所以變更的狀態儲存回資料庫，注意，這時候也會同時刪除您方才刪除的項目。</li>
                                <li>重新讀取：萬一您操作失誤，想要重新操作，在尚未按下儲存變更前，可以選擇此鍵將使選單重新由資料庫讀取出來，以重新操作。</li>
                                <li>如果系統使用的選單結構較大，您操作後的儲存、或點選展開按鈕需要稍作等待。</li>
                            </ol>
                            <p>
                                <input id="BtnVirturl" type="button" value="button-onserverclick" onserverclick="BtnVirturl_Click"
                                    runat="server" style="visibility: hidden" />
                            </p>
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
