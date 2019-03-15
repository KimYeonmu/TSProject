using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.DirectX.Direct3D;

namespace AliveMapTool
{
    public partial class SettingSpriteForm : Form
    {
        public string[] TextureIndex;

        public SettingSpriteForm()
        {
            InitializeComponent();
        }

        private void iTalk_Button_11_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < TextureIndex.Length; i++)
            {
                Sprite2D sprite = new Sprite2D(TextureIndex[i], Convert.ToInt32(AnimationTextBox_X.Text),
                    Convert.ToInt32(AnimationTextBox_Y.Text));
                SpriteManager.SpriteList.Add(sprite);
            }

            Close();
        }
    }
}
