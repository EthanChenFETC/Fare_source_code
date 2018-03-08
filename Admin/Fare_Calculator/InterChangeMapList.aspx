<%@ Page Language="VB" AutoEventWireup="false" CodeFile="InterChangeMapList.aspx.vb" Inherits="Fare_Calculator_InterChangeProhibitRoute" MasterPageFile="~/MasterPage.master"%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table cellspacing="0" cellpadding="5" width="100%" border="0">
    <tr>
        <td>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                <asp:MultiView id="MultiView1" runat="server" ActiveViewIndex="0">
                <asp:View id="View1" runat="server">
                  
                    <asp:GridView ID="GridView1" runat="server" CssClass="Dg" AutoGenerateColumns="False"
                        DataSourceID="sdsInterChangeRouteList" DataKeyNames="UID" AllowPaging="True"
                        AllowSorting="True" PageSize="12">
                        <Columns>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:Button ID="BtnEdit" runat="server" Text="編輯" CausesValidation="False" CommandName="EditProject">
                                    </asp:Button>&nbsp;<asp:Button ID="BtnDel" runat="server" Text="刪除" CausesValidation="False"
                                        CommandName="DeleteGroup" OnClientClick="return confirm('您確定要刪除這筆資料？資料刪除無法回復！');"></asp:Button>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField ReadOnly="true" InsertVisible ="false" DataField="GroupName" SortExpression="UID"
                                HeaderText="地圖群組名稱"></asp:BoundField>
                            <asp:TemplateField SortExpression="ICName" HeaderText="地圖群組交流道">
                                <ItemTemplate>
                                    <asp:Literal ID="ltlICName" runat="server" ></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:CheckBoxField ReadOnly="true" DataField="IsOnline"  SortExpression="IsOnline"
                                HeaderText="啟用"></asp:CheckBoxField>
                        </Columns>
                    </asp:GridView>

            </asp:View>
            <asp:View ID="View2" runat="server">
                        <table>
                            <tr>
                                <td style="width:100px">
                                    *交流道清單：</td>
                                <td>
                                    <asp:Label ID="lblRouteList" runat="server" ForeColor="Red"></asp:Label>
                                    <asp:HiddenField ID="hidRouteList" runat="server" />
                                    &nbsp;&nbsp;&nbsp;<asp:Button ID="btnClear" runat="server" Text="清除路徑" CssClass="Btn" />
                                </td><td></td>
                            </tr>
                            <tr>
                                <td style="width:100px">
                                    *路徑選擇：</td>
                                <td>
                                    <asp:DataList ID="dlICList" runat="server" DataSourceID="sdsHWList" RepeatColumns="1" RepeatLayout="table" DataKeyField="UID" AlternatingItemStyle-BackColor="WhiteSmoke" BorderColor="YellowGreen"  >
                                        <ItemStyle VerticalAlign="Top" />
                                        <ItemTemplate>
                                            <tr><td valign="top" >
                                            <asp:Label id="lblHWName" runat="server" ></asp:Label>：
                                                </td><td>
                                                    <asp:Repeater ID="rptRouteList" runat="server" OnItemCommand ="Repeater_RowCommand" OnItemDataBound ="Repeater_ItemDataBound" >
                                                        <ItemTemplate>
                                                            <asp:Button ID="btnIC" runat="server" CommandName="ICSelect" CssClass="BtnProhibit" />
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                            </td></tr>
                                            <asp:SqlDataSource ID="sdsICListdl" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                                SelectCommand="SELECT i.UID, i.ICName, h.HWName from InterChangeList i inner join HWList h on h.UID = i.HWUID where (@HWUID = 0 or HWUID=@HWUID) and i.isonline > 0 order by h.itemorder, i.itemorder">
                                                <SelectParameters>
                                                    <asp:Parameter Name="HWUID" DefaultValue="0"/>
                                                </SelectParameters>
                                                </asp:SqlDataSource>
                                        </ItemTemplate>
                                    </asp:DataList>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <br />
                                    
                                    <asp:Button ID="btnUpdate" runat="server" Text="更新資料" ValidationGroup="2" />&nbsp;
                                    <asp:Button ID="btnCancel" runat="server" Text="取消返回" ValidationGroup="3" />&nbsp;
                                </td>
                               
                            </tr>
                        </table>
            </asp:View> 
            </asp:MultiView> 
                </ContentTemplate>
            </asp:UpdatePanel>
            <br />
         </td>
    </tr>
</table>
                    <asp:SqlDataSource ID="sdsProjectList" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                        SelectCommand="SELECT UID, ProjectName from InterChangeFareProject order by itemorder">
                        <SelectParameters>
                        </SelectParameters>
                    </asp:SqlDataSource>
                    <asp:SqlDataSource ID="sdsHWList" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                        SelectCommand="SELECT UID, HWName from HWList where isonline > 0 order by itemorder"></asp:SqlDataSource>
                    <asp:SqlDataSource ID="sdsICList1" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                        SelectCommand="SELECT i.UID, i.ICName from InterChangeList i inner join HWList h on h.UID = i.HWUID where (@HWUID = 0 or HWUID=@HWUID) and i.isonline > 0 order by h.itemorder, i.itemorder">
                        <SelectParameters>
                            <asp:Parameter Name="HWUID" DefaultValue="0"/>
                        </SelectParameters>
                        </asp:SqlDataSource>
                     <asp:SqlDataSource ID="sdsInterChangeRouteList" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                        DeleteCommand="DELETE FROM InterChangeMapGroupRelation WHERE [GroupUID] = @UID"
                        InsertCommand="Insert into InterChangeMapGroupRelation (GroupUID, ICUID) values(@GroupUID, @ICUID)" InsertCommandType ="Text" 
                        UpdateCommand="Update InterChangeMapGroupRelation set GroupUID = @GroupUID, ICUID = @ICUID where UID = @UID" UpdateCommandType="Text" 
                        SelectCommand="Select UID, GroupName , ItemOrder, IsOnline from InterChangeMapGroup order by UID " SelectCommandType="Text">
                        
                        <DeleteParameters>
                            <asp:Parameter Type="Int32" Name="UID"></asp:Parameter>
                        </DeleteParameters>
                        <InsertParameters>
                            <asp:Parameter Name="GroupUID" DefaultValue ="1" Type="Int32" />
                            <asp:Parameter Name="ICUID" DefaultValue ="1"/>
                        </InsertParameters>
                        <UpdateParameters>
                             <asp:Parameter Name="UID"  Type="Int32" />
                            <asp:Parameter Name="GroupUID" DefaultValue ="1" Type="Int32" />
                            <asp:Parameter Name="ICUID" DefaultValue ="1"/>
                        </UpdateParameters>
                        <SelectParameters>
                        </SelectParameters>
                    </asp:SqlDataSource>

</asp:Content> 