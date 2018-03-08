<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="PublishBlock.aspx.vb" Inherits="Publish_PublishBlock" ValidateRequest="false"  EnableEventValidation="false"%>
<%@ Register TagPrefix="FCKeditorV2" Namespace="FredCK.FCKeditorV2" Assembly="FredCK.FCKeditorV2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                <asp:View ID="View1" runat="server">
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 65%">
                                區塊編號：<asp:TextBox ID="txtBlockID" runat="server" MaxLength="10" ValidationGroup="5"></asp:TextBox>
                                區塊名稱：<asp:TextBox ID="txtBlockName" runat="server" ValidationGroup="5"></asp:TextBox>&nbsp;<asp:CheckBox
                                    ID="cb_html" runat="server" Text="HTML格式" />
                                <asp:Button ID="btnNewBlock" runat="server" Text="新增區塊" />
                                <asp:Label ID="lbMessage" runat="server" ForeColor="Red"></asp:Label>
                                <asp:RequiredFieldValidator ID="rfv1" runat="server" ControlToValidate="txtBlockID"
                                    Display="Dynamic" ErrorMessage="請輸入區塊編號！" ValidationGroup="5"></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="rfv2" runat="server" ControlToValidate="txtBlockName"
                                    Display="Dynamic" ErrorMessage="請輸入區塊名稱！" ValidationGroup="5"></asp:RequiredFieldValidator></td>
                            <td align="right" style="width: 35%">
                                區塊搜尋：<asp:TextBox ID="txt_Search" runat="server"></asp:TextBox>
                                <asp:Button ID="btn_Search" runat="server" CausesValidation="False" Text="搜尋" />
                                <asp:Button ID="btn_Clear" runat="server" CausesValidation="False" Text="清除" /></td>
                        </tr>
                    </table>
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="UID"
                        DataSourceID="sds_PublishBlock_List">
                        <Columns>
                            <asp:BoundField DataField="BlockID" HeaderText="區塊編號" SortExpression="BlockID" />
                            <asp:TemplateField HeaderText="區塊名稱" ShowHeader="False" SortExpression="BlockName">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox1" runat="server" Text=''></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Select"
                                        Text=''></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Html格式" SortExpression="Html">
                                <EditItemTemplate>
                                    <asp:CheckBox ID="CheckBox1" runat="server"  Text="HTML格式" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="CheckBox1" runat="server" Enabled="False"
                                        Text="HTML格式" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="UpdateTime" HeaderText="更新時間" ReadOnly="True" SortExpression="UpdateTime" />
                            <asp:TemplateField HeaderText="編輯" ShowHeader="False">
                                <EditItemTemplate>
                                    <asp:Button ID="Button1" runat="server" CausesValidation="True" CommandName="Update"
                                        Text="更新" />
                                    <asp:Button ID="Button2" runat="server" CausesValidation="False" CommandName="Cancel"
                                        Text="取消" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Button ID="Button2" runat="server" CausesValidation="False" CommandName="Edit"
                                        Text="編輯" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="刪除" ShowHeader="False">
                                <ItemTemplate>
                                    <asp:Button ID="Button3" runat="server" CausesValidation="False" CommandName="Delete"
                                        OnClientClick="return confirm('您確定要刪除此區塊上稿？')" Text="刪除" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:SqlDataSource ID="sds_PublishBlock_List" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                        DeleteCommand="DELETE FROM PublishBlock WHERE (UID = @UID)" InsertCommand="INSERT INTO PublishBlock(BlockID, BlockName, UpdateTime, UpdateUser, Html) VALUES (@BlockID, @BlockName, GETDATE(), @UserID, @Html)"
                        SelectCommand="SELECT BlockID, BlockName, UpdateTime, UpdateUser, Html, UID FROM PublishBlock WHERE (BlockID LIKE '%' + @Keyword + '%') OR (BlockName LIKE N'%' + @Keyword + N'%')"
                        UpdateCommand="UPDATE PublishBlock SET BlockID = @BlockID, BlockName = @BlockName, Html = @Html, UpdateTime = GETDATE(), UpdateUser = @UpdateUser WHERE (UID = @UID)">
                        <InsertParameters>
                            <asp:ControlParameter ControlID="txtBlockID" Name="BlockID" PropertyName="Text" />
                            <asp:ControlParameter ControlID="txtBlockName" Name="BlockName" PropertyName="Text" />
                            <asp:SessionParameter Name="UserID" SessionField="UserID" />
                            <asp:ControlParameter ControlID="cb_html" Name="Html" PropertyName="Checked" />
                        </InsertParameters>
                        <SelectParameters>
                            <asp:ControlParameter ControlID="txt_Search" DefaultValue="%" Name="Keyword" PropertyName="Text" />
                        </SelectParameters>
                        <DeleteParameters>
                            <asp:Parameter Name="UID" />
                        </DeleteParameters>
                        <UpdateParameters>
                            <asp:Parameter Name="BlockID" />
                            <asp:Parameter Name="BlockName" />
                            <asp:Parameter Name="Html" />
                            <asp:SessionParameter Name="UpdateUser" SessionField="UserID" />
                            <asp:Parameter Name="UID" />
                        </UpdateParameters>
                    </asp:SqlDataSource>
                </asp:View>
                <asp:View ID="View2" runat="server">
                    <asp:Label ID="lbBlockSubject" runat="server" Font-Size="Medium"></asp:Label><br />
                    <FCKeditorV2:FCKeditor ID="FCKeditor3" runat="server" Height="460px" ToolbarStartExpanded="true"
                        Width="100%">
                    </FCKeditorV2:FCKeditor>
                    <asp:SqlDataSource ID="sds_Content" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                        SelectCommand="SELECT Content, UpdateTime, BlockName, BlockID, Html FROM PublishBlock WHERE (UID = @UID)"
                        UpdateCommand="UPDATE PublishBlock SET Content = @Content, UpdateTime = GETDATE(), UpdateUser = @UserID WHERE (UID = @UID)">
                        <UpdateParameters>
                            <asp:Parameter Name="Content" />
                            <asp:SessionParameter Name="UserID" SessionField="UserID" />
                            <asp:ControlParameter ControlID="GridView1" Name="UID" PropertyName="SelectedValue" />
                        </UpdateParameters>
                        <SelectParameters>
                            <asp:ControlParameter ControlID="GridView1" Name="UID" PropertyName="SelectedValue" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                    <asp:Button ID="btnSubmit" runat="server" Text="確定更新" />&nbsp;
                    <asp:Button ID="btn_Cancel" runat="server" CausesValidation="False" OnClientClick="return confirm('您確定要取消編輯返回清單嗎？')"
                        Text="取消返回" /></asp:View>
                <asp:View ID="View3" runat="server">
                    <asp:Label ID="lbBlockSubject2" runat="server" Font-Size="Medium"></asp:Label><br />
                    <asp:TextBox ID="txt_Content" runat="server" Rows="12" TextMode="MultiLine" Width="100%"></asp:TextBox><br />
                    <asp:Button ID="btn_Submit2" runat="server" Text="確定更新" />&nbsp;
                    <asp:Button ID="btn_Cancel2" runat="server" CausesValidation="False" OnClientClick="return confirm('您確定要取消編輯返回清單嗎？')"
                        Text="取消返回" /></asp:View>
            </asp:MultiView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

