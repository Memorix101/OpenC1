using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace OneAmEngine.Audio
{
    class MdxSound : ISound
    {
        //SecondaryBuffer _buffer;
        //Buffer3D _buffer3d;
        bool _is3d;

        public int Id { get; set; }
        public object Owner { get; set; }
        public bool MuteAtMaximumDistance { get; set; }

        /*internal MdxSound(Device device, string filename, bool is3d)
		{
			BufferDescription desc = new BufferDescription();

            if (is3d)
            {
                desc.Control3D = true;
                desc.Guid3DAlgorithm = DSoundHelper.Guid3DAlgorithmDefault;
                desc.Mute3DAtMaximumDistance = true;
            }
			desc.ControlVolume = true;
			desc.ControlFrequency = true;
			_buffer = new SecondaryBuffer(filename, desc, device);
            
            if (is3d)
            {
                _buffer3d = new Buffer3D(_buffer);
                _buffer3d.Mode = Mode3D.Normal;
                _is3d = true;
            }
		}*/

        float ISound.Duration => throw new NotImplementedException();

        float ISound.Volume { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Vector3 Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Vector3 Velocity { set => throw new NotImplementedException(); }
        public int Frequency { set => throw new NotImplementedException(); }

        public bool IsPlaying => throw new NotImplementedException();

        public float MinimumDistance { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public float MaximumDistance { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public void Play(bool loop)
        {
            throw new NotImplementedException();
        }
    }
}