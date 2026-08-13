using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using OneAmEngine;

namespace OpenC1.Parsers.Funks
{
    class WindscreenFunk : BaseFunk
    {
        public Vector2 Speed;
        Vector2 _uvOffset;
        SamplerState _lastSamplerState;
        Vehicle _vehicle;
        Texture2D _origTexture;

        public WindscreenFunk(string materialName, Vehicle vehicle)
        {
            _vehicle = vehicle;
            MaterialName = materialName;
            Speed = new Vector2(0.3f);
        }

        public override void Resolve()
        {
            base.Resolve();
            if (Material != null)
                _origTexture = Material.Texture;
        }

        public override void BeforeRender()
        {
            // SamplerState is immutable as of MonoGame: swap the whole object instead of
            // setting individual fields on the bound state (which throws at runtime).
            _lastSamplerState = GameEngine.Device.SamplerStates[0];
            if (_lastSamplerState.AddressU != TextureAddressMode.Wrap)
                GameEngine.Device.SamplerStates[0] = SamplerState.LinearWrap;

         /*   GameVars.CurrentEffect.TexCoordsOffset = _uvOffset;
            GameVars.CurrentEffect.TexCoordsMultiplier = 0.1f;
            GameVars.CurrentEffect.CommitChanges();*/
        }

        public override void AfterRender()
        {
            if (_lastSamplerState != null && _lastSamplerState.AddressU != TextureAddressMode.Wrap)
                GameEngine.Device.SamplerStates[0] = _lastSamplerState;

        /*    GameVars.CurrentEffect.TexCoordsOffset = Vector2.Zero;
            GameVars.CurrentEffect.TexCoordsMultiplier = 1;
            GameVars.CurrentEffect.CommitChanges();*/
        }

        public override void Update()
        {
            float y = Math.Min(1, _vehicle.Chassis.Actor.AngularVelocity.Y / 10);
            Speed.X = y;
            Speed.Y = Math.Min(1, _vehicle.Chassis.Actor.LinearVelocity.Length() / 10); 
            _uvOffset += Speed * GameEngine.ElapsedSeconds;
            if (_uvOffset.X > 10) _uvOffset.X = 10 - _uvOffset.X;
            if (_uvOffset.Y > 10) _uvOffset.Y = 10 - _uvOffset.Y;

            if (_vehicle.CurrentSpecialVolume.Count > 0)
            {
                SpecialVolume vol = _vehicle.CurrentSpecialVolume.Peek();
                CMaterial mat = ResourceCache.GetMaterial(vol.WindscreenMaterial);
                if (mat != null)
                    this.Material.Texture = mat.Texture;
            }
            else
            {
                this.Material.Texture = _origTexture;
            }
        }
    }
}
