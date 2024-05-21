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
                    List<String> lines = new List<String>();
                    string line;
                    System.IO.StreamReader file = new System.IO.StreamReader("resources\\RandomEvents.csv");

                    while ((line = file.ReadLine()) != null)
                    {
                        lines.Add(line);
                    }
                    file.Close();

                    //lines[0];
                }
            }       
        }
    }
}




