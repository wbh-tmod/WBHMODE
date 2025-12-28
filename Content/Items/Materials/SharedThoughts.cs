//#define DEBUG

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using WBHMODE.Content.Tiles;

namespace WBHMODE.Content.Items.Materials
{
    public class SharedThoughts: ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 3;
            ItemID.Sets.SortingPriorityMaterials[Type] = 69; // Hellstone
        }
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 9999;
            Item.value = Item.buyPrice(copper: 0); // 售价
            Item.rare = ItemRarityID.Blue;
            Item.useTurn = true;
            Item.autoReuse = false;
            Item.consumable = true;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
        }
        public override bool CanUseItem(Player player)
        {
            // 生成条件
            return true;
        }

        public override bool? UseItem(Player player)
        {
            SoundEngine.PlaySound(SoundID.Roar, player.Center);
            if (Main.netMode != NetmodeID.MultiplayerClient)
                NPC.SpawnOnPlayer(player.whoAmI, NPCID.BlueSlime);
            else
                NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, -1, -1, null, player.whoAmI, NPCID.BlueSlime);

            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ModContent.ItemType<Materials.Neuro>(), 15).
                AddIngredient(ModContent.ItemType<VoidPowder>(), 30).
                AddTile(TileID.DemonAltar).
                Register();
            CreateRecipe().
                AddIngredient(ModContent.ItemType<Materials.Neuro>(), 15).
                AddIngredient(ModContent.ItemType<VoidPowder>(), 30).
                AddTile(ModContent.TileType<RevivalAltar>()).
                Register();
            return;
        }
    }
}
