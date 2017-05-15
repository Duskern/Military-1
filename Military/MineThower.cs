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
    public delegate void ItemEnabled(object sender);
    public delegate void DeleGateDrawEmpty(object sender); 

    public class MineThower
    {
        public event DeleGateDraw DrawingTarget;
        public event ItemEnabled Enabled;
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

        public void Shoot(ref ObservableCollection<Target> Targets, double commonTime, bool message, int last)
        {
            timer.Tick += new EventHandler(dispatcherTimerWork_Tick);
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Start();
            currentTime = 0;
            while (currentTime < commonTime - 2)
            {
                if (currentTime >= commonTime - 2)
                {
                    return;
                }
                Thread.Sleep(Random.Next(65, 110));
                int TargetIndex = Random.Next(Targets.Count);
                int damage = Random.Next(35, 45);
                if ((Targets[TargetIndex].GetType() == typeof(Target)))
                {
                    Targets[TargetIndex].HealthPoints -= damage;
                    CountHit++;
                }
                else
                {
                    Targets[TargetIndex].HealthPoints -= damage;
                    CountHit++;
                }
                DrawingTarget.Invoke(this);
            }
            if (message && Thread.CurrentThread.Name == last.ToString())
            {
                timer.Tick -= new EventHandler(dispatcherTimerWork_Tick);
                timer.Stop();
                Enabled.Invoke(this);
            }
            return;
        }

        private void dispatcherTimerWork_Tick(object sender, EventArgs e)
        { 
            currentTime++;
        }
    }
}
