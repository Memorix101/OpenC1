﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace OneAmEngine.Audio
{
    class MdxListener : IListener
    {
        SoundEffectInstance _listener;

        private float _DistanceFactor;
        private Vector3 _Position;
        private Vector3 _Velocity;
        private float _RolloffFactor;

        /*public MdxListener()
         {
             /*BufferDescription desc = new BufferDescription();
             desc.PrimaryBuffer = true;
             desc.Control3D = true;
             desc.Mute3DAtMaximumDistance = true;*/
        //Microsoft.DirectX.DirectSound.Buffer buffer = new Microsoft.DirectX.DirectSound.Buffer(desc, device);
        //_listener.set3DNumListeners(0);
        // Orientation = Matrix.Identity;
        // }*/

        public MdxListener()
        {

        }


        public Matrix Orientation
        {
            set
            {
                /*Listener3DOrientation orientation = _listener.Orientation;
				orientation.Front = MdxHelpers.ToMdx(Vector3.Normalize(value.Forward));
				orientation.Top = MdxHelpers.ToMdx(Vector3.Normalize(value.Up));
				_listener.Orientation = orientation;*/
            }
        }

        public void SetOrientation(Vector3 forward)
        {
            /*Listener3DOrientation orientation = _listener.Orientation;
            orientation.Front = MdxHelpers.ToMdx(forward);
            orientation.Top = MdxHelpers.ToMdx(Vector3.Up);
            _listener.Orientation = orientation;*/
        }

        Vector3 IListener.Position
        {
            get => _Position;
            set => _Position = value;
        }
        Vector3 IListener.Velocity
        {
            set => _Velocity = value;
        }
        float IListener.DistanceFactor
        {
            set => _DistanceFactor = value;
        }
        float IListener.RolloffFactor
        {
            set => _RolloffFactor = value;
        }

        public void BeginUpdate()
        {
            //_listener.Deferred = true;
        }

        public void CommitChanges()
        {
            //_listener.CommitDeferredSettings();
           // _listener.update();
        }
    }
}