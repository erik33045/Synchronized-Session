<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Synchronized_Session._Default" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    
</asp:Content>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <p>
    Session will expire in 3 minutes. A warning will show when there are 2 minutes remaining.
    </p>
    <p>
    If you extend the session, the popup will disappear on the current tab and all other tabs sharing the session.
    </p>
    <p>
    If you do not, the current page and all pages sharing the same session will redirect to a session expired page.
    </p>
</asp:Content>
