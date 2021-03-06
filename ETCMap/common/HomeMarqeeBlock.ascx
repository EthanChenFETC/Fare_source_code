<%@ Control Language="VB" AutoEventWireup="false" CodeFile="HomeMarqeeBlock.ascx.vb" Inherits="common_HomeMarqeeBlock" %>
<%@ Register Src="FileManager.ascx" TagName="FileManager" TagPrefix="uc1" %>
            <div id="news">
            <span><img src="images/news.png" height="26" alt="News �ֳ�" /></span>
                <div id="news_marquee">
                    <ul>
                    <asp:Repeater  ID="Repeater1" runat="server"  DataSourceID="sds_news_marquee">
                        <itemtemplate>
                            <li><asp:HyperLink ID="hyperSubject" runat="server"  ></asp:HyperLink></li>
                        </itemtemplate>
                        <AlternatingItemTemplate >
                            <li><asp:HyperLink ID="hyperSubject" runat="server" ></asp:HyperLink></li>
                        </AlternatingItemTemplate>
                    </asp:Repeater>
                </ul> 
            </div>
        </div>
                                    <asp:SqlDataSource ID="sds_news_marquee" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
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
