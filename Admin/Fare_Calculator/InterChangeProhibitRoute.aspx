<%@ Page Language="VB" AutoEventWireup="false" CodeFile="InterChangeProhibitRoute.aspx.vb" Inherits="Fare_Calculator_InterChangeProhibitRoute" MasterPageFile="~/MasterPage.master"%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table cellspacing="0" cellpadding="5" width="100%" border="0">
    <tr>
        <td>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                <asp:MultiView id="MultiView1" runat="server" ActiveViewIndex="0">
                <asp:View id="View1" runat="server">
                   <div class="SearchArea">
                        新增路徑：<asp:Button ID="btnCreate" runat="server" Text="新增" />
                        資料搜尋：
                        <asp:TextBox ID="txtSearch" runat="server" CssClass="form_txt" Columns="20"></asp:TextBox>&nbsp;<asp:Button
                            ID="btnSearch" runat="server" Text="搜尋" CssClass="Btn"></asp:Button>&nbsp;<asp:Button
                                ID="btnShowAll" runat="server" Text="清除" CssClass="Btn"></asp:Button>&nbsp;<asp:Label
                                    ID="lblMessage" runat="server" EnableViewState="False" ForeColor="Red"></asp:Label>
                   </div>
                    <br />
                    <asp:GridView ID="GridView1" runat="server" CssClass="Dg" AutoGenerateColumns="False"
                        DataSourceID="sdsInterChangeRouteList" DataKeyNames="UID, Routes" AllowPaging="True"
                        AllowSorting="True" PageSize="20">
                        <Columns>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:Button ID="BtnEdit" runat="server" Text="編輯" CausesValidation="False" CommandName="EditProject">
                                    </asp:Button>&nbsp;<asp:Button ID="BtnDel" runat="server" Text="刪除" CausesValidation="False"
                                        CommandName="Delete" OnClientClick="return confirm('您確定要刪除這筆資料？資料刪除無法回復！');"></asp:Button>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField ReadOnly="true" InsertVisible ="false" DataField="UID" SortExpression="UID"
                                HeaderText="序號"></asp:BoundField>
                            <asp:TemplateField SortExpression="ProjectAlias" HeaderText="路徑起始交流道">
                                <ItemTemplate>
                                    <asp:Label ID="lblStartICName" runat="server" ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="RouteName" SortExpression="RouteName"
                                HeaderText="路徑清單"></asp:BoundField>
                            <asp:CheckBoxField ReadOnly="true" DataField="IsOnline"  SortExpression="IsOnline"
                                HeaderText="啟用"></asp:CheckBoxField>
                            <asp:BoundField DataField="Notes" SortExpression="Notes"
                                HeaderText="備註說明"></asp:BoundField>
                        </Columns>
                    </asp:GridView>

            </asp:View>
            <asp:View ID="View2" runat="server">
                        <table>
                            <tr>
                                <td style="width:100px">
                                    *選擇專案：</td>
                                <td>
                                    <asp:DropDownList ID="ddlProject" runat="server" DataTextField="ProjectName" DataValueField ="UID" DataSourceID="sdsProjectList" AppendDataBoundItems="true">
                                        <asp:ListItem Text="全部" Value="0" Selected="True"></asp:ListItem>
                                    </asp:DropDownList>
                                </td><td></td>
                            </tr>
                            <tr>
                                <td style="width:100px">
                                    *路徑清單：</td>
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
                                                    <asp:Repeater ID="rptRouteList" runat="server" OnItemCommand ="Repeater_RowCommand" OnItemDataBound ="rptRouteList_ItemDataBound" >
                                                        <ItemTemplate>
                                                            <asp:Button ID="btnIC" runat="server" CommandName="ICSelect"   CssClass="BtnProhibit" />
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
                                <td style="width: 100px">
                                    是否啟用：</td>
                                <td>
                                    <asp:CheckBox ID="cbIsOnline" runat="server" Text="啟用" Checked = "true" ValidationGroup="2"/>
                                <td>
                            </tr>
                            <tr>
                                <td style="width:100px">
                                    *備註說明：</td>
                                <td>
                                    <asp:TextBox ID="txtNotes" runat="server" Columns="80" TextMode="MultiLine" Rows="6"></asp:TextBox>
                                </td><td></td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <br />
                                    <asp:Button ID="btnInsert" runat="server" Text="新增資料" ValidationGroup="2" />&nbsp;
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
                        DeleteCommand="DELETE FROM InterChangeProhibitRoute WHERE [UID] = @UID"
                        InsertCommand="Insert into InterChangeProhibitRoute (ProjectID, StartIC, Routes, RouteName, IsOnline, Notes) values(@ProjectID, @StartIC, @Routes, @RouteName, @IsOnline, @Notes)" InsertCommandType ="Text" 
                        UpdateCommand="Update InterChangeProhibitRoute set ProjectID = @ProjectID, StartIC = @StartIC , Routes=@Routes, RouteName=@RouteName, IsOnline=@IsOnline, Notes=@Notes where UID = @UID" UpdateCommandType="Text" 
                        SelectCommand="Select h.HWName, i.ICName, r.StartIC, r.UID, r.Routes, r.RouteName, r.IsOnline, r.Notes, Case when r.ProjectID = 0 then '適用全部專案' else (Select top 1 ProjectName from InterChangeFareProject p where p.UID = r.ProjectID) end as ProjectName from InterChangeProhibitRoute r inner join InterChangeList i on i.UID = r.StartIC inner Join HWList h on h.UID = i.HWUID where (i.ICName like N'%' + @Key + N'%') or (r.RouteName like N'%' + @Key + N'%') order by h.ItemOrder, i.ItemOrder " SelectCommandType="Text">
                        <FilterParameters>
                            <asp:ControlParameter Name="Key" ControlID="txtSearch" PropertyName="Text" />
                        </FilterParameters>
                        <DeleteParameters>
                            <asp:Parameter Type="Int32" Name="UID"></asp:Parameter>
                        </DeleteParameters>
                        <InsertParameters>
                            <asp:ControlParameter Name="ProjectID" ControlID="ddlProject" PropertyName="SelectedValue" />
                            <asp:Parameter Name="StartIC" DefaultValue ="1"/>
                            <asp:Parameter Name="Routes" DefaultValue =""/>
                            <asp:Parameter Name="RouteName" DefaultValue =""/>
                            <asp:ControlParameter Name="IsOnline" ControlID="cbIsOnline" PropertyName="Checked" />
                            <asp:ControlParameter Name="Notes" ControlID="txtNotes" PropertyName="Text" />
                        </InsertParameters>
                        <UpdateParameters>
                             <asp:Parameter Name="UID"  Type="Int32" />
                            <asp:ControlParameter Name="ProjectID" ControlID="ddlProject" PropertyName="SelectedValue" />
                            <asp:Parameter Name="StartIC" DefaultValue ="1"/>
                            <asp:Parameter Name="Routes" DefaultValue =""/>
                            <asp:Parameter Name="RouteName" DefaultValue =""/>
                            <asp:ControlParameter Name="IsOnline" ControlID="cbIsOnline" PropertyName="Checked" />
                            <asp:ControlParameter Name="Notes" ControlID="txtNotes" PropertyName="Text" />
                        </UpdateParameters>
                        <SelectParameters>
                            <asp:ControlParameter Name="Key" ControlID="txtSearch" PropertyName="Text" DefaultValue="%"/>
                        </SelectParameters>
                    </asp:SqlDataSource>

</asp:Content> 