using System;
using System.Threading;
using System.Threading.Tasks;

namespace TeoVincent.Deadlock
{
    class Program
    {
        private static readonly object lockA = new object();
        private static readonly object lockB = new object();

        static void Main(string[] args)
        {
            Console.WriteLine("Let's get the started. We are in main thread");

            Task.Factory.StartNew(RiseDeadlock); // #01

            Console.WriteLine("We are before lockB in main thread.");
            lock (lockB) // #04
            {
                Console.WriteLine("We are in lockB in main thread.");
                Console.WriteLine("Simulate long running instruction in main thread.");
                Thread.Sleep(5000); // #05
                Console.WriteLine("The long running instruction has been ended in main thread.");
                Console.WriteLine("We are waiting for release of lockA in main thread.");
                lock (lockA) // #06
                {
                    Console.WriteLine("This code will never be executed in main thread.");
                }
            }

            Console.ReadLine();
        }

        private static void RiseDeadlock()
        {
            Console.WriteLine("RiseDeadlock method hes been started in new thread.");
            Console.WriteLine("We are before lockA in RiseDeadlock method.");
            lock (lockA) // #02
            {
                Console.WriteLine("We are in lockA in RiseDeadlock method.");
                Console.WriteLine("Simulate long running instruction in RiseDeadlock method.");
                Thread.Sleep(10000); // #03
                Console.WriteLine("The long running instruction has been ended in RiseDeadlock method.");

                Console.WriteLine("We are in lockA before lockB in RiseDeadlock method.");
                Console.WriteLine("We are waiting for release of lockB in RiseDeadlock method.");
                lock (lockB) // # 07
                {
                    Console.WriteLine("This code will never be executed in RiseDeadlock method.");
                }
            }
        }
    }
}
