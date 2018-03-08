<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Custom.aspx.vb" Inherits="_Custom"  
    EnableEventValidation="false" EnableViewStateMac="false" EnableViewState="false"  ViewStateEncryptionMode="Never"%>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<%@ Register Src="common/PublishBlock.ascx" TagName="PublishBlock" TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="content">
    <uc5:PublishBlock ID="PublishBlock1" runat="server" BlockID="MCustom" />
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
           <select name="select" id="CarType" runat="server" align="absmiddle"  class="select_way">
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
    <div id="build_way_block">
        <div id="build_way_block0">
			<div class="way_area">
                <h1>路段1</h1>
                <div class="way_area_start">
                    <h1>國道</h1>
                    <asp:DropDownList ID="HWList1" runat="server" DatasourceID="sdsHWList" DataTextField="HWName" DataValueField="UID" AutoPostBack="true" CssClass="select_way"
                        AppendDataBoundItems ="true" ToolTip="1" OnSelectedIndexChanged="HWList_SelectedIndexChanged" EnableViewState ="false" >
                        <asp:ListItem Text="請選擇" value ="0"></asp:ListItem>
                    </asp:DropDownList>
                    <h1>交流道<em>(起點)</em></h1>
                    <asp:DropDownList ID="ICListFrom1" runat="server" DataTextField="ICName" DataValueField="UID"  AutoPostBack="true"  CssClass="select_way"
                        EnableViewState ="false" DataSourceID="sdsInterChangeList1" OnSelectedIndexChanged="ICList_SelectedIndexChanged" >
                    </asp:DropDownList>
                </div>
                <div class="way_area_final">
                    <h1>交流道<em>(訖點)</em></h1>
                    <asp:DropDownList ID="ICListTo1" runat="server" DataSourceID="sdsInterChangeList1" DataTextField="ICName" DataValueField="UID"  AutoPostBack="false"  CssClass="select_way" ></asp:DropDownList>
                </div>
            </div>
			<div class="way_area" id="tr2" style="display:none">
                <h1>路段2</h1>
                <div class="way_area_start">
                    <h1>國道</h1>
                    <asp:DropDownList ID="HWList2" runat="server" DatasourceID="sdsHWList" DataTextField="HWName" DataValueField="UID" AutoPostBack="true" CssClass="select_way"
                        AppendDataBoundItems ="true" ToolTip="1" OnSelectedIndexChanged="HWList_SelectedIndexChanged" EnableViewState ="false" >
                        <asp:ListItem Text="請選擇" value ="0"></asp:ListItem>
                    </asp:DropDownList>
                    <h1>交流道<em>(起點)</em></h1>
                    <asp:DropDownList ID="ICListFrom2" runat="server" DataTextField="ICName" DataValueField="UID"  AutoPostBack="true"  CssClass="select_way"
                        EnableViewState ="false" DataSourceID="sdsInterChangeList2" OnSelectedIndexChanged="ICList_SelectedIndexChanged" >
                    </asp:DropDownList>
                </div>
                <div class="way_area_final">
                    <h1>交流道<em>(訖點)</em></h1>
                    <asp:DropDownList ID="ICListTo2" runat="server" DataSourceID="sdsInterChangeList2" DataTextField="ICName" DataValueField="UID"  AutoPostBack="false"  CssClass="select_way" ></asp:DropDownList>
                </div>
            </div>
			<div class="way_area" id="tr3" style="display:none">
                <h1>路段3</h1>
                <div class="way_area_start">
                    <h1>國道</h1>
                    <asp:DropDownList ID="HWList3" runat="server" DatasourceID="sdsHWList" DataTextField="HWName" DataValueField="UID" AutoPostBack="true"
                        AppendDataBoundItems ="true" ToolTip="1" OnSelectedIndexChanged="HWList_SelectedIndexChanged" EnableViewState ="false" CssClass="select_way">
                        <asp:ListItem Text="請選擇" value ="0"></asp:ListItem>
                    </asp:DropDownList>
                    <h1>交流道<em>(起點)</em></h1>
                    <asp:DropDownList ID="ICListFrom3" runat="server" DataTextField="ICName" DataValueField="UID"  AutoPostBack="true"  CssClass="select_way"
                        EnableViewState ="false" DataSourceID="sdsInterChangeList3" OnSelectedIndexChanged="ICList_SelectedIndexChanged" >
                    </asp:DropDownList>
                </div>
                <div class="way_area_final">
                       <h1>交流道<em>(訖點)</em></h1>
                 <asp:DropDownList ID="ICListTo3" runat="server" DataSourceID="sdsInterChangeList3" DataTextField="ICName" DataValueField="UID"  AutoPostBack="false"  CssClass="select_way" ></asp:DropDownList>
                </div>
            </div>
			<div class="way_area" id="tr4" style="display:none">
                <h1>路段4</h1>
                <div class="way_area_start">
                    <h1>國道</h1>
                    <asp:DropDownList ID="HWList4" runat="server" DatasourceID="sdsHWList" DataTextField="HWName" DataValueField="UID" AutoPostBack="true"  CssClass="select_way"
                        AppendDataBoundItems ="true" ToolTip="1" OnSelectedIndexChanged="HWList_SelectedIndexChanged" EnableViewState ="false" >
                        <asp:ListItem Text="請選擇" value ="0"></asp:ListItem>
                    </asp:DropDownList>
                    <h1>交流道<em>(起點)</em></h1>
                    <asp:DropDownList ID="ICListFrom4" runat="server" DataTextField="ICName" DataValueField="UID"  AutoPostBack="true"  CssClass="select_way"
                        EnableViewState ="false" DataSourceID="sdsInterChangeList4" OnSelectedIndexChanged="ICList_SelectedIndexChanged" >
                    </asp:DropDownList>
                </div>
                <div class="way_area_final">
                     <h1>交流道<em>(訖點)</em></h1>
                   <asp:DropDownList ID="ICListTo4" runat="server" DataSourceID="sdsInterChangeList4" DataTextField="ICName" DataValueField="UID"  AutoPostBack="false"  CssClass="select_way" ></asp:DropDownList>
                </div>
            </div>
			<div class="way_area" id="tr5" style="display:none">
                <h1>路段5</h1>
                <div class="way_area_start">
                    <h1>國道</h1>
                    <asp:DropDownList ID="HWList5" runat="server" DatasourceID="sdsHWList" DataTextField="HWName" DataValueField="UID" AutoPostBack="true"  CssClass="select_way"
                        AppendDataBoundItems ="true" ToolTip="1" OnSelectedIndexChanged="HWList_SelectedIndexChanged" EnableViewState ="false" >
                        <asp:ListItem Text="請選擇" value ="0"></asp:ListItem>
                    </asp:DropDownList>
                    <h1>交流道<em>(起點)</em></h1>
                    <asp:DropDownList ID="ICListFrom5" runat="server" DataTextField="ICName" DataValueField="UID"  AutoPostBack="true"  CssClass="select_way"
                        EnableViewState ="false" DataSourceID="sdsInterChangeList5" OnSelectedIndexChanged="ICList_SelectedIndexChanged" >
                    </asp:DropDownList>
                </div>
                <div class="way_area_final">
                    <h1>交流道<em>(訖點)</em></h1>
                    <asp:DropDownList ID="ICListTo5" runat="server" DataSourceID="sdsInterChangeList5" DataTextField="ICName" DataValueField="UID"  AutoPostBack="false"  CssClass="select_way" ></asp:DropDownList>
                </div>
            </div>
			<div class="way_area" id="tr6" style="display:none">
                <h1>路段6</h1>
                <div class="way_area_start">
                    <h1>國道</h1>
                    <asp:DropDownList ID="HWList6" runat="server" DatasourceID="sdsHWList" DataTextField="HWName" DataValueField="UID" AutoPostBack="true" CssClass="select_way"
                        AppendDataBoundItems ="true" ToolTip="1" OnSelectedIndexChanged="HWList_SelectedIndexChanged" EnableViewState ="false" >
                        <asp:ListItem Text="請選擇" value ="0"></asp:ListItem>
                    </asp:DropDownList>
                    <h1>交流道<em>(起點)</em></h1>
                    <asp:DropDownList ID="ICListFrom6" runat="server" DataTextField="ICName" DataValueField="UID"  AutoPostBack="true"  CssClass="select_way"
                        EnableViewState ="false" DataSourceID="sdsInterChangeList6" OnSelectedIndexChanged="ICList_SelectedIndexChanged" >
                    </asp:DropDownList>
                </div>
                <div class="way_area_final">
                     <h1>交流道<em>(訖點)</em></h1>
                   <asp:DropDownList ID="ICListTo6" runat="server" DataSourceID="sdsInterChangeList6" DataTextField="ICName" DataValueField="UID"  AutoPostBack="false"  CssClass="select_way" ></asp:DropDownList>
                </div>
            </div>
			<div class="way_area" id="tr7" style="display:none">
                <h1>路段7</h1>
                <div class="way_area_start">
                    <h1>國道</h1>
                    <asp:DropDownList ID="HWList7" runat="server" DatasourceID="sdsHWList" DataTextField="HWName" DataValueField="UID" AutoPostBack="true" CssClass="select_way"
                        AppendDataBoundItems ="true" ToolTip="1" OnSelectedIndexChanged="HWList_SelectedIndexChanged" EnableViewState ="false" >
                        <asp:ListItem Text="請選擇" value ="0"></asp:ListItem>
                    </asp:DropDownList>
                    <h1>交流道<em>(起點)</em></h1>
                    <asp:DropDownList ID="ICListFrom7" runat="server" DataTextField="ICName" DataValueField="UID"  AutoPostBack="true"  CssClass="select_way"
                        EnableViewState ="false" DataSourceID="sdsInterChangeList7" OnSelectedIndexChanged="ICList_SelectedIndexChanged" >
                    </asp:DropDownList>
                </div>
                <div class="way_area_final">
                     <h1>交流道<em>(訖點)</em></h1>
                   <asp:DropDownList ID="ICListTo7" runat="server" DataSourceID="sdsInterChangeList7" DataTextField="ICName" DataValueField="UID"  AutoPostBack="false"  CssClass="select_way" ></asp:DropDownList>
                </div>
            </div>
			<div class="way_area" id="tr8" style="display:none">
                <h1>路段8</h1>
                <div class="way_area_start">
                    <h1>國道</h1>
                    <asp:DropDownList ID="HWList8" runat="server" DatasourceID="sdsHWList" DataTextField="HWName" DataValueField="UID" AutoPostBack="true" CssClass="select_way"
                        AppendDataBoundItems ="true" ToolTip="1" OnSelectedIndexChanged="HWList_SelectedIndexChanged" EnableViewState ="false" >
                        <asp:ListItem Text="請選擇" value ="0"></asp:ListItem>
                    </asp:DropDownList>
                    <h1>交流道<em>(起點)</em></h1>
                    <asp:DropDownList ID="ICListFrom8" runat="server" DataTextField="ICName" DataValueField="UID"  AutoPostBack="true"  CssClass="select_way"
                        EnableViewState ="false" DataSourceID="sdsInterChangeList8" OnSelectedIndexChanged="ICList_SelectedIndexChanged" >
                    </asp:DropDownList>
                </div>
                <div class="way_area_final">
                    <h1>交流道<em>(訖點)</em></h1>
                    <asp:DropDownList ID="ICListTo8" runat="server" DataSourceID="sdsInterChangeList8" DataTextField="ICName" DataValueField="UID"  AutoPostBack="false"  CssClass="select_way" ></asp:DropDownList>
                </div>
            </div>
			<div class="way_area" id="tr9" style="display:none">
                <h1>路段9</h1>
                <div class="way_area_start">
                    <h1>國道</h1>
                    <asp:DropDownList ID="HWList9" runat="server" DatasourceID="sdsHWList" DataTextField="HWName" DataValueField="UID" AutoPostBack="true" CssClass="select_way"
                        AppendDataBoundItems ="true" ToolTip="1" OnSelectedIndexChanged="HWList_SelectedIndexChanged" EnableViewState ="false" >
                        <asp:ListItem Text="請選擇" value ="0"></asp:ListItem>
                    </asp:DropDownList>
                    <h1>交流道<em>(起點)</em></h1>
                    <asp:DropDownList ID="ICListFrom9" runat="server" DataTextField="ICName" DataValueField="UID"  AutoPostBack="true"  CssClass="select_way"
                        EnableViewState ="false" DataSourceID="sdsInterChangeList9" OnSelectedIndexChanged="ICList_SelectedIndexChanged" >
                    </asp:DropDownList>
                </div>
                <div class="way_area_final">
                     <h1>交流道<em>(訖點)</em></h1>
                   <asp:DropDownList ID="ICListTo9" runat="server" DataSourceID="sdsInterChangeList9" DataTextField="ICName" DataValueField="UID"  AutoPostBack="false"  CssClass="select_way" ></asp:DropDownList>
                </div>
            </div>
			<div class="way_area" id="tr10" style="display:none">
                <h1>路段10</h1>
                <div class="way_area_start">
                    <h1>國道</h1>
                    <asp:DropDownList ID="HWList10" runat="server" DatasourceID="sdsHWList" DataTextField="HWName" DataValueField="UID" AutoPostBack="true" CssClass="select_way"
                        AppendDataBoundItems ="true" ToolTip="1" OnSelectedIndexChanged="HWList_SelectedIndexChanged" EnableViewState ="false" >
                        <asp:ListItem Text="請選擇" value ="0"></asp:ListItem>
                    </asp:DropDownList>
                    <h1>交流道<em>(起點)</em></h1>
                    <asp:DropDownList ID="ICListFrom10" runat="server" DataTextField="ICName" DataValueField="UID"  AutoPostBack="true"  CssClass="select_way"
                        EnableViewState ="false" DataSourceID="sdsInterChangeList10" OnSelectedIndexChanged="ICList_SelectedIndexChanged" >
                    </asp:DropDownList>
                </div>
                <div class="way_area_final">
                     <h1>交流道<em>(訖點)</em></h1>
                   <asp:DropDownList ID="ICListTo10" runat="server" DataSourceID="sdsInterChangeList10" DataTextField="ICName" DataValueField="UID"  AutoPostBack="false"  CssClass="select_way" ></asp:DropDownList>
                </div>
            </div>
			<div class="way_area" id="tr11" style="display:none">
                <h1>路段11</h1>
                <div class="way_area_start">
                    <h1>國道</h1>
                    <asp:DropDownList ID="HWList11" runat="server" DatasourceID="sdsHWList" DataTextField="HWName" DataValueField="UID" AutoPostBack="true" CssClass="select_way"
                        AppendDataBoundItems ="true" ToolTip="1" OnSelectedIndexChanged="HWList_SelectedIndexChanged" EnableViewState ="false" >
                        <asp:ListItem Text="請選擇" value ="0"></asp:ListItem>
                    </asp:DropDownList>
                    <h1>交流道<em>(起點)</em></h1>
                    <asp:DropDownList ID="ICListFrom11" runat="server" DataTextField="ICName" DataValueField="UID"  AutoPostBack="true"  CssClass="select_way"
                        EnableViewState ="false" DataSourceID="sdsInterChangeList11" OnSelectedIndexChanged="ICList_SelectedIndexChanged" >
                    </asp:DropDownList>
                </div>
                <div class="way_area_final">
                     <h1>交流道<em>(訖點)</em></h1>
                   <asp:DropDownList ID="ICListTo11" runat="server" DataSourceID="sdsInterChangeList11" DataTextField="ICName" DataValueField="UID"  AutoPostBack="false"  CssClass="select_way" ></asp:DropDownList>
                </div>
            </div>
			<div class="way_area" id="tr12" style="display:none">
                <h1>路段12</h1>
                <div class="way_area_start">
                    <h1>國道</h1>
                    <asp:DropDownList ID="HWList12" runat="server" DatasourceID="sdsHWList" DataTextField="HWName" DataValueField="UID" AutoPostBack="true" CssClass="select_way"
                        AppendDataBoundItems ="true" ToolTip="1" OnSelectedIndexChanged="HWList_SelectedIndexChanged" EnableViewState ="false" >
                        <asp:ListItem Text="請選擇" value ="0"></asp:ListItem>
                    </asp:DropDownList>
                    <h1>交流道<em>(起點)</em></h1>
                    <asp:DropDownList ID="ICListFrom12" runat="server" DataTextField="ICName" DataValueField="UID"  AutoPostBack="true"  CssClass="select_way"
                        EnableViewState ="false" DataSourceID="sdsInterChangeList12" OnSelectedIndexChanged="ICList_SelectedIndexChanged" >
                    </asp:DropDownList>
                </div>
                <div class="way_area_final">
                      <h1>交流道<em>(訖點)</em></h1>
                  <asp:DropDownList ID="ICListTo12" runat="server" DataSourceID="sdsInterChangeList12" DataTextField="ICName" DataValueField="UID"  AutoPostBack="false" CssClass="select_way"  ></asp:DropDownList>
                </div>
            </div>
			<div class="way_area" id="tr13" style="display:none">
                <h1>路段13</h1>
                <div class="way_area_start">
                    <h1>國道</h1>
                    <asp:DropDownList ID="HWList13" runat="server" DatasourceID="sdsHWList" DataTextField="HWName" DataValueField="UID" AutoPostBack="true" CssClass="select_way"
                        AppendDataBoundItems ="true" ToolTip="1" OnSelectedIndexChanged="HWList_SelectedIndexChanged" EnableViewState ="false" >
                        <asp:ListItem Text="請選擇" value ="0"></asp:ListItem>
                    </asp:DropDownList>
                    <h1>交流道<em>(起點)</em></h1>
                    <asp:DropDownList ID="ICListFrom13" runat="server" DataTextField="ICName" DataValueField="UID"  AutoPostBack="true"  CssClass="select_way"
                        EnableViewState ="false" DataSourceID="sdsInterChangeList13" OnSelectedIndexChanged="ICList_SelectedIndexChanged" >
                    </asp:DropDownList>
                </div>
                <div class="way_area_final">
                     <h1>交流道<em>(訖點)</em></h1>
                   <asp:DropDownList ID="ICListTo13" runat="server" DataSourceID="sdsInterChangeList13" DataTextField="ICName" DataValueField="UID"  AutoPostBack="false"  CssClass="select_way" ></asp:DropDownList>
                </div>
            </div>
			<div class="way_area" id="tr14" style="display:none">
                <h1>路段14</h1>
                <div class="way_area_start">
                    <h1>國道</h1>
                    <asp:DropDownList ID="HWList14" runat="server" DatasourceID="sdsHWList" DataTextField="HWName" DataValueField="UID" AutoPostBack="true" CssClass="select_way"
                        AppendDataBoundItems ="true" ToolTip="1" OnSelectedIndexChanged="HWList_SelectedIndexChanged" EnableViewState ="false" >
                        <asp:ListItem Text="請選擇" value ="0"></asp:ListItem>
                    </asp:DropDownList>
                    <h1>交流道<em>(起點)</em></h1>
                    <asp:DropDownList ID="ICListFrom14" runat="server" DataTextField="ICName" DataValueField="UID"  AutoPostBack="true"  CssClass="select_way"
                        EnableViewState ="false" DataSourceID="sdsInterChangeList14" OnSelectedIndexChanged="ICList_SelectedIndexChanged" >
                    </asp:DropDownList>
                </div>
                <div class="way_area_final">
                     <h1>交流道<em>(訖點)</em></h1>
                   <asp:DropDownList ID="ICListTo14" runat="server" DataSourceID="sdsInterChangeList14" DataTextField="ICName" DataValueField="UID"  AutoPostBack="false"  CssClass="select_way" ></asp:DropDownList>
                </div>
            </div>
			<div class="way_area" id="tr15" style="display:none">
                <h1>路段15</h1>
                <div class="way_area_start">
                    <h1>國道</h1>
                    <asp:DropDownList ID="HWList15" runat="server" DatasourceID="sdsHWList" DataTextField="HWName" DataValueField="UID" AutoPostBack="true" CssClass="select_way"
                        AppendDataBoundItems ="true" ToolTip="1" OnSelectedIndexChanged="HWList_SelectedIndexChanged" EnableViewState ="false" >
                        <asp:ListItem Text="請選擇" value ="0"></asp:ListItem>
                    </asp:DropDownList>
                    <h1>交流道<em>(起點)</em></h1>
                    <asp:DropDownList ID="ICListFrom15" runat="server" DataTextField="ICName" DataValueField="UID"  AutoPostBack="true"  CssClass="select_way"
                        EnableViewState ="false" DataSourceID="sdsInterChangeList15" OnSelectedIndexChanged="ICList_SelectedIndexChanged" >
                    </asp:DropDownList>
                </div>
                <div class="way_area_final">
                     <h1>交流道<em>(訖點)</em></h1>
                   <asp:DropDownList ID="ICListTo15" runat="server" DataSourceID="sdsInterChangeList15" DataTextField="ICName" DataValueField="UID"  AutoPostBack="false"  CssClass="select_way" ></asp:DropDownList>
                </div>
            </div>
			<div class="way_area" id="tr16" style="display:none">
                <h1>路段16</h1>
                <div class="way_area_start">
                    <h1>國道</h1>
                    <asp:DropDownList ID="HWList16" runat="server" DatasourceID="sdsHWList" DataTextField="HWName" DataValueField="UID" AutoPostBack="true" CssClass="select_way"
                        AppendDataBoundItems ="true" ToolTip="16" OnSelectedIndexChanged="HWList_SelectedIndexChanged" EnableViewState ="false" >
                        <asp:ListItem Text="請選擇" value ="0"></asp:ListItem>
                    </asp:DropDownList>
                    <h1>交流道<em>(起點)</em></h1>
                    <asp:DropDownList ID="ICListFrom16" runat="server" DataTextField="ICName" DataValueField="UID"  AutoPostBack="true"  CssClass="select_way"
                        EnableViewState ="false" DataSourceID="sdsInterChangeList16" OnSelectedIndexChanged="ICList_SelectedIndexChanged" >
                    </asp:DropDownList>
                </div>
                <div class="way_area_final">
                     <h1>交流道<em>(訖點)</em></h1>
                   <asp:DropDownList ID="ICListTo16" runat="server" DataSourceID="sdsInterChangeList16" DataTextField="ICName" DataValueField="UID"  AutoPostBack="false"  CssClass="select_way" ></asp:DropDownList>
                </div>
            </div>
			<div class="way_area" id="tr17" style="display:none">
                <h1>路段17</h1>
                <div class="way_area_start">
                    <h1>國道</h1>
                    <asp:DropDownList ID="HWList17" runat="server" DatasourceID="sdsHWList" DataTextField="HWName" DataValueField="UID" AutoPostBack="true" CssClass="select_way"
                        AppendDataBoundItems ="true" ToolTip="1" OnSelectedIndexChanged="HWList_SelectedIndexChanged" EnableViewState ="false" >
                        <asp:ListItem Text="請選擇" value ="0"></asp:ListItem>
                    </asp:DropDownList>
                    <h1>交流道<em>(起點)</em></h1>
                    <asp:DropDownList ID="ICListFrom17" runat="server" DataTextField="ICName" DataValueField="UID"  AutoPostBack="true"  CssClass="select_way"
                        EnableViewState ="false" DataSourceID="sdsInterChangeList17" OnSelectedIndexChanged="ICList_SelectedIndexChanged" >
                    </asp:DropDownList>
                </div>
                <div class="way_area_final">
                      <h1>交流道<em>(訖點)</em></h1>
                  <asp:DropDownList ID="ICListTo17" runat="server" DataSourceID="sdsInterChangeList17" DataTextField="ICName" DataValueField="UID"  AutoPostBack="false"  CssClass="select_way" ></asp:DropDownList>
                </div>
            </div>
			<div class="way_area" id="tr18" style="display:none">
                <h1>路段18</h1>
                <div class="way_area_start">
                    <h1>國道</h1>
                    <asp:DropDownList ID="HWList18" runat="server" DatasourceID="sdsHWList" DataTextField="HWName" DataValueField="UID" AutoPostBack="true" CssClass="select_way"
                        AppendDataBoundItems ="true" ToolTip="1" OnSelectedIndexChanged="HWList_SelectedIndexChanged" EnableViewState ="false" >
                        <asp:ListItem Text="請選擇" value ="0"></asp:ListItem>
                    </asp:DropDownList>
                    <h1>交流道<em>(起點)</em></h1>
                    <asp:DropDownList ID="ICListFrom18" runat="server" DataTextField="ICName" DataValueField="UID"  AutoPostBack="true"  CssClass="select_way"
                        EnableViewState ="false" DataSourceID="sdsInterChangeList18" OnSelectedIndexChanged="ICList_SelectedIndexChanged" >
                    </asp:DropDownList>
                </div>
                <div class="way_area_final">
                     <h1>交流道<em>(訖點)</em></h1>
                   <asp:DropDownList ID="ICListTo18" runat="server" DataSourceID="sdsInterChangeList18" DataTextField="ICName" DataValueField="UID"  AutoPostBack="false" CssClass="select_way"  ></asp:DropDownList>
                </div>
            </div>
			<div class="way_area" id="tr19" style="display:none">
                <h1>路段19</h1>
                <div class="way_area_start">
                    <h1>國道</h1>
                    <asp:DropDownList ID="HWList19" runat="server" DatasourceID="sdsHWList" DataTextField="HWName" DataValueField="UID" AutoPostBack="true" CssClass="select_way"
                        AppendDataBoundItems ="true" ToolTip="1" OnSelectedIndexChanged="HWList_SelectedIndexChanged" EnableViewState ="false" >
                        <asp:ListItem Text="請選擇" value ="0"></asp:ListItem>
                    </asp:DropDownList>
                    <h1>交流道<em>(起點)</em></h1>
                    <asp:DropDownList ID="ICListFrom19" runat="server" DataTextField="ICName" DataValueField="UID"  AutoPostBack="true"  CssClass="select_way"
                        EnableViewState ="false" DataSourceID="sdsInterChangeList19" OnSelectedIndexChanged="ICList_SelectedIndexChanged" >
                    </asp:DropDownList>
                </div>
                <div class="way_area_final">
                    <asp:DropDownList ID="ICListTo19" runat="server" DataSourceID="sdsInterChangeList19" DataTextField="ICName" DataValueField="UID"  AutoPostBack="false"  CssClass="select_way" ></asp:DropDownList>
                </div>
            </div>
			<div class="way_area" id="tr20" style="display:none">
                <h1>路段20</h1>
                <div class="way_area_start">
                    <h1>國道</h1>
                    <asp:DropDownList ID="HWList20" runat="server" DatasourceID="sdsHWList" DataTextField="HWName" DataValueField="UID" AutoPostBack="true" CssClass="select_way"
                        AppendDataBoundItems ="true" ToolTip="1" OnSelectedIndexChanged="HWList_SelectedIndexChanged" EnableViewState ="false" >
                        <asp:ListItem Text="請選擇" value ="0"></asp:ListItem>
                    </asp:DropDownList>
                    <h1>交流道<em>(起點)</em></h1>
                    <asp:DropDownList ID="ICListFrom20" runat="server" DataTextField="ICName" DataValueField="UID"  AutoPostBack="true"  CssClass="select_way"
                        EnableViewState ="false" DataSourceID="sdsInterChangeList20" OnSelectedIndexChanged="ICList_SelectedIndexChanged" >
                    </asp:DropDownList>
                </div>
                <div class="way_area_final">
                    <h1>交流道<em>(訖點)</em></h1>
                    <asp:DropDownList ID="ICListTo20" runat="server" DataSourceID="sdsInterChangeList20" DataTextField="ICName" DataValueField="UID"  AutoPostBack="false" CssClass="select_way"  ></asp:DropDownList>
                </div>
            </div>
        </div>
        <div align="right" style="padding-right:5px"><button type="button" class="btn btn-outline btn-success" onclick="javascript:AddDDL();">more</button></div>
    </div>
    <div class="btn_start"><a href="#" onclick="javascript:__doPostBack('ctl00$ContentPlaceHolder1$imgCalculate','');return false;"></a></div>
    <asp:ImageButton ID="imgCalculate" runat="server" ImageUrl = "images/btn_star.jpg" BorderWidth="0" width="204px" Height="57px" ToolTip="開始計算!"  onmouseover="this.src='images/btn_star_a.jpg'" onfocus="this.src='images/btn_star.jpg'" onmouseout="this.src='images/btn_star.jpg'" onblur="this.src='images/btn_star_a.jpg'" Visible="false"/>
    <uc5:PublishBlock ID="PublishBlock2" runat="server" BlockID="MHelpCus" />
    <input type="hidden" id="hidDDL" value="1" runat="server"/>
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
                </div>
           </div>
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

                                        <asp:SqlDataSource ID="sdsHWList" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                            SelectCommand="Select HWName, UID from HWList where IsOnline > 0 and @IsScan = 0  order by ItemOrder">
                                            <SelectParameters>
                                                <asp:Parameter Name="IsScan" DefaultValue="0" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        <asp:SqlDataSource ID="sdsInterChangeList1" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                            SelectCommand="Select ICName, UID from InterChangeList where HWUID = @HWUID and IsOnline > 0 and @IsScan = 0  and IsVirture> 0 order by ItemOrder">
                                            <SelectParameters>
                                            <asp:ControlParameter ControlID ="HWList1" Name="HWUID"  DefaultValue = "0" />
                                                <asp:Parameter Name="IsScan" DefaultValue="0" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        <asp:SqlDataSource ID="sdsInterChangeList2" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                            SelectCommand="Select ICName, UID from InterChangeList where HWUID = @HWUID and IsOnline > 0 and @IsScan = 0  and IsVirture> 0 order by ItemOrder">
                                            <SelectParameters>
                                            <asp:ControlParameter ControlID ="HWList2" Name="HWUID"  DefaultValue = "0" />
                                                <asp:Parameter Name="IsScan" DefaultValue="0" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        <asp:SqlDataSource ID="sdsInterChangeList3" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                            SelectCommand="Select ICName, UID from InterChangeList where HWUID = @HWUID and IsOnline > 0 and @IsScan = 0  and IsVirture> 0 order by ItemOrder">
                                            <SelectParameters>
                                            <asp:ControlParameter ControlID ="HWList3" Name="HWUID"  DefaultValue = "0" />
                                                <asp:Parameter Name="IsScan" DefaultValue="0" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        <asp:SqlDataSource ID="sdsInterChangeList4" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                            SelectCommand="Select ICName, UID from InterChangeList where HWUID = @HWUID and IsOnline > 0 and @IsScan = 0  and IsVirture> 0 order by ItemOrder">
                                            <SelectParameters>
                                            <asp:ControlParameter ControlID ="HWList4" Name="HWUID"  DefaultValue = "0" />
                                                <asp:Parameter Name="IsScan" DefaultValue="0" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        <asp:SqlDataSource ID="sdsInterChangeList5" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                            SelectCommand="Select ICName, UID from InterChangeList where HWUID = @HWUID and IsOnline > 0 and @IsScan = 0  and IsVirture> 0 order by ItemOrder">
                                            <SelectParameters>
                                            <asp:ControlParameter ControlID ="HWList5" Name="HWUID"  DefaultValue = "0" />
                                                <asp:Parameter Name="IsScan" DefaultValue="0" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        <asp:SqlDataSource ID="sdsInterChangeList6" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                            SelectCommand="Select ICName, UID from InterChangeList where HWUID = @HWUID and IsOnline > 0 and @IsScan = 0  and IsVirture> 0 order by ItemOrder">
                                            <SelectParameters>
                                            <asp:ControlParameter ControlID ="HWList6" Name="HWUID"  DefaultValue = "0" />
                                                <asp:Parameter Name="IsScan" DefaultValue="0" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        <asp:SqlDataSource ID="sdsInterChangeList7" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                            SelectCommand="Select ICName, UID from InterChangeList where HWUID = @HWUID and IsOnline > 0 and @IsScan = 0  and IsVirture> 0 order by ItemOrder">
                                            <SelectParameters>
                                            <asp:ControlParameter ControlID ="HWList7" Name="HWUID"  DefaultValue = "0" />
                                                <asp:Parameter Name="IsScan" DefaultValue="0" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        <asp:SqlDataSource ID="sdsInterChangeList8" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                            SelectCommand="Select ICName, UID from InterChangeList where HWUID = @HWUID and IsOnline > 0 and @IsScan = 0  and IsVirture> 0 order by ItemOrder">
                                            <SelectParameters>
                                            <asp:ControlParameter ControlID ="HWList8" Name="HWUID"  DefaultValue = "0" />
                                                <asp:Parameter Name="IsScan" DefaultValue="0" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        <asp:SqlDataSource ID="sdsInterChangeList9" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                            SelectCommand="Select ICName, UID from InterChangeList where HWUID = @HWUID and IsOnline > 0 and @IsScan = 0  and IsVirture> 0 order by ItemOrder">
                                            <SelectParameters>
                                            <asp:ControlParameter ControlID ="HWList9" Name="HWUID"  DefaultValue = "0" />
                                                <asp:Parameter Name="IsScan" DefaultValue="0" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        <asp:SqlDataSource ID="sdsInterChangeList10" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                            SelectCommand="Select ICName, UID from InterChangeList where HWUID = @HWUID and IsOnline > 0 and @IsScan = 0  and IsVirture> 0 order by ItemOrder">
                                            <SelectParameters>
                                            <asp:ControlParameter ControlID ="HWList10" Name="HWUID"  DefaultValue = "0" />
                                                <asp:Parameter Name="IsScan" DefaultValue="0" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        <asp:SqlDataSource ID="sdsInterChangeList11" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                            SelectCommand="Select ICName, UID from InterChangeList where HWUID = @HWUID and IsOnline > 0 and @IsScan = 0  and IsVirture> 0 order by ItemOrder">
                                            <SelectParameters>
                                            <asp:ControlParameter ControlID ="HWList11" Name="HWUID"  DefaultValue = "0" />
                                                <asp:Parameter Name="IsScan" DefaultValue="0" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        <asp:SqlDataSource ID="sdsInterChangeList12" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                            SelectCommand="Select ICName, UID from InterChangeList where HWUID = @HWUID and IsOnline > 0 and @IsScan = 0  and IsVirture> 0 order by ItemOrder">
                                            <SelectParameters>
                                            <asp:ControlParameter ControlID ="HWList12" Name="HWUID"  DefaultValue = "0" />
                                                <asp:Parameter Name="IsScan" DefaultValue="0" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        <asp:SqlDataSource ID="sdsInterChangeList13" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                            SelectCommand="Select ICName, UID from InterChangeList where HWUID = @HWUID and IsOnline > 0 and @IsScan = 0  and IsVirture> 0 and @IsScan = 0 order by ItemOrder">
                                            <SelectParameters>
                                            <asp:ControlParameter ControlID ="HWList13" Name="HWUID"  DefaultValue = "0" />
                                                <asp:Parameter Name="IsScan" DefaultValue="0" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        <asp:SqlDataSource ID="sdsInterChangeList14" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                            SelectCommand="Select ICName, UID from InterChangeList where HWUID = @HWUID and IsOnline > 0 and @IsScan = 0  and IsVirture> 0 order by ItemOrder">
                                            <SelectParameters>
                                            <asp:ControlParameter ControlID ="HWList14" Name="HWUID"  DefaultValue = "0" />
                                                <asp:Parameter Name="IsScan" DefaultValue="0" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        <asp:SqlDataSource ID="sdsInterChangeList15" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                            SelectCommand="Select ICName, UID from InterChangeList where HWUID = @HWUID and IsOnline > 0 and @IsScan = 0  and IsVirture> 0 order by ItemOrder">
                                            <SelectParameters>
                                            <asp:ControlParameter ControlID ="HWList15" Name="HWUID"  DefaultValue = "0" />
                                                <asp:Parameter Name="IsScan" DefaultValue="0" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        <asp:SqlDataSource ID="sdsInterChangeList16" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                            SelectCommand="Select ICName, UID from InterChangeList where HWUID = @HWUID and IsOnline > 0 and @IsScan = 0  and IsVirture> 0 order by ItemOrder">
                                            <SelectParameters>
                                            <asp:ControlParameter ControlID ="HWList16" Name="HWUID"  DefaultValue = "0" />
                                                <asp:Parameter Name="IsScan" DefaultValue="0" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        <asp:SqlDataSource ID="sdsInterChangeList17" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                            SelectCommand="Select ICName, UID from InterChangeList where HWUID = @HWUID and IsOnline > 0 and @IsScan = 0  and IsVirture> 0 order by ItemOrder">
                                            <SelectParameters>
                                            <asp:ControlParameter ControlID ="HWList17" Name="HWUID"  DefaultValue = "0" />
                                                <asp:Parameter Name="IsScan" DefaultValue="0" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        <asp:SqlDataSource ID="sdsInterChangeList18" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                            SelectCommand="Select ICName, UID from InterChangeList where HWUID = @HWUID and IsOnline > 0 and @IsScan = 0  and IsVirture> 0 order by ItemOrder">
                                            <SelectParameters>
                                            <asp:ControlParameter ControlID ="HWList18" Name="HWUID"  DefaultValue = "0" />
                                                <asp:Parameter Name="IsScan" DefaultValue="0" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        <asp:SqlDataSource ID="sdsInterChangeList19" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                            SelectCommand="Select ICName, UID from InterChangeList where HWUID = @HWUID and IsOnline > 0 and @IsScan = 0  and IsVirture> 0 order by ItemOrder">
                                            <SelectParameters>
                                            <asp:ControlParameter ControlID ="HWList19" Name="HWUID"  DefaultValue = "0" />
                                                <asp:Parameter Name="IsScan" DefaultValue="0" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        <asp:SqlDataSource ID="sdsInterChangeList20" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                            SelectCommand="Select ICName, UID from InterChangeList where HWUID = @HWUID and IsOnline > 0 and @IsScan = 0  and IsVirture> 0 order by ItemOrder">
                                            <SelectParameters>
                                            <asp:ControlParameter ControlID ="HWList20" Name="HWUID"  DefaultValue = "0" />
                                                <asp:Parameter Name="IsScan" DefaultValue="0" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
 
      <script type="text/javascript" language="javascript" >
          Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
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
          };


          function AddDDL() {
              if (document.getElementById('<%=hidDDL.ClientID%>').value > 20) {
                  alert("已達最多查詢路段數量!!");
                  return;
              }
              var i = 2;
              for (i = 2; i < 21; i++) {
                  var ddl = document.getElementById("tr" + i.toString());
                  if (ddl.style.display = "none") {
                      ddl.style.display = "block";
                      if (i <= document.getElementById('<%=hidDDL.ClientID%>').value)
                          continue;
                      else {
                          document.getElementById('<%=hidDDL.ClientID%>').value = i;
                          break;
                      }
                  }
              }
          }
          for (i = 2; i <= '<%=hidDDL.Value%>'; i++) {
              var ddl = document.getElementById("tr" + i.toString());
              if (ddl.style.display = "none") {
                  ddl.style.display = "block";
              }
          }
  </script>
  
</asp:Content>
