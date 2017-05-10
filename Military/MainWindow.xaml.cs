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
        int militaries = 20;
        double timeEplaseed = 0, time = 20;
        Ellipse simplePoint = new Ellipse();
        Generator generator = new Generator();
        ObservableCollection<Target> TargetList = new ObservableCollection<Target>();
        ObservableCollection<Aviation> AviationList = new ObservableCollection<Aviation>();
        ObservableCollection<MineThower> MineThowerList = new ObservableCollection<MineThower>();
        ObservableCollection<Thread> AviationsThreads = new ObservableCollection<Thread>();
        ObservableCollection<Thread> Mine_ThrowersThreads = new ObservableCollection<Thread>();
        DispatcherTimer dispatcherTimerWork = new DispatcherTimer();
        DispatcherTimer dispatcherTimerGen = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            button_Start.IsEnabled = false;
        }

        private void button_Generate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClearThreads();
                if (MineThowerList != null && TargetList != null)
                {
                    int k = MineThowerList.Count;
                    int tt = root_Canvas.Children.Count;
                    int z = TargetList.Count;
                }
                root_Canvas.Children.Clear();
                militaries = Convert.ToInt32(militaries_Count.Text);
                if (militaries <= 0)
                {
                    throw new Exception("Count of militaries can't be less or equal 0");
                }
                TargetList.Clear();
                AviationList.Clear();
                MineThowerList.Clear();
                int targetCount = generator.GenereteTargets(ref TargetList, militaries);
                int avaiationCount = generator.GenerateAviations(ref AviationList, targetCount);
                int mineThowerCount = generator.GenerateMineThowers(ref MineThowerList, targetCount);
                count_MineThowers.Content = "Count mine-thowers : " + Convert.ToInt32(mineThowerCount);
                count_Aviations.Content = "Count aviations: " + Convert.ToInt32(avaiationCount);
                count_Targets.Content = "Count targets : " + Convert.ToInt32(targetCount);
                foreach (var target in TargetList)
                {
                    initTarget(target);
                }
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
            timeEplaseed = 0; 
            dispatcherTimerWork.Tick -= new EventHandler(dispatcherTimerWork_Tick);
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
                Mine_ThrowersThreads.Add(new Thread(() => item.Shoot(ref TargetList, timeEplaseed, time)));
            }
            foreach (var item in AviationList)
            {
                AviationsThreads.Add(new Thread(() => item.Shoot(ref TargetList, timeEplaseed, time)));
            }
            StartMineThowers();
            StartAviations();
            dispatcherTimerWork.Tick += new EventHandler(dispatcherTimerWork_Tick);
            dispatcherTimerWork.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimerWork.Start();
            button_Start.IsEnabled = false;
        }

        public void StartMineThowers()
        {
            foreach (var item in Mine_ThrowersThreads)
            {
                item.Start();
                Thread.Sleep(100);
            }
        }

        public void StartAviations()
        {
            foreach (var item in AviationsThreads)
            {
                item.Start();
                Thread.Sleep(100);
            }
        } 

        private void dispatcherTimerWork_Tick(object sender, EventArgs e)
        {
            if (timeEplaseed == time)
            {
                ClearThreads();
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
                timeEplaseed = timeEplaseed + 1;
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
                simplePoint.Fill = generator.targertsColor(target);
                if (target.GetType() == typeof(Target))
                {
                    root_Canvas.Children.Add(simplePoint);
                }
                else
                {
                    TextBlock textBlock = new TextBlock();
                    textBlock.Text = "Miss!";
                    textBlock.FontSize = 8;
                    textBlock.FontStyle = FontStyles.Italic;
                    textBlock.Foreground = new SolidColorBrush(Colors.DeepSkyBlue);
                    Canvas.SetLeft(textBlock, target.X - 10);
                    Canvas.SetTop(textBlock, target.Y + 10);
                    root_Canvas.Children.Add(textBlock);
                    TargetList.Remove(target);
                }
            }
        }

        private void initTarget(Target target)
        {
            if (target != null)
            {
                simplePoint = new Ellipse();
                simplePoint.Width = 10;
                simplePoint.Height = 10;
                simplePoint.StrokeThickness = 5;
                simplePoint.Margin = new Thickness(target.X - 5, target.Y - 5, 1, 1);
                simplePoint.Tag = (target.X).ToString() + (target.Y).ToString();
                simplePoint.Fill = generator.targertsColor(target);
                if (target.GetType() == typeof(Target))
                {
                    root_Canvas.Children.Add(simplePoint);
                }
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            ClearThreads();
        }
        private void ClearThreads()
        {
            dispatcherTimerWork.Stop();
            dispatcherTimerWork.Tick -= new EventHandler(dispatcherTimerWork_Tick);
            //if (threadTowers != null)
            //{
            //    threadTowers.Abort(); 
            //}
            //if (threadAviations != null)
            //{
            //    threadAviations.Abort();
            //}
            foreach (var item in AviationsThreads)
            {
                item.Abort();
            }
            foreach (var item in Mine_ThrowersThreads)
            {
                item.Abort();
            }
            AviationsThreads.Clear();
            Mine_ThrowersThreads.Clear();
            button_Generate.IsEnabled = false;
        }
    }
}
