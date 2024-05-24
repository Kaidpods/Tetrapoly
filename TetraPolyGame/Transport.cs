using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents.DocumentStructures;

namespace TetraPolyGame
{
    public class Transport : Utility
    {
        public Transport(string name, int position, int price, int rent, Player owned, bool mortgaged,
                        int mortgagePrice, int mortgageCost) : base(name, position, price, rent, owned, mortgaged, mortgagePrice, mortgageCost)
        {
            rent = 25;
        }

        public override int GetRent()
        {
            CheckMultiplier();
            return Convert.ToInt32(base.GetRent() * GetMult());
        }

        public void CheckMultiplier()
        {
            ObservableCollection<Card> playerCards = WhoOwns().CardsOwned;
            int amount = 0;
            foreach (Card card in playerCards)
            {
                if (card is Transport)
                {
                    Transport transport = (Transport)card;
                    if (transport.IsMortgaged() == false)
                    {
                        amount++;
                    }
                }
            }
            switch (amount)
            {
                case 1:
                    SetMultiplier(1);
                    break;

                case 2:
                    SetMultiplier(2);
                    break;

                case 3:
                    SetMultiplier(4);
                    break;

                case 4:
                    SetMultiplier(8);
                    break;
            }
        }
    }
}
