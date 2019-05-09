using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace OneAmEngine
{
    public class ScreenEffects
    {
        public event EventHandler FadeCompleted;

        private enum FadeDirection
        {
            None,
            FadeIn,
            FadeOut
        }

        private static ScreenEffects _instance;
        public static ScreenEffects Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ScreenEffects();
                return _instance;
            }
        }

        private Color _fadeColor;
        private float _alpha;
        private FadeDirection _fadeDirection;
        private Texture2D _fadeTexture;
        public float FadeSpeed = 350;

        private ScreenEffects()
        {
            _fadeDirection = FadeDirection.None;

            _fadeTexture = new Texture2D(GameEngine.Device, 1, 1);
            _fadeTexture.SetData<Color>(new Color[] { Color.Black });
        }

        /// <summary>
        /// Helper draws a translucent black fullscreen sprite, used for fading
        /// screens in and out, and for darkening the background behind popups.
        /// </summary>
        public void FadeScreen()
        {
            _alpha = 0;
            _fadeDirection = FadeDirection.FadeOut;
        }

        public void UnFadeScreen()
        {
            _alpha = 255;
            _fadeDirection = FadeDirection.FadeIn;
        }

        #region IDrawableObject Members

        public void Update(GameTime gameTime)
        {
            if (_fadeDirection == FadeDirection.FadeOut)
            {
                _alpha += FadeSpeed * GameEngine.ElapsedSeconds;
                if (_alpha >= 255)
                    CompleteFade();
            }
            else if (_fadeDirection == FadeDirection.FadeIn)
            {
                _alpha -= FadeSpeed * 0.5f * GameEngine.ElapsedSeconds;
                if (_alpha <= 0)
                    CompleteFade();
            }

        }

        public void Draw()
        {
            if (_fadeDirection != FadeDirection.None)
            {
                Viewport viewport = GameEngine.Device.Viewport;

                GameEngine.SpriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Deferred, SaveStateMode.SaveState);


                GameEngine.SpriteBatch.Draw(_fadeTexture,
                                 new Rectangle(0, 0, viewport.Width, viewport.Height),
                                 new Color(255, 255, 255, (byte)_alpha));

                GameEngine.SpriteBatch.End();
            }
        }

        private void CompleteFade()
        {
            _fadeDirection = FadeDirection.None;
            if (FadeCompleted != null)
            {
                FadeCompleted(this, null);
            }
        }

        #endregion

        public static Texture2D TakeScreenshot()
        {
            int w = GameEngine.Device.PresentationParameters.BackBufferWidth;
            int h = GameEngine.Device.PresentationParameters.BackBufferHeight;

            ResolveTexture2D screenshot = new ResolveTexture2D(GameEngine.Device, w, h, 1, GameEngine.Device.PresentationParameters.BackBufferFormat);
            GameEngine.Device.ResolveBackBuffer(screenshot);
            return screenshot;
        }
    }
}
