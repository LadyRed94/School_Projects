using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Timer1
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            //Thread.Sleep(1000);
            Looping();
            timer.Stop();


            Console.WriteLine(timer.ElapsedMilliseconds);

            Console.WriteLine("Hello World!");
        }

        static void Looping()
        {
            Parallel.For(0,
                100,
                (i) => Thread.Sleep(1000)
                ); ;

        }
    }
}