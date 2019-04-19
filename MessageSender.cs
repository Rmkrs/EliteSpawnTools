namespace EliteSpawnTools
{
    using VRage.Game.ModAPI;

    public class MessageSender
    {
        private const ushort ConnectionId = 43169;
        private readonly IMyMultiplayer multiplayer;
        private readonly IMyUtilities utilities;

        public MessageSender(IMyMultiplayer multiplayer, IMyUtilities utilities)
        {
            this.multiplayer = multiplayer;
            this.utilities = utilities;
        }

        public void SendMessage(IMyPlayer player)
        {
            var message = new ReplaceInventoryMessage { SteamId = player.SteamUserId };
            var xml = this.utilities.SerializeToXML(message);
            var bytes = System.Text.Encoding.Unicode.GetBytes(xml);
            this.multiplayer.SendMessageToServer(ConnectionId, bytes);
        }
    }
}
