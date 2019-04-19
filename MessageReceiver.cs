namespace EliteSpawnTools
{
    using System.Collections.Generic;
    using System.Linq;
    using Sandbox.ModAPI;
    using VRage.Game.ModAPI;

    public class MessageReceiver
    {
        private const ushort ConnectionId = 43169;
        private readonly IMyMultiplayer multiplayer;
        private readonly IMyUtilities utilities;
        private readonly MessageLogger messageLogger;
        private readonly SpawnToolsReplacer spawnToolsReplacer;

        public MessageReceiver(IMyMultiplayer multiplayer, IMyUtilities utilities)
        {
            this.multiplayer = multiplayer;
            this.utilities = utilities;
            this.messageLogger = new MessageLogger();
            this.spawnToolsReplacer = new SpawnToolsReplacer();
        }

        public void StartListening()
        {
            this.multiplayer.RegisterMessageHandler(ConnectionId, this.MessageReceived);
            this.messageLogger.LogMessage("MessageReceiver started listening");
        }

        public void StopListening()
        {
            this.multiplayer.UnregisterMessageHandler(ConnectionId, this.MessageReceived);
            this.messageLogger.LogMessage("MessageReceiver stopped listening");
        }

        private void MessageReceived(byte[] bytes)
        {
            this.messageLogger.LogMessage("Received a message.");
            var xml = System.Text.Encoding.Unicode.GetString(bytes);
            var message = this.utilities.SerializeFromXML<ReplaceInventoryMessage>(xml);
            var playerList = new List<IMyPlayer>();
            MyAPIGateway.Players.GetPlayers(playerList, p => p != null && p.SteamUserId == message.SteamId);
            var player = playerList.FirstOrDefault();
            if (player == null)
            {
                this.messageLogger.LogMessage($"Received ReplaceInventoryMessage for player {message.SteamId} but this player could not be found in the collection");
                return;
            }

            if (player.Character == null)
            {
                this.messageLogger.LogMessage($"Found Player for ReplaceInventoryMessage but the player does not have a character");
                return;
            }

            this.spawnToolsReplacer.Replace(player.Character);
            this.messageLogger.LogMessage("Replaced Spawntools after receiving message.");
        }
    }
}
