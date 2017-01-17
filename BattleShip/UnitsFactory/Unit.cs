using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip
{
    interface Unit
    {
        LinkedList<System.Drawing.Point> coordinates { get; set; }
    }



    class MarineUnit : Unit
    {
        public MarineUnit(LinkedList<System.Drawing.Point> coordinates)
        {
            this.coordinates = new LinkedList<System.Drawing.Point>();
            foreach(var i in coordinates)
            {
                this.coordinates.AddLast(i);
            }        
        }

        public LinkedList<System.Drawing.Point> coordinates { get; set; }

    }

    class LandUnit : Unit
    {
        public LandUnit(LinkedList<System.Drawing.Point> coordinates)
        {
            this.coordinates = coordinates;
        }

        public LinkedList<System.Drawing.Point> coordinates { get; set; }

    }

    class Plane : Unit
    {
        public Plane(LinkedList<System.Drawing.Point> coordinates)
        {
            this.coordinates = coordinates;
        }

        public LinkedList<System.Drawing.Point> coordinates { get; set; }
    }

}
