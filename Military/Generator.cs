using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Shapes;

namespace Military
{
    public class Generator
    {
        Random random = new Random();

        public Polyline TargetUI;
        public PointCollection pointsCollection;

        public int trueCount { get; set; }

        public int GenereteTargets(ref ObservableCollection<Target> TargetList , int militaries)
        {
            TargetList = new ObservableCollection<Target>();
            int currentCount = 0;
            int less = 0;
            int code = 0;
            int x0 = 0, y0 = 10, xMax = 840, yMax = 600;
            while (y0 < yMax)
            {
                int valueMiss = random.Next(1, 40);
                if (valueMiss == 3 || valueMiss == 7)
                {
                    if (x0 >= xMax)
                    {
                        x0 = 0;
                        y0 += 30;
                    }
                    if (y0 <= yMax)
                    {
                        Target target = new Target(x0, y0 + 7, code);
                        TargetList.Add(target);
                        x0 += 30;
                        currentCount++;
                        code++;
                    }
                }
                else
                {
                    if (x0 >= xMax)
                    {
                        x0 = 0;
                        y0 += 30;
                    }
                    if (y0 <= yMax)
                    {
                        TargetList.Add(new EmptyTarget(x0, y0 - 8, 0));
                        x0 += 30;
                        less++;
                    }
                }
            }
            trueCount = currentCount - 1;
            return trueCount;
        }

        public int GenerateAviations(ref ObservableCollection<Aviation> AviationList)
        {
            int currentCount = 0;
            int avaiationCount = random.Next(2, trueCount/4);
            int code = 1;
            AviationList = new ObservableCollection<Aviation>();
            do
            {
                AviationList.Add(new Aviation(random, code));
                currentCount++;
                code++;
            }
            while (currentCount < avaiationCount + 1);
            return AviationList.Count;
        }

        public int GenerateMineThowers(ref ObservableCollection<MineThower> MineThowerList)
        {
            int currentCount = 0;
            int mineThowerCount = random.Next(2, trueCount/4);
            int code = 1;
            MineThowerList = new ObservableCollection<MineThower>();
            do
            {
                MineThowerList.Add(new MineThower(random, code));
                currentCount++;
                code++;
            }
            while (currentCount < mineThowerCount+ 1);
            return MineThowerList.Count;
        }

        public SolidColorBrush targertsColor(Target target)
        {
            if (target.HealthPoints == 100)
            {
                return new SolidColorBrush(Colors.DeepSkyBlue);
            }
            else if (target.HealthPoints < 100 && target.HealthPoints >= 55)
            {
                return new SolidColorBrush(Colors.Lime);
            }
            else if (target.HealthPoints < 55 && target.HealthPoints == 50)
            {
                return new SolidColorBrush(Colors.Yellow);
            }
            else if (target.HealthPoints < 50 && target.HealthPoints >= 25)
            {
                return new SolidColorBrush(Colors.Magenta);
            }
            else if (target.HealthPoints < 25 && target.HealthPoints >= 1)
            {
                return new SolidColorBrush(Colors.OrangeRed);
            }
            else
            {
                return new SolidColorBrush(Colors.Crimson);
            }
        }

        public SolidColorBrush emptiesColor(Target target)
        {
            if (target.HealthPoints == 100)
            {
                return new SolidColorBrush(Colors.Black);
            }
            else if (target.HealthPoints < 100 && target.HealthPoints >= 55)
            {
                Color randomColor = Color.FromRgb((byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256));
                return new SolidColorBrush(randomColor);
            }
            else if (target.HealthPoints < 55 && target.HealthPoints == 50)
            {
                Color randomColor = Color.FromRgb((byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(56));
                return new SolidColorBrush(randomColor);
            }
            else if (target.HealthPoints < 50 && target.HealthPoints >= 25)
            {
                Color randomColor = Color.FromRgb((byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256));
                return new SolidColorBrush(randomColor);
            }
            else if (target.HealthPoints < 25 && target.HealthPoints >= 1)
            {
                Color randomColor = Color.FromRgb((byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256));
                return new SolidColorBrush(randomColor);
            }
            else
            {
                Color randomColor = Color.FromRgb((byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256));
                return new SolidColorBrush(randomColor);
            }
        }
    }
}
