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

        private void deployMyOwn_button_Click(object sender, RoutedEventArgs e)
        {
            newGame.deployUnits();
        }

        private void deployRandom_button_Click(object sender, RoutedEventArgs e)
        {
            newGame.test(); 
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            newGame.play();
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

        public void test()
        {

        }


        public void play()
        {
            match = new Play();
        }

    }



    class Play
    {
        /*
         * delete Deployer
         * lock deploy option
         */

    }
    
    



    


    


}
