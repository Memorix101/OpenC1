using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using OpenC1.Parsers;
using Microsoft.Xna.Framework;
using System.Threading;
using OneAmEngine;
using System.Globalization;

namespace OpenC1.Screens
{
    class LoadRaceScreen : IGameScreen
    {
        Texture2D _loadingTexture;

        public IGameScreen Parent { get; set; }
        //private Thread _loadRaceThread;
        PlayGameScreen _raceScreen;

        public LoadRaceScreen(IGameScreen parent)
        {
            Parent = parent;
            _loadingTexture = new PixFile("LOADSCRN.pix").PixMaps[0].Texture;

            //_loadRaceThread = new Thread(LoadRaceThreadProc);
            //_loadRaceThread.Start();
        }

        public void Update()
        {
            //if (_loadRaceThread.ThreadState != ThreadState.Running)
            {
				LoadRaceThreadProc();
                GameEngine.Screen = _raceScreen;
            }
        }

        public void Render()
        {
            GameEngine.SpriteBatch.Begin();
            GameEngine.SpriteBatch.Draw(_loadingTexture, new Rectangle(0, 0, GameEngine.Window.Width, GameEngine.Window.Height), Color.White);
            GameEngine.SpriteBatch.End();
        }

        private void LoadRaceThreadProc()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
            _raceScreen = new PlayGameScreen(Parent);
        }
    }
}
