using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BattleShip
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        Game newGame = new Game();

        public GameWindow()
        {
            InitializeComponent();


            newGame.showBoard(GameArea);


            marine_4_radioButton.Checked += new RoutedEventHandler(Deployer.getChosedUnit);
            marine_3_radioButton.Checked += new RoutedEventHandler(Deployer.getChosedUnit);
            marine_2_radioButton.Checked += new RoutedEventHandler(Deployer.getChosedUnit);
            marine_1_radioButton.Checked += new RoutedEventHandler(Deployer.getChosedUnit);
            land_4_radioButton.Checked += new RoutedEventHandler(Deployer.getChosedUnit);
            land_3_radioButton.Checked += new RoutedEventHandler(Deployer.getChosedUnit);
            land_2_radioButton.Checked += new RoutedEventHandler(Deployer.getChosedUnit);
            plane_radioButton.Checked += new RoutedEventHandler(Deployer.getChosedUnit);


        }


        private void deployRandom_button_Click(object sender, RoutedEventArgs e)
        {
            newGame.deployUnitRandom();
        }

        private void deployMyOwn_button_Click(object sender, RoutedEventArgs e)
        {
            newGame.deployUnits();
        }

        private void readFromFile_button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not supported");
        }


        private void play_button_Click(object sender, RoutedEventArgs e)
        {
            deployRandom_button.Click -= deployRandom_button_Click;
            deployMyOwn_button.Click -= deployMyOwn_button_Click;
            readFromFile_button.Click -= readFromFile_button_Click;

            newGame.play();
        }

        public void a()
        {
            deployRandom_button.Click -= deployRandom_button_Click;

        }

        
    }


    public class Game
    {
        private Player player1;
        private Player player2;
        private Play match;
        
        public Game()
        {
            player1 = new HumanPlayer();
            player2 = new ComputerPlayer();
        }


        public void showBoard(Grid gameArea)
        {
            gameArea.Children.Add(player1.playerBoard.boardView);
            gameArea.Children.Add(player1.fireBoard.boardView);
        }

        public void deployUnits()
        {
            Deployer deployer = new Deployer(player1.playerBoard, player1.unitList);
        }

        public void deployUnitRandom()
        {
            Deployer deployer = new Deployer(player1.playerBoard, player1.unitList);
            deployer.deployRandom();
        }


        public void play()
        {
            match = new Play(player1, player2);
            match.start();
        }

    }




    class Play
    {
        private static Player player1;
        private static Player player2;
        private static Random rand = new Random();
        private static int shotFired = 0;

        private static List<int> shot = new List<int>();

        public Play(Player p1, Player p2)
        {
            player1 = p1;
            player2 = p2;
        }

        public void start()
        {
            Deployer deployer = new Deployer(player2.playerBoard, player2.unitList);
            deployer.deployRandom();

            MessageBox.Show("You first");
        }
  


        public static void fire(object sender, EventArgs e)
        {
            Rectangle firedField = (Rectangle)sender;
            string rectName = firedField.Name.ToString();

            int x = Int32.Parse(rectName.Substring(1, rectName.IndexOf('y') - 1));
            int y = Int32.Parse(rectName.Substring(rectName.IndexOf('y') + 1));

            if(player2.playerBoard.board[x,y].Fill == Config._placedUnitColor_)
            {
                player1.fireBoard.board[x, y].Fill = Config._hitColor_;
            }
            else
            {
                player1.fireBoard.board[x, y].Fill = Config._missedColor_;           
            }
            shotFired++;
            
            
            computerFire();
        }



        private static void computerFire()
        {
            int x = rand.Next(Config._boardWidth_);
            int y = rand.Next(Config._boardHeight_);

            while (shot.Contains(x))
            {
                x = rand.Next(Config._boardWidth_);
                y = rand.Next(Config._boardHeight_);
            }
            shot.Add(x);

            if(player1.playerBoard.board[x,y].Fill == Config._marineUnitColor_ || player1.playerBoard.board[x, y].Fill == Config._landUnitColor_)
            {
                player1.playerBoard.board[x, y].Fill = Config._hitColor_;
            }
        }
    }
    
    
}
