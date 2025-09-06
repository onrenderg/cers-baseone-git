using CERSWebApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace CERSWebApi.Controllers
{
    public class GetOTPController : ApiController
    {
        string statuscode = "", statusmessage = "";
        int status_code;
#if !DEBUG
        [BearerAuthentication]
#endif

        public HttpResponseMessage Get(string MobileNo)
        {
            var response = new Generic_Responce();
            List<UserDetails_Get_otpid> List_ = new List<UserDetails_Get_otpid>();
            var item = new UserDetails_Get_otpid();

            try
            {
                MobileNo = AESCryptography.DecryptAES(MobileNo);
                string OTPassword;
                Random r = new Random();
                int num = r.Next(Convert.ToInt32(100000), Convert.ToInt32(999999));
                int valueofotp = num;
                int num1 = r.Next(Convert.ToInt32(1000), Convert.ToInt32(9999));
                int valueofotpid = num1;

                /*    if (MobileNo == "8219211012" || MobileNo == "9418948889" || MobileNo == "9816867728")
                    {
                        OTPassword = "123456";
                    }
                    else
                    {
                        OTPassword = (Convert.ToString(valueofotp));
                    }*/

                OTPassword = (Convert.ToString(valueofotp));
                //fixed otp password for all users
                // OTPassword = "123456";//comment before hosting

                string OTPID = (Convert.ToString(valueofotpid));
                string variable2 = "CERS (Expenditure Reporting App) ";
                string message = "Namaskar! " + OTPassword + " is your OTP for " + variable2
                    + "of State Election Commission, Himachal Pradesh.  HPGOVT";


                DBAccess objDBAccess = new DBAccess();
                SqlCommand cmd = new SqlCommand();
                DataTable dt = new DataTable();
                //Define Output
                SqlParameter status_code_ = new SqlParameter("@status_code", SqlDbType.Int);
                SqlParameter status_message_ = new SqlParameter("@status_message", SqlDbType.VarChar, 200);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@MobileNo", MobileNo);
                cmd.Parameters.AddWithValue("@Otppassword", OTPassword);
                cmd.Parameters.AddWithValue("@otpId", OTPID);
                //attach Output
                status_code_.Direction = ParameterDirection.Output;
                status_message_.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(status_message_);
                cmd.Parameters.Add(status_code_);

                dt = objDBAccess.getDBData(cmd, "[sec].[Mobile_CERS_SaveOtp_New]");
                if (dt.TableName == "OK")
                {
                    response.status_code = (int)(status_code_.Value);
                    response.Message = (string)(status_message_.Value); ;
                }
                if (response.status_code == 200)
                {
                    item.OTPID = AESCryptography.EncryptAES(OTPID.ToString());
                    List_.Add(item);

                    string smsresponse = string.Empty;
                   /* if (MobileNo == "8219211012" || MobileNo == "9418948889" || MobileNo == "9816867728")
                    {
                        smsresponse = "Test User";
                    }
                    else  //uncomment before hosting  
                    {*/
                        SendOtpSms sendOtpSms = new SendOtpSms();
                        smsresponse = sendOtpSms.sendSingleSMS(MobileNo, message);

                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@MobileNo", MobileNo);
                        cmd.Parameters.AddWithValue("@otpmessage", message);
                        cmd.Parameters.AddWithValue("@smsresponse", smsresponse);
                        cmd.Parameters.AddWithValue("@otpId", OTPID);
                        dt = objDBAccess.getDBData(cmd, "[sec].[Mobile_CERS_updateOTPresponse]");



                    /* }*/
                }
                response.data = List_;
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
       /* int checknooftimesotpsentin2minutes(string MobileNo)
        {

            int nooftime = 0;
            DBAccess objDBAccess = new DBAccess();
            SqlCommand cmd = new SqlCommand();
            DataTable dt = new DataTable();
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@MobileNo", MobileNo);
            dt = objDBAccess.getDBData(cmd, "[sec].[Mobile_CERS_CheckOtpAttempts]");
            if (dt.Rows.Count > 0)
            {
                statusmessage = dt.Rows[0]["msg"].ToString().Trim();
                statuscode = dt.Rows[0]["statuscode"].ToString();
                nooftime = int.Parse(dt.Rows[0]["nooftimes"].ToString());
                status_code = int.Parse(dt.Rows[0]["statuscode"].ToString());
            }
            return nooftime;

        }*/


    }

}

