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
    class Player
    {
        private bool click;
        public int mX, mY;
        public Texture2D tMouse;
        public Texture2D Tile;
        public int p;
        public bool win;
        private Random random;
        public int score;
        public Player(Texture2D _texture, Texture2D _texture2, int player)
        {
            tMouse = _texture;
            Tile = _texture2;
            p = player;
            win = false;
            random = new Random();
            score = 0;
        }
        public void Draw(SpriteBatch spriteBatch, ref bool turn)
        {
                spriteBatch.Draw(tMouse, new Rectangle(mX - tMouse.Width / 16 + 10, mY - tMouse.Height / 16 + 10, tMouse.Width / 8, tMouse.Height / 8), turn? new Color(255, 169, 169, 255): new Color(164, 218, 255, 255));
        }
        public void KeyboardHandler(Texture2D _move, Texture2D _click, ref bool turn, ref List<List<Tile>> _Field, ref bool firstturn, ref bool endgame, ref bool pwin, ref bool darktheme, ref bool gamestarted, ref bool mode, ref float time)
        {
            if (click)
            {
                tMouse = _click;
            }
            else
            {
                tMouse = _move;
            }
            MouseState mState = Mouse.GetState();
            mX = mState.X;
            mY = mState.Y;
            if (mState.LeftButton == ButtonState.Pressed)
            {
                click = true;        
            }
            else if (mState.LeftButton == ButtonState.Released)
            {
                click = false;
            }
            if (gamestarted)
            {
                if (endgame)
                {
                    if (mState.RightButton == ButtonState.Pressed)
                    {
                        Clean(ref _Field);
                        endgame = false;
                        win = false;
                        pwin = false;
                        turn = random.Next(0, 2) == 0 ? true : false;
                        firstturn = true;
                        if (!mode)
                        {
                            score++;
                        }
                        else
                        {
                            time = 60;
                        }
                    }
                }
                else
                {
                    if (win == false && p == 1 ? turn : !turn)
                    {
                        for (int i = 0; i < 14; i++)
                        {
                            for (int j = 0; j < 14; j++)
                            {
                                if ((mState.LeftButton == ButtonState.Pressed) && ((mX > _Field[i][j].X && mX < _Field[i][j].X + _Field[i][j].tile.Width / 6) && (mY > _Field[i][j].Y && mY < _Field[i][j].Y + _Field[i][j].tile.Width / 6)))
                                {
                                    if (_Field[i][j].type == 0 && _Field[i][j].can)
                                    {
                                        _Field[i][j].type = turn ? 1 : 2;
                                        if (mode)
                                        {
                                            score++;
                                        }
                                        int tempX = i;
                                        int tempY = j;
                                        for (int k = 0; k < 14; k++)
                                        {
                                            for (int l = 0; l < 14; l++)
                                            {
                                                if (firstturn)
                                                {
                                                    if ((Math.Abs(k - tempX) > 2 || Math.Abs(l - tempY) > 2) && _Field[k][l].type == 0)
                                                    {
                                                        _Field[k][l].can = false;
                                                    }
                                                    else
                                                    {
                                                        _Field[k][l].can = true;
                                                    }
                                                }
                                                else
                                                {
                                                    if ((Math.Abs(k - tempX) <= 2 && Math.Abs(l - tempY) <= 2))
                                                    {
                                                        _Field[k][l].can = true;
                                                    }

                                                }

                                            }
                                        }
                                        firstturn = false;
                                        turn = !turn;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        private void Clean(ref List<List<Tile>> _Field)
        {
            for (int i = 0; i < 14; i++)
            {
                for (int j = 0; j < 14; j++)
                {
                    _Field[i][j].type = 0;
                    _Field[i][j].can = true;
                }
            }
        }
    }
}