﻿using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using TipBot;
using TwitchLib.Api;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;

namespace Blockcore.TipBot.Logic
{
    public class TwitchBot
    {
        private readonly TipBotSettings settings;

        private readonly TwitchAPI api;

        private readonly TwitchClient client;

        public TwitchBot(IOptionsMonitor<TipBotSettings> options)
        {
            this.settings = options.CurrentValue;

            api = new TwitchAPI();
            api.Settings.ClientId = settings.Twitch.ClientId;
            api.Settings.AccessToken = settings.Twitch.AccessToken; // App Secret is not an Accesstoken

            ConnectionCredentials credentials = new ConnectionCredentials(settings.Twitch.Username, settings.Twitch.OAuth);
            
            var clientOptions = new ClientOptions
            {
                MessagesAllowedInPeriod = 750,
                ThrottlingPeriod = TimeSpan.FromSeconds(30)
            };

            WebSocketClient customClient = new WebSocketClient(clientOptions);

            client = new TwitchClient(customClient);

            // TODO: Spawn multiple instances pr. channel definition in array. For now only grab the first one.
            var channel = settings.Twitch.Channels[0];

            client.Initialize(credentials, channel);

            client.OnLog += Client_OnLog;
            client.OnJoinedChannel += Client_OnJoinedChannel;
            client.OnMessageReceived += Client_OnMessageReceived;
            client.OnWhisperReceived += Client_OnWhisperReceived;
            client.OnNewSubscriber += Client_OnNewSubscriber;
            client.OnConnected += Client_OnConnected;

            client.Connect();
        }


        private void Client_OnLog(object sender, OnLogArgs e)
        {
            Console.WriteLine($"{e.DateTime.ToString()}: {e.BotUsername} - {e.Data}");
        }

        private void Client_OnConnected(object sender, OnConnectedArgs e)
        {
            Console.WriteLine($"Connected to {e.AutoJoinChannel}");
        }

        private void Client_OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            Console.WriteLine("Hey guys! I am a bot connected via TwitchLib!");
            client.SendMessage(e.Channel, "Hi, tiptippy the tipping bot has arrived. I'll help you share the love. Try out <@tiptippy balance>.");
        }

        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            if (e.ChatMessage.Message == "@tiptippy balance")
            {
                client.SendMessage(e.ChatMessage.Channel, $"{e.ChatMessage.Username}, you have 1500.00000000 CITY!");
            }

            if (e.ChatMessage.Message.StartsWith("@tiptippy tip "))
            {
                string[] values = e.ChatMessage.Message.Split(' ');

                if (values.Length != 4)
                {
                    client.SendMessage(e.ChatMessage.Channel, "That's not how you send tips... check out <@tiptippy  help> command.");
                }

                client.SendMessage(e.ChatMessage.Channel, $"{e.ChatMessage.Username} tipped {values[2]} {values[3]} CITY");
            }

            // if (e.ChatMessage.Message.Contains("badword"))
            //       client.TimeoutUser(e.ChatMessage.Channel, e.ChatMessage.Username, TimeSpan.FromMinutes(30), "Bad word! 30 minute timeout!");
        }

        private void Client_OnWhisperReceived(object sender, OnWhisperReceivedArgs e)
        {
            if (e.WhisperMessage.Message == "balance")
            {
                client.SendWhisper(e.WhisperMessage.Username, $"{e.WhisperMessage.Username}, you have 1500.00000000 CITY!");
            }

            if (e.WhisperMessage.Username == "my_friend")
                client.SendWhisper(e.WhisperMessage.Username, "Hey! Whispers are so cool!!");
        }

        private void Client_OnNewSubscriber(object sender, OnNewSubscriberArgs e)
        {
            if (e.Subscriber.SubscriptionPlan == SubscriptionPlan.Prime)
                client.SendMessage(e.Channel, $"Welcome {e.Subscriber.DisplayName} to the substers! You just earned 500 points! So kind of you to use your Twitch Prime on this channel!");
            else
                client.SendMessage(e.Channel, $"Welcome {e.Subscriber.DisplayName} to the substers! You just earned 500 points!");
        }
    }
}
