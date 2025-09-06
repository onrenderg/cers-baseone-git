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
    public class ViewRemarksController : ApiController
    {

#if !DEBUG
        [BearerAuthentication]
#endif

        public HttpResponseMessage Get(string ExpenseId)
        {
            var response = new Generic_Responce();
            try
            {
                ExpenseId = AESCryptography.DecryptAES(ExpenseId);
                DBAccess objDBAccess = new DBAccess();
                SqlCommand cmd = new SqlCommand();
                DataTable dt = new DataTable();
                cmd.Parameters.AddWithValue("@ExpenseId", ExpenseId);
                dt = objDBAccess.getDBData(cmd, "sec.Mobile_getremarks");

                List<ViewAllRemarks_Get> List_ = new List<ViewAllRemarks_Get>();
                foreach (DataRow dr in dt.Rows)
                {
                    var item = new ViewAllRemarks_Get();


                    item.ExpenseId = AESCryptography.EncryptAES(dr["ExpenseId"].ToString());
                    item.UserRemarksId = AESCryptography.EncryptAES(dr["UserRemarksId"].ToString());
                    item.ObserverRemarksId = AESCryptography.EncryptAES(dr["ObserverRemarksId"].ToString());
                    item.ObserverRemarks = AESCryptography.EncryptAES(dr["ObserverRemarks"].ToString());
                    item.UserRemarks = AESCryptography.EncryptAES(dr["UserRemarks"].ToString());
                    item.UserRemarksDtTm = AESCryptography.EncryptAES(dr["UserRemarksDtTm"].ToString());
                    item.ObserverRemarksDtTm = AESCryptography.EncryptAES(dr["ObserverRemarksDtTm"].ToString());

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

