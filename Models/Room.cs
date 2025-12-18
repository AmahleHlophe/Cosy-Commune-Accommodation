namespace cosycommune.Models
{
    public class Room
    {
        public int Id { get; set; }

        [Display(Name = "Room Number")]
        public int RoomNumber { get; set; }

        [Display(Name = "Room Type")]
        public string? RoomType { get; set; }

        [Display(Name = "Price")]
        public decimal? Price { get; set; }

        [Display(Name = "Capacity")]
        public int Capacity{ get; set; }

        [Display(Name = "IsOccupied")]
        public bool IsOccupied {get;set;}


        //Navigation
        public IEnumerable<Lease>? Leases {get;set;}
    }
}