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
    abstract class Player
    {
        public List<Unit> unitList;

        public Board playerBoard;
        public Board fireBoard;
        
    }


    class HumanPlayer : Player
    {
        public HumanPlayer()
        {
            playerBoard = new PlayerBoard();
            fireBoard = new FireBoard();

            playerBoard.generateBoardView();
            fireBoard.generateBoardView();

            unitList = new List<Unit>();
        }     
    }



    class ComputerPlayer : Player
    {

        public ComputerPlayer()
        {
            playerBoard = new PlayerBoard();
            fireBoard = new FireBoard();

            unitList = new List<Unit>();
        }
    }

}
