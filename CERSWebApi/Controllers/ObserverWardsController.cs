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
    public class ObserverWardsController : ApiController
    {
  
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
                cmd.Parameters.AddWithValue("@MobileNo", MobileNo);
                dt = objDBAccess.getDBData(cmd, "sec.Mobile_getobserver_wards");

                List<ObserverWards> List_ = new List<ObserverWards>();
                foreach (DataRow dr in dt.Rows)
                {
                    var item = new ObserverWards();

                   
                    item.Panchayat_Code = AESCryptography.EncryptAES(dr["Panchayat_Code"].ToString());
                    item.Panchayat_Name = AESCryptography.EncryptAES(dr["Panchayat_Name"].ToString());
                    item.Panchayat_Name_Local = AESCryptography.EncryptAES(dr["Panchayat_Name_Local"].ToString());                                

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

