namespace TradeSim.Models.DTOs
{
    public class InstrumentDetailsDto : MarketQuoteDto
    {
        public decimal PreviousClose { get; set; }

        public decimal UpperCircuit { get; set; }

        public decimal LowerCircuit { get; set; }
    }
}