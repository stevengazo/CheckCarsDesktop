namespace CheckCarsDesktop.Models
{
    public class CarService
    {
        public int CarServiceId { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }

        // add is oil change service
        // add actual odometer

        // add next odomete service (only in oil change)

        public int CarId { get; set; }
        public Car Car { get; set; }


    }
}
