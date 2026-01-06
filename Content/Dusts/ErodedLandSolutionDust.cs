using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace WBHMODE.Content.Dusts
{
    public class ErodedLandSolutionDust : ModDust
    {
        public override void SetStaticDefaults()
        {
            UpdateType = DustID.PureSpray;
        }
    }
}
