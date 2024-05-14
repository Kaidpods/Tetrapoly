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
        public MainWindow()
        {
            InitializeComponent();
            Cards = database.GetProperties();

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
                         
                    }
                    t=false; 
                }
                count=count + 1;
            }
        }
        
    }
}