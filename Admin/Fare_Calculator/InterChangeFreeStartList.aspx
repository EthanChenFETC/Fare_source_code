<%@ Page Language="VB" AutoEventWireup="false" CodeFile="InterChangeFreeStartList.aspx.vb" Inherits="Fare_Calculator_InterChangeFreeStartList" MasterPageFile="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table cellspacing="0" cellpadding="5" width="100%" border="0">
    <tr>
        <td>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                   <div class="SearchArea">

                        國道別：<asp:DropDownList ID="ddlHWInsert" runat="server" DataTextField="HWName" DataValueField="UID" AutoPostBack="true" ValidationGroup="insert" DataSourceID="sdsHWList" ></asp:DropDownList>&nbsp;&nbsp;&nbsp;
                        起始交流道：<asp:DropDownList ID="ddlStartIC" runat="server" DataTextField="ICName" DataValueField="UID"  ValidationGroup="insert" DataSourceID="sdsStartIC" AutoPostBack ="true" ></asp:DropDownList>&nbsp;&nbsp;&nbsp;
                        訖點交流道：<asp:RadioButtonList ID="rblStopIC" runat="server" RepeatDirection="Horizontal" RepeatLayout ="Flow"  DataSourceID="sdsStopIC" DataTextField="ICName" DataValueField ="UID" ></asp:RadioButtonList>&nbsp;&nbsp;&nbsp;&nbsp;
                        起始或經過免費：<asp:RadioButtonList ID="rblFreeType" runat="server" RepeatDirection="Horizontal" RepeatLayout ="Flow" AutoPostBack="true" ><asp:ListItem Text="起始免費" Value="0" Selected="True" ></asp:ListItem><asp:ListItem Text="終點免費" Value="1"></asp:ListItem><asp:ListItem Text="南下(東向)經過免費" Value="2"></asp:ListItem><asp:ListItem Text="北上(西向)經過免費" Value="3"></asp:ListItem><asp:ListItem Text="雙向經過免費" Value="4"></asp:ListItem></asp:RadioButtonList>&nbsp;&nbsp;&nbsp;&nbsp;
                       <asp:Button ID="btnCreate" runat="server" Text="新增"  ValidationGroup="insert"/><br />
                        資料搜尋：
                        <asp:DropDownList ID="ddlHWList" runat="server" DataSourceID="sdsHWList" DataTextField="HWName" DataValueField ="UID" AppendDataBoundItems="true">
                            <asp:ListItem Text = "全部國道" Value = "0" ></asp:ListItem>
                        </asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;
                       起始或經過免費：<asp:RadioButtonList ID="rblFreeTypeSearch" runat="server" RepeatDirection="Horizontal" RepeatLayout ="Flow" AutoPostBack="true"  ><asp:ListItem Text="起始免費" Value="0" Selected="True" ></asp:ListItem><asp:ListItem Text="終點免費" Value="1" ></asp:ListItem><asp:ListItem Text="南下(東向)經過免費" Value="2"></asp:ListItem><asp:ListItem Text="北上(西向)經過免費" Value="3"></asp:ListItem><asp:ListItem Text="雙向經過免費" Value="4"></asp:ListItem></asp:RadioButtonList>&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:TextBox ID="txtSearch" runat="server" CssClass="form_txt" Columns="20"></asp:TextBox>&nbsp;<asp:Button
                            ID="btnSearch" runat="server" Text="搜尋" CssClass="Btn"></asp:Button>&nbsp;<asp:Button
                                ID="btnShowAll" runat="server" Text="清除" CssClass="Btn"></asp:Button>&nbsp;<asp:Label
                                    ID="lblMessage" runat="server" EnableViewState="False" ForeColor="Red"></asp:Label></div>
                    <br />
                    <asp:GridView ID="GridView1" runat="server" CssClass="Dg" AutoGenerateColumns="False"
                        DataSourceID="sdsInterChangeList" DataKeyNames="UID" AllowPaging="True"
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
                                    <asp:Label ID="lblHWName" runat="server" ></asp:Label>
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
                            <asp:TemplateField ShowHeader="False">
                                <EditItemTemplate>
                                    <asp:RadioButtonList ID="rblFreeType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" >
                                        <asp:ListItem Text="起始免費" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="終點免費" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="南下(東向)經過免費" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="北上(西向)經過免費" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="雙向經過免費" Value="4"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblFreeType" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="IsOnline" HeaderText="啟用">
                                <EditItemTemplate>
                                    <asp:CheckBox ID="ckbIsOnline" runat="server" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="ckbIsOnline" runat="server" Enabled ="false"/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField ReadOnly="true" DataField="UpdateDateTime" SortExpression="UpdateDateTime"
                                HeaderText="更新時間"></asp:BoundField>
                        </Columns>
                    </asp:GridView>
                  </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <br />
                     <asp:SqlDataSource ID="sdsInterChangeList" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                        DeleteCommand="DELETE FROM InterChangeStartFree WHERE [UID] = @UID"
                        InsertCommand="Insert into InterChangeStartFree (StartIC, StopIC,FreeType) values (@StartIC,@StopIC, @FreeType)"
                        UpdateCommand="UPDATE InterchangeStartFree set IsOnline=@IsOnline, FreeType=@FreeType, UpdateDateTime = getdate() where UID = @UID"
                        SelectCommand="select f.UID, f.IsOnline,f.FreeType, Convert(varchar(20),f.UpdateDateTime,111) as UpdateDateTime, h.HWName, i.ICName as StartICName, i2.ICName as StopICName, i.HWUID from InterchangeStartFree f inner join InterChangeList i on i.UID = f.StartIC inner join InterChangeList i2 on i2.UID = f.StopIC inner join HWLIST h on h.UID = i.HWUID where (@HWUID = 0 or @HWUID = h.UID) and f.FreeType= @FreeType and (i.ICName like N'%' + @Key + N'%' or h.HWName like N'%' + @Key + N'%' or i2.ICName like N'%' + @Key + N'%') order by h.ItemOrder, i.ItemOrder, f.UpdateDatetime" SelectCommandType="Text">
                        <FilterParameters>
                            <asp:ControlParameter Name="Key" ControlID="txtSearch" PropertyName="Text" />
                        </FilterParameters>
                        <DeleteParameters>
                            <asp:Parameter Type="Int32" Name="UID"></asp:Parameter>
                        </DeleteParameters>
                        <InsertParameters>
                            <asp:ControlParameter Name="StartIC" ControlID="ddlStartIC" PropertyName="SelectedValue" />
                            <asp:ControlParameter Name="StopIC" ControlID="rblStopIC" PropertyName="SelectedValue" />
                            <asp:ControlParameter Name="FreeType" ControlID="rblFreeType" PropertyName="SelectedValue" />
                        </InsertParameters>
                        <UpdateParameters>
                            <asp:Parameter Type="Int32" Name="UID"></asp:Parameter>
                            <asp:Parameter Type="Int32" Name="FreeType"></asp:Parameter>
                            <asp:Parameter Type="Boolean" Name="IsOnline"></asp:Parameter>
                        </UpdateParameters>
                        <SelectParameters>
                                <asp:ControlParameter Name="HWUID" ControlID="ddlHWList" PropertyName="SelectedValue" DefaultValue="0"/>
                                <asp:ControlParameter Name="Key" ControlID="txtSearch" PropertyName="Text" DefaultValue="%"/>
                                <asp:ControlParameter Name="FreeType" ControlID="rblFreeTypeSearch" PropertyName="SelectedValue"/>
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
                        SelectCommand="SELECT UID, ICName from InterChangeList where  (HWUID=@HWUID) and ((ItemOrder = (Select ItemOrder from InterchangeList where UID = @StartIC) + 1) or (ItemOrder =  (Select ItemOrder from InterchangeList where UID = @StartIC) - 1)) and  isonline > 0 order by itemorder">
                        <SelectParameters>
                            <asp:ControlParameter Name="HWUID" ControlID="ddlHWInsert" PropertyName="SelectedValue" DefaultValue="0"/>
                            <asp:ControlParameter Name="StartIC" ControlID="ddlStartIC" PropertyName="SelectedValue" DefaultValue="0"/>
                        </SelectParameters>
                        </asp:SqlDataSource>
                    
        </td>
    </tr>
</table>
</asp:Content> 