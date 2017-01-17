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
    sealed class Config
    {
        public static readonly int _unitSegmentSize_ = 20;

        public static readonly string[] _unitTypes_ = { "PLANE", "MARINE", "LAND" };

        public static readonly int _boardWidth_ = 22;
        public static readonly int _boardHeight_ = 14;

        public static readonly int _boardWidthPixels_ = _boardWidth_ * _unitSegmentSize_;
        public static readonly int _boardHeightPixels_ = _boardHeight_ * _unitSegmentSize_;


        public static readonly int _maxSegmentsMarineUnit_ = (_boardWidth_ * _boardHeight_) / 4;
        public static readonly int _maxSegmentsLandUnit_ = (_boardWidth_ * _boardHeight_) / 4;


        //--> COLORS
        public static readonly SolidColorBrush _marineAreaColor_ = new SolidColorBrush(Color.FromRgb(26, 87, 186));
        public static readonly SolidColorBrush _landAreaColor_ = new SolidColorBrush(Color.FromRgb(1, 119, 17));
        public static readonly SolidColorBrush _areaColor_ = new SolidColorBrush(Color.FromRgb(170, 170, 170));

        public static readonly SolidColorBrush _marineUnitColor_ = new SolidColorBrush(Color.FromRgb(65, 71, 65));
        public static readonly SolidColorBrush _landUnitColor_ = new SolidColorBrush(Color.FromRgb(25, 56, 31));
        public static readonly SolidColorBrush _planeUnitColor_ = new SolidColorBrush(Color.FromRgb(221, 221, 221));

        public static readonly SolidColorBrush _hitColor_ = new SolidColorBrush(Color.FromRgb(6, 201, 48));
        public static readonly SolidColorBrush _missedColor_ = new SolidColorBrush(Color.FromRgb(191, 51, 51));

        public static readonly SolidColorBrush _mouseEnterSegmentColor_ = new SolidColorBrush(Color.FromRgb(255, 255, 0));


    }
}
