using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CERSWebApi
{
    public partial class GetDeclarationPdf : Page
    {
        private SqlConnection connectionString = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConn"].ToString());


        protected void Page_Load(object sender, EventArgs e)
        {
            string MobileNo;
            MobileNo = Request.QueryString["MobileNo"];

            SqlCommand cmdAddPDF = new SqlCommand("sec.Mobile_getpdfdecdata", connectionString);
            cmdAddPDF.CommandType = CommandType.StoredProcedure;

            cmdAddPDF.Parameters.AddWithValue("@MobileNo", Convert.ToString(Request.QueryString["MobileNo"]).Trim());


            SqlDataAdapter objda = new SqlDataAdapter();
            objda.SelectCommand = cmdAddPDF;
            DataTable mytable = new DataTable();
            objda.Fill(mytable);
            StringBuilder str = new StringBuilder();

            str.Append("<html><body>");
                str.Append("<table  style='width:100%; border-collapse: collapse;' >");
            str.Append("<tr><td style ='width:100%;text-align:center;font-size:20px;'>Form of Declaration </br></br></td></tr>");   
            
            str.Append("<tr><td><ul style='list-style:none;'><li>Before the District Election Officer..........Constituency of " + mytable.Rows[0]["Panchayat_Name"] + ".</li></ul></td></tr>");
             
            str.Append("<tr>");
            str.Append("<td>");
            str.Append("<ul style='list-style:none;'><li>Declaration of Shri/Smt./Km " + mytable.Rows[0]["VOTER_NAME"] + " son/wife/daughter of Shri " + mytable.Rows[0]["RELATIVE_NAME"] + ".</br></br></li>");
            str.Append("<li>I, " + mytable.Rows[0]["VOTER_NAME"] +
                " son/wife/daughter of Shri " + mytable.Rows[0]["RELATIVE_NAME"] + " aged " + mytable.Rows[0]["AGE"] +
                " resident of " + mytable.Rows[0]["HOUSE_NO"] + " do hereby solemnly affirm and declare as under : -" + ".</br></br></li>");
            str.Append("<li>That I was a candidate at the general election/bye-election to the constituency of "
                + mytable.Rows[0]["Panchayat_Name"] + " , the result of which was declared on " + mytable.Rows[0]["ResultDate"] + ".</li>");
           str.Append("</ul></td>");
            str.Append("</tr>");

            str.Append("<tr>");
            str.Append("<td>");
            str.Append("<ul style='list-style:none;'>");
            str.Append("<li>1. That I was a candidate at the general election/bye-election to the constituency of " + mytable.Rows[0]["Panchayat_Name"] +
                " , the result of which was declared on " + mytable.Rows[0]["ResultDate"] + ".</br></br></li>");
            str.Append("<li>2. That I/my election agent kept a separate and correct account (running into .....pages) " +
                "of all expenditure in connection with the above election incurred or authorized by me or by my election agent " +
                "between " + mytable.Rows[0]["NominationDate"] + " (date of nomination) and " + mytable.Rows[0]["ResultDate"] +
                " (date of declaration of result thereof), both days inclusive." + ".</br></br></li>");
            str.Append("<li>3.That the said account was maintained in the form prescribed under rule 92 of the Himachal Pradesh Panchayati Raj (Election) Rules,1994 and a true copy thereof is annexed" +
                "hereto with the supporting vouchers / bills mentioned in the said account." +
                ".</br></br></li>");
            str.Append("<li>4.That the account of my election expenditure annexed hereto include all items of election expenditure incurred or authorized by me or by my election agent and nothing has been" +
                        "concealed or withheld/suppressed therefrom." +
                ".</br></br></li>");
            str.Append("<li>5. That the statements in the foregoing paragraphs 1 to 4 are true to my personal knowledge and that nothing is false and nothing material has been concealed." +
                ".</br></br></li>");
            str.Append("</ul></td>");
            str.Append("</tr>");


            str.Append("<tr>");
            str.Append("<td style='text-align:right;'>Deponent." + ".</td>");
            str.Append("</tr>");


            str.Append("<tr>");
            str.Append("<td style='text-align:right;'>Solemnly affirmed by " + mytable.Rows[0]["VOTER_NAME"] + ".</td>");
            str.Append("</tr>");

            str.Append("<tr>");
            str.Append("<td style='text-align:right;'> on " + mytable.Rows[0]["datemonth"] + " this day of " + mytable.Rows[0]["yr"] + " before me" + ".</td>");
            str.Append("</tr>");

            //str.Append("<table  style='width:100%; border-collapse: collapse;' >");
            //str.Append("<tr>");
            //str.Append("<td style='width:35%'>VOTER_NAME : </td>");
            //str.Append("<td style='width:35%'>" + mytable.Rows[0]["VOTER_NAME"] + "  </td>");

            //str.Append("<td style='width:15%'>RELATION_TYPE : </td>");
            //str.Append("<td style='width:15%'>" + mytable.Rows[0]["RELATION_TYPE"] + "  </td>");

           // str.Append("</tr>");
            str.Append("</table>");


            string strFileName;
            strFileName = "Print"; string strCurrentPageDir = HttpContext.Current.Request.Url.Authority + "/";
            byte[] fileByts = HtmlStringToPdf(str.ToString().Replace("View as PDF", ""), strCurrentPageDir, strFileName);
            if (fileByts != null)
            {
                HttpContext.Current.Response.ContentType = "Application/pdf";
                HttpContext.Current.Response.AppendHeader("Content-Disposition", "inline;filename=Print.pdf");
                HttpContext.Current.Response.BinaryWrite(fileByts);
                HttpContext.Current.Response.End();
            }
            //fileByts = HtmlToPdf(HtmlString, MobileNo);

            //HttpContext.Current.Response.ContentType = "application/pdf";
            //HttpContext.Current.Response.AddHeader("content-disposition", "inline;" + "filename=" + MobileNo + ".pdf");
            //HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //HttpContext.Current.Response.BinaryWrite(fileByts);
            //HttpContext.Current.Response.End();
        }
        /*  public StringBuilder newPDF(DataTable dt)
          {
              StringBuilder str = new StringBuilder();
              try
              {
                  DataRow dr = dt.Rows(0);
                  DataTable dtpatcat = new DataTable();

              }
              catch
              {

              }
          }*/
        public byte[] HtmlStringToPdf(string strHtml, string strCurrentPageDir, string strFileName)
        {
            //string strTempHtmlPath = HttpContext.Current.Server.MapPath("temp/" + strFileName + ".html");
            string strTempHtmlPath = HttpContext.Current.Server.MapPath("~/temp/" + strFileName + ".html");
            if (System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/temp")) == false)
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/temp"));
            if (System.IO.File.Exists(strTempHtmlPath) == true)
                System.IO.File.Delete(strTempHtmlPath);
            strHtml = @"<html><head><meta http-equiv=\""Content-Type\"" content=\""text/html; charset=utf-8\"" /></head><body>" + strHtml + "</body></html>";
            strHtml = System.Text.RegularExpressions.Regex.Unescape(strHtml);
            strHtml = strHtml.Replace("</form></body></html>", "");
            CreateTempHtmlFile(strHtml, strTempHtmlPath);
            //byte[] fileBytes = UrlToPdf(strTempHtmlPath);
            byte[] fileBytes = HtmlToPdfDirect(strTempHtmlPath);
            if (System.IO.File.Exists(strTempHtmlPath) == true)
                System.IO.File.Delete(strTempHtmlPath);
            return fileBytes;
        }

        public byte[] UrlToFileBytes(string url)
        {
            byte[] fileBytes = HtmlToPdfDirect(url);
            return fileBytes;
        }
        public byte[] HtmlToPdfDirect(string url)
        {
            var wkhtmlDir = HttpContext.Current.Server.MapPath(HttpContext.Current.Request.ApplicationPath.Replace("//", "/") + "/wkhtmltopdf/");
            var wkhtml = HttpContext.Current.Server.MapPath(HttpContext.Current.Request.ApplicationPath.Replace("//", "/") + "/wkhtmltopdf/wkhtmltopdf.exe");
            string strTempPdfPath = HttpContext.Current.Server.MapPath("temp/RPTPCRBulk.pdf");
            if (System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("temp")) == false)
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("temp"));
            if (System.IO.File.Exists(strTempPdfPath) == true)
                System.IO.File.Delete(strTempPdfPath);

            string switches = "";
            switches += "--print-media-type ";
            switches += "--margin-top 15mm --margin-bottom 20mm --margin-right 5mm --margin-left 5mm ";
            switches += "--page-size A4 ";
            // Dim strFont As String = "MangalRegular"
            string strFont = "Mangal";
            string strleftfooter = "NIC-(https://sechimachal.nic.in/)";
            switches += (Convert.ToString("--footer-left \"") + strleftfooter) + "\"  ";
            switches += (Convert.ToString("--footer-center \"") + "Page [page] of [topage] ") + "\"  ";
            switches += (Convert.ToString(" --header-font-size 8"));
            switches += (Convert.ToString(" --footer-font-size 8 "));
            // 'switches += (Convert.ToString("--footer-font-name """) & strFont) + """  "
            string strdate = System.DateTime.Now.ToString("dd-MM-yyyy hh:mm ", CultureInfo.InvariantCulture);
            switches += (Convert.ToString("--footer-right \"") + strdate) + "\"  ";
            switches += "--encoding UTF-8  ";
            switches = " -q " + switches + " " + url + "  " + strTempPdfPath;

            var proc = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = wkhtml,
                    Arguments = switches,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    WorkingDirectory = wkhtmlDir,
                    CreateNoWindow = true
                }
            };
            proc.Start();

            proc.WaitForExit();

            using (Stream fs = new FileStream(strTempPdfPath, FileMode.Open))
            {
                using (Stream sOut = proc.StandardOutput.BaseStream)
                {
                    byte[] buffer = new byte[32769];
                    int read;
                    try
                    {
                        while ((read = sOut.Read(buffer, 0, buffer.Length)) > 0)
                            fs.Write(buffer, 0, read);
                    }
                    catch (Exception ex)
                    {
                    }
                }

                string errorStr = proc.StandardError.ReadToEnd();
                if (fs.Length == 0)
                {
                }
                MemoryStream ms = new MemoryStream();
                byte[] bytes = new byte[fs.Length + 1];
                fs.Read(bytes, 0, (int)fs.Length); ;
                ms.Write(bytes, 0, (int)fs.Length);
                fs.Close();

                return ms.ToArray();
            }
        }

        public string CreateTempHtmlFile(string DivContent, string strTempHtmlPath)
        {
            System.IO.StreamWriter s = new System.IO.StreamWriter(strTempHtmlPath, false);
            if (System.IO.File.Exists(strTempHtmlPath))
            {
                s.WriteLine(DivContent);
                s.Close();
            }
            else
            {
                Directory.CreateDirectory(strTempHtmlPath);
                s.WriteLine(DivContent);
                s.Close();
            }
            return strTempHtmlPath;
        }

    }
}


