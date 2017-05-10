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
        private const int damage_degree = 50;
        public int CountShell { get; set; } 
        public int CountHit { get; set; } 
        public int TotalDamage { get; set; }
        Random Random { get; set; }

        public Aviation( Random random)
        {
            CountShell = 20;
            CountHit = 0;
            TotalDamage = 0; 
            Random = random;
        }

        public void Shoot(ref ObservableCollection<Target> Targets, double currentTime, double commonTime)
        { 
            while (currentTime < commonTime)
            {
                Thread.Sleep(Random.Next(150,500));
                int TargetIndex = Random.Next(Targets.Count); 
                if (Targets[TargetIndex].HealthPoints > 25 && (Targets[TargetIndex].GetType() == typeof(Target)))
                {
                    CountShell--;
                    if (CountShell > 0)
                    {
                        Targets[TargetIndex].HealthPoints -= damage_degree;
                        CountHit++;
                        TotalDamage += damage_degree;
                    }
                }
            }            
        } 

    }
}
