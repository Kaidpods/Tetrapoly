﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Windows.Automation.Peers;
namespace TetraPolyGame
{
    public class Player
    { 
	    protected bool _IsAilve;
        protected bool _InJail;
        protected string _Name;
        protected int _Money;
        protected int _Position;
        protected List<Card> _CardsOwend = [];
        //this is the constroctor for the Player
        public Player(string name, int money, int position, Card cardowend)
        {
            _Name = name;
            _Money = money;
            _Position = position;
            _CardsOwend.Add(cardowend);
            _IsAilve = true;
            _InJail = false;    
        }
        //check a is the card 
        public bool CheckSet(Card check)
        {
            bool t = false;
            int count = 0;
            while (_CardsOwend[count] != null)
            {
                if (_CardsOwend[count] is Card && _CardsOwend[count]== check)
                {
                    t = true;
                }
                count = count + 1;
            }           
            return t;
        }
        // it add a hous
        public void AddHouse(Property pro)
        {
            pro.AddHouse();
        }
        // it remove a hous
        public void RemoveHouse(Property pro, int whichpro,int mnyhosetoremove)
        {
            for(int x=0;x!= mnyhosetoremove;x++) 
            {
                pro.RemoveHouse(mnyhosetoremove);
            }
        }
        // it buy a card
        public void byCard(Card CARD)
        {
            _CardsOwend.Add(CARD);
        }
        // it sell a card
        public void SellCard(Card CARD)
        {
            _CardsOwend.Remove(CARD);
        }
        // it mortgage a card
        public void MortgageCard(Card card)
        {
            if (card.IsMortgaged() == false)
            {
                card.ToggleMorgaged();
                int mor = card.GetMortgagePrice();
                addMoney(mor);
            }
            
        }
        // it onmortgage a card
        public void OnMortgageCard(Card card)
        {
            if (card.IsMortgaged() == true)
            {
                int mor = card.GetMortgagePrice();
                LoseMoney(mor);
                card.ToggleMorgaged();
            }
        }
        // get a random nummber between 1 and 6
        public int RollDice()
        {
            Random d = new Random();
            int dn=d.Next(1,6);
            return dn;
        }
        //it move the player
        virtual public void MovePlayer()
        {
            if (_IsAilve == true)
            {
                if (_InJail == false)
                {
                    int count = 0;
                    bool b = true;
                    while (b = true)
                    {
                        int move = RollDice();
                        int move2 = RollDice();
                        _Position = _Position + move + move2;
                        if (move2 == move)
                        {
                            count = count + 1;
                            if (count != 3)
                            {
                                b = true;
                            }
                            else
                            {
                                _Position = -1;
                                _InJail = true;
                                b = false;
                            }
                        }
                        else
                        {
                            b=false;
                        }
                    }
                }
                if (_InJail == true)
                {
                    int r1 = RollDice();
                    int r2 = RollDice();
                    if (r1 == r2)
                    {
                        _InJail = false;
                        _Position = 9;
                    }
                }
            }
        }
        // buy a card
        virtual public void buy(bool chois,Card gc)
        {
            if (chois==true)
            { 
                int se = gc.GetPrice();
                LoseMoney(se);
                byCard(gc);
            }

        }
        //player the lose money
        virtual public void LoseMoney(int money)
        {
            _Money = _Money - money;
            if(_Money > 0){
                bool b=checktotalmorgag();
                if (b==true) 
                {
                    asktomortgage();
                }
                if (b==false)
                {
                    _IsAilve=false;
                }
            }
        }
        //can the player be save if the player morgage a property
        public bool checktotalmorgag()
        {
            int count = 0;
            int total = 0;
            while (_CardsOwend[count]!=null) 
            {
                if (_CardsOwend[count] is Card && _CardsOwend[count].IsMortgaged() == false)
                {
                    total=_CardsOwend[count].GetMortgagePrice(); 
                }
               
                count =count + 1;
            }
            if (total >= (_Money * -1))
            {
                return true;
            }else
            {
                return false;
            }
        }
        //add money
        public void addMoney(int money)
        {
            _Money += money;
        }
        //when you go pass go
        public void PassedGo()
        {
            if (_Position <= 40)
            {
                _Position = _Position - 40;
                addMoney(200);
            }
        }
        // get a Position
        public int GetPosition()
        {
            return _Position;
        }
        // get a Ailve
        public bool GetAilve()
        {
            return _IsAilve;
        }
        // get a money
        public int Getmoney()
        {
            return _Money;
        }
        // get a Cards
        public List<Card> GetCards()
        {
            return _CardsOwend;
        }
    }
}