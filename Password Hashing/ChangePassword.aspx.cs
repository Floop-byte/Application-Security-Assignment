using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Data.SqlClient;
using System.Data;

namespace Password_Hashing
{
    public partial class ChangePassword : System.Web.UI.Page
    {

        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        static string finalHash;
        static string salt;
        private object tb_pwd;
        static string finalHashWithDbSalt;
        static string finalHashWithDbLastSalt;
        static string finalHashWithDbLastSalt2;

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public void btn_Submit_click(object sender, EventArgs e)
        {
            string comment = checkPassword(HttpUtility.HtmlEncode(tb_newpwd.Text));
            if (comment.Length > 1)
            {
                string message = "alert('" + "Password  " + comment.ToString() + "')";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", message, true);
                return;
            }

            string pwd = HttpUtility.HtmlEncode(tb_newpwd.Text.ToString().Trim());
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] saltByte = new byte[8];

            rng.GetBytes(saltByte);
            salt = Convert.ToBase64String(saltByte);

            SHA512Managed hashing = new SHA512Managed();

            string pwdWithSalt = pwd + salt;
            byte[] plainHash = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwd));
            byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));

            finalHash = Convert.ToBase64String(hashWithSalt);

            string userid = HttpUtility.HtmlEncode(tb_email.Text);
            
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select * FROM ACCOUNT WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);

            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["Email"] != DBNull.Value)
                        {
                            if (reader["Email"].ToString() == userid.ToString())
                            {
                                string LastdbSalt = getDBLastSalt(userid);
                                string pwdWithLastDbSalt = pwd + LastdbSalt;
                                byte[] hashWithLastDbSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithLastDbSalt));
                                finalHashWithDbLastSalt = Convert.ToBase64String(hashWithLastDbSalt);

                                string LastdbSalt2 = getDBLastSalt(userid);
                                string pwdWithLastDbSalt2 = pwd + LastdbSalt2;
                                byte[] hashWithLastDbSalt2 = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithLastDbSalt2));
                                finalHashWithDbLastSalt2 = Convert.ToBase64String(hashWithLastDbSalt);
                                string lastpwhashwithsalt = reader["lastpwhash"].ToString();
                                string lastpwhashwithsalt2 = reader["lastpwhash2"].ToString();

                                if (finalHashWithDbLastSalt != lastpwhashwithsalt && finalHashWithDbLastSalt2 != lastpwhashwithsalt2)
                                {
                                    string currpwd = HttpUtility.HtmlEncode(tb_currpwd.Text.ToString().Trim());

                                    string dbHash = getDBHash(userid);
                                    string dbSalt = getDBSalt(userid);
                                    try
                                    {
                                        if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                                        {
                                            string currpwdWithSalt = currpwd + dbSalt;
                                            byte[] currhashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(currpwdWithSalt));
                                            string curruserHash = Convert.ToBase64String(currhashWithSalt);

                                            if (curruserHash.Equals(dbHash))
                                            {
                                                    changePw();
                                            }
                                            else
                                            {
                                                lbl_error.Text = "Userid or password is not valid. Please try again.";
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        throw new Exception(ex.ToString());
                                    }

                                    finally { }
                                }
                                else
                                {
                                    lbl_error.Text = "Password used before!";
                                }
                            }
                            else
                            {
                                lbl_error.Text = "Invaid Email";
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally
            {
                connection.Close();
            }
    }
        protected void changePw()
        {
            string lastpwhash = "";
            string lastpwhash2 = "";
            string lastpwsalt = "";
            string lastpwsalt2 = "";
            string userid = HttpUtility.HtmlEncode(tb_email.Text);
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select * FROM ACCOUNT WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);

            try
            {

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["Email"] != DBNull.Value)
                        {
                            if (reader["Email"].ToString() == userid.ToString())
                            {
                                lastpwhash2 = reader["lastpwhash"].ToString();
                                lastpwsalt2 = reader["lastpwsalt"].ToString();
                                lastpwhash = reader["PasswordHash"].ToString();
                                    lastpwsalt = reader["PasswordSalt"].ToString();
                            }
                            else
                            {
                                lbl_error.Text = "Password has been used before";
                            }
                        }
                        else
                        {
                            lbl_error.Text = "Invalid Email";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            try
                {
                    string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("UPDATE Account SET PasswordHash = @PasswordHash, lastpwhash = @lastpwhash, lastpwsalt = @lastpwsalt, lastpwhash2 = @lastpwhash2, lastpwsalt2 = @lastpwsalt2, PasswordSalt=@PasswordSalt WHERE Email=@Email"))
                        {
                            using (SqlDataAdapter sda = new SqlDataAdapter())
                            {
                                cmd.CommandType = CommandType.Text;
                                cmd.Parameters.AddWithValue("@PasswordHash", finalHash);
                                cmd.Parameters.AddWithValue("@lastpwhash", lastpwhash);
                                cmd.Parameters.AddWithValue("@lastpwhash2", lastpwhash2);
                                cmd.Parameters.AddWithValue("@lastpwsalt", lastpwsalt);
                                cmd.Parameters.AddWithValue("@lastpwsalt2", lastpwsalt2);
                                cmd.Parameters.AddWithValue("@PasswordSalt", salt);
                                cmd.Parameters.AddWithValue("@Email", userid);

                                cmd.Connection = con;
                                try
                                {
                                    con.Open();
                                    cmd.ExecuteNonQuery();
                                    lbl_error.Text = "Successful!";
                                }
                                catch (Exception ex)
                                {
                                //lbl_Error.Text = ex.ToString();
                                throw new Exception(ex.ToString());
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
            if (Regex.IsMatch(password, "[A-Z]"))
            {

            }
            else
            {
                comment += "Needs uppercase letter, A-Z |";
            }
            if (Regex.IsMatch(password, "[!#$%&'()*+,-./:;<=>?@^_`{|}~]"))
            {

            }
            else
            {
                comment += "Needs special character, e.g *!@#$% |";
            }
            if (Regex.IsMatch(password, "[0-9]"))
            {

            }
            else
            {
                comment += "Needs number, 0-9";
            }
            return comment;
        }
        protected string getDBSalt(string userid)
        {

            string s = null;

            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select PASSWORDSALT FROM ACCOUNT WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["PASSWORDSALT"] != null)
                        {
                            if (reader["PASSWORDSALT"] != DBNull.Value)
                            {
                                s = reader["PASSWORDSALT"].ToString();
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { connection.Close(); }
            return s;

        }

        protected string getDBLastSalt(string userid)
        {

            string s = null;

            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select LASTPWSALT FROM ACCOUNT WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["lastpwsalt"] != null)
                        {
                            if (reader["lastpwsalt"] != DBNull.Value)
                            {
                                s = reader["lastpwsalt"].ToString();
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { connection.Close(); }
            return s;

        }

        protected string getDBLastSalt2(string userid)
        {

            string s = null;

            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select LASTPWSALT2 FROM ACCOUNT WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["lastpwsalt2"] != null)
                        {
                            if (reader["lastpwsalt2"] != DBNull.Value)
                            {
                                s = reader["lastpwsalt2"].ToString();
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { connection.Close(); }
            return s;

        }

        protected string getDBHash(string userid)
        {

            string h = null;

            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select PasswordHash FROM Account WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["PasswordHash"] != null)
                        {
                            if (reader["PasswordHash"] != DBNull.Value)
                            {
                                h = reader["PasswordHash"].ToString();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { connection.Close(); }
            return h;
        }

        protected string getDBPwDateTime(string userid)
        {

            string s = null;

            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select PasswordCreationDate FROM ACCOUNT WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["PasswordCreationDate"] != null)
                        {
                            if (reader["PasswordCreationDate"] != DBNull.Value)
                            {
                                s = reader["PasswordCreationDate"].ToString();
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { connection.Close(); }
            return s;

        }
        protected string getDBLastHash(string userid)
        {

            string h = null;

            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select LASTPWHASH FROM Account WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["lastpwhash"] != null)
                        {
                            if (reader["lastpwhash"] != DBNull.Value)
                            {
                                h = reader["lastpwhash"].ToString();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { connection.Close(); }
            return h;
        }

        protected string getDBLastHash2(string userid)
        {

            string h = null;

            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select LASTPWHASH2 FROM Account WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["lastpwhash2"] != null)
                        {
                            if (reader["lastpwhash2"] != DBNull.Value)
                            {
                                h = reader["lastpwhash2"].ToString();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { connection.Close(); }
            return h;
        }
    }
}