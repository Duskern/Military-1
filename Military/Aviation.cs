using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Military
{
    public class Aviation
    {
        Random random = new Random();

        private const int damage_degree = 50;
        public int CountShell { get; set; } 
        public int CountHit { get; set; } 
        public int TotalDamage { get; set; }

        public Aviation()
        {
            CountShell = 20;
            CountHit = 0;
            TotalDamage = 0;
        } 

        public void Shoot(ref ObservableCollection<Target> targets)
        {
            Thread.Sleep(1000);
            int TargetIndex = random.Next(targets.Count);
            while(targets[TargetIndex] == null)
            {
                TargetIndex = random.Next(targets.Count);
            }
            if (targets[TargetIndex].HealthPoints > 25)
            {
                CountShell--;
                if (CountShell > 0)
                {  
                    targets[TargetIndex].HealthPoints -= damage_degree;
                    CountHit++;
                    TotalDamage += damage_degree;
                }
            }
        } 

    }
}
