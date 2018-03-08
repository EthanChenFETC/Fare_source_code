<%@ Page Language="VB" AutoEventWireup="false" CodeFile="InterChangeFareEdit.aspx.vb" Inherits="Fare_Calculator_InterChangeFareEdit" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table cellspacing="0" cellpadding="5" width="100%" border="0">
    <tr>
        <td>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                   <div class="SearchArea">
                        資料搜尋：
                        <asp:TextBox ID="txtSearch" runat="server" CssClass="form_txt" Columns="20"></asp:TextBox>&nbsp;<asp:Button
                            ID="btnSearch" runat="server" Text="搜尋" CssClass="Btn"></asp:Button>&nbsp;<asp:Button
                                ID="btnShowAll" runat="server" Text="清除" CssClass="Btn"></asp:Button>&nbsp;<asp:Label
                                    ID="lblMessage" runat="server" EnableViewState="False" ForeColor="Red"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                       (費率表名稱：<asp:TextBox ID="txtFareProjectName" runat="server" ValidationGroup ="add"  ></asp:TextBox>&nbsp;&nbsp;
                       從費率表：<asp:DropDownList ID="ddlFareList" runat="server" DataSourceID ="sdsFareProject" DataValueField="UID" DataTextField="FareName" AppendDataBoundItems="true" ValidationGroup ="add" AutoPostBack="false"   >
                           <asp:ListItem Text="新增費率表" Value ="-1" Selected="True"></asp:ListItem> 
                       </asp:DropDownList>&nbsp;&nbsp;
                       <asp:Checkbox ID="ckbIsDefault" runat="server" Text="是否為預設標準費率" />&nbsp;&nbsp;
                       <asp:Button ID="btnAdd" runat="server" Text="新增" ValidationGroup ="add"  CssClass="Btn"/>)<br /></div>
                    <br />
                    <asp:GridView ID="GridView1" runat="server" CssClass="Dg" AutoGenerateColumns="False"
                        DataSourceID="sdsFareListProject" DataKeyNames="UID" AllowPaging="True"
                        AllowSorting="True" PageSize="20" >
                        <Columns>
                            <asp:TemplateField ShowHeader="False">
                                <EditItemTemplate>
                                    <asp:Button runat="server" Text="更新" CommandName="Update" CausesValidation="True"
                                        ID="Button1"></asp:Button>&nbsp;<asp:Button runat="server" Text="取消" CommandName="Cancel"
                                            CausesValidation="False" ID="Button2"></asp:Button>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Button ID="BtnEdit" runat="server" Text="編輯" CausesValidation="False" CommandName="Edit">
                                    </asp:Button >&nbsp;
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="FareName" SortExpression="FareName"
                                HeaderText="費率表名稱"></asp:BoundField>
                            <asp:CheckBoxField DataField ="IsDefault" SortExpression ="IsDefault" HeaderText="是否為預設標準費率" />
                            <asp:CheckBoxField DataField ="IsOnline" SortExpression ="IsDefault" HeaderText="啟用" />
                            <asp:BoundField DataField="Notes" SortExpression="Notes"
                                HeaderText="備註"></asp:BoundField>
                            <asp:BoundField ReadOnly="true" DataField="UpdateDateTime" SortExpression="UpdateDateTime"
                                HeaderText="更新時間"></asp:BoundField>
                            <asp:TemplateField HeaderText="費率表">
                                <ItemTemplate>
                                    <asp:Button ID="BtnFare" runat="server" Text="費率表詳細內容" CausesValidation="False" CommandName="FareEdit" CssClass="Btn">
                                    </asp:Button >&nbsp;
                                    <asp:Button ID="BtnDel" runat="server" Text="刪除" CausesValidation="False"
                                        CommandName="DeleteItem" CommandArgument ='<%# Bind("UID") %>' OnClientClick="return confirm('您確定要刪除這筆資料？資料刪除無法回復！');"></asp:Button>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <br />
                    <asp:DropDownList ID="ddlHWList" runat="server" DataSourceID="sdsHWList" DataTextField="HWName" DataValueField ="UID" AppendDataBoundItems="true" AutoPostBack ="true" ValidationGroup="ICFare" Visible="false">
                            <asp:ListItem Text = "全部國道" Value = "0" Selected="True" ></asp:ListItem>
                        </asp:DropDownList> &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    <asp:DropDownList ID="ddlICList" runat="server" DataSourceID="sdsICList" DataTextField="ICName" DataValueField ="UID" AppendDataBoundItems="true" AutoPostBack ="true" ValidationGroup="ICFare" Visible="false">
                            <asp:ListItem Text = "全部交流道" Value = "0" Selected="True" ></asp:ListItem>
                        </asp:DropDownList> &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    <asp:Button ID="btnAddFare" runat="server" Text="重整費率表" ValidationGroup ="addFare" Visible="false"  CssClass="Btn"/><asp:Label ID="lblFareAddMessage" runat="server" ForeColor="Red"></asp:Label>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    <asp:Button ID="btnCancel" runat="server" ValidationGroup="Cancel" Text="取消返回" Visible="false"  CssClass="Btn"/>
                    <asp:GridView ID="GridView2" runat="server" CssClass="Dg" AutoGenerateColumns="False"
                        DataSourceID="sdsInterChangeFare" DataKeyNames="UID" AllowPaging="True"
                        AllowSorting="True" PageSize="12" Visible ="false" >
                        <Columns>
                            <asp:TemplateField ShowHeader="False">
                                <EditItemTemplate>
                                    <asp:Button runat="server" Text="更新" CommandName="Update" CausesValidation="True"
                                        ID="Button1"></asp:Button>&nbsp;<asp:Button runat="server" Text="取消" CommandName="Cancel"
                                            CausesValidation="False" ID="Button2"></asp:Button>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:Button runat="server" Text="儲存" CommandName="Save" CausesValidation="True"
                                        ID="btnSave"></asp:Button>&nbsp;<asp:Button runat="server" Text="取消" CommandName="CancelSave"
                                            CausesValidation="False" ID="btnSaveCancel"></asp:Button>
                                </FooterTemplate>
                                <ItemTemplate>
                                    <asp:Button ID="BtnEdit" runat="server" Text="編輯" CausesValidation="False" CommandName="Edit">
                                    </asp:Button >&nbsp;
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField ReadOnly="true" DataField="HWName" SortExpression="HWName"
                                HeaderText="國道別"></asp:BoundField>
                            <asp:BoundField ReadOnly="true" DataField="ICName" SortExpression="ICName"
                                HeaderText="交流道名稱"></asp:BoundField>
                            <asp:BoundField DataField="FareS" SortExpression="FareS"
                                HeaderText="小車南下費率"></asp:BoundField>
                            <asp:BoundField DataField="FareM" SortExpression="FareM"
                                HeaderText="中車南下費率"></asp:BoundField>
                            <asp:BoundField DataField="FareG" SortExpression="FareG"
                                HeaderText="大車南下費率"></asp:BoundField>
                            <asp:BoundField DataField="FareSN" SortExpression="FareSN"
                                HeaderText="小車北上費率"></asp:BoundField>
                            <asp:BoundField DataField="FareMN" SortExpression="FareMN"
                                HeaderText="中車北上費率"></asp:BoundField>
                            <asp:BoundField DataField="FareGN" SortExpression="FareGN"
                                HeaderText="大車北上費率"></asp:BoundField>
                            <asp:BoundField DataField="FareSDiff" SortExpression="FareSDiff"
                                HeaderText="小車南下差別費率"></asp:BoundField>
                            <asp:BoundField DataField="FareMDiff" SortExpression="FareMDiff"
                                HeaderText="中車南下差別費率"></asp:BoundField>
                            <asp:BoundField DataField="FareGDiff" SortExpression="FareGDiff"
                                HeaderText="大車南下差別費率"></asp:BoundField>
                            <asp:BoundField DataField="FareSDiffN" SortExpression="FareSDiffN"
                                HeaderText="小車北上差別費率"></asp:BoundField>
                            <asp:BoundField DataField="FareMDiffN" SortExpression="FareMDiffN"
                                HeaderText="中車北上差別費率"></asp:BoundField>
                            <asp:BoundField DataField="FareGDiffN" SortExpression="FareGDiffN"
                                HeaderText="大車北上差別費率"></asp:BoundField>
                            <asp:BoundField DataField="FareSAdd" SortExpression="FareSAdd"
                                HeaderText="小車南下加價費率"></asp:BoundField>
                            <asp:BoundField DataField="FareMAdd" SortExpression="FareMAdd"
                                HeaderText="中車南下加價費率"></asp:BoundField>
                            <asp:BoundField DataField="FareGAdd" SortExpression="FareGAdd"
                                HeaderText="大車南下加價費率"></asp:BoundField>
                            <asp:BoundField DataField="FareSAddN" SortExpression="FareSAddN"
                                HeaderText="小車北上加價費率"></asp:BoundField>
                            <asp:BoundField DataField="FareMAddN" SortExpression="FareMAddN"
                                HeaderText="中車北上加價費率"></asp:BoundField>
                            <asp:BoundField DataField="FareGAddN" SortExpression="FareGAddN"
                                HeaderText="大車北上加價費率"></asp:BoundField>
                        </Columns>
                    </asp:GridView>
            <br />
                  </ContentTemplate>
            </asp:UpdatePanel>
           <br /><br />
                   <asp:SqlDataSource ID="sdsHWList" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                        SelectCommand="SELECT UID, HWName from HWList where isonline > 0 order by itemorder"></asp:SqlDataSource>
                     <asp:SqlDataSource ID="sdsFareProject" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                        SelectCommand="select * from InterChangeFareListProject order by  ItemOrder, UID desc" SelectCommandType="Text">
                       <SelectParameters>
                        </SelectParameters>
                    </asp:SqlDataSource>
                   <asp:SqlDataSource ID="sdsICList" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                        SelectCommand="SELECT i.UID, i.ICName from InterChangeList i inner join HWList h on h.UID = i.HWUID where (@HWUID = 0 or HWUID = @HWUID) and i.isonline > 0 order by h.ItemOrder, i.itemorder">
                       <SelectParameters>
                           <asp:ControlParameter ControlID ="ddlHWList" Name ="HWUID" PropertyName="SelectedValue" DefaultValue="0" />
                        </SelectParameters>
                    </asp:SqlDataSource>

                     <asp:SqlDataSource ID="sdsFareListProject" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                        UpdateCommand="UPDATE InterChangeFareListProject set FareName=@FareName,IsDefault=@IsDefault,Notes = @Notes, IsOnline = @IsOnline,UpdateDateTime = getdate() where UID = @UID"
                        SelectCommand="select * from InterChangeFareListProject where (@UID < 0 or UID = @UID) and FareName Like N'%' + @FareName + N'%' order by UID desc  " SelectCommandType="Text">
                        <FilterParameters>
                            <asp:ControlParameter Name="FareName" ControlID="txtSearch" PropertyName="Text" />
                        </FilterParameters>
                         <UpdateParameters>
                            <asp:Parameter Type="Int32" Name="UID"></asp:Parameter>
                            <asp:Parameter Type="Boolean" Name="IsDefault"></asp:Parameter>
                            <asp:Parameter Type="Boolean" Name="IsOnline"></asp:Parameter>
                            <asp:Parameter Type="String" Name="FareName"></asp:Parameter>
                            <asp:Parameter Type="String" Name="Notes"></asp:Parameter>
                        </UpdateParameters>
                        <SelectParameters>
                            <asp:Parameter Type="int32" Name="UID" DefaultValue ="-1"></asp:Parameter>
                            <asp:ControlParameter Name="FareName" ControlID="txtSearch" PropertyName="Text" DefaultValue="%"/>
                        </SelectParameters>
                    </asp:SqlDataSource>

                     <asp:SqlDataSource ID="sdsInterChangeFare" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                        UpdateCommand="UPDATE InterChangeFareList set FareS=@FareS,FareM=@FareM,FareG=@FareG,FareSN=@FareSN,FareMN=@FareMN,FareGN=@FareGN,FareSDiff=@FareSDiff,FareMDiff=@FareMDiff,FareGDiff=@FareGDiff,FareSDiffN=@FareSDiffN,FareMDiffN=@FareMDiffN,FareGDiffN=@FareGDiffN,FareSAdd=@FareSAdd,FareMAdd=@FareMAdd,FareGAdd=@FareGAdd,FareSAddN=@FareSAddN,FareMAddN=@FareMAddN,FareGAddN=@FareGAddN, UpdateDateTime = getdate() where UID = @UID"
                        SelectCommand="select ic.HWUID,  h.HWName, ic.ICName, f.* from InterChangeList ic inner join HWList h on h.UID = ic.HWUID inner join InterChangeFareList f on f.ICUID = ic.UID where f.FareListID = @FareListID and (@HWUID=0 or ic.HWUID = @HWUID)  and (@ICUID=0 or f.ICUID = @ICUID) order by h.ItemOrder, ic.ItemOrder  " SelectCommandType="Text">
                         <UpdateParameters>
                            <asp:Parameter Type="Int32" Name="UID"></asp:Parameter>
                            <asp:Parameter Type="Double" Name="FareS"></asp:Parameter>
                            <asp:Parameter Type="Double" Name="FareM"></asp:Parameter>
                            <asp:Parameter Type="Double" Name="FareG"></asp:Parameter>
                            <asp:Parameter Type="Double" Name="FareSN"></asp:Parameter>
                            <asp:Parameter Type="Double" Name="FareMN"></asp:Parameter>
                            <asp:Parameter Type="Double" Name="FareGN"></asp:Parameter>
                            <asp:Parameter Type="Double" Name="FareSDiff"></asp:Parameter>
                            <asp:Parameter Type="Double" Name="FareMDiff"></asp:Parameter>
                            <asp:Parameter Type="Double" Name="FareGDiff"></asp:Parameter>
                            <asp:Parameter Type="Double" Name="FareSDiffN"></asp:Parameter>
                            <asp:Parameter Type="Double" Name="FareMDiffN"></asp:Parameter>
                            <asp:Parameter Type="Double" Name="FareGDiffN"></asp:Parameter>
                            <asp:Parameter Type="Double" Name="FareSAdd"></asp:Parameter>
                            <asp:Parameter Type="Double" Name="FareMAdd"></asp:Parameter>
                            <asp:Parameter Type="Double" Name="FareGAdd"></asp:Parameter>
                            <asp:Parameter Type="Double" Name="FareSAddN"></asp:Parameter>
                            <asp:Parameter Type="Double" Name="FareMAddN"></asp:Parameter>
                            <asp:Parameter Type="Double" Name="FareGAddN"></asp:Parameter>
                        </UpdateParameters>
                        <SelectParameters>
                            <asp:Parameter Type="int32" Name="FareListID"></asp:Parameter>
                            <asp:ControlParameter Name="HWUID" ControlID="ddlHWList" PropertyName="SelectedValue" DefaultValue="0"/>
                            <asp:ControlParameter Name="ICUID" ControlID="ddlICList" PropertyName="SelectedValue" DefaultValue="0"/>
                        </SelectParameters>
                    </asp:SqlDataSource>
         </td>
    </tr>
</table>
</asp:Content> 