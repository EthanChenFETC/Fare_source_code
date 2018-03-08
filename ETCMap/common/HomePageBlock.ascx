<%@ Control Language="VB" AutoEventWireup="false" CodeFile="HomePageBlock.ascx.vb" Inherits="common_HomePageBlock" %>
<%@ Register Src="FileManager.ascx" TagName="FileManager" TagPrefix="uc1" %>
            <div id="indexNews">
            <p class="title18">程穝</p> 
                <table class="newsstyle" cellpadding = 0 cellspacing= 0>
                    <asp:Repeater  ID="Repeater1" runat="server"  DataSourceID="sds_Important_News">
                        <itemtemplate>
                        <tr>
                            <td><asp:HyperLink ID="hyperSubject" runat="server"  ></asp:HyperLink>
                                <asp:Image ID="ImageNew" runat="server" AlternateText="程穝New瓜ボ" align="absmiddle" Visible="false" />

                            </td>
                            <td nowrap="nowrap"><asp:Literal ID="lblDate" runat="server"></asp:Literal></td>
                        </tr>
                        </itemtemplate>
                        <AlternatingItemTemplate >
                            <td><asp:HyperLink ID="hyperSubject" runat="server" ></asp:HyperLink><asp:Image ID="ImageNew" runat="server" AlternateText="程穝New瓜ボ" align="absmiddle" Visible="false" /></td>
                            <td nowrap="nowrap"><asp:Literal ID="lblDate" runat="server"></asp:Literal></td>
                        </AlternatingItemTemplate>
                    </asp:Repeater>
                </table>
                <div align="right" style="padding-right:25px"><asp:Button ID="btnMore" runat="server" CssClass ="btn btn-outline btn-success" Text="more" ToolTip="程穝"  /></div>
            </div> 

                                    <asp:SqlDataSource ID="sds_Important_News" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                        ProviderName="<%$ ConnectionStrings:AdminModuleConnectionString.ProviderName %>"
                                        SelectCommand="Net2_F_HomeHeadBlock_List" SelectCommandType="StoredProcedure">
                                        <SelectParameters>
                                            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32" />
                                            <asp:Parameter DefaultValue="0" Name="PublishInfoID" Type="Int32" />
                                            <asp:Parameter DefaultValue="12" Name="QTY" Type="Int32" />
                                            <asp:Parameter DefaultValue="1" Name="GroupID" Type="Int32" />
                                            <asp:Parameter DefaultValue="3" Name="SiteID" Type="Int32" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
