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
    public delegate void DeleGateDraw(object sender);

    public class MineThower
    {
        public event DeleGateDraw DrawingTarget;
        public int Name { get; set; }
        public int CountHit { get; set; }
        public Random Random { get; set; }
        DispatcherTimer timer = new DispatcherTimer();
        int currentTime = 0;

        public MineThower(Random random, int сode)
        {
            Name = сode;
            CountHit = 0;

            Random = random;
        }

        public void Shoot(ref ObservableCollection<Target> Targets, double commonTime)
        {
            timer.Tick += new EventHandler(dispatcherTimerWork_Tick);
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Start();
            currentTime = 0;
            int TargetIndex = 0;
            int targetCount = Targets.Count;
            while (currentTime <= commonTime)
            {
                if (StopThowersTime(commonTime))
                {
                    return;
                }
                else
                {
                    TargetIndex = Random.Next(0, Targets.Count+ Random.Next(0,5));
                    int damage = Random.Next(35, 45);
                    Thread.Sleep(Random.Next(75, 100));
                    Targets[TargetIndex].HealthPoints -= damage;
                    CountHit++;
                    DrawingTarget.Invoke(this);
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
