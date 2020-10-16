using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace DailyAlgorithmWPF
{
    [Serializable]
    public class Problem
    {
        public string platformId;
        public string problemId;
        public string name;

        public bool solved = false;
        public int difficulty = 0;

        public long lastTimestamp;

        public List<JudgeResult> judgeResults;

        public Problem() : this(null, null, null)
        {

        }

        public Problem(string platformId, string problemId, string name)
        {
            this.platformId = platformId;
            this.problemId = problemId;
            this.name = name;

            judgeResults = new List<JudgeResult>();
        }

        public JudgeResult GetJudgeResult(string judgeId)
        {
            foreach (JudgeResult jr in judgeResults)
            {
                if (jr.judgeId == judgeId)
                {
                    return jr;
                }
            }
            return null;
        }

        public void AddResult(JudgeResult result)
        {
            lastTimestamp = Math.Max(lastTimestamp, result.timestamp);

            if (result.result == Result.AC)
            {
                solved = true;
            }

            JudgeResult prev = GetJudgeResult(result.judgeId);
            if(prev == null)
            {
                judgeResults.Add(result);
            }
            else
            {
                // 결과만 업데이트 (PENDING -> AC / WA)
                prev.result = result.result;
            }

            judgeResults.Sort((a, b) => a.timestamp.CompareTo(b.timestamp));
        }

        public bool IsSolvedInDay(DateTime date)
        {
            if (!solved) return false;
            DateTime solveTime = GetSolvedDay();
            return (date <= solveTime && solveTime < date.AddDays(1));
        }

        // 만약 date날에 시도하였다면 그 날의 결과 중 가장 최근 결과를 반환한다.
        // 아닐 경우 null을 반환한다.
        public JudgeResult GetLatestTryIn(DateTime date)
        {
            DateTime next = date.AddDays(1);
            for (int i = judgeResults.Count - 1; i >= 0; i--)
            {
                DateTime dt = judgeResults[i].GetDateTime();
                if (date <= dt && dt < next) return judgeResults[i];
            }
            return null;
        }

        public DateTime GetSolvedDay()
        {
            foreach (JudgeResult result in judgeResults)
            {
                if (result.result == Result.AC)
                {
                    return result.GetDateTime().Date;
                }
            }

            return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        }

        public int GetTries()
        {
            return judgeResults.Count;
        }

        // date 날짜까지만 반영
        public int GetTries(DateTime date)
        {
            int tries = 0;
            foreach (JudgeResult result in judgeResults)
            {
                if (result.GetDateTime() >= date.AddDays(1)) break;
                tries++;
            }
            return tries;
        }

        public bool IsSolved()
        {
            return solved;
        }

        // date 날짜까지만 반영
        public bool IsSolved(DateTime date)
        {
            foreach (JudgeResult result in judgeResults)
            {
                if (result.GetDateTime() >= date.AddDays(1)) break;
                if (result.result == Result.AC) return true;
            }
            return false;
        }

        public BitmapImage GetDifficultyImage()
        {
            if (difficulty == -1)
            {
                return new BitmapImage(new Uri("99.png", UriKind.Relative));
            }
            return new BitmapImage(new Uri(difficulty + ".png", UriKind.Relative));
        }
    }
}
