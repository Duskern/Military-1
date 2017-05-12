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

    public class MineThower
    {
        public event DeleGateDraw DrawingTarget;
        public event ItemEnabled Enabled;
        public int CountHit { get; set; }
        public int TotalDamage { get; set; }
        public int CountMiss{ get; set; }
        public Random Random { get; set; }
        DispatcherTimer timer = new DispatcherTimer();
        int currentTime = 0;

        public MineThower(Random random)
        {
            CountHit = 0;
            CountMiss = 0;
            TotalDamage = 0;
            Random = random;
        }

        public void Shoot(ref ObservableCollection<Target> Targets, double commonTime, int countThreadsMine, bool message)
        {
            timer.Tick += new EventHandler(dispatcherTimerWork_Tick);
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Start();
            currentTime = 0;
            while (currentTime < commonTime - 2)
            {
                Thread.Sleep(Random.Next(80, 150));
                int TargetIndex = Random.Next(Targets.Count);
                int damage = Random.Next(35, 45);
                if ((Targets[TargetIndex].GetType() == typeof(Target)))
                {
                    Targets[TargetIndex].HealthPoints -= damage;
                    CountHit++;
                    TotalDamage += damage;
                }
                else
                {
                    CountMiss++;
                    Targets[TargetIndex].HealthPoints -= damage;
                }
                DrawingTarget.Invoke(this);
            }
            if (currentTime == commonTime-2)
            {
                timer.Tick -= new EventHandler(dispatcherTimerWork_Tick);
                timer.Stop();
                
                if (Thread.CurrentThread.Name.ToString() == (countThreadsMine).ToString() && message)
                {
                    MessageBox.Show("Shooting has been finished!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    Enabled.Invoke(this);
                }
                return;
            }
        }


        private  void dispatcherTimerWork_Tick(object sender, EventArgs e)
        { 
            currentTime++;
        }
    }
}
