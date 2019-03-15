using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AliveMapTool
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void tileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateTileForm form = new CreateTileForm();
            form.Location = UtilManager.GetFormCenter(form.Size);
            form.Show();
        }

        private void iTalk_ThemeContainer1_DragDrop(object sender, DragEventArgs e)
        {
            SettingSpriteForm form = new SettingSpriteForm();

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] file = (string[])e.Data.GetData(DataFormats.FileDrop);

                form.TextureIndex = new string[file.Length];

                for (int i = 0; i < file.Length; i++)
                {
                    form.TextureIndex[i] = "IMAGE" + TextureManager.TextureMap.Count;
                    if (TextureManager.AddTexture(form.TextureIndex[i], file[i]))
                    {
                        
                        form.Show();
                        form.Location = UtilManager.GetFormCenter(form.Size);
                        form.TopLevel = true;
                    }
                }
            }
        }

        private void iTalk_ThemeContainer1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.All;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
    }
}