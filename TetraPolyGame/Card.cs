using System;
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
        private player owned;
        private bool mortgaged;
        private int mortgagePrice;
        private int mortgageCost;

        public Card(string name, int price, int rent, player owned, bool mortgaged,
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
        public void ToggleOwnership(player WhoBought)
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

        public bool IsOwned()
        {
            return owned;
        }

        public bool IsMortgaged()
        {
            return mortgaged;
        }
    }

}
