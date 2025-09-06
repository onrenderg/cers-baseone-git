using CERSWebApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace EvmTracingWebApi.Controllers
{
    public class PaymentModeController : ApiController
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
                           

                dt = objDBAccess.getDBData(cmd, "sec.Mobile_getPaymentModes");

                List<PaymentModes_Get> List_ = new List<PaymentModes_Get>();
                foreach (DataRow dr in dt.Rows)
                {
                    var item = new PaymentModes_Get();

                    item.paymode_code = AESCryptography.EncryptAES(dr["paymode_code"].ToString());
                    item.paymode_Desc = AESCryptography.EncryptAES(dr["paymode_Desc"].ToString());
                    item.paymode_Desc_Local = AESCryptography.EncryptAES(dr["paymode_Desc_Local"].ToString());
                                      

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

