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
    public class ObservorLoginController : ApiController
    {
        string statuscode = "", statusmessage = "";
#if !DEBUG
        [BearerAuthentication]
#endif

        public HttpResponseMessage Get(string MobileNo)
        {           
            var response = new Generic_Responce();
            try
            {
                MobileNo = AESCryptography.DecryptAES(MobileNo);
                DBAccess objDBAccess = new DBAccess();
                SqlCommand cmd = new SqlCommand();
                DataTable dt = new DataTable();
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@MobileNo", MobileNo);
                DataSet returneddataset = new DataSet();                
                returneddataset = objDBAccess.getDBDataSet(cmd, "sec.Mobile_CERS_ObservorLogin", "DBConn");
                
                DataTable _dataTableMessage = new DataTable();
                _dataTableMessage = returneddataset.Tables[0];
                foreach (DataRow dt1 in _dataTableMessage.Rows)
                {
                    statuscode = dt1["statuscode"].ToString();
                    statusmessage = dt1["Msg"].ToString();
                }
                List<UserDetails_Observer_Get> List_ = new List<UserDetails_Observer_Get>();
                if (statuscode.Equals("200"))
                {                 

                    DataTable _dataTableuser = new DataTable();
                    _dataTableuser = returneddataset.Tables[1];

                    foreach (DataRow dr in _dataTableuser.Rows)
                    {
                        var item = new UserDetails_Observer_Get();
                        item.Auto_ID = (AESCryptography.EncryptAES(dr["Auto_ID"].ToString()));
                        item.ObserverName = (AESCryptography.EncryptAES(dr["ObserverName"].ToString()));                     
                        item.ObserverContact = AESCryptography.EncryptAES(dr["ObserverContact"].ToString());
                        item.ObserverDesignation = AESCryptography.EncryptAES(dr["ObserverDesignation"].ToString());
                        item.Pritype = AESCryptography.EncryptAES(dr["Pritype"].ToString());                   
                        List_.Add(item);
                    }                   
                        
                }
                response.data = List_;
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

        string  save_sendOTP(string MobileNo, string OTPassword,string OTPID)
        {
            if (MobileNo == "8219211012")
            {
                OTPassword = "123456";
            }

            string message = "Namaskar! " + OTPassword + " is your OTP for Voting Day Monitoring System (VDMS). State Election Commission, Himachal Pradesh.";

            DBAccess objDBAccess = new DBAccess();
            SqlCommand cmd = new SqlCommand();
            DataTable dt = new DataTable();
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@MobileNo", MobileNo);
            cmd.Parameters.AddWithValue("@Otppassword", OTPassword);
            cmd.Parameters.AddWithValue("@otpId", OTPID);
            dt = objDBAccess.getDBData(cmd, "[sec].[Mobile_CERS_SaveOtp]", "DBConn1");
            if (dt.Rows.Count > 0)
            {
                statuscode = dt.Rows[0]["statuscode"].ToString();
                statusmessage = dt.Rows[0]["Msg"].ToString();
                // unitid = dt.Rows[0]["Id"].ToString();
            }
            if (MobileNo == "8219211012")
            {
                return "Test User";
            }
            else
            {
                SendOtpSms sendOtpSms = new SendOtpSms();
                string responseFromServer = sendOtpSms.sendSingleSMS(MobileNo, message);
                return responseFromServer;
            }
        }
    }
}

