using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web.Script.Serialization;

namespace CERSWebApi
{
    public partial class FinalSubmitExpenditureNov23 : System.Web.UI.Page
    {
        public SqlConnection dbconnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConn"].ToString());
        protected SqlDataAdapter _adapter;
        protected DataSet _recourdDS = new DataSet();
        protected DataTable _dataTableTotal;
        protected static string key = "CERS&NicHP@23@ece";
        string AutoID;

        protected void Page_Load(object sender, EventArgs e)
        {
            Message message = new Message();
            System.Collections.Specialized.NameValueCollection nvc = Request.Form;

            AutoID = nvc["AutoID"];
            AutoID = DecryptAES(AutoID);

            try
            {
                dbconnection.Open();
                SqlCommand cmd = new SqlCommand("sec.Mobile_finalsaveDataNov23", dbconnection);
                cmd.Parameters.AddWithValue("@AutoID", AutoID);

                _adapter = new SqlDataAdapter(cmd);
                cmd.CommandType = CommandType.StoredProcedure;
                DataSet _RecourdDS = new DataSet();
                _adapter.Fill(_RecourdDS);
                DataTable _messageTableRecords = _RecourdDS.Tables[0];
                _messageTableRecords.TableName = "MessageTable";
                foreach (DataRow dtDataRow in _messageTableRecords.Rows)
                {
                    message.status = dtDataRow["statuscode"].ToString();
                    message.message = dtDataRow["Msg"].ToString();
                }

                dbconnection.Close();
            }
            catch (Exception exp)
            {
                message.status = "400";
                message.message = exp.Message.ToString();
                dbconnection.Close();
            }
            responseData.Text = new JavaScriptSerializer().Serialize(message);
        }

        public static string EncryptAES(string plainText)
        {
            try
            {
#if DEBUG
                {
                    return plainText;
                }
#else
                {
                    var plainBytes = Encoding.UTF8.GetBytes(plainText);
                    return Convert.ToBase64String(Encrypt(plainBytes, getRijndaelManaged(key)));
                }
#endif

            }
            catch (Exception)
            {
#if DEBUG
                {
                    return plainText;
                }
#else
                {
                    return "";
                }
#endif
            }

        }

        public static string DecryptAES(string encryptedText)
        {
            try
            {

                var encryptedBytes = Convert.FromBase64String(encryptedText);
                return Encoding.UTF8.GetString(Decrypt(encryptedBytes, getRijndaelManaged(key)));
            }
            catch (Exception)
            {
#if DEBUG
                {
                    return encryptedText;
                }
#else
                {
                    return "";
                }
#endif
            }

        }

        private static RijndaelManaged getRijndaelManaged(string secretKey)
        {
            var keyBytes = new byte[16];
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
            Array.Copy(secretKeyBytes, keyBytes, Math.Min(keyBytes.Length, secretKeyBytes.Length));
            return new RijndaelManaged
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                KeySize = 128,
                BlockSize = 128,
                Key = keyBytes,
                IV = keyBytes
            };
        }

        private static byte[] Encrypt(byte[] plainBytes, RijndaelManaged rijndaelManaged)
        {
            return rijndaelManaged.CreateEncryptor()
                .TransformFinalBlock(plainBytes, 0, plainBytes.Length);
        }

        private static byte[] Decrypt(byte[] encryptedData, RijndaelManaged rijndaelManaged)
        {
            return rijndaelManaged.CreateDecryptor()
                .TransformFinalBlock(encryptedData, 0, encryptedData.Length);
        }

        public class Message
        {
            public string status { get; set; }
            public string message { get; set; }

        }

    }
}