namespace cosycommune.Models
{
    /// <summary>
    /// A standard class representing a resident's checkout or move-out record.
    /// </summary>
    public class Checkout
    {
        // Unique identifier for the record
        public int Id { get; set; }

        // Link to the resident/user
        public string? UserId { get; set; }

        // The date the resident plans to leave
        public DateTime VacateDate { get; set; }

        // The reason provided for moving out
        public string? Reason { get; set; }

        // Current status (e.g., Pending, Approved, Completed)
        public string? Status { get; set; }

        // When the record was created
        public DateTime CreatedAt { get; set; }

        // Constructor to initialize defaults
        public Checkout()
        {
            CreatedAt = DateTime.Now;
            Status = "Pending";
        }
    }
}