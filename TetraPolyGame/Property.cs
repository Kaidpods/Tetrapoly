using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetraPolyGame
{
    public class Property : Card
    {
        private int numberOfHouses;
        private string colour; // e.g., "Brown," "Blue," etc.
        private int position;

        public Property(string name, int position, int price, int rent, Player owned, bool mortgaged,
                        int mortgagePrice, int mortgageCost, int numberOfHouses, string colour)
            : base(name, price, rent, owned, mortgaged, mortgagePrice, mortgageCost)
        {
            this.numberOfHouses = numberOfHouses;
            this.colour = colour;
            this.position = position;
        }

        public int GetHouseCount()
        {
            return numberOfHouses;
        }

        public void AddHouse()
        {
            // Implement logic to add a house to the property
            // Example: numberOfHouses++;
        }

        public override int GetRent()
        {
            // Implement logic to calculate and set the rent based on house count and other factors
            // Example: rent = baseRent + (numberOfHouses * houseRentIncrement);
            return base.GetRent();
        }
        
        private int GetPosition()
        {
            return position;
        }

        public override void UpdateRent()
        {
            int CurrentRent = GetRent();

            int houses = GetHouseCount();
            if (houses != 0)
            {
                switch (colour)
                {
                    case "BROWN":

                        break;
                    case "LBLUE":

                        break;
                    case "PINK":

                        break;
                    case "ORANGE":

                        break;
                    case "RED":

                        break;
                    case "YELLOW":

                        break;
                    case "GREEN":

                        break;
                    case "DBLUE":

                        break;

                }
            }
        }
    }
}
