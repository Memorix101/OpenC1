using System;
using System.Collections.Generic;
using System.Text;
using OneAmEngine;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using OpenC1.Parsers;
using System.IO;
using Microsoft.Xna.Framework.Input;

namespace OpenC1.Screens
{
    class GameSelectionScreen : BaseMenuScreen
    {
        float _showTime;
		int _selectedIndex = 0;
		List<string> _mods = new List<string>();


        public GameSelectionScreen(BaseMenuScreen parent)
            : base(parent)
        {
            //_inAnimation = new AnimationPlayer(LoadAnimation("MAI2AWAY.fli"));
            //_inAnimation.Play(false);
            //_outAnimation = new AnimationPlayer(LoadAnimation("MAI2AWAY.fli"));

            ScreenEffects.Instance.FadeSpeed = 300;
            ScreenEffects.Instance.UnFadeScreen();

            _showTime = GameEngine.TotalSeconds;

			string[] mods = Directory.GetDirectories("GameData");
			foreach (string game in mods)
				_mods.Add(new DirectoryInfo(game).Name);
        }

        public override void Render()
        {
            base.Render();

            GameEngine.SpriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Deferred, SaveStateMode.SaveState);

			RenderDefaultBackground();

			WriteTitleLine("OpenC1 - choose game:");

			float y = 120;
			for (int i = 0; i < _mods.Count; i++)
			{
				Color c = Color.White;
				if (i == _selectedIndex)
					GameEngine.SpriteBatch.DrawString(_font, "< " + _mods[i] + " >", new Vector2(40, y), Color.YellowGreen);
				else
					GameEngine.SpriteBatch.DrawString(_font, "  " + _mods[i], new Vector2(40, y), Color.White);
				y += 35;
			}
            
            GameEngine.SpriteBatch.End();
        }

		public override void Update()
		{
            if (GameEngine.Input.WasPressed(Keys.Up) || GameEngine.Input.WasPressed(Buttons.DPadUp) || GameEngine.Input.WasPressed(Buttons.LeftThumbstickUp))
                _selectedIndex = Math.Max(0, _selectedIndex-1);
			else if (GameEngine.Input.WasPressed(Keys.Down) || GameEngine.Input.WasPressed(Buttons.DPadDown) || GameEngine.Input.WasPressed(Buttons.LeftThumbstickDown))
				_selectedIndex = Math.Min(_mods.Count-1, _selectedIndex+1);

            base.Update();
        }

        protected override void OnOutAnimationFinished()
		{
			GameVars.BasePath = Path.Combine(Environment.CurrentDirectory, "GameData") + "\\" + _mods[_selectedIndex] + "\\";
            OneAmEngine.Audio.MusicPlayer.modName = _mods[_selectedIndex];
            GameVars.DetectEmulationMode();
			GameEngine.Screen = new MainMenuScreen(null);
		}

        protected override void OnInAnimationFinished()
        {
            //throw new NotImplementedException();
        }
    }
}
