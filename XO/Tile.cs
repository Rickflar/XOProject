using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Design;
using Microsoft.Xna.Framework.Content;
using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Linq;

namespace XO
{
    class Tile
    {
        public int type;
        public Texture2D tile;
        public int X, Y;
        public bool can;
        public Tile(Texture2D _tile, int _X, int _Y, int _type)
        {
            tile = _tile;
            X = _X;
            Y = _Y;
            type = _type;
            can = true;
        }
        public void Draw(SpriteBatch spriteBatch, Color color1, Color color2)
        {
            spriteBatch.Draw(tile, new Rectangle(0 + X, Y, tile.Width / 6, tile.Height / 6), can ? color1 : color2);
        }
    }
}