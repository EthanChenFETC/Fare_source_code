<%@ Page Language="VB" AutoEventWireup="false" CodeFile="InterChangeFareEdit.aspx.vb" Inherits="Fare_Calculator_InterChangeFareEdit" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table cellspacing="0" cellpadding="5" width="100%" border="0">
    <tr>
        <td>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                   <div class="SearchArea">
                        ��Ʒj�M�G
                        <asp:TextBox ID="txtSearch" runat="server" CssClass="form_txt" Columns="20"></asp:TextBox>&nbsp;<asp:Button
                            ID="btnSearch" runat="server" Text="�j�M" CssClass="Btn"></asp:Button>&nbsp;<asp:Button
                                ID="btnShowAll" runat="server" Text="�M��" CssClass="Btn"></asp:Button>&nbsp;<asp:Label
                                    ID="lblMessage" runat="server" EnableViewState="False" ForeColor="Red"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                       (�O�v��W�١G<asp:TextBox ID="txtFareProjectName" runat="server" ValidationGroup ="add"  ></asp:TextBox>&nbsp;&nbsp;
                       �q�O�v��G<asp:DropDownList ID="ddlFareList" runat="server" DataSourceID ="sdsFareProject" DataValueField="UID" DataTextField="FareName" AppendDataBoundItems="true" ValidationGroup ="add" AutoPostBack="false"   >
                           <asp:ListItem Text="�s�W�O�v��" Value ="-1" Selected="True"></asp:ListItem> 
                       </asp:DropDownList>&nbsp;&nbsp;
                       <asp:Checkbox ID="ckbIsDefault" runat="server" Text="�O�_���w�]�зǶO�v" />&nbsp;&nbsp;
                       <asp:Button ID="btnAdd" runat="server" Text="�s�W" ValidationGroup ="add"  CssClass="Btn"/>)<br /></div>
                    <br />
                    <asp:GridView ID="GridView1" runat="server" CssClass="Dg" AutoGenerateColumns="False"
                        DataSourceID="sdsFareListProject" DataKeyNames="UID" AllowPaging="True"
                        AllowSorting="True" PageSize="20" >
                        <Columns>
                            <asp:TemplateField ShowHeader="False">
                                <EditItemTemplate>
                                    <asp:Button runat="server" Text="��s" CommandName="Update" CausesValidation="True"
                                        ID="Button1"></asp:Button>&nbsp;<asp:Button runat="server" Text="����" CommandName="Cancel"
                                            CausesValidation="False" ID="Button2"></asp:Button>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Button ID="BtnEdit" runat="server" Text="�s��" CausesValidation="False" CommandName="Edit">
                                    </asp:Button >&nbsp;
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="FareName" SortExpression="FareName"
                                HeaderText="�O�v��W��"></asp:BoundField>
                            <asp:CheckBoxField DataField ="IsDefault" SortExpression ="IsDefault" HeaderText="�O�_���w�]�зǶO�v" />
                            <asp:CheckBoxField DataField ="IsOnline" SortExpression ="IsDefault" HeaderText="�ҥ�" />
                            <asp:BoundField DataField="Notes" SortExpression="Notes"
                                HeaderText="�Ƶ�"></asp:BoundField>
                            <asp:BoundField ReadOnly="true" DataField="UpdateDateTime" SortExpression="UpdateDateTime"
                                HeaderText="��s�ɶ�"></asp:BoundField>
                            <asp:TemplateField HeaderText="�O�v��">
                                <ItemTemplate>
                                    <asp:Button ID="BtnFare" runat="server" Text="�O�v��ԲӤ��e" CausesValidation="False" CommandName="FareEdit" CssClass="Btn">
                                    </asp:Button >&nbsp;
                                    <asp:Button ID="BtnDel" runat="server" Text="�R��" CausesValidation="False"
                                        CommandName="DeleteItem" CommandArgument ='<%# Bind("UID") %>' OnClientClick="return confirm('�z�T�w�n�R���o����ơH��ƧR���L�k�^�_�I');"></asp:Button>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <br />
                    <asp:DropDownList ID="ddlHWList" runat="server" DataSourceID="sdsHWList" DataTextField="HWName" DataValueField ="UID" AppendDataBoundItems="true" AutoPostBack ="true" ValidationGroup="ICFare" Visible="false">
                            <asp:ListItem Text = "������D" Value = "0" Selected="True" ></asp:ListItem>
                        </asp:DropDownList> &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    <asp:DropDownList ID="ddlICList" runat="server" DataSourceID="sdsICList" DataTextField="ICName" DataValueField ="UID" AppendDataBoundItems="true" AutoPostBack ="true" ValidationGroup="ICFare" Visible="false">
                            <asp:ListItem Text = "������y�D" Value = "0" Selected="True" ></asp:ListItem>
                        </asp:DropDownList> &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    <asp:Button ID="btnAddFare" runat="server" Text="����O�v��" ValidationGroup ="addFare" Visible="false"  CssClass="Btn"/><asp:Label ID="lblFareAddMessage" runat="server" ForeColor="Red"></asp:Label>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    <asp:Button ID="btnCancel" runat="server" ValidationGroup="Cancel" Text="������^" Visible="false"  CssClass="Btn"/>
                    <asp:GridView ID="GridView2" runat="server" CssClass="Dg" AutoGenerateColumns="False"
                        DataSourceID="sdsInterChangeFare" DataKeyNames="UID" AllowPaging="True"
                        AllowSorting="True" PageSize="12" Visible ="false" >
                        <Columns>
                            <asp:TemplateField ShowHeader="False">
                                <EditItemTemplate>
                                    <asp:Button runat="server" Text="��s" CommandName="Update" CausesValidation="True"
                                        ID="Button1"></asp:Button>&nbsp;<asp:Button runat="server" Text="����" CommandName="Cancel"
                                            CausesValidation="False" ID="Button2"></asp:Button>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:Button runat="server" Text="�x�s" CommandName="Save" CausesValidation="True"
                                        ID="btnSave"></asp:Button>&nbsp;<asp:Button runat="server" Text="����" CommandName="CancelSave"
                                            CausesValidation="False" ID="btnSaveCancel"></asp:Button>
                                </FooterTemplate>
                                <ItemTemplate>
                                    <asp:Button ID="BtnEdit" runat="server" Text="�s��" CausesValidation="False" CommandName="Edit">
                                    </asp:Button >&nbsp;
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField ReadOnly="true" DataField="HWName" SortExpression="HWName"
                                HeaderText="��D�O"></asp:BoundField>
                            <asp:BoundField ReadOnly="true" DataField="ICName" SortExpression="ICName"
                                HeaderText="��y�D�W��"></asp:BoundField>
                            <asp:BoundField DataField="FareS" SortExpression="FareS"
                                HeaderText="�p���n�U�O�v"></asp:BoundField>
                            <asp:BoundField DataField="FareM" SortExpression="FareM"
                                HeaderText="�����n�U�O�v"></asp:BoundField>
                            <asp:BoundField DataField="FareG" SortExpression="FareG"
                                HeaderText="�j���n�U�O�v"></asp:BoundField>
                            <asp:BoundField DataField="FareSN" SortExpression="FareSN"
                                HeaderText="�p���_�W�O�v"></asp:BoundField>
                            <asp:BoundField DataField="FareMN" SortExpression="FareMN"
                                HeaderText="�����_�W�O�v"></asp:BoundField>
                            <asp:BoundField DataField="FareGN" SortExpression="FareGN"
                                HeaderText="�j���_�W�O�v"></asp:BoundField>
                            <asp:BoundField DataField="FareSDiff" SortExpression="FareSDiff"
                                HeaderText="�p���n�U�t�O�O�v"></asp:BoundField>
                            <asp:BoundField DataField="FareMDiff" SortExpression="FareMDiff"
                                HeaderText="�����n�U�t�O�O�v"></asp:BoundField>
                            <asp:BoundField DataField="FareGDiff" SortExpression="FareGDiff"
                                HeaderText="�j���n�U�t�O�O�v"></asp:BoundField>
                            <asp:BoundField DataField="FareSDiffN" SortExpression="FareSDiffN"
                                HeaderText="�p���_�W�t�O�O�v"></asp:BoundField>
                            <asp:BoundField DataField="FareMDiffN" SortExpression="FareMDiffN"
                                HeaderText="�����_�W�t�O�O�v"></asp:BoundField>
                            <asp:BoundField DataField="FareGDiffN" SortExpression="FareGDiffN"
                                HeaderText="�j���_�W�t�O�O�v"></asp:BoundField>
                            <asp:BoundField DataField="FareSAdd" SortExpression="FareSAdd"
                                HeaderText="�p���n�U�[���O�v"></asp:BoundField>
                            <asp:BoundField DataField="FareMAdd" SortExpression="FareMAdd"
                                HeaderText="�����n�U�[���O�v"></asp:BoundField>
                            <asp:BoundField DataField="FareGAdd" SortExpression="FareGAdd"
                                HeaderText="�j���n�U�[���O�v"></asp:BoundField>
                            <asp:BoundField DataField="FareSAddN" SortExpression="FareSAddN"
                                HeaderText="�p���_�W�[���O�v"></asp:BoundField>
                            <asp:BoundField DataField="FareMAddN" SortExpression="FareMAddN"
                                HeaderText="�����_�W�[���O�v"></asp:BoundField>
                            <asp:BoundField DataField="FareGAddN" SortExpression="FareGAddN"
                                HeaderText="�j���_�W�[���O�v"></asp:BoundField>
                        </Columns>
                    </asp:GridView>
            <br />
                  </ContentTemplate>
            </asp:UpdatePanel>
           <br /><br />
                   <asp:SqlDataSource ID="sdsHWList" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                        SelectCommand="SELECT UID, HWName from HWList where isonline > 0 order by itemorder"></asp:SqlDataSource>
                     <asp:SqlDataSource ID="sdsFareProject" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                        SelectCommand="select * from InterChangeFareListProject order by  ItemOrder, UID desc" SelectCommandType="Text">
                       <SelectParameters>
                        </SelectParameters>
                    </asp:SqlDataSource>
                   <asp:SqlDataSource ID="sdsICList" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                        SelectCommand="SELECT i.UID, i.ICName from InterChangeList i inner join HWList h on h.UID = i.HWUID where (@HWUID = 0 or HWUID = @HWUID) and i.isonline > 0 order by h.ItemOrder, i.itemorder">
                       <SelectParameters>
                           <asp:ControlParameter ControlID ="ddlHWList" Name ="HWUID" PropertyName="SelectedValue" DefaultValue="0" />
                        </SelectParameters>
                    </asp:SqlDataSource>

                     <asp:SqlDataSource ID="sdsFareListProject" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                        UpdateCommand="UPDATE InterChangeFareListProject set FareName=@FareName,IsDefault=@IsDefault,Notes = @Notes, IsOnline = @IsOnline,UpdateDateTime = getdate() where UID = @UID"
                        SelectCommand="select * from InterChangeFareListProject where (@UID < 0 or UID = @UID) and FareName Like N'%' + @FareName + N'%' order by UID desc  " SelectCommandType="Text">
                        <FilterParameters>
                            <asp:ControlParameter Name="FareName" ControlID="txtSearch" PropertyName="Text" />
                        </FilterParameters>
                         <UpdateParameters>
                            <asp:Parameter Type="Int32" Name="UID"></asp:Parameter>
                            <asp:Parameter Type="Boolean" Name="IsDefault"></asp:Parameter>
                            <asp:Parameter Type="Boolean" Name="IsOnline"></asp:Parameter>
                            <asp:Parameter Type="String" Name="FareName"></asp:Parameter>
                            <asp:Parameter Type="String" Name="Notes"></asp:Parameter>
                        </UpdateParameters>
                        <SelectParameters>
                            <asp:Parameter Type="int32" Name="UID" DefaultValue ="-1"></asp:Parameter>
                            <asp:ControlParameter Name="FareName" ControlID="txtSearch" PropertyName="Text" DefaultValue="%"/>
                        </SelectParameters>
                    </asp:SqlDataSource>

                     <asp:SqlDataSource ID="sdsInterChangeFare" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                        UpdateCommand="UPDATE InterChangeFareList set FareS=@FareS,FareM=@FareM,FareG=@FareG,FareSN=@FareSN,FareMN=@FareMN,FareGN=@FareGN,FareSDiff=@FareSDiff,FareMDiff=@FareMDiff,FareGDiff=@FareGDiff,FareSDiffN=@FareSDiffN,FareMDiffN=@FareMDiffN,FareGDiffN=@FareGDiffN,FareSAdd=@FareSAdd,FareMAdd=@FareMAdd,FareGAdd=@FareGAdd,FareSAddN=@FareSAddN,FareMAddN=@FareMAddN,FareGAddN=@FareGAddN, UpdateDateTime = getdate() where UID = @UID"
                        SelectCommand="select ic.HWUID,  h.HWName, ic.ICName, f.* from InterChangeList ic inner join HWList h on h.UID = ic.HWUID inner join InterChangeFareList f on f.ICUID = ic.UID where f.FareListID = @FareListID and (@HWUID=0 or ic.HWUID = @HWUID)  and (@ICUID=0 or f.ICUID = @ICUID) order by h.ItemOrder, ic.ItemOrder  " SelectCommandType="Text">
                         <UpdateParameters>
                            <asp:Parameter Type="Int32" Name="UID"></asp:Parameter>
                            <asp:Parameter Type="Double" Name="FareS"></asp:Parameter>
                            <asp:Parameter Type="Double" Name="FareM"></asp:Parameter>
                            <asp:Parameter Type="Double" Name="FareG"></asp:Parameter>
                            <asp:Parameter Type="Double" Name="FareSN"></asp:Parameter>
                            <asp:Parameter Type="Double" Name="FareMN"></asp:Parameter>
                            <asp:Parameter Type="Double" Name="FareGN"></asp:Parameter>
                            <asp:Parameter Type="Double" Name="FareSDiff"></asp:Parameter>
                            <asp:Parameter Type="Double" Name="FareMDiff"></asp:Parameter>
                            <asp:Parameter Type="Double" Name="FareGDiff"></asp:Parameter>
                            <asp:Parameter Type="Double" Name="FareSDiffN"></asp:Parameter>
                            <asp:Parameter Type="Double" Name="FareMDiffN"></asp:Parameter>
                            <asp:Parameter Type="Double" Name="FareGDiffN"></asp:Parameter>
                            <asp:Parameter Type="Double" Name="FareSAdd"></asp:Parameter>
                            <asp:Parameter Type="Double" Name="FareMAdd"></asp:Parameter>
                            <asp:Parameter Type="Double" Name="FareGAdd"></asp:Parameter>
                            <asp:Parameter Type="Double" Name="FareSAddN"></asp:Parameter>
                            <asp:Parameter Type="Double" Name="FareMAddN"></asp:Parameter>
                            <asp:Parameter Type="Double" Name="FareGAddN"></asp:Parameter>
                        </UpdateParameters>
                        <SelectParameters>
                            <asp:Parameter Type="int32" Name="FareListID"></asp:Parameter>
                            <asp:ControlParameter Name="HWUID" ControlID="ddlHWList" PropertyName="SelectedValue" DefaultValue="0"/>
                            <asp:ControlParameter Name="ICUID" ControlID="ddlICList" PropertyName="SelectedValue" DefaultValue="0"/>
                        </SelectParameters>
                    </asp:SqlDataSource>
         </td>
    </tr>
</table>
</asp:Content> 