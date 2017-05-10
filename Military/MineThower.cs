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

        public void Shoot(ref ObservableCollection<Target> Targets, double currentTime, double commonTime)
        {
            while (currentTime < commonTime)
            {
                Thread.Sleep(Random.Next(100,450));
                int TargetIndex = Random.Next(Targets.Count);
                int damage = Random.Next(35, 45); 
                Targets[TargetIndex].HealthPoints -= damage;
                if ((Targets[TargetIndex].GetType() == typeof(Target)))
                {
                    CountMiss++;
                }
                else
                {
                    //TotalDamage += damage;
                    CountHit++;
                }
            }
        }
    }
}
