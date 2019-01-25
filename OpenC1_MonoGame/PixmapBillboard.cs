using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using OpenC1.Parsers;
using OneAmEngine;

namespace OpenC1
{
    class PixmapBillboard
    {
        VertexPositionTexture[] _vertices;
        VertexBuffer _vertexBuffer;
        VertexDeclaration _vertexDeclaration;

        List<PixMap> _pixmaps;
        float _currentFrameTime;
        int _currentFrame;
        Vector3 _scale;
        Matrix _scaleMatrix;

        public PixmapBillboard(Vector2 scale, string filename)
        {
            _scale = new Vector3(scale, 1);
            CreateGeometry();
            _vertexDeclaration = new VertexDeclaration(VertexPositionTexture.VertexDeclaration.GetVertexElements());

            PixFile pix = new PixFile(filename);
            _pixmaps = pix.PixMaps;
        }

        public void BeginBatch()
        {
            //OneAmEngine.Engine.Device.Vertices[0].SetSource(_vertexBuffer, 0, VertexPositionTexture.SizeInBytes);
            //OneAmEngine.Engine.Device.VertexDeclaration = _vertexDeclaration;
            OneAmEngine.Engine.Device.SetVertexBuffer(_vertexBuffer);
            //OneAmEngine.Engine.CurrentEffect.CurrentTechnique.Passes[0].Begin();
        }

        public void EndBatch()
        {
            //OneAmEngine.Engine.CurrentEffect.CurrentTechnique.Passes[0].End();
        }

        public void Render(Vector3 position)
        {
            if (_pixmaps.Count == 0) return;

            Update();
            BeginBatch();

            Matrix world = Matrix.CreateScale(0.03f) * Matrix.CreateBillboard(position, OneAmEngine.Engine.Camera.Position, Vector3.Up, Vector3.Forward);

            BasicEffect2 effect = GameVars.CurrentEffect;
            effect.World = _scaleMatrix * world;
            effect.Texture = _pixmaps[_currentFrame].Texture;
            OneAmEngine.Engine.Device.RasterizerState.CullMode = CullMode.None;
            OneAmEngine.Engine.Device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
            EndBatch();
        }

        private void CreateGeometry()
        {
            Vector3 topLeftFront = new Vector3(-0.5f, 1.0f, 0.5f);
            Vector3 bottomLeftFront = new Vector3(-0.5f, 0.0f, 0.5f);
            Vector3 topRightFront = new Vector3(0.5f, 1.0f, 0.5f);
            Vector3 bottomRightFront = new Vector3(0.5f, 0.0f, 0.5f);

            Vector2 textureTopLeft = new Vector2(0.0f, 0.0f);
            Vector2 textureTopRight = new Vector2(1.0f, 0.0f);
            Vector2 textureBottomLeft = new Vector2(0.0f, 1.0f);
            Vector2 textureBottomRight = new Vector2(1.0f, 1.0f);

            _vertices = new VertexPositionTexture[4];
            _vertices[0] = new VertexPositionTexture(topLeftFront, textureTopLeft);
            _vertices[1] = new VertexPositionTexture(bottomLeftFront, textureBottomLeft);
            _vertices[2] = new VertexPositionTexture(topRightFront, textureTopRight);
            _vertices[3] = new VertexPositionTexture(bottomRightFront, textureBottomRight);

            /*_vertexBuffer = new VertexBuffer(OneAmEngine.Engine.Device,
                                                 VertexPositionTexture.SizeInBytes * _vertices.Length,
                                                 BufferUsage.WriteOnly);*/

            _vertexBuffer = new VertexBuffer(OneAmEngine.Engine.Device, new VertexDeclaration(VertexPositionTexture.VertexDeclaration.GetVertexElements()),
                                                 VertexPositionTexture.VertexDeclaration.VertexStride * _vertices.Length,
                                                 BufferUsage.WriteOnly);

            _vertexBuffer.SetData<VertexPositionTexture>(_vertices);
        }


        public void Update()
        {
            _currentFrameTime += OneAmEngine.Engine.ElapsedSeconds;
            if (_currentFrameTime > 0.03f)
            {
                _currentFrame++;
                if (_currentFrame == _pixmaps.Count) _currentFrame = 0;
                Vector3 texSize = new Vector3(_pixmaps[_currentFrame].Texture.Width, _pixmaps[_currentFrame].Texture.Height, 1);
                _scaleMatrix = Matrix.CreateScale(_scale * texSize);
                _currentFrameTime = 0;
            }
        }
    }
}
