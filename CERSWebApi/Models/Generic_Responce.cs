
namespace CERSWebApi.Models
{
    public class Generic_Responce
    {
        public int status_code { get; set; } = 500;
        public string Message { get; set; } = "Something went Wrong\nPlease try again later";       
        public string ExpenseID { get; set; }      
        public string developer_message { get; set; } = "Web Service is in Exception";
        public object error_list { get; set; }
        public object data { get; set; }
        public string TokenID { get; set; }
    }
}