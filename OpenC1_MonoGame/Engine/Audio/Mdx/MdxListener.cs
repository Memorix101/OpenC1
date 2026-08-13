using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace OneAmEngine.Audio
{
    /// <summary>
    /// Formerly the DirectSound listener, now a thin shell around MonoGame's
    /// AudioListener. The old deferred updates (BeginUpdate/CommitChanges) are gone -
    /// MonoGame applies changes immediately.
    /// </summary>
    class MdxListener : IListener
    {
        internal readonly AudioListener XnaListener = new AudioListener();

        public Matrix Orientation
        {
            set
            {
                XnaListener.Forward = Vector3.Normalize(value.Forward);
                XnaListener.Up = Vector3.Normalize(value.Up);
            }
        }

        public void SetOrientation(Vector3 forward)
        {
            if (forward != Vector3.Zero)
                XnaListener.Forward = Vector3.Normalize(forward);
            XnaListener.Up = Vector3.Up;
        }

        Vector3 IListener.Position
        {
            get => XnaListener.Position;
            set => XnaListener.Position = value;
        }

        Vector3 IListener.Velocity
        {
            set => XnaListener.Velocity = value;
        }

        float IListener.DistanceFactor
        {
            // DirectSound counted in metres per world unit, which maps onto MonoGame's
            // global DistanceScale.
            set { if (value > 0) SoundEffect.DistanceScale = value; }
        }

        float IListener.RolloffFactor
        {
            set { if (value > 0) SoundEffect.DopplerScale = value; }
        }

        public void BeginUpdate() { }

        public void CommitChanges() { }
    }
}
