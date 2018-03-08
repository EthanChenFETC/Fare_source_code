Imports System.Data
Imports System.Data.SqlClient

Partial Class SystemManager_AdminAnnounceEdit
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("PublishMode") Is Nothing And Not Session("PublisKey") Is Nothing Then
            'This is EditMode
            ReadBackContent()

            Me.PublishButton.Text = "確定更新"
        Else
            'This is NewMode
            Session("PublishMode") = "Add"

        End If
    End Sub

    Private Sub ReadBackContent()
        Dim PublicID As String = Session("PublisKey").ToString
        Dim sql As String = "SELECT Subject, Content FROM AdminAnnounce WHERE (PublicID = @PublicID)" ' & PublicID & ")"

        Using dr As SqlDataReader = ClassDB.GetDataReaderParam(sql, New SqlParameter("@PublicID", PublicID))
            Try
                If dr IsNot Nothing Then
                    If dr.Read Then
                        Me.SubjectTextBox.Text = dr("Subject").ToString
                        Me.FCKeditor3.Value = dr("Content").ToString
                    End If
                End If
            Catch ex As Exception
                WriteErrLog(ex, Me.Page)
            Finally

            End Try
        End Using
        Me.PublishButton.Text = "更新公告"

    End Sub

    Protected Sub PublishButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles PublishButton.Click
        Select Case Session("PublishMode").ToString
            Case "Add"
                Me.SDS_AdminAnnoce_Insert.InsertParameters("Content").DefaultValue = Me.FCKeditor3.Value
                Me.SDS_AdminAnnoce_Insert.Insert()
            Case "Edit"
                Me.SDS_AdminAnnoce_Insert.UpdateParameters("PublicID").DefaultValue = Session("PublisKey").ToString()
                Me.SDS_AdminAnnoce_Insert.UpdateParameters("Content").DefaultValue = Me.FCKeditor3.Value
                Me.SDS_AdminAnnoce_Insert.Update()
            Case Else
                'InterfaceBuilder.ChangeUserControlByPassFileName(Me.Page, "AdminAnnounceList.ascx", "ascx")
                InterfaceBuilder.TabGoPre(Me.Page)
        End Select

    End Sub

    Protected Sub SDS_AdminAnnoce_Insert_Inserted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceStatusEventArgs) Handles SDS_AdminAnnoce_Insert.Inserted
        If e.AffectedRows > 0 Then
            Session("PublishMode") = Nothing
            Session("PublisKey") = Nothing

            InterfaceBuilder.TabGoPre(Me.Page)
            'InterfaceBuilder.ChangeUserControlByPassFileName(Me.Page, "AdminAnnounceList.ascx", "ascx")
        End If
    End Sub

    Protected Sub SDS_AdminAnnoce_Insert_Updated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceStatusEventArgs) Handles SDS_AdminAnnoce_Insert.Updated
        If e.AffectedRows > 0 Then
            Session("PublishMode") = Nothing
            Session("PublisKey") = Nothing

            InterfaceBuilder.TabGoPre(Me.Page)
            'InterfaceBuilder.ChangeUserControlByPassFileName(Me.Page, "AdminAnnounceList.ascx", "ascx")
        End If
    End Sub
End Class
