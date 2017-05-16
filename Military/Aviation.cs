using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Military
{
    public class Aviation
    {
        public event DeleGateDraw DrawingAvia;
        private const int damage_degree = 50;
        public int Name { get; set; }
        public int CountDestroyed { get; set; } 
        public int CountShell { get; set; } 
        public int CountHit { get; set; } 
        Random Random { get; set; }
        int currentTime = 0;
        DispatcherTimer timer = new DispatcherTimer();

        public Aviation(Random random, int сode)
        {
            Name = сode;
            CountShell = 20;
            CountHit = 0;
            CountDestroyed = 0;
            Random = random;
        }

        public void Shoot(ref ObservableCollection<Target> Targets, double commonTime)
        {
            timer.Tick += new EventHandler(dispatcherTimerWork_Tick);
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Start();
            currentTime = 0;
            int TargetIndex = 0;
            while (currentTime <= commonTime)
            {
                if (StopThowersTime(commonTime))
                {
                    return;
                }
                else
                {
                    TargetIndex = Random.Next(Targets.Count);
                    while (Targets[TargetIndex].GetType() != typeof(Target))
                    {
                        TargetIndex = Random.Next(Targets.Count);
                    }
                    if (Targets[TargetIndex].HealthPoints > 25)
                    {
                        if (CountShell > 0)
                        {
                            Thread.Sleep(Random.Next(120, 170));
                            if (Targets[TargetIndex].HealthPoints > 25 && Targets[TargetIndex].HealthPoints <= 50)
                            {
                                CountDestroyed++;
                            }
                            Targets[TargetIndex].HealthPoints -= damage_degree;
                            CountShell--;
                            CountHit++;
                            DrawingAvia.Invoke(this);
                        }
                    }
                }
            }
        }

        private void dispatcherTimerWork_Tick(object sender, EventArgs e)
        {
            currentTime++;
        }

        private bool StopThowersTime(double commonTime)
        {
            if (currentTime == commonTime)
            {
                timer.Tick -= new EventHandler(dispatcherTimerWork_Tick);
                timer.Stop();
                return true;
            }
            else
            {
                return false;
            }
        } 

    }            
} 
