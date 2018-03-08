<%@ Page Language="VB" AutoEventWireup="false" CodeFile="InterChangeMapGroup.aspx.vb" Inherits="Fare_Calculator_InterChangeMapGroup" MasterPageFile="~/MasterPage.master"%>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table cellspacing="0" cellpadding="5" width="100%" border="0">
    <tr>
        <td>
            
                <asp:MultiView id="MultiView1" runat="server" ActiveViewIndex="0">
                <asp:View id="View1" runat="server">
                   <div class="SearchArea">
                        新增地圖群組：<asp:Button ID="btnCreate" runat="server" Text="新增" />
                        資料搜尋：
                        <asp:TextBox ID="txtSearch" runat="server" CssClass="form_txt" Columns="20"></asp:TextBox>&nbsp;<asp:Button
                            ID="btnSearch" runat="server" Text="搜尋" CssClass="Btn"></asp:Button>&nbsp;<asp:Button
                                ID="btnShowAll" runat="server" Text="清除" CssClass="Btn"></asp:Button>&nbsp;<asp:Label
                                    ID="lblMessage" runat="server" EnableViewState="False" ForeColor="Red"></asp:Label>
                   </div>
                    <br />
                    <asp:GridView ID="GridView1" runat="server" CssClass="Dg" AutoGenerateColumns="False"
                        DataSourceID="sdsInterChangeFareProject" DataKeyNames="UID" AllowPaging="True"
                        AllowSorting="True" PageSize="20">
                        <Columns>
                            <asp:BoundField ReadOnly="true" InsertVisible ="false" DataField="UID" SortExpression="UID"
                                HeaderText="群組序號"></asp:BoundField>
                            <asp:BoundField DataField="GroupName" SortExpression="GroupName"
                                HeaderText="群組名稱"></asp:BoundField>
                            <asp:BoundField DataField="ItemOrder" SortExpression="ItemOrder"
                                HeaderText="排序"></asp:BoundField>
                            <asp:CheckBoxField DataField = "IsOnline" SortExpression = "IsOnline" HeaderText = "啟用" />
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:Button ID="BtnEdit" runat="server" Text="編輯" CausesValidation="False" CommandName="EditProject" >
                                    </asp:Button>&nbsp;<asp:Button ID="BtnDel" runat="server" Text="刪除" CausesValidation="False"
                                        CommandName="Delete" OnClientClick="return confirm('您確定要刪除這筆資料？資料刪除無法回復！');"></asp:Button>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>

            </asp:View>
            <asp:View ID="View2" runat="server">
                        <table>
                            <tr>
                                <td>
                                    *群組名稱：</td>
                                <td>
                                    <asp:TextBox ID="txtCName" runat="server" Text="" ValidationGroup="2"></asp:TextBox></td>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCName"
                                        ErrorMessage="請輸入群組名稱！" ValidationGroup="2"></asp:RequiredFieldValidator></td>
                            </tr>
                            <tr>
                                <td>
                                    排序：</td>
                                <td>
                                    <asp:TextBox ID="txtItemOrder" runat="server" Text="1" ValidationGroup="2"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtItemOrder" ValidationExpression="\d">請輸入數字</asp:RegularExpressionValidator>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtCName"
                                        ErrorMessage="請輸入排序！" ValidationGroup="2"></asp:RequiredFieldValidator>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>
                                    是否上線：</td>
                                <td>
                                    <asp:CheckBox ID="cbIsOnline" runat="server" Text="上線" Checked = "true" ValidationGroup="2"/></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>
                                    <br />
                                    <asp:Button ID="btnInsert" runat="server" Text="新增資料" ValidationGroup="2" />&nbsp;
                                    <asp:Button ID="btnUpdate" runat="server" Text="更新資料" ValidationGroup="2" />&nbsp;
                                    <asp:Button ID="btnCancel" runat="server" Text="取消返回" ValidationGroup="3" />&nbsp;
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                        </table>
            </asp:View> 
            </asp:MultiView> 

            <br />
         </td>
    </tr>
</table>
                    <asp:SqlDataSource ID="sdsFareList" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                        SelectCommand="SELECT UID, GroupName, ItemOrder, IsOnline from InterChangeMapGroup order by UID">
                        <SelectParameters>
                        </SelectParameters>
                    </asp:SqlDataSource>

                     <asp:SqlDataSource ID="sdsInterChangeFareProject" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                        DeleteCommand="DELETE FROM InterChangeMapGroup WHERE [UID] = @UID"
                        InsertCommand="Insert into InterChangeMapGroup (GroupName, ItemOrder, IsOnline, UpdateDateTime) values(@GroupName, @ItemOrder, @IsOnline, getdate())" InsertCommandType ="Text" 
                        UpdateCommand="Update InterChangeMapGroup set GroupName = @GroupName, ItemOrder = @ItemOrder, IsOnline = @IsOnline, UpdateDateTime = getdate() where UID = @UID" UpdateCommandType="Text" 
                        SelectCommand="SELECT UID, GroupName, ItemOrder, IsOnline from InterChangeMapGroup order by UID" SelectCommandType="Text">
                        <DeleteParameters>
                            <asp:Parameter Type="Int32" Name="UID"></asp:Parameter>
                        </DeleteParameters>
                        <InsertParameters>
                            <asp:ControlParameter Name="GroupName" ControlID="txtCName" PropertyName="Text" />
                            <asp:ControlParameter Name="ItemOrder" ControlID="txtItemOrder" PropertyName="Text" />
                            <asp:ControlParameter Name="IsOnline" ControlID="cbIsOnline" PropertyName="Checked" />
                        </InsertParameters>
                        <UpdateParameters>
                             <asp:Parameter Name="UID"  Type="Int32" />
                            <asp:ControlParameter Name="GroupName" ControlID="txtCName" PropertyName="Text" />
                            <asp:ControlParameter Name="ItemOrder" ControlID="txtItemOrder" PropertyName="Text" />
                            <asp:ControlParameter Name="IsOnline" ControlID="cbIsOnline" PropertyName="Checked" />
                        </UpdateParameters>
                        <SelectParameters>
                        </SelectParameters>
                    </asp:SqlDataSource>

</asp:Content> 