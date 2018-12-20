using System;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Exceptions;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;
using UnityEngine;

namespace TwitchLib.Unity
{
    public class Client : TwitchClient, ITwitchClient
    {
        public new bool OverrideBeingHostedCheck { get; set; }
        
        #region Events
        /// <summary>
        /// Fires whenever a log write happens.
        /// </summary>
        public new event EventHandler<OnLogArgs> OnLog;

        /// <summary>
        /// Fires when client connects to Twitch.
        /// </summary>
        public new event EventHandler<OnConnectedArgs> OnConnected;

        /// <summary>
        /// Fires when client joins a channel.
        /// </summary>
        public new event EventHandler<OnJoinedChannelArgs> OnJoinedChannel;

        /// <summary>
        /// Fires on logging in with incorrect details, returns ErrorLoggingInException.
        /// </summary>
        public new event EventHandler<OnIncorrectLoginArgs> OnIncorrectLogin;

        /// <summary>
        /// Fires when connecting and channel state is changed, returns ChannelState.
        /// </summary>
        public new event EventHandler<OnChannelStateChangedArgs> OnChannelStateChanged;

        /// <summary>
        /// Fires when a user state is received, returns UserState.
        /// </summary>
        public new event EventHandler<OnUserStateChangedArgs> OnUserStateChanged;

        /// <summary>
        /// Fires when a new chat message arrives, returns ChatMessage.
        /// </summary>
        public new event EventHandler<OnMessageReceivedArgs> OnMessageReceived;

        /// <summary>
        /// Fires when a new whisper arrives, returns WhisperMessage.
        /// </summary>
        public new event EventHandler<OnWhisperReceivedArgs> OnWhisperReceived;

        /// <summary>
        /// Fires when a chat message is sent, returns username, channel and message.
        /// </summary>
        public new event EventHandler<OnMessageSentArgs> OnMessageSent;

        /// <summary>
        /// Fires when a whisper message is sent, returns username and message.
        /// </summary>
        public new event EventHandler<OnWhisperSentArgs> OnWhisperSent;

        /// <summary>
        /// Fires when command (uses custom chat command identifier) is received, returns channel, command, ChatMessage, arguments as string, arguments as list.
        /// </summary>
        public new event EventHandler<OnChatCommandReceivedArgs> OnChatCommandReceived;

        /// <summary>
        /// Fires when command (uses custom whisper command identifier) is received, returns command, Whispermessage.
        /// </summary>
        public new event EventHandler<OnWhisperCommandReceivedArgs> OnWhisperCommandReceived;

        /// <summary>
        /// Fires when a new viewer/chatter joined the channel's chat room, returns username and channel.
        /// </summary>
        public new event EventHandler<OnUserJoinedArgs> OnUserJoined;

        /// <summary>
        /// Fires when a moderator joined the channel's chat room, returns username and channel.
        /// </summary>
        public new event EventHandler<OnModeratorJoinedArgs> OnModeratorJoined;

        /// <summary>
        /// Fires when a moderator joins the channel's chat room, returns username and channel.
        /// </summary>
        public new event EventHandler<OnModeratorLeftArgs> OnModeratorLeft;

        /// <summary>
        /// Fires when new subscriber is announced in chat, returns Subscriber.
        /// </summary>
        public new event EventHandler<OnNewSubscriberArgs> OnNewSubscriber;

        /// <summary>
        /// Fires when current subscriber renews subscription, returns ReSubscriber.
        /// </summary>
        public new event EventHandler<OnReSubscriberArgs> OnReSubscriber;

        /// <summary>
        /// Fires when a hosted streamer goes offline and hosting is killed.
        /// </summary>
        public new event EventHandler OnHostLeft;

        /// <summary>
        /// Fires when Twitch notifies client of existing users in chat.
        /// </summary>
        public new event EventHandler<OnExistingUsersDetectedArgs> OnExistingUsersDetected;

        /// <summary>
        /// Fires when a PART message is received from Twitch regarding a particular viewer
        /// </summary>
        public new event EventHandler<OnUserLeftArgs> OnUserLeft;

        /// <summary>
        /// Fires when the joined channel begins hosting another channel.
        /// </summary>
        public new event EventHandler<OnHostingStartedArgs> OnHostingStarted;

        /// <summary>
        /// Fires when the joined channel quits hosting another channel.
        /// </summary>
        public new event EventHandler<OnHostingStoppedArgs> OnHostingStopped;

        /// <summary>
        /// Fires when bot has disconnected.
        /// </summary>
        public new event EventHandler<Communication.Events.OnDisconnectedEventArgs> OnDisconnected;

        /// <summary>
        /// Forces when bot suffers conneciton error.
        /// </summary>
        public new event EventHandler<OnConnectionErrorArgs> OnConnectionError;

        /// <summary>
        /// Fires when a channel's chat is cleared.
        /// </summary>
        public new event EventHandler<OnChatClearedArgs> OnChatCleared;

        /// <summary>
        /// Fires when a viewer gets timedout by any moderator.
        /// </summary>
        public new event EventHandler<OnUserTimedoutArgs> OnUserTimedout;

        /// <summary>
        /// Fires when client successfully leaves a channel.
        /// </summary>
        public new event EventHandler<OnLeftChannelArgs> OnLeftChannel;

        /// <summary>
        /// Fires when a viewer gets banned by any moderator.
        /// </summary>
        public new event EventHandler<OnUserBannedArgs> OnUserBanned;

        /// <summary>
        /// Fires when a list of moderators is received.
        /// </summary>
        public new event EventHandler<OnModeratorsReceivedArgs> OnModeratorsReceived;

        /// <summary>
        /// Fires when confirmation of a chat color change request was received.
        /// </summary>
        public new event EventHandler<OnChatColorChangedArgs> OnChatColorChanged;

        /// <summary>
        /// Fires when data is either received or sent.
        /// </summary>
        public new event EventHandler<OnSendReceiveDataArgs> OnSendReceiveData;

        /// <summary>
        /// Fires when client receives notice that a joined channel is hosting another channel.
        /// </summary>
        public new event EventHandler<OnNowHostingArgs> OnNowHosting;

        /// <summary>
        /// Fires when the library detects another channel has started hosting the broadcaster's stream. MUST BE CONNECTED AS BROADCASTER.
        /// </summary>
        public new event EventHandler<OnBeingHostedArgs> OnBeingHosted;

        /// <summary>
        /// Fires when a raid notification is detected in chat
        /// </summary>
        public new event EventHandler<OnRaidNotificationArgs> OnRaidNotification;

        /// <summary>
        /// Fires when a subscription is gifted and announced in chat
        /// </summary>
        public new event EventHandler<OnGiftedSubscriptionArgs> OnGiftedSubscription;

        /// <summary>Fires when TwitchClient attempts to host a channel it is in.</summary>
        public new event EventHandler OnSelfRaidError;

        /// <summary>Fires when TwitchClient receives generic no permission error from Twitch.</summary>
        public new event EventHandler OnNoPermissionError;

        /// <summary>Fires when newly raided channel is mature audience only.</summary>
        public new event EventHandler OnRaidedChannelIsMatureAudience;

        /// <summary>Fires when a ritual for a new chatter is received.</summary>
        public new event EventHandler<OnRitualNewChatterArgs> OnRitualNewChatter;

        /// <summary>Fires when the client was unable to join a channel.</summary>
        public new event EventHandler<OnFailureToReceiveJoinConfirmationArgs> OnFailureToReceiveJoinConfirmation;

        /// <summary>Fires when data is received from Twitch that is not able to be parsed.</summary>
        public new event EventHandler<OnUnaccountedForArgs> OnUnaccountedFor;
        #endregion

        public Client() : base(null)
        {
            ThreadDispatcher.EnsureCreated();

            base.OverrideBeingHostedCheck = true;

            base.OnLog += (sender, e) => { ThreadDispatcher.Enqueue(() => OnLog?.Invoke(sender, e)); };
            base.OnConnected += ((sender, e) => { ThreadDispatcher.Enqueue(() => OnConnected?.Invoke(sender, e)); });

            base.OnJoinedChannel += ((sender, e) => {

                ThreadDispatcher.Enqueue(() => OnJoinedChannel?.Invoke(sender, e));

                if (OnBeingHosted == null) return;
                if (e.Channel.ToLower() != TwitchUsername && !OverrideBeingHostedCheck)
                    ThreadDispatcher.Enqueue(() => throw new BadListenException("BeingHosted", "You cannot listen to OnBeingHosted unless you are connected to the broadcaster's channel as the broadcaster. You may override this by setting the TwitchClient property OverrideBeingHostedCheck to true."));
            });

            base.OnIncorrectLogin += ((sender, e) => { ThreadDispatcher.Enqueue(() => OnIncorrectLogin?.Invoke(sender, e)); });
            base.OnChannelStateChanged += ((sender, e) => { ThreadDispatcher.Enqueue(() => OnChannelStateChanged?.Invoke(sender, e)); });
            base.OnUserStateChanged += ((sender, e) => { ThreadDispatcher.Enqueue(() => OnUserStateChanged?.Invoke(sender, e)); });
            base.OnMessageReceived += ((sender, e) => { ThreadDispatcher.Enqueue(() => OnMessageReceived?.Invoke(sender, e)); });
            base.OnWhisperReceived += ((sender, e) => { ThreadDispatcher.Enqueue(() => OnWhisperReceived?.Invoke(sender, e)); });
            base.OnMessageSent += ((sender, e) => { ThreadDispatcher.Enqueue(() => OnMessageSent?.Invoke(sender, e)); });
            base.OnWhisperSent += ((sender, e) => { ThreadDispatcher.Enqueue(() => OnWhisperSent?.Invoke(sender, e)); });
            base.OnChatCommandReceived += ((sender, e) => { ThreadDispatcher.Enqueue(() => OnChatCommandReceived?.Invoke(sender, e)); });
            base.OnWhisperCommandReceived += ((sender, e) => { ThreadDispatcher.Enqueue(() => OnWhisperCommandReceived?.Invoke(sender, e)); });
            base.OnUserJoined += ((sender, e) => { ThreadDispatcher.Enqueue(() => OnUserJoined?.Invoke(sender, e)); });
            base.OnModeratorJoined += ((sender, e) => { ThreadDispatcher.Enqueue(() => OnModeratorJoined?.Invoke(sender, e)); });
            base.OnModeratorLeft += ((sender, e) => { ThreadDispatcher.Enqueue(() => OnModeratorLeft?.Invoke(sender, e)); });
            base.OnNewSubscriber += ((sender, e) => { ThreadDispatcher.Enqueue(() => OnNewSubscriber?.Invoke(sender, e)); });
            base.OnReSubscriber += ((sender, e) => { ThreadDispatcher.Enqueue(() => OnReSubscriber?.Invoke(sender, e)); });
            base.OnHostLeft += ((sender, e) => { ThreadDispatcher.Enqueue(() => OnHostLeft?.Invoke(sender, e)); });
            base.OnExistingUsersDetected += ((sender, e) => { ThreadDispatcher.Enqueue(() => OnExistingUsersDetected?.Invoke(sender, e)); });
            base.OnUserLeft += ((sender, e) => { ThreadDispatcher.Enqueue(() => OnUserLeft?.Invoke(sender, e)); });
            base.OnHostingStarted += ((sender, e) => { ThreadDispatcher.Enqueue(() => OnHostingStarted?.Invoke(sender, e)); });
            base.OnHostingStopped += ((sender, e) => { ThreadDispatcher.Enqueue(() => OnHostingStopped?.Invoke(sender, e)); });
            base.OnDisconnected += ((sender, e) => { ThreadDispatcher.Enqueue(() => OnDisconnected?.Invoke(sender, e)); });
            base.OnConnectionError += ((sender, e) => { ThreadDispatcher.Enqueue(() => OnConnectionError?.Invoke(sender, e)); });
            base.OnChatCleared += ((sender, e) => { ThreadDispatcher.Enqueue(() => OnChatCleared?.Invoke(sender, e)); });
            base.OnUserTimedout += ((sender, e) => { ThreadDispatcher.Enqueue(() => OnUserTimedout?.Invoke(sender, e)); });
            base.OnLeftChannel += ((sender, e) => { ThreadDispatcher.Enqueue(() => OnLeftChannel?.Invoke(sender, e)); });
            base.OnUserBanned += ((sender, e) => { ThreadDispatcher.Enqueue(() => OnUserBanned?.Invoke(sender, e)); });
            base.OnModeratorsReceived += ((sender, e) => { ThreadDispatcher.Enqueue(() => OnModeratorsReceived?.Invoke(sender, e)); });
            base.OnChatColorChanged += ((sender, e) => { ThreadDispatcher.Enqueue(() => OnChatColorChanged?.Invoke(sender, e)); });
            base.OnSendReceiveData += ((sender, e) => { ThreadDispatcher.Enqueue(() => OnSendReceiveData?.Invoke(sender, e)); });
            base.OnNowHosting += ((sender, e) => { ThreadDispatcher.Enqueue(() => OnNowHosting?.Invoke(sender, e)); });
            base.OnBeingHosted += ((sender, e) => { ThreadDispatcher.Enqueue(() => OnBeingHosted?.Invoke(sender, e)); });
            base.OnRaidNotification += ((sender, e) => { ThreadDispatcher.Enqueue(() => OnRaidNotification?.Invoke(sender, e)); });
            base.OnGiftedSubscription += ((sender, e) => { ThreadDispatcher.Enqueue(() => OnGiftedSubscription?.Invoke(sender, e)); });
            base.OnRaidedChannelIsMatureAudience += ((sender, e) => { ThreadDispatcher.Enqueue(() => OnRaidedChannelIsMatureAudience?.Invoke(sender, e)); });
            base.OnRitualNewChatter += ((sender, e) => { ThreadDispatcher.Enqueue(() => OnRitualNewChatter?.Invoke(sender, e)); });
            base.OnFailureToReceiveJoinConfirmation += ((sender, e) => { ThreadDispatcher.Enqueue(() => OnFailureToReceiveJoinConfirmation?.Invoke(sender, e)); });
            base.OnUnaccountedFor += ((sender, e) => { ThreadDispatcher.Enqueue(() => OnUnaccountedFor?.Invoke(sender, e)); });
            base.OnSelfRaidError += ((sender, e) => { ThreadDispatcher.Enqueue(() => OnSelfRaidError?.Invoke(sender, e)); });
            base.OnNoPermissionError += ((sender, e) => { ThreadDispatcher.Enqueue(() => OnNoPermissionError?.Invoke(sender, e)); });
        }

        private new void HandleNotInitialized()
        {
            ThreadDispatcher.Enqueue(() => throw new ClientNotInitializedException("The twitch client has not been initialized and cannot be used. Please call Initialize();"));
        }
    } 
}

