<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Success.aspx.cs" Inherits="Password_Hashing.Success" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
    <script>
        var count = 10;

        var counter = setInterval(timer, 1000); //1000 will  run it every 1 second

        function timer() {
            count = count - 1;
            if (count <= 0) {
                clearInterval(counter);
                alert("Session has timed out, please login again.");
                document.getElementById("btnLogout").click();
            }
        }
    </script>
<body>
    <form id="form1" runat="server">
    <div>
        <h2>User Profile</h2>
        <h2>User ID : <asp:Label ID="lbl_userID" runat="server"></asp:Label>
        </h2>
        <h2>First Name :&nbsp;
            <asp:Label ID="lbl_fname" runat="server"></asp:Label>
        </h2>
        <h2>Last Name :&nbsp;
            <asp:Label ID="lbl_lname" runat="server"></asp:Label>
        </h2>
        <h2>DOB:&nbsp;
            <asp:Label ID="lbl_dob" runat="server"></asp:Label>
        </h2>
        <h2>Credit Card :&nbsp;
            <asp:Label ID="lbl_credit" runat="server"></asp:Label>
        </h2>
        <asp:Button ID="btnLogout" runat="server" Text="Logout" OnClick="LogoutMe"/>
    </div>
    </form>
</body>

</html>
