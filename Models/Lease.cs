namespace cosycommune.Models
{
    public class Lease
    {
        public int Id { get; set; }

        [Display(Name = "Room")]
        public Room? Room { get; set; }
        public int RoomId { get;set; }

        [Display(Name = "Tenant")]
        public Tenant? Tenant { get; set; }
        public int TenantId { get;set; }

        [DataType(DataType.Date)]
        [Display(Name = "Lease Start Date")]
        public DateTime LeaseStart { get;set; }

        [DataType(DataType.Date)]
        [Display(Name = "Lease End Date")]
        public DateTime LeaseEnd { get;set; }

        [Display(Name = "Monthly Rent")]
        public decimal? MonthlyRent { get; set; }

        [Display(Name = "Deposit")]
        public decimal? Deposit { get; set; }

        [Display(Name = "IsOccupied")]
        public bool IsOccupied {get;set;}
    }
}