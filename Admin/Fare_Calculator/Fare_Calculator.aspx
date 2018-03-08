<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Fare_Calculator.aspx.vb" Inherits="Fare_Calculator_Fare_Calculator" MasterPageFile="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    日期區間：<asp:TextBox runat="server" CssClass="form_txt" Columns="20" id=txtSdate onclick=showCalender(this);></asp:TextBox>
～
<asp:TextBox runat="server" CssClass="form_txt" Columns="20" id=txtEdate onclick=showCalender(this);></asp:TextBox>
<asp:Button
    ID="btnSearch" runat="server" Text="送出查詢" CssClass="Btn"></asp:Button>&nbsp;&nbsp;
<asp:Label
            ID="lblMessage" runat="server" EnableViewState="False" ForeColor="Red"></asp:Label>&nbsp;&nbsp;<br />
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="SDS_Fare_Calculator_List"
    AllowPaging="True" PageSize="12" DataKeyNames="LastUpdate">
        <Columns>
        <asp:BoundField DataField="LastUpdate" HeaderText="計算日期" SortExpression="CustomCount" ReadOnly="True" />      
        <asp:BoundField DataField="CustomCount" HeaderText="自訂行程" SortExpression="CustomCount" ReadOnly="True" />
        <asp:BoundField DataField="SuggestCount" HeaderText="最低費用行程" SortExpression="SuggestCount" ReadOnly="True" />
        <asp:BoundField DataField="MapTotalCount" HeaderText="地圖版當日總數" SortExpression="MapTotalCount" ReadOnly="True" />
        <asp:BoundField DataField="TotalCount" HeaderText="當日總數" SortExpression="TotalCount" ReadOnly="True" />
        <asp:BoundField DataField="MCustomCount" HeaderText="行動版自訂行程" SortExpression="MCustomCount" ReadOnly="True" />
        <asp:BoundField DataField="MSuggestCount" HeaderText="行動版最低費用行程" SortExpression="MSuggestCount" ReadOnly="True" />
        <asp:BoundField DataField="MTotalCount" HeaderText="行動版當日總數" SortExpression="MTotalCount" ReadOnly="True" />
        <asp:BoundField DataField="WTotalCount" HeaderText="總計" SortExpression="WTotalCount" ReadOnly="True" />
        </Columns>
     <EmptyDataTemplate>
        目前沒有任何資料！
    </EmptyDataTemplate>
    </asp:GridView>
    <asp:SqlDataSource ID="SDS_Fare_Calculator_List" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
         SelectCommand="select yy.LastUpdate, ISNULL(yy.CustomCount, 0) as CustomCount, ISNULL(yy.SuggestCount, 0) as SuggestCount, ISNULL(yy.MapTotalCount, 0) as MapTotalCount, ISNULL(yy.MCustomCount, 0) as MCustomCount, ISNULL(yy.MSuggestCount, 0) as MSuggestCount, ISNULL(yy.TotalCount, 0) as TotalCount, ISNULL(yy.MTotalCount, 0) as MTotalCount, ISNULL(yy.TotalCount,0) + ISNULL(yy.MTotalCount, 0)  as WTotalCount from (select a.LastUpdateDateTime as LastUpdate, (Select sum(h.MapCount) from FareCalculateCount h where h.LastUpdateDateTime = a.LastUpdateDateTime and h.SiteID = 1) as MapTotalCount , (Select sum(b.CustomCount) from FareCalculateCount b where b.LastUpdateDateTime = a.LastUpdateDateTime ) as CustomCount, (Select sum(c.MCustomCount) from FareCalculateCount c where c.LastUpdateDateTime = a.LastUpdateDateTime ) as MCustomCount, (Select sum(d.SuggestCount) from FareCalculateCount d where d.LastUpdateDateTime = a.LastUpdateDateTime) as SuggestCount, (Select sum(e.MSuggestCount) from FareCalculateCount e where e.LastUpdateDateTime = a.LastUpdateDateTime) as MSuggestCount, (Select sum(f.CustomCount+f.SuggestCount+f.MapCount) from FareCalculateCount f where f.LastUpdateDateTime = a.LastUpdateDateTime) as TotalCount, (Select sum(g.MCustomCount+g.MSuggestCount) from FareCalculateCount g where g.LastUpdateDateTime = a.LastUpdateDateTime) as MTotalCount from FareCalculateCount a where a.LastUpdateDateTime in (select Distinct LastUpdateDateTime as Lastu from FareCalculateCount) and (@sDate = '' or (Convert(varchar(20),Convert(datetime, @sDate),112) <= Convert(varchar(20),a.LastUpdate,112) and Convert(varchar(20),Convert(datetime, @eDate),112) >= Convert(varchar(20),a.LastUpdate,112))) group by a.LastUpdateDateTime) as yy order by yy.LastUpdate desc" SelectCommandType="text">
         <SelectParameters>
            <asp:ControlParameter Name="sDate" ControlID="txtSdate" PropertyName="Text" DefaultValue="2013/10/01"/>
            <asp:ControlParameter Name="eDate" ControlID="txtEdate" PropertyName="Text" DefaultValue="3000/12/31"/>
         </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>
