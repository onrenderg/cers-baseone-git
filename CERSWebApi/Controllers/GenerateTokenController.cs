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
    public class GenerateTokenController : ApiController
    {
   
#if !DEBUG
       [BasicAuthentication]
#endif  
        public HttpResponseMessage Get()
        {          
            var response = new Generic_Responce();
            try
            {          

                DBAccess objDBAccess = new DBAccess();
                SqlCommand cmd = new SqlCommand();
                DataTable dt = new DataTable();
                SqlParameter status_code_ = new SqlParameter("@status_code", SqlDbType.Int);
                SqlParameter status_message_ = new SqlParameter("@status_message", SqlDbType.VarChar, 200);

                cmd.Parameters.Clear();           
                status_code_.Direction = ParameterDirection.Output;
                status_message_.Direction = ParameterDirection.Output;
                dt = objDBAccess.getDBData(cmd, "[sec].[mobile_bearer_token_get]", "DBConn");
                if (dt.Rows.Count > 0)
                {
                    response.status_code = int.Parse( dt.Rows[0]["status_code"].ToString());
                    response.Message = dt.Rows[0]["status_message"].ToString();
                    response.TokenID =  dt.Rows[0]["token_id"].ToString();
                }
              
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

    }
}

