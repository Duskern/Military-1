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
        Dictionary<int, int> targetCoordinate; 
        ObservableCollection<Target> TargetList;
        ObservableCollection<Aviation> AviationList;
        ObservableCollection<MineThower> MineThowerList;
        ObservableCollection<Thread> AviationsThreads = new ObservableCollection<Thread>();
        ObservableCollection<Thread> Mine_ThrowersThreads = new ObservableCollection<Thread>();
        DispatcherTimer dispatcherTimerWork = new DispatcherTimer();
        DispatcherTimer dispatcherTimerGen = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            button_Start.IsEnabled = false;
        } 

        public void DrawTarget(Target target)
        {
            simplePoint = new Ellipse();
            simplePoint.Width = 10;
            simplePoint.Height = 10;
            simplePoint.Fill = new SolidColorBrush(Colors.White);
            simplePoint.StrokeThickness = 5;
            simplePoint.Margin = new Thickness(target.X - 5, target.Y - 5, 1, 1);
            simplePoint.Tag = (target.X - 5).ToString() + (target.Y - 5).ToString();
            if (target.GetType() == typeof(Target))
            {
                root_Canvas.Children.Add(simplePoint);
            } 
            //else
            //{
            //    TextBlock textBlock = new TextBlock();
            //    textBlock.Text = "E";
            //    textBlock.Foreground = new SolidColorBrush(Colors.Azure);
            //    Canvas.SetLeft(textBlock, target.X);
            //    Canvas.SetTop(textBlock, target.Y);
            //    root_Canvas.Children.Add(textBlock);
            //}
        }

        public void DesrtroyingTarget(Target target)
        {
            if (target != null)
            {
                root_Canvas.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate ()
                {
                    root_Canvas.UpdateLayout();
                    simplePoint = new Ellipse();
                    simplePoint.Width = 10;
                    simplePoint.Height = 10;
                    simplePoint.Fill = new SolidColorBrush(Colors.White);
                    simplePoint.StrokeThickness = 5;
                    simplePoint.Margin = new Thickness(target.X - 5, target.Y - 5, 1, 1);
                    simplePoint.Tag = (target.X - 5).ToString() + (target.Y - 5).ToString();
                    FrameworkElement result = root_Canvas.Children.Cast<FrameworkElement>()
                                           .Where(x => x.Tag != null &&
                                                  x.Tag.ToString() == simplePoint.Tag.ToString())
                                           .First();
                    if (root_Canvas.Children.Contains(result))
                {
                int index = root_Canvas.Children.IndexOf(result);
                (root_Canvas.Children[index] as Ellipse).Fill = new SolidColorBrush(Colors.Yellow);
                }
            }));
            }
        }

        private void button_Generate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dispatcherTimerGen.Tick -= new EventHandler(dispatcherTimerGen_Tick);
                button_Generate.IsEnabled = false;
                root_Canvas.Children.Clear();
                targetCoordinate = new Dictionary<int, int>();
                int i = 0;
                militaries = Convert.ToInt32(militaries_Count.Text);
                if (militaries <= 0)
                {
                    throw new Exception("Count of militaries can't be less or equal 0");
                }
                int avaiationCount = random.Next(1, militaries);
                count_Aviations.Content = "Count aviations: " + Convert.ToInt32(avaiationCount);
                AviationList = new ObservableCollection<Aviation>();
                do
                {
                    AviationList.Add(new Aviation());
                    i++;
                }
                while (i < avaiationCount);
                int mineThowerCount = random.Next(1, militaries);
                count_MineThowers.Content = "Count mine-thowers : " + Convert.ToInt32(mineThowerCount);
                i = 0;
                MineThowerList = new ObservableCollection<MineThower>();
                do
                {
                    MineThowerList.Add(new MineThower());
                    i++;
                }
                while (i < mineThowerCount);
                int targetCount = random.Next(1, militaries);
                int emptiesCount = 0;
                i = 0;
                TargetList = new ObservableCollection<Target>();
                do
                {
                    x = random.Next(30, 680);
                    y = random.Next(30, 580);
                    if (targetCoordinate.Contains(new KeyValuePair<int, int>(x, y)))
                    {
                        do
                        {
                            x = random.Next(30, 680);
                            y = random.Next(30, 580);
                        }
                        while (targetCoordinate.Contains(new KeyValuePair<int, int>(x, y)));
                        targetCoordinate.Add(x, y);
                    }
                    else
                    {
                        targetCoordinate.Add(x, y);
                    }
                    int miss = random.Next(1, 5);
                    if (miss == 3)
                    {
                        TargetList.Add(new EmptyTarget(x,y));
                        emptiesCount++;
                    }
                    else
                    {
                        TargetList.Add(new Target(x, y));
                    }
                    i++;
                }
                while (i < targetCount);
                count_Targets.Content = "Count targets : " + Convert.ToInt32(targetCount - emptiesCount);
                //foreach (var target in TargetList)
                //{

                //    root_Canvas.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate ()
                //    {
                //        root_Canvas.UpdateLayout();
                //    }));
                //    Thread.Sleep(1000);
                //    DrawTarget(target);
                //}
                dispatcherTimerGen.Tick += new EventHandler(dispatcherTimerGen_Tick);
                dispatcherTimerGen.Interval = new TimeSpan(0, 0, 1);
                dispatcherTimerGen.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, " Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void dispatcherTimerGen_Tick(object sender, EventArgs e)
        {
            if (targetNumber == TargetList.Count - 1)
            {
                targetNumber = 0;
                MessageBox.Show("Press button 'Start military maneurus' ", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                button_Generate.IsEnabled = true;
                button_Start.IsEnabled = true;
                TargetList.Clear();
                targetCoordinate.Clear();
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

        private void button_Start_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dispatcherTimerWork.Tick -= new EventHandler(dispatcherTimerWork_Tick);
                time = Convert.ToInt32(militaries_Time.Text);
                if (time <= 0)
                {
                    throw new Exception("Time can't be less or equal 0");
                }
                button_Generate.IsEnabled = true;
                dispatcherTimerWork.Tick += new EventHandler(dispatcherTimerWork_Tick);
                dispatcherTimerWork.Interval = new TimeSpan(0, 0, 1);
                dispatcherTimerWork.Start();
                button_Generate.IsEnabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, " Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }  

        private void dispatcherTimerWork_Tick(object sender, EventArgs e)
        {
            if (dispatcherTimerWork.Interval != new TimeSpan(0, 0, time))
            {
                foreach (var i in AviationList)
                {
                    AviationsThreads.Add(new Thread(() => i.Shoot(ref TargetList)));
                }
                foreach (var i in MineThowerList)
                {
                    Mine_ThrowersThreads.Add(new Thread(() => i.Shoot(ref TargetList)));
                }
                foreach (var i in AviationsThreads)
                {
                    i.Start();
                }
                foreach (var i in Mine_ThrowersThreads)
                {
                    i.Start();
                }
                foreach (var target in TargetList)
                {
                    DesrtroyingTarget(target);
                }
            }
            else
            {
                foreach (var i in AviationsThreads)
                {
                    i.Abort();
                }
                foreach (var i in Mine_ThrowersThreads)
                {
                    i.Abort();
                }
                AviationsThreads.Clear();
                Mine_ThrowersThreads.Clear();
                AviationList.Clear();
                MineThowerList.Clear();
                dispatcherTimerWork.Stop();
                dispatcherTimerWork.Tick -= new EventHandler(dispatcherTimerWork_Tick);
                return;
            }
        }

    }
}
