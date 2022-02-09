<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="Password_Hashing.Registration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 282px;
        }
        .auto-style2 {
            height: 46px;
        }
        .auto-style3 {
            width: 282px;
            height: 46px;
        }
        .auto-style4 {
            height: 23px;
        }
        .auto-style5 {
            width: 282px;
            height: 23px;
        }
    </style>
    <script type="text/javascript">
        function validate() {
            var str = document.getElementById('<%=tb_pwd.ClientID%>').value;
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
    
        <asp:Label ID="Label1" runat="server" Text="Account Registration"></asp:Label>
        <br />
        <br />
   </h2>
        <table class="style1">
            <tr>
                <td class="style3">
        <asp:Label ID="lbl_fname" runat="server" Text="First Name"></asp:Label>
                </td>
                <td class="auto-style1">
                    <asp:TextBox ID="tb_fname" runat="server" Height="36px" Width="280px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="auto-style2">
                    <asp:Label ID="lbl_lname" runat="server" Text="Last Name"></asp:Label>
                </td>
                <td class="auto-style3">
                    <asp:TextBox ID="tb_lname" runat="server" Height="32px" Width="281px"></asp:TextBox>
                </td>
            </tr>
                        <tr>
                <td class="style3">
        <asp:Label ID="lbl_credit" runat="server" Text="Credit Card Info"></asp:Label>
                </td>
                <td class="auto-style1">
                    <asp:TextBox ID="tb_credit" runat="server" Height="32px" Width="281px"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1"
    ControlToValidate="tb_credit" runat="server" ForeColor="Red"
    ErrorMessage="Only Numbers allowed"
    ValidationExpression="\d+">
</asp:RegularExpressionValidator>
                </td>
            </tr>
                        <tr>
                <td class="style6">
        <asp:Label ID="lbl_email" runat="server" Text="Email Address"></asp:Label>
                </td>
                <td class="auto-style1">
                    <asp:TextBox ID="tb_email" runat="server" Height="32px" Width="281px" OnTextChanged="tb_credit_TextChanged" TextMode ="Email"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="tb_email"
    ForeColor="Red" ValidationExpression="^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
    Display = "Dynamic" ErrorMessage = "Invalid email address"/>
                </td>
            </tr>
                        <tr>
                <td class="style3">
        <asp:Label ID="lbl_password" runat="server" Text="Password"></asp:Label>
                </td>
                <td class="auto-style1">
                    <asp:TextBox ID="tb_pwd" runat="server" Height="32px" Width="281px" onkeyup="javascript:validate()" TextMode="Password"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="pwdcheck" runat="server" Text=""></asp:Label>
                </td>
            </tr>
                        <tr>
                <td class="auto-style4">
                    <asp:Label ID="lbl_dob" runat="server" Text="Date Of Birth"></asp:Label>
                </td>
                <td class="auto-style5">
                    <asp:TextBox ID="tb_dob" runat="server" Height="32px" Width="281px" placeholder="YYYY-MM-DD"></asp:TextBox>
                </td>
            </tr>
                        <tr>
                <td class="auto-style4">
                    <asp:Label ID="Photo" runat="server" Text="Photo"></asp:Label>
                </td>
                <td class="auto-style5">
                    <asp:TextBox ID="TextBox2" runat="server" Height="32px" Width="282px"></asp:TextBox>
                </td>
            </tr>
                        <tr>
                <td class="style4">
       
                </td>
                <td class="auto-style1">
    <asp:Button ID="btn_Submit" runat="server" Height="48px" 
        onclick="btn_Submit_Click" Text="Submit" Width="288px" />
                </td>
                <td>
                    <asp:Button onclick ="btn_login_click" Text ="Go to Login Page" runat="server" />
                </td>
            </tr>
            
    </table>
&nbsp;<br />
        <asp:Label ID="lb_error1" runat="server"></asp:Label>
        <br />
        <asp:Label ID="lb_error2" runat="server"></asp:Label>
    <br />
        <br />
    
    </div>
    </form>
</body>
</html>
