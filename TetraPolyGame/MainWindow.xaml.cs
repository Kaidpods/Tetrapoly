using System.Collections;
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
                if (pos == Cards[count].)
                {
                    int cut = 0;
                    while (Players[cut]!=null)
                    {
                        bool onby = Players[cut].CheckSet(Cards[count]);
                        if (onby==true) 
                        {
                            if (Players[cut] != Players[turn])
                            {

                            }
                            if (Players[cut] == Players[turn]) 
                            {
                                
                            }
                        }
                        cut = cut + 1;
                    }
                    t=false; 
                }
                count=count + 1;
            }
        }
        public bool doyouwanttobuy() 
        { 
            return true; 
        }
    }
}