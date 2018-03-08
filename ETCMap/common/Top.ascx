<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Top.ascx.vb" Inherits="common_TopArea" EnableViewState ="false"  %>
<div id="header">
    <a href="http://www.freeway.gov.tw" target="_blank"><img src="<%=Page.ResolveUrl("~/images/logo.png")%>" alt="國道高速公路局" width="130" height="90" border="0" /></a>
    <a href="<%=Page.ResolveUrl("~/Default.aspx") %>"><img src="<%=Page.ResolveUrl("~/images/toptitle.png") %>" alt="計程通行費試算" width="482" height="90" border="0"  /></a>
    
    <div align="right" class="toplinks">
                    <asp:Repeater  ID="Repeater1" runat="server"  DataSourceID="sds_TopMenu">
                        <itemtemplate>
                            <asp:HyperLink ID="hyperSubject" runat="server" NavigateUrl="~/Default.aspx"  ></asp:HyperLink>
                        </itemtemplate>
                        <SeparatorTemplate>
                            /
                        </SeparatorTemplate>
                    </asp:Repeater>
        <br />

    </div>
    <div id="cssmenu">
        <asp:Literal ID="ltMenu" runat="server"></asp:Literal>
    </div>
</div>   
                                      <asp:SqlDataSource ID="sds_TopMenu" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                        ProviderName="<%$ ConnectionStrings:AdminModuleConnectionString.ProviderName %>"
                                        SelectCommand="Select NodeID, Text, RefPath, Target from SiteMap where NodeID in (Select NodeID from SiteMap_Group where GroupID = @GroupID and SiteID = @SiteID) order by AllNodeOrder ">
                                        <SelectParameters>
                                            <asp:Parameter DefaultValue="0" Name="PublishInfoID" Type="Int32" />
                                            <asp:Parameter DefaultValue="12" Name="QTY" Type="Int32" />
                                            <asp:Parameter DefaultValue="1" Name="GroupID" Type="Int32" />
                                            <asp:Parameter DefaultValue="3" Name="SiteID" Type="Int32" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
