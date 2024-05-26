using System.IO;
using System.Windows;

namespace TetraPolyGame
{
    /// <summary>
    /// At the start of each round there is a 10% chance a random event occurs which effects all players
    /// </summary>
    public class RandomEvent
    {
        //create the random outside the loop
        public static Random RandomGen = new Random();

        public static bool EventChance()
        {
            //percentage chance of a random event happening
            int chancePercentage = 5;


                //gets a number between 0 and 100
                int randomValue = RandomGen.Next(100);
                //runs a random event if the number is below 10
                if (randomValue <= chancePercentage)
                {
                    return true;
                }
                return false;
        }

        public static List<Event> GetEvents()
        {
            List<Event> Events = new List<Event>();

            //read from csv file containing unique id for each case, name of event, description of event
            List<String> lines = new List<String>();
            string line;
            System.IO.StreamReader file = new System.IO.StreamReader("resources\\RandomEvents.csv");
            try
            {
                while ((line = file.ReadLine()) != null)
                {
                    lines.Add(line);
                }
                file.Close();

                //lines[0];
            }catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            Shuffle.Shuffle.ShuffleList(Events);
            return Events;
        }
    }
}




