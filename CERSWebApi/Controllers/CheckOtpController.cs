using CERSWebApi.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CERSWebApi.Controllers
{
    public class CheckOtpController : ApiController
    {
        
#if !DEBUG
        [BearerAuthentication]
#endif

        public HttpResponseMessage Get(string MobileNo, string UserOtp, string otpId)
        {
            var response = new Generic_Responce();
            try
            {
                MobileNo = AESCryptography.DecryptAES(MobileNo);
                UserOtp = AESCryptography.DecryptAES(UserOtp);
                otpId = AESCryptography.DecryptAES(otpId);

                DBAccess objDBAccess = new DBAccess();
                SqlCommand cmd = new SqlCommand();
                DataTable dt = new DataTable();
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@MobileNo", MobileNo);
                cmd.Parameters.AddWithValue("@userotp", UserOtp);
                cmd.Parameters.AddWithValue("@otpId", otpId);
                            

              //  dt = objDBAccess.getDBData(cmd, "sec.Mobile_CERS_CheckOtp", "DBConn1");
                dt = objDBAccess.getDBData(cmd, "sec.Mobile_CERS_CheckOtp");
                string statuscode = "", statusmessage = "";
                foreach (DataRow dt1 in dt.Rows)
                {
                    statuscode = dt1["statuscode"].ToString();
                    statusmessage = dt1["Msg"].ToString();
                }

               
                response.status_code =int.Parse(statuscode);
                response.Message = statusmessage;
                response.developer_message = response.Message;
                response.data = null;
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

