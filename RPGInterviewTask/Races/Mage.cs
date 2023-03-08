using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace RPGInterviewTask.Races
{
    public class Mage : Race
    {
        public override int Strength { get; set; } = 2;
        public override int Agility { get; set; } = 1;
        public override int Intelligence { get; set; } = 3;
        public override int Range { get;} = 3;
        public override char Symbol => '*';
        public override int PositionRow { get; internal set; } = 1;
        public override int PositionCol { get; internal set; } = 1;
    }
}
