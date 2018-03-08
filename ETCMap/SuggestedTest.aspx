<%@ Page Language="VB" MasterPageFile="~/MasterPageFare.master" AutoEventWireup="false" CodeFile="SuggestedTest.aspx.vb" Inherits="_SuggestedTest" 
    EnableEventValidation="false" EnableViewStateMac="false" EnableViewState="false"  ViewStateEncryptionMode="Never"%>
<%@ Register Src="common/PublishBlock.ascx" TagName="PublishBlock" TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHolder1" Runat="Server">
 
    <!-- 左區塊開始 -->
    <asp:HiddenField ID="hidd1" runat="server" />
<tr>
    <td align="left" valign="top" width="16px" background="images/box_04.png" width="16px">&nbsp;</td>
	<td align="left" valign="top" background="images/box_05.png">
        <p class="title18">試算通行費3步驟： 1.選擇車種  2.選擇繳費方式  3.選擇行程</p>
    <table width="100%" border="0">
	    <tr>
		    <td colspan="3" align="left" valign="top">
                <uc5:PublishBlock ID="PublishBlock1" runat="server" BlockID="SuggestNote" />
		    </td>
        </tr>
        <tr>
		    <td colspan="3" align="left" valign="middle">
                <p>
                <asp:UpdatePanel ID="UpdatePanelDate" runat="server" UpdateMode="Conditional" >
                    <ContentTemplate>
                            
                    </ContentTemplate>
                </asp:UpdatePanel>
                        <img src="images/icon_dot_Green2.png" alt=" " width="14" height="14" align="absmiddle" />&nbsp;
                    選擇通行日期：<asp:TextBox ID="datePicker" runat="server" OnTextChanged="TextChanged"/>&nbsp;
                    <img src="images/Calendar.gif" alt="Calendar" width="30" height="30" border="0" align="absmiddle" onclick="document.getElementById('<%=datePicker.ClientID %>').focus()" />&nbsp
                    時段：<asp:DropDownList ID="ddlTimePediod" runat="server" DataTextField="TimePeriod" DataValueField ="ProjectUID"  ></asp:DropDownList>
                 <p>
                    <asp:Literal ID="ltlProjectInform" runat="server"></asp:Literal>
                </p>

	            </p>
           </td>
        </tr>
        <tr>
		    <td align="left" valign="middle" nowrap="nowrap" style="width: 20%"><p><img src="images/step1.png" alt="步驟1." width="80" height="33" align="absmiddle" />選擇車種</p></td>
            <td width="80%" align="left" valign="middle" colspan="2">  
                <select name="select" id="CarType" runat="server" align="absmiddle" >
                    <option selected="selected" value="1">小型車</option>
                    <option value="2">計程車(乘客用)</option>
                    <option value="3">大型車</option>
                    <option value="4">聯結車</option>
                </select>
            </td>
        </tr>
        <tr>
		    <td width="20%" align="left" valign="middle" nowrap="nowrap"><p><img src="images/step2.png" alt="步驟2." width="80" height="33" align="absmiddle" />選擇繳費方式</p></td>
		    <td width="80%" align="left" valign="middle" colspan="2">
                <select name="FareType" id="FareType" runat="server" align="absmiddle" >
                    <option value="1" selected="selected" >eTag用戶</option>
                    <option value="2">預約用戶</option>
                    <option value="3">繳費用戶(未申辦eTag)</option>
                </select>
		    </td>
        </tr>
              <tr>
		      <td width="19%" align="left" valign="middle" nowrap="nowrap"><p><img src="images/step3.png" alt="步驟3." width="80" height="33" align="absmiddle" />選擇行程</p></td>
		      <td width="30%" align="left" valign="middle"><p><strong>交流道<span class="color025d00">(起點)</span></strong></p></td>
		      <td width="60%" align="left" valign="middle"><p><strong>交流道<span class="color025d00">(訖點)</span></strong></p></td>
		      </tr>
<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
<ContentTemplate>
              <tr>
                <td width="19%" align="left" valign="top" nowrap="nowrap">&nbsp;</td>
                <td align="left" valign="middle">
                    <asp:TextBox ID="txtFrom" runat="server" Text = "請輸入交流道名稱"  Columns ="32" MaxLength ="30"></asp:TextBox>
                </td>
                <td align="left" valign="middle">
                    <asp:TextBox ID="txtTo" runat="server" Text = "請輸入交流道名稱"  Columns ="32" MaxLength ="30"></asp:TextBox>
                </td>
              </tr>
</ContentTemplate>
</asp:UpdatePanel>
 
        <tr>
            <td colspan="3" align="center" valign="top">
                <p><asp:ImageButton ID="imgCalculate" runat="server" ImageUrl = "images/btn_star.jpg" BorderWidth="0" width="204px" Height="57px" ToolTip="開始計算!"  onmouseover="this.src='images/btn_star_a.jpg'" onfocus="this.src='images/btn_star.jpg'" onmouseout="this.src='images/btn_star.jpg'" onblur="this.src='images/btn_star_a.jpg'"/></p>
            </td>
        </tr>
    </table>
    <div class="line"></div>
	<table width="100%" border="0">
        <tr>
            <td width="1%" align="left" nowrap="nowrap"><p class="title">貼心小幫手</p></td>
            <td width="1%" align="left" nowrap="nowrap">若您不知道駕駛路線或交流道名稱可利用</td>
            <td width="1%" align="left" nowrap="nowrap"><a href="http://1968.freeway.gov.tw/" target="_blank"><img src="images/help_02.jpg" alt="國道1968" width="145" height="69" border="0" /></a></td>
            <td width="1%" align="left" nowrap="nowrap">或是</td>
            <td width="1%" align="left" nowrap="nowrap"><a href="https://maps.google.com.tw/maps?hl=zh-TW" target="_blank"><img src="images/help_04.jpg" alt="Google Map" width="146" height="69" border="0" /></a></td>
            <td width="72" align="left">查詢。</td>
        </tr>
    </table>
    <div class="line"></div>
    <a name="buttom">&nbsp;</a>
    <asp:UpdatePanel ID="UpdatePanel100" runat="server">
            <ContentTemplate >
                <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0" >
                    <asp:View ID="View1" runat="server">
                        <div id="printarea">
                            <table width="732" border="0" align="center" cellpadding="0" cellspacing="0" >
                                <t>
                                    <td colspan="2">
                                        <p class="title">試算結果</p>
                                        <p class="result"><img src="images/icon_dot_Green2.png" alt=" " width="14" height="14" align="absmiddle" />&nbsp;<asp:Literal ID="ltlProjectName" runat="server"></asp:Literal>
                                            通行日期：<font color=blue ><asp:Literal ID="ltlDate" runat="server"></asp:Literal></font>，時段：<font color=blue ><asp:Literal ID="ltlTimePeriod" runat="server"></asp:Literal></font>。</p>
                                        <p class="time">【各車種費率:小型車<asp:Literal ID="ltlFareS" runat="server"></asp:Literal>元/公里、大型車<asp:Literal ID="ltlFareM" runat="server"></asp:Literal>元/公里、聯結車<asp:Literal ID="ltlFareG" runat="server"></asp:Literal>元/公里】</p>
                                        <asp:Literal ID="ltlProjectNotes" runat="server"></asp:Literal>
                                    </td>
                                </t r>
                                <tr>
                                    <td><img src="images/testbox_02.jpg" width="367" height="38" alt="通行費用" /></td>
                                    <td><img src="images/testbox_03.jpg" width="365" height="38" alt="通行路徑" /></td>
                                </tr>
                                <tr>
                                    <td background="images/testbox_04.jpg" width="367"  valign="top"><table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="style2">
                                <tr>
                                    <td width="75%" align="left" valign="top">通行費</td>
                                    <td width="25%" align="right" valign="top" nowrap="nowrap">$<asp:Literal ID="ltlFare" runat="server"></asp:Literal><br /></td>
                                </tr>
                                <tr id="trLongDistant" runat="server">
                                    <td align="left" valign="top">減:<span class="Fd934">長途折扣 </span></td>
                                    <td align="right" valign="top" nowrap="nowrap"><font color="red">-$<asp:Literal ID="ltlDiscount" runat="server"></asp:Literal></font></td>
                                </tr>
                                <tr id="trFreeMiles" runat="server">
                                    <td align="left" valign="top">減:<span class="Fd934">優惠里程</span></td>
                                    <td align="right" valign="top" nowrap="nowrap"><font color="red">-$<asp:Literal ID="ltlFreeFare" runat="server"></asp:Literal></font></td>
                                </tr>
                                <tr id="trFareDiff" runat="server">
                                    <td align="left" valign="top"><asp:Literal ID="ltlMinusTitle" runat="server" Text="減" ></asp:Literal>:<span class="Fd934">差別費率</span></td>
                                    <td align="right" valign="top" nowrap="nowrap" class="Fd934"><font color="red"><asp:Literal ID="ltlMinus" runat="server" Text="-" ></asp:Literal>$<asp:Literal ID="ltlDiff" runat="server" ></asp:Literal></font></td>
                                </tr>
                                <tr id="trFareAdd" runat="server">
                                    <td align="left" valign="top">加:<span class="Fd934"><asp:Literal ID="ltlAddName" runat="server" Text="差別費率"></asp:Literal></span></td>
                                    <td align="right" valign="top" nowrap="nowrap"><font color="navy">+$<asp:Literal ID="ltlAdd" runat="server"></asp:Literal></font></td>
                                </tr>
                                <tr align="left" id ="trSeperatorLine" runat="server">
                                    <td colspan="2" valign="top"><img src="images/line333.jpg" alt=" " width="330" height="10" border="0" /></td>
                                </tr>
                                <tr id="trTotal" runat="server">
                                    <td align="left" valign="top"><strong>合計<asp:Literal ID="ltlTotal" runat="server" Text="(四捨五入後)"></asp:Literal></strong></td>
                                    <td align="right" valign="top" nowrap="nowrap"><strong>$<asp:Literal ID="TotalFare" runat="server"></asp:Literal></strong></td>
                                </tr>
                                <tr id="trETag" runat="server">
                                    <td align="left" valign="top"><strong><asp:Label ID="lblIsETag" runat="server" CssClass ="Fd93401" Text = "預儲帳戶足餘額9折扣款"></asp:Label></strong></td>
                                    <td align="right" valign="top" nowrap="nowrap"><strong><span class="Fd93401">$<asp:Literal ID="ltlETag" runat="server"></asp:Literal></span></strong></td>
                                </tr>
                                <tr id="trReserved" runat="server">
                                    <td align="left" valign="top"><strong><asp:Label ID="lblReserve" runat="server" CssClass ="Fd93401" Text = "預約用戶帳戶足餘額xx折扣款"></asp:Label></strong></td>
                                    <td align="right" valign="top" nowrap="nowrap"><strong><span class="Fd93401">$<asp:Literal ID="ltlReserved" runat="server"></asp:Literal></span></strong></td>
                                </tr>
                                <tr id="tr46day" runat="server" visible="false" >
                                    <td align="left" valign="top"><strong><asp:Label ID="lbl46" runat="server" CssClass ="Fd93401" Text = "通行日起第4~6天主動繳費9折"></asp:Label></strong></td>
                                    <td align="right" valign="top" nowrap="nowrap"><strong><span class="Fd93401">$<asp:Literal ID="ltl46day" runat="server"></asp:Literal></span></strong></td>
                                </tr>
                                <tr id="tr7day" runat="server" visible="false" >
                                    <td align="left" valign="top"><strong><asp:Label ID="lbl7" runat="server" CssClass ="Fd93401" Text = "通行日起第7天之後繳費(無折扣)"></asp:Label></strong></td>
                                    <td align="right" valign="top" nowrap="nowrap"><strong><span class="Fd93401">$<asp:Literal ID="ltl7day" runat="server"></asp:Literal></span></strong></td>
                                </tr>
                            </table>
                            
                        </td>
                        <td width="365" valign="top" background="images/testbox_05.jpg">
                            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="style2">
                                <tr>
                                    <td align="left" valign="top">
                                        <asp:Literal ID="ltlRoadTrip" runat="server"></asp:Literal><br /><br />
                                        <font color=red>(註:通行費採對用路人較有利的『牌價法』計算，非以行駛里程乘以通行費率計算，請參考貼心提醒說明)</font>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td><img src="images/testbox_06.jpg" width="367" height="18" alt="" /></td>
                        <td><img src="images/testbox_07.jpg" width="365" height="18" alt="" /></td>
                    </tr>
                </table>
	                        <asp:HiddenField ID="hdFareNormal" runat="server" />
	                        <asp:HiddenField ID="hdFareDiscount" runat="server" />
	                        <asp:HiddenField ID="hdFreeFare" runat="server" />
	                        <asp:HiddenField ID="hdRoadTrip" runat="server" />
                            <p align="center"><image id="ibtPrint" runat="server" src = "images/btn_print.jpg"  Border="0" width="204px" Height="57px" ToolTip="列印試算結果!" onmouseover="this.src='images/btn_print_a.jpg'" onfocus="this.src='images/btn_print.jpg'" onmouseout="this.src='images/btn_print.jpg'" onblur="this.src='images/btn_print_a.jpg'" onclick="printPage()"/></p>
                            <p align="center"><asp:Literal ID="ltlSearchTime" runat="server" Visible="false" ></asp:Literal> </p>
                            <uc5:PublishBlock ID="PublishBlock2" runat="server" BlockID="SuggestRemind" />
                        </div>
                    </asp:View> 
                </asp:MultiView> 
            </ContentTemplate>
        </asp:UpdatePanel>

<!-- footer區塊結束 -->

        <asp:SqlDataSource ID="sdsRepeater" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
            SelectCommand="Select top 2 UID from InterChangeList">
            <SelectParameters>
            </SelectParameters>
        </asp:SqlDataSource>
 
    </td>
    <td align="left" valign="top" background="images/box_06.png" width="18px">&nbsp;</td>
</tr>
    <asp:HiddenField ID="availableTagst" runat="server" />
<script >
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

    var dataG = [<%= availableTagst.Value %>];
    $(document).ready(function () {
        EndRequestHandler();
    });
    function EndRequestHandler() {
        $.get("AutoCom.aspx?txt=" + encodeURIComponent('國道'),
          function (data, status) {
              dataG = data.split(",");
          });
        $("#<%=txtFrom.ClientID %>").autocomplete({
            open: function () {
                $(this).autocomplete('widget').css('z-index', 1000);
            },
            source: dataG
        });
        $("#<%=txtTo.ClientID%>").autocomplete({
            open: function () {
                $(this).autocomplete('widget').css('z-index', 1000);
            },
            source: dataG
        });
        $(function () {
            $("#<%=datePicker.ClientID%>").datepicker({
                        dateFormat: 'yy/mm/dd',
                        onSelect: function (selected, event) {
                            javascript: __doPostBack('<%=datePicker.ClientID.Replace("_", "$")%>', '');
                }
            });
        });
        $(function () {
            $("#<%=Me.imgCalculate.ClientID%>").click(function () {
                 $('#mask').css({ 'display': 'block' });
             });
             $('.mask').click(function () {
                 $('#mask').css({ 'display': 'none' });
             });
         });
    };
    function textChange(txt) {
        $.get("<%=PathManager.ApplicationUrl  %>AutoCom.aspx?txt=" + encodeURIComponent(txt.value),
                function (data, status) {
                    if (data != "") {
                        dataG = data.split(",");
                        $("#<%=txtFrom.ClientID %>").autocomplete({
                            open: function () {
                                $(this).autocomplete('widget').css('z-index', 1000);
                            },
                            source: dataG
                        });
                        $("#<%=txtTo.ClientID %>").autocomplete({
                            open: function () {
                                $(this).autocomplete('widget').css('z-index', 1000);
                            },
                            source: dataG
                        });
                    }
                });

          }

        </script>
    
 </asp:Content>
