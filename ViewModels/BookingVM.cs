namespace cosycommune.ViewModels
{
    public class BookingVM
    {
        public string? BookingType {get;set;}
        public string? FullName {get;set;}
        public string? IdNumber {get;set;}
        public string? Email {get;set;}


        public List<Room>? Rooms { get;set; }
    }
}