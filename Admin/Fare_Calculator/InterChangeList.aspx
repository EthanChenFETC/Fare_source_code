<%@ Page Language="VB" AutoEventWireup="false" CodeFile="InterChangeList.aspx.vb" Inherits="Fare_Calculator_InterChangeList" MasterPageFile="~/MasterPage.master"%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table cellspacing="0" cellpadding="5" width="100%" border="0">
    <tr>
        <td>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                <asp:MultiView id="MultiView1" runat="server" ActiveViewIndex="0">
                <asp:View id="View1" runat="server">
                   <div class="SearchArea">
                        新增交流道：<asp:Button ID="btnCreate" runat="server" Text="新增" />
                        資料搜尋：
                        <asp:DropDownList ID="ddlHWListSearch" runat="server" DataSourceID="sdsHWList" DataTextField="HWName" DataValueField ="UID" AutoPostBack ="false" AppendDataBoundItems="true">
                            <asp:ListItem Text="全部" Value="0" Selected ="True"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddlDirection" runat="server" AutoPostBack ="false" Visible="false">
                            <asp:ListItem Text="全部" Value="-1" Selected ="True"></asp:ListItem>
                            <asp:ListItem Text="南下" Value="1"></asp:ListItem>
                            <asp:ListItem Text="北上" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:TextBox ID="txtSearch" runat="server" CssClass="form_txt" Columns="20"></asp:TextBox>&nbsp;<asp:Button
                            ID="btnSearch" runat="server" Text="搜尋" CssClass="Btn"></asp:Button>&nbsp;<asp:Button
                                ID="btnShowAll" runat="server" Text="清除" CssClass="Btn"></asp:Button>&nbsp;<asp:Label
                                    ID="lblMessage" runat="server" EnableViewState="False" ForeColor="Red"></asp:Label>
                   </div>
                    <br />
                    <asp:GridView ID="GridView1" runat="server" CssClass="Dg" AutoGenerateColumns="False"
                        DataSourceID="sdsInterChangeList" DataKeyNames="UID" AllowPaging="True"
                        AllowSorting="True" PageSize="20">
                        <Columns>
                            <asp:TemplateField ShowHeader="False">
                                <EditItemTemplate>
                                    <asp:Button runat="server" Text="更新" CommandName="Update" CausesValidation="True"
                                        ID="Button1"></asp:Button>&nbsp;<asp:Button runat="server" Text="取消" CommandName="Cancel"
                                            CausesValidation="False" ID="Button2"></asp:Button>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Button ID="BtnEdit" runat="server" Text="編輯" CausesValidation="False" CommandName="Edit">
                                    </asp:Button>&nbsp;<asp:Button ID="BtnDel" runat="server" Text="刪除" CausesValidation="False"
                                        CommandName="DeleteItem" CommandArgument ='<%# Bind("UID") %>' OnClientClick="return confirm('您確定要刪除這筆資料？資料刪除無法回復！');"></asp:Button>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField ReadOnly="true" InsertVisible ="false" DataField="UID" SortExpression="UID"
                                HeaderText="交流道序號"></asp:BoundField>
                            <asp:BoundField DataField="ICName" SortExpression="ICName"
                                HeaderText="交流道名稱"></asp:BoundField>
                            <asp:TemplateField SortExpression="HWUID" HeaderText="國道別">
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlHWList" runat="server" DataSourceID="sdsHWList"
                                        DataTextField="HWName" DataValueField="UID">
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblHWName" runat="server" ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ICMiles" SortExpression="ICMiles"
                                HeaderText="南下公里數"></asp:BoundField>
                            <asp:BoundField DataField="ICMilesN" SortExpression="ICMilesN"
                                HeaderText="北上公里數"></asp:BoundField>
                            <asp:BoundField DataField="ICMilesBetween" SortExpression="ICMilesBetween"
                                HeaderText="南下計費里程"></asp:BoundField>
                            <asp:BoundField DataField="ICMilesBetweenN" SortExpression="ICMilesBetweenN"
                                HeaderText="北上計費里程"></asp:BoundField>
                            <asp:CheckBoxField DataField="OutSouth" SortExpression="OutSouth"
                                HeaderText="南下(東向)出口"></asp:CheckBoxField>
                            <asp:CheckBoxField DataField="OutNorth" SortExpression="OutNorth"
                                HeaderText="北上(西向)出口"></asp:CheckBoxField>
                            <asp:CheckBoxField DataField="InSouth" SortExpression="InSouth"
                                HeaderText="南下(東向)入口"></asp:CheckBoxField>
                            <asp:CheckBoxField DataField="InNorth" SortExpression="InNorth"
                                HeaderText="北上(西向)入口"></asp:CheckBoxField>
                            <asp:TemplateField SortExpression="ICAlias" HeaderText="交流道群組" Visible="false" >
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlICList" runat="server" DataSourceID="sdsICListGroup" DataTextField ="ICName" DataValueField="UID" >
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAlias" runat="server" ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:CheckBoxField DataField ="IsVirture" SortExpression ="IsVirture" HeaderText = "清單顯示" />
                            <asp:BoundField DataField="ItemOrder" SortExpression="ItemOrder"
                                HeaderText="排序"></asp:BoundField>
                            <asp:CheckBoxField DataField = "IsOnline" SortExpression = "IsOnline" HeaderText = "啟用" />
                            <asp:BoundField DataField="AxisX" SortExpression="AxisX"
                                HeaderText="X座標"></asp:BoundField>
                            <asp:BoundField DataField="AxisY" SortExpression="AxisY"
                                HeaderText="Y座標"></asp:BoundField>
                            <asp:BoundField DataField="MapCoords" SortExpression="MapCoords"
                                HeaderText="地圖點選範圍"></asp:BoundField>
                            <asp:BoundField DataField="Notes" SortExpression="Notes"
                                HeaderText="備註"></asp:BoundField>
                            
                        </Columns>
                    </asp:GridView>

            </asp:View>
            <asp:View ID="View2" runat="server">
                        <table>
                            <tr>
                                <td>
                                    *交流道名稱：</td>
                                <td>
                                    <asp:TextBox ID="txtCName" runat="server" Text="" ValidationGroup="2"></asp:TextBox></td>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCName"
                                        ErrorMessage="請輸入交流道名稱！" ValidationGroup="2"></asp:RequiredFieldValidator></td>
                            </tr>
                            <tr>
                                <td>
                                    *國道別：</td>
                                <td>
                                    <asp:DropDownList ID="ddlHWList" runat="server" DataSourceID="sdsHWList" DataTextField="HWName" DataValueField="UID" AutoPostBack="false" AppendDataBoundItems="false" ValidationGroup="2"></asp:DropDownList>
                                </td><td></td>
                            </tr>
                            <tr>
                                <td>
                                    *交流道南下里程數：</td>
                                <td>
                                    南下：<asp:TextBox ID="txtMiles" runat="server" Text=""></asp:TextBox>&nbsp;&nbsp;&nbsp;
                                    北上：<asp:TextBox ID="txtMilesN" runat="server" Text=""></asp:TextBox>
                                </td>
                                <td>
                                <asp:CompareValidator id="CompareValidator2" runat="server"
errormessage="請填寫正確的數字格式" display="Dynamic" controltovalidate="txtMiles"
operator="DataTypeCheck" type="Double"></asp:CompareValidator>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtMiles"
                                        ErrorMessage="請輸入交流道里程數！" ValidationGroup="2"></asp:RequiredFieldValidator></td>
                                <asp:CompareValidator id="CompareValidator1" runat="server"
errormessage="請填寫正確的數字格式" display="Dynamic" controltovalidate="txtMiles"
operator="DataTypeCheck" type="Double"></asp:CompareValidator>

                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtMilesN"
                                        ErrorMessage="請輸入交流道里程數！" ValidationGroup="2"></asp:RequiredFieldValidator></td>
                            </tr>
                            <tr>
                                <td>
                                    *計費里程：</td>
                                <td>
                                    南下：<asp:TextBox ID="txtMilesBetween" runat="server" Text=""></asp:TextBox>&nbsp;&nbsp;&nbsp;
                                    北上：<asp:TextBox ID="txtMilesBetweenN" runat="server" Text=""></asp:TextBox>

                                </td>
                                <td>
                                <asp:CompareValidator id="CompareValidator3" runat="server" errormessage="請填寫正確的數字格式" display="Dynamic" controltovalidate="txtMilesBetween"
                                    operator="DataTypeCheck" type="Double" ></asp:CompareValidator>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtMilesBetween"
                                        ErrorMessage="請輸入交流道里程數！" ValidationGroup="2"></asp:RequiredFieldValidator></td>
                                <asp:CompareValidator id="CompareValidator4" runat="server" errormessage="請填寫正確的數字格式" display="Dynamic" controltovalidate="txtMilesBetweenN"
                                    operator="DataTypeCheck" type="Double" ></asp:CompareValidator>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtMilesBetweenN"
                                        ErrorMessage="請輸入交流道里程數！" ValidationGroup="2"></asp:RequiredFieldValidator></td>
                            </tr>
                            <tr>
                                <td>
                                    *出入口方向：</td>
                                <td>
                                    <asp:CheckBox ID="ckbOutSouth" runat="server" Text="南下(東向)出口" />
                                    <asp:CheckBox ID="ckbOutNorth" runat="server" Text="北上(西向)出口" />
                                    <asp:CheckBox ID="ckbInSouth" runat="server" Text="南下(東向)入口" />
                                    <asp:CheckBox ID="ckbInNorth" runat="server" Text="北上(西向)入口" />
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    *交流道群組：</td>
                                <td>
                                    <asp:DropDownList ID="ddlICGroup" runat="server" DataSourceID ="sdsICListGroup" DataTextField="ICName" DataValueField="UID" AppendDataBoundItems="true">
                                        <asp:ListItem Text="獨立群組" Value="0" Selected="True" ></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                </td>
                            </tr>
                           
                            <tr>
                                <td>
                                    *順序：</td>
                                <td>
                                    <asp:TextBox ID="txtItemOrder" runat="server" Text="1"></asp:TextBox></td>
                                <td>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="txtItemOrder" ErrorMessage="請填寫正確的數字格式" ValidationExpression="^[0-9]{1,8}?$" ValidationGroup="2"></asp:RegularExpressionValidator>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtItemOrder"
                                        ErrorMessage="請輸入順序！" ValidationGroup="2"></asp:RequiredFieldValidator></td>
                            </tr>
                            <tr>
                                <td>
                                    清單是否顯示：</td>
                                <td>
                                    <asp:CheckBox ID="cbIsVirture" runat="server" Text="顯示" Checked = "true" ValidationGroup="2"/></td>
                                <td></td>

                            </tr>
                            <tr>
                                <td>
                                    是否上線：</td>
                                <td>
                                    <asp:CheckBox ID="cbIsOnline" runat="server" Text="上線/停用" Checked = "true" ValidationGroup="2"/></td>
                                <td></td>

                            </tr>
                            <tr>
                                <td>
                                    座標：</td>
                                <td>
                                    *X座標：<asp:TextBox ID="txtAxisX" runat="server" Text="0"></asp:TextBox>
                                    *Y座標：<asp:TextBox ID="txtAxisY" runat="server" Text="0"></asp:TextBox>
                                    *地圖點選範圍：<asp:TextBox ID="txtMapCoords" runat="server" Text="0"></asp:TextBox>
                                </td> <td>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtAxisX" ErrorMessage="請填寫正確的數字格式" ValidationExpression="^[0-9]{1,8}(.[0-9]{1,2})?$" ValidationGroup="2"></asp:RegularExpressionValidator>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtAxisX"
                                        ErrorMessage="請輸入X座標！" ValidationGroup="2"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtAxisY" ErrorMessage="請填寫正確的數字格式" ValidationExpression="^[0-9]{1,8}(.[0-9]{1,2})?$" ValidationGroup="2"></asp:RegularExpressionValidator>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtAxisY"
                                        ErrorMessage="請輸入X座標！" ValidationGroup="2"></asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtMapCoords"
                                        ErrorMessage="請輸入3組座標！" ValidationGroup="2"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    *備註：</td>
                                <td>
                                    <asp:TextBox ID="txtNotes" runat="server" Text="" TextMode ="MultiLine" Rows="8" Columns="60" ></asp:TextBox></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>
                                    <br />
                                    <asp:Button ID="btnInsert" runat="server" Text="新增資料" ValidationGroup="2" />&nbsp;
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
                </ContentTemplate>
            </asp:UpdatePanel>
            <br />
         </td>
    </tr>
</table>
                    <asp:SqlDataSource ID="sdsHWList" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                        SelectCommand="SELECT UID, HWName from HWList where isonline > 0 order by itemorder"></asp:SqlDataSource>
                    <asp:SqlDataSource ID="sdsICListGroup" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                        SelectCommand="SELECT UID, ICName from InterChangeList where isonline > 0 And ICAlias = UID order by itemorder">
                    </asp:SqlDataSource>
                     <asp:SqlDataSource ID="sdsInterChangeList" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                        DeleteCommand="InterChangeList_Delete" DeleteCommandType="StoredProcedure" 
                        InsertCommand="InterChangeList_Add"  InsertCommandType ="StoredProcedure" 
                        UpdateCommand="UPDATE InterChangeList set HWUID=@HWUID, ICName=@ICName, ICMiles=@ICMiles, ICMilesBetween=@ICMilesBetween, ICMilesN=@ICMilesN, ICMilesBetweenN=@ICMilesBetweenN, OutSouth=@OutSouth, OutNorth=@OutNorth, InSouth=@InSouth, InNorth=@InNorth, ItemOrder=@ItemOrder, IsOnline=@IsOnline,IsVirture=@IsVirture,AxisX=@AxisX,AxisY=@AxisY, MapCoords=@MapCoords, Notes=@Notes where UID = @UID"
                        SelectCommand="Select *,(Select Top 1 a.ICName from InterChangeList a where a.UID = b.ICAlias) as AliasICName,  (Select HWName from HWList where UID=HWUID) as HWName from InterChangeList b where (@HWUID=0 or HWUID=@HWUID) and ICName LIKE N'%' + @ICName + N'%' order by HWUID, ItemOrder" SelectCommandType="Text">
                        <%--<FilterParameters>
                            <asp:ControlParameter Name="ICName" ControlID="txtSearch" PropertyName="Text" />
                        <%--</FilterParameters>--%>
                        <DeleteParameters>
                            <asp:Parameter Type="Int32" Name="ICUID"></asp:Parameter>
                        </DeleteParameters>
                        <InsertParameters>
                            <asp:ControlParameter Name="HWUID" ControlID="ddlHWList" PropertyName="SelectedValue" />
                            <asp:ControlParameter Name="ICName" ControlID="txtCName" PropertyName="Text" />
                            <asp:ControlParameter Name="ICMiles" ControlID="txtMiles" PropertyName="Text" />
                            <asp:ControlParameter Name="ICMilesBetween" ControlID="txtMilesBetween" PropertyName="Text" />
                            <asp:ControlParameter Name="ICMilesN" ControlID="txtMilesN" PropertyName="Text" />
                            <asp:ControlParameter Name="ICMilesBetweenN" ControlID="txtMilesBetweenN" PropertyName="Text" />
                            <asp:ControlParameter Name="OutSouth" ControlID="ckbOutSouth" PropertyName="Checked" />
                            <asp:ControlParameter Name="OutNorth" ControlID="ckbOutNorth" PropertyName="Checked" />
                            <asp:ControlParameter Name="InSouth" ControlID="ckbInSouth" PropertyName="Checked" />
                            <asp:ControlParameter Name="InNorth" ControlID="ckbInNorth" PropertyName="Checked" />
                            <asp:ControlParameter Name="ICAlias" ControlID="ddlICGroup" PropertyName="SelectedValue" />
                            <asp:ControlParameter Name="ItemOrder" ControlID="txtItemOrder" PropertyName="Text" />
                            <asp:ControlParameter Name="IsVirture" ControlID="cbIsVirture" PropertyName="Checked" />
                            <asp:ControlParameter Name="IsOnline" ControlID="cbIsOnline" PropertyName="Checked" />
                            <asp:ControlParameter Name="AxisX" ControlID="txtAxisX" PropertyName="Text" />
                            <asp:ControlParameter Name="AxisY" ControlID="txtAxisY" PropertyName="Text" />
                            <asp:ControlParameter Name="MapCoords" ControlID="txtMapCoords" PropertyName="Text" />
                            <asp:ControlParameter Name="Notes" ControlID="txtNotes" PropertyName="Text" />
                            <asp:Parameter Name="retVal" Direction="Output" Type="Int32" />
                        </InsertParameters>
                        <UpdateParameters>
                            <asp:Parameter Type="Int32" Name="UID"></asp:Parameter>
                            <asp:Parameter Type="Int32" Name="HWUID"></asp:Parameter>
                            <asp:Parameter Type="String" Name="ICName"></asp:Parameter>
                            <asp:Parameter Type="Double" Name="ICMiles"></asp:Parameter>
                            <asp:Parameter Type="Double" Name="ICMilesBetween"></asp:Parameter>
                            <asp:Parameter Type="Double" Name="ICMilesN"></asp:Parameter>
                            <asp:Parameter Type="Double" Name="ICMilesBetweenN"></asp:Parameter>
                            <asp:Parameter Type="Boolean" Name="OutSouth"></asp:Parameter>
                            <asp:Parameter Type="Boolean" Name="OutNorth"></asp:Parameter>
                            <asp:Parameter Type="Boolean" Name="InSouth"></asp:Parameter>
                            <asp:Parameter Type="Boolean" Name="InNorth"></asp:Parameter>
                            <asp:Parameter Type="Int32" Name="ICAlias"></asp:Parameter>
                            <asp:Parameter Type="Int32" Name="ItemOrder"></asp:Parameter>
                            <asp:Parameter Type="Boolean" Name="IsVirture"></asp:Parameter>
                            <asp:Parameter Type="Boolean" Name="IsOnline"></asp:Parameter>
                            <asp:Parameter Type="Int32" Name="AxisX"></asp:Parameter>
                            <asp:Parameter Type="Int32" Name="AxisY"></asp:Parameter>
                            <asp:Parameter Type="String" Name="MapCoords"></asp:Parameter>
                            <asp:Parameter Type="String" Name="Notes"></asp:Parameter>
                        </UpdateParameters>
                        <SelectParameters>
                            <asp:ControlParameter Name="HWUID" ControlID="ddlHWListSearch" PropertyName="SelectedValue" DefaultValue="0"/>
                            <asp:ControlParameter Name="ICName" ControlID="txtSearch" PropertyName="Text" DefaultValue="%"/>
                        </SelectParameters>
                    </asp:SqlDataSource>

</asp:Content> 