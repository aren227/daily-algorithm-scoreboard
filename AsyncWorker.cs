using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DailyAlgorithmWPF
{
    abstract class AsyncWorker
    {
        private int delay;

        private Thread thread;

        public AsyncWorker(int delay)
        {
            this.delay = delay;

            thread = new Thread(new ThreadStart(Loop));
            thread.Start();
        }
        protected abstract void Work();

        private void Loop()
        {
            while (true)
            {
                Work();
                Thread.Sleep(delay);
            }
        }

        public void Stop()
        {
            thread.Interrupt();
        }
    }
}
