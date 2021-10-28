using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;


namespace Lab3Q1
{
    public class HelperFunctions
    {
        /**
         * Counts number of words, separated by spaces, in a line.
         * @param line string in which to count words
         * @param start_idx starting index to search for words
         * @return number of words in the line
         */
        public static int WordCount(ref string line, int start_idx)
        {
            // YOUR IMPLEMENTATION HERE
            int length = line.Length;
            int count=0;

            if (String.IsNullOrWhiteSpace(line))
            {
                return 0;
            }

            while (start_idx < length)
            {
                while (start_idx < length && char.IsWhiteSpace(line[start_idx]))
                    start_idx++;

                while (start_idx < length && !char.IsWhiteSpace(line[start_idx]))
                    start_idx++;


                count++;
            }

            return count;      
        }


        /**
        * Reads a file to count the number of words each actor speaks.
        *
        * @param filename file to open
        * @param mutex mutex for protected access to the shared wcounts map
        * @param wcounts a shared map from character -> word count
        */
        public static void CountCharacterWords(string filename,
                                 Mutex mutex,
                                 Dictionary<string, int> wcounts)
        {

            //===============================================
            //  IMPLEMENT THIS METHOD INCLUDING THREAD SAFETY
            //===============================================

             string line;  // for storing each line read from the file
             string character = "";  // empty character to start
             System.IO.StreamReader file = new System.IO.StreamReader(filename);

             while ((line = file.ReadLine()) != null)
             {
                //=================================================
                // YOUR JOB TO ADD WORD COUNT INFORMATION TO MAP
                //=================================================

                // Is the line a dialogueLine?
                //    If yes, get the index and the character name.
                //      if index > 0 and character not empty
                //        get the word counts
                //          if the key exists, update the word counts
                //          else add a new key-value to the dictionary
                //    reset the character
                int index = IsDialogueLine(line, ref character);

                //dialogue line
                if ( index!= -1)
                {
                    //dialogue line for character
                    if (index > 0 && character != null)
                    {
                        int count = WordCount(ref line, index);

                        mutex.WaitOne();
                        if (wcounts.ContainsKey(character))
                        {
                            
                            wcounts[character] += count;
                            
                        }

                        else
                        {
                            
                            wcounts.Add(character, count);
                           
                        }
                        mutex.ReleaseMutex();

                    }
                    
                }
                
                              
               }
            // Close the file
            file.Close();
        }



        /**
         * Checks if the line specifies a character's dialogue, returning
         * the index of the start of the dialogue.  If the
         * line specifies a new character is speaking, then extracts the
         * character's name.
         *
         * Assumptions: (doesn't have to be perfect)
         *     Line that starts with exactly two spaces has
         *       CHARACTER. <dialogue>
         *     Line that starts with exactly four spaces
         *       continues the dialogue of previous character
         *
         * @param line line to check
         * @param character extracted character name if new character,
         *        otherwise leaves character unmodified
         * @return index of start of dialogue if a dialogue line,
         *      -1 if not a dialogue line
         */
        static int IsDialogueLine(string line, ref string character)
        {

            // new character
            if (line.Length >= 3 && line[0] == ' '
                && line[1] == ' ' && line[2] != ' ')
            {
                // extract character name

                int start_idx = 2;
                int end_idx = 3;
                while (end_idx <= line.Length && line[end_idx - 1] != '.')
                {
                    ++end_idx;
                }

                // no name found
                if (end_idx >= line.Length)
                {
                    return 0;
                }

                // extract character's name
                character = line.Substring(start_idx, end_idx - start_idx - 1);
                return end_idx;
            }

            // previous character
            if (line.Length >= 5 && line[0] == ' '
                && line[1] == ' ' && line[2] == ' '
                && line[3] == ' ' && line[4] != ' ')
            {
                // continuation
                return 4;
            }

            return 0;
        }

        /**
         * Sorts characters in descending order by word count
         *
         * @param wcounts a map of character -> word count
         * @return sorted vector of {character, word count} pairs
         */
        public static List<Tuple<string, int>> SortCharactersByWordcount(Dictionary<string, int> wordcount)
        {

            // Implement sorting by word count here
            List<Tuple<string, int>> sortedByValueList = new List<Tuple<string, int>>();         


            foreach (KeyValuePair<string,int> item in wordcount.OrderByDescending(key=>key.Value))
            {
                sortedByValueList.Add(Tuple.Create(item.Key, item.Value));
                
            }
         
            return sortedByValueList;

        }


        /**
         * Prints the List of Tuple<int, string>
         *
         * @param sortedList
         * @return Nothing
         */
        public static void PrintListofTuples(List<Tuple<string, int>> sortedList)
        {

          // Implement printing here
          foreach(Tuple<string,int> item in sortedList)
          {
                Console.WriteLine(item);
          }

        }
    }
}
