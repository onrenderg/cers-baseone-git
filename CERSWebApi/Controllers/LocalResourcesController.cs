using CERSWebApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CERSWebApi.Controllers
{
    public class LocalResourcesController : ApiController
    {
        
#if !DEBUG
        [BearerAuthentication]
#endif

        public HttpResponseMessage Get()
        {
            var response = new Generic_Responce();
            try
            {               
                DBAccess objDBAccess = new DBAccess();
                SqlCommand cmd = new SqlCommand();
                DataTable dt = new DataTable();
                cmd.Parameters.Clear();              

                dt = objDBAccess.getDBData(cmd, "sec.Mobile_getLocalResources");

                List<LocalResourcesDetails_Get> List_ = new List<LocalResourcesDetails_Get>();
                foreach (DataRow dr in dt.Rows)
                {
                    var item = new LocalResourcesDetails_Get();
                    item.MultipleResourceKey = AESCryptography.EncryptAES(dr["MultipleResourceKey"].ToString());
                    item.ResourceKey = AESCryptography.EncryptAES(dr["ResourceKey"].ToString());
                    item.ResourceValue = AESCryptography.EncryptAES(dr["ResourceValue"].ToString());
                    item.LocalResourceValue = AESCryptography.EncryptAES(dr["LocalResourceValue"].ToString());                   
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

