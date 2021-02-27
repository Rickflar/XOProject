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
    class AI
    {
        public bool turn, firstTurn;
        private Random random;
        public int[,] gameMatrix;
        public AI(int[,] _gameMatrix, bool _Turn)
        {
            gameMatrix = _gameMatrix;
            turn = _Turn;
            firstTurn = _Turn;
            random = new Random();
        }
        public Update()
        {
            if (turn)
            {
                makeTurn();
            }
        }
        private void makeTurn()
        {
            if (firstTurn)
            {
                if (random.Next(0, 2) == 1)
                {
                    gameMatrix[0, 0] = 2;
                }
                else
                {
                    gameMatrix[1, 1] = 2;
                }
            }
        }
    }
}