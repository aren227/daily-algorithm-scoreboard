using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DailyAlgorithmWPF
{
    class SolvedAC : AsyncWorker
    {
        private ConcurrentQueue<Problem> queue;

        public SolvedAC() : base(100)
        {
            queue = new ConcurrentQueue<Problem>();
        }

        public void UpdateDifficulty(Problem problem)
        {
            queue.Enqueue(problem);
        }

        protected override void Work()
        {
            if (queue.IsEmpty) return;

            Problem problem;
            if (!queue.TryDequeue(out problem)) return;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.solved.ac/v2/problems/show.json?id=" + problem.problemId);
                request.Method = "GET";

                string result;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        result = reader.ReadToEnd();
                    }
                }

                Regex levelReg = new Regex("\"level\":(\\d+)");
                Match match = levelReg.Match(result);

                if (match.Success)
                {
                    int level = int.Parse(match.Groups[1].Value);
                    problem.difficulty = level;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
