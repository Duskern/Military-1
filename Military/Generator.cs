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

        public int GenereteTargets(ref ObservableCollection<Target> TargetList , ref List<int> targetsIndex)
        {
            TargetList = new ObservableCollection<Target>();
            targetsIndex = new List<int>();
            int currentCount = 0;
            int less = 0;
            int code = 0;
            int x0 = 0, y0 = 10, xMax = 840, yMax = 600;
            while (y0 < yMax)
            {
                int valueMiss = random.Next(1, 40);
                if (valueMiss == 3 || valueMiss == 7 || valueMiss == 9 || valueMiss == 5)
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
                        targetsIndex.Add(TargetList.IndexOf(target));
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
            trueCount = TargetList.Count - less;
            return trueCount;
        }

        public int GenerateAviations(ref ObservableCollection<Aviation> AviationList)
        {
            int currentCount = 0;
            int a = trueCount;
            int avaiationCount = random.Next(2, trueCount/4);
            int code = 1;
            AviationList = new ObservableCollection<Aviation>();
            do
            {
                AviationList.Add(new Aviation(random, code));
                currentCount++;
                code++;
            }
            while (currentCount < avaiationCount);
            return AviationList.Count;
        }

        public int GenerateMineThowers(ref ObservableCollection<MineThower> MineThowerList)
        {
            int currentCount = 0; 
            int mineThowerCount = random.Next(2, trueCount/ 4);
            int code = 1;
            MineThowerList = new ObservableCollection<MineThower>();
            do
            {
                MineThowerList.Add(new MineThower(random, code));
                currentCount++;
                code++;
            }
            while (currentCount < mineThowerCount);
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
                return new SolidColorBrush(Colors.Red);
            }
        }

        public SolidColorBrush emptiesColor(Target target)
        {
            Color randomColor = Color.FromRgb((byte)random.Next(105, 256), (byte)random.Next(105, 256), (byte)random.Next(105, 256));
            if (target.HealthPoints == 100)
            {
                return new SolidColorBrush(Colors.Black);
            }
            else if (target.HealthPoints < 100 && target.HealthPoints >= 55)
            {
                randomColor = Color.FromRgb((byte)random.Next(105, 256), (byte)random.Next(105, 256), (byte)random.Next(105, 256));
                return new SolidColorBrush(randomColor);
            }
            else if (target.HealthPoints < 55 && target.HealthPoints == 50)
            {
                return new SolidColorBrush(Colors.Yellow);
            }
            else if (target.HealthPoints < 50 && target.HealthPoints >= 25)
            {
                randomColor = Color.FromRgb((byte)random.Next(105, 256), (byte)random.Next(105, 256), (byte)random.Next(105, 256));
                return new SolidColorBrush(randomColor);
            }
            else if (target.HealthPoints < 25 && target.HealthPoints >= 1)
            {
                randomColor = Color.FromRgb((byte)random.Next(105, 256), (byte)random.Next(105, 256), (byte)random.Next(105, 256));
                return new SolidColorBrush(randomColor);
            }
            else
            {
                return new SolidColorBrush(randomColor);
            }
        }

    }
}
