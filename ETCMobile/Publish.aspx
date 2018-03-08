<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Publish.aspx.vb" Inherits="Publish" %>
<%@ Register Src="common/Publisher4.ascx" TagName="Publisher" TagPrefix="uc1" %>
    <asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 
        <uc1:Publisher ID="Publisher1" runat="server" />
    </asp:Content>

