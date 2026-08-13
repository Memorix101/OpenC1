using System;
using System.Collections.Generic;
using System.Text;
using OpenC1.Parsers;
using Microsoft.Xna.Framework;
using OneAmEngine;
using OpenC1.Gfx;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace OpenC1
{
	enum EmulationMode
	{
		Demo,
		Full,
		SplatPackDemo,
		SplatPack
	}

    static class GameVars
    {
        public static PaletteFile Palette { get; set; }
        public static int DrawDistance;
        public static Vector3 Scale = new Vector3(6, 6, 6);
        public static Matrix ScaleMatrix = Matrix.CreateScale(Scale);
        public static int NbrSectionsRendered = 0;
        public static int NbrSectionsChecked = 0;
        public static int NbrDrawCalls = 0;
        public static bool CullingOff { get; set; }
        public static Color FogColor;

        // As of XNA 4 / MonoGame render states are immutable: instead of
        // device.RasterizerState.CullMode = ... you assign a ready-made object.
        // Created once here rather than per draw call.
        public static readonly RasterizerState CullBackFaces =
            new RasterizerState { CullMode = CullMode.CullClockwiseFace };
        public static readonly RasterizerState CullDisabled =
            new RasterizerState { CullMode = CullMode.None };
        public static readonly RasterizerState CullDisabledSkidMarks =
            new RasterizerState { CullMode = CullMode.None, DepthBias = -0.00002f };

        /// <summary>
        /// Pushes changed effect parameters (World, Texture, ...) to the GPU.
        /// In XNA 3 effect.CommitChanges() did this inside Begin()/End(); as of
        /// XNA 4 / MonoGame a parameter change only takes effect after Apply(),
        /// which has to run before every single draw call.
        /// </summary>
        public static void ApplyCurrentEffect()
        {
            CurrentEffect.CurrentTechnique.Passes[0].Apply();
        }

        /// <summary>
        /// Sets up the states for the 3D pass. Needed because SpriteBatch leaves behind
        /// CullCounterClockwise, LinearClamp and DepthStencilState.None - which would
        /// cull away the entire world and disable depth testing.
        /// </summary>
        public static void SetupWorldRenderStates(GraphicsDevice device)
        {
            // The palette textures are cutouts: index 0 becomes alpha 0. A global alpha
            // test used to handle that (ReferenceAlpha 100), which no longer exists as of
            // XNA 4 - hence alpha blending, otherwise the transparent areas (pedestrians,
            // fences, signs) get filled in black.
            device.BlendState = BlendState.AlphaBlend;
            device.DepthStencilState = DepthStencilState.Default;
            device.SamplerStates[0] = SamplerState.LinearWrap;
            device.RasterizerState = CullBackFaces;
            CullingOff = false;
        }
        public static string BasePath;
        public static BasicEffect CurrentEffect; //BasicEffect2
        public static ParticleEmitter SparksEmitter;
        public static string SelectedCarFileName;
        public static RaceInfo SelectedRaceInfo;
        public static Texture2D SelectedRaceScene;
        public static EmulationMode Emulation;
        public static int SkillLevel = 1;
        public static bool FullScreen;

		public static void DetectEmulationMode()
		{
			if (File.Exists(GameVars.BasePath + "RACES\\CASTLE.TXT") || File.Exists(GameVars.BasePath + "RACES\\TINSEL.TXT"))
			{
				if (!File.Exists(GameVars.BasePath + "NETRACES.TXT"))
					GameVars.Emulation = EmulationMode.SplatPackDemo;
				else
					GameVars.Emulation = EmulationMode.SplatPack;
			}
			else
			{
				if (!File.Exists(GameVars.BasePath + "NETRACES.TXT"))
					GameVars.Emulation = EmulationMode.Demo;
				else
					GameVars.Emulation = EmulationMode.Full;
			}
		}
        
    }
}
