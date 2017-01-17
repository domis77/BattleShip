using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace BattleShip
{
    abstract class Board
    {
        public Rectangle[,] board;
        public Grid boardView;

        protected HorizontalAlignment horizontalAlignment;
        private Brush currentFillColor;
        

        abstract protected void fillboard();


        public void generateBoardView()
        {
            boardView = new Grid();
            boardView.Width = Config._boardWidthPixels_;
            boardView.Height = Config._boardHeightPixels_;
            boardView.HorizontalAlignment = horizontalAlignment;
            boardView.VerticalAlignment = VerticalAlignment.Top;




            for (int x = 0; x < Config._boardWidth_; x++)
            {
                boardView.ColumnDefinitions.Add(new ColumnDefinition());

                for (int y = 0; y < board.GetLength(1); y++)
                {
                    Grid.SetColumn(board[x, y], x);
                }
            }
            for (int y = 0; y < Config._boardHeight_; y++)
            {
                boardView.RowDefinitions.Add(new RowDefinition());

                for (int x = 0; x < board.GetLength(0); x++)
                {
                    Grid.SetRow(board[x, y], y);
                }
            }

            for (int x = 0; x < board.GetLength(0); x++)
            {
                for (int y = 0; y < board.GetLength(1); y++)
                {
                    boardView.Children.Add(board[x, y]);
                }
            }
        }


        protected void mouseEnterSegment(object sender, RoutedEventArgs e)
        {
            Rectangle segment = (Rectangle)sender;
            currentFillColor = segment.Stroke;
            segment.Stroke = Config._mouseEnterSegmentColor_;
        }

        protected void mouseLeaveSegment(object sender, RoutedEventArgs e)
        {
            Rectangle segment = (Rectangle)sender;
            segment.Stroke = currentFillColor;
        }
    }



    class PlayerBoard : Board
    {

        public PlayerBoard()
        {
            horizontalAlignment = HorizontalAlignment.Left;
            fillboard();
        }

        

        override protected void fillboard()
        {
            board = new Rectangle[Config._boardWidth_, Config._boardHeight_];

            for (int x = 0; x < board.GetLength(0); x++)
            {
                for (int y = 0; y < board.GetLength(1); y++)
                {
                    board[x, y] = new Rectangle();
                    board[x, y].Width = Config._unitSegmentSize_;
                    board[x, y].Height = Config._unitSegmentSize_;
                    board[x, y].Name = "x" + x.ToString() + "y" + y.ToString();

                    board[x, y].MouseEnter += mouseEnterSegment;
                    board[x, y].MouseEnter += Deployer.mouseEnterDeploy;

                    board[x, y].MouseLeave += mouseLeaveSegment;
                    board[x, y].MouseLeave += Deployer.mouseLeaveDeploy;

                    board[x, y].MouseLeftButtonDown += Deployer.placeUnit;

                    if (x < board.GetLength(0) / 2)
                    {
                        board[x, y].Fill = Config._landAreaColor_;
                        board[x, y].Stroke = Config._landAreaColor_;
                    }
                    else
                    {
                        board[x, y].Fill = Config._marineAreaColor_;
                        board[x, y].Stroke = Config._marineAreaColor_;
                    }
                }
            }
        }

    }


    class FireBoard : Board
    {
        public FireBoard()
        {
            horizontalAlignment = HorizontalAlignment.Right;
            fillboard();
        }

        override protected void fillboard()
        {
            board = new Rectangle[Config._boardWidth_, Config._boardHeight_];

            for (int x = 0; x < board.GetLength(0); x++)
            {
                for (int y = 0; y < board.GetLength(1); y++)
                {
                    board[x, y] = new Rectangle();
                    board[x, y].Width = Config._unitSegmentSize_;
                    board[x, y].Height = Config._unitSegmentSize_;

                    board[x, y].MouseLeftButtonDown += Play.fire;

                    board[x, y].Fill = Config._areaColor_;
                    board[x, y].Stroke = Config._areaColor_;
                    board[x, y].Name = "x" + x.ToString() + "y" + y.ToString();

                    board[x, y].MouseEnter += mouseEnterSegment;
                    board[x, y].MouseLeave += mouseLeaveSegment;
                }
            }
        }
    }
}
