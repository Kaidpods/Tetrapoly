using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TetraPolyGame
{
    public class Property : Card
    {
        private int numberOfHouses;
        private string colour; // e.g., "Brown," "Blue," etc.
        private int[] houseRent;

        public Property(string name, int position, int price, int rent, int[] houseRent, Player owned, bool mortgaged,
                        int mortgagePrice, int mortgageCost, int numberOfHouses, string colour)
            : base(name, position, price, rent, owned, mortgaged, mortgagePrice, mortgageCost)
        {
            this.numberOfHouses = numberOfHouses;
            this.colour = colour;
            this.houseRent = houseRent;
        }

        /// <summary>
        /// Get the count of houses.
        /// </summary>
        /// <returns>The number of houses.</returns>
        public int GetHouseCount()
        {
            return numberOfHouses;
        }

        /// <summary>Adds a house to the count of houses.</summary>
        public void AddHouse()
        {
            numberOfHouses++;
        }

        /// <summary>Removes a specified number of houses from the total count.</summary>
        /// <param name="HouseAmount">The number of houses to remove.</param>
        public void RemoveHouse(int HouseAmount)
        {

            numberOfHouses--;
        }

        /// <summary>
        /// Gets the color of the object.
        /// </summary>
        /// <returns>The color of the object as a string.</returns>
        public string GetColour()
        {
            return colour;
        }

        /// <summary>
        /// Gets the rent value for a specific property.
        /// </summary>
        /// <returns>The rent value for the property.</returns>
        public override int GetRent()
        {
            UpdateRent();
            return base.GetRent();
        }

        public int[] GetHouseRents()
        {
            return houseRent;
        }
       

        /// <summary>
        /// Updates the rent for a specific house based on the number of houses owned.
        /// </summary>
        /// <remarks>
        /// This method sets the rent for a house based on the number of houses owned by a player.
        /// It retrieves the rent value from the houseRent array using the numberOfHouses as an index.
        /// </remarks>
        public override void UpdateRent()
        {
            ObservableCollection<Card> cards = WhoOwns().CardsOwned;

            double multiplier = 1;
            int colourAmount = 0;

            foreach (Card card in cards)
            {
                if (card is Property)
                {
                    Property tempProp = (Property)card;
                    if (tempProp.colour == colour)
                    {
                        colourAmount++;
                    }
                }
            }

            switch (colour)
            {
                case "BROWN": 
                    if (colourAmount == 2)
                    {
                        multiplier = 2;
                    }
                    break;
                case "LBLUE":
                    if (colourAmount == 3)
                    {
                        multiplier = 2;
                    }
                    break;
                case "PINK":
                    if (colourAmount == 3)
                    {
                        multiplier = 2;
                    }
                    break;

                case "ORANGE":
                    if (colourAmount == 3)
                    {
                        multiplier = 2;
                    }
                    break;
                case "RED":
                    if (colourAmount == 3)
                    {
                        multiplier = 2;
                    }
                    break;

                case "YELLOW":
                    if (colourAmount == 3)
                    {
                        multiplier = 2;
                    }
                    break;
                case "GREEN":
                    if (colourAmount == 3)
                    {
                        multiplier = 2;
                    }
                    break;

                case "DBLUE":
                    if (colourAmount == 2)
                    {
                        multiplier = 2;
                    }
                    break;
            }

            SetRent(Convert.ToInt32(houseRent[numberOfHouses] * multiplier));
        }

        /// <summary>Returns a string representation of the object.</summary>
        /// <returns>A string containing the base object's string representation, house rent values, color, and position.</returns>
        public override string ToString()
        {
            StringBuilder sb = new();
            sb.Append(base.ToString());
            sb.Append("[" + houseRent[1] +", ");
            sb.Append(houseRent[2] + ", ");
            sb.Append(houseRent[3] + ", ");
            sb.Append(houseRent[4] + ", ");
            sb.Append(houseRent[5] + "]");
            sb.Append(", " + colour);
            sb.Append(", " + GetPosition());

            return sb.ToString();
        }
    }
}
