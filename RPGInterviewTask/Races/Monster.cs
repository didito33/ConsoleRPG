using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGInterviewTask.Races
{
    public class Monster : Race
    {
        private static Random rdn = new Random();
        public override int Strength { get; set; } = rdn.Next(1, 3);
        public override int Agility { get; set; } = rdn.Next(1, 3);
        public override int Intelligence { get; set; } = rdn.Next(1, 3);
        public override int Range { get;} = 1;
        public override char Symbol => 'M';
        public override int PositionRow { get; internal set; } = rdn.Next(0, 10);
        public override int PositionCol { get; internal set; } = rdn.Next(0, 10);
    }
}
