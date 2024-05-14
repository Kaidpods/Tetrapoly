using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetraPolyGame
{
    public class Transport : Utility
    {
        public Transport(double multiplier, string name, int position, int price, int rent, Player owned, bool mortgaged,
                        int mortgagePrice, int mortgageCost) : base(multiplier,name,position, price, rent, owned, mortgaged, mortgagePrice, mortgageCost)
        {
            rent = 25;
            multiplier = 1.0;
        }
        public int GetRailRent()
        {
            return Convert.ToInt32(GetRent() * GetMult());
        }
    }
}
