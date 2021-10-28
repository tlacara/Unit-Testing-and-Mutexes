using System;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;

namespace Lab3Q1
{
    class Program
    {
        static void Main(string[] args)
        {
            // map and mutex for thread safety
            Mutex mutex = new Mutex();
            Dictionary<string, int> wcountsSingleThread = new Dictionary<string, int>();
            Dictionary<string, int> wcountsMultiThread = new Dictionary<string, int>();
            Stopwatch singlewatch = new Stopwatch();
            Stopwatch multiwatch = new Stopwatch();
            TimeSpan ts = multiwatch.Elapsed;
            TimeSpan ts1 = singlewatch.Elapsed;



            var filenames = new List<string> {
                "../../data/shakespeare_antony_cleopatra.txt",
                "../../data/shakespeare_hamlet.txt",
                "../../data/shakespeare_julius_caesar.txt",
                "../../data/shakespeare_king_lear.txt",
                "../../data/shakespeare_macbeth.txt",
                "../../data/shakespeare_merchant_of_venice.txt",
                "../../data/shakespeare_midsummer_nights_dream.txt",
                "../../data/shakespeare_much_ado.txt",
                "../../data/shakespeare_othello.txt",
                "../../data/shakespeare_romeo_and_juliet.txt",
           };


            //=============================================================
            // YOUR IMPLEMENTATION HERE TO COUNT WORDS IN SINGLE THREAD
            //=============================================================

            singlewatch.Start();
            foreach (string item in filenames)
            {
                HelperFunctions.CountCharacterWords(item, mutex, wcountsSingleThread);
            }
            singlewatch.Stop();

            var sortedByValueList = HelperFunctions.SortCharactersByWordcount(wcountsSingleThread);
            HelperFunctions.PrintListofTuples(sortedByValueList);

            Console.WriteLine();

            Console.WriteLine("SingleThread is Done!");

            string elapsedTime1 = String.Format("{0:00}:{1:00}.{2:00}", ts1.Minutes, ts1.Seconds, ts1.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime1);

            Console.WriteLine();


            //=============================================================
            // YOUR IMPLEMENTATION HERE TO COUNT WORDS IN MULTIPLE THREADS
            //=============================================================

            Thread[] thread = new Thread[filenames.Count];

            int i = 0;

            multiwatch.Start();
            foreach (string item in filenames)
            {
                thread[i] = new Thread(() => HelperFunctions.CountCharacterWords(
             filename: item,
             mutex: mutex,
             wcounts: wcountsMultiThread));
                thread[i].Start();
                
                i++;
            }

            foreach(Thread item in thread)
            {
                item.Join();
            }

            multiwatch.Stop();    

            var sortedByValueList1 = HelperFunctions.SortCharactersByWordcount(wcountsMultiThread);
            HelperFunctions.PrintListofTuples(sortedByValueList1);

            Console.WriteLine();
            Console.WriteLine("MultiThread is Done!");

            string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);

            Console.ReadLine();
        }
    }
}
