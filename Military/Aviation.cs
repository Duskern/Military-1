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
        public event ItemEnabled Enabled;
        private const int damage_degree = 50;
        public string Name { get; set; }
        public int CountDestroyed { get; set; } 
        public int CountShell { get; set; } 
        public int CountHit { get; set; } 
        public int TotalDamage { get; set; }
        Random Random { get; set; }
        int currentTime = 0;
        DispatcherTimer timer = new DispatcherTimer();

        public Aviation(Random random, int сode)
        {
            Name = "Aircraft #" + сode.ToString();
            CountShell = 10;
            CountHit = 0;
            TotalDamage = 0;
            CountDestroyed = 0;
            Random = random;
        }

        public void Shoot(ref ObservableCollection<Target> Targets, double commonTime, int countThreadsAviations, bool message)
        {
            timer.Tick += new EventHandler(dispatcherTimerWork_Tick);
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Start();
            currentTime = 0;
            while (currentTime < commonTime - 2)
            {
                int TargetIndex = Random.Next(Targets.Count);
                if (Targets[TargetIndex].HealthPoints > 25 && (Targets[TargetIndex].GetType() == typeof(Target)))
                {
                    CountShell--;
                    if (CountShell > 0)
                    {
                        if (Targets[TargetIndex].HealthPoints > 25 && Targets[TargetIndex].HealthPoints <= 50)
                        {
                            CountDestroyed++;
                        }
                        Thread.Sleep(Random.Next(85, 160));
                        Targets[TargetIndex].HealthPoints -= damage_degree;
                        DrawingAvia.Invoke(this);
                        CountHit++;
                        TotalDamage += damage_degree;
                    }
                }
            }
            if (currentTime == commonTime - 2)
            {
                timer.Tick -= new EventHandler(dispatcherTimerWork_Tick);
                timer.Stop();
                if (Thread.CurrentThread.Name.ToString() == (countThreadsAviations).ToString() && message)
                {   
                    MessageBox.Show("Shooting has been finished!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    Enabled.Invoke(this);
                }
                return;
            }
        }
            private void dispatcherTimerWork_Tick(object sender, EventArgs e)
            {
                currentTime++;
            }
    }            
} 
