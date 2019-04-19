namespace EliteSpawnTools
{
    using System;
    using System.Timers;
    using Sandbox.ModAPI;
    using VRage.Game;
    using VRage.Game.Components;

    [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
    public class Main : MySessionComponentBase
    {
        private Timer timer;
        private SpawnToolsReplacer spawnToolsReplacer;
        private MessageReceiver messageReceiver;
        private Action actionAfterCharacterFound;

        public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
        {
            base.Init(sessionComponent);
            this.spawnToolsReplacer = new SpawnToolsReplacer();
            this.timer = new Timer();
            this.timer.Elapsed += this.TimerElapsedForInit;
            this.timer.Interval = 1000;
            this.timer.Enabled = true;
        }

        protected override void UnloadData()
        {
            this.messageReceiver?.StopListening();
            base.UnloadData();
        }

        private void TimerElapsedForInit(object sender, ElapsedEventArgs e)
        {
            if (MyAPIGateway.Utilities == null || MyAPIGateway.Multiplayer == null || MyAPIGateway.Session == null)
            {
                return;
            }

            if (MyAPIGateway.Utilities.IsDedicated)
            {
                this.timer.Stop();
                this.timer.Elapsed -= this.TimerElapsedForInit;
                this.StartListeningForMessagesFromClients();
                return;
            }

            this.timer.Stop();
            this.timer.Elapsed -= this.TimerElapsedForInit;

            if (MyAPIGateway.Multiplayer.IsServer)
            {
                this.actionAfterCharacterFound = this.ReplaceLocalInventory;
            }
            else
            {
                this.actionAfterCharacterFound = this.SendMessage;
            }

            this.WaitForCharacter();
        }

        private void StartListeningForMessagesFromClients()
        {
            this.messageReceiver = new MessageReceiver(MyAPIGateway.Multiplayer, MyAPIGateway.Utilities);
            this.messageReceiver.StartListening();
        }

        private void WaitForCharacter()
        {
            this.timer.Elapsed += this.TimerElapsedForWaitForCharacter;
            this.timer.Start();
        }

        private void TimerElapsedForWaitForCharacter(object sender, ElapsedEventArgs e)
        {
            if (MyAPIGateway.Session.Player == null || MyAPIGateway.Session.Player.Character == null)
            {
                return;
            }

            this.timer.Stop();
            this.timer.Elapsed -= this.TimerElapsedForWaitForCharacter;
            this.actionAfterCharacterFound.Invoke();
        }

        private void ReplaceLocalInventory()
        {
            this.spawnToolsReplacer.Replace(MyAPIGateway.Session.Player.Character);
        }

        private void SendMessage()
        {
            var messageSender = new MessageSender(MyAPIGateway.Multiplayer, MyAPIGateway.Utilities);
            messageSender.SendMessage(MyAPIGateway.Session.Player);
        }
    }
}