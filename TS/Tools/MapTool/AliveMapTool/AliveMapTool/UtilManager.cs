using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AliveMapTool
{
    class UtilManager
    {
        public static Point MainFormLocation;
        public static Size MainFormSize;

        public static Point GetFormCenter(Size childFormSize)
        {
            Point pt = new Point(
                MainFormLocation.X + MainFormSize.Width/2 - childFormSize.Width/2,
                MainFormLocation.Y + MainFormSize.Height/2 - childFormSize.Height/2);

            return pt;
        }

    }
}
