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

        // Ab XNA 4 / MonoGame sind Renderstates unveraenderlich: statt
        // device.RasterizerState.CullMode = ... wird ein fertiges Objekt zugewiesen.
        // Deshalb hier einmalig angelegt statt pro Draw-Call neu erzeugt.
        public static readonly RasterizerState CullBackFaces =
            new RasterizerState { CullMode = CullMode.CullClockwiseFace };
        public static readonly RasterizerState CullDisabled =
            new RasterizerState { CullMode = CullMode.None };
        public static readonly RasterizerState CullDisabledSkidMarks =
            new RasterizerState { CullMode = CullMode.None, DepthBias = -0.00002f };

        /// <summary>
        /// Uebertraegt geaenderte Effektparameter (World, Texture, ...) an die GPU.
        /// In XNA 3 leistete das effect.CommitChanges() innerhalb von Begin()/End();
        /// ab XNA 4 / MonoGame wirkt eine Parameteraenderung erst nach Apply(), und
        /// zwar vor jedem einzelnen Draw-Call.
        /// </summary>
        public static void ApplyCurrentEffect()
        {
            CurrentEffect.CurrentTechnique.Passes[0].Apply();
        }

        /// <summary>
        /// Setzt die Zustaende fuer den 3D-Durchgang. Noetig, weil SpriteBatch
        /// CullCounterClockwise, LinearClamp und DepthStencilState.None hinterlaesst -
        /// damit waere die komplette Welt weggecullt bzw. ohne Tiefentest.
        /// </summary>
        public static void SetupWorldRenderStates(GraphicsDevice device)
        {
            // Die Palettentexturen sind Cutouts: Index 0 wird zu Alpha 0. Frueher hat
            // das ein globaler Alpha-Test erledigt (ReferenceAlpha 100), den es ab
            // XNA 4 nicht mehr gibt - deshalb hier Alphablending, sonst werden die
            // transparenten Bereiche (Fussgaenger, Zaeune, Schilder) schwarz gefuellt.
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
