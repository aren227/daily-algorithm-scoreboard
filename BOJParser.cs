using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DailyAlgorithmWPF
{
    class BOJParser : AsyncWorker
    {
        private string username;

        private JudgeDatabase db;
        private SolvedAC solvedAC;

        public BOJParser(string username, JudgeDatabase db) : base(1000 * 90)
        {
            this.username = username;
            this.db = db;
            solvedAC = new SolvedAC();
        }

        public string GetPlatformName()
        {
            return "boj";
        }

        public void UpdateProblemStatus()
        {
            lock (db.locker)
            {
                try
                {
                    // 프로필을 긁자
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.acmicpc.net/user/" + username);
                    request.Method = "GET";
                    request.ContentType = "text/html";

                    string result;

                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            result = reader.ReadToEnd();
                        }
                    }

                    Regex reg = new Regex("\"problem_title\".+?(?=problem)problem\\/(\\d+)\"\\s+.+?(?=>)>(.+?(?=<))");
                    Match match = reg.Match(result);

                    int stopIndex = result.IndexOf("풀지 못한 문제");

                    while (match.Success)
                    {
                        if (stopIndex != -1 && match.Index > stopIndex) break;

                        string pid = match.Groups[1].Value;
                        string name = match.Groups[2].Value;

                        Problem problem = db.GetProblem(GetPlatformName(), pid);
                        problem.name = name;
                        problem.solved = true; // 채점 결과에 대한 정보는 없지만, 푼 문제이므로 AC 처리.

                        match = match.NextMatch();
                    }

                    // 채점 현황을 긁자
                    request = (HttpWebRequest)WebRequest.Create("https://www.acmicpc.net/status?problem_id=&user_id=" + username + "&language_id=-1&result_id=-1");
                    request.Method = "GET";
                    request.ContentType = "text/html";

                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            result = reader.ReadToEnd();
                        }
                    }

                    reg = new Regex("<tr\\s+id\\s+=\\s+\"solution-\\d+\"");
                    match = reg.Match(result);
                    while (match.Success)
                    {
                        int end = result.IndexOf("</tr>", match.Index);

                        string substr = result.Substring(match.Index, end - match.Index);

                        Regex jidReg = new Regex("<tr\\s+id\\s*=\\s*\"solution-(\\d+)\"\\s*>");
                        Match propertyMatch = jidReg.Match(substr);

                        string jid = propertyMatch.Groups[1].Value;

                        Regex titleReg = new Regex("title=\"([^\"]+)\"");
                        propertyMatch = titleReg.Match(substr);

                        string title = propertyMatch.Groups[1].Value;

                        Regex pidReg = new Regex("class=\"problem_title tooltip-click\\s+\">(\\d+)");
                        propertyMatch = pidReg.Match(substr);

                        string pid = propertyMatch.Groups[1].Value;

                        Regex resultReg = new Regex("class=\"result-text\">\\s*<span class=\"([\\w-]+)");
                        propertyMatch = resultReg.Match(substr);

                        Result res;
                        switch (propertyMatch.Groups[1].Value)
                        {
                            case "result-judging":
                                res = Result.PENDING;
                                break;
                            case "result-ac":
                                res = Result.AC;
                                break;
                            case "result-wa":
                                res = Result.WA;
                                break;
                            default:
                                res = Result.ETC;
                                break;
                        }

                        Regex timeReg = new Regex("data-timestamp=\"(\\d+)\"");
                        propertyMatch = timeReg.Match(substr);

                        long timestamp = long.Parse(propertyMatch.Groups[1].Value);

                        JudgeResult judgeResult = new JudgeResult(jid, timestamp, res);

                        Problem problem = db.GetProblem(GetPlatformName(), pid);
                        problem.name = title;
                        problem.AddResult(judgeResult);

                        // Enqueue
                        solvedAC.UpdateDifficulty(problem);

                        match = match.NextMatch();
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        protected override void Work()
        {
            UpdateProblemStatus();
        }
    }
}
