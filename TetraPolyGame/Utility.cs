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
        private int position;
        public Utility(double multiplier, string name, int position, int price, int rent, Player owned, bool mortgaged,
                        int mortgagePrice, int mortgageCost) : base(name, position, price, rent, owned, mortgaged, mortgagePrice, mortgageCost)
        {
            this.multiplier = multiplier;
            multiplier = 1.0;
        }

        public int GetRollRent(int diceRoll)
        {
            return Convert.ToInt32(diceRoll * multiplier);
        }

        public double GetMult()
        {
            return multiplier;
        }
    }
}
