using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace DailyAlgorithmWPF
{
    class StartupRegister
    {

        public static void Register()
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            rk.SetValue("DailyAlgorithmScoreboard", "C:\\Users\\mathd\\source\\repos\\DailyAlgorithmWPF\\DailyAlgorithmWPF\\bin\\Release\\DailyAlgorithmWPF.exe");
        }

    }
}
