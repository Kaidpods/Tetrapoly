using System.IO;

namespace TetraPolyGame
{
    /// <summary>
    /// At the start of each round there is a 10% chance a random event occurs which effects all players
    /// </summary>
    internal class RandomEvent
    {
        //create the random outside the loop
        public static Random RandomGen = new Random();

        public static void EventChance()
        {
            //percentage chance of a random event happening
            int chancePercentage = 10;

            for (int i = 0; i < 100; i++)
            {
                //gets a number between 0 and 100
                int randomValue = RandomGen.Next(100);
                //runs a random event if the number is below 10
                if (randomValue < chancePercentage)
                {
                    //read from csv file containing unique id for each case, name of event, description of event
                }
            }       
        }


        /// <summary>
        /// Read data from a file. This works 
        /// </summary>
        /// <param name="filename">The name of the file to read.</param>
        /// <returns>List containing the lines in the file.</returns>
        /// <exception cref="Exception">Any errors are thrown up to be dealty with.</exception>
        public static List<string> ReadFromFile(string filename)
        {
            List<string> rows = new List<string>();
            try
            {
                using (StreamReader reader = new StreamReader(filename))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        rows.Add(line);
                    }
                }
            }
            catch (FileNotFoundException fnfe)
            {
                throw new Exception("Exception thrown: " + fnfe.Message);
            }
            catch (IOException ioe)
            {
                throw new Exception("Exception thrown: " + ioe.Message);
            }
            return rows;
        }
    }
}




