using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AliveMapTool
{
    class InputManager
    {
        public static bool[] Keys = new bool[256];

        public static bool GetKeyDown(Keys key)
        {
            if (Keys[(int) key])
            {
                return true;
            }

            return false;
        }
    }
}
