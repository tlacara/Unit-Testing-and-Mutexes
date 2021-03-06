using System;
using System.Collections.Generic;

namespace Lab3Q1
{
    public class WordCountTester
    {
        static void Main()
        {
          try {


                //=================================================
                // Implement your tests here. Check all the edge case scenarios.
                // Create a large list which iterates over WCTester
                //=================================================
               

                List<Tuple<string, int, int>> list = new List<Tuple<string, int, int>>();

                list.Add(Tuple.Create(" hello world", 0, 2)); //correct output      
                list.Add(Tuple.Create("", 0,0)); //
                list.Add(Tuple.Create(" ", 0, 0));
                list.Add(Tuple.Create("hello world",5,1));
                list.Add(Tuple.Create("world", 5, 0));
                list.Add(Tuple.Create("h", 0, 0)); //incorrect output

                foreach (Tuple<string,int,int> item in list)
                {
                    WCTester(item.Item1, item.Item2,item.Item3);
                }


            } catch(UnitTestException e) {
              Console.WriteLine(e);
            }

            Console.ReadLine();
           
        }


        /**
         * Tests word_count for the given line and starting index
         * @param line line in which to search for words
         * @param start_idx starting index in line to search for words
         * @param expected expected answer
         * @throws UnitTestException if the test fails
         */
          static void WCTester(string line, int start_idx, int expected) {

            //=================================================
            // Implement: comparison between the expected and
            // the actual word counter results
            //=================================================
            int result =HelperFunctions.WordCount(ref line, start_idx);

            if (result != expected) {
              throw new UnitTestException(ref line, start_idx, result, expected, String.Format("UnitTestFailed: result:{0} expected:{1}, line: {2} starting from index {3}", result, expected, line, start_idx));
            }

            Console.WriteLine("Result matches expected value");
           }
    }
}
