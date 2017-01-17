using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip
{
    class UnitFactory
    {
        public static Unit getUnit(String type, LinkedList<System.Drawing.Point> coordinates)
        {
            if(type == null)
            {
                throw new System.ArgumentNullException("Null type of Unit");
            }
            if (type.Equals("PLANE", StringComparison.CurrentCultureIgnoreCase)) {
                return new Plane(coordinates);
            }
            if (type.Equals("MARINE", StringComparison.CurrentCultureIgnoreCase)) {
                return new MarineUnit(coordinates);
            }
            if (type.Equals("LAND", StringComparison.CurrentCultureIgnoreCase)) {
                return new LandUnit(coordinates);
            }
            return null;
        }
    }

}
