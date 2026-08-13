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
        /// <summary>Distance from car underside to ground beyond which no shadow is cast.</summary>
        const float MaxShadowDropDistance = 4f;
        /// <summary>Maximum height difference within the shadow quad.</summary>
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

                // When a ray hits nothing (car airborne, hole in the mesh) WorldImpact is
                // the origin - the shadow quad would be stretched across the map all the
                // way to the world origin.
                if (hit.Shape == null)
                    return;

                // Too far away means the car is airborne or the ray hit something else
                // entirely. Either way the result is a twisted polygon floating in space.
                if (Math.Abs(points[i].Y - hit.WorldImpact.Y) > MaxShadowDropDistance)
                    return;

                points[i] = hit.WorldImpact + offset;
            }

            // A shadow lies flat on the ground. If the four hits are far apart the quad
            // stands at an angle - better to draw none at all.
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
            // NonPremultiplied rather than AlphaBlend: the vertex colour (10,10,10,100)
            // is not premultiplied. With MonoGame's AlphaBlend (which expects premultiplied
            // values) the shadow would come out as a near-black patch.
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
