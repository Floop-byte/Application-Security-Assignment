<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="Password_Hashing.ChangePassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
        .auto-style2 {
            width: 194px;
        }
        .auto-style3 {
            width: 194px;
            height: 23px;
        }
        .auto-style4 {
            width: 156px;
            height: 23px;
        }
        .auto-style5 {
            width: 156px;
        }
    </style>
    <script type="text/javascript">
        function validate() {
            var str = document.getElementById('<%=tb_newpwd.ClientID%>').value;
            if (str.length < 12) {
                document.getElementById("pwdcheck").innerHTML = "Password length must be at least 12 characters!";
                document.getElementById("pwdcheck").style.color = "Red";
                return ("too_short");
            }
            else if (str.search(/[0-9]/) == -1) {
                document.getElementById("pwdcheck").innerHTML = "Password must have at least one number!"
                document.getElementById("pwdcheck").style.color = "Red";
                return ("no_num")
            }
            else if (str.search(/[A-Z]/) == -1) {
                document.getElementById("pwdcheck").innerHTML = "Password must have at least one uppercase letter!"
                document.getElementById("pwdcheck").style.color = "Red";
                return ("no_upper")
            }
            else if (str.search(/[a-z]/) == -1) {
                document.getElementById("pwdcheck").innerHTML = "Password must have at least one lowercase letter!"
                document.getElementById("pwdcheck").style.color = "Red";
                return ("no_lower")
            }
            else if (str.search(/[!"#$%&'()*+,-./:;<=>?@[\]^_`{|}~]/) == -1) {
                document.getElementById("pwdcheck").innerHTML = "Password must have at least special character."
                document.getElementById("pwdcheck").style.color = "Red";
                return ("no_special")
            }
            else {
                document.getElementById("pwdcheck").innerHTML = "Very Strong Password"
                document.getElementById("pwdcheck").style.color = "Green";
                return ("strong_pwd")
            }
        }
        function MyFunction(msg) {
            alert(msg)
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table class="auto-style1">
                <tr>
                    <td class="auto-style3">Email: </td>
                    <td class="auto-style4">
                        <asp:TextBox ID="tb_email" runat="server" TextMode="Email"></asp:TextBox>
                    </td>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="tb_email"
    ForeColor="Red" ValidationExpression="^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
    Display = "Dynamic" ErrorMessage = "Invalid email address"/>
                </tr>
                <tr>
                    <td class="auto-style2">Current Password: </td>
                    <td class="auto-style5">
                        <asp:TextBox ID="tb_currpwd" runat="server" TextMode="Password" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style2">New Password:</td>
                    <td class="auto-style5">
                        <asp:TextBox ID="tb_newpwd" runat="server" onkeyup ="javascript:validate()" TextMode="Password"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="pwdcheck" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <asp:Button ID="btn_submit" onclick="btn_Submit_click" runat="server" Text="Button" />
        <br />
        <asp:Label ID="lbl_error" runat="server" Text=""></asp:Label>
    </form>
</body>
</html>
