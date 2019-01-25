using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OneAmEngine;

namespace OpenC1.Parsers.Funks
{
    class RollFunk : BaseFunk
    {
        public Vector2 Speed;
        Vector2 _uvOffset;
        TextureAddressMode _lastMode;

        public RollFunk()
        {
            
        }

        public override void BeforeRender()
        {
            _lastMode = OneAmEngine.Engine.Device.SamplerStates[0].AddressU;
            if (_lastMode != TextureAddressMode.Wrap)
                OneAmEngine.Engine.Device.SamplerStates[0].AddressU = OneAmEngine.Engine.Device.SamplerStates[0].AddressV = TextureAddressMode.Wrap;

            GameVars.CurrentEffect.TexCoordsOffset = _uvOffset;
        }

        public override void AfterRender()
        {
            if (_lastMode != TextureAddressMode.Wrap)
                OneAmEngine.Engine.Device.SamplerStates[0].AddressU = OneAmEngine.Engine.Device.SamplerStates[0].AddressV = _lastMode;

            GameVars.CurrentEffect.TexCoordsOffset = Vector2.Zero;
        }

        public override void Update()
        {
            _uvOffset += Speed * 0.8f * OneAmEngine.Engine.ElapsedSeconds;
            if (_uvOffset.X > 1) _uvOffset.X = 1 - _uvOffset.X;
            if (_uvOffset.Y > 1) _uvOffset.Y = 1 - _uvOffset.Y;
        }
    }
}
