namespace TradeSim.Models.ViewModels
{
    public class IndexCardViewModel
    {
        public string Name { get; set; } = string.Empty;

        public decimal Value { get; set; }

        public decimal ChangePercent { get; set; }
    }
}