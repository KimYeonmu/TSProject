using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace AliveMapTool
{
    class Sprite2D
    {
        private Sprite _sprite;
        private Texture2D _texture;
        private Rectangle _imageRect;
        private Point _animationFrame;
        private double _nowFrame;
        private Microsoft.DirectX.Direct3D.Font _font = null;

        public Sprite2D(string strKey, int animationX, int animationY)
        {
            _texture = TextureManager.TextureMap[strKey];
            _sprite = new Sprite(Direct3D.D3DDevice);
            _imageRect = new Rectangle(0, 0, _texture.ImageInfo.Width / animationX, _texture.ImageInfo.Height / animationY);
            _animationFrame = new Point(animationX, animationY);

            FontDescription fd = new FontDescription();
            fd.Height = 24;
            fd.FaceName = "맑은 고딕";
            _font = new Microsoft.DirectX.Direct3D.Font(Direct3D.D3DDevice, fd);
        }

        public void Render()
        {
            _imageRect.X = (int)(_nowFrame) * _texture.ImageInfo.Width / _animationFrame.X;


            _sprite.Begin(SpriteFlags.AlphaBlend | SpriteFlags.SortTexture);
            _sprite.Draw(_texture.TextureImg, _imageRect, new Vector3(0,0,0), new Vector3(5, 5, 0), Color.White.ToArgb());
            _font.DrawText(null, "now frame : " + _nowFrame, 10, 10, Color.White);
            _sprite.End();
        }

        public void Update(float dt)
        {
            _nowFrame += dt;

            if (_nowFrame > _animationFrame.X)
            {
                _nowFrame = 0;
            }
        }

        public void Release()
        {
            if (_sprite != null) _sprite.Dispose();
        }
    }
}