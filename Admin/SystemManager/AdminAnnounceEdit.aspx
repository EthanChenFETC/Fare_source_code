<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="AdminAnnounceEdit.aspx.vb" Inherits="SystemManager_AdminAnnounceEdit" ValidateRequest="false" %>
<%@ Register TagPrefix="FCKeditorV2" Namespace="FredCK.FCKeditorV2" Assembly="FredCK.FCKeditorV2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="PublishPanel">
                <p>
                    <label class="LeftLabel">
                        文章標題：</label>
                    <asp:TextBox ID="SubjectTextBox" runat="server" CssClass="txt"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="SubjectTextBox"
                        Display="Dynamic" ErrorMessage="請填寫文章標題!"></asp:RequiredFieldValidator></p>
                <p>
                </p>
                <p>
                    <span onclick="document.all.ctl00_ContentPlaceHolder1_FCKeditor3___Frame.style.height='250px';"
                        style="cursor: hand"></span>
                </p>
                <FCKeditorV2:FCKeditor ID="FCKeditor3" runat="server" Height="300px" ToolbarStartExpanded="false"
                    Width="90%">
                </FCKeditorV2:FCKeditor>
                &nbsp;&nbsp;<br />
                <asp:Button ID="PublishButton" runat="server" CssClass="Btn" Text="新增公告" />&nbsp;
                <asp:SqlDataSource ID="SDS_AdminAnnoce_Insert" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                    DeleteCommand="DELETE FROM [Publication] WHERE [PublicID] = @PublicID" InsertCommand="INSERT INTO AdminAnnounce(Subject, Content, DepartmentID, UpdateUser, UpdateDateTime, PostUser, PostDateTime, PostDepartmentID) VALUES (@Subject, @Content, @DepartmentID, @UpdateUser, GETDATE(), @PostUser, GETDATE(), @PostDepartmentID)"
                    UpdateCommand="UPDATE AdminAnnounce SET Subject = @Subject, Content = @Content, UpdateUser = @UpdateUser, UpdateDateTime = GETDATE(), DepartmentID = @DepartmentID WHERE (PublicID = @PublicID)">
                    <DeleteParameters>
                        <asp:Parameter Name="PublicID" Type="Int32" />
                    </DeleteParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="Content" />
                        <asp:SessionParameter Name="UpdateUser" SessionField="UserID" />
                        <asp:SessionParameter DefaultValue="" Name="DepartmentID" SessionField="DepartmentID" />
                        <asp:ControlParameter ControlID="SubjectTextBox" Name="Subject" PropertyName="Text" />
                        <asp:Parameter Name="PublicID" />
                    </UpdateParameters>
                    <InsertParameters>
                        <asp:ControlParameter ControlID="SubjectTextBox" Name="Subject" PropertyName="Text" />
                        <asp:Parameter Name="Content" />
                        <asp:SessionParameter Name="DepartmentID" SessionField="DepartmentID" />
                        <asp:SessionParameter Name="UpdateUser" SessionField="UserID" />
                        <asp:SessionParameter Name="PostUser" SessionField="UserID" />
                        <asp:SessionParameter Name="PostDepartmentID" SessionField="DepartmentID" />
                    </InsertParameters>
                </asp:SqlDataSource>
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="PublicID"
                    DataSourceID="SDS_AdminAnnoce_List" Visible="False">
                    <Columns>
                        <asp:BoundField DataField="PublicID" HeaderText="PublicID" InsertVisible="False"
                            ReadOnly="True" SortExpression="PublicID" />
                        <asp:BoundField DataField="Subject" HeaderText="Subject" SortExpression="Subject" />
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="SDS_AdminAnnoce_List" runat="server" ConnectionString="<%$ ConnectionStrings:AdminModuleConnectionString %>"
                    DeleteCommand="DELETE FROM [AdminAnnounce] WHERE [PublicID] = @PublicID" InsertCommand="INSERT INTO AdminAnnounce(Subject, DepartmentID, UpdateUser, UpdateDateTime) VALUES (@Subject, @DepartmentID, @UpdateUser, @UpdateDateTime)"
                    SelectCommand="SELECT PublicID, Subject FROM AdminAnnounce ORDER BY PublicID DESC"
                    UpdateCommand="UPDATE AdminAnnounce SET Subject = @Subject, DepartmentID = @DepartmentID, UpdateUser = @UpdateUser, UpdateDateTime = @UpdateDateTime WHERE (PublicID = @PublicID)">
                    <DeleteParameters>
                        <asp:Parameter Name="PublicID" Type="Int32" />
                    </DeleteParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="Subject" Type="String" />
                        <asp:Parameter Name="DepartmentID" Type="Int32" />
                        <asp:Parameter Name="UpdateUser" Type="Int32" />
                        <asp:Parameter Name="UpdateDateTime" Type="DateTime" />
                        <asp:Parameter Name="PublicID" Type="Int32" />
                    </UpdateParameters>
                    <InsertParameters>
                        <asp:Parameter Name="Subject" Type="String" />
                        <asp:Parameter Name="Content" Type="String" />
                        <asp:Parameter Name="DepartmentID" Type="Int32" />
                        <asp:Parameter Name="UpdateUser" Type="Int32" />
                        <asp:Parameter Name="UpdateDateTime" Type="DateTime" />
                    </InsertParameters>
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

