<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="PublishList.aspx.vb" Inherits="Publish_PublishList" title="Untitled Page" %>
<%@ Register TagPrefix="pnwc" Namespace="PNayak.Web.UI.WebControls" Assembly="PNayak.Web.UI.WebControls.ExportButton" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                <tr>
                    <td>
                        <div class="SearchArea">
                            <p>
                            更新日期：<asp:TextBox runat="server" CssClass="form_txt" Columns="20" id="txtSdate" onclick="showCalender(this);" Text="2016/01/01"></asp:TextBox>
                                ～
                            <asp:TextBox runat="server" CssClass="form_txt" Columns="20" id="txtEdate" onclick="showCalender(this);" Text="3000/01/01"></asp:TextBox>
                                <br />
                            單位：<asp:DropDownList ID="ddlDepartment" runat="server" DataTextField="DepartmentName" DataValueField="DepartmentID" DataSourceID ="sdsDepartment" AppendDataBoundItems="true" AutoPostBack ="true">
                                <asp:ListItem Text="全部" Value="0"></asp:ListItem>
                            </asp:DropDownList>&nbsp;&nbsp;
                            姓名：<asp:DropDownList ID="ddlUsers" runat="server" DataTextField="Name" DataValueField="UserID" AppendDataBoundItems="true" >
                                <asp:ListItem Text="全部" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                                <br />
                            <asp:DropDownList ID="ddlSites" runat="server" AutoPostBack="True" DataValueField="SiteID" DataTextField="SiteName" DataSourceID ="sdsSites" AppendDataBoundItems="true" >
                                <asp:ListItem Text="全部" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:DropDownList ID="ddlSiteMap" runat="server" AutoPostBack="True">
                                <asp:ListItem Text="全部" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                             <asp:Label ID="lblMessage" runat="server" EnableViewState="False" ForeColor="Red"></asp:Label>
                                <br />
                                <asp:RadioButtonList ID="rblIsHistory" runat="server" AutoPostBack="True" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                <asp:ListItem Selected="True" Value="False">線上資料</asp:ListItem>
                                <asp:ListItem Value="True">歷史資料</asp:ListItem>
                            </asp:RadioButtonList>
                            資料搜尋：<asp:TextBox ID="txtSearch" runat="server" Columns="20" CssClass="form_txt"></asp:TextBox>
                            <asp:Button ID="btnSearch" runat="server" CssClass="Btn" Text="搜尋" />&nbsp;&nbsp;
                            <asp:Button ID="btnShowAll" runat="server" CssClass="Btn" Text="清除" />&nbsp;&nbsp;
                            
                            <pnwc:exportbutton id="Exportbutton1" runat="server"  CssClass="Btn" text="輸出Excel檔案" ></pnwc:exportbutton>
                            </p>

                            <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                DataKeyNames="PublicID" DataSourceID="SqlDS_Net2_Publication_Admin_GetList" PageSize="12">
                                <Columns>
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:Button ID="Button2" runat="server" CausesValidation="False" CommandName="Delete"
                                                OnClientClick="return confirm('您確定要刪除這筆資料？資料刪除無法回復！');" Text="刪除文章" />&nbsp;
                                            <asp:HiddenField ID="HiddenField_AttFiles" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="PublicID" HeaderText="編號" InsertVisible="False" ReadOnly="True"
                                        SortExpression="PublicID" />
                                    <asp:BoundField DataField="SiteName" HeaderText="網站" SortExpression="SiteName" />
                                    <asp:BoundField DataField="NodeText" HeaderText="單元" SortExpression="NodeText" />
                                    <asp:TemplateField HeaderText="文章" ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False"
                                                CommandName="EditPublish"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="超連結" SortExpression="SubjectLink" Visible="false">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="SubjectLink" runat="server" ></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImageLinkFile" runat="server" ImageUrl="~/common/AttFileIcons/icon_normal2.gif"
                                                Visible="False" CommandName="BtnLink" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="PostDate" HeaderText="上稿日" SortExpression="PostDate" />
                                    <asp:BoundField DataField="PublishDate" HeaderText="上架日期" ReadOnly="True" SortExpression="PublishDate" />
                                    <asp:BoundField DataField="PublishExpireDate" HeaderText="下架日期" ReadOnly="True" SortExpression="PublishExpireDate" />
                                    <asp:TemplateField HeaderText="附檔" SortExpression="AttFiles">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox1" runat="server" ></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Image ID="ImageAttFile" runat="server" ImageUrl="~/common/AttFileIcons/icon_normal2.gif"
                                                Visible="False" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ReviseStatus" HeaderText="審核狀態" ReadOnly="True" SortExpression="ReviseStatus" Visible="false"  />
                                
        <asp:TemplateField HeaderText="檢索設定" Visible="false" >
            <ItemTemplate>
                <asp:Button ID="BtnMeta" runat="server" Text="設定" CausesValidation="False" CommandName="BtnMeta" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="文章編輯" Visible="false">
            <ItemTemplate>
                <asp:Button ID="BtnPublish" runat="server" Text="編輯" CausesValidation="False" CommandName="BtnPublish"/>
            </ItemTemplate>
        </asp:TemplateField>

                                </Columns>
                                <EmptyDataTemplate>
                                    目前沒有任何您已發佈的資料！
                                </EmptyDataTemplate>
                            </asp:GridView>

                        </div>
                    </td>
                </tr>
            </table>

                            <asp:SqlDataSource ID="SqlDS_Net2_Publication_Admin_GetList" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                DeleteCommand="Net2_Publish_Del" DeleteCommandType="StoredProcedure" FilterExpression="Subject like '%{0}%' OR NodeText like '%{1}%'"
                                SelectCommand="Net2_Publication_Admin_GetList" SelectCommandType="StoredProcedure">
                                <FilterParameters>
                                    <asp:ControlParameter ControlID="txtSearch" Name="Subject" PropertyName="Text" />
                                    <asp:ControlParameter ControlID="txtSearch" Name="NodeText" PropertyName="Text" />
                                </FilterParameters>
                                <SelectParameters>
                                    <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32" />
                                    <asp:Parameter  Name="Admin" Type="Int32" />
                                    <asp:Parameter Name="SuperAdmin" Type="Int32" />
                                    <asp:ControlParameter ControlID="txtSDate" PropertyName="Text" DefaultValue="2016/01/01" Name="SDate" Type="String" />
                                    <asp:ControlParameter ControlID="txtEDate" PropertyName="Text" DefaultValue="3000/01/01" Name="EDate" Type="String" />
                                    <asp:ControlParameter ControlID="rblIsHistory" DefaultValue="False" Name="IsHistory"
                                        PropertyName="SelectedValue" Type="Boolean" />
                                    <asp:ControlParameter ControlID="ddlSites" DefaultValue="" Name="SiteID" PropertyName="SelectedValue"
                                        Type="int32" />
                                    <asp:ControlParameter ControlID="ddlSitemap" DefaultValue="" Name="NodeID" PropertyName="SelectedValue"
                                        Type="int32" />
                                    <asp:ControlParameter ControlID="ddlDepartment" DefaultValue="" Name="DepartmentID" PropertyName="SelectedValue"
                                        Type="int32" />
                                    <asp:ControlParameter ControlID="ddlUsers" DefaultValue="" Name="UserID" PropertyName="SelectedValue"
                                        Type="int32" />
                                </SelectParameters>
                                <DeleteParameters>
                                    <asp:Parameter Name="PublicID" Type="Int32" />
                                </DeleteParameters>
                            </asp:SqlDataSource>
                            <asp:SqlDataSource ID="Net2_Publication_Admin_GetList_Adminstrator" runat="server"
                                ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>" DeleteCommand="Net2_Publish_Del"
                                DeleteCommandType="StoredProcedure" FilterExpression="Subject like '%{0}%' OR NodeText like '%{1}%'"
                                SelectCommand="Net2_Publication_Admin_GetList_Adminstrator" SelectCommandType="StoredProcedure">
                                <FilterParameters>
                                    <asp:ControlParameter ControlID="txtSearch" Name="Subject" PropertyName="Text" />
                                    <asp:ControlParameter ControlID="txtSearch" Name="NodeText" PropertyName="Text" />
                                </FilterParameters>
                                <DeleteParameters>
                                    <asp:Parameter Name="PublicID" Type="Int32" />
                                </DeleteParameters>
                                <SelectParameters>
                                    <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32" />
                                    <asp:ControlParameter ControlID="rblIsHistory" DefaultValue="False" Name="IsHistory"
                                        PropertyName="SelectedValue" Type="Boolean" />
                                    <asp:ControlParameter ControlID="ddlSiteMap" Name="NodeID" PropertyName="SelectedValue"
                                        Type="Int32" />
                                    <asp:ControlParameter ControlID="ddlSites" Name="SiteID" PropertyName="SelectedValue"
                                        Type="Int32" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            
                            <asp:SqlDataSource ID="sds_Publish" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
    SelectCommand="Net2_Publication_Admin_GetList" SelectCommandType="StoredProcedure"
    DeleteCommand="Net2_Publish_Del" DeleteCommandType="StoredProcedure" FilterExpression="Subject like '%{0}%' OR NodeText like '%{1}%'">
    <FilterParameters>
        <asp:ControlParameter Name="Subject" ControlID="txtSearch" PropertyName="Text" />
        <asp:ControlParameter Name="NodeText" ControlID="txtSearch" PropertyName="Text" />
    </FilterParameters>
    <SelectParameters>
        <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32" />
        <asp:SessionParameter Name="UserID" SessionField="UserID" Type="Int32" />
        <asp:ControlParameter ControlID="rblIsHistory" DefaultValue="False" Name="IsHistory"
            PropertyName="SelectedValue" Type="Boolean" />
    </SelectParameters>
    <DeleteParameters>
        <asp:Parameter Name="PublicID" Type="Int32" />
    </DeleteParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sdsDepartment" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
        SelectCommand="Select DepartmentID, DepartmentName from Department order by ItemOrder" SelectCommandType="Text" >
        <SelectParameters>
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sdsUsers" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
        SelectCommand="Select UserID, Name from Accounts_Users where IsOnline>0 and DepartmentID = @DepartmentID" SelectCommandType="Text" >
        <SelectParameters>
            <asp:ControlParameter ControlID="ddlDepartment" Name="DepartmentID" PropertyName="SelectedValue" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sdsSites" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
        SelectCommand="Select SiteName, SiteID from Sites where IsOnline>0 order by SiteID" SelectCommandType="Text" >
    </asp:SqlDataSource>

</asp:Content>

