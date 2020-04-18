using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E7_Gear_Optimizer
{
    public class Item
    {
        public string ID { get; }
        private ItemType type;
        private Set set;
        private Grade grade;
        private int iLvl;
        private int enhance;
        private Stat main;
        private Stat[] subStats;
        public SStats AllStats { get; set; } = new SStats();
        private float wss;
        private float flatWss;
        public bool Locked { get; set; }
        public Hero Equipped { get; set; }

        public Item(string id, ItemType type, Set set, Grade grade, int iLvl, int enhance, Stat main, Stat[] subStats, Hero equipped, bool locked)
        {
            ID = id;
            this.type = type;
            this.set = set;
            this.grade = grade;
            this.iLvl = iLvl;
            this.enhance = enhance;
            this.main = main;
            this.subStats = subStats;
            Equipped = equipped;
            Locked = locked;

            AllStats.SetStat(main);
            AllStats.SetStats(subStats);

            calcWSS();
        }

        public Item() { }


        public ItemType Type { get => type; set => type = value; }
        public Set Set { get => set; set => set = value; }
        public Grade Grade { get => grade; set => grade = value; }
        public int ILvl
        {
            get => iLvl;
            set
            {
                if (value > 0)
                {
                    iLvl = value;
                }
            }
        }
        public int Enhance
        {
            get => enhance;
            set
            {
                if (value > -1 && value < 16)
                {
                    enhance = value;
                }
            }
        }
        public Stat Main
        {
            get => main;
            set
            {
                main = value;
                AllStats.SetStat(main);
            }
        }
        public Stat[] SubStats
        {
            get => subStats;
            set
            {
                subStats = value;
                AllStats.SetStats(subStats);
            }
        }

        public float WSS { get => wss; }
        public float FlatWSS { get => flatWss; }

        private const float wssMultiplier = 2f / 3f;

        // average HP of all soul weavers, knights, warriors at 6* awakened
        // hardcoded for personal use
        private const float flatAverageHP = 5742.22f;
        private const float flatAverageDef = 637.83f;

        // average ATK of all thiefs, rangers, mages, warriors at 6* awakened
        // hardcoded for personal use
        private const float flatAverageAtk = 1088.45f;

        public void calcWSS()
        {
            wss = 0;
            flatWss = 0;
            foreach (Stat s in subStats)
            {
                switch (s.Name)
                {
                    case Stats.ATKPercent:
                    case Stats.DEFPercent:
                    case Stats.HPPercent:
                    case Stats.EFF:
                    case Stats.RES:
                        wss += 100 * s.Value / 48;
                        break;
                    case Stats.Crit:
                        wss += 100 * s.Value / 30;
                        break;
                    case Stats.CritDmg:
                        wss += 100 * s.Value / 42;
                        break;
                    case Stats.SPD:
                        wss += s.Value / 24;
                        break;
                    default:
                        break;
                }

                float flatPercentage = 0.0f;
                switch (s.Name)
                {
                    case Stats.ATK:
                        flatPercentage = s.Value / flatAverageAtk;
                        break;
                    case Stats.DEF:
                        flatPercentage = s.Value / flatAverageDef;
                        break;
                    case Stats.HP:
                        flatPercentage = s.Value / flatAverageHP;
                        break;
                    default:
                        break;
                }
                flatWss += 100 * flatPercentage / 48;
                wss += flatWss;
            }
            wss *= wssMultiplier;
        }
    }
}
