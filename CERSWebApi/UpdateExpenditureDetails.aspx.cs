using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace CERSWebApi
{
    public partial class UpdateExpenditureDetails : System.Web.UI.Page
    {
        public SqlConnection dbconnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConn"].ToString());
        protected SqlDataAdapter _adapter;
        protected DataSet _recourdDS = new DataSet();
        protected DataTable _dataTableTotal;

        string ExpenseID, expDate, expCode, amtType, amount, amountoutstanding, paymentDate, voucherBillNumber, payMode, payeeName,
            payeeAddress, sourceMoney, remarks, data;

        protected void Page_Load(object sender, EventArgs e)
        {
            Message message = new Message();
            System.Collections.Specialized.NameValueCollection nvc = Request.Form;

            ExpenseID = nvc["ExpenseID"];
            expDate = nvc["expDate"];
            expCode = nvc["expCode"];
            amtType = nvc["amtType"];
            amount = nvc["amount"];
            amountoutstanding = nvc["amountoutstanding"];
            paymentDate = nvc["paymentDate"];
            voucherBillNumber = nvc["voucherBillNumber"];
            payMode = nvc["payMode"];
            payeeName = nvc["payeeName"];
            payeeAddress = nvc["payeeAddress"];
            sourceMoney = nvc["sourceMoney"];
            remarks = nvc["remarks"];
            data = nvc["file"];
             
            ExpenseID = DecryptAES(ExpenseID);
            expDate = DecryptAES(expDate);
            expCode = DecryptAES(expCode);
            amtType = DecryptAES(amtType);
            amount = DecryptAES(amount);
            amountoutstanding = DecryptAES(amountoutstanding);
            paymentDate = DecryptAES(paymentDate);
            voucherBillNumber = DecryptAES(voucherBillNumber);
            payMode = DecryptAES(payMode);
            payeeName = DecryptAES(payeeName);
            payeeAddress = DecryptAES(payeeAddress);
            sourceMoney = DecryptAES(sourceMoney);
            remarks = DecryptAES(remarks);
            try
            {
                dbconnection.Open();
                SqlCommand cmd = new SqlCommand("sec.Mobile_updatesaveData", dbconnection);
                cmd.Parameters.AddWithValue("@ExpenseID", ExpenseID);
                cmd.Parameters.AddWithValue("@expDate", expDate);
                cmd.Parameters.AddWithValue("@expCode", expCode);
                cmd.Parameters.AddWithValue("@amtType", amtType);
                cmd.Parameters.AddWithValue("@amount", amount);
                cmd.Parameters.AddWithValue("@amountoutstanding", amountoutstanding);
                cmd.Parameters.AddWithValue("@paymentDate", paymentDate);
                cmd.Parameters.AddWithValue("@voucherBillNumber", voucherBillNumber);
                cmd.Parameters.AddWithValue("@payMode", payMode);
                cmd.Parameters.AddWithValue("@payeeName", payeeName);
                cmd.Parameters.AddWithValue("@payeeAddress", payeeAddress);
                cmd.Parameters.AddWithValue("@sourceMoney", sourceMoney);
                cmd.Parameters.AddWithValue("@remarks", remarks);
                if (data != null && data.Length > 0)
                {
                    cmd.Parameters.AddWithValue("@evidenceFile", Convert.FromBase64String(data));
                }
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

        public static String EncryptAES(String plainText)
        {
            string key = "CERS&NicHP@23@ece";
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
return  "";
                }
#endif
            }

        }

        public static String DecryptAES(String encryptedText)
        {
            string key = "CERS&NicHP@23@ece";
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
return  "";
                }
#endif
            }

        }

        private static RijndaelManaged getRijndaelManaged(String secretKey)
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