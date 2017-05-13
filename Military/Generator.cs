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
            int targetCount = random.Next(10, militaries);
            int currentCount = 0;
            int less = 0;
            int code = 1;
            while (currentCount < targetCount)
            {
                int x = random.Next(30, 820);
                int y = random.Next(35, 577);
                int valueMiss = random.Next(1, 12);
                if (valueMiss == 3 || valueMiss == 7 || valueMiss == 9 || valueMiss == 5)
                {
                    Target target = new Target(x, y, code);
                    if (TargetList.Count == 0)
                    {
                        TargetList.Add(target);
                        currentCount++;
                        code++;
                    }
                    bool checkUnique = true;
                    foreach (Target tempTarget in TargetList)
                    {
                        if (target.X > tempTarget.X - 25 && target.X < tempTarget.X + 25 &&
                            target.Y > tempTarget.Y - 25 && target.Y < tempTarget.Y + 25)
                        {
                            checkUnique = false;
                        }
                    }
                    if (checkUnique)
                    {
                        TargetList.Add(target);
                        currentCount++;
                        code++;
                    }
                }
                else
                {
                    if (TargetList.Count == 0)
                    {
                        TargetList.Add(new EmptyTarget(x, y, 0));
                    }
                    bool checkUniqueEmpty = true;
                    foreach (Target tempTarget in TargetList)
                    {
                        if (x > tempTarget.X - 40 && x < tempTarget.X + 40 &&
                            y > tempTarget.Y - 40 && y < tempTarget.Y + 40)
                        {
                            checkUniqueEmpty = false;
                        }
                    }
                    if (checkUniqueEmpty)
                    {
                        TargetList.Add(new EmptyTarget(x, y, 0));
                        less++;
                    }
                }
            }
            int count = TargetList.Count - less;
            trueCount = count;
            return count;
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
            while (currentCount < avaiationCount);
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
            if (target.HealthPoints == 100)
            {
                return new SolidColorBrush(Colors.MintCream);
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
                Color randomColor = Color.FromRgb((byte)random.Next(105, 256), (byte)random.Next(105, 256), (byte)random.Next(105, 256));
                return new SolidColorBrush(randomColor);
            }
        }

    }
}
