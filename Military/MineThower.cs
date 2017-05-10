using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Military
{
    public class MineThower
    {
        public int CountHit { get; set; }
        public int TotalDamage { get; set; }
        public int CountMiss{ get; set; }
        public Random Random { get; set; }

        public MineThower( Random random)
        {
            CountHit = 0;
            CountMiss = 0;
            TotalDamage = 0;
            Random = random;
        } 

        public void Shoot(ref ObservableCollection<Target> Targets, int currentTime, int commonTime)
        {
            while (currentTime < commonTime)
            {
                Thread.Sleep(1000);
                int x = Random.Next(60, 790);
                int y = Random.Next(45, 577);
                int damage = Random.Next(35, 45);
                for (int i = 0; i < Targets.Count; i++)
                {
                    if (Object.ReferenceEquals(null, Targets[i]) || Targets[i] == null || Targets[i].Equals(null))
                    {
                        Targets.Remove(Targets[i]);
                        break;
                    }
                    if (Targets[i].X >= x - 30 && Targets[i].X <= x + 30 &&
                        Targets[i].Y >= y - 30 && Targets[i].Y <= y + 30 && Targets[i]!=null)
                    {
                        Targets[i].HealthPoints -= damage;
                        CountHit++;
                        TotalDamage += damage;
                        break;
                    }
                    else
                    {
                        CountMiss++;
                        Targets.Add(new EmptyTarget(x, y));
                    }
                }
            }
        }
    }
}
