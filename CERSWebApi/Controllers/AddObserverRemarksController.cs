using CERSWebApi.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CERSWebApi.Controllers
{
    public class AddObserverRemarksController : ApiController
    {
        string statuscode, statusmessage;
#if !DEBUG
        [BearerAuthentication]
#endif


        //save deatils
        public HttpResponseMessage Post(ObserverRemarks_POST items)
        {
            var response = new Generic_Responce();
            try
            {
                items.ExpenseId = AESCryptography.DecryptAES(items.ExpenseId);
                items.ObserverId = AESCryptography.DecryptAES(items.ObserverId);
                items.ObserverRemarks = AESCryptography.DecryptAES(items.ObserverRemarks);             
              
                DBAccess objDBAccess = new DBAccess();
                SqlCommand cmd = new SqlCommand();
                DataTable dt = new DataTable();
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@ExpenseId", items.ExpenseId);
                cmd.Parameters.AddWithValue("@ObserverId", items.ObserverId);
                cmd.Parameters.AddWithValue("@ObserverRemarks", items.ObserverRemarks);
              
               
                dt = objDBAccess.getDBData(cmd, "sec.Mobile_saveobserverremarks");
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

