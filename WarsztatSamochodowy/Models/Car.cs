namespace WarsztatSamochodowy.Models
{
    public class Car

    {
        public int CarId { get; set; }
        public string? CarMake { get; set; }
        public string? CarModel { get; set; }
        public DateTime Arrival { get; set; }
        public DateTime DateDone { get; set; }
        public CarOwner? Owner { get; set; }
        public string? Registration { get; set; }


    }
}
