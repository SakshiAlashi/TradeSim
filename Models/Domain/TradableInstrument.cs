namespace TradeSim.Models.Domain
{
    public class TradableInstrument
    {
        public int Id { get; set; }

        public string Symbol { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Exchange { get; set; } = string.Empty;

        public string InstrumentToken { get; set; } = string.Empty;

        public string AssetClass { get; set; } = string.Empty;

        public bool IsTradable { get; set; } = true;
    }
}