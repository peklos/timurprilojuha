namespace AdmissionSystem.Models
{
    public class Specialty
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int PlacesCount { get; set; }
        public double MinScore { get; set; }
        public string Description { get; set; }
    }
}
