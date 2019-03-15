using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.DirectX.Direct3D;


namespace AliveMapTool
{
    class MainGame : IDisposable
    {
        public MainForm EngineForm = new MainForm();
        public DrawForm RenderForm = new DrawForm();
        public ToolForm ToolForm = new ToolForm();

        public static Camera MainCamera;
        private static int _lastTime = Environment.TickCount;

        public bool Init()
        {
            EngineForm.iTalk_Panel1.Size = new Size(EngineForm.Size.Width-6, EngineForm.Height-7);

            RenderForm.TopLevel = false;
            RenderForm.Parent = EngineForm.iTalk_Panel1;
            RenderForm.Size = RenderForm.Parent.Size;
            EngineForm.iTalk_Panel1.Controls.Add(RenderForm);

            ToolForm.Size = new Size(ToolForm.Size.Width, EngineForm.Size.Height);
            ToolForm.Location = new Point(EngineForm.Size.Width+EngineForm.Location.X,0);

            UtilManager.MainFormLocation = EngineForm.Location;
            UtilManager.MainFormSize = EngineForm.Size;

            Direct3D.CreateDevice(RenderForm, true);

            MainCamera = new Camera();

            RenderForm.Show();
            EngineForm.Show();
            ToolForm.Show();
            return true;
        }

        
        public void Render()
        {
            Direct3D.D3DDevice.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.DarkSlateGray, 1.0f, 0);

            Direct3D.D3DDevice.BeginScene();

            for (int i = 0; i < SpriteManager.SpriteList.Count; i++)
                SpriteManager.SpriteList[i].Render();

            Direct3D.D3DDevice.EndScene();

            Direct3D.D3DDevice.Present();
        }

        public void Update()
        {
            int now = Environment.TickCount;
            float dt = (float)(now - _lastTime) / 1000; // / 1000
            _lastTime = now;

            for (int i = 0; i < SpriteManager.SpriteList.Count; i++)
                SpriteManager.SpriteList[i].Update(dt);

            MainCamera.Update();
        }

        public void Dispose()
        {
            for (int i = 0; i < SpriteManager.SpriteList.Count; i++)
                SpriteManager.SpriteList[i].Release();

            Direct3D.Release();
        }
    }
}
