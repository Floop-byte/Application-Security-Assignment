using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Text.RegularExpressions;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace Password_Hashing
{
    public partial class Registration : System.Web.UI.Page
    {

        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;

        static string line = "\r";

        //static string isDebug = ConfigurationManager.AppSettings["isDebug"].ToString();


        protected void Page_Load(object sender, EventArgs e)
        {

        }

            private string checkPassword(string password)
            {
                string comment = "";
                if (password.Length < 12)
                {
                    comment += "Too short, need 12 characters |";
                }
                if (Regex.IsMatch(password, "[a-z]"))
                {
                }
                else
                {
                    comment += "Needs lowercase letter, a-z |";
                }
                if(Regex.IsMatch(password, "[A-Z]"))
                {
                
                }
                else
                {
                    comment += "Needs uppercase letter, A-Z |";
                }
                if(Regex.IsMatch(password, "[!#$%&'()*+,-./:;<=>?@^_`{|}~]"))
                {
                
                }
                else
                {
                    comment += "Needs special character, e.g *!@#$% |";
                }
                if(Regex.IsMatch(password, "[0-9]"))
                {
                
                }
                else
                {
                    comment += "Needs number, 0-9";
                }
                return comment;
            }

        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            string comment = checkPassword(HttpUtility.HtmlEncode(tb_pwd.Text));
            if(comment.Length > 1)
            {
                string message = "alert('" + "Password  " + comment.ToString() + "')";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", message , true);
                return;
            }
            string pwd = HttpUtility.HtmlEncode(tb_pwd.Text.ToString().Trim());
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] saltByte = new byte[8];

            rng.GetBytes(saltByte);
            salt = Convert.ToBase64String(saltByte);

            SHA512Managed hashing = new SHA512Managed();

            string pwdWithSalt = pwd + salt;
            byte[] plainHash = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwd));
            byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));

            finalHash = Convert.ToBase64String(hashWithSalt);

            RijndaelManaged cipher = new RijndaelManaged();
            cipher.GenerateKey();
            Key = cipher.Key;
            IV = cipher.IV;


            createAccount();
        }


        protected void createAccount()
        {

            try
            {
                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Account VALUES(@FirstName, @LastName,@DOB,@Email,@CreditCard,@PasswordHash,@PasswordSalt,@DateTimeRegistered,@IV,@Key,@lastpwhash,@lastpwsalt,@lastpwhash2,@lastpwsalt2, @PasswordCreationDate)"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@FirstName", HttpUtility.HtmlEncode(tb_fname.Text.Trim()));
                            cmd.Parameters.AddWithValue("@LastName", HttpUtility.HtmlEncode(tb_lname.Text.Trim()));
                            cmd.Parameters.AddWithValue("@CreditCard", HttpUtility.HtmlEncode(Convert.ToBase64String( encryptData(tb_credit.Text.Trim()))));
                            cmd.Parameters.AddWithValue("@Email", HttpUtility.HtmlEncode(tb_email.Text.Trim()));
                            cmd.Parameters.AddWithValue("@PasswordHash", HttpUtility.HtmlEncode(finalHash));
                            cmd.Parameters.AddWithValue("@PasswordSalt", salt);
                            cmd.Parameters.AddWithValue("@DOB", HttpUtility.HtmlEncode(tb_dob.Text.Trim()));
                            cmd.Parameters.AddWithValue("@DateTimeRegistered", DateTime.Now);
                            cmd.Parameters.AddWithValue("@IV", Convert.ToBase64String(IV));
                            cmd.Parameters.AddWithValue("@Key", Convert.ToBase64String(Key));
                            cmd.Parameters.AddWithValue("@lastpwhash", "");
                            cmd.Parameters.AddWithValue("@lastpwsalt", "");
                            cmd.Parameters.AddWithValue("@lastpwhash2", "");
                            cmd.Parameters.AddWithValue("@lastpwsalt2", "");
                            cmd.Parameters.AddWithValue("@PasswordCreationDate", DateTime.Now);
                            cmd.Connection = con;
                            try
                            {
                                con.Open();
                                cmd.ExecuteNonQuery();
                                lb_error1.Text = "Success";
                            }
                            catch(Exception ex)
                            {
                                lb_error1.Text = "An error has occured.";
                            }
                            finally
                            {
                                con.Close();
                            }
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        protected byte[] encryptData(string data)
        {
            byte[] cipherText = null;
            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;
                ICryptoTransform encryptTransform = cipher.CreateEncryptor();
                byte[] plainText = Encoding.UTF8.GetBytes(data);
                cipherText = encryptTransform.TransformFinalBlock(plainText, 0, plainText.Length);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { }
            return cipherText;
        }

        protected void tb_pwd_TextChanged(object sender, EventArgs e)
        {

        }

        protected void tb_credit_TextChanged(object sender, EventArgs e)
        {

        }

        protected void btn_login_click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx", false);
        }
    }
}