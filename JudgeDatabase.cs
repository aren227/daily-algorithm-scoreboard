using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Xml.Serialization;

namespace DailyAlgorithmWPF
{
    class JudgeDatabase
    {
        List<Problem> problems;

        public object locker = new object();

        public JudgeDatabase()
        {
            problems = new List<Problem>();
            Load();

            DispatcherTimer saveTimer = new DispatcherTimer();
            saveTimer.Interval = TimeSpan.FromSeconds(200);
            saveTimer.Tick += SaveTick;
            saveTimer.Start();
        }

        public Problem GetProblem(string platformId, string problemId)
        {
            lock (locker)
            {
                Problem problem = null;
                foreach (Problem p in problems)
                {
                    if (p.platformId == platformId && p.problemId == problemId)
                    {
                        problem = p;
                        break;
                    }
                }

                if (problem == null)
                {
                    problem = new Problem(platformId, problemId, "No Name");
                    problems.Add(problem);
                }

                return problem;
            }
        }

        public int GetTotalSolved()
        {
            int result = 0;
            foreach (Problem p in problems)
            {
                if (p.solved) result++;
            }
            return result;
        }

        public int GetDailySolved()
        {
            return GetSolvedInDay(DateTime.Today);
        }

        public int GetSolvedInDay(DateTime date)
        {
            int result = 0;
            foreach (Problem p in problems)
            {
                if (p.IsSolvedInDay(date)) result++;
            }
            return result;
        }

        public List<DateTime> GetTrainedDays()
        {
            HashSet<DateTime> days = new HashSet<DateTime>();
            foreach (Problem p in problems)
            {
                foreach (JudgeResult jr in p.judgeResults)
                {
                    days.Add(jr.GetDateTime().Date);
                }
            }

            // 언제 시도했는지 알 수 없는 문제들 제거
            days.Remove(new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            List<DateTime> result = new List<DateTime>(days);
            result.Sort();

            return result;
        }

        public List<Problem> GetProblemsTriedIn(DateTime date)
        {
            List<Tuple<JudgeResult, Problem>> pairs = new List<Tuple<JudgeResult, Problem>>();
            foreach (Problem p in problems)
            {
                JudgeResult jr = p.GetLatestTryIn(date);
                if (jr == null) continue;
                pairs.Add(new Tuple<JudgeResult, Problem>(jr, p));
            }

            // 가장 최근 시도 순으로 정렬
            pairs.Sort((a, b) => -a.Item1.timestamp.CompareTo(b.Item1.timestamp));

            List<Problem> result = new List<Problem>();
            foreach(var p in pairs)
            {
                result.Add(p.Item2);
            }

            return result;
        }

        private void Load()
        {
            if (System.IO.File.Exists(GetDataPath()))
            {
                TextReader reader = null;
                try
                {
                    var serializer = new XmlSerializer(typeof(List<Problem>));
                    reader = new StreamReader(GetDataPath());
                    problems = (List<Problem>)serializer.Deserialize(reader);
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }
            }

            // Remove Invalid Judge Result
            foreach(Problem p in problems)
            {
                p.judgeResults.RemoveAll((value) => (value.judgeId.Length == 0));
            }
        }

        private void Save()
        {
            lock (locker)
            {
                TextWriter writer = null;
                try
                {
                    var serializer = new XmlSerializer(typeof(List<Problem>));
                    writer = new StreamWriter(GetDataPath());
                    serializer.Serialize(writer, problems);
                }
                finally
                {
                    if (writer != null)
                        writer.Close();
                }
            }
        }

        private void SaveTick(object sender, EventArgs e)
        {
            Save();
        }

        private string GetDataPath()
        {
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\DailyAlgorithmScoreboard";
            if (!System.IO.Directory.Exists(dir))
            {
                System.IO.Directory.CreateDirectory(dir);
            }
            return dir + "\\judge_data.xml";
        }
    }
}
