<%@ Page Language="VB" AutoEventWireup="false" CodeFile="InterChangeFareProject.aspx.vb" Inherits="Fare_Calculator_InterChangeFareProject" MasterPageFile="~/MasterPage.master"%>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table cellspacing="0" cellpadding="5" width="100%" border="0">
    <tr>
        <td>
            
                <asp:MultiView id="MultiView1" runat="server" ActiveViewIndex="0">
                <asp:View id="View1" runat="server">
                   <div class="SearchArea">
                        新增專案：<asp:Button ID="btnCreate" runat="server" Text="新增" />
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
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:Button ID="BtnEdit" runat="server" Text="編輯" CausesValidation="False" CommandName="EditProject">
                                    </asp:Button>&nbsp;<asp:Button ID="BtnDel" runat="server" Text="刪除" CausesValidation="False"
                                        CommandName="Delete" OnClientClick="return confirm('您確定要刪除這筆資料？資料刪除無法回復！');"></asp:Button>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField ReadOnly="true" InsertVisible ="false" DataField="UID" SortExpression="UID"
                                HeaderText="專案序號"></asp:BoundField>
                            <asp:BoundField DataField="ProjectName" SortExpression="ICName"
                                HeaderText="專案名稱"></asp:BoundField>
                            <asp:TemplateField SortExpression="ProjectAlias" HeaderText="母專案">
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlProject" runat="server" AppendDataBoundItems="true" DataTextField="ProjectName" DataValueField="UID" DataSourceID ="sdsProject">
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblProject" runat="server" ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="StartDate" HeaderText="上線日期">
                                <EditItemTemplate>
                                    <ew:CalendarPopup ID="CalendarPopupStartDate" runat="server" CssClass="txt40" Culture="Chinese (Taiwan)"
                                DisplayOffsetY="-100" PostedDate="" UpperBoundDate="12/31/9999 23:59:59" DayNameFormat="Full" ShowGoToToday="True"
                                         
                                        >
                                        </ew:CalendarPopup>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblStartDate" runat="server" ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="EndDate" HeaderText="停用日期">
                                <EditItemTemplate>
                                    <ew:CalendarPopup ID="CalendarPopupEndDate" runat="server" CssClass="txt40" Culture="Chinese (Taiwan)"
                            DisplayOffsetY="-100" PostedDate="" UpperBoundDate="12/31/9999 23:59:59" DayNameFormat="Full" ShowGoToToday="True"
                                         
                                        >
                                        </ew:CalendarPopup>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblEndDate" runat="server" ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="StartHour" SortExpression="StartHour"
                                HeaderText="開始時段"></asp:BoundField>
                            <asp:BoundField DataField="EndHour" SortExpression="EndHour"
                                HeaderText="結束時段"></asp:BoundField>
                            <asp:CheckBoxField DataField = "IsFree" SortExpression = "IsOnline" HeaderText = "是否免費" Text ="免費" />
                            <asp:BoundField DataField="FareS" SortExpression="FareS"
                                HeaderText="小車費率"></asp:BoundField>
                            <asp:BoundField DataField="FareM" SortExpression="FareM"
                                HeaderText="中車費率"></asp:BoundField>
                            <asp:BoundField DataField="FareG" SortExpression="FareG"
                                HeaderText="大車費率"></asp:BoundField>
                            <asp:BoundField DataField="FareSDiscount" SortExpression="FareSDiscount"
                                HeaderText="小車折扣費率"></asp:BoundField>
                            <asp:BoundField DataField="FareMDiscount" SortExpression="FareMDiscount"
                                HeaderText="中車折扣費率"></asp:BoundField>
                            <asp:BoundField DataField="FareGDiscount" SortExpression="FareGDiscount"
                                HeaderText="大車折扣費率"></asp:BoundField>
                            <asp:BoundField DataField="FreeMiles" SortExpression="FreeMiles"
                                HeaderText="免費里程"></asp:BoundField>
                            <asp:BoundField DataField="DiscountMiles" SortExpression="DiscountMiles"
                                HeaderText="長途折扣里程"></asp:BoundField>
                            <asp:BoundField DataField="RateS2M" SortExpression="RateS2M"
                                HeaderText="中車費率倍數"></asp:BoundField>
                            <asp:BoundField DataField="RateS2G" SortExpression="RateS2G"
                                HeaderText="大車費率倍數"></asp:BoundField>
                            <asp:BoundField DataField="RateLongDistant" SortExpression="RateLongDistant"
                                HeaderText="長途折扣比率"></asp:BoundField>
                            <asp:BoundField DataField="RateETag" SortExpression="RateETag"
                                HeaderText="ETag折扣比率"></asp:BoundField>
                            <asp:BoundField DataField="RateReserved" SortExpression="RateReserved"
                                HeaderText="預約用戶折扣優惠比率"></asp:BoundField>
                            <asp:BoundField DataField="RateNoReserved" SortExpression="RateNoReserved"
                                HeaderText="無預儲帳戶限期內繳費折扣比率"></asp:BoundField>
                            <asp:BoundField DataField="ItemOrder" SortExpression="ItemOrder"
                                HeaderText="計畫排序"></asp:BoundField>
                            <asp:CheckBoxField DataField = "IsLongDistant" SortExpression = "IsLongDistant" HeaderText = "長途折扣" Text ="顯示" />
                            <asp:CheckBoxField DataField = "IsFreeMiles" SortExpression = "IsFreeMiles" HeaderText = "優惠里程" Text ="顯示" />
                            <asp:CheckBoxField DataField = "IsDifferent" SortExpression = "IsDifferent" HeaderText = "差別費率" Text ="顯示" />
                            <asp:CheckBoxField DataField = "IsAdd" SortExpression = "IsAdd" HeaderText = "加價費率" Text ="顯示" />
                            <asp:CheckBoxField DataField = "IsReserved" SortExpression = "IsReserved" HeaderText = "預儲帳戶" Text ="顯示" />
                            <asp:CheckBoxField DataField = "IsJustInTime" SortExpression = "IsJustInTime" HeaderText = "4到6日繳款" Text ="顯示" />
                            <asp:CheckBoxField DataField = "IsTaxi" SortExpression = "IsTaxi" HeaderText = "計程車僅顯示通行費" Text ="啟用" />
                            <asp:CheckBoxField DataField = "IsOnline" SortExpression = "IsOnline" HeaderText = "啟用" />
                            <asp:TemplateField SortExpression="Notes" HeaderText="備註">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtNotes" runat="server" TextMode ="MultiLine" Rows="4" Columns="40" ></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblNotes" runat="server" ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="費率表" >
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlFareListProject" runat="server" DataTextField="FareName" DataValueField ="UID"  DataSourceID="sdsFareList" AppendDataBoundItems="true">
                                        
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="BtnFare" runat="server"  CausesValidation="False" CommandName="BaseFare"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemTemplate>
                                    <asp:Button ID="BtnPreview" runat="server" Text="預覽" CausesValidation="False" CommandName="ViewProject">
                                    </asp:Button>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>

            </asp:View>
            <asp:View ID="View2" runat="server">
                        <table>
                            <tr>
                                <td>
                                    *計畫名稱：</td>
                                <td>
                                    <asp:TextBox ID="txtCName" runat="server" Text="" ValidationGroup="2"></asp:TextBox></td>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCName"
                                        ErrorMessage="請輸入計畫名稱！" ValidationGroup="2"></asp:RequiredFieldValidator></td>
                            </tr>
                            <tr>
                                <td>
                                    *選擇費率表：</td>
                                <td>
                                    <asp:DropDownList ID="ddlFareListProject" runat="server" DataTextField="FareName" DataValueField ="UID" DataSourceID="sdsFareList" >
                                        
                                    </asp:DropDownList>
                                </td><td></td>
                            </tr>
                            <tr>
                                <td>
                                    *母計畫：</td>
                                <td>
                                    <asp:DropDownList ID="ddlFareList" runat="server" DataTextField="ProjectName" DataValueField ="UID" DataSourceID="sdsProject" AppendDataBoundItems="true">
                                        <asp:ListItem Text="無" Value="0" Selected="True"></asp:ListItem>
                                    </asp:DropDownList>
                                </td><td></td>
                            </tr>
                            <tr>
                                <td>
                                    *啟用日期：</td>
                                <td>
                                    <ew:CalendarPopup ID="CalendarPopupStartDate" runat="server" CssClass="txt40" Culture="Chinese (Taiwan)"
                                        DisplayOffsetY="-100" PostedDate="" UpperBoundDate="12/31/9999 23:59:59" DayNameFormat="Full" ShowGoToToday="True">
                                    </ew:CalendarPopup>
                                </td><td></td>
                            </tr>
                            <tr>
                                <td>
                                    *截止日期：</td>
                                <td>
                                    <ew:CalendarPopup ID="CalendarPopupEndDate" runat="server" CssClass="txt40" Culture="Chinese (Taiwan)"
                                        DisplayOffsetY="-100" PostedDate="" UpperBoundDate="12/31/9999 23:59:59" DayNameFormat="Full" ShowGoToToday="True">
                                    </ew:CalendarPopup>
                                </td><td></td>
                            </tr>
                            <tr>
                                <td>
                                    *適用時段：</td>
                                <td>自
                                    <asp:DropDownList ID="ddlStartHour" runat="server">
                                        <asp:ListItem Text="0" Value="0" Selected="True" ></asp:ListItem>
                                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                        <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                        <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                        <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                        <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                        <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                        <asp:ListItem Text="13" Value="13"></asp:ListItem>
                                        <asp:ListItem Text="14" Value="14"></asp:ListItem>
                                        <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                        <asp:ListItem Text="16" Value="16"></asp:ListItem>
                                        <asp:ListItem Text="17" Value="17"></asp:ListItem>
                                        <asp:ListItem Text="18" Value="18"></asp:ListItem>
                                        <asp:ListItem Text="19" Value="19"></asp:ListItem>
                                        <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                        <asp:ListItem Text="21" Value="21"></asp:ListItem>
                                        <asp:ListItem Text="22" Value="22"></asp:ListItem>
                                        <asp:ListItem Text="23" Value="23"></asp:ListItem>
                                        <asp:ListItem Text="24" Value="24"></asp:ListItem>
                                    </asp:DropDownList>時至
                                    <asp:DropDownList ID="ddlEndHour" runat="server">
                                        <asp:ListItem Text="0" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                        <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                        <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                        <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                        <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                        <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                        <asp:ListItem Text="13" Value="13"></asp:ListItem>
                                        <asp:ListItem Text="14" Value="14"></asp:ListItem>
                                        <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                        <asp:ListItem Text="16" Value="16"></asp:ListItem>
                                        <asp:ListItem Text="17" Value="17"></asp:ListItem>
                                        <asp:ListItem Text="18" Value="18"></asp:ListItem>
                                        <asp:ListItem Text="19" Value="19"></asp:ListItem>
                                        <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                        <asp:ListItem Text="21" Value="21"></asp:ListItem>
                                        <asp:ListItem Text="22" Value="22"></asp:ListItem>
                                        <asp:ListItem Text="23" Value="23"></asp:ListItem>
                                        <asp:ListItem Text="24" Value="24" Selected="True" ></asp:ListItem>
                                    </asp:DropDownList>時
                                </td><td></td>
                            </tr>
                            <tr>
                                <td>
                                    是否免費：</td>
                                <td>
                                    <asp:CheckBox ID="cbIsFree" runat="server" Text="暫停收費時段"  ValidationGroup="2"/></td>
                                <td></td>

                            </tr>
                            <tr>
                                <td>
                                    *牌價費率：</td>
                                <td>
                                    小車費率<asp:TextBox ID="txtFareS" runat="server" Text="0"></asp:TextBox><br />
                                    中車費率<asp:TextBox ID="txtFareM" runat="server" Text="0"></asp:TextBox><br />
                                    大車費率<asp:TextBox ID="txtFareG" runat="server" Text="0"></asp:TextBox><br />
                                </td>
                                <td>
                                    <asp:CompareValidator id="CompareValidator2" runat="server" errormessage="請填寫正確的數字格式" display="Dynamic" controltovalidate="txtFareS"
                                    operator="DataTypeCheck" type="Double"></asp:CompareValidator>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtFareS"
                                        ErrorMessage="請輸入小車費率！" ValidationGroup="2"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator id="CompareValidator1" runat="server" errormessage="請填寫正確的數字格式" display="Dynamic" controltovalidate="txtFareM"
                                    operator="DataTypeCheck" type="Double"></asp:CompareValidator>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtFareM"
                                        ErrorMessage="請輸入中車費率！" ValidationGroup="2"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator id="CompareValidator4" runat="server" errormessage="請填寫正確的數字格式" display="Dynamic" controltovalidate="txtFareG"
                                    operator="DataTypeCheck" type="Double"></asp:CompareValidator>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtFareG"
                                        ErrorMessage="請輸入大車費率！" ValidationGroup="2"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    *長途折扣費率：</td>
                                <td>
                                    小車費率<asp:TextBox ID="txtFareSDiscount" runat="server" Text="0"></asp:TextBox><br />
                                    中車費率<asp:TextBox ID="txtFareMDiscount" runat="server" Text="0"></asp:TextBox><br />
                                    大車費率<asp:TextBox ID="txtFareGDiscount" runat="server" Text="0"></asp:TextBox><br />
                                </td>
                                <td>
                                    <asp:CompareValidator id="CompareValidator5" runat="server" errormessage="請填寫正確的數字格式" display="Dynamic" controltovalidate="txtFareSDiscount"
                                    operator="DataTypeCheck" type="Double"></asp:CompareValidator>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtFareSDiscount"
                                        ErrorMessage="請輸入小車費率！" ValidationGroup="2"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator id="CompareValidator6" runat="server" errormessage="請填寫正確的數字格式" display="Dynamic" controltovalidate="txtFareMDiscount"
                                    operator="DataTypeCheck" type="Double"></asp:CompareValidator>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtFareMDiscount"
                                        ErrorMessage="請輸入中車費率！" ValidationGroup="2"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator id="CompareValidator7" runat="server" errormessage="請填寫正確的數字格式" display="Dynamic" controltovalidate="txtFareGDiscount"
                                    operator="DataTypeCheck" type="Double"></asp:CompareValidator>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtFareGDiscount"
                                        ErrorMessage="請輸入大車費率！" ValidationGroup="2"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    *免費里程：</td>
                                <td>
                                    <asp:TextBox ID="txtFreeMiles" runat="server" Text="1"></asp:TextBox></td>
                                <td>
                                    <asp:CompareValidator id="CompareValidator3" runat="server" errormessage="請填寫正確的數字格式" display="Dynamic" controltovalidate="txtFreeMiles"
                                    operator="DataTypeCheck" type="Double"></asp:CompareValidator>                                    
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtFreeMiles"
                                        ErrorMessage="請輸入免費里程！" ValidationGroup="2"></asp:RequiredFieldValidator></td>
                            </tr>
                            <tr>
                                <td>
                                    *長途優惠里程：</td>
                                <td>
                                    <asp:TextBox ID="txtDiscountMiles" runat="server" Text="1"></asp:TextBox></td>
                                <td>
                                    <asp:CompareValidator id="CompareValidator13" runat="server" errormessage="請填寫正確的數字格式" display="Dynamic" controltovalidate="txtDiscountMiles"
                                    operator="DataTypeCheck" type="Double"></asp:CompareValidator>                                    
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="txtDiscountMiles"
                                        ErrorMessage="請輸入長途優惠里程！" ValidationGroup="2"></asp:RequiredFieldValidator></td>
                            </tr>
                            <tr>
                                <td>
                                    *中車為小車費率倍數：</td>
                                <td>
                                    <asp:TextBox ID="txtRateS2M" runat="server" Text="1"></asp:TextBox></td>
                                <td>
                                    <asp:CompareValidator id="CompareValidator8" runat="server" errormessage="請填寫正確的數字格式" display="Dynamic" controltovalidate="txtRateS2M"
                                    operator="DataTypeCheck" type="Double"></asp:CompareValidator>                                    
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtRateS2M"
                                        ErrorMessage="請輸入中車為小車費率倍數！" ValidationGroup="2"></asp:RequiredFieldValidator></td>
                            </tr>
                            <tr>
                                <td>
                                    *大車為小車費率倍數：</td>
                                <td>
                                    <asp:TextBox ID="txtRateS2G" runat="server" Text="1"></asp:TextBox></td>
                                <td>
                                    <asp:CompareValidator id="CompareValidator10" runat="server" errormessage="請填寫正確的數字格式" display="Dynamic" controltovalidate="txtRateS2G"
                                    operator="DataTypeCheck" type="Double"></asp:CompareValidator>                                    
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="txtRateS2G"
                                        ErrorMessage="請輸入大車為小車費率倍數！" ValidationGroup="2"></asp:RequiredFieldValidator></td>
                            </tr>
                            <tr>
                                <td>
                                    *長途折扣折扣比率：</td>
                                <td>
                                    <asp:TextBox ID="txtRateLongDistant" runat="server" Text="1"></asp:TextBox></td>
                                <td>
                                    <asp:CompareValidator id="CompareValidator11" runat="server" errormessage="請填寫正確的數字格式" display="Dynamic" controltovalidate="txtRateLongDistant"
                                    operator="DataTypeCheck" type="Double"></asp:CompareValidator>                                    
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="txtRateLongDistant"
                                        ErrorMessage="請輸入長途折扣折扣比率！" ValidationGroup="2"></asp:RequiredFieldValidator></td>
                            </tr>
                            <tr>
                                <td>
                                    *ETag用戶折扣比率：</td>
                                <td>
                                    <asp:TextBox ID="txtRateETag" runat="server" Text="1"></asp:TextBox></td>
                                <td>
                                    <asp:CompareValidator id="CompareValidator12" runat="server" errormessage="請填寫正確的數字格式" display="Dynamic" controltovalidate="txtRateETag"
                                    operator="DataTypeCheck" type="Double"></asp:CompareValidator>                                    
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="txtRateETag"
                                        ErrorMessage="請輸入ETag用戶折扣比率！" ValidationGroup="2"></asp:RequiredFieldValidator></td>
                            </tr>
                            <tr>
                                <td>
                                    *預約用戶折扣優惠比率：</td>
                                <td>
                                    <asp:TextBox ID="txtRateReserved" runat="server" Text="1"></asp:TextBox></td>
                                <td>
                                    <asp:CompareValidator id="CompareValidator9" runat="server" errormessage="請填寫正確的數字格式" display="Dynamic" controltovalidate="txtRateReserved"
                                    operator="DataTypeCheck" type="Double"></asp:CompareValidator>                                    
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txtRateReserved"
                                        ErrorMessage="請輸入預儲帳戶折扣優惠比率！" ValidationGroup="2"></asp:RequiredFieldValidator></td>
                            </tr>
                            <tr>
                                <td>
                                    *無預儲帳戶限期內繳費折扣優惠比率：</td>
                                <td>
                                    <asp:TextBox ID="txtRateNoReserved" runat="server" Text="1"></asp:TextBox></td>
                                <td>
                                    <asp:CompareValidator id="CompareValidator14" runat="server" errormessage="請填寫正確的數字格式" display="Dynamic" controltovalidate="txtRateNoReserved"
                                    operator="DataTypeCheck" type="Double"></asp:CompareValidator>                                    
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="txtRateNoReserved"
                                        ErrorMessage="請輸入無預儲帳戶限期內繳費折扣優惠比率！" ValidationGroup="2"></asp:RequiredFieldValidator></td>
                            </tr>
                            <tr>
                                <td>
                                    *計畫排序：</td>
                                <td>
                                    <asp:TextBox ID="txtItemOrder" runat="server" Text="1"></asp:TextBox></td>
                                <td>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="txtItemOrder" ErrorMessage="請填寫正確的數字格式" ValidationExpression="[0-9]" ValidationGroup="2"></asp:RegularExpressionValidator>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtItemOrder"
                                        ErrorMessage="請輸入排序！" ValidationGroup="2"></asp:RequiredFieldValidator></td>
                            </tr>
                            <tr>
                                <td>
                                    顯示長途折扣並加入計算：</td>
                                <td>
                                    <asp:CheckBox ID="cbIsLongDistant" runat="server" Text="長途折扣" Checked = "true" ValidationGroup="2"/></td>
                                <td></td>

                            </tr>
                            <tr>
                                <td>
                                    顯示免費里程並加入計算：</td>
                                <td>
                                    <asp:CheckBox ID="cbIsFreeMiles" runat="server" Text="免費里程" Checked = "true" ValidationGroup="2"/></td>
                                <td></td>

                            </tr>
                            <tr>
                                <td>
                                    顯示差別費率並加入計算：</td>
                                <td>
                                    <asp:CheckBox ID="cbIsDifferent" runat="server" Text="差別費率" Checked = "true" ValidationGroup="2"/></td>
                                <td></td>

                            </tr>
                            <tr>
                                <td>
                                    顯示加價費率並加入計算：</td>
                                <td>
                                    <asp:CheckBox ID="cbIsAdd" runat="server" Text="加價費率" Checked = "true" ValidationGroup="2"/></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>
                                    顯示eTag用戶折扣並加入計算：</td>
                                <td>
                                    <asp:CheckBox ID="cbIsETag" runat="server" Text="eTag帳戶" Checked = "true" ValidationGroup="2"/></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>
                                    顯示預約用戶折扣並加入計算：</td>
                                <td>
                                    <asp:CheckBox ID="cbIsReserved" runat="server" Text="預約用戶" Checked = "true" ValidationGroup="2"/></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>
                                    顯示4到6日繳款折扣並加入計算：</td>
                                <td>
                                    <asp:CheckBox ID="cbIsJustInTime" runat="server" Text="4到6日繳款" Checked = "true" ValidationGroup="2"/></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>
                                    計程車僅顯示通行費：</td>
                                <td>
                                    <asp:CheckBox ID="cbIsTaxi" runat="server" Text="計程車僅顯示通行費" Checked = "true" ValidationGroup="2"/></td>
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
                                    *備註：</td>
                                <td>
                                    <asp:TextBox ID="txtNotes" runat="server" Text="" TextMode ="MultiLine" Rows="8" Columns="60" ></asp:TextBox></td>
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
                    <asp:SqlDataSource ID="sdsProject" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                        SelectCommand="SELECT UID, ProjectName from InterChangeFareProject order by itemorder">
                        <SelectParameters>
                        </SelectParameters>
                    </asp:SqlDataSource>
                    <asp:SqlDataSource ID="sdsFareList" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                        SelectCommand="SELECT UID, FareName from InterChangeFareListProject where IsOnline > 0  order by itemorder">
                        <SelectParameters>
                        </SelectParameters>
                    </asp:SqlDataSource>

                     <asp:SqlDataSource ID="sdsInterChangeFareProject" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                        DeleteCommand="DELETE FROM InterChangeFareProject WHERE [UID] = @UID"
                        InsertCommand="InterChangeFareProject_Add" InsertCommandType ="StoredProcedure" 
                        UpdateCommand="InterChangeFareProject_Update" UpdateCommandType="StoredProcedure" 
                        SelectCommand="Select *,(Select Top 1 ProjectName from InterChangeFareProject f where f.UID = p.ProjectAlias) as aProjectName, (Select Top 1 FareName from InterChangeFareListProject f where f.UID = FareListID) as FareName  from InterChangeFareProject p where ProjectName LIKE N'%' + @ProjectName + N'%' order by StartDate Desc, EndDate desc, ItemOrder" SelectCommandType="Text">
                        <FilterParameters>
                            <asp:ControlParameter Name="ProjectName" ControlID="txtSearch" PropertyName="Text" />
                        </FilterParameters>
                        <DeleteParameters>
                            <asp:Parameter Type="Int32" Name="UID"></asp:Parameter>
                        </DeleteParameters>
                        <InsertParameters>
                            <asp:ControlParameter Name="ProjectName" ControlID="txtCName" PropertyName="Text" />
                            <asp:ControlParameter Name="ProjectAlias" ControlID="ddlFareList" PropertyName="SelectedValue" />
                            <asp:ControlParameter Name="FareListID" ControlID="ddlFareListProject" PropertyName="SelectedValue" />
                            <asp:ControlParameter Name="StartDate" ControlID="CalendarPopupStartDate" PropertyName="SelectedDate" />
                            <asp:ControlParameter Name="EndDate" ControlID="CalendarPopupEndDate" PropertyName="SelectedDate" />
                            <asp:ControlParameter Name="StartHour" ControlID="ddlStartHour" PropertyName="SelectedValue" />
                            <asp:ControlParameter Name="EndHour" ControlID="ddlEndHour" PropertyName="SelectedValue" />
                            <asp:ControlParameter Name="FareS" ControlID="txtFareS" PropertyName="Text" />
                            <asp:ControlParameter Name="FareM" ControlID="txtFareM" PropertyName="Text" />
                            <asp:ControlParameter Name="FareG" ControlID="txtFareG" PropertyName="Text" />
                            <asp:ControlParameter Name="FareSDiscount" ControlID="txtFareSDiscount" PropertyName="Text" />
                            <asp:ControlParameter Name="FareMDiscount" ControlID="txtFareMDiscount" PropertyName="Text" />
                            <asp:ControlParameter Name="FareGDiscount" ControlID="txtFareGDiscount" PropertyName="Text" />
                            <asp:ControlParameter Name="FreeMiles" ControlID="txtFreeMiles" PropertyName="Text" />
                            <asp:ControlParameter Name="DiscountMiles" ControlID="txtDiscountMiles" PropertyName="Text" />
                            <asp:ControlParameter Name="RateS2M" ControlID="txtRateS2M" PropertyName="Text" />
                            <asp:ControlParameter Name="RateS2G" ControlID="txtRateS2G" PropertyName="Text" />
                            <asp:ControlParameter Name="RateLongDistant" ControlID="txtRateLongDistant" PropertyName="Text" />
                            <asp:ControlParameter Name="RateETag" ControlID="txtRateETag" PropertyName="Text" />
                            <asp:ControlParameter Name="RateReserved" ControlID="txtRateReserved" PropertyName="Text" />
                            <asp:ControlParameter Name="RateNoReserved" ControlID="txtRateNoReserved" PropertyName="Text" />
                            <asp:ControlParameter Name="ItemOrder" ControlID="txtItemOrder" PropertyName="Text" />
                            <asp:ControlParameter Name="IsFree" ControlID="cbIsFree" PropertyName="Checked" />
                            <asp:ControlParameter Name="IsLongDistant" ControlID="cbIsLongDistant" PropertyName="Checked" />
                            <asp:ControlParameter Name="IsFreeMiles" ControlID="cbIsFreeMiles" PropertyName="Checked" />
                            <asp:ControlParameter Name="IsDifferent" ControlID="cbIsDifferent" PropertyName="Checked" />
                            <asp:ControlParameter Name="IsAdd" ControlID="cbIsAdd" PropertyName="Checked" />
                            <asp:ControlParameter Name="IsETag" ControlID="cbIsEtag" PropertyName="Checked" />
                            <asp:ControlParameter Name="IsReserved" ControlID="cbIsReserved" PropertyName="Checked" />
                            <asp:ControlParameter Name="IsJustInTime" ControlID="cbIsJustInTime" PropertyName="Checked" />
                            <asp:ControlParameter Name="IsTaxi" ControlID="cbIsTaxi" PropertyName="Checked" />
                            <asp:ControlParameter Name="IsOnline" ControlID="cbIsOnline" PropertyName="Checked" />
                            <asp:ControlParameter Name="Notes" ControlID="txtNotes" PropertyName="Text" />
                            <asp:Parameter Name="retVal" Direction="Output" Type="Int32" />
                        </InsertParameters>
                        <UpdateParameters>
                             <asp:Parameter Name="UID"  Type="Int32" />
                             <asp:ControlParameter Name="ProjectName" ControlID="txtCName" PropertyName="Text" />
                            <asp:ControlParameter Name="ProjectAlias" ControlID="ddlFareList" PropertyName="SelectedValue" />
                            <asp:ControlParameter Name="FareListID" ControlID="ddlFareListProject" PropertyName="SelectedValue" />
                            <asp:ControlParameter Name="StartDate" ControlID="CalendarPopupStartDate" PropertyName="SelectedDate" />
                            <asp:ControlParameter Name="EndDate" ControlID="CalendarPopupEndDate" PropertyName="SelectedDate" />
                            <asp:ControlParameter Name="StartHour" ControlID="ddlStartHour" PropertyName="SelectedValue" />
                            <asp:ControlParameter Name="EndHour" ControlID="ddlEndHour" PropertyName="SelectedValue" />
                            <asp:ControlParameter Name="FareS" ControlID="txtFareS" PropertyName="Text" />
                            <asp:ControlParameter Name="FareM" ControlID="txtFareM" PropertyName="Text" />
                            <asp:ControlParameter Name="FareG" ControlID="txtFareG" PropertyName="Text" />
                            <asp:ControlParameter Name="FareSDiscount" ControlID="txtFareSDiscount" PropertyName="Text" />
                            <asp:ControlParameter Name="FareMDiscount" ControlID="txtFareMDiscount" PropertyName="Text" />
                            <asp:ControlParameter Name="FareGDiscount" ControlID="txtFareGDiscount" PropertyName="Text" />
                            <asp:ControlParameter Name="FreeMiles" ControlID="txtFreeMiles" PropertyName="Text" />
                            <asp:ControlParameter Name="DiscountMiles" ControlID="txtDiscountMiles" PropertyName="Text" />
                            <asp:ControlParameter Name="RateS2M" ControlID="txtRateS2M" PropertyName="Text" />
                            <asp:ControlParameter Name="RateS2G" ControlID="txtRateS2G" PropertyName="Text" />
                            <asp:ControlParameter Name="RateLongDistant" ControlID="txtRateLongDistant" PropertyName="Text" />
                            <asp:ControlParameter Name="RateETag" ControlID="txtRateETag" PropertyName="Text" />
                            <asp:ControlParameter Name="RateReserved" ControlID="txtRateReserved" PropertyName="Text" />
                            <asp:ControlParameter Name="RateNoReserved" ControlID="txtRateNoReserved" PropertyName="Text" />
                            <asp:ControlParameter Name="ItemOrder" ControlID="txtItemOrder" PropertyName="Text" />
                            <asp:ControlParameter Name="IsFree" ControlID="cbIsFree" PropertyName="Checked" />
                            <asp:ControlParameter Name="IsLongDistant" ControlID="cbIsLongDistant" PropertyName="Checked" />
                            <asp:ControlParameter Name="IsFreeMiles" ControlID="cbIsFreeMiles" PropertyName="Checked" />
                            <asp:ControlParameter Name="IsDifferent" ControlID="cbIsDifferent" PropertyName="Checked" />
                            <asp:ControlParameter Name="IsAdd" ControlID="cbIsAdd" PropertyName="Checked" />
                            <asp:ControlParameter Name="IsETag" ControlID="cbIsEtag" PropertyName="Checked" />
                            <asp:ControlParameter Name="IsReserved" ControlID="cbIsReserved" PropertyName="Checked" />
                            <asp:ControlParameter Name="IsJustInTime" ControlID="cbIsJustInTime" PropertyName="Checked" />
                            <asp:ControlParameter Name="IsTaxi" ControlID="cbIsTaxi" PropertyName="Checked" />
                            <asp:ControlParameter Name="IsOnline" ControlID="cbIsOnline" PropertyName="Checked" />
                            <asp:ControlParameter Name="Notes" ControlID="txtNotes" PropertyName="Text" />
                            <asp:Parameter Name="retVal" Direction="Output" Type="Int32" />
                        </UpdateParameters>
                        <SelectParameters>
                            <asp:ControlParameter Name="ProjectName" ControlID="txtSearch" PropertyName="Text" DefaultValue="%"/>
                        </SelectParameters>
                    </asp:SqlDataSource>

</asp:Content> 