using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Configuration;

namespace DailyAlgorithmWPF
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private JudgeDatabase db;

        public MainWindow()
        {
            db = new JudgeDatabase();

            BOJParser parser = new BOJParser("mathdong01", db);

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(10);
            timer.Tick += Tick;
            timer.Start();

            StartupRegister.Register();

            InitializeComponent();

            LoadWindowLocation();
        }

        private void Tick(object sender, EventArgs e)
        {
            // 화면 업데이트
            List<DateTime> days = db.GetTrainedDays();

            ndays.Content = "알고리즘 폐관수련 " + days.Count + "일차";
            total_solved_count.Content = db.GetTotalSolved().ToString();
            daily_solved_count.Content = db.GetDailySolved().ToString();

            feed.Children.Clear();

            for (int i = days.Count - 1; i >= 0; i--)
            {
                DateTime dt = days[i];

                DateLabel dateLabel = new DateLabel(dt);

                if(i < days.Count - 1)
                {
                    dateLabel.Margin = new Thickness(2, 12, 2, 4);
                }

                feed.Children.Add(dateLabel);

                List<Problem> problems = db.GetProblemsTriedIn(dt);
                foreach (Problem p in problems)
                {
                    ProblemPanel panel = new ProblemPanel(p, dt);
                    feed.Children.Add(panel);
                }
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                SaveWindowLocation();
            }
        }

        private void LoadWindowLocation()
        {
            System.Drawing.Point winLoc = Properties.Settings.Default.WinLoc;

            Left = winLoc.X;
            Top = winLoc.Y;
        }

        private void SaveWindowLocation()
        {
            Properties.Settings.Default.WinLoc = new System.Drawing.Point((int)Left, (int)Top);
            Properties.Settings.Default.Save();
        }
    }
}
