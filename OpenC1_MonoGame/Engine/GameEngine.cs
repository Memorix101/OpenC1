using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using OneAmEngine;
using OneAmEngine.Audio;

namespace OneAmEngine
{
    public static class GameEngine
    {
        public static Game Game;
        private static ContentManager _contentManager;
        private static ICamera _camera;
        public static IPlayer Player { get; set; }
        private static SpriteBatch _spriteBatch;
        private static FrameRateCounter _fpsCounter;

        public static DebugRenderer DebugRenderer;
        public static IGameScreen Screen { get; set; }
        public static GraphicsDevice Device { get; private set; }
        public static BasicEffect CurrentEffect;
        public static InputProvider Input { get; set; }
        public static float DrawDistance { get; set; }
        public static float ElapsedSeconds { get; private set; }
        public static float TotalSeconds { get; private set; }
        public static RandomGenerator Random { get; private set; }
        public static ISoundEngine Audio { get; set; }
        public static float TimeScale { get; set; }

        private static bool _isFullScreen;
        public static Vector2 ScreenSize;

        private static RenderTarget2D screenshot;

        public static void Startup(Game game, GraphicsDeviceManager graphics)
        {
            Game = game;
            Device = graphics.GraphicsDevice;
            _isFullScreen = graphics.IsFullScreen;

            screenshot = new RenderTarget2D(Device, Device.PresentationParameters.BackBufferWidth,
                Device.PresentationParameters.BackBufferHeight, true, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);

            DrawDistance = 1000;

            _contentManager = new ContentManager(Game.Services);

            Input = new InputProvider(Game);
            DebugRenderer = new DebugRenderer();
            _spriteBatch = new SpriteBatch(Device);
            _fpsCounter = new FrameRateCounter();
            Random = new RandomGenerator();
            TimeScale = 1;
            Window = Game.Window.ClientBounds;
        }


        public static void Update(GameTime gameTime)
        {
            ElapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds * TimeScale;
            TotalSeconds = (float)gameTime.TotalGameTime.TotalSeconds;

            GameConsole.Clear();
            _fpsCounter.Update(gameTime);
            Input.Update(gameTime);
            Audio?.Update();
            Screen.Update();
            ScreenEffects.Instance.Update(gameTime);
            DebugRenderer.Update(gameTime);

            if (GameEngine.Input.WasPressed(Keys.P))
            {
                TakeScreenshot();
                //MessageRenderer.Instance.PostMainMessage("destroy.pix", 50, 0.7f, 0.003f, 1.4f);
            }
        }

        public static void Render(GameTime gameTime)
        {
            Device.SetRenderTarget(screenshot);
            Device.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };
            Screen.Render();
            DebugRenderer.Draw();
            ScreenEffects.Instance.Draw();
            GameConsole.Render();
            DebugRenderer.DrawText();
            _fpsCounter.Draw(gameTime);
            Device.SetRenderTarget(null);

            //redraw everything to window
            Device.Clear(Color.Black);
            SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
                SamplerState.LinearClamp, DepthStencilState.Default,
                RasterizerState.CullNone);
            SpriteBatch.Draw(screenshot, new Rectangle(0, 0, Device.PresentationParameters.BackBufferWidth, Device.PresentationParameters.BackBufferHeight), Color.White);

            SpriteBatch.End();
        }

        public static ContentManager ContentManager
        {
            get { return _contentManager; }
        }

        public static ICamera Camera
        {
            get { return _camera; }
            set
            {
                _camera = value;
                _camera.DrawDistance = DrawDistance;
            }
        }

        public static Rectangle Window { get; private set; }

        public static float AspectRatio
        {
            get
            {
                if (_isFullScreen)
                    return ScreenSize.X / ScreenSize.Y;
                else
                    return (float)Window.Width / (float)Window.Height;
            }
        }

        public static SpriteBatch SpriteBatch
        {
            get { return _spriteBatch; }
        }

        public static int Fps
        {
            get { return _fpsCounter.FrameRate; }
        }

        private static void TakeScreenshot()
        {
            int count = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "/", "ndump*.bmp").Length + 1;
            string name = "/ndump" + count.ToString("000") + ".bmp";

            FileStream fileStream = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "/" + name, FileMode.Create);
            screenshot.SaveAsJpeg(fileStream, Device.PresentationParameters.BackBufferWidth, Device.PresentationParameters.BackBufferHeight);
            fileStream.Close();

            //MessageRenderer.Instance.PostHeaderMessage("Screenshot dumped to " + name, 3);
        }
    }
}