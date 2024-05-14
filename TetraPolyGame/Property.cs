using System;
using System.Collections.Generic;
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
        private int position;
        private int[] houseRent;

        public Property(string name, int position, int price, int rent, int[] houseRent, Player owned, bool mortgaged,
                        int mortgagePrice, int mortgageCost, int numberOfHouses, string colour)
            : base(name, price, rent, owned, mortgaged, mortgagePrice, mortgageCost)
        {
            this.numberOfHouses = numberOfHouses;
            this.colour = colour;
            this.position = position;
            this.houseRent = houseRent;
        }

        public int GetHouseCount()
        {
            return numberOfHouses;
        }

        public void AddHouse()
        {
            numberOfHouses++;
        }

        public void RemoveHouse()
        {

            numberOfHouses--;
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
            SetRent(houseRent[numberOfHouses]);
        }

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
            sb.Append(", " + position);

            return sb.ToString();
        }
    }
}
