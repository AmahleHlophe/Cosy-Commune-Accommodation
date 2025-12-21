namespace cosycommune.Models
{
    public class Facility
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Amenity { get; set; }

        [Required]
        public DateTime Date { get; set; }

        /// <summary>
        /// TimeOnly is ideal for clock times (e.g., 14:00) without a date component.
        /// Perfect for a "Start Time" field.
        /// </summary>
        [Required]
        public TimeOnly StartTime { get; set; }

        /// <summary>
        /// TimeSpan represents a duration or elapsed time.
        /// Perfect for "How long is the booking?".
        /// </summary>
        [Required]
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Helper property to calculate when the booking ends.
        /// Note: TimeOnly + TimeSpan automatically wraps around midnight.
        /// </summary>
        [NotMapped] // Prevents this from being created as a column in the database
        public TimeOnly EndTime => StartTime.Add(Duration);

        /// <summary>
        /// Optional: Returns a human-readable summary of the time slot.
        /// </summary>
        [NotMapped]
        public string TimeSlotSummary => $"{StartTime:HH:mm} to {EndTime:HH:mm} ({Duration.TotalHours} hrs)";
    }
}