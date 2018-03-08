<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Publisher4.ascx.vb" Inherits="common_Publisher4" %>
<%@ Register Assembly="CutePager" Namespace="CutePager" TagPrefix="cc1" %>
<%@ Register Src="FileManager.ascx" TagName="FileManager" TagPrefix="uc1" %>
<%@ Register Src="FileManagerCounter.ascx" TagName="FileManagerCounter" TagPrefix="uc2" %>
        <div class="content" id="tblOne" runat="server">
            <h1><asp:Literal id="ltSubject" runat="server"></asp:Literal></h1>
            <asp:Literal ID="ltContent" runat="server"></asp:Literal>
            <uc1:FileManager ID="FileManager3" runat="server"  counterenable="1" />
              <span class="updatedTime">本頁最後更新日期：<asp:Literal ID="ltUpdateDateTime" runat="server"></asp:Literal></span>
              <div align="center" style="padding-right:5px"><button type="button" class="btn btn-outline btn-success" onclick="history.back()">回上一頁</button></div>
		</div>
<div class="content" id="tblList" runat="server">
          <div class="newsstyle"><ul>
  <asp:Repeater ID="rpt" runat="server">
      <ItemTemplate>
          <li><asp:HyperLink ID="hl_Subject" runat="server" ></asp:HyperLink><asp:literal ID="ltldate" runat="server"></asp:literal></li>
      </ItemTemplate>
  </asp:Repeater>
     </ul><div align="center" style="padding-right:5px"><button type="button" class="btn btn-outline btn-success" onclick="history.back()">回上一頁</button></div>
	</div>
    </div>
<asp:Panel ID="PublisherDetail" runat="server" ></asp:Panel> 


