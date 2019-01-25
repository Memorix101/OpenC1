using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using OpenC1.HUD;
using OpenC1.Parsers;
using OneAmEngine;
using System.IO;
using System.Reflection;
using System.Diagnostics;

namespace OpenC1.Screens
{
    abstract class BaseMenuScreen : IGameScreen
    {
        public IGameScreen Parent { get; private set; }
        protected AnimationPlayer _inAnimation, _outAnimation;
        protected Rectangle _rect;
        protected int _selectedOption;
        protected List<IMenuOption> _options = new List<IMenuOption>();
        protected bool _waitingForOutAnimation;
		public SpriteFont _font;
		private int _currentLine;
		private Viewport v;

        public BaseMenuScreen(IGameScreen parent)
        {
            Parent = parent;
			Viewport viewport = OneAmEngine.Engine.Device.Viewport;
			_rect = new Rectangle(0, 0, OneAmEngine.Engine.Device.Viewport.Width, OneAmEngine.Engine.Device.Viewport.Height);
            OneAmEngine.Engine.Camera = new SimpleCamera();
			_font = OneAmEngine.Engine.ContentManager.Load<SpriteFont>("Fontana");
        }

        public void ReturnToParent()
        {
			if (Parent is BaseMenuScreen)
			{
				if (((BaseMenuScreen)Parent)._inAnimation != null)
					((BaseMenuScreen)Parent)._inAnimation.Play(false);
			}
            OneAmEngine.Engine.Screen = Parent;
        }

		public void RenderDefaultBackground()
		{
			OneAmEngine.Engine.SpriteBatch.Draw(OneAmEngine.Engine.ContentManager.Load<Texture2D>("menu-background"), _rect, Color.White);
		}

		public void WriteTitleLine(string text)
		{
			OneAmEngine.Engine.SpriteBatch.DrawString(_font, text, new Vector2(20, 30), Color.Red, 0, Vector2.Zero, 1.5f, SpriteEffects.None, 0);
			_currentLine = 80;
		}

		public void WriteLine(string text)
		{
			OneAmEngine.Engine.SpriteBatch.DrawString(_font, text, new Vector2(30, _currentLine), Color.LightGray);
			_currentLine += 35;
		}

		public void WriteLine(string text, int line)
		{
			_currentLine = line;
			WriteLine(text);
		}

        #region IGameScreen Members

		public virtual void Update()
		{
			if (_waitingForOutAnimation)
			{
				if (_outAnimation == null || !_outAnimation.IsPlaying)
				{
					_waitingForOutAnimation = false;
					OnOutAnimationFinished();
					return;
				}
			}
			else
			{
				if (OneAmEngine.Engine.Input.WasPressed(Keys.Escape) && Parent != null)
				{
					if (SoundCache.IsInitialized) SoundCache.Play(SoundIds.UI_Esc, null, false);
					ReturnToParent();
				}
				if (OneAmEngine.Engine.Input.WasPressed(Keys.Down))
				{
					if (_selectedOption < _options.Count - 1)
						_selectedOption++;
					else
						_selectedOption = 0;
					if (SoundCache.IsInitialized) SoundCache.Play(SoundIds.UI_UpDown, null, false);
				}
				else if (OneAmEngine.Engine.Input.WasPressed(Keys.Up))
				{
					if (_selectedOption > 0)
						_selectedOption--;
					else
						_selectedOption = _options.Count - 1;
					if (SoundCache.IsInitialized) SoundCache.Play(SoundIds.UI_UpDown, null, false);
				}
				else if (OneAmEngine.Engine.Input.WasPressed(Keys.Enter))
				{
					if (_options.Count == 0 || _options[_selectedOption].CanBeSelected)
					{
						if (SoundCache.IsInitialized) SoundCache.Play(SoundIds.UI_Ok, null, false);
						PlayOutAnimation();
					}
				}
			}
			if (_inAnimation != null) _inAnimation.Update();
			if (_outAnimation != null) _outAnimation.Update();
		}

        private void PlayOutAnimation()
        {
            if (_outAnimation != null) _outAnimation.Play(false);
            _waitingForOutAnimation = true;
        }

        public virtual void Render()
        {
            OneAmEngine.Engine.Device.Clear(Color.Black);

            OneAmEngine.Engine.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            if (_outAnimation != null && _waitingForOutAnimation)
                OneAmEngine.Engine.SpriteBatch.Draw(_outAnimation.GetCurrentFrame(), _rect, Color.White);
            else if (_inAnimation != null)
                OneAmEngine.Engine.SpriteBatch.Draw(_inAnimation.GetCurrentFrame(), _rect, Color.White);

			if (GameVars.BasePath != null)
			{
				Vector2 pos = BaseHUDItem.ScaleVec2(0.01f, 0.92f);
				Version v = Assembly.GetExecutingAssembly().GetName().Version;
				OneAmEngine.Engine.SpriteBatch.DrawString(_font, "Open C1 v" + v.Major + "." + v.Minor + "." + v.Build + " - " + new DirectoryInfo(GameVars.BasePath).Name, pos, Color.Red, 0, Vector2.Zero, 1.1f, SpriteEffects.None, 0);
			}

            if (ShouldRenderOptions())
            {
                _options[_selectedOption].RenderInSpriteBatch();
                OneAmEngine.Engine.SpriteBatch.End();
                _options[_selectedOption].RenderOutsideSpriteBatch();
            }
            else
            {
                OneAmEngine.Engine.SpriteBatch.End();
            }
        }

        public abstract void OnOutAnimationFinished();

        #endregion

        public bool ShouldRenderOptions()
        {
            return _options.Count > 0 && !_inAnimation.IsPlaying && !_outAnimation.IsPlaying && !_waitingForOutAnimation;
        }

        public static List<Texture2D> LoadAnimation(string filename)
        {
            FliFile fli = new FliFile(filename);
            if (fli.Exists)
                return fli.Frames;
            filename = filename.Substring(0, filename.Length - 3) + "png";
            if (File.Exists(GameVars.BasePath + "anim\\" + filename))
            {
                FileStream _stream = new FileStream(GameVars.BasePath + "anim\\" + filename, FileMode.Open);
                return new List<Texture2D> { Texture2D.FromStream(OneAmEngine.Engine.Device, _stream) };
            }

            filename = filename.Substring(0, filename.Length - 3) + "pix";
            PixFile pix = new PixFile(filename);
            if (pix.Exists)
                return new List<Texture2D> { pix.PixMaps[0].Texture };

            return null;
        }
    }
}
