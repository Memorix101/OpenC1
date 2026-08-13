using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace OneAmEngine.Audio
{
    /// <summary>
    /// Urspruenglich ein DirectSound-Wrapper (Managed DirectX). Laeuft jetzt komplett
    /// auf MonoGame-Audio.
    ///
    /// Zwei Unterschiede zu DirectSound, die hier abgebildet werden:
    ///  - DirectSound stellt die Abspielrate absolut in Hz ein. MonoGame kennt nur
    ///    Pitch von -1..+1, also eine Oktave runter bzw. hoch. Umgerechnet wird ueber
    ///    das Verhaeltnis zur Samplerate der Datei, die aus dem WAV-Header gelesen wird.
    ///  - 3D-Ton laeuft ueber AudioEmitter/AudioListener statt ueber Buffer3D.
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
        /// Liest die Samplerate aus dem fmt-Chunk eines RIFF/WAVE-Streams.
        /// Bei unerwartetem Aufbau wird 22050 angenommen - das trifft nur die
        /// Tonhoehenumrechnung, nicht die Wiedergabe selbst.
        /// </summary>
        static int ReadWaveSampleRate(Stream stream)
        {
            const int fallback = 22050;
            try
            {
                using (BinaryReader reader = new BinaryReader(stream, System.Text.Encoding.ASCII, true))
                {
                    if (new string(reader.ReadChars(4)) != "RIFF") return fallback;
                    reader.ReadInt32(); // Chunkgroesse
                    if (new string(reader.ReadChars(4)) != "WAVE") return fallback;

                    while (stream.Position < stream.Length - 8)
                    {
                        string chunkId = new string(reader.ReadChars(4));
                        int chunkSize = reader.ReadInt32();
                        if (chunkId == "fmt ")
                        {
                            reader.ReadInt16(); // Format
                            reader.ReadInt16(); // Kanaele
                            int sampleRate = reader.ReadInt32();
                            return sampleRate > 0 ? sampleRate : fallback;
                        }
                        stream.Position += chunkSize + (chunkSize % 2); // Chunks sind wortweise ausgerichtet
                    }
                }
            }
            catch (Exception)
            {
                // Kaputter oder unbekannter Header - Standardwert genuegt.
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
        /// Abspielrate in Hz, wie sie DirectSound erwartet. Wird auf MonoGames
        /// Pitch-Bereich umgerechnet; ausserhalb einer Oktave wird begrenzt.
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
                // Apply3D verlangt Mono-Samples. Stereo-Dateien bleiben 2D.
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
