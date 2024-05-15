using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetraPolyGame
{
    public class AllCards
    {
        // private static field to store the instance of the class
        private static AllCards CardDB;

        // all players of the quiz
        private List<Card> allCards;

        // The constructor for this type of class is private. This prevents any direct calls to create instances
        // of the class. Creates a new instance of a List to hold all the players.
        private AllCards()
        {
            allCards = new List<Card>();

            // if client information was stored in a file or database, it could be loaded here.
        }

        // Method to control access to the class. It checks if an instance has been created.
        // If it hasn't, it creates it; otherwise, it returns the existing instance.
        public static AllCards Instance
        {
            get
            {
                // check if an instance of the class has already been created.
                if (CardDB == null)
                {
                    CardDB = new AllCards();
                }
                return CardDB;
            }
        }

        /// <summary>
        /// Method to add a client to the database
        /// </summary>
        /// <param name="client">The client to add</param>
        public void AddClient(Card client)
        {
            allCards.Add(client);
        }

        public List<Card> GetCards()
        {
            return allCards;
        }

    }
}
