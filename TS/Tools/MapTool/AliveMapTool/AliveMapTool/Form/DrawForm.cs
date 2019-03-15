using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTalk;

namespace AliveMapTool
{
    public partial class DrawForm : System.Windows.Forms.Form
    {
        public DrawForm()
        {
            InitializeComponent();

        }

        private void DrawForm_KeyDown(object sender, KeyEventArgs e)
        {
            if ((int) e.KeyCode < InputManager.Keys.Length)
            {
                InputManager.Keys[(int) e.KeyCode] = true;
            }
        }

        private void DrawForm_KeyUp(object sender, KeyEventArgs e)
        {
            if ((int)e.KeyCode < InputManager.Keys.Length)
            {
                InputManager.Keys[(int)e.KeyCode] = false;
            }
        }

        private void DrawForm_MouseWheel(object sender, MouseEventArgs e)
        {
            MainGame.MainCamera.MouseWheelInput(e.Delta);
        }
    }
}
