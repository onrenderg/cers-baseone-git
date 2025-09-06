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
    public class ObserverCandidatesController : ApiController
    {
  
#if !DEBUG
        [BearerAuthentication]
#endif       
      
        public HttpResponseMessage Get(string PanchWardCode)
        {
            var response = new Generic_Responce();
            try
            {
                PanchWardCode = AESCryptography.DecryptAES(PanchWardCode);
                DBAccess objDBAccess = new DBAccess();
                SqlCommand cmd = new SqlCommand();
                DataTable dt = new DataTable();
                cmd.Parameters.AddWithValue("@PanchWardCode", PanchWardCode);
                dt = objDBAccess.getDBData(cmd, "sec.Mobile_getobserver_candidates");

                List<ObserverCandidates> List_ = new List<ObserverCandidates>();
                foreach (DataRow dr in dt.Rows)
                {
                    var item = new ObserverCandidates();

                   
                    item.VOTER_NAME = AESCryptography.EncryptAES(dr["VOTER_NAME"].ToString());
                    item.AUTO_ID = AESCryptography.EncryptAES(dr["AUTO_ID"].ToString());
                    item.Amount = AESCryptography.EncryptAES(dr["Amount"].ToString());                                      

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

