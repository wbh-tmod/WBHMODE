using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using WBHMODE.Common.Players;

namespace WBHMODE.Content.Buffs
{
    public class ComposureRingDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            //Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
            BuffID.Sets.GrantImmunityWith[Type].Add(ModContent.BuffType<ComposureRingDebuff>());
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<ModGlobalPlayer>().composureRingDebuff = true;
        }
    }
}
