namespace cosycommune.Models
{
    public class Tenant
    {
        public int Id { get; set; }

        [Display(Name = "Name/s")]
        public string? FullName { get; set; }

        [Display(Name = "ID Number")]
        public string? IdNumber { get; set; }

        [Display(Name = "ID Number")]
        public string? Email { get; set; }

        [Display(Name = "ID Number")]
        public DateTime CreatedAt { get; set; }
        
        //Navigation
        public IEnumerable<Lease>? Leases { get;set; }
    }
}