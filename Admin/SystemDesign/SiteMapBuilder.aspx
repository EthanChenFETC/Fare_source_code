<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="SiteMapBuilder.aspx.vb" Inherits="SystemDesign_SiteMapBuilder" Title="Untitled Page" %>

<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table cellspacing="0" cellpadding="5" width="100%" border="0">
        <tr>
            <td id="tr_Edit" runat="server">
                <table class="Dg" id="Table1" cellspacing="1" cellpadding="5" border="0">
                    <tr class="DgHeader">
                        <td>
                            網站選單管理</td>
                        <td>
                            操作說明</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DropDownList ID="ddlSites" runat="server" AppendDataBoundItems="True" AutoPostBack="True"
                                DataSourceID="SDS_Sites_List" DataTextField="SiteName" DataValueField="SiteID">
                                <asp:ListItem Value="0">請選擇網站</asp:ListItem>
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SDS_Sites_List" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                SelectCommand="SELECT SiteID, SiteName FROM Sites WHERE (SiteID > 0)"></asp:SqlDataSource>
                                    <asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatDirection="Horizontal"
                                        AutoPostBack="True">
                                        <asp:ListItem Value="1" Selected="True">移動與更名模式</asp:ListItem>
                                        <asp:ListItem Value="2">新增項目模式</asp:ListItem>
                                        <asp:ListItem Value="3">刪除項目模式</asp:ListItem>
                                    </asp:RadioButtonList>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <ComponentArt:TreeView ID="TreeView1" runat="server" AutoPostBackOnSelect="true"
                                        CssClass="TreeView" EnableViewState="true" ImagesBaseUrl="~/common/imagesTree/"
                                        DropSiblingEnabled="True" LineImagesFolderUrl="~/common/imagesTree/lines/" ShowLines="true"
                                        NodeLabelPadding="3" ItemSpacing="0" LineImageHeight="20" LineImageWidth="19"
                                        NodeEditCssClass="NodeEdit" HoverNodeCssClass="HoverTreeNode" SelectedNodeCssClass="SelectedTreeNode"
                                        NodeCssClass="TreeNode" NodeEditingEnabled="false" DragAndDropEnabled="false"
                                        Width="320" Height="580px" AutoPostBackOnNodeMove="false">
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
                                            <asp:Button ID="btn_clearreload" runat="server" Text="重新讀取" CssClass="Btn"></asp:Button>&nbsp;
                                            <asp:Label ID="lblMessage_Add" runat="server" EnableViewState="False" ForeColor="Red"></asp:Label></p>
                                            <asp:Literal ID="Err" runat="server"></asp:Literal>
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
                                <li><strong>中文名稱：如果您使用中文名稱當成選項名稱，請留意在變更好名稱之後先按下儲存鈕，您才能再切換到新增或刪除模式，否則文字會呈現亂碼。 (2006_01已改善本問題)</strong></li>
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
