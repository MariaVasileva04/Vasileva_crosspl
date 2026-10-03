namespace Vasileva_crosspl.Models
{
    public class Pipeline
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Diameter { get; set; }
        public decimal Length {  get; set; }
        public string Material { get; set; }
        public DateTime InstallDate { get; set; }
        public DateTime? LastMaintenanceDate { get; set; }

        //расчет износа
        public double CalculateWearPercentage()
        {
            int yearsInService = DateTime.Now.Year - InstallDate.Year;
            double baseWear = yearsInService * 2.5; //2.5% в год
            return Math.Min(baseWear, 100);
        }

        //требует ли труба срочного ТО
        public bool RequiresUrgentMaintenance()
        {
            if (CalculateWearPercentage() > 80) return true;
            if (LastMaintenanceDate == null) return true;
            return (DateTime.Now - LastMaintenanceDate.Value).TotalDays > 1095; //3 года
        }


    }
}
