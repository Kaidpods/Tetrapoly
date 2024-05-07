using System;

public class Player
{
	public Player()
	{
        protected bool _IsAilve;
        protected bool _InJail;
        protected bool _IsBankrupt;
        protected string _Name;
        protected int _Money;
        protected int _Position;
        protected List<card> _CardsOwend;
        public Player(bool isialve, string name, int money, int position, int cardowend, bool isailve, bool injail,bool isbankrupt)
        {
            _IsAilve = isailve;
            _Name = name;
            _Money = money;
            _Position = position;
            _CardsOwend.Add(cardowend);
            _IsAilve = isailve;
            _InJail =injail;
            _IsBankrupt=isbankrupt;
        }
        public bool CheckSet(card check)
        {
            bool t=false;
            if(check == _CardsOwend)
            {
                t= true;
            }
            return t;
        }
        public void AddHouse(property pro,int whichpro) {
            _CardsOwend[whichpro].Porperty(pro);
        }
        public int RemoveHouse(property pro, int whichpro)
        {
            _CardsOwend[whichpro].Property(pro);
        }
        public void byCard(card CARD)
        {
            _CardsOwend.Add(CARD);            
        }
        public void SellCard(card CARD)
        {
            _CardsOwend.Remove(CARD);
        }
        public void MortgageCard(card  card)
        { 
        
        }
        public int RollDice()
        {

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
            
        }
        public bool CheckBankrupt()
        {
            
        }
        public bool CheckAilve()
        {
            
        }
    }
}
