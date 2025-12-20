namespace cosycommune.Models
{
    public class Maintenance
    {
        public int Id {get;set;}

        // The Unit number associated with the tenant logging the issue
    [Display(Name = "Unit ")]
    public string? Unit { get; set; }

    [Required(ErrorMessage = "Please select a category")]
    public string? Category { get; set; }

    [Required(ErrorMessage = "Please select a priority level")]
    public string? Priority { get; set; } // Low, Medium, High

    [Required(ErrorMessage = "Please specify the location within the unit")]
    [Display(Name = "Location in Unit")]
    [StringLength(100)]
    public string? LocationInUnit { get; set; }

    [Required(ErrorMessage = "Please select a preferred contact time")]
    [Display(Name = "Best Time for Entry")]
    public string? PreferredAccessTime { get; set; }

    [Required(ErrorMessage = "Please provide a description of the issue")]
    [DataType(DataType.MultilineText)]
    [StringLength(1000)]
    public string? Description { get; set; }

    // Path to the stored image
    public string? ImagePath { get; set; }

    // Used for file upload in the form (Not mapped to Database)
    [NotMapped]
    [Display(Name = "Upload Photo")]
    public IFormFile? ImageFile { get; set; }

    // Tracking properties
    public string Status { get; set; } = "Pending"; // Pending, InProgress, Completed
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Linking to the Tenant
    public Guid TenantId { get; set; }
    public virtual Tenant? Tenant { get; set; }
    }
}