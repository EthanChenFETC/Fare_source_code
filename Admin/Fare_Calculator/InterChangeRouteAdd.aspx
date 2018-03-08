<%@ Page Language="VB" AutoEventWireup="false" CodeFile="InterChangeRouteAdd.aspx.vb" Inherits="Fare_Calculator_InterChangeRouteAdd" MasterPageFile="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table cellspacing="0" cellpadding="5" width="100%" border="0">
    <tr>
        <td>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                   <div class="SearchArea">

                        國道別：<asp:DropDownList ID="ddlHWInsert" runat="server" DataTextField="HWName" DataValueField="UID" AutoPostBack="true" ValidationGroup="insert" DataSourceID="sdsHWList" ></asp:DropDownList>&nbsp;&nbsp;&nbsp;
                        起始交流道：<asp:DropDownList ID="ddlStartIC" runat="server" DataTextField="ICName" DataValueField="UID"  ValidationGroup="insert" DataSourceID="sdsStartIC" AutoPostBack ="true" ></asp:DropDownList>&nbsp;&nbsp;&nbsp;
                        訖點交流道：<asp:DropDownList ID="ddlStopIC" runat="server" DataTextField="ICName" DataValueField="UID"  ValidationGroup="insert" DataSourceID="sdsStopIC"  ></asp:DropDownList>&nbsp;&nbsp;&nbsp;
                        X軸座標：<asp:TextBox ID="txtAxisX" runat="server" Text="0"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;
                        Y軸座標：<asp:TextBox ID="txtAxisY" runat="server" Text="0"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;
                        排序：<asp:TextBox ID="txtItemOrder" runat="server" Text="1"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;
                        啟用：<asp:CheckBox ID="ckbIsOnline" runat="server" Checked="true" Text="啟用" />&nbsp;&nbsp;&nbsp;&nbsp;
                       <asp:Button ID="btnCreate" runat="server" Text="新增"  ValidationGroup="insert"/><br />
                        資料搜尋：
                        <asp:DropDownList ID="ddlHWList" runat="server" DataSourceID="sdsHWList" DataTextField="HWName" DataValueField ="UID" AppendDataBoundItems="true">
                            <asp:ListItem Text = "全部國道" Value = "0" ></asp:ListItem>
                        </asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:TextBox ID="txtSearch" runat="server" CssClass="form_txt" Columns="20"></asp:TextBox>&nbsp;<asp:Button
                            ID="btnSearch" runat="server" Text="搜尋" CssClass="Btn"></asp:Button>&nbsp;<asp:Button
                                ID="btnShowAll" runat="server" Text="清除" CssClass="Btn"></asp:Button>&nbsp;<asp:Label
                                    ID="lblMessage" runat="server" EnableViewState="False" ForeColor="Red"></asp:Label></div>
                    <br />
                    <asp:GridView ID="GridView1" runat="server" CssClass="Dg" AutoGenerateColumns="False"
                        DataSourceID="sdsInterChangeRouteList" DataKeyNames="UID" AllowPaging="True"
                        AllowSorting="True" PageSize="12">
                        <Columns>
                            <asp:TemplateField ShowHeader="False">
                                <EditItemTemplate>
                                    <asp:Button runat="server" Text="更新" CommandName="UpdateItem" CausesValidation="True"
                                        ID="btnUpdate"></asp:Button>&nbsp;<asp:Button runat="server" Text="取消" CommandName="Cancel"
                                            CausesValidation="False" ID="Button2"></asp:Button>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Button ID="BtnEdit" runat="server" Text="編輯" CausesValidation="False" CommandName="Edit">
                                    </asp:Button >&nbsp;<asp:Button ID="BtnDel" runat="server" Text="刪除" CausesValidation="False"
                                        CommandName="Delete" OnClientClick="return confirm('您確定要刪除這筆資料？資料刪除無法回復！');"></asp:Button>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField ReadOnly="true" DataField="UID" SortExpression="UID"
                                HeaderText="序號"></asp:BoundField>
                            <asp:TemplateField SortExpression="HWUID" HeaderText="國道別">
                                <ItemTemplate>
                                    <asp:Label ID="lblHWName" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="StartIC" HeaderText="起點交流道名稱">
                                <ItemTemplate>
                                    <asp:Label ID="lblStartICName" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="StopIC" HeaderText="訖點交流道名稱">
                                <ItemTemplate>
                                    <asp:Label ID="lblStopICName" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="AxisX" SortExpression="AxisX"
                                HeaderText="X軸座標"></asp:BoundField>
                            <asp:BoundField DataField="AxisY" SortExpression="AxisY"
                                HeaderText="Y軸座標"></asp:BoundField>
                            <asp:BoundField DataField="ItemOrder" SortExpression="ItemOrder"
                                HeaderText="排序"></asp:BoundField>
                            <asp:TemplateField SortExpression="IsOnline" HeaderText="啟用">
                                <EditItemTemplate>
                                    <asp:CheckBox ID="ckbIsOnline" runat="server"  />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="ckbIsOnline" runat="server" Enabled ="false"/>
                                </ItemTemplate>
                            </asp:TemplateField>
                           
                        </Columns>
                    </asp:GridView>
                  </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <br />
                     <asp:SqlDataSource ID="sdsInterChangeRouteList" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                        DeleteCommand="DELETE FROM InterChangeRouteList WHERE [UID] = @UID"
                        InsertCommand="Insert into InterChangeRouteList (StartIC, StopIC,AxisX, AxisY, ItemOrder, IsOnline) values (@StartIC,@StopIC,@AxisX, @AxisY, @ItemOrder, @IsOnline)"
                        UpdateCommand="UPDATE InterChangeRouteList set IsOnline=@IsOnline, AxisX=@AxisX, AxisY=@AxisY, ItemOrder=@ItemOrder where UID = @UID"
                        SelectCommand="select f.UID, f.IsOnline,f.AxisX, f.AxisY, f.ItemOrder, f.IsOnline , h.HWName, i.ICName as StartICName, i2.ICName as StopICName, i.HWUID from InterChangeRouteList f inner join InterChangeList i on i.UID = f.StartIC inner join InterChangeList i2 on i2.UID = f.StopIC inner join HWLIST h on h.UID = i.HWUID where (@HWUID = 0 or @HWUID = h.UID)  and (i.ICName like N'%' + @Key + N'%' or h.HWName like N'%' + @Key + N'%' or i2.ICName like N'%' + @Key + N'%') order by h.ItemOrder, i.ItemOrder, i2.ItemOrder" SelectCommandType="Text">
                        <FilterParameters>
                            <asp:ControlParameter Name="Key" ControlID="txtSearch" PropertyName="Text" />
                        </FilterParameters>
                        <DeleteParameters>
                            <asp:Parameter Type="Int32" Name="UID"></asp:Parameter>
                        </DeleteParameters>
                        <InsertParameters>
                            <asp:ControlParameter Name="StartIC" ControlID="ddlStartIC" PropertyName="SelectedValue" />
                            <asp:ControlParameter Name="StopIC" ControlID="ddlStopIC" PropertyName="SelectedValue" />
                            <asp:ControlParameter Name="AxisX" ControlID="txtAxisX" PropertyName="Text" />
                            <asp:ControlParameter Name="AxisY" ControlID="txtAxisY" PropertyName="Text" />
                            <asp:ControlParameter Name="ItemOrder" ControlID="txtItemOrder" PropertyName="Text" />
                            <asp:ControlParameter Name="IsOnline" ControlID="ckbIsOnline" PropertyName="Checked" />
                        </InsertParameters>
                        <UpdateParameters>
                            <asp:Parameter Type="Int32" Name="UID"></asp:Parameter>
                            <asp:Parameter Type="Int32" Name="AxisX"></asp:Parameter>
                            <asp:Parameter Type="Int32" Name="AxisY"></asp:Parameter>
                            <asp:Parameter Type="Int32" Name="ItemOrder"></asp:Parameter>
                            <asp:Parameter Type="Boolean" Name="IsOnline"></asp:Parameter>
                        </UpdateParameters>
                        <SelectParameters>
                                <asp:ControlParameter Name="HWUID" ControlID="ddlHWList" PropertyName="SelectedValue" DefaultValue="0"/>
                                <asp:ControlParameter Name="Key" ControlID="txtSearch" PropertyName="Text" DefaultValue="%"/>
                    </SelectParameters>
                    </asp:SqlDataSource>

                    <asp:SqlDataSource ID="sdsHWList" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                        SelectCommand="SELECT UID, HWName from HWList where isonline > 0 order by itemorder"></asp:SqlDataSource>
                    <asp:SqlDataSource ID="sdsStartIC" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                        SelectCommand="SELECT UID, ICName from InterChangeList where  (HWUID=@HWUID) and isonline > 0 order by itemorder">
                        <SelectParameters>
                            <asp:ControlParameter Name="HWUID" ControlID="ddlHWInsert" PropertyName="SelectedValue" DefaultValue="0"/>
                        </SelectParameters>
                        </asp:SqlDataSource>
                    <asp:SqlDataSource ID="sdsStopIC" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                        SelectCommand="SELECT UID, ICName from InterChangeList where  (HWUID=@HWUID) and UID <> @StartIC and  isonline > 0 order by itemorder">
                        <SelectParameters>
                            <asp:ControlParameter Name="HWUID" ControlID="ddlHWInsert" PropertyName="SelectedValue" DefaultValue="0"/>
                            <asp:ControlParameter Name="StartIC" ControlID="ddlStartIC" PropertyName="SelectedValue" DefaultValue="0"/>
                        </SelectParameters>
                        </asp:SqlDataSource>
                    
        </td>
    </tr>
</table>
</asp:Content> 