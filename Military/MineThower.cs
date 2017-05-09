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
        Random random = new Random();

        public int CountHit { get; set; }
        public int TotalDamage { get; set; }
        public int CountMiss{ get; set; }

        public MineThower()
        {
            CountHit = 0;
            CountMiss = 0;
            TotalDamage = 0;
        } 

        public void Shoot(ref ObservableCollection<Target> targets)
        {
            Thread.Sleep(100);
            int TargetIndex = random.Next(targets.Count);
            int damage = random.Next(35, 45);
            targets[TargetIndex].HealthPoints -= damage;
            if (targets[TargetIndex].GetType() != typeof(Target))
            {
                CountMiss++; 
            }
            else
            {
                CountHit++;
                TotalDamage += damage;
            }
        }
    }
}
