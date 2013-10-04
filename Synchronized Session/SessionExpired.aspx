<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Synchronized_Session._Default" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    
</asp:Content>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <p style="color:red">
    Session has expired
    </p>
      
        <script type="text/javascript">
            //Disable the session, since why would we want to show the popup again?
            $(function () {
                window.EnableSessionExpiration = false;
            });
    </script>                      
</asp:Content>

