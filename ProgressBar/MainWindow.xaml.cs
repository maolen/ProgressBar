using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;

namespace ProgressBar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
        }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        protected virtual void OnPropertyChanged(string property)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
        private double timerComplete = 0.0;
        public double TimerCompleted
        {
            get => timerComplete; 
            set
            {
                if (timerComplete != value)
                {
                    timerComplete = value;
                    OnPropertyChanged("TimerCompleted");
                }
            }
        }

        private void StartAnimation(object sender, RoutedEventArgs e)
        {
            TimerCompleted = 0.0;

            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += (s, ea) =>
            {
                TimerCompleted += 1.0;
                if (TimerCompleted >= 100.0)
                    timer.Stop();
            };
            timer.Interval = new TimeSpan(0, 0, 0, 0, 30);  // 2/sec
            timer.Start();
        }
    }
}
