using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AliveMapTool
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>

        [STAThread]
        

        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            MainGame mainGame;

            using (mainGame = new MainGame())
            {
                if (mainGame.Init())
                {
                    mainGame.RenderForm.Show();

                    while (mainGame.RenderForm.Created)
                    {
                        mainGame.Render();
                        mainGame.Update();

                        Thread.Sleep(1);

                        Application.DoEvents();
                    }
                }

            }
        }
    }
}
