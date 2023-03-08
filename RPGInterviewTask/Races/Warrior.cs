using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGInterviewTask.Races
{
    public class Warrior : Race
    {
        public override int Strength { get; set; } = 3;
        public override int Agility { get; set; } = 3;
        public override int Intelligence { get; set; } = 0;
        public override int Range { get;} = 1;

        public override char Symbol => '@';
        public override int PositionRow { get; internal set; } = 1;
        public override int PositionCol { get; internal set; } = 1;
    }
}
