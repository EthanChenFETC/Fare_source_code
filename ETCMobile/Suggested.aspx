<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Suggested.aspx.vb" Inherits="_Suggested"  
    EnableEventValidation="false" EnableViewStateMac="false" EnableViewState="false"  ViewStateEncryptionMode="Never"%>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<%@ Register Src="common/PublishBlock.ascx" TagName="PublishBlock" TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="content">
    <uc5:PublishBlock ID="PublishBlock1" runat="server" BlockID="MSuggest" />
    <asp:UpdatePanel ID="UpdatePanelDate" runat="server" UpdateMode="Conditional" >
       <ContentTemplate>
       </ContentTemplate>
   </asp:UpdatePanel>
           <img src="images/icon_dot_Green2.png" alt=" " width="14" height="14" align="absmiddle" />&nbsp;
                   選擇通行日期：<asp:TextBox ID="datePicker" runat="server" OnTextChanged="TextChanged"/>&nbsp;
                   <img src="images/Calendar.gif" alt="Calendar" width="30" height="30" border="0" align="absmiddle" onclick="document.getElementById('<%=datePicker.ClientID %>').focus()" />&nbsp
                   時段：<asp:DropDownList ID="ddlTimePediod" runat="server" DataTextField="TimePeriod" DataValueField ="ProjectUID"  ></asp:DropDownList>

   <ul>
       <li class="step_01">選擇車種
           <select name="select" id="CarType" runat="server" align="absmiddle" class="select_way">
               <option selected="selected" value="1">小型車</option>
               <option value="2">計程車(乘客用)</option>
               <option value="3">大型車</option>
               <option value="4">聯結車</option>
           </select>
       </li>
       <li class="step_02">繳費方式
            <select name="FareType" id="FareType" runat="server" align="absmiddle" class="select_way">
                <option value="1" selected="selected" >eTag用戶</option>
                <option value="2">預約用戶</option>
                <option value="3">繳費用戶(未申辦eTag)</option>
            </select>
        </li>
        <li class="step_03">選擇行程</li>
    </ul>
    <asp:HiddenField ID = "hidd1" runat="server" Value = "" />
<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
<ContentTemplate>
                <div class="txt_area">
                    <h1>交流道<em>(起點)</em></h1>
                    <asp:TextBox ID="txtFrom" runat="server" Text = "請輸入交流道名稱"  Columns ="32" MaxLength ="30" class="txtbox" ></asp:TextBox>
                </div>
                <div class="txt_area">
                    <h1>交流道<em>(訖點)</em></h1>
                    <asp:TextBox ID="txtTo" runat="server" Text = "請輸入交流道名稱"  Columns ="32" MaxLength ="30" class="txtbox" ></asp:TextBox>
                </div>
</ContentTemplate>
</asp:UpdatePanel>
    <div class="btn_start"><a href="#" onclick="javascript:__doPostBack('ctl00$ContentPlaceHolder1$imgCalculate','');return false;"></a></div>
<asp:ImageButton ID="imgCalculate" runat="server" ImageUrl = "images/btn_star.jpg" BorderWidth="0" width="204px" Height="57px" ToolTip="開始計算!"  onmouseover="this.src='images/btn_star_a.jpg'" onfocus="this.src='images/btn_star.jpg'" onmouseout="this.src='images/btn_star.jpg'" onblur="this.src='images/btn_star_a.jpg'" Visible="false"/> 
    <uc5:PublishBlock ID="PublishBlock2" runat="server" BlockID="MHelpSug" />
     <a name="buttom">&nbsp;</a>
    <div id="result" runat="server">
        <h1>試算結果</h1>
        <p class="result"><img src="imgs/icon_dot_Green2.png" alt=" " width="14" height="14" align="absmiddle" />
            <asp:Literal ID="ltlProjectName" runat="server"/>&nbsp;通行日期：<font color=blue ><asp:Literal ID="ltlDate" runat="server"></asp:Literal></font>，時段：<font color=blue ><asp:Literal ID="ltlTimePeriod" runat="server"></asp:Literal></font>。
        </p>
        <p class="time">【各車種費率:小型車<asp:Literal ID="ltlFareS" runat="server"/>元/公里、大型車<asp:Literal ID="ltlFareM" runat="server"></asp:Literal>元/公里、聯結車<asp:Literal ID="ltlFareG" runat="server"></asp:Literal>元/公里】</p>
        <asp:Literal ID="ltlProjectNotes" runat="server"></asp:Literal>
                    <div class="result_area">
                    <h1 class="result_title_price"></h1>
                    <div class="result_table">
                    <div class="result_table_list">通行費牌價</div>
                    <div class="result_table_nums">$<asp:Literal ID="ltlFare" runat="server"></asp:Literal></div>
                    </div>
                    <div class="result_table" id="trLongDistant" runat="server">
                    <div class="result_table_list">減:<span class="Fd934">長途折扣 </span></em></div>
                    <div class="result_table_nums"><font color="red">-$<asp:Literal ID="ltlDiscount" runat="server"></asp:Literal></font></div>
                    </div>
                    <div class="result_table" id="trFreeMiles" runat="server">
                    <div class="result_table_list">減:<span class="Fd934">優惠里程</span></div>
                    <div class="result_table_nums"><font color="red">-$<asp:Literal ID="ltlFreeFare" runat="server"/></font></div>
                    </div>
                    <div class="result_table" id="trFareDiff" runat="server">
                    <div class="result_table_list"><asp:Literal ID="ltlMinusTitle" runat="server" Text="減" ></asp:Literal>:<span class="Fd934">差別費率</span></div>
                    <div class="result_table_nums"><font color="red"><asp:Literal ID="ltlMinus" runat="server" Text="-" ></asp:Literal>$<asp:Literal ID="ltlDiff" runat="server" ></asp:Literal></font></div>
                    </div>
                    <div class="result_table" id="trFareAdd" runat="server">
                    <div class="result_table_list">加:<span class="Fd934"><asp:Literal ID="ltlAddName" runat="server" Text="差別費率"></asp:Literal></span></div>
                    <div class="result_table_nums">+$<asp:Literal ID="ltlAdd" runat="server" ></asp:Literal></div>
                    </div>
                    <div class="result_table_horizon" id ="trSeperatorLine" runat="server"></div>
                    <div class="result_table" id="trTotal" runat="server">
                   <div class="result_table_list"><h2>合計<asp:Literal ID="ltlTotal" runat="server" Text="(四捨五入後)"></asp:Literal></h2></div>
                    <div class="result_table_nums"><h2>$<asp:Literal ID="TotalFare" runat="server"></asp:Literal></h2></div>
                    </div>
                    <div class="result_table" id="trEtag" runat="server">
                    <div class="result_table_list"><h3><asp:Label ID="lblIsETag" runat="server" CssClass ="Fd93401" Text = "足餘額9折扣款"></asp:Label></h3></div>
                   <div class="result_table_nums"><h3>$<asp:Literal ID="ltlETag" runat="server"></asp:Literal></h3></div>
                    </div>
                    <div class="result_table" id="trReserved" runat="server">
                    <div class="result_table_list"><h3><asp:Label ID="lblReserve" runat="server" CssClass ="Fd93401" Text = "足餘額9折扣款"></asp:Label></h3></div>
                   <div class="result_table_nums"><h3>$<asp:Literal ID="ltlReserved" runat="server"></asp:Literal></h3></div>
                    </div>
                    <div class="result_table" id="tr46day" runat="server">
                    <div class="result_table_list"><h3><asp:Label ID="lbl46" runat="server" CssClass ="Fd93401" Text = "通行日起第4~6天主動繳費9折"></asp:Label></h3></div>
                   <div class="result_table_nums"><h3>$<asp:Literal ID="ltl46day" runat="server"></asp:Literal></h3></div>
                    </div>
                    <div class="result_table" id="tr7day" runat="server">
                    <div class="result_table_list"><h3><asp:Label ID="lbl7" runat="server" CssClass ="Fd93401" Text = "通行日起第7天之後繳費(無折扣)"></asp:Label></h3></div>
                   <div class="result_table_nums"><h3>$<asp:Literal ID="ltl7day" runat="server"></asp:Literal></h3></div>
                    </div>
                    </div>

                    <div class="result_area">
                    <h1 class="result_title_way"></h1>
                    <ul>
                    <li><asp:Literal ID="ltlRoadTrip" runat="server"></asp:Literal>
                    </li>
                    </ul>
                    </div>
        <uc5:PublishBlock ID="PublishBlock3" runat="server" BlockID="CustomRemindMobile" />
                </div> </div>
                            <asp:HiddenField ID="hdFareNormal" runat="server" />
	                        <asp:HiddenField ID="hdFareDiscount" runat="server" />
	                        <asp:HiddenField ID="hdFreeFare" runat="server" />
	                        <asp:HiddenField ID="hdRoadTrip" runat="server" />

<!-- footer區塊結束 -->

        <asp:SqlDataSource ID="sdsRepeater" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
            SelectCommand="Select top 2 UID from InterChangeList">
            <SelectParameters>
            </SelectParameters>
        </asp:SqlDataSource>
    <asp:HiddenField ID="availableTagst" runat="server" />
<script type="text/javascript" language="javascript" >
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
        $("#<%=txtFrom.ClientID%>").autocomplete({
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
        $.get("<%=PathManager.ApplicationUrl %>AutoCom.aspx?txt=" + encodeURIComponent(txt.value),
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
