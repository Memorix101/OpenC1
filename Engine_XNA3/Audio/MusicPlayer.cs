using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using Microsoft.DirectX.DirectSound;
using Microsoft.Xna.Framework.Media;
using NAudio.Wave;

namespace OneAmEngine.Audio
{
    public class MusicPlayer
    {
        private WaveOutEvent _music;
        public static string modName;
        string gamePath = AppDomain.CurrentDomain.BaseDirectory + "/GameData/" + modName + "/MUSIC";
        private bool found = false;

        public MusicPlayer()
        {
            if (Directory.Exists(gamePath))
            {
                found = true;
                Shuffle();
            }
        }

        private void Shuffle()
        {
            if (!found)
                return;

            var vorbisStream = new NAudio.Vorbis.VorbisWaveReader(gamePath + "/track0" + new Random().Next(2, 9) + ".ogg");
            _music = new WaveOutEvent();
            _music.Init(vorbisStream);
            _music.Stop();
        }

        public void Play()
        {
            if (!found)
                return;

            _music.Play();
        }
        public void Stop()
        {
            if (!found)
                return;

            _music.Stop();
        }

        public void Update()
        {
            if (!found)
                return;

            if (_music.PlaybackState == PlaybackState.Stopped)
            {
                _music.Stop();
                Shuffle();
                _music.Play();
            }
        }
    }
}