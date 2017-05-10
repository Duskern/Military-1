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
        Random random = new Random();
        int militaries = 20,  x = 350, y = 400, time = 20, targetNumber = 0;
        Ellipse simplePoint;
        ObservableCollection<Target> TargetList;
        ObservableCollection<Aviation> AviationList;
        ObservableCollection<MineThower> MineThowerList;
        ObservableCollection<Thread> AviationsThreads = new ObservableCollection<Thread>();
        ObservableCollection<Thread> Mine_ThrowersThreads = new ObservableCollection<Thread>();
        DispatcherTimer dispatcherTimerWork = new DispatcherTimer();
        DispatcherTimer dispatcherTimerGen = new DispatcherTimer();
        DispatcherTimer dispatcherTimerDraw= new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            button_Start.IsEnabled = false;
        }

        private SolidColorBrush targertsColor(Target target)
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

        private void button_Generate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dispatcherTimerGen.Tick -= new EventHandler(dispatcherTimerGen_Tick);
                if (thread != null)
                {
                    thread.Abort();
                }
                //foreach (var item in AviationsThreads)
                //{
                //    item.Abort();
                //}
                foreach (var item in Mine_ThrowersThreads)
                {
                    item.Abort();
                }
                if (MineThowerList != null && TargetList!=null)
                {
                    int k = MineThowerList.Count;
                    int tt = root_Canvas.Children.Count;
                    int z = TargetList.Count;
                }
                AviationsThreads.Clear();
                Mine_ThrowersThreads.Clear();
                button_Generate.IsEnabled = false;
                root_Canvas.Children.Clear();
                militaries = Convert.ToInt32(militaries_Count.Text);
                if (militaries <= 0)
                {
                    throw new Exception("Count of militaries can't be less or equal 0");
                }
                ///////////////////////////////////////
                int targetCount = random.Next(5, militaries);
                int currentCount = 0; 
                TargetList = new ObservableCollection<Target>();
                while (currentCount < targetCount)
                {
                    x = random.Next(60, 790);
                    y = random.Next(45, 577);
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
                    currentCount = 0;
                    int avaiationCount = random.Next(1, militaries);
                    count_Aviations.Content = "Count aviations: " + Convert.ToInt32(avaiationCount);
                    AviationList = new ObservableCollection<Aviation>();
                    do
                    {
                        AviationList.Add(new Aviation(random));
                        currentCount++;
                    }
                    while (currentCount < avaiationCount);
                    int mineThowerCount = random.Next(1, militaries);
                    count_MineThowers.Content = "Count mine-thowers : " + Convert.ToInt32(mineThowerCount); 
                /////////////////////////////////////
                    currentCount = 0;
                    MineThowerList = new ObservableCollection<MineThower>();
                    do
                    {
                        MineThowerList.Add(new MineThower(random));
                        currentCount++;
                    }
                    while (currentCount < mineThowerCount);
                count_Targets.Content = "Count targets : " + Convert.ToInt32(targetCount);
                if (thread != null)
                {
                    thread.Abort();
                }
                dispatcherTimerGen.Tick += new EventHandler(dispatcherTimerGen_Tick);
                dispatcherTimerGen.Interval = new TimeSpan(0, 0, 1);
                dispatcherTimerGen.Start();  
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, " Error", MessageBoxButton.OK, MessageBoxImage.Error);
                button_Generate.IsEnabled = true;
            }
        }

        private void dispatcherTimerGen_Tick(object sender, EventArgs e)
        {
            if (targetNumber == TargetList.Count)
            {
                targetNumber = 0;
                MessageBox.Show("Press button 'Start military maneurus' ", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                button_Generate.IsEnabled = true;
                button_Start.IsEnabled = true;
                dispatcherTimerWork.Stop();
                dispatcherTimerGen.Tick -= new EventHandler(dispatcherTimerGen_Tick);
                return;
            }
            else
            {
                int k = TargetList.Count();
                DrawTarget(TargetList[targetNumber]);
                targetNumber++;
            }
        } 

        int timeEplaseed = 0;
        Thread thread;
        private void button_Start_Click(object sender, RoutedEventArgs e)
        {
            timeEplaseed = 0;
            dispatcherTimerWork.Tick -= new EventHandler(dispatcherTimerWork_Tick);
            time = Convert.ToInt32(militaries_Time.Text);
            foreach (var item in MineThowerList)
            {
                Mine_ThrowersThreads.Add(new Thread(() => item.Shoot(ref TargetList, timeEplaseed, time)));
            }
            foreach (var item in AviationList)
            {
                AviationsThreads.Add(new Thread(() => item.Shoot(ref TargetList, timeEplaseed, time)));
            }
            
            thread = new Thread(new ThreadStart(StartManeevers));
            thread.Start();
            dispatcherTimerWork.Tick += new EventHandler(dispatcherTimerWork_Tick);
            dispatcherTimerWork.Interval = new TimeSpan(0, 0, 0, 1);
            dispatcherTimerWork.Start();
            button_Start.IsEnabled = false;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (thread != null)
            {
                thread.Abort();
            }
            //foreach (var item in AviationsThreads)
            //{
            //    item.Abort();
            //}
            foreach (var item in Mine_ThrowersThreads)
            {
                item.Abort();
            }
        }

        public void StartManeevers()
        {
                //foreach (var item in AviationsThreads)
                //{
                //item.Start();
                //Thread.Sleep(1000);
                //}
                foreach (var item in Mine_ThrowersThreads)
                {
                    item.Start();
                    Thread.Sleep(1000);
                }
        }
           
        private void dispatcherTimerWork_Tick(object sender, EventArgs e)
        {
            if (timeEplaseed == time)
            {
                dispatcherTimerWork.Stop();
                dispatcherTimerWork.Tick -= new EventHandler(dispatcherTimerWork_Tick);
                button_Generate.IsEnabled = true;
                MessageBox.Show("Maneuvers done!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            else
            {

                for (int i = 0; i < TargetList.Count; i++)
                {
                    DrawTarget(TargetList[i]);
                }
                timeEplaseed++;
            }
        }

      
        public void DrawTarget(Target target)
        {
            if (target != null)
            {
                simplePoint = new Ellipse();
                simplePoint.Width = 10;
                simplePoint.Height = 10;
                simplePoint.StrokeThickness = 5;
                simplePoint.Margin = new Thickness(target.X - 5, target.Y - 5, 1, 1);
                simplePoint.Tag = (target.X).ToString() + (target.Y).ToString();
                simplePoint.Fill = targertsColor(target);
                if (target.GetType() == typeof(Target))
                {
                    root_Canvas.Children.Add(simplePoint);
                }
                else
                {
                    TextBlock textBlock = new TextBlock();
                    textBlock.Text = ";)";
                    textBlock.FontSize = 9;
                    textBlock.FontStyle = FontStyles.Italic;
                    textBlock.Foreground = new SolidColorBrush(Colors.DeepSkyBlue);
                    Canvas.SetLeft(textBlock, target.X-10);
                    Canvas.SetTop(textBlock, target.Y+10);
                    root_Canvas.Children.Add(textBlock);
                    TargetList.Remove(target);
                }
            }
        }
    }
}
