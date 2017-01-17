using System;
using System.Collections.Generic;
using System.Drawing;
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
    class Deployer
    {
        private static System.Windows.Shapes.Rectangle[,] playerBoard;
        private static List<Unit> unitList;
        private static LinkedList<System.Drawing.Point> coordinates = new LinkedList<System.Drawing.Point>();
        private static bool deployMyOwn = false;

        public Deployer(Board board, List<Unit> _unitList)
        {
            playerBoard = board.board;
            unitList = _unitList;

            deployMyOwn = true;
        }


        private static string unitType;
        private static int unitSegments;

        public static void getChosedUnit(object sender, EventArgs e)
        {
            if (!deployMyOwn)
            {
                MessageBox.Show("Choose deploy way!");
                return;
            }

            RadioButton radioButton = (RadioButton)sender;
            string chosenUnit = radioButton.Content.ToString();

            unitType = chosenUnit.Substring(0, chosenUnit.ToString().Length - 1);
            unitSegments = Int32.Parse(chosenUnit.Substring(chosenUnit.Length - 1));
        }


        private static List<System.Windows.Media.Brush> mouseOverTmp = new List<System.Windows.Media.Brush>();
        public static void mouseEnterDeploy(object sender, EventArgs e)
        {
            if (unitType != null)
            {
                System.Windows.Shapes.Rectangle rect = (System.Windows.Shapes.Rectangle)sender;
                string rectName = rect.Name.ToString();

                int x = Int32.Parse(rectName.Substring(1, rectName.IndexOf('y') - 1));
                int y = Int32.Parse(rectName.Substring(rectName.IndexOf('y') + 1));

                SolidColorBrush currentUnitColor;
                if (unitType.Equals("LAND"))
                {
                    currentUnitColor = Config._landUnitColor_;
                }
                else if (unitType.Equals("MARINE"))
                {
                    currentUnitColor = Config._marineUnitColor_;
                }
                else
                {
                    currentUnitColor = Config._planeUnitColor_;
                }


                for (int xi = x, i = 0; i < unitSegments; xi++, i++)
                {
                    try
                    {
                        mouseOverTmp.Add(playerBoard[xi, y].Fill);
                        playerBoard[xi, y].Fill = currentUnitColor;
                    }
                    catch
                    {
                        return;
                    }
                }
            }
        }

        public static void mouseLeaveDeploy(object sender, EventArgs e)
        {
            if (unitType != null && mouseOverTmp.Count > 0)
            {
                System.Windows.Shapes.Rectangle rect = (System.Windows.Shapes.Rectangle)sender;
                string rectName = rect.Name.ToString();

                int x = Int32.Parse(rectName.Substring(1, rectName.IndexOf('y') - 1));
                int y = Int32.Parse(rectName.Substring(rectName.IndexOf('y') + 1));

                for (int xi = x, i = 0; i < unitSegments; xi++, i++)
                {
                    try
                    {
                        playerBoard[xi, y].Fill = mouseOverTmp[i];
                    }
                    catch
                    {
                        return;
                    }
                }
                mouseOverTmp.Clear();
            }
        }



        public static bool deployed = false;
        public static void placeUnit(object sender, EventArgs e)
        {
            if (!deployMyOwn)
            {
                MessageBox.Show("Choose deploy way!");
                return;
            }
            if (unitType == null)
            {
                MessageBox.Show("Choose unit type!");
                return;
            }

            System.Windows.Shapes.Rectangle rect = (System.Windows.Shapes.Rectangle)sender;
            string rectName = rect.Name.ToString();

            bool isLandArea = mouseOverTmp[0].Equals(Config._landAreaColor_);
            bool isMarineArea = mouseOverTmp[0].Equals(Config._marineAreaColor_);
            
            bool isLandUnit = unitType.Equals("LAND", StringComparison.CurrentCultureIgnoreCase);
            bool isMarineUnit = unitType.Equals("MARINE", StringComparison.CurrentCultureIgnoreCase);
            bool isPlaneUnit = unitType.Equals("PLANE", StringComparison.CurrentCultureIgnoreCase);


            int x = Int32.Parse(rectName.Substring(1, rectName.IndexOf('y') - 1));
            int y = Int32.Parse(rectName.Substring(rectName.IndexOf('y') + 1));


            

            if(isLandArea || isMarineArea)
            {
                //LAND---------------------------->
                if (isLandArea && isLandUnit)
                {
                    bool allowDeploy = true;
                    deployed = true;

                    for (int i = 0; i < unitSegments; i++)
                    {
                        try
                        {
                            if(mouseOverTmp[i] == Config._marineAreaColor_)
                            {
                                allowDeploy = false;
                                MessageBox.Show("You can't deploy LAND Unit on the water!");
                                return;
                            }
                            if (mouseOverTmp[i] != Config._landAreaColor_)
                            {
                                allowDeploy = false;
                                MessageBox.Show("Units can't contact each other!");
                                return;
                            }
                        }
                        catch
                        {
                            allowDeploy = false;
                        }
                    }

                    try
                    {
                        if (playerBoard[x - 1, y].Fill != Config._landAreaColor_)
                        {
                            allowDeploy = false;
                            MessageBox.Show("Units can't contact each other!");
                            return;
                        }
                    }
                    catch { }

                    if (playerBoard[x + unitSegments, y].Fill != Config._landAreaColor_ && playerBoard[x + unitSegments, y].Fill != Config._marineAreaColor_)
                    {
                        allowDeploy = false;
                        MessageBox.Show("Units can't contact each other!");
                        return;
                    }
                    


                    for (int xi = x, i = 0; i < unitSegments; xi++, i++)
                    {
                        try
                        {
                            if (playerBoard[xi, y - 1].Fill != Config._landAreaColor_)
                            {
                                allowDeploy = false;
                                MessageBox.Show("Units can't contact each other!");
                                return;
                            }
                        }
                        catch { }

                        try
                        {
                            if (playerBoard[xi, y + 1].Fill != Config._landAreaColor_)
                            {
                                allowDeploy = false;
                                MessageBox.Show("Units can't contact each other!");
                                return;
                            }
                        }
                        catch { }
                    }


                    if (allowDeploy)
                    {
                        for (int xi = x, i = 0; i < unitSegments; xi++, i++)
                        {
                            playerBoard[xi, y].Fill = Config._landUnitColor_;
                            coordinates.AddLast(new System.Drawing.Point(xi, y));
                        }
                        unitList.Add(UnitFactory.getUnit(unitType, coordinates));
                        mouseOverTmp.Clear();
                    }
                    coordinates.Clear();
                }
                else if(isLandArea && isMarineUnit)
                {
                    MessageBox.Show("You can't deploy LAND Unit on the water!");
                    return;
                }





                //MARINE---------------------------->
                else if (isMarineArea && isMarineUnit)
                {
                    bool allowDeploy = true;
                    deployed = true;

                    for (int i=0; i < unitSegments; i++)
                    {
                        try
                        {
                            if(mouseOverTmp[i] != Config._marineAreaColor_)
                            {
                                allowDeploy = false;
                                MessageBox.Show("Units can't contact each other!");
                                return;
                            }
                        }
                        catch
                        {
                            allowDeploy = false;
                        }
                    }

                    if (playerBoard[x - 1, y].Fill != Config._marineAreaColor_ && playerBoard[x - 1, y].Fill != Config._landAreaColor_)
                    {
                        allowDeploy = false;
                        MessageBox.Show("Units can't contact each other!");
                        return;
                    }
                    try
                    {
                        if(playerBoard[x + unitSegments, y].Fill != Config._marineAreaColor_ )
                        {
                            allowDeploy = false;
                            MessageBox.Show("Units can't contact each other!");
                            return;
                        }
                    }
                    catch { }

                    for (int xi = x, i = 0; i < unitSegments; xi++, i++)
                    {
                        try
                        {
                            if(playerBoard[xi, y-1].Fill != Config._marineAreaColor_)
                            {
                                allowDeploy = false;
                                MessageBox.Show("Units can't contact each other!");
                                return;
                            }                            
                        }
                        catch { }
                       
                        try
                        {
                            if (playerBoard[xi, y + 1].Fill != Config._marineAreaColor_)
                            {
                                allowDeploy = false;
                                MessageBox.Show("Units can't contact each other!");
                                return;
                            }
                        }
                        catch { }
                    }



                    if (allowDeploy)
                    {
                        for (int xi = x, i = 0; i < unitSegments; xi++, i++)
                        {
                            playerBoard[xi, y].Fill = Config._marineUnitColor_;
                            coordinates.AddLast(new System.Drawing.Point(xi, y));
                        }
                        unitList.Add(UnitFactory.getUnit(unitType, coordinates));
                        mouseOverTmp.Clear();
                    }
                    coordinates.Clear();                    
                }
                else if(isMarineArea && isLandUnit)
                {
                    MessageBox.Show("You can't deploy MARINE Unit on the land!");
                    return;
                }



                //PLANE---------------------------->
                else if (isPlaneUnit)
                {
                    playerBoard[x, y].Fill = Config._planeUnitColor_;
                }
            }
            else
            {
                MessageBox.Show("Field is not empty!");
            }
        }







        public void deployRandom()
        {
            Random rand = new Random();
            bool allowDeploy = true;
            int numberOfTries = 0;

            bool isLandUnit = false;
            bool isMarineUnit = false;

            int maxX = playerBoard.GetLength(0);
            int maxY = playerBoard.GetLength(1);

            int randX = 0;
            int randY = 0;


            while (numberOfTries < 10)
            {
                randY = rand.Next(maxY);
                unitType = Config._unitTypes_[rand.Next(Config._unitTypes_.Length)];
                //isLandUnit = false;
                //isMarineUnit = false;

                if (unitType == "LAND")
                {
                    unitSegments = rand.Next(2, 5);
                    randX = rand.Next(maxX / 2);
                    isLandUnit = true;
                }
                else if (unitType == "MARINE")
                {
                    unitSegments = rand.Next(1, 5);
                    randX = rand.Next(maxX / 2, maxX);
                    isMarineUnit = true;
                }


                while (true)
                {
                    List<System.Drawing.Point> currentUnitsPosition = new List<System.Drawing.Point>();
                    if (unitList.Count > 0)
                    {
                        currentUnitsPosition.Clear();
                        foreach (var unit in unitList)
                        {
                            foreach (var position in unit.coordinates)
                            {
                                currentUnitsPosition.Add(position);
                            }
                        }
                    }
                    else
                    {
                        if ((randX + unitSegments) - 1 < maxX)
                        {
                            if ((isLandUnit && randX < (maxX / 2)) || (isMarineUnit && randX > maxX / 2))
                            {
                                for (int xi = randX, i = 0; i < unitSegments; xi++, i++)
                                {
                                    coordinates.AddLast(new System.Drawing.Point(xi, randY));
                                }
                                unitList.Add(UnitFactory.getUnit(unitType, coordinates));

                                coordinates.Clear();
                            }
                        }
                        else
                        {
                            numberOfTries++;
                            break;
                        }
                    }



                    if (isLandUnit)
                    {
                        if (randX < maxX / 2)
                        {
                            if ((randX + unitSegments) - 1 > maxX / 2)
                            {
                                numberOfTries++;
                                break;
                            }
                            foreach (var position in currentUnitsPosition)
                            {
                                if (randX == position.X && randY == position.Y)
                                {
                                    numberOfTries++;
                                    break;
                                }
                                if (randX == position.X - 1 && randY == position.Y)
                                {
                                    numberOfTries++;
                                    break;
                                }
                                if (randX == position.X + 1 && randY == position.Y)
                                {
                                    numberOfTries++;
                                    break;
                                }
                                if (randX == position.X && randY == position.Y - 1)
                                {
                                    numberOfTries++;
                                    break;
                                }
                                if (randX == position.X && randY == position.Y + 1)
                                {
                                    numberOfTries++;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            numberOfTries++;
                            break;
                        }
                    }


                    else if (isMarineUnit)
                    {
                        if (randX > maxX / 2)
                        {
                            if ((randX + unitSegments) - 1 > maxX)
                            {
                                numberOfTries++;
                                break;
                            }
                            foreach (var position in currentUnitsPosition)
                            {
                                if (randX == position.X && randY == position.Y)
                                {
                                    numberOfTries++;
                                    break;
                                }
                                if (randX == position.X - 1 && randY == position.Y)
                                {
                                    numberOfTries++;
                                    break;
                                }
                                if (randX == position.X + 1 && randY == position.Y)
                                {
                                    numberOfTries++;
                                    break;
                                }
                                if (randX == position.X && randY == position.Y - 1)
                                {
                                    numberOfTries++;
                                    break;
                                }
                                if (randX == position.X && randY == position.Y + 1)
                                {
                                    numberOfTries++;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            numberOfTries++;
                            break;
                        }
                    }


                    if (allowDeploy)
                    {
                        for (int xi = randX, i = 0; i < unitSegments; xi++, i++)
                        {
                            playerBoard[xi, randY].Fill = Config._placedUnitColor_;
                            coordinates.AddLast(new System.Drawing.Point(xi, randY));
                        }
                        unitList.Add(UnitFactory.getUnit(unitType, coordinates));

                        coordinates.Clear();
                        numberOfTries = 0;
                        break;
                    }
                }
            }
        }

    }
}
