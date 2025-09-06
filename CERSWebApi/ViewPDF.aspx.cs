using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;


namespace CERSWebApi
{
    public partial class ViewPDF : Page
    {
        private SqlConnection connectionString = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConn"].ToString());

        protected void Page_Load(object sender, EventArgs e)
        {
            
            string ExpenseID;
            ExpenseID = Request.QueryString["ExpenseID"];

            SqlCommand cmdAddPDF = new SqlCommand("sec.Mobile_getpdf", connectionString);
            cmdAddPDF.CommandType = CommandType.StoredProcedure;

            cmdAddPDF.Parameters.AddWithValue("@ExpenseID", Convert.ToString(Request.QueryString["ExpenseID"]).Trim());
            

            SqlDataAdapter objda = new SqlDataAdapter();
            objda.SelectCommand = cmdAddPDF;
            DataTable objdt = new DataTable();
            objda.Fill(objdt);
            if (objdt.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(objdt.Rows[0]["evidenceFile"].ToString()))
                {
                    byte[] bytes = (byte[])objdt.Rows[0]["evidenceFile"];
                    Response.BufferOutput = true;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.AddHeader("Content-Length", bytes.Length.ToString());
                    Response.AddHeader("content-disposition", "inline;filename=" + Request.QueryString["ExpenseID"]);
                    Response.ContentType = "application/pdf";
                    Response.BinaryWrite(bytes);
                    Response.Flush();
                    Response.Clear();
                }
                else
                {
                    Response.Write("<script language='javascript'>alert('Document Does not Exist.');</script>");
                }

            }
            else
            {
                Response.Write("<script language='javascript'>alert('Document Does not Exist.');</script>");
            }

        }

    }
}