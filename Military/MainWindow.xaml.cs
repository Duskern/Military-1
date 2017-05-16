using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Military
{
    public partial class MainWindow : Window
    {
        ObjectsHelper objectsHelper = new ObjectsHelper();
        Generator generator = new Generator();
        private object threadLock = new object();
        public ObservableCollection<Target> TargetList = new ObservableCollection<Target>();
        public ObservableCollection<Aviation> AviationList = new ObservableCollection<Aviation>();
        public ObservableCollection<MineThower> MineThowerList = new ObservableCollection<MineThower>();
        public static List<KeyValuePair<int, int>> targetsStats = new List<KeyValuePair<int, int>>();
        public static List<KeyValuePair<int, int>> mineThowersStats = new List<KeyValuePair<int, int>>();
        public static List<List<KeyValuePair<int, int>>> aviationsStats = new List<List<KeyValuePair<int, int>>>();

        public MainWindow()
        {
            InitializeComponent();
            inintPartMenu();
            OptionTarget.Children.Add(objectsHelper.TargetUI);
            OptionTarget.Children.Add(objectsHelper.OptionText);
            OptionTarget.Children.Add(objectsHelper.EmptyUI);
            OptionTarget.Children.Add(objectsHelper.nextOption);
        }
        

        private void button_Generate_Click(object sender, RoutedEventArgs e)
        {
            try
            { 
                root_Canvas.Children.Clear();
              //   < Line  X1 = "0" X2 = "860" Y1 = "620" Y2 = "620" Fill = "Crimson"
              //      Stroke = "Crimson" StrokeThickness = "10" />
  
              //< Line  X1 = "850" X2 = "850" Y1 = "0" Y2 = "628" Fill = "Crimson"
              //      Stroke = "Crimson" StrokeThickness = "10" />
                  Line downLine = new Line();
                downLine.X1 = 0;
                downLine.X2 = 870;
                downLine.Y1 = 612;
                downLine.Y2 = 612;
                downLine.StrokeThickness = 20;
                Line sideLine = new Line();
                sideLine.X1 = 850;
                sideLine.Y1 = 0;
                sideLine.X2 = 850;
                sideLine.Y2 = 602; 
                sideLine.StrokeThickness = 24;
                downLine.Stroke = new SolidColorBrush(Colors.Lime);
                sideLine.Stroke = new SolidColorBrush(Colors.Yellow);
                root_Canvas.Children.Add(downLine);
                root_Canvas.Children.Add(sideLine);
                int targetCount = generator.GenereteTargets(ref TargetList, objectsHelper.militaries);
                int avaiationCount = generator.GenerateAviations(ref AviationList);
                int mineThowerCount = generator.GenerateMineThowers(ref MineThowerList);
                count_MineThowers.Content = "Count mine-thowers : " + Convert.ToInt32(mineThowerCount);
                count_Aviations.Content = "Count aviations: " + Convert.ToInt32(avaiationCount);
                count_Targets.Content = "Count targets : " + Convert.ToInt32(targetCount);
                foreach (var target in TargetList)
                {
                    initTarget(target);
                }
                button_TargetStats.IsEnabled = false;
                button_MineThowerStats.IsEnabled = false;
                button_AviationStats.IsEnabled = false;
                button_Generate.IsEnabled = true; 
                button_Start.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, " Error", MessageBoxButton.OK, MessageBoxImage.Error);
                button_Generate.IsEnabled = true;
            }
        }

        private void button_Start_Click(object sender, RoutedEventArgs e)
        {
            button_Generate.IsEnabled = false;
            try
            {
                objectsHelper.time = Convert.ToInt32(militaries_Time.Text);
                objectsHelper.Mine_ThrowersThreads.Clear();
                objectsHelper.AviationsThreads.Clear();
                if (objectsHelper.time <= 0)
                {
                    throw new ArgumentException("Time can't be less or equal then 0!");
                }
            }
            catch (ArgumentException argumentException)
            {
                MessageBox.Show(argumentException.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            foreach (var item in MineThowerList)
            {
                objectsHelper.Mine_ThrowersThreads.Add(new Thread(() => item.Shoot(ref TargetList, objectsHelper.time)));
                item.DrawingTarget += DrawEventTargets;
            }
            foreach (var item in AviationList)
            {
                objectsHelper.AviationsThreads.Add(new Thread(() => item.Shoot(ref TargetList, objectsHelper.time)));
                item.DrawingAvia += DrawEventTargets;
            }
            objectsHelper.currentTime = 0;
            objectsHelper.timer.Tick += new EventHandler(dispatcherTimerWork_Tick);
            objectsHelper.timer.Interval = TimeSpan.FromSeconds(1);
            objectsHelper.timer.Start();
            StartMineThowers();
            StartAviations();
            button_Start.IsEnabled = false;
        }
        
       

        private void DrawEventTargets(object sender)
        {
            for (int i = 0; i < TargetList.Count; i++)
            {
                lock (threadLock)
                {                  
                   DrawTarget(TargetList[i]);
                }
            }
        }

        public void StartMineThowers()
        {
            foreach (var item in objectsHelper.Mine_ThrowersThreads)
            {
                item.Start();
                Thread.Sleep(50);
            }
        }

        public void StartAviations()
        {
            foreach (var item in objectsHelper.AviationsThreads)
            { 
                item.Start();
                Thread.Sleep(50);
            }
        }

        public void DrawTarget(Target target)
        {
            try
            {
                Dispatcher.Invoke((Action)delegate
                {
                    drawUI(target, generator.emptiesColor(target));
                });
            }
            catch (Exception){}
        }

        private void initTarget(Target target)
        {
            drawUI(target, new SolidColorBrush(Colors.Black));
        } 

        private void drawUI(Target target, SolidColorBrush emptyColor)
        {
            if (target.GetType() == typeof(Target))
            {
                objectsHelper.pointsCollection = new PointCollection(); ;
                objectsHelper.TargetUI = new Polyline();
                objectsHelper.pointsCollection.Add(new Point(target.X, target.Y));
                objectsHelper.pointsCollection.Add(new Point(target.X + 14, target.Y - 16));
                objectsHelper.pointsCollection.Add(new Point(target.X + 26, target.Y));
                objectsHelper.pointsCollection.Add(new Point(target.X + 26, target.Y + 14));
                objectsHelper.pointsCollection.Add(new Point(target.X, target.Y + 14));
                objectsHelper.TargetUI.Points = objectsHelper.pointsCollection;
                objectsHelper.TargetUI.StrokeDashArray = new DoubleCollection() { 5, 1, 3, 1 };
                objectsHelper.TargetUI.Fill = generator.targertsColor(target);
                objectsHelper.TargetUI.StrokeThickness = 1;
                int targetNumber = TargetList.IndexOf(target);
                TextBlock targetNumberUI = new TextBlock();
                targetNumberUI.Text = "#" + target.Name.ToString();
                targetNumberUI.FontSize = 13;
                targetNumberUI.FontStyle = FontStyles.Italic;
                targetNumberUI.Foreground = new SolidColorBrush(Colors.Black);
                targetNumberUI.FontWeight = FontWeights.Bold;
                Canvas.SetLeft(targetNumberUI, target.X+2);
                Canvas.SetTop(targetNumberUI, target.Y - 2);
                root_Canvas.Children.Add(objectsHelper.TargetUI);
                root_Canvas.Children.Add(targetNumberUI);
            }
            else
            {
                objectsHelper.EmptyUI = new Ellipse();
                objectsHelper.EmptyUI.Width = 25;
                objectsHelper.EmptyUI.Height = 25;
                objectsHelper.EmptyUI.StrokeThickness = 1;
                objectsHelper.EmptyUI.Stroke = new SolidColorBrush(Colors.Black);
                objectsHelper.EmptyUI.Margin = new Thickness(target.X, target.Y, 1, 1);
                objectsHelper.EmptyUI.Fill = emptyColor;
                root_Canvas.Children.Add(objectsHelper.EmptyUI);
            }
        }  

        private void inintPartMenu()
        {
            objectsHelper.pointsCollection = new PointCollection();
            objectsHelper.TargetUI = new Polyline();
            int X = 13; int Y = 5;
            objectsHelper.pointsCollection.Add(new Point(X, Y));
            objectsHelper.pointsCollection.Add(new Point(X + 8, Y - 10));
            objectsHelper.pointsCollection.Add(new Point(X + 16, Y));
            objectsHelper.pointsCollection.Add(new Point(X + 16, Y + 10));
            objectsHelper.pointsCollection.Add(new Point(X, Y + 10));
            objectsHelper.TargetUI.Points = objectsHelper.pointsCollection;
            objectsHelper.TargetUI.StrokeDashArray = new DoubleCollection() { 5, 1, 3, 1 };
            objectsHelper.TargetUI.Fill = new SolidColorBrush(Colors.DeepSkyBlue);
            objectsHelper.TargetUI.StrokeThickness = 1.5;
            objectsHelper.OptionText = new TextBlock();
            objectsHelper.OptionText.FontSize = 16;
            objectsHelper.OptionText.FontStyle = FontStyles.Italic;
            objectsHelper.OptionText.Foreground = new SolidColorBrush(Colors.DeepSkyBlue);
            Canvas.SetLeft(objectsHelper.OptionText, X + 30);
            Canvas.SetTop(objectsHelper.OptionText, Y - 9);
            objectsHelper.OptionText.Text = "Targets";
            objectsHelper.EmptyUI = new Ellipse();
            objectsHelper.EmptyUI.Width = 18;
            objectsHelper.EmptyUI.Height = 18;
            objectsHelper.EmptyUI.StrokeThickness = 2;
            objectsHelper.EmptyUI.Stroke = new SolidColorBrush(Colors.MistyRose);
            objectsHelper.EmptyUI.Margin = new Thickness(X, Y + 20, 1, 1);
            objectsHelper.EmptyUI.Fill = new SolidColorBrush(Colors.Black);
            objectsHelper.nextOption = new TextBlock();
            objectsHelper.nextOption.Foreground = new SolidColorBrush(Colors.MistyRose);
            Canvas.SetLeft(objectsHelper.nextOption, X + 30);
            Canvas.SetTop(objectsHelper.nextOption, Y + 20);
            objectsHelper.nextOption.FontSize = 16;
            objectsHelper.nextOption.FontStyle = FontStyles.Italic;
            objectsHelper.nextOption.Text = "Empties";
            
        }

        private void button_TargetStats_Click(object sender, RoutedEventArgs e)
        {
            ShowTargetsStats();
        }

        private void ShowTargetsStats()
        {
            targetsStats = new List<KeyValuePair<int, int>>();
            List<Target> targets = TargetList.ToList();

            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i].HealthPoints < 0)
                {
                    targets[i].HealthPoints = 0;
                }
            }
            targets.OrderBy(x => x.Name).ToList();
            foreach (var target in targets)
            {
                if (target.GetType() == typeof(Target))
                {
                    targetsStats.Add(new KeyValuePair<int, int>(target.Name, target.HealthPoints));
                }
            }
            TargetStats targetChar = new TargetStats();
            targetChar.ShowDialog();
        }

        private void dispatcherTimerWork_Tick(object sender, EventArgs e)
        {
            if (objectsHelper.currentTime == objectsHelper.time + 4)
            {
                button_Generate.IsEnabled = true;
                button_TargetStats.IsEnabled = true;
                button_MineThowerStats.IsEnabled = true;
                button_AviationStats.IsEnabled = true;
                ShowTargetsStats();
                objectsHelper.timer.Stop();
                objectsHelper.timer.Tick -= new EventHandler(dispatcherTimerWork_Tick);
            }
            else
            {
                objectsHelper.currentTime++;
            }
        }

        private void SafeExit()
        {
            foreach (var item in MineThowerList)
            {
                item.DrawingTarget -= DrawEventTargets;
            }
            foreach (var item in MineThowerList)
            {
                item.DrawingTarget -= DrawEventTargets;
            }
            foreach (var item in objectsHelper.Mine_ThrowersThreads)
            {
                item.Abort();
            }
            foreach (var item in objectsHelper.AviationsThreads)
            {
                item.Abort();
            }
        }

        private void button_AviationStats_Click(object sender, RoutedEventArgs e)
        {
            aviationsStats = new List<List<KeyValuePair<int, int>>>();
            List<Aviation> aviations = AviationList.ToList();
            aviations.OrderBy(x => x.Name).ToList();
            List<KeyValuePair<int, int>> hit = new List<KeyValuePair<int, int>>();
            List<KeyValuePair<int, int>> shell = new List<KeyValuePair<int, int>>();
            List<KeyValuePair<int, int>> destroyed = new List<KeyValuePair<int, int>>();
            foreach (var aircraft in aviations)
            {
                hit.Add(new KeyValuePair<int, int>(aircraft.Name, aircraft.CountHit));
                shell.Add(new KeyValuePair<int, int>(aircraft.Name, aircraft.CountShell)); 
                destroyed.Add(new KeyValuePair<int, int>(aircraft.Name, aircraft.CountDestroyed)); 
            }
            aviationsStats.Add(hit);
            aviationsStats.Add(shell);
            aviationsStats.Add(destroyed);
            AviationStats aviaStatistic = new AviationStats();
            aviaStatistic.ShowDialog();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            SafeExit();
        } 

        private void button_MineThowerStats_Click(object sender, RoutedEventArgs e)
        {
            mineThowersStats = new List<KeyValuePair<int, int>>();
            List<MineThower> mineThowers = MineThowerList.ToList();
            mineThowers.OrderBy(x => x.Name).ToList();
            foreach (var mineThower in mineThowers)
            {
                mineThowersStats.Add(new KeyValuePair<int, int>(mineThower.Name, mineThower.CountHit));
            }
            MineThowerStats thowerStats = new MineThowerStats();
            thowerStats.ShowDialog();
        }

    }
}
