
namespace CERSWebApi.Models
{
    public class UserDetails_Get_otpid
    {
        public string OTPID { get; set; }

    }
	
    public class UserDetails_Get
    {
        public string UserType { get; set; }
        public string AUTO_ID { get; set; }
        public string EPIC_NO { get; set; }
        public string VOTER_NAME { get; set; }
        public string RELATION_TYPE { get; set; }
        public string RELATIVE_NAME { get; set; }
        public string GENDER { get; set; }
        public string AGE { get; set; }
        public string EMAIL_ID { get; set; }
        public string MOBILE_NUMBER { get; set; }
        public string AgentName { get; set; }
        public string AgentMobile { get; set; }
        public string Panchayat_Name { get; set; }
        public string LoggedInAs { get; set; }                 
        public string OTPID { get; set; }                 
        public string NominationForName { get; set; }                 
        public string NominationForNameLocal { get; set; }                 
        public string PollDate { get; set; }                 
        public string postcode { get; set; }                 
        public string LimitAmt { get; set; }                 
        public string NominationDate { get; set; }                 
        public string ResultDate { get; set; }                 
        public string Resultdatethirtydays { get; set; }     
        public string Block_Code { get; set; }     
        public string panwardcouncilname { get; set; }     
        public string panwardcouncilnamelocal { get; set; }     
        public string ExpStatus { get; set; }     
        


        //observer

                   
    }

    public class UserDetails_Observer_Get
    {
        public string Auto_ID { get; set; }
        public string ObserverName { get; set; }
        public string ObserverContact { get; set; }
        public string ObserverDesignation { get; set; }
        public string Pritype { get; set; }
    }


    }