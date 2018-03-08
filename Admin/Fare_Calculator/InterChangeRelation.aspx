<%@ Page Language="VB" AutoEventWireup="false" CodeFile="InterChangeRelation.aspx.vb" Inherits="Fare_Calculator_InterChangeRelation" MasterPageFile="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table cellspacing="0" cellpadding="5" width="100%" border="0">
    <tr>
        <td>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                <asp:MultiView id="MultiView1" runat="server" ActiveViewIndex="0">
                <asp:View id="View1" runat="server">
                   <div class="SearchArea">
                        新增關聯：<asp:Button ID="btnCreate" runat="server" Text="新增" />
                        資料搜尋：
                        <asp:DropDownList ID="ddlHWList1" runat="server" DataSourceID="sdsHWList" DataTextField="HWName" DataValueField ="UID" AutoPostBack ="true" AppendDataBoundItems="true">
                            <asp:ListItem Text = "全部國道" Value = "0" ></asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddlICList1" runat="server" DataSourceID="sdsICList1" DataTextField="ICName" DataValueField="UID" AutoPostBack="false" AppendDataBoundItems="true" ValidationGroup="2"></asp:DropDownList>
                        <asp:TextBox ID="txtSearch" runat="server" CssClass="form_txt" Columns="20"></asp:TextBox>&nbsp;<asp:Button
                            ID="btnSearch" runat="server" Text="搜尋" CssClass="Btn"></asp:Button>&nbsp;<asp:Button
                                ID="btnShowAll" runat="server" Text="清除" CssClass="Btn"></asp:Button>&nbsp;<asp:Label
                                    ID="lblMessage" runat="server" EnableViewState="False" ForeColor="Red"></asp:Label></div>
                    <br />
                    <asp:GridView ID="GridView1" runat="server" CssClass="Dg" AutoGenerateColumns="False"
                        DataSourceID="sdsInterChangeList" DataKeyNames="UID, StartIC, HWUIDD" AllowPaging="True"
                        AllowSorting="True" PageSize="12">
                        <Columns>
                            <asp:TemplateField ShowHeader="False">
                                <EditItemTemplate>
                                    <asp:Button runat="server" Text="更新" CommandName="Update" CausesValidation="True"
                                        ID="Button1"></asp:Button>&nbsp;<asp:Button runat="server" Text="取消" CommandName="Cancel"
                                            CausesValidation="False" ID="Button2"></asp:Button>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Button ID="BtnEdit" runat="server" Text="編輯" CausesValidation="False" CommandName="Edit">
                                    </asp:Button >&nbsp;<asp:Button ID="BtnDel" runat="server" Text="刪除" CausesValidation="False"
                                        CommandName="Delete" OnClientClick="return confirm('您確定要刪除這筆資料？資料刪除無法回復！');"></asp:Button>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField ReadOnly="true" DataField="HWNameO" SortExpression="HWNameO"
                                HeaderText="國道別"></asp:BoundField>
                            <asp:BoundField ReadOnly="true" DataField="StartIC" SortExpression="StartIC"
                                HeaderText="交流道序號"></asp:BoundField>
                            <asp:BoundField ReadOnly="true" DataField="ICNameO" SortExpression="ICNameO"
                                HeaderText="交流道名稱"></asp:BoundField>
                            <asp:TemplateField SortExpression="HWUIDD" HeaderText="關聯國道別">
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlHWList" runat="server" DataSourceID="sdsHWList" AutoPostBack="true"
                                        DataTextField="HWName" DataValueField="UID" OnSelectedIndexChanged="ddlHWList4_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblHWName" runat="server" ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="StopIC" HeaderText="關聯交流道">
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlICList" runat="server" DataSourceID="sdsICList4"
                                        DataTextField="ICName" DataValueField="UID" >
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblICName" runat="server" ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="DirectionStart" HeaderText="起點方向">
                                <EditItemTemplate>
                                    <asp:RadioButtonList ID="rblDirectionStart" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                        <asp:ListItem Text="南下(東向)" Value = "0"></asp:ListItem>
                                        <asp:ListItem Text="北上(西向)" Value = "1"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:RadioButtonList ID="rblDirectionStart" runat="server" Enabled="false"  RepeatDirection="Horizontal" RepeatLayout="Flow">
                                        <asp:ListItem Text="南下(東向)" Value = "0"></asp:ListItem>
                                        <asp:ListItem Text="北上(西向)" Value = "1"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="DirectionStop" HeaderText="訖點方向">
                                <EditItemTemplate>
                                    <asp:RadioButtonList ID="rblDirectionStop" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                        <asp:ListItem Text="南下(東向)" Value = "0"></asp:ListItem>
                                        <asp:ListItem Text="北上(西向)" Value = "1"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:RadioButtonList ID="rblDirectionStop" runat="server" Enabled="false"  RepeatDirection="Horizontal" RepeatLayout="Flow">
                                        <asp:ListItem Text="南下(東向)" Value = "0"></asp:ListItem>
                                        <asp:ListItem Text="北上(西向)" Value = "1"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="啟用">
                                <EditItemTemplate>
                                    <asp:CheckBox ID="ckbIsOnline" runat="server" Checked="true"  />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="ckbIsOnline" runat="server" Enabled="false" Checked="true"  />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>

            </asp:View>
            <asp:View ID="View2" runat="server">
                        <table>
                            <tr>
                                <td>
                                    *國道別：</td>
                                <td>
                                    <asp:DropDownList ID="ddlHWList2" runat="server" DataSourceID="sdsHWList" DataTextField="HWName" DataValueField="UID" AutoPostBack="true" AppendDataBoundItems="false" ></asp:DropDownList>
                                <td>
                            </tr>
                            <tr>
                                <td>
                                    *起始交流道：</td>
                                <td>
                                    <asp:DropDownList ID="ddlICList2" runat="server" DataSourceID="sdsICList2" DataTextField="ICName" DataValueField="UID" AutoPostBack="false" AppendDataBoundItems="false" ValidationGroup="2"></asp:DropDownList>
                                <td>
                            </tr>
                            <tr>
                                <td>
                                    *關聯國道別：</td>
                                <td>
                                    <asp:DropDownList ID="ddlHWList3" runat="server" DataSourceID="sdsHWList" DataTextField="HWName" DataValueField="UID" AutoPostBack="true" AppendDataBoundItems="false" ></asp:DropDownList>
                                <td>
                            </tr>
                            <tr>
                                <td>
                                    *關聯交流道：</td>
                                <td>
                                    <asp:DropDownList ID="ddlICList3" runat="server" DataSourceID="sdsICList3" DataTextField="ICName" DataValueField="UID" AutoPostBack="false" AppendDataBoundItems="false" ValidationGroup="2"></asp:DropDownList>
                                <td>
                            </tr>
                            <tr>
                                <td>
                                    *起始交流道行駛方向：</td>
                                <td>
                                    <asp:RadioButtonList ID="rblDirectionStart" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                        <asp:ListItem Text="南下" Value = "0" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="北上" Value = "1"></asp:ListItem>
                                    </asp:RadioButtonList>
                                <td>
                            </tr>
                            <tr>
                                <td>
                                    *終點交流道行駛方向：</td>
                                <td>
                                    <asp:RadioButtonList ID="rblDirectionStop" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                        <asp:ListItem Text="南下" Value = "0" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="北上" Value = "1"></asp:ListItem>
                                    </asp:RadioButtonList>
                                <td>
                            </tr>
                            <tr>
                                <td>
                                    是否上線：</td>
                                <td>
                                    <asp:CheckBox ID="cbIsOnline" runat="server" Text="上線/停用" Checked = "true" ValidationGroup="2"/>
                                <td>
                            </tr>
                            <tr>
                                <td>
                                    <br />
                                    <asp:Button ID="btnInsert" runat="server" Text="新增資料" ValidationGroup="2" />&nbsp;
                                    <asp:Button ID="btnCancel" runat="server" Text="取消返回" ValidationGroup="2" />&nbsp;
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                        </table>
            </asp:View> 
            </asp:MultiView> 
                </ContentTemplate>
            </asp:UpdatePanel>
            <br />
                    <asp:SqlDataSource ID="sdsHWList" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                        SelectCommand="SELECT UID, HWName from HWList where isonline > 0 order by itemorder"></asp:SqlDataSource>
                    <asp:SqlDataSource ID="sdsICList1" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                        SelectCommand="SELECT UID, ICName from InterChangeList where  HWUID=@HWUID and isonline > 0 order by itemorder">
                        <SelectParameters>
                            <asp:ControlParameter Name="HWUID" ControlID="ddlHWList1" PropertyName="SelectedValue" DefaultValue="0"/>
                        </SelectParameters>
                        </asp:SqlDataSource>
                    <asp:SqlDataSource ID="sdsICList2" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                        SelectCommand="SELECT UID, ICName from InterChangeList where HWUID=@HWUID and isonline > 0 order by itemorder">
                        <SelectParameters>
                            <asp:ControlParameter Name="HWUID" ControlID="ddlHWList2" PropertyName="SelectedValue" DefaultValue="0"/>
                        </SelectParameters>
                        </asp:SqlDataSource>
                    <asp:SqlDataSource ID="sdsICList3" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                        SelectCommand="SELECT UID, ICName from InterChangeList where HWUID=@HWUID and isonline > 0 order by itemorder">
                        <SelectParameters>
                            <asp:ControlParameter Name="HWUID" ControlID="ddlHWList3" PropertyName="SelectedValue" DefaultValue="0"/>
                        </SelectParameters>
                        </asp:SqlDataSource>
                    <asp:SqlDataSource ID="sdsICList4" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                        SelectCommand="SELECT UID, ICName from InterChangeList where HWUID=@HWUID and isonline > 0 order by itemorder">
                        <SelectParameters>
                            <asp:Parameter Name="HWUID"  DefaultValue="0"/>
                        </SelectParameters>
                        </asp:SqlDataSource>
                     <asp:SqlDataSource ID="sdsInterChangeList" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                        DeleteCommand="DELETE FROM InterChangeRelation WHERE [UID] = @UID"
                        InsertCommand="Insert into InterChangeRelation (StartIC,StopIC,DirectionStart,DirectionStop,IsOnline) values (@StartIC,@StopIC,@DirectionStart,@DirectionStop,@IsOnline)"
                        UpdateCommand="UPDATE InterChangeRelation set StartIC=@StartIC,StopIC=@StopIC,DirectionStart=@DirectionStart,DirectionStop=@DirectionStop,IsOnline=@IsOnline where UID = @UID"
                        SelectCommand="select ir.*, i1.ICName as ICNameO, i2.ICName as ICNameD, i1.HWUID as HWUIDO, i2.HWUID as HWUIDD, h1.HWName as HWNameO, h2.HWName as HWNameD from InterChangeRelation ir inner join InterChangeList i1 on i1.UID = ir.StartIC inner join InterChangeList i2 on i2.UID = ir.StopIC inner join HWList h1 on h1.UID = i1.HWUID inner join HWList h2 on h2.UID = i2.HWUID where (@HWUID=0 or i1.HWUID = @HWUID) and (@ICUID=0 or i1.UID = @ICUID) and i1.ICName Like N'%' + @ICName + N'%' order by i1.HWUID, i1.ItemOrder, i2.HWUID, i2.ItemOrder  " SelectCommandType="Text">
                        <FilterParameters>
                            <asp:ControlParameter Name="ICName" ControlID="txtSearch" PropertyName="Text" />
                        </FilterParameters>
                        <DeleteParameters>
                            <asp:Parameter Type="Int32" Name="UID"></asp:Parameter>
                        </DeleteParameters>
                        <InsertParameters>
                            <asp:ControlParameter Name="StartIC" ControlID="ddlICList2" PropertyName="SelectedValue" />
                            <asp:ControlParameter Name="StopIC" ControlID="ddlICList3" PropertyName="Text" />
                            <asp:ControlParameter Name="DirectionStart" ControlID="rblDirectionStart" PropertyName="SelectedValue" />
                            <asp:ControlParameter Name="DirectionStop" ControlID="rblDirectionStop" PropertyName="SelectedValue" />
                            <asp:ControlParameter Name="IsOnline" ControlID="cbIsOnline" PropertyName="Checked" />
                        </InsertParameters>
                        <UpdateParameters>
                            <asp:Parameter Type="Int32" Name="UID"></asp:Parameter>
                            <asp:Parameter Type="Int32" Name="StartIC"></asp:Parameter>
                            <asp:Parameter Type="Int32" Name="StopIC"></asp:Parameter>
                            <asp:Parameter Type="Int32" Name="DirectionStart"></asp:Parameter>
                            <asp:Parameter Type="Int32" Name="DirectionStop"></asp:Parameter>
                            <asp:Parameter Type="Boolean" Name="IsOnline"></asp:Parameter>
                        </UpdateParameters>
                        <SelectParameters>
                            <asp:ControlParameter Name="HWUID" ControlID="ddlHWList1" PropertyName="SelectedValue" DefaultValue="0"/>
                            <asp:ControlParameter Name="ICUID" ControlID="ddlICList1" PropertyName="SelectedValue" DefaultValue="0"/>
                            <asp:ControlParameter Name="ICName" ControlID="txtSearch" PropertyName="Text" DefaultValue="%"/>
                        </SelectParameters>
                    </asp:SqlDataSource>
        </td>
    </tr>
</table>
</asp:Content> 