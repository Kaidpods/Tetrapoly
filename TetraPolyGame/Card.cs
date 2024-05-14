﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TetraPolyGame
{
    public abstract class Card
    {
        private string name;
        private int price;
        private int rent;
        private Player_dupe owned;
        private bool mortgaged;
        private int mortgagePrice;
        private int mortgageCost;

        public Card(string name, int price, int rent, Player_dupe owned, bool mortgaged,
                    int mortgagePrice, int mortgageCost)
        {
            this.name = name;
            this.price = price;
            this.rent = rent;
            this.owned = owned;
            this.mortgaged = mortgaged;
            this.mortgagePrice = mortgagePrice;
            this.mortgageCost = mortgageCost;
        }
        public void ToggleOwnership(Player_dupe WhoBought)
        {
            owned = WhoBought;
        }

        public void ToggleMorgaged()
        {
            mortgaged = !mortgaged;
        }

        public virtual int GetRent()
        {
            // Implement your rent calculation logic here
            // Example: return rent * numberOfHouses;
            return rent;
        }

        public virtual int GetPrice()
        {
            return price;
        }

        public virtual void SetRent(int RentToSet)
        {
            // Implement your rent calculation logic here
            // Example: return rent * numberOfHouses;
            rent = RentToSet;
        }

        public virtual void UpdateRent()
        {
            // Implement your rent update logic here
            // Example: rent = baseRent + (numberOfHouses * houseRentIncrement);
        }

        public Player_dupe IsOwned()
        {
            return owned;
        }

        public bool IsMortgaged()
        {
            return mortgaged;
        }

        public int GetMortgagePrice()
        {
            return mortgageCost;
        }

        public virtual string ToString()
        {
            StringBuilder sb = new();
            sb.Append(name +", ");
            sb.Append(price +", ");
            sb.Append(rent + ", ");
            sb.Append(mortgageCost + ", ");
            sb.Append(mortgagePrice);

            return sb.ToString();
        }
    }

}
