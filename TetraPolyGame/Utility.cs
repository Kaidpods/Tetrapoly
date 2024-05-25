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
    public class Utility : Card
    {
        private double multiplier;
        public Utility(string name, int position, int price, int rent, Player owned, bool mortgaged,
                        int mortgagePrice, int mortgageCost) : base(name, position, price, rent, owned, mortgaged, mortgagePrice, mortgageCost)
        {
            multiplier = 4;
        }

        public int GetRollRent(int diceRoll)
        {
            CheckMultiplier();
            return Convert.ToInt32(diceRoll * multiplier);
        }

        public double GetMult()
        {
            return multiplier;
        }

        public void SetMultiplier(double mult)
        {
            multiplier = mult;
        }

        public virtual void CheckMultiplier()
        {
                ObservableCollection<Card> playerCards = WhoOwns().CardsOwned;
                int amount = 0;
                foreach (Card card in playerCards)
                {
                    if (card is Utility)
                    {
                        Utility utility = (Utility)card;
                        if (utility.IsMortgaged() == false)
                        {
                            amount++;
                        }
                    }
                }
                switch (amount)
                {
                    case 1:
                        multiplier = 4 ;
                        break;

                    case 2:
                        multiplier = 10;
                        break;
                }
            }
        }
    }
