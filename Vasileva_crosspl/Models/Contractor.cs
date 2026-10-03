namespace Vasileva_crosspl.Models
{
    public  enum MaintenanceStrategy
    {
        Annual, //раз в год
        Biennial, //раз в два года
        ConditionBased //по состоянию
    }
    
    public class Contractor
    {
        public int Id {  get; set; }
        public string CompanyName {  get; set; }
        public MaintenanceStrategy Strategy { get; set; }
        public decimal BasePricePerKm {  get; set; } //базовая цена за км
        public int Rating {  get; set; }//рейтинг от 1 до 5

        //расчет даты след то в зависимости от стратегии
        public DateTime CalcNextMainDate(DateTime lastDate)
        {
            return Strategy switch
            {
                MaintenanceStrategy.Annual => lastDate.AddYears(1),
                MaintenanceStrategy.Biennial => lastDate.AddYears(2),
                MaintenanceStrategy.ConditionBased => lastDate.AddMonths(6),
                _ => lastDate.AddYears(1)
            };
        }

        //расчет стоимости то
        public decimal CalcCost(Pipeline pipe)
        {
            decimal diam = pipe.Diameter > 1000 ? 1.5m : 1.0m;
            decimal ratingDiscount = Rating < 3 ? 0.95m : 1.0m; //скидка 5% за низкий рейтинг
            
            return pipe.Length * BasePricePerKm * diam * ratingDiscount;

        }
    }
}
