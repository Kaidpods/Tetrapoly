using System;

namespace TetraPolyGame
{
	public class Player
	{
        protected bool _IsAilve;
        protected bool _InJail;
        protected bool _IsBankrupt;
        protected string _Name;
        protected int _Money;
        protected int _Position;
        protected List<Card> _CardsOwend;
        public Player(string name, int money, int position, List<Card> cardsOwned)
        {
            this._Name = name;
            this._Money = money;
            this._Position = position;
            this._CardsOwend= cardsOwned;
            _IsAilve = true;
            _InJail = false;
            _IsBankrupt = false;

        }
        //public bool CheckSet(Card check)
        //{
        //    bool t=false;
        //    if(check == _CardsOwend)
        //    {
        //        t= true;
        //    }
        //    return t;
        //}
        public void AddHouse(Property pro)
        {
            pro.AddHouse();
        }
        public void RemoveHouse(Property pro)
        {
            pro.RemoveHouse();
        }
        public void byCard(Card CARD)
        {
            _CardsOwend.Add(CARD);            
        }
        public void SellCard(Card CARD)
        {
            _CardsOwend.Remove(CARD);
        }
        public void MortgageCard(Card  card)
        { 
        
        }
        public int RollDice()
        {
            //temporary right now
            return 4;
        }
        public void MovePlayer()
        {
            
        }
        public void LoseMoney(int money)
        {
            
        }
        public void addMoney(int money)
        {

        }
        public void PassedGo()
        {
            
        }
        public int CheckPosition()
        {
            return _Position;
        }
        public bool CheckBankrupt()
        {
            return _IsBankrupt;
        }
        public bool CheckAilve()
        {
            return _IsAilve;
        }
    }
}
