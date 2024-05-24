using System;
using System.Collections.Generic;
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
            multiplier = 1;
        }

        public int GetRollRent(int diceRoll)
        {
            return Convert.ToInt32(diceRoll * multiplier);
        }

        public double GetMult()
        {
            return multiplier;
        }

        public void SetMultiplier(double multiplier)
        {
            this.multiplier = multiplier;
        }
    }
}
