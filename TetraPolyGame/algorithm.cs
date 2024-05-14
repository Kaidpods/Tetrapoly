using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetraPolyGame
{
     class algorithm : Player
    {
        //this is the constroctor for the algorithm 
        public algorithm(string name, int money, int position, Card cardowend, bool isailve, bool injail) : base(name, money, position, cardowend, isailve, injail)
        {
        }
        //this make the algorithm buy card
        public override void buy(Card gc)
        {
            int se = gc.GetPrice();
            LoseMoney(se);
            byCard(gc);
        }
        //this make the algorithm Lose Money
        public override void LoseMoney(int money)
        {
            if (_Money > 0)
            {
                bool b = checktotalmorgag();
                if (b == true)
                {
                    int count = 0;
                    int total = 0;
                    while (_CardsOwend[count] != null)
                    {
                        if (_CardsOwend[count] is Card && _CardsOwend[count].GetIsMortageged() == false)
                        {
                            total = _CardsOwend[count].GetMortgagePrice();
                            _CardsOwend[count].setIsMortageged(true);
                        }

                        count = count + 1;
                    }
                    _Money = _Money + total;
                }
                if (b == false)
                {
                    _IsAilve = false;
                }
            }
        }
        //this make the algorithm Move
        public override void MovePlayer()
        {
            base.MovePlayer();
            int count = 0;
            while (_CardsOwend[count] != null)
            {
                if (_CardsOwend[count] is Card && _CardsOwend[count].GetIsMortageged() == true)
                {
                    int num = _CardsOwend[count].GetMortgageCost();
                    if ( _Money == num)
                    {
                        base.OnMortgageCard(_CardsOwend[count]);
                    }
                }

                count = count + 1;
            }
        }

    }
}
