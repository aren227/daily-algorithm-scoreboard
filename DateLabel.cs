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
    class DateLabel : Label
    {
        private DateTime date;

        public DateLabel(DateTime date)
        {
            this.date = date;

            FontFamily = new System.Windows.Media.FontFamily("메이플스토리");
            FontSize = 15;
            Padding = new Thickness(0, 0, 0, 0);
            Margin = new Thickness(2, 4, 2, 4);
            Height = 20;
            Content = GetText();
            Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            HorizontalContentAlignment = System.Windows.HorizontalAlignment.Left;
        }

        private string GetText()
        {
            DateTime today = DateTime.Now.Date;
            if (today <= date)
            {
                return "오늘";
            }
            if (today.AddDays(-1) <= date)
            {
                return "어제";
            }
            if (today.AddDays(-2) <= date)
            {
                return "그저께";
            }
            return date.Month + "월 " + date.Day + "일";
        }
    }
}
