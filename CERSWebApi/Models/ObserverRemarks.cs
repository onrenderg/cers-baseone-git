
namespace CERSWebApi.Models
{
    public class ObserverRemarks_POST
    {
        public string ExpenseId { get; set; }
        public string ObserverId { get; set; }
        public string ObserverRemarks { get; set; }
                    
    }
    public class ObserverRemarks_update
    {
        public string ExpenseId { get; set; }
        public string ObserverRemarksId { get; set; }
        public string ObserverRemarks { get; set; }

    }

}