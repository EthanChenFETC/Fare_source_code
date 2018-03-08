<%@ Page Language="VB" MasterPageFile="~/MasterPageFare.master" AutoEventWireup="false" CodeFile="Custom.aspx.vb" Inherits="_Custom" 
    EnableEventValidation="false" EnableViewStateMac="false"  EnableViewState="false"  ViewStateEncryptionMode="Never"%>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<%@ Register Src="common/PublishBlock.ascx" TagName="PublishBlock" TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHolder1" Runat="Server">

    <!-- 左區塊開始 -->
<tr>
    <td align="left" valign="top" width="16px" background="images/box_04.png" width="16px">&nbsp;</td>
	<td align="left" valign="top" background="images/box_05.png"><p class="title18">試算通行費3步驟： 1.選擇車種  2.選擇繳費方式  3.選擇行程</p>
    <table width="100%" border="0" cellpadding="0" cellspacing="0">
	    <tr>
		    <td colspan="2" align="left" valign="top">
                <uc5:PublishBlock ID="PublishBlock1" runat="server" BlockID="CustomNote" />
		    </td>
        </tr>
        <tr>
		    <td colspan="3" align="left" valign="middle"><p>
                <asp:UpdatePanel ID="UpdatePanelDate" runat="server" UpdateMode="Conditional" >
                    <ContentTemplate>
                                            </ContentTemplate>
               </asp:UpdatePanel>
                <img src="images/icon_dot_Green2.png" alt=" " width="14" height="14" align="absmiddle" />&nbsp;
                   選擇通行日期：<asp:TextBox ID="datePicker" runat="server" OnTextChanged="TextChanged"/>&nbsp;
                   <img src="images/Calendar.gif" alt="Calendar" width="30" height="30" border="0" align="absmiddle" onclick="document.getElementById('<%=datePicker.ClientID %>').focus()" />&nbsp
                   時段：<asp:DropDownList ID="ddlTimePediod" runat="server" DataTextField="TimePeriod" DataValueField ="ProjectUID" ></asp:DropDownList>
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
		    <td width="20%" align="left" valign="top" nowrap="nowrap"><p><img src="images/step3.png" alt="步驟3." width="80" height="33" align="absmiddle" />選擇行程</p></td>
		    <td width="80%" align="left" valign="middle" colspan="2">

                        <asp:HiddenField ID = "hidd1" runat="server" Value = "" />
                        <table width="100%" border="0" align="center" cellpadding="3" cellspacing="0" class="style1">
                            <tbody>
                                <tr class = "contents_font_01">
                                    <th width="4%" align="center" valign="middle" nowrap="nowrap" bgcolor="#F2F5E7">路段</th>
                                    <th width="10%" align="left" nowrap="nowrap" bgcolor="#F2F5E7">國道名稱</th>
                                    <th align="left" nowrap="nowrap" bgcolor="#F2F5E7">交流道(起點)</th>
                                    <th align="left" nowrap="nowrap" bgcolor="#E1ECD5">交流道(訖點)</th>
                                </tr>
                                
                                        <tr class = "contents_font_01">
                                            <td align="center" valign="middle" nowrap="nowrap" bgcolor="#F2F5E7">1</td>
                                            <td align="left" nowrap="nowrap" bgcolor="#F2F5E7">
                                                <asp:DropDownList ID="HWList1" runat="server" DatasourceID="sdsHWList" DataTextField="HWName" DataValueField="UID" AutoPostBack="true"
                                                    AppendDataBoundItems ="true" ToolTip="1" OnSelectedIndexChanged="HWList_SelectedIndexChanged" EnableViewState ="false" >
                                                    <asp:ListItem Text="請選擇" value ="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>

                                            <td align="left" nowrap="nowrap" bgcolor="#F2F5E7">
                                                <asp:DropDownList ID="ICListFrom1" runat="server" DataTextField="ICName" DataValueField="UID"  AutoPostBack="true" 
                                                   EnableViewState ="false" DataSourceID="sdsInterChangeList1" OnSelectedIndexChanged="ICList_SelectedIndexChanged" >
                                                </asp:DropDownList>
                                            </td>
                      
                                            <td align="left" nowrap="nowrap" bgcolor="#E1ECD5">
                                                <asp:DropDownList ID="ICListTo1" runat="server" DataSourceID="sdsInterChangeList1" DataTextField="ICName" DataValueField="UID"  AutoPostBack="false"  
                                                    >
                                                </asp:DropDownList>
                                            </td>
                                        </tr> 
                                        <tr class = "contents_font_01">
                                            <td align="center" valign="middle" nowrap="nowrap" bgcolor="#F2F5E7">2</td>
                                            <td align="left" nowrap="nowrap" bgcolor="#F2F5E7">
                                                <asp:DropDownList ID="HWList2" runat="server" DatasourceID="sdsHWList" DataTextField="HWName" DataValueField="UID" AutoPostBack="true"
                                                    AppendDataBoundItems ="true" ToolTip="1" OnSelectedIndexChanged="HWList_SelectedIndexChanged" EnableViewState ="false" >
                                                    <asp:ListItem Text="請選擇" value ="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td align="left" nowrap="nowrap" bgcolor="#F2F5E7">
                                                <asp:DropDownList ID="ICListFrom2" runat="server" DataTextField="ICName" DataValueField="UID"  AutoPostBack="true" 
                                                   EnableViewState ="false" DataSourceID="sdsInterChangeList2" OnSelectedIndexChanged="ICList_SelectedIndexChanged" >
                                                </asp:DropDownList>
                                            </td>
                      
                                            <td align="left" nowrap="nowrap" bgcolor="#E1ECD5">
                                                <asp:DropDownList ID="ICListTo2" runat="server" DataSourceID="sdsInterChangeList2" DataTextField="ICName" DataValueField="UID"  AutoPostBack="false"  
                                                    >
                                                </asp:DropDownList>
                                            </td>
                                        </tr> 
                                        <tr class = "contents_font_01">
                                            <td align="center" valign="middle" nowrap="nowrap" bgcolor="#F2F5E7">3</td>
                                            <td align="left" nowrap="nowrap" bgcolor="#F2F5E7">
                                                <asp:DropDownList ID="HWList3" runat="server" DatasourceID="sdsHWList" DataTextField="HWName" DataValueField="UID" AutoPostBack="true"
                                                    AppendDataBoundItems ="true" ToolTip="1" OnSelectedIndexChanged="HWList_SelectedIndexChanged" EnableViewState ="false" >
                                                    <asp:ListItem Text="請選擇" value ="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td align="left" nowrap="nowrap" bgcolor="#F2F5E7">
                                                <asp:DropDownList ID="ICListFrom3" runat="server" DataTextField="ICName" DataValueField="UID"  AutoPostBack="true" 
                                                   EnableViewState ="false" DataSourceID="sdsInterChangeList3" OnSelectedIndexChanged="ICList_SelectedIndexChanged" >
                                                </asp:DropDownList>
                                            </td>
                      
                                            <td align="left" nowrap="nowrap" bgcolor="#E1ECD5">
                                                <asp:DropDownList ID="ICListTo3" runat="server" DataSourceID="sdsInterChangeList3" DataTextField="ICName" DataValueField="UID"  AutoPostBack="false"  
                                                    >
                                                </asp:DropDownList>
                                            </td>
                                        </tr> 
                                        <tr class = "contents_font_01">
                                            <td align="center" valign="middle" nowrap="nowrap" bgcolor="#F2F5E7">4</td>
                                            <td align="left" nowrap="nowrap" bgcolor="#F2F5E7">
                                                <asp:DropDownList ID="HWList4" runat="server" DatasourceID="sdsHWList" DataTextField="HWName" DataValueField="UID" AutoPostBack="true"
                                                    AppendDataBoundItems ="true" ToolTip="1" OnSelectedIndexChanged="HWList_SelectedIndexChanged" EnableViewState ="false" >
                                                    <asp:ListItem Text="請選擇" value ="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td align="left" nowrap="nowrap" bgcolor="#F2F5E7">
                                                <asp:DropDownList ID="ICListFrom4" runat="server" DataTextField="ICName" DataValueField="UID"  AutoPostBack="true" 
                                                   EnableViewState ="false" DataSourceID="sdsInterChangeList4" OnSelectedIndexChanged="ICList_SelectedIndexChanged" >
                                                </asp:DropDownList>
                                            </td>
                      
                                            <td align="left" nowrap="nowrap" bgcolor="#E1ECD5">
                                                <asp:DropDownList ID="ICListTo4" runat="server" DataSourceID="sdsInterChangeList4" DataTextField="ICName" DataValueField="UID"  AutoPostBack="false"  
                                                    >
                                                </asp:DropDownList>
                                            </td>
                                        </tr> 
                                        <tr class = "contents_font_01">
                                            <td align="center" valign="middle" nowrap="nowrap" bgcolor="#F2F5E7">5</td>
                                            <td align="left" nowrap="nowrap" bgcolor="#F2F5E7">
                                                <asp:DropDownList ID="HWList5" runat="server" DatasourceID="sdsHWList" DataTextField="HWName" DataValueField="UID" AutoPostBack="true"
                                                    AppendDataBoundItems ="true" ToolTip="1" OnSelectedIndexChanged="HWList_SelectedIndexChanged" EnableViewState ="false" >
                                                    <asp:ListItem Text="請選擇" value ="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td align="left" nowrap="nowrap" bgcolor="#F2F5E7">
                                                <asp:DropDownList ID="ICListFrom5" runat="server" DataTextField="ICName" DataValueField="UID"  AutoPostBack="true" 
                                                   EnableViewState ="false" DataSourceID="sdsInterChangeList5" OnSelectedIndexChanged="ICList_SelectedIndexChanged" >
                                                </asp:DropDownList>
                                            </td>
                                            <td align="left" nowrap="nowrap" bgcolor="#E1ECD5">
                                                <asp:DropDownList ID="ICListTo5" runat="server" DataSourceID="sdsInterChangeList5" DataTextField="ICName" DataValueField="UID"  AutoPostBack="false"  
                                                    >
                                                </asp:DropDownList>
                                            </td>
                                        </tr> 
                                <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional" Visible="false" >
                                    <ContentTemplate>
                                        <tr class = "contents_font_01">
                                            <td align="center" valign="middle" nowrap="nowrap" bgcolor="#F2F5E7">6</td>
                                            <td align="left" nowrap="nowrap" bgcolor="#F2F5E7">
                                                <asp:DropDownList ID="HWList6" runat="server" DatasourceID="sdsHWList" DataTextField="HWName" DataValueField="UID" AutoPostBack="true"
                                                    AppendDataBoundItems ="true" ToolTip="1" OnSelectedIndexChanged="HWList_SelectedIndexChanged" EnableViewState ="false" >
                                                    <asp:ListItem Text="請選擇" value ="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td align="left" nowrap="nowrap" bgcolor="#F2F5E7">
                                                <asp:DropDownList ID="ICListFrom6" runat="server" DataTextField="ICName" DataValueField="UID"  AutoPostBack="true" 
                                                   EnableViewState ="false" DataSourceID="sdsInterChangeList6" OnSelectedIndexChanged="ICList_SelectedIndexChanged" >
                                                </asp:DropDownList>
                                            </td>
                                            <td align="left" nowrap="nowrap" bgcolor="#E1ECD5">
                                                <asp:DropDownList ID="ICListTo6" runat="server" DataSourceID="sdsInterChangeList6" DataTextField="ICName" DataValueField="UID"  AutoPostBack="false"  
                                                    >
                                                </asp:DropDownList>
                                            </td>
                                        </tr> 
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional" Visible="false" >
                                    <ContentTemplate>
                                        <tr class = "contents_font_01">
                                            <td align="center" valign="middle" nowrap="nowrap" bgcolor="#F2F5E7">7</td>
                                            <td align="left" nowrap="nowrap" bgcolor="#F2F5E7">
                                                <asp:DropDownList ID="HWList7" runat="server" DatasourceID="sdsHWList" DataTextField="HWName" DataValueField="UID" AutoPostBack="true"
                                                    AppendDataBoundItems ="true" ToolTip="1" OnSelectedIndexChanged="HWList_SelectedIndexChanged" EnableViewState ="false" >
                                                    <asp:ListItem Text="請選擇" value ="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td align="left" nowrap="nowrap" bgcolor="#F2F5E7">
                                                <asp:DropDownList ID="ICListFrom7" runat="server" DataTextField="ICName" DataValueField="UID"  AutoPostBack="true" 
                                                   EnableViewState ="false" DataSourceID="sdsInterChangeList7" OnSelectedIndexChanged="ICList_SelectedIndexChanged" >
                                                </asp:DropDownList>
                                            </td>
                                            <td align="left" nowrap="nowrap" bgcolor="#E1ECD5">
                                                <asp:DropDownList ID="ICListTo7" runat="server" DataSourceID="sdsInterChangeList7" DataTextField="ICName" DataValueField="UID"  AutoPostBack="false"  
                                                    >
                                                </asp:DropDownList>
                                            </td>
                                        </tr> 
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Conditional" Visible="false" >
                                    <ContentTemplate>
                                        <tr class = "contents_font_01">
                                            <td align="center" valign="middle" nowrap="nowrap" bgcolor="#F2F5E7">8</td>
                                            <td align="left" nowrap="nowrap" bgcolor="#F2F5E7">
                                                <asp:DropDownList ID="HWList8" runat="server" DatasourceID="sdsHWList" DataTextField="HWName" DataValueField="UID" AutoPostBack="true"
                                                    AppendDataBoundItems ="true" ToolTip="1" OnSelectedIndexChanged="HWList_SelectedIndexChanged" EnableViewState ="false" >
                                                    <asp:ListItem Text="請選擇" value ="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td align="left" nowrap="nowrap" bgcolor="#F2F5E7">
                                                <asp:DropDownList ID="ICListFrom8" runat="server" DataTextField="ICName" DataValueField="UID"  AutoPostBack="true" 
                                                   EnableViewState ="false" DataSourceID="sdsInterChangeList8" OnSelectedIndexChanged="ICList_SelectedIndexChanged" >
                                                </asp:DropDownList>
                                            </td>
                                            <td align="left" nowrap="nowrap" bgcolor="#E1ECD5">
                                                <asp:DropDownList ID="ICListTo8" runat="server" DataSourceID="sdsInterChangeList8" DataTextField="ICName" DataValueField="UID"  AutoPostBack="false"  
                                                    >
                                                </asp:DropDownList>
                                            </td>
                                        </tr> 
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="UpdatePanel9" runat="server" UpdateMode="Conditional" Visible="false" >
                                    <ContentTemplate>
                                        <tr class = "contents_font_01">
                                            <td align="center" valign="middle" nowrap="nowrap" bgcolor="#F2F5E7">9</td>
                                            <td align="left" nowrap="nowrap" bgcolor="#F2F5E7">
                                                <asp:DropDownList ID="HWList9" runat="server" DatasourceID="sdsHWList" DataTextField="HWName" DataValueField="UID" AutoPostBack="true"
                                                    AppendDataBoundItems ="true" ToolTip="1" OnSelectedIndexChanged="HWList_SelectedIndexChanged" EnableViewState ="false" >
                                                    <asp:ListItem Text="請選擇" value ="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td align="left" nowrap="nowrap" bgcolor="#F2F5E7">
                                                <asp:DropDownList ID="ICListFrom9" runat="server" DataTextField="ICName" DataValueField="UID"  AutoPostBack="true" 
                                                   EnableViewState ="false" DataSourceID="sdsInterChangeList9" OnSelectedIndexChanged="ICList_SelectedIndexChanged" >
                                                </asp:DropDownList>
                                            </td>
                                            <td align="left" nowrap="nowrap" bgcolor="#E1ECD5">
                                                <asp:DropDownList ID="ICListTo9" runat="server" DataSourceID="sdsInterChangeList9" DataTextField="ICName" DataValueField="UID"  AutoPostBack="false"  
                                                    >
                                                </asp:DropDownList>
                                            </td>
                                        </tr> 
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="UpdatePanel10" runat="server" UpdateMode="Conditional" Visible="false"  >
                                    <ContentTemplate>
                                        <tr class = "contents_font_01">
                                            <td align="center" valign="middle" nowrap="nowrap" bgcolor="#F2F5E7">10</td>
                                            <td align="left" nowrap="nowrap" bgcolor="#F2F5E7">
                                                <asp:DropDownList ID="HWList10" runat="server" DatasourceID="sdsHWList" DataTextField="HWName" DataValueField="UID" AutoPostBack="true"
                                                    AppendDataBoundItems ="true" ToolTip="1" OnSelectedIndexChanged="HWList_SelectedIndexChanged" EnableViewState ="false" >
                                                    <asp:ListItem Text="請選擇" value ="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td align="left" nowrap="nowrap" bgcolor="#F2F5E7">
                                                <asp:DropDownList ID="ICListFrom10" runat="server" DataTextField="ICName" DataValueField="UID"  AutoPostBack="true" 
                                                   EnableViewState ="false" DataSourceID="sdsInterChangeList10" OnSelectedIndexChanged="ICList_SelectedIndexChanged" >
                                                </asp:DropDownList>
                                            </td>
                                            <td align="left" nowrap="nowrap" bgcolor="#E1ECD5">
                                                <asp:DropDownList ID="ICListTo10" runat="server" DataSourceID="sdsInterChangeList10" DataTextField="ICName" DataValueField="UID"  AutoPostBack="false"  
                                                    >
                                                </asp:DropDownList>
                                            </td>
                                        </tr> 
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="UpdatePanel11" runat="server" UpdateMode="Conditional" Visible="false"  >
                                    <ContentTemplate>
                                        <tr class = "contents_font_01">
                                            <td align="center" valign="middle" nowrap="nowrap" bgcolor="#F2F5E7">11</td>
                                            <td align="left" nowrap="nowrap" bgcolor="#F2F5E7">
                                                <asp:DropDownList ID="HWList11" runat="server" DatasourceID="sdsHWList" DataTextField="HWName" DataValueField="UID" AutoPostBack="true"
                                                    AppendDataBoundItems ="true" ToolTip="1" OnSelectedIndexChanged="HWList_SelectedIndexChanged" EnableViewState ="false" >
                                                    <asp:ListItem Text="請選擇" value ="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td align="left" nowrap="nowrap" bgcolor="#F2F5E7">
                                                <asp:DropDownList ID="ICListFrom11" runat="server" DataTextField="ICName" DataValueField="UID"  AutoPostBack="true" 
                                                   EnableViewState ="false" DataSourceID="sdsInterChangeList11" OnSelectedIndexChanged="ICList_SelectedIndexChanged" >
                                                </asp:DropDownList>
                                            </td>
                                            <td align="left" nowrap="nowrap" bgcolor="#E1ECD5">
                                                <asp:DropDownList ID="ICListTo11" runat="server" DataSourceID="sdsInterChangeList11" DataTextField="ICName" DataValueField="UID"  AutoPostBack="false"  
                                                    >
                                                </asp:DropDownList>
                                            </td>
                                        </tr> 
                                     </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="UpdatePanel12" runat="server" UpdateMode="Conditional" Visible="false"  >
                                    <ContentTemplate>
                                       <tr class = "contents_font_01" >
                                            <td align="center" valign="middle" nowrap="nowrap" bgcolor="#F2F5E7">12</td>
                                            <td align="left" nowrap="nowrap" bgcolor="#F2F5E7">
                                                <asp:DropDownList ID="HWList12" runat="server" DatasourceID="sdsHWList" DataTextField="HWName" DataValueField="UID" AutoPostBack="true"
                                                    AppendDataBoundItems ="true" ToolTip="1" OnSelectedIndexChanged="HWList_SelectedIndexChanged" EnableViewState ="false" >
                                                    <asp:ListItem Text="請選擇" value ="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td align="left" nowrap="nowrap" bgcolor="#F2F5E7">
                                                <asp:DropDownList ID="ICListFrom12" runat="server" DataTextField="ICName" DataValueField="UID"  AutoPostBack="true" 
                                                   EnableViewState ="false" DataSourceID="sdsInterChangeList12" OnSelectedIndexChanged="ICList_SelectedIndexChanged" >
                                                </asp:DropDownList>
                                            </td>
                                            <td align="left" nowrap="nowrap" bgcolor="#E1ECD5">
                                                <asp:DropDownList ID="ICListTo12" runat="server" DataSourceID="sdsInterChangeList12" DataTextField="ICName" DataValueField="UID"  AutoPostBack="false"  
                                                    >
                                                </asp:DropDownList>
                                            </td>
                                        </tr> 
                                     </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="UpdatePanel13" runat="server" UpdateMode="Conditional" Visible="false"  >
                                    <ContentTemplate>
                                       <tr class = "contents_font_01">
                                            <td align="center" valign="middle" nowrap="nowrap" bgcolor="#F2F5E7">13</td>
                                            <td align="left" nowrap="nowrap" bgcolor="#F2F5E7">
                                                <asp:DropDownList ID="HWList13" runat="server" DatasourceID="sdsHWList" DataTextField="HWName" DataValueField="UID" AutoPostBack="true"
                                                    AppendDataBoundItems ="true" ToolTip="1" OnSelectedIndexChanged="HWList_SelectedIndexChanged" EnableViewState ="false" >
                                                    <asp:ListItem Text="請選擇" value ="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td align="left" nowrap="nowrap" bgcolor="#F2F5E7">
                                                <asp:DropDownList ID="ICListFrom13" runat="server" DataTextField="ICName" DataValueField="UID"  AutoPostBack="true" 
                                                   EnableViewState ="false" DataSourceID="sdsInterChangeList13" OnSelectedIndexChanged="ICList_SelectedIndexChanged" >
                                                </asp:DropDownList>
                                            </td>
                                            <td align="left" nowrap="nowrap" bgcolor="#E1ECD5">
                                                <asp:DropDownList ID="ICListTo13" runat="server" DataSourceID="sdsInterChangeList13" DataTextField="ICName" DataValueField="UID"  AutoPostBack="false"  
                                                    >
                                                </asp:DropDownList>
                                            </td>
                                        </tr> 
                                      </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="UpdatePanel14" runat="server" UpdateMode="Conditional" Visible="false"  >
                                    <ContentTemplate>
                                      <tr class = "contents_font_01">
                                            <td align="center" valign="middle" nowrap="nowrap" bgcolor="#F2F5E7">14</td>
                                            <td align="left" nowrap="nowrap" bgcolor="#F2F5E7">
                                                <asp:DropDownList ID="HWList14" runat="server" DatasourceID="sdsHWList" DataTextField="HWName" DataValueField="UID" AutoPostBack="true"
                                                    AppendDataBoundItems ="true" ToolTip="1" OnSelectedIndexChanged="HWList_SelectedIndexChanged" EnableViewState ="false" >
                                                    <asp:ListItem Text="請選擇" value ="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td align="left" nowrap="nowrap" bgcolor="#F2F5E7">
                                                <asp:DropDownList ID="ICListFrom14" runat="server" DataTextField="ICName" DataValueField="UID"  AutoPostBack="true" 
                                                   EnableViewState ="false" DataSourceID="sdsInterChangeList14" OnSelectedIndexChanged="ICList_SelectedIndexChanged" >
                                                </asp:DropDownList>
                                            </td>
                                            <td align="left" nowrap="nowrap" bgcolor="#E1ECD5">
                                                <asp:DropDownList ID="ICListTo14" runat="server" DataSourceID="sdsInterChangeList14" DataTextField="ICName" DataValueField="UID"  AutoPostBack="false"  
                                                    >
                                                </asp:DropDownList>
                                            </td>
                                        </tr> 
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="UpdatePanel15" runat="server" UpdateMode="Conditional" Visible="false"  >
                                    <ContentTemplate>
                                        <tr class = "contents_font_01">
                                            <td align="center" valign="middle" nowrap="nowrap" bgcolor="#F2F5E7">15</td>
                                            <td align="left" nowrap="nowrap" bgcolor="#F2F5E7">
                                                <asp:DropDownList ID="HWList15" runat="server" DatasourceID="sdsHWList" DataTextField="HWName" DataValueField="UID" AutoPostBack="true"
                                                    AppendDataBoundItems ="true" ToolTip="1" OnSelectedIndexChanged="HWList_SelectedIndexChanged" EnableViewState ="false" >
                                                    <asp:ListItem Text="請選擇" value ="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td align="left" nowrap="nowrap" bgcolor="#F2F5E7">
                                                <asp:DropDownList ID="ICListFrom15" runat="server" DataTextField="ICName" DataValueField="UID"  AutoPostBack="true" 
                                                   EnableViewState ="false" DataSourceID="sdsInterChangeList15" OnSelectedIndexChanged="ICList_SelectedIndexChanged" >
                                                </asp:DropDownList>
                                            </td>
                                            <td align="left" nowrap="nowrap" bgcolor="#E1ECD5">
                                                <asp:DropDownList ID="ICListTo15" runat="server" DataSourceID="sdsInterChangeList15" DataTextField="ICName" DataValueField="UID"  AutoPostBack="false"  
                                                    >
                                                </asp:DropDownList>
                                            </td>
                                        </tr> 
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="UpdatePanel16" runat="server" UpdateMode="Conditional" Visible="false"  >
                                    <ContentTemplate>
                                        <tr class = "contents_font_01">
                                            <td align="center" valign="middle" nowrap="nowrap" bgcolor="#F2F5E7">16</td>
                                            <td align="left" nowrap="nowrap" bgcolor="#F2F5E7">
                                                <asp:DropDownList ID="HWList16" runat="server" DatasourceID="sdsHWList" DataTextField="HWName" DataValueField="UID" AutoPostBack="true"
                                                    AppendDataBoundItems ="true" ToolTip="1" OnSelectedIndexChanged="HWList_SelectedIndexChanged" EnableViewState ="false" >
                                                    <asp:ListItem Text="請選擇" value ="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td align="left" nowrap="nowrap" bgcolor="#F2F5E7">
                                                <asp:DropDownList ID="ICListFrom16" runat="server" DataTextField="ICName" DataValueField="UID"  AutoPostBack="true" 
                                                   EnableViewState ="false" DataSourceID="sdsInterChangeList16" OnSelectedIndexChanged="ICList_SelectedIndexChanged" >
                                                </asp:DropDownList>
                                            </td>
                                            <td align="left" nowrap="nowrap" bgcolor="#E1ECD5">
                                                <asp:DropDownList ID="ICListTo16" runat="server" DataSourceID="sdsInterChangeList16" DataTextField="ICName" DataValueField="UID"  AutoPostBack="false"  
                                                    >
                                                </asp:DropDownList>
                                            </td>
                                        </tr> 
                                     </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="UpdatePanel17" runat="server" UpdateMode="Conditional" Visible="false"  >
                                    <ContentTemplate>
                                       <tr class = "contents_font_01">
                                            <td align="center" valign="middle" nowrap="nowrap" bgcolor="#F2F5E7">17</td>
                                            <td align="left" nowrap="nowrap" bgcolor="#F2F5E7">
                                                <asp:DropDownList ID="HWList17" runat="server" DatasourceID="sdsHWList" DataTextField="HWName" DataValueField="UID" AutoPostBack="true"
                                                    AppendDataBoundItems ="true" ToolTip="1" OnSelectedIndexChanged="HWList_SelectedIndexChanged" EnableViewState ="false" >
                                                    <asp:ListItem Text="請選擇" value ="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td align="left" nowrap="nowrap" bgcolor="#F2F5E7">
                                                <asp:DropDownList ID="ICListFrom17" runat="server" DataTextField="ICName" DataValueField="UID"  AutoPostBack="true" 
                                                   EnableViewState ="false" DataSourceID="sdsInterChangeList17" OnSelectedIndexChanged="ICList_SelectedIndexChanged" >
                                                </asp:DropDownList>
                                            </td>
                                            <td align="left" nowrap="nowrap" bgcolor="#E1ECD5">
                                                <asp:DropDownList ID="ICListTo17" runat="server" DataSourceID="sdsInterChangeList17" DataTextField="ICName" DataValueField="UID"  AutoPostBack="false"  
                                                    >
                                                </asp:DropDownList>
                                            </td>
                                        </tr> 
                                     </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="UpdatePanel18" runat="server" UpdateMode="Conditional" Visible="false"  >
                                    <ContentTemplate>
                                      <tr class = "contents_font_01">
                                            <td align="center" valign="middle" nowrap="nowrap" bgcolor="#F2F5E7">18</td>
                                            <td align="left" nowrap="nowrap" bgcolor="#F2F5E7">
                                                <asp:DropDownList ID="HWList18" runat="server" DatasourceID="sdsHWList" DataTextField="HWName" DataValueField="UID" AutoPostBack="true"
                                                    AppendDataBoundItems ="true" ToolTip="1" OnSelectedIndexChanged="HWList_SelectedIndexChanged" EnableViewState ="false" >
                                                    <asp:ListItem Text="請選擇" value ="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td align="left" nowrap="nowrap" bgcolor="#F2F5E7">
                                                <asp:DropDownList ID="ICListFrom18" runat="server" DataTextField="ICName" DataValueField="UID"  AutoPostBack="true" 
                                                   EnableViewState ="false" DataSourceID="sdsInterChangeList18" OnSelectedIndexChanged="ICList_SelectedIndexChanged" >
                                                </asp:DropDownList>
                                            </td>
                                            <td align="left" nowrap="nowrap" bgcolor="#E1ECD5">
                                                <asp:DropDownList ID="ICListTo18" runat="server" DataSourceID="sdsInterChangeList18" DataTextField="ICName" DataValueField="UID"  AutoPostBack="false"  
                                                    >
                                                </asp:DropDownList>
                                            </td>
                                        </tr> 
                                     </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="UpdatePanel19" runat="server" UpdateMode="Conditional" Visible="false"  >
                                    <ContentTemplate>
                                       <tr class = "contents_font_01">
                                            <td align="center" valign="middle" nowrap="nowrap" bgcolor="#F2F5E7">19</td>
                                            <td align="left" nowrap="nowrap" bgcolor="#F2F5E7">
                                                <asp:DropDownList ID="HWList19" runat="server" DatasourceID="sdsHWList" DataTextField="HWName" DataValueField="UID" AutoPostBack="true"
                                                    AppendDataBoundItems ="true" ToolTip="1" OnSelectedIndexChanged="HWList_SelectedIndexChanged" EnableViewState ="false" >
                                                    <asp:ListItem Text="請選擇" value ="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td align="left" nowrap="nowrap" bgcolor="#F2F5E7">
                                                <asp:DropDownList ID="ICListFrom19" runat="server" DataTextField="ICName" DataValueField="UID"  AutoPostBack="true" 
                                                   EnableViewState ="false" DataSourceID="sdsInterChangeList19" OnSelectedIndexChanged="ICList_SelectedIndexChanged" >
                                                </asp:DropDownList>
                                            </td>
                                            <td align="left" nowrap="nowrap" bgcolor="#E1ECD5">
                                                <asp:DropDownList ID="ICListTo19" runat="server" DataSourceID="sdsInterChangeList19" DataTextField="ICName" DataValueField="UID"  AutoPostBack="false"  
                                                    >
                                                </asp:DropDownList>
                                            </td>
                                        </tr> 
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="UpdatePanel20" runat="server" UpdateMode="Conditional" Visible="false"  >
                                    <ContentTemplate>
                                        <tr class = "contents_font_01">
                                            <td align="center" valign="middle" nowrap="nowrap" bgcolor="#F2F5E7">20</td>
                                            <td align="left" nowrap="nowrap" bgcolor="#F2F5E7">
                                                <asp:DropDownList ID="HWList20" runat="server" DatasourceID="sdsHWList" DataTextField="HWName" DataValueField="UID" AutoPostBack="true"
                                                    AppendDataBoundItems ="true" ToolTip="1" OnSelectedIndexChanged="HWList_SelectedIndexChanged" EnableViewState ="false" >
                                                    <asp:ListItem Text="請選擇" value ="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td align="left" nowrap="nowrap" bgcolor="#F2F5E7">
                                                <asp:DropDownList ID="ICListFrom20" runat="server" DataTextField="ICName" DataValueField="UID"  AutoPostBack="true" 
                                                   EnableViewState ="false" DataSourceID="sdsInterChangeList20" OnSelectedIndexChanged="ICList_SelectedIndexChanged" >
                                                </asp:DropDownList>
                                            </td>
                                            <td align="left" nowrap="nowrap" bgcolor="#E1ECD5">
                                                <asp:DropDownList ID="ICListTo20" runat="server" DataSourceID="sdsInterChangeList20" DataTextField="ICName" DataValueField="UID"  AutoPostBack="false"  
                                                    >
                                                </asp:DropDownList>
                                            </td>
                                        </tr> 
                                      </ContentTemplate>
                                </asp:UpdatePanel>
                               
                          </tbody>
                        </table>
                <input type="hidden" id="hidDDL" value="5" runat="server"/>

                <div align="right" >
                    <asp:Button ID="lbtMore" runat="server" Text  = "More" class="btn btn-outline btn-success" ></asp:Button>
                </div>
		    </td>
        </tr>
        <tr>
            <td colspan="3" align="center" valign="top">
                <p><asp:ImageButton ID="imgCalculate" runat="server" ImageUrl = "images/btn_star.png" BorderWidth="0" width="204px" Height="57px" ToolTip="開始計算!"  onmouseover="this.src='images/btn_star_a.png'" onfocus="this.src='images/btn_star.png'" onmouseout="this.src='images/btn_star.png'" onblur="this.src='images/btn_star_a.png'"/></p>
            </td>
        </tr>
	</table>
    <div class="line"></div>
	<table width="100%" border="0" cellpadding="0" cellspacing="0">
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
                                <tr>
                                    <td colspan="2">
                                        <p class="title">試算結果</p>
                                        <p class="result"><img src="images/icon_dot_Green2.png" alt=" " width="14" height="14" align="absmiddle" />&nbsp;<asp:Literal ID="ltlProjectName" runat="server"></asp:Literal>
                                            通行日期：<font color=blue ><asp:Literal ID="ltlDate" runat="server"></asp:Literal></font>，時段：<font color=blue ><asp:Literal ID="ltlTimePeriod" runat="server"></asp:Literal></font>。</p>
                                        <p class="time">【各車種費率:小型車<asp:Literal ID="ltlFareS" runat="server"></asp:Literal>元/公里、大型車<asp:Literal ID="ltlFareM" runat="server"></asp:Literal>元/公里、聯結車<asp:Literal ID="ltlFareG" runat="server"></asp:Literal>元/公里】</p>
                                        <asp:Literal ID="ltlProjectNotes" runat="server"></asp:Literal>
                                    </td>
                                </tr>
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
                            <uc5:PublishBlock ID="PublishBlock2" runat="server" BlockID="CustomRemind" />
                            
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
                                        <asp:SqlDataSource ID="sdsHWList" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                            SelectCommand="Select HWName, UID from HWList where IsOnline > 0 and @IsScan = 0 order by ItemOrder">
                                            <SelectParameters>
                                                <asp:Parameter Name="IsScan" DefaultValue="0" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        <asp:SqlDataSource ID="sdsInterChangeList1" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                            SelectCommand="Select ICName, UID from InterChangeList where HWUID = @HWUID and IsOnline > 0 and IsVirture > 0 and @IsScan = 0 order by ItemOrder">
                                            <SelectParameters>
                                            <asp:ControlParameter ControlID ="HWList1" Name="HWUID"  DefaultValue = "0" />
                                                <asp:Parameter Name="IsScan" DefaultValue="0" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        <asp:SqlDataSource ID="sdsInterChangeList2" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                            SelectCommand="Select ICName, UID from InterChangeList where HWUID = @HWUID and IsOnline > 0 and IsVirture > 0 and @IsScan = 0 order by ItemOrder">
                                            <SelectParameters>
                                            <asp:ControlParameter ControlID ="HWList2" Name="HWUID"  DefaultValue = "0" />
                                                <asp:Parameter Name="IsScan" DefaultValue="0" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        <asp:SqlDataSource ID="sdsInterChangeList3" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                            SelectCommand="Select ICName, UID from InterChangeList where HWUID = @HWUID and IsOnline > 0 and IsVirture > 0 and @IsScan = 0 order by ItemOrder">
                                            <SelectParameters>
                                            <asp:ControlParameter ControlID ="HWList3" Name="HWUID"  DefaultValue = "0" />
                                                <asp:Parameter Name="IsScan" DefaultValue="0" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        <asp:SqlDataSource ID="sdsInterChangeList4" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                            SelectCommand="Select ICName, UID from InterChangeList where HWUID = @HWUID and IsOnline > 0 and IsVirture > 0 and @IsScan = 0 order by ItemOrder">
                                            <SelectParameters>
                                            <asp:ControlParameter ControlID ="HWList4" Name="HWUID"  DefaultValue = "0" />
                                                <asp:Parameter Name="IsScan" DefaultValue="0" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        <asp:SqlDataSource ID="sdsInterChangeList5" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                            SelectCommand="Select ICName, UID from InterChangeList where HWUID = @HWUID and IsOnline > 0 and IsVirture > 0 and @IsScan = 0 order by ItemOrder">
                                            <SelectParameters>
                                            <asp:ControlParameter ControlID ="HWList5" Name="HWUID"  DefaultValue = "0" />
                                                <asp:Parameter Name="IsScan" DefaultValue="0" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        <asp:SqlDataSource ID="sdsInterChangeList6" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                            SelectCommand="Select ICName, UID from InterChangeList where HWUID = @HWUID and IsOnline > 0 and IsVirture > 0 and @IsScan = 0 order by ItemOrder">
                                            <SelectParameters>
                                            <asp:ControlParameter ControlID ="HWList6" Name="HWUID"  DefaultValue = "0" />
                                                <asp:Parameter Name="IsScan" DefaultValue="0" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        <asp:SqlDataSource ID="sdsInterChangeList7" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                            SelectCommand="Select ICName, UID from InterChangeList where HWUID = @HWUID and IsOnline > 0 and IsVirture > 0 and @IsScan = 0 order by ItemOrder">
                                            <SelectParameters>
                                            <asp:ControlParameter ControlID ="HWList7" Name="HWUID"  DefaultValue = "0" />
                                                <asp:Parameter Name="IsScan" DefaultValue="0" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        <asp:SqlDataSource ID="sdsInterChangeList8" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                            SelectCommand="Select ICName, UID from InterChangeList where HWUID = @HWUID and IsOnline > 0 and IsVirture > 0 and @IsScan = 0 order by ItemOrder">
                                            <SelectParameters>
                                            <asp:ControlParameter ControlID ="HWList8" Name="HWUID"  DefaultValue = "0" />
                                                <asp:Parameter Name="IsScan" DefaultValue="0" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        <asp:SqlDataSource ID="sdsInterChangeList9" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                            SelectCommand="Select ICName, UID from InterChangeList where HWUID = @HWUID and IsOnline > 0 and IsVirture > 0 and @IsScan = 0 order by ItemOrder">
                                            <SelectParameters>
                                            <asp:ControlParameter ControlID ="HWList9" Name="HWUID"  DefaultValue = "0" />
                                                <asp:Parameter Name="IsScan" DefaultValue="0" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        <asp:SqlDataSource ID="sdsInterChangeList10" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                            SelectCommand="Select ICName, UID from InterChangeList where HWUID = @HWUID and IsOnline > 0 and IsVirture > 0 and @IsScan = 0 order by ItemOrder">
                                            <SelectParameters>
                                            <asp:ControlParameter ControlID ="HWList10" Name="HWUID"  DefaultValue = "0" />
                                                <asp:Parameter Name="IsScan" DefaultValue="0" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        <asp:SqlDataSource ID="sdsInterChangeList11" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                            SelectCommand="Select ICName, UID from InterChangeList where HWUID = @HWUID and IsOnline > 0 and IsVirture > 0 and @IsScan = 0 order by ItemOrder">
                                            <SelectParameters>
                                            <asp:ControlParameter ControlID ="HWList11" Name="HWUID"  DefaultValue = "0" />
                                                <asp:Parameter Name="IsScan" DefaultValue="0" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        <asp:SqlDataSource ID="sdsInterChangeList12" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                            SelectCommand="Select ICName, UID from InterChangeList where HWUID = @HWUID and IsOnline > 0 and IsVirture > 0 and @IsScan = 0 order by ItemOrder">
                                            <SelectParameters>
                                            <asp:ControlParameter ControlID ="HWList12" Name="HWUID"  DefaultValue = "0" />
                                                <asp:Parameter Name="IsScan" DefaultValue="0" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        <asp:SqlDataSource ID="sdsInterChangeList13" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                            SelectCommand="Select ICName, UID from InterChangeList where HWUID = @HWUID and IsOnline > 0 and IsVirture > 0 and @IsScan = 0 and @IsScan = 0 order by ItemOrder">
                                            <SelectParameters>
                                            <asp:ControlParameter ControlID ="HWList13" Name="HWUID"  DefaultValue = "0" />
                                                <asp:Parameter Name="IsScan" DefaultValue="0" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        <asp:SqlDataSource ID="sdsInterChangeList14" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                            SelectCommand="Select ICName, UID from InterChangeList where HWUID = @HWUID and IsOnline > 0 and IsVirture > 0 and @IsScan = 0 order by ItemOrder">
                                            <SelectParameters>
                                            <asp:ControlParameter ControlID ="HWList14" Name="HWUID"  DefaultValue = "0" />
                                                <asp:Parameter Name="IsScan" DefaultValue="0" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        <asp:SqlDataSource ID="sdsInterChangeList15" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                            SelectCommand="Select ICName, UID from InterChangeList where HWUID = @HWUID and IsOnline > 0 and IsVirture > 0 and @IsScan = 0 order by ItemOrder">
                                            <SelectParameters>
                                            <asp:ControlParameter ControlID ="HWList15" Name="HWUID"  DefaultValue = "0" />
                                                <asp:Parameter Name="IsScan" DefaultValue="0" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        <asp:SqlDataSource ID="sdsInterChangeList16" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                            SelectCommand="Select ICName, UID from InterChangeList where HWUID = @HWUID and IsOnline > 0 and IsVirture > 0 and @IsScan = 0 order by ItemOrder">
                                            <SelectParameters>
                                            <asp:ControlParameter ControlID ="HWList16" Name="HWUID"  DefaultValue = "0" />
                                                <asp:Parameter Name="IsScan" DefaultValue="0" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        <asp:SqlDataSource ID="sdsInterChangeList17" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                            SelectCommand="Select ICName, UID from InterChangeList where HWUID = @HWUID and IsOnline > 0 and IsVirture > 0 and @IsScan = 0 order by ItemOrder">
                                            <SelectParameters>
                                            <asp:ControlParameter ControlID ="HWList17" Name="HWUID"  DefaultValue = "0" />
                                                <asp:Parameter Name="IsScan" DefaultValue="0" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        <asp:SqlDataSource ID="sdsInterChangeList18" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                            SelectCommand="Select ICName, UID from InterChangeList where HWUID = @HWUID and IsOnline > 0 and IsVirture > 0 and @IsScan = 0 order by ItemOrder">
                                            <SelectParameters>
                                            <asp:ControlParameter ControlID ="HWList18" Name="HWUID"  DefaultValue = "0" />
                                                <asp:Parameter Name="IsScan" DefaultValue="0" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        <asp:SqlDataSource ID="sdsInterChangeList19" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                            SelectCommand="Select ICName, UID from InterChangeList where HWUID = @HWUID and IsOnline > 0 and IsVirture > 0 and @IsScan = 0 order by ItemOrder">
                                            <SelectParameters>
                                            <asp:ControlParameter ControlID ="HWList19" Name="HWUID"  DefaultValue = "0" />
                                                <asp:Parameter Name="IsScan" DefaultValue="0" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        <asp:SqlDataSource ID="sdsInterChangeList20" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                            SelectCommand="Select ICName, UID from InterChangeList where HWUID = @HWUID and IsOnline > 0 and IsVirture > 0 and @IsScan = 0 order by ItemOrder">
                                            <SelectParameters>
                                            <asp:ControlParameter ControlID ="HWList20" Name="HWUID"  DefaultValue = "0" />
                                                <asp:Parameter Name="IsScan" DefaultValue="0" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
 
      <script type="text/javascript" language="javascript" >
          $(document).ready(function () {
              $("#<%=datePicker.ClientID%>").datepicker({
                  dateFormat: 'yy/mm/dd',
                  onSelect: function (selected, event) {
                      javascript: __doPostBack('<%=datePicker.ClientID.Replace("_", "$")%>', '');
                  }
              });
              $("#<%=Me.imgCalculate.ClientID%>").click(function () {
                  $('#mask').css({ 'display': 'block' });
              });
              $('.mask').click(function () {
                  $('#mask').css({ 'display': 'none' });
              });
          });

          <%--Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
          $(document).ready(function () {
              EndRequestHandler();
          });
          function EndRequestHandler() {
              $(function () {
                  $("#<%=datePicker.ClientID%>").datepicker({
                            dateFormat: 'yy/mm/dd',
                            onSelect: function (selected, event) {
                                javascript: __doPostBack('<%=datePicker.ClientID.Replace("_","$")%>', '');
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
          };--%>

  
  </script>
    
</asp:Content>
