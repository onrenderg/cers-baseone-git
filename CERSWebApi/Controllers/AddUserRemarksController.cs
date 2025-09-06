using CERSWebApi.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CERSWebApi.Controllers
{
    public class AddUserRemarksController : ApiController
    {
        string statuscode, statusmessage;
#if !DEBUG
        [BearerAuthentication]
#endif
        //save deatils
        public HttpResponseMessage Post(UserRemarks_POST items)
        {
            var response = new Generic_Responce();
            try
            {
                items.ExpenseId = AESCryptography.DecryptAES(items.ExpenseId);              
                items.UserRemarks = AESCryptography.DecryptAES(items.UserRemarks);             
                items.ObserverRemarksId = AESCryptography.DecryptAES(items.ObserverRemarksId);             
              
                DBAccess objDBAccess = new DBAccess();
                SqlCommand cmd = new SqlCommand();
                DataTable dt = new DataTable();
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@ExpenseId", items.ExpenseId);                
                cmd.Parameters.AddWithValue("@UserRemarks", items.UserRemarks);              
                cmd.Parameters.AddWithValue("@ObserverRemarksId", items.ObserverRemarksId);              
               
                dt = objDBAccess.getDBData(cmd, "sec.Mobile_saveuserremarks");
                if (dt.Rows.Count > 0)
                {
                    statuscode = dt.Rows[0]["statuscode"].ToString();
                    statusmessage = dt.Rows[0]["Msg"].ToString();                  
                }              

                response.status_code = int.Parse(statuscode);
                response.Message = statusmessage;
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

