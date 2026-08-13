using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace OneAmEngine.Audio
{
    /// <summary>
    /// Originally a DirectSound wrapper (Managed DirectX). Runs entirely on MonoGame
    /// audio now.
    ///
    /// Two differences to DirectSound are bridged here:
    ///  - DirectSound sets the playback rate in absolute Hz. MonoGame only knows Pitch
    ///    from -1..+1, meaning one octave down or up. The conversion goes through the
    ///    ratio to the file's sample rate, which is read from the WAV header.
    ///  - 3D sound runs through AudioEmitter/AudioListener instead of Buffer3D.
    /// </summary>
    class MdxSound : ISound
    {
        readonly bool _is3d;
        bool _apply3dFailed;
        readonly SoundEffect _sndfx;
        readonly SoundEffectInstance _sndInstance;
        readonly AudioEmitter _emitter = new AudioEmitter();
        readonly int _sampleRate;

        public int Id { get; set; }
        public object Owner { get; set; }
        public bool MuteAtMaximumDistance { get; set; }

        internal MdxSound(string filename, bool is3d)
        {
            _is3d = is3d;

            using (FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                _sampleRate = ReadWaveSampleRate(file);
                file.Position = 0;
                _sndfx = SoundEffect.FromStream(file); //OGG sounds are not supported
            }

            _sndInstance = _sndfx.CreateInstance();
        }

        /// <summary>
        /// Reads the sample rate from the fmt chunk of a RIFF/WAVE stream.
        /// Falls back to 22050 on an unexpected layout - that only affects the pitch
        /// conversion, not playback itself.
        /// </summary>
        static int ReadWaveSampleRate(Stream stream)
        {
            const int fallback = 22050;
            try
            {
                using (BinaryReader reader = new BinaryReader(stream, System.Text.Encoding.ASCII, true))
                {
                    if (new string(reader.ReadChars(4)) != "RIFF") return fallback;
                    reader.ReadInt32(); // chunk size
                    if (new string(reader.ReadChars(4)) != "WAVE") return fallback;

                    while (stream.Position < stream.Length - 8)
                    {
                        string chunkId = new string(reader.ReadChars(4));
                        int chunkSize = reader.ReadInt32();
                        if (chunkId == "fmt ")
                        {
                            reader.ReadInt16(); // format
                            reader.ReadInt16(); // channels
                            int sampleRate = reader.ReadInt32();
                            return sampleRate > 0 ? sampleRate : fallback;
                        }
                        stream.Position += chunkSize + (chunkSize % 2); // chunks are word aligned
                    }
                }
            }
            catch (Exception)
            {
                // Broken or unknown header - the default is good enough.
            }
            return fallback;
        }

        public float Duration => (float)_sndfx.Duration.TotalSeconds;

        public float Volume
        {
            get => _sndInstance.Volume;
            set => _sndInstance.Volume = MathHelper.Clamp(value, 0f, 1f);
        }

        /// <summary>
        /// Playback rate in Hz, the way DirectSound expects it. Converted to MonoGame's
        /// pitch range and clamped to one octave either way.
        /// </summary>
        public int Frequency
        {
            set
            {
                if (value <= 0) return;
                float octaves = (float)Math.Log((double)value / _sampleRate, 2);
                _sndInstance.Pitch = MathHelper.Clamp(octaves, -1f, 1f);
            }
        }

        public Vector3 Position
        {
            get => _emitter.Position;
            set
            {
                _emitter.Position = value;
                Apply3D();
            }
        }

        public Vector3 Velocity
        {
            set
            {
                _emitter.Velocity = value;
                Apply3D();
            }
        }

        void Apply3D()
        {
            if (!_is3d || _apply3dFailed) return;

            MdxListener listener = GameEngine.Audio?.GetListener() as MdxListener;
            if (listener == null) return;

            try
            {
                _sndInstance.Apply3D(listener.XnaListener, _emitter);
            }
            catch (InvalidOperationException)
            {
                // Apply3D requires mono samples. Stereo files stay 2D.
                _apply3dFailed = true;
            }
        }

        public bool IsPlaying => _sndInstance.State == SoundState.Playing;
        public float MinimumDistance { get; set; }
        public float MaximumDistance { get; set; }

        public void Pause() { _sndInstance.Pause(); }
        public void Stop() { _sndInstance.Stop(); }

        public void Reset()
        {
            _sndInstance.Stop();
            _sndInstance.Pitch = 0f;
        }

        public void Play(bool loop)
        {
            _sndInstance.IsLooped = loop;
            if (_sndInstance.State == SoundState.Paused)
                _sndInstance.Resume();
            else if (_sndInstance.State != SoundState.Playing)
                _sndInstance.Play();
        }
    }
}
