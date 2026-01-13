using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using WBHMODE.Common.Players;
using WBHMODE.Content.Projectiles;

namespace WBHMODE.Content.Buffs
{
    public class VirtualSoldierBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            // 不显示buff时间
            Main.buffNoTimeDisplay[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            ModGlobalPlayer modPlayer = player.GetModPlayer<ModGlobalPlayer>();
            // 如果当前有属于玩家的僚机的弹幕
            if (player.ownedProjectileCounts[ModContent.ProjectileType<VirtualSoldier>()] > 0)
            {
                modPlayer.VirtualArmyBuff = true;
            }
            // 如果玩家取消了这个召唤物就让buff消失
            if (!modPlayer.VirtualArmyBuff)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
            else
            {
                // 无限buff时间
                player.buffTime[buffIndex] = 9999;
            }
        }
    }
}
