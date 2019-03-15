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
    class Sprite3D
    {
        private VertexBuffer _vertexBuffer;

        private Texture _spriteTexture;

        CustomVertex.PositionTextured[] vertices = new CustomVertex.PositionTextured[4];

        private float _nowFrame = 0;

        private Point _animationFrame;

        private PointF _spriteSize;

        public Sprite3D(string texKey, int animationX, int animationY)
        {
            _spriteTexture = TextureManager.TextureMap[texKey].TextureImg;
            _spriteSize = new PointF(TextureManager.TextureMap[texKey].ImageInfo.Width,
                TextureManager.TextureMap[texKey].ImageInfo.Height);

            _vertexBuffer = new VertexBuffer(typeof(CustomVertex.PositionTextured), 4,
                Direct3D.D3DDevice, Usage.None, CustomVertex.PositionTextured.Format, Pool.Managed);

            _animationFrame = new Point(animationX, animationY);

            vertices[0] = new CustomVertex.PositionTextured(0, _spriteSize.Y/200, 0.0f, 0.0f, 0.0f);
            vertices[1] = new CustomVertex.PositionTextured(_spriteSize.X/animationX/ 200, _spriteSize.Y/ 200, 0.0f, 1.0f / animationX, 0.0f);
            vertices[2] = new CustomVertex.PositionTextured(0.0f, 0.0f, 0.0f, 0.0f, 1.0f);
            vertices[3] = new CustomVertex.PositionTextured(_spriteSize.X/animationX/ 200, 0, 0.0f, 1.0f / animationX, 1.0f);

            using (GraphicsStream data = _vertexBuffer.Lock(0, 0, LockFlags.None))
            {
                data.Write(vertices);

                _vertexBuffer.Unlock();
            }

            Direct3D.D3DDevice.RenderState.Lighting = false;
            Direct3D.D3DDevice.RenderState.CullMode = Cull.None;
        }

        public void Update(float dt)
        {
            _nowFrame += dt * 8;

            if (_nowFrame >= _animationFrame.X)
                _nowFrame = 0;

            vertices[0].Tu = (float)1 / _animationFrame.X * (int)_nowFrame;
            vertices[2].Tu = (float)1 / _animationFrame.X * (int)_nowFrame;

            vertices[1].Tu = (float)1 / _animationFrame.X * (int)(_nowFrame + 1);
            vertices[3].Tu = (float)1 / _animationFrame.X * (int)(_nowFrame + 1);

            using (GraphicsStream data = _vertexBuffer.Lock(0, 0, LockFlags.None))
            {
                data.Write(vertices);

                _vertexBuffer.Unlock();
            }
        }

        public void Render()
        {
            //Direct3D.D3DDevice.SetRenderState(RenderStates.AlphaBlendEnable, true);
            //Direct3D.D3DDevice.SetRenderState(RenderStates.SourceBlend, (float)Blend.One);
            //Direct3D.D3DDevice.SetRenderState(RenderStates.DestinationBlend, (float)Blend.One);

            Direct3D.D3DDevice.SetTexture(0,_spriteTexture);
            Direct3D.D3DDevice.SetStreamSource(0, _vertexBuffer, 0);
            Direct3D.D3DDevice.VertexFormat = CustomVertex.PositionTextured.Format;
  
            Direct3D.D3DDevice.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);

            Direct3D.D3DDevice.SetRenderState(RenderStates.AlphaBlendEnable, false);
        }

        public void Release()
        {
            if (_vertexBuffer != null) _vertexBuffer.Dispose();
        }
    }
}
