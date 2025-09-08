using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace CERSWebApi.Models
{
    public class SendOtpSms
    {
        public string sendSingleSMS(string mobileNo, string message )
        {
            string Status = "";
            try
            {
                string username = "";
                string password = "";
                string senderid = "";
                string isLocalorLive = "";
                string tmpid= ConfigurationManager.AppSettings["template_id"];  
                string secureKey = ConfigurationManager.AppSettings["secureKeyDit"];
                username = ConfigurationManager.AppSettings["senderUserNameDit"];
                password = ConfigurationManager.AppSettings["senderUserPasswordDit"];
                senderid = ConfigurationManager.AppSettings["senderIDDit"];
                isLocalorLive = ConfigurationManager.AppSettings["isLocalorLive"];
                if (isLocalorLive.Contains("Local"))
                {
                    return "SMS bypassed for local development";
                }


                Stream dataStream;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://msdgweb.mgov.gov.in/esms/sendsmsrequestDLT");
                request.ProtocolVersion = HttpVersion.Version10;
                request.KeepAlive = false;
                request.ServicePoint.ConnectionLimit = 1;
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 5.0; Windows 98; DigExt)";
                request.Method = "POST";
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(() => true);
                //System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3 || System.Security.Authentication.SslProtocols.Tls || System.Security.Authentication.SslProtocols.Default || System.Security.Authentication.SslProtocols.Tls11 || System.Security.Authentication.SslProtocols.Tls12;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                string encryptedPassword = encryptedPasswod(password);
                string NewsecureKey = hashGenerator(username.Trim(), senderid.Trim(), message.Trim(), secureKey.Trim());
               // string smsservicetype = "singlemsg";
                string smsservicetype = "otpmsg";
                string query = "username=" + HttpUtility.UrlEncode(username.Trim()) + "&password=" + HttpUtility.UrlEncode(encryptedPassword) + "&smsservicetype=" + HttpUtility.UrlEncode(smsservicetype) + "&content=" + HttpUtility.UrlEncode(message.Trim()) + "&mobileno=" + HttpUtility.UrlEncode(mobileNo) + "&senderid=" + HttpUtility.UrlEncode(senderid.Trim()) + "&key=" + HttpUtility.UrlEncode(NewsecureKey.Trim()) + "&templateid=" + HttpUtility.UrlEncode(tmpid.Trim());
                byte[] byteArray = Encoding.ASCII.GetBytes(query);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;
                dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                Status = ((HttpWebResponse)response).StatusDescription;
                dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
                response.Close();
                //WriteErrorLogToFile("Function Name sendSMS:------template_id : " + tmpid + " send_SMS ------Mobile -- " + mobileNo + "+responseFromServer" + responseFromServer);
                return responseFromServer;

            }
            catch (Exception ex)
            {
                Status = "Msg Not Sent";
               // WriteErrorLogToFile("Function Name sendSMS:------template_id : " + tmpid + " send_SMS ------Mobile -- " + mobileNo + "+Message" + ex.Message.ToString() + "-----------------stacktrace-----------------" + ex.StackTrace.ToString());
            }
            return Status;
        }



        protected string encryptedPasswod(string password)
        {
            byte[] encPwd = Encoding.UTF8.GetBytes(password);
            HashAlgorithm sha1 = HashAlgorithm.Create("SHA1");
            byte[] pp = sha1.ComputeHash(encPwd);
            System.Text.Encoding.UTF8.GetString(pp);
            StringBuilder sb = new StringBuilder();

            foreach (byte b in pp)
                sb.Append(b.ToString("x2"));

            return sb.ToString();
        }

        protected string hashGenerator(string Username, string sender_id, string message, string secure_key)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Username).Append(sender_id).Append(message).Append(secure_key);
            byte[] genkey = Encoding.UTF8.GetBytes(sb.ToString());
            HashAlgorithm sha1 = HashAlgorithm.Create("SHA512");
            byte[] sec_key = sha1.ComputeHash(genkey);
            StringBuilder sb1 = new StringBuilder();

            for (int i = 0; i <= sec_key.Length - 1; i++)
                sb1.Append(sec_key[i].ToString("x2"));

            return sb1.ToString();
        }
    }
}