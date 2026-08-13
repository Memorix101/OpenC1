using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using OpenC1.Physics;
using OneAmEngine;

namespace OpenC1
{
    class ModelShadow
    {
        /// <summary>Maximaler Abstand Auto-Unterkante zu Boden, ab dem kein Schatten mehr faellt.</summary>
        const float MaxShadowDropDistance = 4f;
        /// <summary>Maximaler Hoehenunterschied innerhalb der Schattenflaeche.</summary>
        const float MaxShadowHeightSpread = 2f;

        static VertexDeclaration _vertexDeclaration;

        static ModelShadow()
        {
            //_vertexDeclaration = new VertexDeclaration(VertexPositionColor.VertexElements);
        }

        public static void Render(BoundingBox bb, VehicleChassis chassis)
        {
            Vector3[] points = new Vector3[4];

            Matrix pose = chassis.Actor.GlobalPose;
            float shadowWidth = 0.0f;
            Vector3 pos = new Vector3(bb.Min.X - shadowWidth, 0, bb.Min.Z);
            points[0] = Vector3.Transform(pos, pose);
            pos = new Vector3(bb.Max.X + shadowWidth, 0, bb.Min.Z);
            points[1] = Vector3.Transform(pos, pose);
            pos = new Vector3(bb.Min.X - shadowWidth, 0, bb.Max.Z);
            points[2] = Vector3.Transform(pos, pose);
            pos = new Vector3(bb.Max.X + shadowWidth, 0, bb.Max.Z);
            points[3] = Vector3.Transform(pos, pose);

            StillDesign.PhysX.Scene scene = chassis.Actor.Scene;
            Vector3 offset = new Vector3(0, 0.1f, 0);
            for (int i = 0; i < 4; i++)
            {
                StillDesign.PhysX.RaycastHit hit = scene.RaycastClosestShape(
                    new StillDesign.PhysX.Ray(points[i], Vector3.Down), StillDesign.PhysX.ShapesType.Static);

                // Trifft ein Strahl nichts (Auto in der Luft, Loch im Mesh), ist
                // WorldImpact der Nullpunkt - die Schattenflaeche wuerde quer durch
                // die Karte bis zum Weltursprung gezogen.
                if (hit.Shape == null)
                    return;

                // Zu weit weg heisst: das Auto haengt in der Luft oder der Strahl hat
                // etwas ganz anderes getroffen. Beides ergibt ein verdrehtes Polygon,
                // das frei im Raum steht.
                if (Math.Abs(points[i].Y - hit.WorldImpact.Y) > MaxShadowDropDistance)
                    return;

                points[i] = hit.WorldImpact + offset;
            }

            // Ein Schatten liegt flach auf dem Boden. Klaffen die vier Treffer stark
            // auseinander, steht das Polygon quer - dann lieber gar keinen zeichnen.
            float minY = points[0].Y, maxY = points[0].Y;
            for (int i = 1; i < 4; i++)
            {
                minY = Math.Min(minY, points[i].Y);
                maxY = Math.Max(maxY, points[i].Y);
            }
            if (maxY - minY > MaxShadowHeightSpread)
                return;

            Color shadowColor = new Color(10, 10, 10, 100);
            VertexPositionColor[] verts = new VertexPositionColor[points.Length];
            int i2 = 0;
            for (int i = points.Length-1; i >= 0; i--)
            {
                verts[i2++] = new VertexPositionColor(points[i], shadowColor);
            }

            GraphicsDevice device = GameEngine.Device;
            RasterizerState oldRasterizerState = GameEngine.Device.RasterizerState;
            GameEngine.Device.RasterizerState = GameVars.CullDisabled;
            // NonPremultiplied, nicht AlphaBlend: die Vertexfarbe (10,10,10,100) ist
            // nicht vormultipliziert. Mit MonoGames AlphaBlend (das vormultipliziert
            // erwartet) waere der Schatten eine fast schwarze Flaeche.
            BlendState oldBlendState = GameEngine.Device.BlendState;
            GameEngine.Device.BlendState = BlendState.NonPremultiplied;

            GameVars.CurrentEffect.World = Matrix.Identity;
            GameVars.CurrentEffect.TextureEnabled = false;
            GameVars.CurrentEffect.VertexColorEnabled = true;
            //VertexDeclaration oldVertDecl = device.VertexDeclaration;
            //device.VertexDeclaration = _vertexDeclaration;
            //GameEngine.Device.RasterizerState.AlphaTestEnable = false;
			GameVars.CurrentEffect.PreferPerPixelLighting = false;
            //GameVars.CurrentEffect.LightingEnabled = false; #
            GameVars.ApplyCurrentEffect();

            //device.RasterizerState.AlphaBlendEnable = true; #
            //device.RasterizerState.AlphaBlendOperation = BlendFunction.Add; #
            //device.RasterizerState.DestinationBlend = Blend.InverseSourceAlpha; #
            //device.RasterizerState.SourceBlend = Blend.SourceAlpha; #
            //device.RasterizerState.DepthBufferWriteEnable = false;

            device.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, verts, 0, 2);

            GameVars.CurrentEffect.VertexColorEnabled = false;
            //device.RasterizerState.AlphaBlendEnable = false; #
            //device.RasterizerState.DepthBufferWriteEnable = true; #
            //device.VertexDeclaration = oldVertDecl;
            GameEngine.Device.RasterizerState = oldRasterizerState;
            GameEngine.Device.BlendState = oldBlendState;
            GameVars.CullingOff = false;

			GameVars.CurrentEffect.PreferPerPixelLighting = true;
			//GameEngine.Device.RasterizerState.AlphaTestEnable = true; #
			GameVars.CurrentEffect.TextureEnabled = true;
            
        }
    }
}
