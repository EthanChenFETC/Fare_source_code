<%@ Page Language="VB" AutoEventWireup="true" title="錯誤頁面" %>
<%@ Import Namespace="System.Security.Cryptography" %>
<%@ Import Namespace="System.Threading" %>
<script runat="server">
    Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load 
        Dim delay As Byte() = New Byte(1) {}
        Dim prng As RandomNumberGenerator = New RNGCryptoServiceProvider()
        prng.GetBytes(delay)
        Thread.Sleep(CType(delay(0), Integer))
        Thread.Sleep(CType(delay(1), Integer))
        Dim disposable As IDisposable = TryCast(prng, IDisposable)
        If Not disposable Is Nothing Then
            disposable.Dispose()
        End If
    End Sub
</script>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server" >
    <title>錯誤頁面</title>
</head>
<body>
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
             <div style="border:#0066CC dashed 1px;margin:auto;font-size:13px;width:420px;height:180px;background:#fff; padding-right: 0px; padding-left: 0px; padding-bottom: 0px; padding-top: 0px;">
                 <div style="padding-right: 8px; padding-left: 8px; font-weight: bold; font-size: 17px; padding-bottom: 8px; color: #ffffff; padding-top: 8px; background-color: #0033cc">錯誤！</div>
                 <div style="padding-right: 8px; padding-left: 8px; padding-bottom: 8px; line-height: 24px; padding-top: 8px; overflow: visible;">非常抱歉，系統運作出現錯誤！<br />
                 <a href="Default.aspx">回到首頁</a>。</div></div>
</body>
</html>