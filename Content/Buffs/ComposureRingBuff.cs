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
    public class ComposureRingBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            BuffID.Sets.GrantImmunityWith[Type].Add(ModContent.BuffType<ComposureRingBuff>());
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<ModGlobalPlayer>().composureRingBuff = true;
        }
    }
}
