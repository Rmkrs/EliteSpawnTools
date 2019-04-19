namespace EliteSpawnTools
{
    using Sandbox.Game;
    using VRage.Game;
    using VRage.Game.ModAPI;

    public class SpawnToolsReplacer
    {
        private readonly InventoryItemReplacer inventoryItemReplacer;
        private readonly MessageLogger messageLogger;

        public SpawnToolsReplacer()
        {
            this.inventoryItemReplacer = new InventoryItemReplacer();
            this.messageLogger = new MessageLogger();
        }

        public void Replace(IMyCharacter character)
        {
            var characterInventory = character.GetInventory();
            if (characterInventory == null)
            {
                this.messageLogger.LogMessage("Could not get character inventory");
                return;
            }

            MyInventory inventory = characterInventory as MyInventory;
            if (inventory == null)
            {
                this.messageLogger.LogMessage("Could not transform character inventory to MyInventory");
                return;
            }

            this.inventoryItemReplacer.ReplaceInventoryItem<MyObjectBuilder_PhysicalGunObject>(inventory, "AngleGrinderItem", "AngleGrinder4Item");
            this.inventoryItemReplacer.ReplaceInventoryItem<MyObjectBuilder_PhysicalGunObject>(inventory, "HandDrillItem", "HandDrill4Item");
            this.inventoryItemReplacer.ReplaceInventoryItem<MyObjectBuilder_PhysicalGunObject>(inventory, "WelderItem", "Welder4Item");
            this.inventoryItemReplacer.ReplaceInventoryItem<MyObjectBuilder_PhysicalGunObject>(inventory, "AutomaticRifleItem", "UltimateAutomaticRifleItem");
        }
    }
}
