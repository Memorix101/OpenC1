﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OneAmEngine
{
    public static class TextureGenerator
    {
        public static Texture2D Generate(Color color)
        {
            Texture2D tex = new Texture2D(Engine.Device, 1, 1, false, SurfaceFormat.Color);
            Color[] pixels = new Color[1];
            pixels[0] = color;
            tex.SetData<Color>(pixels);
            return tex;
        }

        public static Texture2D Generate(Color color, int x, int y)
        {
            Texture2D tex = new Texture2D(Engine.Device, x, y, false, SurfaceFormat.Color);
            Color[] pixels = new Color[x * y];
            for (int i = 0; i < pixels.Length; i++)
                pixels[i] = color;
            tex.SetData<Color>(pixels);
            return tex;
        }
    }
}
