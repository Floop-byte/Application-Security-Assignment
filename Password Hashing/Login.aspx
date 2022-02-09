<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Password_Hashing.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
    <script src="https://www.google.com/recaptcha/api.js?render=6Ldg5F8eAAAAAB3eNlGsVs7gTOUlkR1XYsiC3KEv"></script>
    <script>
   function onSubmit(token) {
     document.getElementById("demo-form").submit();
   }
    </script>
<body>
       <form id="form1" runat="server">
    <h2>
        <br />
        <asp:Label ID="Label1" runat="server" Text="Login"></asp:Label>
        <br />
        <br />
   </h2>
        <table class="style1">
            <tr>
                <td class="style3">
        <asp:Label ID="Label2" runat="server" Text="User ID/Email"></asp:Label>
                </td>
                <td class="style2"> 
                    <asp:TextBox ID="tb_userid" runat="server" Height="16px" Width="280px" TextMode ="Email"></asp:TextBox>
                </td>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="tb_userid"
    ForeColor="Red" ValidationExpression="^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
    Display = "Dynamic" ErrorMessage = "Invalid email address"/>
            </tr>
            <tr>
                <td class="style3">
        <asp:Label ID="Label3" runat="server" Text="Password"></asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="tb_pwd" runat="server" Height="16px" Width="281px" TextMode="Password"></asp:TextBox>
                </td>
            </tr>
                        <tr>
                <td class="style3">
       
                </td>
                <td class="style2">
    <asp:Button ID="btn_Submit" runat="server" Height="48px" 
        onclick="btn_Submit_Click" Text="Submit" Width="288px" />
                </td>
                            <td>
                                <asp:Button ID="Button1" runat="server" Height="48px" 
        onclick=" btn_chgpw_click" Text="Change Password" Width="288px" />
                            </td>
            </tr>
    </table>
        <input type ="hidden" id="g-recaptcha-response" name="g-recaptcha-response" />
           <script>
               grecaptcha.ready(function () {
                   grecaptcha.execute('6Ldg5F8eAAAAAB3eNlGsVs7gTOUlkR1XYsiC3KEv', { action: 'Login' }).then(function (token) {
                       document.getElementById("g-recaptcha-response").value = token;
                   })
               })
           </script>
&nbsp;&nbsp;&nbsp;
    <br />
           <br />
        <asp:Label ID="lbl_error" runat="server"></asp:Label>
           <br />
           <br />
           <br />
        <br />
        <br />
   
    <div>
    
    </div>
    </form>
</body>
</html>
