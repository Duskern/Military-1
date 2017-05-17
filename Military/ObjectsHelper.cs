using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Military
{
    public class ObjectsHelper
    {
        public int militaries { get; set; }
        public double time { get; set; }
        public Ellipse EmptyUI { get; set; }
        public TextBlock nextOption { get; set; }
        public TextBlock OptionText { get; set; }
        public ObservableCollection<Thread> AviationsThreads { get; set; }
        public ObservableCollection<Thread> Mine_ThrowersThreads { get; set; }
        public DispatcherTimer timer { get; set; }
        public PointCollection pointsCollection { get; set; }
        public int currentTime { get; set; }
        public Polyline TargetUI { get; set; }
        Random random = new Random();

        public ObjectsHelper()
        {
            militaries = random.Next(5,70); 
            time = 10;
            EmptyUI = new Ellipse();
            nextOption = new TextBlock();
            OptionText = new TextBlock();
            AviationsThreads = new ObservableCollection<Thread>();
            Mine_ThrowersThreads = new ObservableCollection<Thread>();
            timer = new DispatcherTimer();
            pointsCollection = new PointCollection();
            currentTime = 0;
            TargetUI = new Polyline();
        }
    }
}
