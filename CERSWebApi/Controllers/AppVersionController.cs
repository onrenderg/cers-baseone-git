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
    public class AppVersionController : ApiController
    {
#if !DEBUG
        [BearerAuthentication]
#endif

        //getdetails
        public HttpResponseMessage Get(string Platform, string packageid)
        {
            var response = new Generic_Responce();
            try
            {
                Platform = AESCryptography.DecryptAES(Platform);
                packageid = AESCryptography.DecryptAES(packageid);

                DBAccess objDBAccess = new DBAccess();
                SqlCommand cmd = new SqlCommand();
                DataTable dt = new DataTable();            

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Platform", Platform);
                cmd.Parameters.AddWithValue("@packageid", packageid);          
             
                dt = objDBAccess.getDBData(cmd, "[sec].[Mobile_AppVersion_get]");

                List<AppVersionDetails_Get> appVersionDetails = new List<AppVersionDetails_Get>();
                foreach (DataRow dr in dt.Rows)
                {
                    var item = new AppVersionDetails_Get();
                    
                    item.PackageName = AESCryptography.EncryptAES(dr["PackageName"].ToString());
                    item.Platform = AESCryptography.EncryptAES(dr["Platform"].ToString());
                    item.VersionNumber = AESCryptography.EncryptAES(dr["VersionNumber"].ToString());
                    item.WhatsNew = AESCryptography.EncryptAES(dr["WhatsNew"].ToString());
                    item.StoreLink = AESCryptography.EncryptAES(dr["StoreLink"].ToString());
                    item.Mandatory = AESCryptography.EncryptAES(dr["Mandatory"].ToString());
                    item.UpdatedOn = AESCryptography.EncryptAES(dr["UpdatedOn"].ToString());
                    appVersionDetails.Add(item);
             
    }
                response.status_code = 200;
                response.Message = "Successfully Fetched";
                response.developer_message ="Ok";
                response.data = appVersionDetails;
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

