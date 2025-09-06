using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

using System.Net.Security;
using System.Web;
using System.Net.Mail;

using System.Security.Policy;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Configuration;
using System.Security.Cryptography;

namespace CERSWebApi
{
    public class DITSMS
    {
        public string sendSMS(String mobileNos, String message)
        {
            // ''' mobileNos = "9816618921"
            string result = string.Empty;
            result = "F";
            Stream dataStream;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://msdgweb.mgov.gov.in/esms/sendsmsrequest");
            request.ProtocolVersion = HttpVersion.Version10;

            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 5.0; Windows 98; DigExt)";
            request.Method = "POST";
            try
            {
                string smsservicetype = "otpmsg";
                string encryptedPassword = encryptedPasswod(ConfigurationManager.AppSettings["senderUserPasswordDit"]);
                
                string query = 
                    "username=" + HttpUtility.UrlEncode(ConfigurationManager.AppSettings["senderUserNameDit"]) 
                    + "&password=" + HttpUtility.UrlEncode(encryptedPassword)
                    + "&smsservicetype=" + HttpUtility.UrlEncode(smsservicetype) 
                    + "&content=" + HttpUtility.UrlEncode(message)
                    + "&mobileno=" + HttpUtility.UrlEncode(mobileNos) 
                    + "&senderid=" + HttpUtility.UrlEncode(ConfigurationManager.AppSettings["senderIDDit"])
                    + "&key=" + HttpUtility.UrlEncode(ConfigurationManager.AppSettings["secureKeyDit"].Trim())
                    + "&templateid=" + HttpUtility.UrlEncode(ConfigurationManager.AppSettings["template_id"].Trim()); 

             
                


                byte[] byteArray = Encoding.ASCII.GetBytes(query);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;
                dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);

                dataStream.Close();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                //WebResponse response = request.GetResponse();
                String Status = response.StatusDescription;
                dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
            }
            catch (Exception ex)
            {
                string ExceptionMsg = "";
                string Msg = "Ex:" + ex.Message.ToString() + "- Msg :" + message;
                if (Msg.Length > 1000)
                {
                    ExceptionMsg = Msg.Substring(0, 999);
                    result = ExceptionMsg;
                }
                else
                {
                    ExceptionMsg = Msg;
                    result = Msg;
                }
            }
            return result;
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