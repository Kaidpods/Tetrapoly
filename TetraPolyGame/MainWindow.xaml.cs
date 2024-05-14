using System.Collections;
using System.Printing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using MessageBox = System.Windows.Forms.MessageBox;

namespace TetraPolyGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MSSQLdataAccess database = new();
        private List<Card> Cards = new();
        protected List<Player> Players = new();
        private int truncount = 0;
        public MainWindow()
        {
            InitializeComponent();
            Cards = database.GetProperties();

        }
        public void setPlayers(Player p)
        {
            Players.Add(p);
        }
        public void turnorder()
        {
            bool t=true;
            while (t == true)
            {
                if (Players[truncount] == null)
                {
                    truncount = 0;
                }
                Players[truncount].MovePlayer();
                checkposition(truncount);
                t=Onlyoneleft();
                truncount = truncount + 1;
            }
        }
        public bool Onlyoneleft()
        {
            bool b = true;
            int c = 0;
            int count = 0;
            while (Players[c] == null)
            {
                if (Players[c].GetAilve() == true)
                {
                    count=count + 1;
                }
            }
            if(count < 2)
            {
                b= false;
            }else if (count > 1)
            {
                b=true;
            }
            return b;
        }
        public void checkposition(int turn)
        {
            bool t=true;
            int count = 0;
            int pos=Players[turn].GetPosition();
            while (t==true) 
            {
                if (pos == Cards[count].GetPosition())
                {
                    int cut = 0;
                    while (Players[cut]!=null)
                    {
                        bool onby = Players[cut].CheckSet(Cards[count]);
                        if (onby == true)
                        {
                            if (Players[cut] != Players[turn])
                            {
                                int r = Cards[count].GetRent();
                                Players[cut].addMoney(r);
                                Players[turn].LoseMoney(r);
                            }
                            if ((Players[cut] == Players[turn]) && (Cards[count]is Property) && (Players[cut] is not algorithm))
                            {
                                Property tempProp = Cards[count] as Property;
                                DialogResult result = MessageBox.Show("Do you want to buy a house?", "House Buying", MessageBoxButtons.YesNo);
                                if (result == System.Windows.Forms.DialogResult.Yes)
                                {
                                    int cost = 0;
                                    switch (tempProp.GetColour())
                                    {
                                        case "BROWN": { cost = 50; break; }
                                        case "LBLUE": { cost = 50; break; }
                                        case "PINK": { cost = 50; break; }

                                        case "ORANGE": { cost = 100; break; }
                                        case "RED": { cost = 100; break; }

                                        case "YELLOW": { cost = 150; break; }
                                        case "GREEN": { cost = 150; break; }

                                        case "DBLUE": { cost = 200; break; }

                                    }
                                    Players[cut].LoseMoney(cost);
                                    Property pro = Cards[count] as Property;
                                    Players[cut].AddHouse(pro);
                                }
                            }
                            else if((Players[cut] == Players[turn]) && (Cards[count] is Property))
                            { 
                                Property tempProp = Cards[count] as Property;
                                int cost = 0;
                                switch (tempProp.GetColour())
                                {
                                    case "BROWN": { cost = 50; break; }
                                    case "LBLUE": { cost = 50; break; }
                                    case "PINK": { cost = 50; break; }

                                    case "ORANGE": { cost = 100; break; }
                                    case "RED": { cost = 100; break; }

                                    case "YELLOW": { cost = 150; break; }
                                    case "GREEN": { cost = 150; break; }

                                    case "DBLUE": { cost = 200; break; }

                                }
                                Players[cut].LoseMoney(cost);
                                Property pro = Cards[count] as Property;
                                Players[cut].AddHouse(pro);
                            }
                        }
                        cut = cut + 1;
                    }
                    
                    if (null == Cards[count].IsOwned)
                    {
                        if ((Cards[count] is Property) || (Cards[count] is Transport) || (Cards[count] is Utility) && (Players[cut] is not algorithm))
                        {
                            DialogResult result = MessageBox.Show("Do you want to buy a house?", "House Buying", MessageBoxButtons.YesNo);
                            if (result == System.Windows.Forms.DialogResult.Yes)
                            {
                                Players[turn].buy(true, Cards[count]);
                            }
                            else
                            {
                                Players[turn].buy(false, Cards[count]);
                            }
                        }else if((Cards[count] is Property) || (Cards[count] is Transport) || (Cards[count] is Utility))
                        {
                            Players[turn].buy(true, Cards[count]);
                        }
                        if ((Players[turn].GetPosition()==2)||(Players[turn].GetPosition() == 33) || (Players[turn].GetPosition() == 28))
                        {
                            Community.getcard();
                        }
                        if ((Players[turn].GetPosition() == 7) || (Players[turn].GetPosition() == 22) || (Players[turn].GetPosition() == 36))
                        {
                            Chance.getcard();
                        }
                        if (Players[turn].GetPosition() == 30)
                        {
                            Players[turn].SetInJaile(true);
                            Players[turn].setPosition(-1);
                        }
                    }
                    t=false; 
                }
                count=count + 1;
            }
        }
        
    }
}