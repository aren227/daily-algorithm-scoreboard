using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyAlgorithmWPF
{
    public enum Result
    {
        PENDING, AC, WA, ETC
    }

    [Serializable]
    public class JudgeResult
    {
        public string judgeId;
        public long timestamp;
        public Result result;

        public JudgeResult() : this(null, 0, Result.ETC)
        {
            
        }
        public JudgeResult(string judgeId, long timestamp, Result result)
        {
            this.judgeId = judgeId;
            this.timestamp = timestamp;
            this.result = result;
        }

        public DateTime GetDateTime()
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(timestamp).ToLocalTime();
            return dateTime;
        }
    }
}
