namespace EliteSpawnTools
{
    using System;
    using Sandbox.Game;
    using VRage.Game;
    using VRage.ObjectBuilders;

    public class InventoryItemReplacer
    {
        private readonly MessageLogger messageLogger;

        public InventoryItemReplacer()
        {
            this.messageLogger = new MessageLogger();
        }

        public void ReplaceInventoryItem<T>(MyInventory inventory, string oldSubTypeName, string newSubTypeName)
            where T : MyObjectBuilder_Base, new()
        {
            this.ReplaceInventoryItem<T, T>(inventory, oldSubTypeName, 1, newSubTypeName, 1);
        }

        public void ReplaceInventoryItem<TOld, TNew>(MyInventory inventory, string oldSubTypeName, int oldCount, string newSubTypeName, int newCount)
            where TOld : MyObjectBuilder_Base, new()
            where TNew : MyObjectBuilder_Base, new()
        {
            var oldBuilder = MyObjectBuilderSerializer.CreateNewObject<TOld>(oldSubTypeName);
            var existingItem = inventory.FindItem(oldBuilder.GetId());
            if (existingItem == null)
            {
                this.messageLogger.LogMessage($"Existing Item ({typeof(TOld)} - {oldSubTypeName}) not found in inventory so no replacement will be done");
                return;
            }

            var removed = inventory.Remove(existingItem, oldCount);
            if (!removed)
            {
                this.messageLogger.LogMessage($"Removing {oldCount} of {typeof(TOld)} - {oldSubTypeName} could not be done so no replacement will be done");
                return;
            }
            this.messageLogger.LogMessage($"Removing {oldCount} of {typeof(TOld)} - {oldSubTypeName} was done successfully");

            var newBuilder = MyObjectBuilderSerializer.CreateNewObject<TNew>(newSubTypeName);
            var added = inventory.AddItems(newCount, newBuilder);
            this.messageLogger.LogMessage($"Adding {newCount} of {typeof(TNew)} - {newSubTypeName} was{(!added ? " NOT" : String.Empty)} done successfully");
        }
    }
}
