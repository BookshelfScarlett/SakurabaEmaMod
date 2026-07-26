using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SakurabaEmaMod.Globals.Avator
{
    public struct AvatorSettings(Texture2D avatorTexture,Color backgroundColor, float scale = 1, Color? backgroundEdgeColor = null, float multboxSpacing = 0f)
    {
        public Color BackgroundColor = backgroundColor;
        public Color? BackgroundEdgeColor = backgroundEdgeColor; 
        public float MultboxSpacing = multboxSpacing;
        public float Scale = scale;
        public Texture2D AvatorTexture = avatorTexture;
    }
}
