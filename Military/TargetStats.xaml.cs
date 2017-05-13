using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Military
{
    /// <summary>
    /// Interaction logic for TargetStats.xaml
    /// </summary>
    public partial class TargetStats : Window
    {
        public List<KeyValuePair<int, int>> statistic = new List<KeyValuePair<int, int>>();
        public TargetStats()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            statistic = MainWindow.targetsStats;
            mChart.DataContext = statistic;
        }
    }
}
