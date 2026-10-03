namespace Vasileva_crosspl.Models
{
    public class MaintenanceRecord
    {
        public int Id { get; set; }
        public int PipelineId { get; set; }
        public Pipeline? Pipeline { get; set; }

        public int ContractorId { get; set; }
        public Contractor? Contractor { get; set; }

        public DateTime Date { get; set; }
        public decimal ActualCost { get; set; }
        public string Notes { get; set; }
    }
}
