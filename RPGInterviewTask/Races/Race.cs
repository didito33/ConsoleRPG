using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGInterviewTask.Races
{
    public abstract class Race
    {
        private int health;
        public Race()
        {
            Setup();
        }
        public int Health
        {
            get => health;
            set
            {
                if (value <= 0)
                {
                    health = 0;
                    IsAlive = false;
                }
                else
                {
                    health = value;
                }

            }
        }
        public int Mana { get; set; }
        public int Damage { get; private set; }
        public abstract int Strength { get; set; }
        public abstract int Agility { get; set; }
        public abstract int Intelligence { get; set; }
        public abstract int Range { get;}
        public bool IsAlive { get; set; } = true;
        public abstract char Symbol { get; }
        public abstract int PositionRow { get; internal set; } 
        public abstract int PositionCol { get; internal set; }
        public void Setup()
        {
            Health = Strength * 5;
            Mana = Intelligence * 3;
            Damage = Agility * 2;
        }
    }
}
