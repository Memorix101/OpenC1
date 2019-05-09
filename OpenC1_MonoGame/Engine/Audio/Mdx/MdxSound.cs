using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace OneAmEngine.Audio
{
    //https://mysteriousspace.com/2015/05/31/fmod-in-c-its-a-pain-to-set-up-heres-how-i-did-it/
    class MdxSound : ISound
    {
        //Buffer3D _buffer3d;
        bool _is3d;
        private FileStream _filename;
        private float _MinimumDistance;
        private float _MaximumDistance;
        private SoundEffect _sndfx;
        private SoundEffectInstance _sndInstance;

        public int Id { get; set; }
        public object Owner { get; set; }
        public bool MuteAtMaximumDistance { get; set; }

        internal MdxSound(string filename, bool is3d)
        {
            //BufferDescription desc = new BufferDescription();

            if (is3d)
            {
                /*
                 desc.Control3D = true;
                 desc.Guid3DAlgorithm = DSoundHelper.Guid3DAlgorithmDefault;
                 desc.Mute3DAtMaximumDistance = true;
                 */
            }
            /*desc.ControlVolume = true;
			desc.ControlFrequency = true;*/
            //_buffer = new SecondaryBuffer(filename, desc, device);
            _filename = new FileStream(filename, FileMode.Open);

            if (File.Exists(filename))
                Console.WriteLine("YES!!");
            else
                Console.WriteLine("NAH!!");
            _sndfx = SoundEffect.FromStream(_filename); //OGG sounds are not supported
            _filename.Dispose();
            _sndInstance = _sndfx.CreateInstance();

            /*
            if (is3d)
            {
                _buffer3d = new Buffer3D(_buffer);
                _buffer3d.Mode = Mode3D.Normal;
                _is3d = true;
            }*/
        }

        public float Duration => _sndfx.Duration.Seconds;
        public float Volume { get => _sndInstance.Volume; set => _sndInstance.Volume = value; }
        public Vector3 Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); } //_sndInstance.Apply3D(new AudioListener(), new AudioEmitter());
        public Vector3 Velocity { set => throw new NotImplementedException(); }
        public int Frequency { set => throw new NotImplementedException(); }
        public bool IsPlaying => _sndInstance.State == SoundState.Playing;
        public float MinimumDistance { get => _MinimumDistance; set => _MinimumDistance = value; }
        public float MaximumDistance { get => _MaximumDistance; set => _MaximumDistance = value; }
        public void Pause() { _sndInstance.Pause(); }
        public void Stop() { _sndInstance.Stop(); }
        public void Reset() { throw new NotImplementedException(); }
        public void Play(bool loop) { _sndInstance.Play(); }
    }
}