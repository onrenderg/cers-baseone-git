

namespace CERSWebApi.Models
{
    public class ExpenditureDetails_Post
    {
        
        public string ExpenseID { get; set; }
        public string AutoID { get; set; }
        public string expDate { get; set; }
        public string expCode { get; set; }
        public string amtType { get; set; }
        public string amount { get; set; }
        public string paymentDate { get; set; }
        public string voucherBillNumber { get; set; }
        public string payMode { get; set; }
        public string payeeName { get; set; }
     
        public string payeeAddress { get; set; }
        public string sourceMoney { get; set; }
        public string remarks { get; set; }
        public string evidenceFile { get; set; }
      

    }

    public class ExpenditureDetails_Get
    {   
        public string ExpenseID { get; set; }
        public string AutoID { get; set; }
        public string expDate { get; set; }
        public string expCode { get; set; }
        public string amtType { get; set; }
        public string amount { get; set; }
        public string paymentDate { get; set; }
        public string voucherBillNumber { get; set; }
        public string payMode { get; set; }
        public string payeeName { get; set; }   
        public string EnteredOn { get; set; }
        public string payeeAddress { get; set; }
        public string sourceMoney { get; set; }
        public string remarks { get; set; }
        public string DtTm { get; set; }
        public string ExpStatus { get; set; } 
        
        public string ExpTypeName { get; set; }
        public string ExpTypeNameLocal { get; set; }
        public string PayModeName { get; set; }
        public string PayModeNameLocal { get; set; }
        public string evidenceFile { get; set; }
        public string expDateDisplay { get; set; }
        public string paymentDateDisplay { get; set; }
        public string amountoutstanding { get; set; }
        public string ObserverRemarks { get; set; }
        public string CONSTITUENCY_CODE { get; set; }
        public string VOTER_NAME { get; set; }
        public string AgentName { get; set; }
       
        

    }

}