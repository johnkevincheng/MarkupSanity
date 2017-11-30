<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="MarkupSanityTester.WebForm1" ValidateRequest="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <label for="<%= rawHtmlInput.ClientID %>">Input</label>
            <asp:TextBox ID="rawHtmlInput" runat="server" Height="200px" TextMode="MultiLine" Width="500px" />
        </div>
        <div>
            <asp:Button ID="sanitizeButton" runat="server" Text="Sanitize" OnClick="sanitizeButton_Click" />
            <asp:Button ID="measureButton" runat="server" OnClick="measureButton_Click" Text="Measure" />
        </div>
        <div style="border: 1px solid black; padding: 20px;">
            <asp:Literal ID="cleanHtmlDisplay" runat="server" />
        </div>
    </form>
</body>
</html>
