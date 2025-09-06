
using System.Web;
using System.Web.Http.Filters;

using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web.Http.Controllers;
using System.Data;
using System.Data.SqlClient;


namespace CERSWebApi.Models
{
    public class BearerAuthenticationAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var authHeader = actionContext.Request.Headers.Authorization;

            if (authHeader != null)
            {
                var authenticationScheme = actionContext.Request.Headers.Authorization.Scheme;
                var authenticationToken = (HttpUtility.UrlDecode(actionContext.Request.Headers.Authorization.Parameter));

                if (isValid(authenticationToken) && authenticationScheme == "Bearer")
                {
                    return;
                }

            }
            HandleUnathorized(actionContext);
        }

        private static void HandleUnathorized(HttpActionContext actionContext)
        {
            var response = new Generic_Responce();
            response.status_code = 401;
            response.Message = "Unauthorized Request";
            response.developer_message = "Unauthorized Request";
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, response);
            actionContext.Response.Headers.Add("WWW-Authenticate", "Basic Scheme='Data' location = 'http://localhost:");
        }

        private bool isValid(string token)
        {
            DBAccess objDBAccess = new DBAccess();
            SqlCommand cmd = new SqlCommand();
            DataTable dt = new DataTable();
            SqlParameter status_code_ = new SqlParameter("@status_code", SqlDbType.Int);
            SqlParameter status_message_ = new SqlParameter("@status_message", SqlDbType.VarChar, 200);

            cmd.Parameters.Clear();
            cmd.Parameters.Add("@token_id", SqlDbType.VarChar).Value = token;
            status_code_.Direction = ParameterDirection.Output;
            status_message_.Direction = ParameterDirection.Output;


            dt = objDBAccess.getDBData(cmd, "[sec].[mobile_bearer_token_verify]");
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["status_code"].ToString() == "200")
                {
                    var principal = new GenericPrincipal(new GenericIdentity(token), null);
                    Thread.CurrentPrincipal = principal;
                    return true;
                }
            }
            return false;
        }
    }
}