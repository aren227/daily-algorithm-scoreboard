using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DailyAlgorithmWPF
{
    class ProblemPanel : StackPanel
    {
        private Problem problem;
        private DateTime date;

        public ProblemPanel(Problem problem, DateTime date)
        {
            this.problem = problem;

            Orientation = Orientation.Horizontal;
            Height = 24;
            Margin = new Thickness(2, 2, 2, 2);

            Label label = new Label();
            label.FontFamily = new System.Windows.Media.FontFamily("메이플스토리");
            label.FontSize = 18;
            label.Padding = new Thickness(0, 0, 0, 0);
            label.Content = problem.name;
            label.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            label.Width = 300 - 84 - (problem.IsSolved() ? 26 : 0);
            label.VerticalContentAlignment = VerticalAlignment.Center;

            Label triesLabel = new Label();
            triesLabel.FontFamily = new System.Windows.Media.FontFamily("메이플스토리");
            triesLabel.FontSize = 15;
            triesLabel.Padding = new Thickness(0, 0, 0, 0);

            int tries = problem.GetTries(date);
            triesLabel.Content = tries + " " + (tries > 1 ? "tries" : "try");
            triesLabel.Background = new SolidColorBrush(problem.IsSolved(date) ? Color.FromRgb(66, 245, 90) : Color.FromRgb(245, 75, 66));
            triesLabel.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            triesLabel.Width = 80;
            triesLabel.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
            triesLabel.VerticalContentAlignment = VerticalAlignment.Center;
            
            // 푼 경우에만 티어 노출
            if (problem.IsSolved())
            {
                Image tier = new Image();
                tier.Width = 22;
                tier.Height = 22;
                tier.Margin = new Thickness(2, 0, 2, 0);
                tier.Source = problem.GetDifficultyImage();
                tier.VerticalAlignment = VerticalAlignment.Center;

                Children.Add(tier);
            }
            
            Children.Add(label);
            Children.Add(triesLabel);
        }
    }
}
