<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Section.aspx.vb" Inherits="SystemManager_Section" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                <asp:View ID="View1" runat="server">
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 65%">
                                所屬部門：<asp:DropDownList ID="ddl_Department" runat="server" AppendDataBoundItems="True"
                                    DataSourceID="sds_department" DataTextField="DepartmentName" DataValueField="DepartmentID"
                                    ValidationGroup="3">
                                    <asp:ListItem Value="0">選擇所屬部門</asp:ListItem>
                                </asp:DropDownList>
                                科別名稱：<asp:TextBox ID="txt_SectionName" runat="server" ValidationGroup="3"></asp:TextBox>&nbsp;
                                <asp:Button ID="btn_NewSection" runat="server" Text="新增科別" ValidationGroup="3" />
                                <asp:Label ID="lbMessage" runat="server" ForeColor="Red"></asp:Label>
                                <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="ddl_Department"
                                    Display="Dynamic" ErrorMessage="請選擇所屬部門！" MaximumValue="1000" MinimumValue="1"
                                    Type="Integer" ValidationGroup="3"></asp:RangeValidator>
                                <asp:RequiredFieldValidator ID="rfv2" runat="server" ControlToValidate="txt_SectionName"
                                    Display="Dynamic" ErrorMessage="請輸入科別編號！" ValidationGroup="3"></asp:RequiredFieldValidator>
                                <asp:SqlDataSource ID="sds_department" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                    ProviderName="<%$ ConnectionStrings:AdminModuleConnectionString.ProviderName %>"
                                    SelectCommand="SELECT DepartmentID, DepartmentName FROM Department ORDER BY ItemOrder">
                                </asp:SqlDataSource>
                            </td>
                            <td align="right" style="width: 35%">
                                搜尋：<asp:TextBox ID="txt_Search" runat="server"></asp:TextBox>
                                <asp:DropDownList ID="ddl_Department2" runat="server" AppendDataBoundItems="True"
                                    AutoPostBack="True" DataSourceID="sds_department" DataTextField="DepartmentName"
                                    DataValueField="DepartmentID">
                                    <asp:ListItem Value="%">全部部門</asp:ListItem>
                                </asp:DropDownList>
                                <asp:Button ID="btn_Search" runat="server" CausesValidation="False" Text="搜尋" />
                                <asp:Button ID="btn_Clear" runat="server" CausesValidation="False" Text="清除" /></td>
                        </tr>
                    </table>
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="SectionID"
                        DataSourceID="sds_section">
                        <Columns>
                            <asp:BoundField DataField="SectionID" HeaderText="科別編號" InsertVisible="False" ReadOnly="True"
                                SortExpression="SectionID">
                                <HeaderStyle Width="60px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="SectionName" HeaderText="科別名稱" SortExpression="SectionName" />
                            <asp:TemplateField HeaderText="所屬部門" SortExpression="DepartmentID">
                                <EditItemTemplate>
                                    <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="sds_dep" DataTextField="DepartmentName"
                                        DataValueField="DepartmentID" >
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="sds_dep" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                        ProviderName="<%$ ConnectionStrings:AdminModuleConnectionString.ProviderName %>"
                                        SelectCommand="SELECT DepartmentID, DepartmentName FROM Department ORDER BY ItemOrder">
                                    </asp:SqlDataSource>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="UpdateTime" HeaderText="更新時間" ReadOnly="True" SortExpression="UpdateTime" />
                            <asp:TemplateField HeaderText="編輯" ShowHeader="False">
                                <EditItemTemplate>
                                    <asp:Button ID="Button1" runat="server" CausesValidation="True" CommandName="UpdateItem"
                                        Text="更新" />
                                    <asp:Button ID="Button2" runat="server" CausesValidation="False" CommandName="Cancel"
                                        Text="取消" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Button ID="Button3" runat="server" CausesValidation="False" CommandName="Edit"
                                        Text="編輯" />
                                </ItemTemplate>
                                <HeaderStyle Width="60px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="刪除" ShowHeader="False">
                                <ItemTemplate>
                                    <asp:Button ID="Button4" runat="server" CausesValidation="False" CommandName="Delete"
                                        OnClientClick="return confirm('您確定要刪除本科別嗎?')" Text="刪除" />
                                </ItemTemplate>
                                <HeaderStyle Width="60px" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:SqlDataSource ID="sds_section" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                        DeleteCommand="DELETE FROM [Section] WHERE [SectionID] = @SectionID" InsertCommand="INSERT INTO Section(SectionName, DepartmentID, UpdateTime, UpdateUser, UpdateDep) VALUES (@SectionName, @DepartmentID, GETDATE(), @UpdateUser, @UpdateDep)"
                        ProviderName="<%$ ConnectionStrings:AdminModuleConnectionString.ProviderName %>"
                        SelectCommand="SELECT Section.SectionID, Section.SectionName, Section.DepartmentID, Department.DepartmentName, Section.UpdateTime, Department.DepartmentID AS Expr1 FROM Section INNER JOIN Department ON Section.DepartmentID = Department.DepartmentID WHERE (Section.SectionName LIKE N'%' + @Keyword + N'%') AND (CONVERT (varchar(5), Department.DepartmentID) LIKE N'%' + @DepartmentID + N'%') OR (CONVERT (varchar(5), Department.DepartmentID) LIKE @DepartmentID) AND (Department.DepartmentName LIKE N'%' + @Keyword + N'%') ORDER BY Department.ItemOrder"
                        UpdateCommand="UPDATE Section SET SectionName = @SectionName, DepartmentID = @DepartmentID, UpdateTime = GETDATE(), UpdateUser = @UpdateUser, UpdateDep = @UpdateDep WHERE (SectionID = @SectionID)">
                        <DeleteParameters>
                            <asp:Parameter Name="SectionID" Type="Int32" />
                        </DeleteParameters>
                        <UpdateParameters>
                            <asp:Parameter Name="SectionName" Type="String" />
                            <asp:Parameter Name="DepartmentID" Type="Int32" />
                            <asp:Parameter Name="SectionID" Type="Int32" />
                            <asp:SessionParameter Name="UpdateUser" SessionField="UserID" />
                            <asp:SessionParameter Name="UpdateDep" SessionField="DepartmentID" />
                        </UpdateParameters>
                        <InsertParameters>
                            <asp:ControlParameter ControlID="txt_SectionName" Name="SectionName" PropertyName="Text"
                                Type="String" />
                            <asp:ControlParameter ControlID="ddl_Department" Name="DepartmentID" PropertyName="SelectedValue"
                                Type="Int32" />
                            <asp:SessionParameter Name="UpdateUser" SessionField="UserID" />
                            <asp:SessionParameter Name="UpdateDep" SessionField="DepartmentID" />
                        </InsertParameters>
                        <SelectParameters>
                            <asp:ControlParameter ControlID="txt_Search" DefaultValue="%" Name="Keyword" PropertyName="Text" />
                            <asp:ControlParameter ControlID="ddl_Department2" DefaultValue="" Name="DepartmentID"
                                PropertyName="SelectedValue" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </asp:View>
            </asp:MultiView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

