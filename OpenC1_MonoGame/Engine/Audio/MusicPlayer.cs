using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using Microsoft.Xna.Framework.Media;

namespace OneAmEngine.Audio
{
    public class MusicPlayer
    {
        private Song _music;
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

            _music = Song.FromUri(null, new Uri(gamePath + "/track0" + new Random().Next(2, 9) + ".ogg", UriKind.RelativeOrAbsolute));
            MediaPlayer.Stop();
        }

        public void Play()
        {
            if (!found)
                return;

            MediaPlayer.Play(_music);
        }
        public void Stop()
        {
            if (!found)
                return;

            MediaPlayer.Stop();
        }

        public void Update()
        {
            if (!found)
                return;

            if (MediaPlayer.State == MediaState.Stopped)
            {
                MediaPlayer.Stop();
                Shuffle();
                MediaPlayer.Play(_music);
            }
        }
    }
}