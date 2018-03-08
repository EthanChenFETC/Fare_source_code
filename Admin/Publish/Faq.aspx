<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="Faq.aspx.vb" Inherits="Publish_Faq" Title="Untitled Page"  ValidateRequest="false" %>


<%@ Register Src="../common/DatePicker.ascx" TagName="DatePicker" TagPrefix="uc5" %>

<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<%@ Register Src="~/common/UploaderR2.ascx" TagName="Uploader" TagPrefix="uc1" %>
<%@ Register TagPrefix="FCKeditorV2" Namespace="FredCK.FCKeditorV2" Assembly="FredCK.FCKeditorV2" %>
<%@ Register Src="~/common/ViewTextboxUser.ascx" TagName="ViewTextboxUser" TagPrefix="uc2" %>
<%@ Register Src="~/common/ViewTextboxDep.ascx" TagName="ViewTextboxDep" TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Literal ID="ShowJavaScript" runat="server"></asp:Literal>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="False" UpdateMode="Conditional">
        <ContentTemplate>   
                    <asp:MultiView ID="MultiView3" runat="server" >
                <asp:View ID="View3" runat="server">
            <table style="width: 100%">
                <tr>
                    <td style="width: 50%">
            <asp:Button ID="BtnAddNew" runat="server" Text="新增常見問題" /></td>
                    <td align="right" style="width: 50%; text-align: right">
           <asp:Label ID ="xx" runat="server"></asp:Label>  分類查詢資料搜尋：<asp:TextBox ID="txtSearch" runat="server" Columns="20" CssClass="form_txt"></asp:TextBox>
                        <asp:Button
                ID="btnSearch" runat="server" CssClass="Btn" Text="搜尋" OnClick="btnSearch_Click" />&nbsp;<asp:Button ID="btnShowAll"
                    runat="server" CssClass="Btn" Text="清除" OnClick="btnShowAll_Click" />
                        &nbsp;<asp:Label ID="lbMessage" runat="server" EnableViewState="False" ForeColor="Red"></asp:Label></td>
                </tr>
            </table>
            <asp:GridView ID="GridView1" runat="server" DataSourceID="sds_Faq_List" AutoGenerateColumns="False"
                AllowPaging="True" AllowSorting="True" DataKeyNames="PublicID">
                <Columns>
                    <asp:BoundField DataField="PublicID" HeaderText="編號" InsertVisible="False" ReadOnly="True"
                        SortExpression="PublicID" />
                    <asp:BoundField DataField="CateGoryName" HeaderText="分類名稱" SortExpression="CateGoryName" />
                    <asp:TemplateField HeaderText="問題" SortExpression="Subject">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server" ></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:LinkButton ID="LinkButton1" runat="server"
                                CommandName="EditItem" ></asp:LinkButton>                                
                                
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="建立日期" SortExpression="PostDate">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox3" runat="server" ></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server"></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle Width="60px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="下架日期" SortExpression="PublishExpireDate">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox2" runat="server" ></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" ></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle Width="60px" />
                    </asp:TemplateField>
        <asp:TemplateField HeaderText="檢索資料" Visible="false" >
            <ItemTemplate>
                <asp:Label ID="ChkData" runat="server"></asp:Label>
            </ItemTemplate>
                        <HeaderStyle Width="50px" />
        </asp:TemplateField>
      
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:Button ID="Button2" runat="server" Text="刪除" CausesValidation="False" CommandName="Delete"
                                OnClientClick="return confirm('您確定要刪除這筆問題資料？資料刪除無法回復！');"></asp:Button>&nbsp;
                            <asp:HiddenField ID="HiddenField_AttFiles" runat="server" Value='0' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:SqlDataSource ID="sds_Faq_List" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                DeleteCommand="Net2_Faq_Del" DeleteCommandType="StoredProcedure"
                InsertCommand="Net2_Faq_Insert"
                ProviderName="<%$ ConnectionStrings:AdminModuleConnectionString.ProviderName %>"
                SelectCommand="SELECT Faq.PublicID, Faq.CateGoryID, Faq.Subject, Faq.PostDate, FaqCatgry.CateGoryName, Faq.AttFiles, Faq.PublishExpireDate FROM Faq LEFT OUTER JOIN FaqCatgry ON Faq.CateGoryID = FaqCatgry.CateGoryID WHERE (Faq.Subject LIKE N'%' + @Keyword + N'%') OR (FaqCatgry.CateGoryName LIKE N'%' + @Keyword + N'%') ORDER BY Faq.PublicID DESC"
                UpdateCommand="UPDATE Faq SET CateGoryID = @CateGoryID, Subject = @Subject, Content = @Content, PublishDate = @PublishDate, PublishExpireDate = @PublishExpireDate, AttFiles = @AttFiles, UpdateUser = @UpdateUser, UpdateDateTime = GETDATE() WHERE (PublicID = @PublicID)"
                InsertCommandType="StoredProcedure">
                <DeleteParameters>
                    <asp:Parameter Name="PublicID" Type="Int32" />
                </DeleteParameters>
                <UpdateParameters>
                    <asp:ControlParameter ControlID="rblCatgry" Name="CateGoryID" PropertyName="SelectedValue" />
                    <asp:Parameter Name="Subject" />
                    <asp:Parameter Name="Content" />
                    <asp:Parameter Name="PublishDate" />
                    <asp:Parameter Name="PublishExpireDate" />
                    <asp:Parameter Name="AttFiles" />
                    <asp:SessionParameter Name="UpdateUser" SessionField="UserID" />
                    <asp:Parameter Name="PublicID" />
                </UpdateParameters>
                <InsertParameters>
                    <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32" />
                    <asp:ControlParameter ControlID="rblCatgry" Name="CateGoryID" PropertyName="SelectedValue"
                        Type="Int32" />
                    <asp:Parameter Name="Subject" Type="String" />
                    <asp:Parameter Name="Content" Type="String" />
                    <asp:Parameter Name="PublishDate" Type="DateTime" />
                    <asp:Parameter Name="PublishExpireDate" Type="DateTime" />
                    <asp:Parameter Name="AttFiles" Type="String" />
                    <asp:SessionParameter Name="UserID" SessionField="UserID" Type="Int32" />
                    <asp:SessionParameter Name="DepartmentID" SessionField="DepartmentID" Type="Int32" />
                    <asp:Parameter Direction="InputOutput" Name="retVal" Type="Int32" />
                </InsertParameters>
                <SelectParameters>
                    <asp:ControlParameter ControlID="txtSearch" DefaultValue="%" Name="Keyword" PropertyName="Text" />
                </SelectParameters>
            </asp:SqlDataSource>
            &nbsp;&nbsp;&nbsp;
            <br />            
            </asp:View>
            </asp:MultiView>
                <div name="FaqSet" id="FaqSet" ></div>
                <div id="MetaSet" name="MetaSet"></div>
            <asp:MultiView ID="MultiView1" runat="server" >
                <asp:View ID="View1" runat="server">
                        <table border="0" cellpadding="0" cellspacing="0" class="intranet" style="width: 100%">
                            <tr>
                                <td class="TopTitle" colspan="2">
                                    新增問答</td>
                            </tr>
                            <tr>
                                <td class="MidContent" colspan="2">
                                    <table>
                                        <tr>
                                            <td width="250">
                                                分類：
                                            </td>
                                            <td width="90%">
                                                <asp:RadioButtonList ID="rblCatgry" runat="server" DataSourceID="sds_catgry_list"
                                                    DataTextField="CateGoryName" DataValueField="CateGoryID" RepeatColumns="4" RepeatDirection="Horizontal">
                                                </asp:RadioButtonList><asp:SqlDataSource ID="sds_catgry_list" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                                    ProviderName="<%$ ConnectionStrings:AdminModuleConnectionString.ProviderName %>"
                                                    SelectCommand="SELECT CateGoryID, CateGoryName FROM FaqCatgry WHERE (IsOnline > 0 )">
                                                </asp:SqlDataSource>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="250">
                                                問題：
                                                <br />
                                            </td>
                                            <td width="90%">
                                                <asp:TextBox ID="txtSubject" runat="server" Width="80%"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td width="250">
                                                解答：</td>
                                            <td width="90%">
                                                <FCKeditorV2:FCKeditor ID="FCKeditor3" runat="server" Height="300px" ToolbarStartExpanded="false"
                                                    Width="100%">
                                                    &nbsp;</FCKeditorV2:FCKeditor>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="250">
                                                上架日期：&nbsp;
                                            </td>
                                            <td width="90%">
                                                <uc5:DatePicker ID="DatePicker1" runat="server" SetToday="true" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="250">
                                                下架日期：</td>
                                            <td width="90%">
                                                <uc5:DatePicker ID="DatePicker2" runat="server" AllowEmpty="true" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="250">
                                                資料提供單位：</td>
                                            <td width="90%">
                                                <uc3:ViewTextboxDep ID="ViewTextboxDep1" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="250">
                                                資料提供人員：</td>
                                            <td width="90%">
                                                <uc2:ViewTextboxUser ID="ViewTextboxUser1" runat="server" />
                                                <asp:Literal ID="ltl_UpdateDateTime" runat="server"></asp:Literal></td>
                                        </tr>
                                        <tr>
                                            <td width="250">
                                                <br />
                                                附檔上傳：</td>
                                            <td width="90%">
                                                <uc1:Uploader ID="Uploader1" runat="server" SavePath="Faq" SingleUpload="false">
                                                </uc1:Uploader>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="ButtomTitle" colspan="2">
                                                &nbsp;</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="ButtomTitle" colspan="2" style="height: 50px">
                                    <asp:SqlDataSource ID="sds_ReadBackContent" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                                        ProviderName="<%$ ConnectionStrings:AdminModuleConnectionString.ProviderName %>"
                                        SelectCommand="SELECT Faq.Subject, Faq.CateGoryID, Faq.Content, Faq.PublishDate, Faq.PublishExpireDate, Faq.AttFiles, Faq.ResponUser, Faq.ResponDepartment, Accounts_Users.UserName, Faq.PublicID FROM Faq CROSS JOIN Accounts_Users WHERE (Faq.PublicID = @PublicID)">
                                        <SelectParameters>
                                            <asp:Parameter Name="PublicID" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                    <asp:Button ID="btn_Insert" runat="server" Text="確定新增" /><asp:Button ID="btn_Update"
                                        runat="server" Text="確定更新" Visible="False" /><asp:Button ID="btn_Cancel" runat="server"
                                            Text="放棄返回"  CausesValidation="false" />
                                </td>
                            </tr>
                        </table>
                </asp:View>
            </asp:MultiView>
            
        </ContentTemplate>

    </asp:UpdatePanel>
    <a href="#FaqSet" id="cc" name="cc" ></a>
    <a href="#MetaSet" id="aa" name="aa" ></a>
    <asp:Literal  ID="Literal1" runat="server"></asp:Literal>     

</asp:Content>
