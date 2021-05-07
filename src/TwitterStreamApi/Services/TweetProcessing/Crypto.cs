using System.Collections.Generic;

namespace TwitterStreamApi.Services.TweetProcessing
{
    public static class Crypto
    {
        public static readonly Dictionary<string, string> Coins = new()
        {
            // a small sampling...
            { "BTC", "Bitcoin" },
            { "ETH", "Ethereum" },
            { "XRP", "XRP" },
            { "BNB", "Binance Coin" },
            { "DOGE", "Dogecoin" },
            { "ADA", "Cardano" },
            { "USDT", "Tether" },
            { "DOT", "Polkadot" },
            { "XLM", "Stellar" },
            { "BCH", "Bitcoin Cash" },
            { "LTC", "Litecoin" },
            { "VET", "Vechain" },
            { "ETC", "Ethereum Classic" },
            { "TRX", "TRON" },
            { "DASH", "Dash" },
        };
    }
}