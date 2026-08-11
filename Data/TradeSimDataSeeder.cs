using Microsoft.EntityFrameworkCore;
using TradeSim.Models.Domain;

namespace TradeSim.Data
{
    public static class TradeSimDataSeeder
    {
        public static async Task SeedAsync(TradeSimDbContext context)
        {
            // --------------------------------------------------
            // 1. Seed Tradable Instruments
            // --------------------------------------------------

            var instruments = new List<TradableInstrument>
            {
                // =========================
                // BANKING
                // =========================

                new TradableInstrument
                {
                    Symbol = "HDFCBANK",
                    Name = "HDFC Bank Ltd",
                    Exchange = "NSE",
                    InstrumentToken = "HDFCBANK_NSE",
                    AssetClass = "EQUITY",
                    Sector = "Banking",
                    IsTradable = true
                },

                new TradableInstrument
                {
                    Symbol = "ICICIBANK",
                    Name = "ICICI Bank Ltd",
                    Exchange = "NSE",
                    InstrumentToken = "ICICIBANK_NSE",
                    AssetClass = "EQUITY",
                    Sector = "Banking",
                    IsTradable = true
                },

                new TradableInstrument
                {
                    Symbol = "SBIN",
                    Name = "State Bank of India",
                    Exchange = "NSE",
                    InstrumentToken = "SBIN_NSE",
                    AssetClass = "EQUITY",
                    Sector = "Banking",
                    IsTradable = true
                },

                new TradableInstrument
                {
                    Symbol = "AXISBANK",
                    Name = "Axis Bank Ltd",
                    Exchange = "NSE",
                    InstrumentToken = "AXISBANK_NSE",
                    AssetClass = "EQUITY",
                    Sector = "Banking",
                    IsTradable = true
                },

                new TradableInstrument
                {
                    Symbol = "KOTAKBANK",
                    Name = "Kotak Mahindra Bank Ltd",
                    Exchange = "NSE",
                    InstrumentToken = "KOTAKBANK_NSE",
                    AssetClass = "EQUITY",
                    Sector = "Banking",
                    IsTradable = true
                },

                new TradableInstrument
                {
                    Symbol = "INDUSINDBK",
                    Name = "IndusInd Bank Ltd",
                    Exchange = "NSE",
                    InstrumentToken = "INDUSINDBK_NSE",
                    AssetClass = "EQUITY",
                    Sector = "Banking",
                    IsTradable = true
                },

                new TradableInstrument
                {
                    Symbol = "BANKBARODA",
                    Name = "Bank of Baroda",
                    Exchange = "NSE",
                    InstrumentToken = "BANKBARODA_NSE",
                    AssetClass = "EQUITY",
                    Sector = "Banking",
                    IsTradable = true
                },

                // =========================
                // IT
                // =========================

                new TradableInstrument
                {
                    Symbol = "TCS",
                    Name = "Tata Consultancy Services Ltd",
                    Exchange = "NSE",
                    InstrumentToken = "TCS_NSE",
                    AssetClass = "EQUITY",
                    Sector = "IT",
                    IsTradable = true
                },

                new TradableInstrument
                {
                    Symbol = "INFY",
                    Name = "Infosys Ltd",
                    Exchange = "NSE",
                    InstrumentToken = "INFY_NSE",
                    AssetClass = "EQUITY",
                    Sector = "IT",
                    IsTradable = true
                },

                new TradableInstrument
                {
                    Symbol = "HCLTECH",
                    Name = "HCL Technologies Ltd",
                    Exchange = "NSE",
                    InstrumentToken = "HCLTECH_NSE",
                    AssetClass = "EQUITY",
                    Sector = "IT",
                    IsTradable = true
                },

                new TradableInstrument
                {
                    Symbol = "WIPRO",
                    Name = "Wipro Ltd",
                    Exchange = "NSE",
                    InstrumentToken = "WIPRO_NSE",
                    AssetClass = "EQUITY",
                    Sector = "IT",
                    IsTradable = true
                },

                new TradableInstrument
                {
                    Symbol = "TECHM",
                    Name = "Tech Mahindra Ltd",
                    Exchange = "NSE",
                    InstrumentToken = "TECHM_NSE",
                    AssetClass = "EQUITY",
                    Sector = "IT",
                    IsTradable = true
                },

                new TradableInstrument
                {
                    Symbol = "LTIM",
                    Name = "LTIMindtree Ltd",
                    Exchange = "NSE",
                    InstrumentToken = "LTIM_NSE",
                    AssetClass = "EQUITY",
                    Sector = "IT",
                    IsTradable = true
                },

                // =========================
                // ENERGY
                // =========================

                new TradableInstrument
                {
                    Symbol = "RELIANCE",
                    Name = "Reliance Industries Ltd",
                    Exchange = "NSE",
                    InstrumentToken = "RELIANCE_NSE",
                    AssetClass = "EQUITY",
                    Sector = "Energy",
                    IsTradable = true
                },

                new TradableInstrument
                {
                    Symbol = "ONGC",
                    Name = "Oil and Natural Gas Corporation Ltd",
                    Exchange = "NSE",
                    InstrumentToken = "ONGC_NSE",
                    AssetClass = "EQUITY",
                    Sector = "Energy",
                    IsTradable = true
                },

                new TradableInstrument
                {
                    Symbol = "NTPC",
                    Name = "NTPC Ltd",
                    Exchange = "NSE",
                    InstrumentToken = "NTPC_NSE",
                    AssetClass = "EQUITY",
                    Sector = "Energy",
                    IsTradable = true
                },

                new TradableInstrument
                {
                    Symbol = "POWERGRID",
                    Name = "Power Grid Corporation of India Ltd",
                    Exchange = "NSE",
                    InstrumentToken = "POWERGRID_NSE",
                    AssetClass = "EQUITY",
                    Sector = "Energy",
                    IsTradable = true
                },

                new TradableInstrument
                {
                    Symbol = "COALINDIA",
                    Name = "Coal India Ltd",
                    Exchange = "NSE",
                    InstrumentToken = "COALINDIA_NSE",
                    AssetClass = "EQUITY",
                    Sector = "Energy",
                    IsTradable = true
                },

                // =========================
                // FMCG
                // =========================

                new TradableInstrument
                {
                    Symbol = "ITC",
                    Name = "ITC Ltd",
                    Exchange = "NSE",
                    InstrumentToken = "ITC_NSE",
                    AssetClass = "EQUITY",
                    Sector = "FMCG",
                    IsTradable = true
                },

                new TradableInstrument
                {
                    Symbol = "HINDUNILVR",
                    Name = "Hindustan Unilever Ltd",
                    Exchange = "NSE",
                    InstrumentToken = "HINDUNILVR_NSE",
                    AssetClass = "EQUITY",
                    Sector = "FMCG",
                    IsTradable = true
                },

                new TradableInstrument
                {
                    Symbol = "NESTLEIND",
                    Name = "Nestle India Ltd",
                    Exchange = "NSE",
                    InstrumentToken = "NESTLEIND_NSE",
                    AssetClass = "EQUITY",
                    Sector = "FMCG",
                    IsTradable = true
                },

                new TradableInstrument
                {
                    Symbol = "BRITANNIA",
                    Name = "Britannia Industries Ltd",
                    Exchange = "NSE",
                    InstrumentToken = "BRITANNIA_NSE",
                    AssetClass = "EQUITY",
                    Sector = "FMCG",
                    IsTradable = true
                },

                new TradableInstrument
                {
                    Symbol = "DABUR",
                    Name = "Dabur India Ltd",
                    Exchange = "NSE",
                    InstrumentToken = "DABUR_NSE",
                    AssetClass = "EQUITY",
                    Sector = "FMCG",
                    IsTradable = true
                },

                new TradableInstrument
                {
                    Symbol = "MARICO",
                    Name = "Marico Ltd",
                    Exchange = "NSE",
                    InstrumentToken = "MARICO_NSE",
                    AssetClass = "EQUITY",
                    Sector = "FMCG",
                    IsTradable = true
                },

                // =========================
                // AUTO
                // =========================

                new TradableInstrument
                {
                    Symbol = "MARUTI",
                    Name = "Maruti Suzuki India Ltd",
                    Exchange = "NSE",
                    InstrumentToken = "MARUTI_NSE",
                    AssetClass = "EQUITY",
                    Sector = "Auto",
                    IsTradable = true
                },

                new TradableInstrument
                {
                    Symbol = "TATAMOTORS",
                    Name = "Tata Motors Ltd",
                    Exchange = "NSE",
                    InstrumentToken = "TATAMOTORS_NSE",
                    AssetClass = "EQUITY",
                    Sector = "Auto",
                    IsTradable = true
                },

                new TradableInstrument
                {
                    Symbol = "M&M",
                    Name = "Mahindra & Mahindra Ltd",
                    Exchange = "NSE",
                    InstrumentToken = "M&M_NSE",
                    AssetClass = "EQUITY",
                    Sector = "Auto",
                    IsTradable = true
                },

                new TradableInstrument
                {
                    Symbol = "BAJAJ-AUTO",
                    Name = "Bajaj Auto Ltd",
                    Exchange = "NSE",
                    InstrumentToken = "BAJAJ-AUTO_NSE",
                    AssetClass = "EQUITY",
                    Sector = "Auto",
                    IsTradable = true
                },

                new TradableInstrument
                {
                    Symbol = "EICHERMOT",
                    Name = "Eicher Motors Ltd",
                    Exchange = "NSE",
                    InstrumentToken = "EICHERMOT_NSE",
                    AssetClass = "EQUITY",
                    Sector = "Auto",
                    IsTradable = true
                },

                new TradableInstrument
                {
                    Symbol = "HEROMOTOCO",
                    Name = "Hero MotoCorp Ltd",
                    Exchange = "NSE",
                    InstrumentToken = "HEROMOTOCO_NSE",
                    AssetClass = "EQUITY",
                    Sector = "Auto",
                    IsTradable = true
                },

                // =========================
                // PHARMA
                // =========================

                new TradableInstrument
                {
                    Symbol = "SUNPHARMA",
                    Name = "Sun Pharmaceutical Industries Ltd",
                    Exchange = "NSE",
                    InstrumentToken = "SUNPHARMA_NSE",
                    AssetClass = "EQUITY",
                    Sector = "Pharma",
                    IsTradable = true
                },

                new TradableInstrument
                {
                    Symbol = "DRREDDY",
                    Name = "Dr. Reddy's Laboratories Ltd",
                    Exchange = "NSE",
                    InstrumentToken = "DRREDDY_NSE",
                    AssetClass = "EQUITY",
                    Sector = "Pharma",
                    IsTradable = true
                },

                new TradableInstrument
                {
                    Symbol = "CIPLA",
                    Name = "Cipla Ltd",
                    Exchange = "NSE",
                    InstrumentToken = "CIPLA_NSE",
                    AssetClass = "EQUITY",
                    Sector = "Pharma",
                    IsTradable = true
                },

                new TradableInstrument
                {
                    Symbol = "DIVISLAB",
                    Name = "Divi's Laboratories Ltd",
                    Exchange = "NSE",
                    InstrumentToken = "DIVISLAB_NSE",
                    AssetClass = "EQUITY",
                    Sector = "Pharma",
                    IsTradable = true
                },

                new TradableInstrument
                {
                    Symbol = "APOLLOHOSP",
                    Name = "Apollo Hospitals Enterprise Ltd",
                    Exchange = "NSE",
                    InstrumentToken = "APOLLOHOSP_NSE",
                    AssetClass = "EQUITY",
                    Sector = "Pharma",
                    IsTradable = true
                },

                // =========================
                // TELECOM
                // =========================

                new TradableInstrument
                {
                    Symbol = "BHARTIARTL",
                    Name = "Bharti Airtel Ltd",
                    Exchange = "NSE",
                    InstrumentToken = "BHARTIARTL_NSE",
                    AssetClass = "EQUITY",
                    Sector = "Telecom",
                    IsTradable = true
                },

                // =========================
                // INFRASTRUCTURE
                // =========================

                new TradableInstrument
                {
                    Symbol = "LT",
                    Name = "Larsen & Toubro Ltd",
                    Exchange = "NSE",
                    InstrumentToken = "LT_NSE",
                    AssetClass = "EQUITY",
                    Sector = "Infrastructure",
                    IsTradable = true
                },

                new TradableInstrument
                {
                    Symbol = "ADANIPORTS",
                    Name = "Adani Ports and Special Economic Zone Ltd",
                    Exchange = "NSE",
                    InstrumentToken = "ADANIPORTS_NSE",
                    AssetClass = "EQUITY",
                    Sector = "Infrastructure",
                    IsTradable = true
                },

                new TradableInstrument
                {
                    Symbol = "ULTRACEMCO",
                    Name = "UltraTech Cement Ltd",
                    Exchange = "NSE",
                    InstrumentToken = "ULTRACEMCO_NSE",
                    AssetClass = "EQUITY",
                    Sector = "Infrastructure",
                    IsTradable = true
                },

                // =========================
                // METALS
                // =========================

                new TradableInstrument
                {
                    Symbol = "TATASTEEL",
                    Name = "Tata Steel Ltd",
                    Exchange = "NSE",
                    InstrumentToken = "TATASTEEL_NSE",
                    AssetClass = "EQUITY",
                    Sector = "Metals",
                    IsTradable = true
                },

                new TradableInstrument
                {
                    Symbol = "HINDALCO",
                    Name = "Hindalco Industries Ltd",
                    Exchange = "NSE",
                    InstrumentToken = "HINDALCO_NSE",
                    AssetClass = "EQUITY",
                    Sector = "Metals",
                    IsTradable = true
                },

                new TradableInstrument
                {
                    Symbol = "JSWSTEEL",
                    Name = "JSW Steel Ltd",
                    Exchange = "NSE",
                    InstrumentToken = "JSWSTEEL_NSE",
                    AssetClass = "EQUITY",
                    Sector = "Metals",
                    IsTradable = true
                },

                // =========================
                // CONSUMER / RETAIL
                // =========================

                new TradableInstrument
                {
                    Symbol = "TITAN",
                    Name = "Titan Company Ltd",
                    Exchange = "NSE",
                    InstrumentToken = "TITAN_NSE",
                    AssetClass = "EQUITY",
                    Sector = "Consumer",
                    IsTradable = true
                },

                new TradableInstrument
                {
                    Symbol = "TRENT",
                    Name = "Trent Ltd",
                    Exchange = "NSE",
                    InstrumentToken = "TRENT_NSE",
                    AssetClass = "EQUITY",
                    Sector = "Consumer",
                    IsTradable = true
                },

                // =========================
                // CONGLOMERATE / INDUSTRIAL
                // =========================

                new TradableInstrument
                {
                    Symbol = "ADANIENT",
                    Name = "Adani Enterprises Ltd",
                    Exchange = "NSE",
                    InstrumentToken = "ADANIENT_NSE",
                    AssetClass = "EQUITY",
                    Sector = "Conglomerate",
                    IsTradable = true
                },

                new TradableInstrument
                {
                    Symbol = "SIEMENS",
                    Name = "Siemens Ltd",
                    Exchange = "NSE",
                    InstrumentToken = "SIEMENS_NSE",
                    AssetClass = "EQUITY",
                    Sector = "Industrial",
                    IsTradable = true
                },

                new TradableInstrument
                {
                    Symbol = "BEL",
                    Name = "Bharat Electronics Ltd",
                    Exchange = "NSE",
                    InstrumentToken = "BEL_NSE",
                    AssetClass = "EQUITY",
                    Sector = "Industrial",
                    IsTradable = true
                }
            };

            // --------------------------------------------------
            // 2. Insert only instruments that don't exist
            // --------------------------------------------------

            var existingSymbols = await context.TradableInstruments
                .Select(x => new
                {
                    x.Symbol,
                    x.Exchange
                })
                .ToListAsync();

            foreach (var instrument in instruments)
            {
                bool exists = existingSymbols.Any(x =>
                    x.Symbol == instrument.Symbol &&
                    x.Exchange == instrument.Exchange);

                if (!exists)
                {
                    context.TradableInstruments.Add(instrument);
                }
            }

            await context.SaveChangesAsync();

            // --------------------------------------------------
            // 3. Seed Market Instrument Profiles
            // --------------------------------------------------

            await SeedMarketInstrumentProfilesAsync(context);
        }

        // --------------------------------------------------
        // Seed Market Instrument Profiles
        // --------------------------------------------------

        private static async Task SeedMarketInstrumentProfilesAsync(
            TradeSimDbContext context)
        {
            var profileData =
                new Dictionary<string, MarketProfileSeedData>
                {
                    // =========================
                    // BANKING
                    // =========================

                    ["HDFCBANK"] =
                        new(1680.00m, 1.50m, 1.10m, 22_000_000, 1790m, 1363m),

                    ["ICICIBANK"] =
                        new(1450.00m, 1.55m, 1.08m, 18_000_000, 1600m, 1050m),

                    ["SBIN"] =
                        new(820.00m, 1.90m, 1.20m, 35_000_000, 950m, 500m),

                    ["AXISBANK"] =
                        new(1200.00m, 1.65m, 1.12m, 12_000_000, 1350m, 850m),

                    ["KOTAKBANK"] =
                        new(1950.00m, 1.40m, 0.95m, 8_000_000, 2300m, 1600m),

                    ["INDUSINDBK"] =
                        new(1050.00m, 2.20m, 1.35m, 7_000_000, 1600m, 900m),

                    ["BANKBARODA"] =
                        new(250.00m, 2.00m, 1.25m, 25_000_000, 300m, 150m),

                    // =========================
                    // IT
                    // =========================

                    ["TCS"] =
                        new(4312.20m, 1.40m, 0.85m, 10_000_000, 4592m, 3311m),

                    ["INFY"] =
                        new(1782.40m, 1.60m, 0.95m, 18_000_000, 2006m, 1358m),

                    ["HCLTECH"] =
                        new(1650.00m, 1.55m, 0.90m, 9_000_000, 1900m, 1200m),

                    ["WIPRO"] =
                        new(550.00m, 1.70m, 0.88m, 14_000_000, 700m, 400m),

                    ["TECHM"] =
                        new(1650.00m, 1.80m, 1.00m, 6_000_000, 1800m, 1100m),

                    ["LTIM"] =
                        new(5600.00m, 1.75m, 0.90m, 1_500_000, 6500m, 4500m),

                    // =========================
                    // ENERGY
                    // =========================

                    ["RELIANCE"] =
                        new(2845.50m, 1.80m, 1.05m, 25_000_000, 3217m, 2220m),

                    ["ONGC"] =
                        new(250.00m, 2.10m, 1.15m, 30_000_000, 345m, 180m),

                    ["NTPC"] =
                        new(350.00m, 1.50m, 0.85m, 20_000_000, 450m, 250m),

                    ["POWERGRID"] =
                        new(330.00m, 1.30m, 0.70m, 15_000_000, 380m, 220m),

                    ["COALINDIA"] =
                        new(400.00m, 1.90m, 1.05m, 18_000_000, 550m, 350m),

                    // =========================
                    // FMCG
                    // =========================

                    ["ITC"] =
                        new(430.00m, 1.20m, 0.75m, 30_000_000, 528m, 399m),

                    ["HINDUNILVR"] =
                        new(2450.00m, 1.30m, 0.65m, 4_000_000, 3035m, 2170m),

                    ["NESTLEIND"] =
                        new(2550.00m, 1.10m, 0.55m, 2_000_000, 2800m, 2100m),

                    ["BRITANNIA"] =
                        new(5200.00m, 1.30m, 0.65m, 1_000_000, 6200m, 4300m),

                    ["DABUR"] =
                        new(520.00m, 1.20m, 0.60m, 4_000_000, 700m, 450m),

                    ["MARICO"] =
                        new(750.00m, 1.25m, 0.65m, 5_000_000, 850m, 500m),

                    // =========================
                    // AUTO
                    // =========================

                    ["MARUTI"] =
                        new(12500.00m, 1.70m, 1.05m, 1_500_000, 15000m, 9500m),

                    ["TATAMOTORS"] =
                        new(700.00m, 2.20m, 1.30m, 35_000_000, 1100m, 650m),

                    ["M&M"] =
                        new(3200.00m, 1.90m, 1.20m, 5_000_000, 3800m, 2300m),

                    ["BAJAJ-AUTO"] =
                        new(8500.00m, 1.60m, 0.95m, 1_000_000, 10500m, 7000m),

                    ["EICHERMOT"] =
                        new(6000.00m, 1.80m, 1.05m, 1_500_000, 7000m, 4000m),

                    ["HEROMOTOCO"] =
                        new(5000.00m, 1.70m, 0.95m, 1_500_000, 6000m, 3500m),

                    // =========================
                    // PHARMA
                    // =========================

                    ["SUNPHARMA"] =
                        new(1750.00m, 1.50m, 0.75m, 8_000_000, 2000m, 1200m),

                    ["DRREDDY"] =
                        new(1250.00m, 1.60m, 0.70m, 3_000_000, 1500m, 900m),

                    ["CIPLA"] =
                        new(1500.00m, 1.40m, 0.65m, 5_000_000, 1750m, 1000m),

                    ["DIVISLAB"] =
                        new(6500.00m, 1.70m, 0.80m, 1_000_000, 7500m, 4500m),

                    ["APOLLOHOSP"] =
                        new(7500.00m, 1.80m, 0.90m, 1_000_000, 8500m, 5000m),

                    // =========================
                    // TELECOM
                    // =========================

                    ["BHARTIARTL"] =
                        new(1900.00m, 1.70m, 0.95m, 12_000_000, 2200m, 1200m),

                    // =========================
                    // INFRASTRUCTURE
                    // =========================

                    ["LT"] =
                        new(3800.00m, 1.70m, 1.05m, 3_000_000, 4500m, 2500m),

                    ["ADANIPORTS"] =
                        new(1450.00m, 2.20m, 1.25m, 8_000_000, 1800m, 800m),

                    ["ULTRACEMCO"] =
                        new(12500.00m, 1.60m, 1.00m, 600_000, 14000m, 9000m),

                    // =========================
                    // METALS
                    // =========================

                    ["TATASTEEL"] =
                        new(180.00m, 2.30m, 1.35m, 50_000_000, 220m, 100m),

                    ["HINDALCO"] =
                        new(750.00m, 2.20m, 1.30m, 12_000_000, 900m, 500m),

                    ["JSWSTEEL"] =
                        new(1100.00m, 2.10m, 1.25m, 15_000_000, 1400m, 700m),

                    // =========================
                    // CONSUMER
                    // =========================

                    ["TITAN"] =
                        new(3500.00m, 1.70m, 0.95m, 3_000_000, 4200m, 2500m),

                    ["TRENT"] =
                        new(6000.00m, 2.10m, 1.05m, 2_000_000, 7500m, 3000m),

                    // =========================
                    // INDUSTRIAL / CONGLOMERATE
                    // =========================

                    ["ADANIENT"] =
                        new(2500.00m, 2.50m, 1.35m, 8_000_000, 3500m, 1800m),

                    ["SIEMENS"] =
                        new(7000.00m, 1.60m, 1.00m, 1_000_000, 8500m, 5000m),

                    ["BEL"] =
                        new(400.00m, 2.00m, 1.10m, 25_000_000, 500m, 180m)
                };

            var instruments = await context.TradableInstruments
                .ToListAsync();

            foreach (var instrument in instruments)
            {
                if (!profileData.TryGetValue(
                        instrument.Symbol,
                        out var data))
                {
                    continue;
                }

                bool profileExists =
                    await context.MarketInstrumentProfiles
                        .AnyAsync(p =>
                            p.TradableInstrumentId == instrument.Id);

                if (profileExists)
                {
                    continue;
                }

                decimal support = Math.Round(
                    data.BasePrice * 0.97m,
                    2);

                decimal resistance = Math.Round(
                    data.BasePrice * 1.03m,
                    2);

                var profile = new MarketInstrumentProfile
                {
                    TradableInstrumentId =
                        instrument.Id,

                    BasePrice =
                        data.BasePrice,

                    VolatilityPercent =
                        data.VolatilityPercent,

                    Beta =
                        data.Beta,

                    TrendBias =
                        GetTrendBias(instrument.Symbol),

                    MomentumBias =
                        GetMomentumBias(instrument.Symbol),

                    AverageVolume =
                        data.AverageVolume,

                    PreviousClose =
                        data.BasePrice,

                    FiftyTwoWeekHigh =
                        data.FiftyTwoWeekHigh,

                    FiftyTwoWeekLow =
                        data.FiftyTwoWeekLow,

                    TickSize = 0.05m,

                    PriceBandPercent = 20m,

                    SupportLevel = support,

                    ResistanceLevel = resistance
                };

                context.MarketInstrumentProfiles.Add(profile);
            }

            await context.SaveChangesAsync();
        }

        // --------------------------------------------------
        // Trend Bias
        // --------------------------------------------------

        private static decimal GetTrendBias(string symbol)
        {
            return symbol switch
            {
                "RELIANCE" => 0.15m,
                "TCS" => 0.12m,
                "INFY" => 0.14m,
                "HDFCBANK" => 0.16m,
                "ICICIBANK" => 0.15m,
                "SBIN" => 0.12m,

                "ITC" => 0.08m,
                "HINDUNILVR" => 0.07m,
                "NESTLEIND" => 0.06m,

                "TATAMOTORS" => 0.10m,
                "SUNPHARMA" => 0.11m,
                "BHARTIARTL" => 0.13m,

                _ => 0.08m
            };
        }

        // --------------------------------------------------
        // Momentum Bias
        // --------------------------------------------------

        private static decimal GetMomentumBias(string symbol)
        {
            return symbol switch
            {
                "INFY" => 0.12m,
                "RELIANCE" => 0.10m,
                "TCS" => 0.08m,
                "HDFCBANK" => 0.10m,
                "ICICIBANK" => 0.09m,

                "TATAMOTORS" => 0.14m,
                "TATASTEEL" => 0.12m,
                "ADANIENT" => 0.15m,

                "ITC" => 0.05m,
                "HINDUNILVR" => 0.04m,
                "NESTLEIND" => 0.03m,

                _ => 0.06m
            };
        }
    }

    // --------------------------------------------------
    // Seed data structure for MarketInstrumentProfile
    // --------------------------------------------------

    public record MarketProfileSeedData(
        decimal BasePrice,
        decimal VolatilityPercent,
        decimal Beta,
        long AverageVolume,
        decimal FiftyTwoWeekHigh,
        decimal FiftyTwoWeekLow
    );
}