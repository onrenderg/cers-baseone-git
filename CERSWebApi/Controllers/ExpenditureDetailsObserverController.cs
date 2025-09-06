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
    public class ExpenditureDetailsObserverController : ApiController
    {
  
#if !DEBUG
        [BearerAuthentication]
#endif       
      
        public HttpResponseMessage Get(string AutoID)
        {
            var response = new Generic_Responce();
            try
            {
                AutoID = AESCryptography.DecryptAES(AutoID);
                DBAccess objDBAccess = new DBAccess();
                SqlCommand cmd = new SqlCommand();
                DataTable dt = new DataTable();
                cmd.Parameters.AddWithValue("@AutoID", AutoID);
                dt = objDBAccess.getDBData(cmd, "sec.Mobile_getsaveData_observer");

                List<ExpenditureDetails_Get> List_ = new List<ExpenditureDetails_Get>();
                foreach (DataRow dr in dt.Rows)
                {
                    var item = new ExpenditureDetails_Get();

                   
                    item.ExpenseID = AESCryptography.EncryptAES(dr["ExpenseID"].ToString());
                    item.AutoID = AESCryptography.EncryptAES(dr["AutoID"].ToString());
                    item.expDate = AESCryptography.EncryptAES(dr["expDate"].ToString());
                    item.expCode = AESCryptography.EncryptAES(dr["expCode"].ToString());
                    item.amtType = AESCryptography.EncryptAES(dr["amtType"].ToString());
                    item.amount = AESCryptography.EncryptAES(dr["amount"].ToString());
                    item.paymentDate = AESCryptography.EncryptAES(dr["paymentDate"].ToString());
                    item.voucherBillNumber = AESCryptography.EncryptAES(dr["voucherBillNumber"].ToString());
                    item.payMode = AESCryptography.EncryptAES(dr["payMode"].ToString());
                    item.payeeName = AESCryptography.EncryptAES(dr["payeeName"].ToString());                   
                    item.payeeAddress = AESCryptography.EncryptAES(dr["payeeAddress"].ToString());
                    item.sourceMoney = AESCryptography.EncryptAES(dr["sourceMoney"].ToString());                   
                    item.remarks = AESCryptography.EncryptAES(dr["remarks"].ToString());
                    item.DtTm = AESCryptography.EncryptAES(dr["DtTm"].ToString());
                    item.ExpStatus = AESCryptography.EncryptAES(dr["ExpStatus"].ToString());                     
                    item.ExpTypeName = AESCryptography.EncryptAES(dr["ExpTypeName"].ToString());                   
                    item.ExpTypeNameLocal = AESCryptography.EncryptAES(dr["ExpTypeNameLocal"].ToString());
                    item.PayModeName = AESCryptography.EncryptAES(dr["PayModeName"].ToString());
                    item.PayModeNameLocal = AESCryptography.EncryptAES(dr["PayModeNameLocal"].ToString());                   
                    item.evidenceFile = AESCryptography.EncryptAES(dr["evidenceFile"].ToString());                   
                    item.expDateDisplay = AESCryptography.EncryptAES(dr["expDateDisplay"].ToString());                   
                    item.paymentDateDisplay = AESCryptography.EncryptAES(dr["paymentDateDisplay"].ToString());                   
                    item.amountoutstanding = AESCryptography.EncryptAES(dr["amountoutstanding"].ToString());                   
                    item.CONSTITUENCY_CODE = AESCryptography.EncryptAES(dr["CONSTITUENCY_CODE"].ToString());                   
                    item.VOTER_NAME = AESCryptography.EncryptAES(dr["VOTER_NAME"].ToString());                   
                    item.AgentName = AESCryptography.EncryptAES(dr["AgentName"].ToString());                   
                    item.ObserverRemarks = AESCryptography.EncryptAES(dr["ObserverRemarks"].ToString());                   

                    List_.Add(item);
                }
                if (List_.Any())
                {
                    response.status_code = 200;
                    response.Message = "Success";
                }
                else
                {
                    response.status_code = 404;
                    response.Message = "No Record Found";
                }
                response.developer_message = response.Message;
                response.data = List_;
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

