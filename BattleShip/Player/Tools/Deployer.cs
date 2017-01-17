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


            LinkedList<System.Drawing.Point> coordinates = new LinkedList<System.Drawing.Point>();
            

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







        public static void deployRandom()
        {

        }
    }
}
