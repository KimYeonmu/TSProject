using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace AliveMapTool
{
    class Camera
    {
        private Vector3 _cameraPosition = new Vector3(0,0,-10);
        private Vector3 _cameraTarget = new Vector3(0,0,0);
        private float _cameraSpeed = 0.1f;

        public Camera()
        {
            Direct3D.D3DDevice.Transform.Projection = Matrix.PerspectiveFovLH(
                Geometry.DegreeToRadian(45),
                (float) Direct3D.D3DDevice.Viewport.Width / (float) Direct3D.D3DDevice.Viewport.Height,
                1.0f, 1000);
        }

        public void Update()
        {
            KeyInput();

            Direct3D.D3DDevice.Transform.View = Matrix.LookAtLH(
                _cameraPosition, _cameraTarget, new Vector3(0, 1, 0));
        }

        public void KeyInput()
        {
            if (InputManager.GetKeyDown(Keys.W))
            {
                _cameraPosition.Y += _cameraSpeed;
                _cameraTarget.Y += _cameraSpeed;
            }

            if (InputManager.GetKeyDown(Keys.S))
            {
                _cameraPosition.Y -= _cameraSpeed;
                _cameraTarget.Y -= _cameraSpeed;
            }

            if (InputManager.GetKeyDown(Keys.A))
            {
                _cameraPosition.X -= _cameraSpeed;
                _cameraTarget.X -= _cameraSpeed;
            }

            if (InputManager.GetKeyDown(Keys.D))
            {
                _cameraPosition.X += _cameraSpeed;
                _cameraTarget.X += _cameraSpeed;
            }
        }

        public void MouseWheelInput(float Delta)
        {
            _cameraPosition.Z += Delta / 100;
            _cameraTarget.Z += Delta / 100;
        }
    }
}
