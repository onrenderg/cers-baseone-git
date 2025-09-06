using CERSWebApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace CERSWebApi.Controllers
{
    public class AddExpenditureController : ApiController
    {
        string statuscode, statusmessage, ExpenseID;
#if !DEBUG
        [BearerAuthentication]
#endif       
        public HttpResponseMessage Post(ExpenditureDetails_Post items)
        {
            var response = new Generic_Responce();
            try
            {                
                items.AutoID = AESCryptography.DecryptAES(items.AutoID);
                items.expDate = AESCryptography.DecryptAES(items.expDate);
                items.expCode = AESCryptography.DecryptAES(items.expCode);
                items.amtType = AESCryptography.DecryptAES(items.amtType);
                items.amount = AESCryptography.DecryptAES(items.amount);
                items.paymentDate = AESCryptography.DecryptAES(items.paymentDate);
                items.voucherBillNumber = AESCryptography.DecryptAES(items.voucherBillNumber);
                items.payMode = AESCryptography.DecryptAES(items.payMode);
                items.payeeName = AESCryptography.DecryptAES(items.payeeName);
                items.payeeAddress = AESCryptography.DecryptAES(items.payeeAddress);
                items.sourceMoney = AESCryptography.DecryptAES(items.sourceMoney);
                items.remarks = AESCryptography.DecryptAES(items.remarks);
                // items.evidenceFile = AESCryptography.DecryptAES(items.evidenceFile);

                var file = HttpContext.Current.Request.Files.Count > 0 ?HttpContext.Current.Request.Files[0] : null;

                DBAccess objDBAccess = new DBAccess();
                SqlCommand cmd = new SqlCommand();
                DataTable dt = new DataTable();


                cmd.Parameters.Clear();
               
                cmd.Parameters.AddWithValue("@AutoID", items.AutoID);
                cmd.Parameters.AddWithValue("@expDate", items.expDate);
                cmd.Parameters.AddWithValue("@expCode", items.expCode);
                cmd.Parameters.AddWithValue("@Model", items.amtType);
                cmd.Parameters.AddWithValue("@amount", items.amount);
                cmd.Parameters.AddWithValue("@paymentDate", items.paymentDate);
                cmd.Parameters.AddWithValue("@voucherBillNumber", items.voucherBillNumber);
                cmd.Parameters.AddWithValue("@payMode", items.payMode);
                cmd.Parameters.AddWithValue("@payeeName", items.payeeName);
                cmd.Parameters.AddWithValue("@payeeAddress", items.payeeAddress);
                cmd.Parameters.AddWithValue("@sourceMoney", items.sourceMoney);
                cmd.Parameters.AddWithValue("@remarks", items.remarks);
                if (file != null && file.ContentLength > 0)
                {
                    cmd.Parameters.AddWithValue("@evidenceFile", file.InputStream);
                }
                dt = objDBAccess.getDBData(cmd, "sec.Mobile_saveData");
                if (dt.Rows.Count > 0)
                {
                    statuscode = dt.Rows[0]["statuscode"].ToString();
                    statusmessage = dt.Rows[0]["Msg"].ToString();
                    ExpenseID = dt.Rows[0]["ExpenseID"].ToString();
                }

                response.status_code = int.Parse(statuscode);
                response.Message = statusmessage;
                response.ExpenseID = ExpenseID;
                response.developer_message = response.Message;

                return Request.CreateResponse((HttpStatusCode)response.status_code, response);
            }
            catch (Exception ex)
            {
                if (string.IsNullOrEmpty(response.developer_message))
                {
                    response.developer_message = ex.Message;
                }
                return Request.CreateResponse((HttpStatusCode)response.status_code, response);
            }
        }



      
    }
}

