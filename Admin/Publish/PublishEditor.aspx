<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="PublishEditor.aspx.vb" Inherits="Publish_PublishEditor2" ValidateRequest="false" %>

<%@ Register Src="../common/UploaderR2.ascx" TagName="UploaderR2" TagPrefix="uc3" %>

<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<%@ Register TagPrefix="uc1" TagName="SiteTree" Src="~/common/SiteTree.ascx" %>
<%@ Register TagPrefix="FCKeditorV2" Namespace="FredCK.FCKeditorV2" Assembly="FredCK.FCKeditorV2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<asp:Panel ID="show1" runat="server">
    <table cellspacing="0" cellpadding="5" width="100%" border="0">
        <tr>
            <td valign="top">
                <br />
                <uc1:SiteTree ID="SiteTree1" TreeViewHeight="270" TreeViewWidth="280" IsNeedNavigateUrl="false"
                    runat="server" TreeTitle="多向上稿"></uc1:SiteTree>
                <br />
                <!--<uc1:SiteTree ID="SiteTreeKnow" TreeViewHeight="160" TreeViewWidth="280" IsNeedNavigateUrl="false"
                    runat="server" TreeTitle="知識分類"></uc1:SiteTree> -->
            </td>
            <td class="td80percent" valign="top">
                <div class="PublishPanel">
                    <asp:Literal ID="ltlBR" runat="server"></asp:Literal>
                    <p>
                        <label class="LeftLabel">
                            <span style="color: #ff0000">*</span>文章標題：</label>
                        <asp:TextBox ID="SubjectTextBox" runat="server" CssClass="txt"></asp:TextBox><span
                            style="color: #ff0000"></span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="SubjectTextBox"
                            ErrorMessage="請填寫文章標題!" Display="Dynamic"></asp:RequiredFieldValidator>&nbsp;
                    </p>
                    <p>
                        <label class="LeftLabel">
                            置頂文章：</label>
                        <asp:RadioButtonList ID="rblImportant" runat="server" RepeatDirection="Horizontal"
                            RepeatLayout="Flow" Width="300px">
                            <asp:ListItem Value="1">是</asp:ListItem>
                            <asp:ListItem Selected="True" Value="0">否</asp:ListItem>
                        </asp:RadioButtonList>&nbsp; &nbsp;<span onclick="document.all.ctl00_ContentPlaceHolder1_FCKeditor3___Frame.style.height='550px';"
                            style="cursor: hand">(放大文字框)</span>&nbsp;<span onclick="document.all.ctl00_ContentPlaceHolder1_FCKeditor3___Frame.style.height='250px';"
                                style="cursor: hand">(還原文字框)</span></p>
                    <p>
                        <FCKeditorV2:FCKeditor ID="FCKeditor3" runat="server" ToolbarStartExpanded="false"
                            Height="300px" Width="100%">
                        </FCKeditorV2:FCKeditor>
                        <p>
                        </p>
                        <p>
                            <label class="LeftLabel">
                            <span style="color: #ff0000">*</span>上架日期：</label>
                            <ew:CalendarPopup ID="CalendarPopupPublishDate" runat="server" CssClass="txt40" Culture="Chinese (Taiwan)" DayNameFormat="Full" DisplayOffsetY="-100" PostedDate="" ShowGoToToday="True" UpperBoundDate="12/31/9999 23:59:59">
                            </ew:CalendarPopup>
                        </p>
                        <p>
                            <label class="LeftLabel">
                            下架日期：</label>
                            <ew:CalendarPopup ID="CalendarPopupExpireDate" runat="server" CssClass="txt40" Culture="Chinese (Taiwan)" DayNameFormat="Full" DisplayOffsetY="-100" GoToTodayText="今天日期:" Nullable="True" PostedDate="" SelectedValue="10/24/2006 22:04:24" ShowGoToToday="True" UpperBoundDate="12/31/9999 23:59:59">
                            </ew:CalendarPopup>
                        </p>
                        <p>
                            <label class="LeftLabel">
                            資料提供人員：</label>
                            <asp:Literal ID="ltlPostUser" runat="server"></asp:Literal>
                        </p>
                        <p>
                            <label class="LeftLabel">
                            資料提供日期：</label>
                            <asp:Literal ID="ltlPostDateTime" runat="server"></asp:Literal>
                        </p>
                        <p>
                            <label class="LeftLabel">
                            資料更新人員：</label>
                            <asp:Literal ID="ltlUpdateUser" runat="server"></asp:Literal>
                        </p>
                        <p>
                            <label class="LeftLabel">
                            最後更新日期：</label>
                            <asp:Literal ID="ltlUpdateDateTime" runat="server"></asp:Literal>
                        </p>
                        <p>
                            <uc3:UploaderR2 ID="UploaderR2a" runat="server" SavePath="Publish" />
                            <p>
                                <asp:Button ID="PublishButton" runat="server" CssClass="Btn" Text="新增文章" />
                                <input type="button" value="取消返回" onclick="javascript: window.history.back();" onkeypress="javascript: window.history.back();" class="Btn"  />
                                <asp:Button ID="btnPreview" runat="server" CssClass="Btn" Text="預覽" />
                            </p>
                            <div style="display:none">
                                <p>
                                    <asp:Button ID="TempInsertButton" runat="server" CssClass="Btn" Text="暫存文章" Visible="false" />
                                    &nbsp; &nbsp;
                                    <label class="SubjectLinkLabel">
                                    <span style="color: #ff0000"></span>文章標題超連結：</label>
                                    <asp:TextBox ID="SubjectLink" runat="server" CssClass="txt" Text=""></asp:TextBox>
                                    <span style="color: #ff0000"></span>
                                </p>
                                首頁圖檔 <span onclick="document.all.HomePhoto.style.display='BLOCK';" style="cursor: hand">(開啟)</span> <span onclick="document.all.HomePhoto.style.display='NONE';" style="cursor: hand">(關閉)</span>
                                <div id="HomePhoto" style="display: none">
                                    <asp:Image ID="HeadImage" runat="server" Visible="False" />
                                    <p>
                                        <label class="LeftLabel">
                                        首頁圖檔：</label>
                                        <asp:FileUpload ID="FileUploadHomePhoto" runat="server" />
                                        &nbsp;&nbsp;建議尺寸寬336pixel 高172pixel&nbsp;
                                        <asp:CheckBox ID="HotCheckBox" runat="server" Text="Hot!" Visible="False" />
                                    </p>
                                    <p>
                                        <label class="LeftLabel">
                                        圖檔說明：</label>
                                        <asp:TextBox ID="HomePhotoInfoTextBox" runat="server" CssClass="txt60"></asp:TextBox>
                                    </p>
                                </div>
                                <p>
                                    <label class="LeftLabel">
                                    <span style="color: #ff0000">*</span>知識關鍵字詞：</label>
                                    <asp:TextBox ID="KeywordTextBox" runat="server" CssClass="txt"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="KeywordTextBox" Display="Dynamic" ErrorMessage="請填寫知識關鍵字詞!"></asp:RequiredFieldValidator>
                                </p>
                            </div>
                            <p>
                            </p>
                            <p>
                            </p>
                        </p>
                    </p>
                    
                </div>
            </td>
        </tr>
    </table>
</asp:Panel>

                        <asp:SqlDataSource ID="SDS_Publish_Insert" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                            DeleteCommand="DELETE FROM [Publication] WHERE [PublicID] = @PublicID" InsertCommand="INSERT INTO [Publication] ([Subject], [Content], [Keyword], [PostDate], [PublishDate], [PublishExpireDate], [Hot], [AttFiles], [HomePhoto], [HomePhotoInfo], [UserID], [ReviseStatus], [UpdateUser], [DepartmentID], [UpdateDateTime], [RefPath], [PublishToDownload]) VALUES (@Subject, @Content, @Keyword, GetDate(), @PublishDate, @PublishExpireDate, @Hot, @AttFiles, @HomePhoto, @HomePhotoInfo, @UserID, @ReviseStatus, @UpdateUser, @DepartmentID, GetDate(), @RefPath, @PublishToDownload)"
                            SelectCommand="SELECT [Subject], [Content], [Keyword], [PostDate], [PublishDate], [PublishExpireDate], [Hot], [AttFiles], [PublicID], [HomePhoto], [HomePhotoInfo], [UserID], [ReviseStatus], [UpdateUser], [UpdateDateTime], [RefPath], [PublishToDownload] FROM [Publication]"
                            UpdateCommand="UPDATE [Publication] SET [Subject] = @Subject, [Content] = @Content, [Keyword] = @Keyword, [PostDate] = @PostDate, [PublishDate] = @PublishDate, [PublishExpireDate] = @PublishExpireDate, [Hot] = @Hot, [AttFiles] = @AttFiles, [HomePhoto] = @HomePhoto, [HomePhotoInfo] = @HomePhotoInfo, [UserID] = @UserID, [ReviseStatus] = @ReviseStatus, [UpdateUser] = @UpdateUser, [DepartmentID] = @DepartmentID, [UpdateDateTime] = @UpdateDateTime, [RefPath] = @RefPath, [PublishToDownload] = @PublishToDownload WHERE [PublicID] = @PublicID">
                            <DeleteParameters>
                                <asp:Parameter Name="PublicID" Type="Int32" />
                            </DeleteParameters>
                            <UpdateParameters>
                                <asp:Parameter Name="Subject" Type="String" />
                                <asp:Parameter Name="Content" Type="String" />
                                <asp:Parameter Name="Keyword" Type="String" />
                                <asp:Parameter Name="PostDate" Type="DateTime" />
                                <asp:Parameter Name="PublishDate" Type="DateTime" />
                                <asp:Parameter Name="PublishExpireDate" Type="DateTime" />
                                <asp:Parameter Name="Hot" Type="Boolean" />
                                <asp:Parameter Name="AttFiles" Type="String" />
                                <asp:Parameter Name="HomePhoto" Type="String" />
                                <asp:Parameter Name="HomePhotoInfo" Type="String" />
                                <asp:Parameter Name="UserID" Type="Int32" />
                                <asp:Parameter Name="ReviseStatus" Type="Int16" />
                                <asp:Parameter Name="UpdateUser" Type="Int32" />
                                <asp:Parameter Name="DepartmentID" Type="Int32" />
                                <asp:Parameter Name="UpdateDateTime" Type="DateTime" />
                                <asp:Parameter Name="RefPath" Type="String" />
                                <asp:Parameter Name="PublishToDownload" Type="Boolean" />
                                <asp:Parameter Name="PublicID" Type="Int32" />
                            </UpdateParameters>
                            <InsertParameters>
                                <asp:Parameter Name="Subject" Type="String" />
                                <asp:Parameter Name="Content" Type="String" />
                                <asp:Parameter Name="Keyword" Type="String" />
                                <asp:Parameter Name="PublishDate" Type="DateTime" />
                                <asp:Parameter Name="PublishExpireDate" Type="DateTime" />
                                <asp:Parameter Name="Hot" Type="Boolean" />
                                <asp:Parameter Name="AttFiles" Type="String" />
                                <asp:Parameter Name="HomePhoto" Type="String" />
                                <asp:Parameter Name="HomePhotoInfo" Type="String" />
                                <asp:SessionParameter Name="UserID" SessionField="UserID" Type="int32" />
                                <asp:Parameter Name="ReviseStatus" Type="Int16" />
                                <asp:SessionParameter Name="Updateuser" SessionField="UserID" Type="int32" />
                                <asp:SessionParameter Name="DepartmentID" SessionField="DepartmentID" Type="int32" />
                                <asp:Parameter Name="RefPath" Type="String" />
                                <asp:Parameter Name="PublishToDownload" Type="Boolean" />
                            </InsertParameters>
                        </asp:SqlDataSource>
                        <asp:SqlDataSource ID="SDS_Department_Name" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                            SelectCommand="SELECT [DepartmentName] FROM [Department] WHERE ([DepartmentID] = @DepartmentID)">
                            <SelectParameters>
                                <asp:SessionParameter Name="DepartmentID" SessionField="DepartmentID" Type="Int32" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                        <asp:SqlDataSource ID="SDS_User_Name" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                            SelectCommand="SELECT [Name] FROM [Accounts_Users] WHERE ([UserID] = @UserID)">
                            <SelectParameters>
                                <asp:SessionParameter Name="UserID" SessionField="UserID" Type="Int32" />
                            </SelectParameters>
                        </asp:SqlDataSource>
</asp:Content>
