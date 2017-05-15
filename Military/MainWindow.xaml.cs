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
        int militaries = 70;
        double time = 10;
        Ellipse EmptyUI = new Ellipse();
        TextBlock nextOption;
        TextBlock OptionText;
        Generator generator = new Generator();
        ObservableCollection<Target> TargetList = new ObservableCollection<Target>();
        ObservableCollection<Aviation> AviationList = new ObservableCollection<Aviation>();
        ObservableCollection<MineThower> MineThowerList = new ObservableCollection<MineThower>();
        ObservableCollection<Thread> AviationsThreads = new ObservableCollection<Thread>();
        ObservableCollection<Thread> Mine_ThrowersThreads = new ObservableCollection<Thread>();
        DispatcherTimer dispatcherTimerWork = new DispatcherTimer();
        DispatcherTimer dispatcherTimerGen = new DispatcherTimer();
        public Polyline TargetUI = new Polyline();
        private object threadLock = new object();
        public PointCollection pointsCollection = new PointCollection();
        public static List<KeyValuePair<int, int>> targetsStats = new List<KeyValuePair<int, int>>();
        public static List<KeyValuePair<int, int>> mineThowersStats = new List<KeyValuePair<int, int>>();
        public static List<List<KeyValuePair<int, int>>> aviationsStats = new List<List<KeyValuePair<int, int>>>();
        Random random = new Random();
        DispatcherTimer timer = new DispatcherTimer();
        int currentTime = 0;

        public MainWindow()
        {
            InitializeComponent();
            inintPartMenu();
            OptionTarget.Children.Add(TargetUI);
            OptionTarget.Children.Add(OptionText);
            OptionTarget.Children.Add(EmptyUI);
            OptionTarget.Children.Add(nextOption);
        }
        

        private void button_Generate_Click(object sender, RoutedEventArgs e)
        {
            try
            { 
                Mine_ThrowersThreads.Clear();
                AviationsThreads.Clear();
                Thread.Sleep(100);
                root_Canvas.Children.Clear();
                militaries = Convert.ToInt32(militaries_Count.Text);
                if (militaries <= 0)
                {
                    throw new Exception("Count of militaries can't be less or equal 0");
                }
                Thread.Sleep(200);
                int targetCount = generator.GenereteTargets(ref TargetList, militaries);
                Thread.Sleep(300);
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
                time = Convert.ToInt32(militaries_Time.Text);
                if (time <= 0)
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
                Mine_ThrowersThreads.Add(new Thread(() => item.Shoot(ref TargetList, time)));
                item.DrawingTarget += DrawEventTargets;
            }
            foreach (var item in AviationList)
            {
                AviationsThreads.Add(new Thread(() => item.Shoot(ref TargetList, time)));
                item.DrawingAvia += DrawEventTargets;
            }
            currentTime = 0;
            timer.Tick += new EventHandler(dispatcherTimerWork_Tick);
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Start();
            StartMineThowers();
            StartAviations();
            button_Start.IsEnabled = false;
        }
        
        private void dispatcherTimerWork_Tick(object sender, EventArgs e)
        {
            currentTime++;
            if (currentTime == time + 7)
            {
                foreach (var item in MineThowerList)
                {
                    item.DrawingTarget -= DrawEventTargets;
                }
                foreach (var item in MineThowerList)
                {
                    item.DrawingTarget -= DrawEventTargets;
                }
                button_Generate.IsEnabled = true;
                button_TargetStats.IsEnabled = true;
                button_MineThowerStats.IsEnabled = true;
                button_AviationStats.IsEnabled = true;
                ShowTargetsStats();
            }
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
            foreach (var item in Mine_ThrowersThreads)
            {
                item.Start();
                Thread.Sleep(50);
            }
        }

        public void StartAviations()
        {
            foreach (var item in AviationsThreads)
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
                if (target.GetType() == typeof(Target))
                {
                        pointsCollection = new PointCollection(); ;
                        TargetUI = new Polyline();
                        pointsCollection.Add(new Point(target.X, target.Y));
                        pointsCollection.Add(new Point(target.X + 14, target.Y - 16));
                        pointsCollection.Add(new Point(target.X + 26, target.Y));
                        pointsCollection.Add(new Point(target.X + 26, target.Y + 14));
                        pointsCollection.Add(new Point(target.X, target.Y + 14));
                        TargetUI.Points = pointsCollection;
                        TargetUI.StrokeDashArray = new DoubleCollection() { 5, 1, 3, 1 };
                        TargetUI.Fill = generator.targertsColor(target);
                        TargetUI.StrokeThickness = 1;
                        int targetNumber = TargetList.IndexOf(target);
                        TextBlock targetNumberUI = new TextBlock();
                        targetNumberUI.Text = target.Name.ToString();
                        targetNumberUI.FontSize = 13;
                        targetNumberUI.FontStyle = FontStyles.Italic;
                        targetNumberUI.Foreground = new SolidColorBrush(Colors.Black);
                        targetNumberUI.FontWeight = FontWeights.Bold;
                        Canvas.SetLeft(targetNumberUI, target.X);
                        Canvas.SetTop(targetNumberUI, target.Y - 2);
                        root_Canvas.Children.Add(TargetUI);
                        root_Canvas.Children.Add(targetNumberUI);
                    }
                    else
                    {
                        Color randomColor = Color.FromRgb((byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256));
                        EmptyUI = new Ellipse();
                        EmptyUI.Width = 25;
                        EmptyUI.Height = 25;
                        EmptyUI.StrokeThickness = 1;
                        EmptyUI.Margin = new Thickness(target.X, target.Y, 1, 1);
                        EmptyUI.Fill = generator.emptiesColor(target);
                        root_Canvas.Children.Add(EmptyUI);
                    }
                });
            }
            catch (Exception){}
        }

        private void initTarget(Target target)
        {
            if (target.GetType() == typeof(Target))
            {
                pointsCollection = new PointCollection(); ;
                TargetUI = new Polyline();
                pointsCollection.Add(new Point(target.X, target.Y));
                pointsCollection.Add(new Point(target.X + 14, target.Y - 16));
                pointsCollection.Add(new Point(target.X + 26, target.Y));
                pointsCollection.Add(new Point(target.X + 26, target.Y + 14));
                pointsCollection.Add(new Point(target.X, target.Y + 14));
                TargetUI.Points = pointsCollection;
                TargetUI.StrokeDashArray = new DoubleCollection() { 5, 1, 3, 1 };
                TargetUI.Fill = generator.targertsColor(target);
                TargetUI.StrokeThickness = 1;
                int targetNumber = TargetList.IndexOf(target);
                TextBlock targetNumberUI = new TextBlock();
                targetNumberUI.Text = target.Name.ToString();
                targetNumberUI.FontSize = 13;
                targetNumberUI.FontStyle = FontStyles.Italic;
                targetNumberUI.Foreground = new SolidColorBrush(Colors.Black);
                targetNumberUI.FontWeight = FontWeights.Bold;
                Canvas.SetLeft(targetNumberUI, target.X);
                Canvas.SetTop(targetNumberUI, target.Y - 2);
                root_Canvas.Children.Add(TargetUI);
                root_Canvas.Children.Add(targetNumberUI);
            }
            else
            {
                EmptyUI = new Ellipse();
                EmptyUI.Width = 25;
                EmptyUI.Height = 25;
                EmptyUI.StrokeThickness = 1;
                EmptyUI.Margin = new Thickness(target.X, target.Y, 1, 1);
                EmptyUI.Fill = new SolidColorBrush(Colors.Black);
                root_Canvas.Children.Add(EmptyUI);
            }
        }

        private void inintPartMenu()
        {
            pointsCollection = new PointCollection();
            TargetUI = new Polyline();
            int X = 13; int Y = 5;
            pointsCollection.Add(new Point(X, Y));
            pointsCollection.Add(new Point(X + 8, Y - 10));
            pointsCollection.Add(new Point(X + 16, Y));
            pointsCollection.Add(new Point(X + 16, Y + 10));
            pointsCollection.Add(new Point(X, Y + 10));
            TargetUI.Points = pointsCollection;
            TargetUI.StrokeDashArray = new DoubleCollection() { 5, 1, 3, 1 };
            TargetUI.Fill = new SolidColorBrush(Colors.DeepSkyBlue);
            TargetUI.StrokeThickness = 1.5;
            OptionText = new TextBlock();
            OptionText.FontSize = 16;
            OptionText.FontStyle = FontStyles.Italic;
            OptionText.Foreground = new SolidColorBrush(Colors.DeepSkyBlue);
            Canvas.SetLeft(OptionText, X + 30);
            Canvas.SetTop(OptionText, Y - 9);
            OptionText.Text = "Targets";
            EmptyUI = new Ellipse();
            EmptyUI.Width = 18;
            EmptyUI.Height = 18;
            EmptyUI.StrokeThickness = 5;
            EmptyUI.Margin = new Thickness(X, Y + 20, 1, 1);
            EmptyUI.Fill = new SolidColorBrush(Colors.MintCream);
            nextOption = new TextBlock();
            nextOption.Foreground = new SolidColorBrush(Colors.MintCream);
            Canvas.SetLeft(nextOption, X + 30);
            Canvas.SetTop(nextOption, Y + 20);
            nextOption.FontSize = 16;
            nextOption.FontStyle = FontStyles.Italic;
            nextOption.Text = "Empties";
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
