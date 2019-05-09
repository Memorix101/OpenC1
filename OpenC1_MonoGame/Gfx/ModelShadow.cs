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
                points[i] = hit.WorldImpact + offset;
            }

            Color shadowColor = new Color(10, 10, 10, 100);
            VertexPositionColor[] verts = new VertexPositionColor[points.Length];
            int i2 = 0;
            for (int i = points.Length-1; i >= 0; i--)
            {
                verts[i2++] = new VertexPositionColor(points[i], shadowColor);
            }

            GraphicsDevice device = GameEngine.Device;
            CullMode oldCullMode = GameEngine.Device.RasterizerState.CullMode;
            GameEngine.Device.RasterizerState.CullMode = CullMode.None;
            
            GameVars.CurrentEffect.World = Matrix.Identity;
            GameVars.CurrentEffect.TextureEnabled = false;
            GameVars.CurrentEffect.VertexColorEnabled = true;
            //VertexDeclaration oldVertDecl = device.VertexDeclaration;
            //device.VertexDeclaration = _vertexDeclaration;
            //GameEngine.Device.RasterizerState.AlphaTestEnable = false;
			GameVars.CurrentEffect.PreferPerPixelLighting = false;
            //GameVars.CurrentEffect.LightingEnabled = false; #

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
            GameEngine.Device.RasterizerState.CullMode = oldCullMode;

			GameVars.CurrentEffect.PreferPerPixelLighting = true;
			//GameEngine.Device.RasterizerState.AlphaTestEnable = true; #
			GameVars.CurrentEffect.TextureEnabled = true;
            
        }
    }
}
