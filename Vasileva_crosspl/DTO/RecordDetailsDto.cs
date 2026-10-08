namespace Vasileva_crosspl.Dtos
{
    public class RecordDetailsDto
    {
        public int RecordId { get; set; }
        public string Pipeline { get; set; }
        public string Contractor { get; set; }
        public DateTime Date { get; set; }
        public decimal ActualCost { get; set; }
        public decimal EstimatedCost { get; set; }
        public string Notes { get; set; }
    }
}