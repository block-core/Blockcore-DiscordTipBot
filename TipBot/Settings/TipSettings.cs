﻿using NBitcoin;
using TipBot.Helpers;

namespace TipBot
{
    public class TipBotDiscordSettings {
        public bool Enabled { get; set; }

        public string Token { get; set; }

        /// <summary>Nickname of a user that will receive a message in case of a fatal error.</summary>
        public string SupportUsername { get; set; }

        public string SupportDiscriminator { get; set; }

        public ulong SupportUserId { get; set; } = 0;
    }

    public class TipBotTwitchSettings
    {
        public bool Enabled { get; set; }

        public string ClientId { get; set; }

        public string AccessToken { get; set; }

        public string Username { get; set; }

        public string OAuth { get; set; }

        public string[] Channels { get; set; }
    }

    public class TipBotTwitterSettings {
        public bool Enabled { get; set; }
    }


    public class TipBotRedditSettings { 
        public bool Enabled { get; set; }
    }

    public class TipBotSettings
    {
        public TipBotDiscordSettings Discord { get; set; }

        public TipBotTwitchSettings Twitch { get; set; }

        public TipBotTwitterSettings Twitter { get; set; }

        public TipBotRedditSettings Reddit { get; set; }

        public string ApiUrl { get; set; } = "http://127.0.0.1:48334/";

        /// <summary>
        /// The recovery phrase (mnemonic) used to create the initial wallet. This is only needed on first startup.
        /// </summary>
        public string WalletRecoveryPhrase { get; set; }

        /// <summary>
        /// Name of the tipbot wallet, default value is "tipbot".
        /// </summary>
        public string WalletName { get; set; } = "tipbot";

        public string WalletPassword { get; set; }

        public bool UseSegwit { get; set; } = true;

        public bool TipsEnabled { get; set; } = true;

        public string Ticker { get; set; } = "BTC";

        public decimal NetworkFee { get; set; } = 0.01m;

        public uint PregeneratedAddressesCount { get; set; } = 8000;

        public int MinConfirmationsForDeposit { get; set; } = 16;

        public decimal MinWithdrawAmount { get; set; } = 0.1m;

        public decimal MinQuizAmount { get; set; } = 0.1m;

        public decimal MinMakeItRainTipAmount { get; set; } = 0.1m;

        public decimal MinTipAmount { get; set; } = 0.001m;

        public int MaxChartUsersCount { get; set; } = 3;

        public int MaxDaysChartCount { get; set; } = 30;

        /// <summary>Specifies if bit should attempt to update the database on startup.</summary>
        public bool EnableMigrations { get; set; } = true;

        /// <summary>DB connection string. Local db will be used if it's null.</summary>
        public string ConnectionString { get; set; } = @"Data Source=127.0.0.1;Initial Catalog=TipBot;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public string BotOptionalPrefix { get; set; } = "tipbot";

        /// <summary>Default time in which self destructed messages are deleted.</summary>
        public int SelfDestructedMessagesDelaySeconds { get; set; } = 20;
    }
}
