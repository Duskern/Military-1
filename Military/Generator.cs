using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Military
{
    public class Generator
    {
        Random random = new Random(); 

        public int GenereteTargets(ref ObservableCollection<Target> TargetList , int militaries)
        {
            TargetList = new ObservableCollection<Target>();
            int targetCount = random.Next(5, militaries);
            int currentCount = 1;
            TargetList = new ObservableCollection<Target>();
            while (currentCount < targetCount)
            {
                int x = random.Next(60, 790);
                int y = random.Next(45, 577);
                Target target = new Target(x, y);
                if (TargetList.Count == 0)
                {
                    TargetList.Add(target);
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
                }
            }
            return targetCount;
        }

        public int GenerateAviations(ref ObservableCollection<Aviation> AviationList, int militaries)
        {
            int currentCount = 0;
            int avaiationCount = random.Next(1, militaries);
            AviationList = new ObservableCollection<Aviation>();
            do
            {
                AviationList.Add(new Aviation(random));
                currentCount++;
            }
            while (currentCount < avaiationCount);
            return avaiationCount;
        }

        public int GenerateMineThowers(ref ObservableCollection<MineThower> MineThowerList, int militaries)
        {
            int currentCount = 0;
            int mineThowerCount = random.Next(1, militaries);
            MineThowerList = new ObservableCollection<MineThower>();
            do
            {
                MineThowerList.Add(new MineThower(random));
                currentCount++;
            }
            while (currentCount < mineThowerCount);
            return mineThowerCount;
        }

        public SolidColorBrush targertsColor(Target target)
        {
            if (target.HealthPoints == 100)
            {
                return new SolidColorBrush(Colors.White);
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
                return new SolidColorBrush(Colors.Coral);
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

    }
}
