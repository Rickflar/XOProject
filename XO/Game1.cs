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
    enum GameState
    {
        MainMenu,
        HowToPlayMenu,
        Gameplay,
    }
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private SpriteFont Font, H2Font;
        private List<Texture2D> Tiles = new List<Texture2D>();
        private List<List<Tile>> Field = new List<List<Tile>>();
        private Random random;
        private bool turn;
        private Color fieldColor;
        private Player player, player2;
        private int x,y, size;
        private bool firstturn, endgame,darktheme, gamestarted, mode,tempmode;
        GameState _state;
        private int counter1 = 0;
        private int counter2 = 0;
        private float timeleft = 60;
        
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            x = 390;
            y = 165;
            random = new Random();
            size = 14;
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Tiles.Add(Content.Load<Texture2D>("Tiles/X"));
            Tiles.Add(Content.Load<Texture2D>("Tiles/O"));
            Tiles.Add(Content.Load<Texture2D>("Tiles/Tile"));
            Tiles.Add(Content.Load<Texture2D>("Mouse/mouse"));
            Tiles.Add(Content.Load<Texture2D>("Mouse/mouse2"));
            Tiles.Add(Content.Load<Texture2D>("Tiles/Menu"));
            Tiles.Add(Content.Load<Texture2D>("Tiles/Back"));
            Tiles.Add(Content.Load<Texture2D>("Tiles/Switcher"));
            Tiles.Add(Content.Load<Texture2D>("Tiles/Point"));
            Font = Content.Load<SpriteFont>("Fonts/Font");
            H2Font = Content.Load<SpriteFont>("Fonts/H2Font");
            int X = x;
            int Y = y;
            for (int i = 0; i < size; i++)
            {
                List<Tile> Row = new List<Tile>();
                Field.Add(Row);
                for (int j = 0; j < size; j++)
                {
                    Field[i].Add(new Tile(Tiles[2], X, Y, 0));
                    X += Tiles[2].Width / 6 + 10;
                }
                X = x;
                Y += Tiles[2].Height / 6 + 10;
            }
            player = new Player(Tiles[3], Tiles[2], 1);
            player2 = new Player(Tiles[3], Tiles[2], 2);
            firstturn = true;
            endgame = false;
            darktheme = false;
            gamestarted = false;
            mode = false;
            tempmode = mode;
        }
        protected override void Initialize()
        {
            turn = random.Next(0, 2) == 0 ? true : false;
            fieldColor = new Color(235, 235, 235, 255);
            graphics.PreferredBackBufferWidth = 1366;
            graphics.PreferredBackBufferHeight = 768;
            graphics.ToggleFullScreen();
            graphics.ApplyChanges();
            base.Initialize();
            _state = GameState.MainMenu;
        }
        protected void UpdateMainMenu(GameTime gameTime)
        {
            counter2++;
            MainMenuKeyboardHandler();
        }
        protected void UpdateHowToPlayMenu(GameTime gameTime)
        {
            HowToPlayHeyboardHandler();
        }
        protected void UpdateGameplay(GameTime gameTime)
        {
            if (mode)
            {
                if (timeleft > 0)
                {
                    timeleft -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
            counter1++;
            if (_state == GameState.Gameplay)
            {
                if (counter1 == 10)
                {
                    gamestarted = true;
                }
            }
            KeyboardState kState = Keyboard.GetState();
            if (kState.IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            if (turn)
            {
                player.KeyboardHandler(Tiles[4], Tiles[3], ref turn, ref Field, ref firstturn, ref endgame, ref player2.win, ref darktheme, ref gamestarted, ref mode, ref timeleft);
                Check();
                if (player.win)
                {
                    turn = true;
                }
            }
            else
            {
                player2.KeyboardHandler(Tiles[4], Tiles[3], ref turn, ref Field, ref firstturn, ref endgame, ref player.win, ref darktheme, ref gamestarted, ref mode, ref timeleft);
                Check();
                if (player2.win)
                {
                    turn = false;
                }
            }
            KeyboardHandler();
            base.Update(gameTime);
        }
        protected override void Update(GameTime gameTime)
        {
            switch (_state)
            {
                case GameState.MainMenu:
                    UpdateMainMenu(gameTime);
                    break;
                case GameState.HowToPlayMenu:
                    UpdateHowToPlayMenu(gameTime);
                    break;
                case GameState.Gameplay:
                    UpdateGameplay(gameTime);
                    break;
            }
        }
        protected void MainMenuKeyboardHandler()
        {
            MouseState State = Mouse.GetState();
            if (State.LeftButton == ButtonState.Pressed && ((State.X > (int)GraphicsDevice.PresentationParameters.BackBufferWidth / 2 - Tiles[0].Width / 2 + 20) && (State.X < (int)GraphicsDevice.PresentationParameters.BackBufferWidth / 2 - 20) && (State.Y > 0 && State.Y < 100)))
            {
                darktheme = true;
            }
            else if (State.LeftButton == ButtonState.Pressed && ((State.X > (int)GraphicsDevice.PresentationParameters.BackBufferWidth / 2 - 20) && (State.X < (int)GraphicsDevice.PresentationParameters.BackBufferWidth / 2 + Tiles[0].Width / 2) && (State.Y > 0 && State.Y < 100)))
            {
                darktheme = false;
            }
            else if (State.LeftButton == ButtonState.Pressed && ((State.X > (int)GraphicsDevice.PresentationParameters.BackBufferWidth / 2 - Tiles[5].Width / 4) && (State.X < (int)GraphicsDevice.PresentationParameters.BackBufferWidth / 2 + Tiles[5].Width / 4) && (State.Y > (int)GraphicsDevice.PresentationParameters.BackBufferHeight / 2 - Tiles[5].Width / 4 && State.Y < (int)GraphicsDevice.PresentationParameters.BackBufferHeight / 2)))
            {
                _state = GameState.Gameplay;
            }
            else if (State.LeftButton == ButtonState.Pressed && ((State.X > (int)GraphicsDevice.PresentationParameters.BackBufferWidth / 2 - Tiles[5].Width / 4) && (State.X < (int)GraphicsDevice.PresentationParameters.BackBufferWidth / 2) && (State.Y > (int)GraphicsDevice.PresentationParameters.BackBufferHeight / 2 && State.Y < (int)GraphicsDevice.PresentationParameters.BackBufferHeight / 2 + Tiles[5].Width / 4)))
            {
                _state = GameState.HowToPlayMenu;
            }
            else if (State.LeftButton == ButtonState.Pressed && ((State.X > (int)GraphicsDevice.PresentationParameters.BackBufferWidth / 2 ) && (State.X < (int)GraphicsDevice.PresentationParameters.BackBufferWidth / 2 + Tiles[5].Width / 4) && (State.Y > (int)GraphicsDevice.PresentationParameters.BackBufferHeight / 2 && State.Y < (int)GraphicsDevice.PresentationParameters.BackBufferHeight / 2 + Tiles[5].Width / 4)))
            {
                Exit();
            }
            else if (State.LeftButton == ButtonState.Pressed && ((State.X > (int)GraphicsDevice.PresentationParameters.BackBufferWidth / 2 - Tiles[7].Width / 4) && (State.X < (int)GraphicsDevice.PresentationParameters.BackBufferWidth / 2 + Tiles[7].Width / 4) && (State.Y > (int)GraphicsDevice.PresentationParameters.BackBufferHeight - Tiles[7].Width / 4 && State.Y < (int)GraphicsDevice.PresentationParameters.BackBufferHeight + Tiles[7].Width / 4-40)))
            {
                if (counter2 % 5 == 0)
                {
                    mode = !mode;
                }
            }
            if (turn)
            {
                player.KeyboardHandler(Tiles[4], Tiles[3], ref turn, ref Field, ref firstturn, ref endgame, ref player2.win, ref darktheme, ref gamestarted, ref mode, ref timeleft);
            }
            else
            {
                player2.KeyboardHandler(Tiles[4], Tiles[3], ref turn, ref Field, ref firstturn, ref endgame, ref player.win, ref darktheme, ref gamestarted, ref mode, ref timeleft);
            }
            tempmode = mode;
        }
        protected void HowToPlayHeyboardHandler()
        {
            MouseState State = Mouse.GetState();
            if (State.LeftButton == ButtonState.Pressed && ((State.X > (int)GraphicsDevice.PresentationParameters.BackBufferWidth / 2 - Tiles[0].Width / 2 + 20) && (State.X < (int)GraphicsDevice.PresentationParameters.BackBufferWidth / 2 - 20) && (State.Y > 0 && State.Y < 100)))
            {
                darktheme = true;
            }
            else if (State.LeftButton == ButtonState.Pressed && ((State.X > (int)GraphicsDevice.PresentationParameters.BackBufferWidth / 2 - 20) && (State.X < (int)GraphicsDevice.PresentationParameters.BackBufferWidth / 2 + Tiles[0].Width / 2) && (State.Y > 0 && State.Y < 100)))
            {
                darktheme = false;
            }
            else if (State.LeftButton == ButtonState.Pressed && ((State.X > (int)GraphicsDevice.PresentationParameters.BackBufferWidth / 2 - Tiles[6].Width / 4) && (State.X < (int)GraphicsDevice.PresentationParameters.BackBufferWidth / 2 + Tiles[6].Width / 4) && (State.Y > 660  && State.Y < 860)))
            {
                _state = GameState.MainMenu;
                mode = tempmode;
            }
            if (turn)
            {
                player.KeyboardHandler(Tiles[4], Tiles[3], ref turn, ref Field, ref firstturn, ref endgame, ref player2.win, ref darktheme, ref gamestarted, ref mode, ref timeleft);
            }
            else
            {
                player2.KeyboardHandler(Tiles[4], Tiles[3], ref turn, ref Field, ref firstturn, ref endgame, ref player.win, ref darktheme, ref gamestarted, ref mode, ref timeleft);
            }
        }
        protected void GameplayKeyboardHandler()
        {            
            MouseState State = Mouse.GetState();
            if(State.LeftButton == ButtonState.Pressed&&((State.X> (int)GraphicsDevice.PresentationParameters.BackBufferWidth / 2 - Tiles[0].Width / 2 + 20)&&(State.X< (int)GraphicsDevice.PresentationParameters.BackBufferWidth / 2 - 20) && (State.Y > 0 && State.Y < 100))){
                darktheme = true;
            }
            else if (State.LeftButton == ButtonState.Pressed && ((State.X > (int)GraphicsDevice.PresentationParameters.BackBufferWidth / 2 - 20) && (State.X < (int)GraphicsDevice.PresentationParameters.BackBufferWidth / 2 + Tiles[0].Width / 2) && (State.Y > 0 && State.Y < 100))){
                darktheme = false;
            }
        }
        protected void KeyboardHandler()
        {
            switch (_state)
            {
                case GameState.MainMenu:
                    MainMenuKeyboardHandler();
                    break;
                case GameState.HowToPlayMenu:
                    HowToPlayHeyboardHandler();
                    break;
                case GameState.Gameplay:
                    GameplayKeyboardHandler();
                    break;
            }
        }
        protected void MainMenuDraw(GameTime gameTime)
        {
            if (darktheme)
            {
                GraphicsDevice.Clear(new Color(27, 27, 27));
            }
            else
            {
                GraphicsDevice.Clear(Color.White);

            }
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            spriteBatch.Draw(Tiles[0], new Rectangle((int)GraphicsDevice.PresentationParameters.BackBufferWidth / 2 - Tiles[0].Width / 2 + 20, 10, Tiles[0].Width / 2, Tiles[0].Height / 2), new Color(244, 121, 121, 255));
            spriteBatch.Draw(Tiles[1], new Rectangle((int)GraphicsDevice.PresentationParameters.BackBufferWidth / 2 - 20, 10, Tiles[1].Width / 2, Tiles[1].Height / 2), new Color(129, 195, 241, 255));
            MouseState State = Mouse.GetState();           
            spriteBatch.Draw(Tiles[5], new Rectangle((int)GraphicsDevice.PresentationParameters.BackBufferWidth / 2 - Tiles[5].Width / 4, (int)GraphicsDevice.PresentationParameters.BackBufferHeight / 2 - Tiles[5].Width / 4, Tiles[5].Width / 2, Tiles[5].Height / 2), Color.White);
            spriteBatch.Draw(Tiles[7], new Rectangle((int)GraphicsDevice.PresentationParameters.BackBufferWidth / 2 - Tiles[7].Width / 4, (int)GraphicsDevice.PresentationParameters.BackBufferHeight - Tiles[7].Width / 4, Tiles[7].Width / 2, Tiles[7].Height / 2), darktheme ? new Color(51, 51, 51, 255) : new Color(235, 235, 235, 255));
            spriteBatch.Draw(Tiles[8], new Rectangle((mode?(int)GraphicsDevice.PresentationParameters.BackBufferWidth / 2 + 52 : (int)GraphicsDevice.PresentationParameters.BackBufferWidth / 2) - Tiles[8].Width / 4, (int)GraphicsDevice.PresentationParameters.BackBufferHeight - Tiles[8].Width / 4, Tiles[8].Width / 2, Tiles[8].Height / 2), mode? new Color(132, 250, 147, 255) : new Color(244, 121, 121, 255));
            spriteBatch.DrawString(Font, (mode? "Режим на время" :"Классический режим"), new Vector2((int)GraphicsDevice.PresentationParameters.BackBufferWidth / 2 - Font.MeasureString(mode ? "Режим на время" : "Классический режим").X / 2+(mode?200:-250), (int)GraphicsDevice.PresentationParameters.BackBufferHeight - Tiles[7].Width / 4+30), darktheme ? new Color(51, 51, 51, 255) : new Color(235, 235, 235, 255));
            spriteBatch.DrawString(Font, "v.1.0", new Vector2(5, (int)GraphicsDevice.PresentationParameters.BackBufferHeight - 35), darktheme ? new Color(51, 51, 51, 255) : new Color(235, 235, 235, 255));
            if (turn)
            {
                player.Draw(spriteBatch, ref turn);
            }
            else
            {
                player2.Draw(spriteBatch, ref turn);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
        protected void HowToPlayDraw(GameTime gameTime)
        {
            if (darktheme)
            {
                GraphicsDevice.Clear(new Color(27, 27, 27));
            }
            else
            {
                GraphicsDevice.Clear(Color.White);

            }
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            spriteBatch.Draw(Tiles[0], new Rectangle((int)GraphicsDevice.PresentationParameters.BackBufferWidth / 2 - Tiles[0].Width / 2 + 20, 10, Tiles[0].Width / 2, Tiles[0].Height / 2), new Color(244, 121, 121, 255));
            spriteBatch.Draw(Tiles[1], new Rectangle((int)GraphicsDevice.PresentationParameters.BackBufferWidth / 2 - 20, 10, Tiles[1].Width / 2, Tiles[1].Height / 2), new Color(129, 195, 241, 255));
            MouseState State = Mouse.GetState();
            spriteBatch.DrawString(Font, "XO - это старые добрые крестики-нолики на нестандартном поле 14х14.", new Vector2((int)GraphicsDevice.PresentationParameters.BackBufferWidth / 2 - Font.MeasureString("XO - это старые добрые крестики-нолики на нестандартном поле 14х14.").X / 2, 180), darktheme ? new Color(51, 51, 51, 255) : new Color(235, 235, 235, 255));
            spriteBatch.DrawString(Font, "Играйте в классический режим, в котором вам предстоит собрать пять в ряд ", new Vector2((int)GraphicsDevice.PresentationParameters.BackBufferWidth / 2 - Font.MeasureString("Играйте в классический режим, в котором вам предстоит собрать пять в ряд ").X / 2, 220), darktheme ? new Color(51, 51, 51, 255) : new Color(235, 235, 235, 255));
            spriteBatch.DrawString(Font, "или в режим на время, где необходимо собрать как можно больше очков.", new Vector2((int)GraphicsDevice.PresentationParameters.BackBufferWidth / 2 - Font.MeasureString("или в режим на время, где необходимо собрать как можно больше очков.").X / 2, 260), darktheme ? new Color(51, 51, 51, 255) : new Color(235, 235, 235, 255));
            spriteBatch.DrawString(Font, "Управление максимально простое:", new Vector2((int)GraphicsDevice.PresentationParameters.BackBufferWidth / 2 - Font.MeasureString("Управление максимально простое:").X / 2, 340), darktheme ? new Color(51, 51, 51, 255) : new Color(235, 235, 235, 255));
            spriteBatch.DrawString(Font, "Используйте мышь для управления ходом игры, ", new Vector2((int)GraphicsDevice.PresentationParameters.BackBufferWidth / 2 - Font.MeasureString("Используйте мышь для управления ходом игры, ").X / 2, 380), darktheme ? new Color(51, 51, 51, 255) : new Color(235, 235, 235, 255));
            spriteBatch.DrawString(Font, "А также клавишу ESC для выхода из игры.", new Vector2((int)GraphicsDevice.PresentationParameters.BackBufferWidth / 2 - Font.MeasureString("А также клавишу ESC для выхода из игры.").X / 2, 420), darktheme ? new Color(51, 51, 51, 255) : new Color(235, 235, 235, 255));
            spriteBatch.DrawString(Font, "Также, опробуйте темную тему, кликнув на логотип игры!", new Vector2((int)GraphicsDevice.PresentationParameters.BackBufferWidth / 2 - Font.MeasureString("Также, опробуйте темную тему, кликнув на логотип игры!").X / 2, 500), darktheme ? new Color(51, 51, 51, 255) : new Color(235, 235, 235, 255));
            spriteBatch.DrawString(Font, "Приятной игры!", new Vector2((int)GraphicsDevice.PresentationParameters.BackBufferWidth / 2 - Font.MeasureString("Приятной игры!").X / 2, 580), darktheme ? new Color(51, 51, 51, 255) : new Color(235, 235, 235, 255));
            spriteBatch.Draw(Tiles[6], new Rectangle((int)GraphicsDevice.PresentationParameters.BackBufferWidth / 2 - Tiles[6].Width/4, 660, Tiles[6].Width / 2, Tiles[6].Height / 2), Color.White);
            if (turn)
            {
                player.Draw(spriteBatch, ref turn);
            }
            else
            {
                player2.Draw(spriteBatch, ref turn);
            }
            spriteBatch.DrawString(Font, "v.1.0", new Vector2(5, (int)GraphicsDevice.PresentationParameters.BackBufferHeight - 35), darktheme ? new Color(51, 51, 51, 255) : new Color(235, 235, 235, 255));
            spriteBatch.End();
            base.Draw(gameTime);
        }
        protected void GameplayDraw(GameTime gameTime)
        {
            if (darktheme)
            {
                GraphicsDevice.Clear(new Color(27,27,27));
            }
            else
            {
                GraphicsDevice.Clear(Color.White);
                
            }
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            spriteBatch.Draw(Tiles[0], new Rectangle((int)GraphicsDevice.PresentationParameters.BackBufferWidth / 2 - Tiles[0].Width / 2 + 20, 10, Tiles[0].Width / 2, Tiles[0].Height / 2), new Color(244, 121, 121, 255));
            spriteBatch.Draw(Tiles[1], new Rectangle((int)GraphicsDevice.PresentationParameters.BackBufferWidth / 2 - 20, 10, Tiles[1].Width / 2, Tiles[1].Height / 2), new Color(129, 195, 241, 255));
            spriteBatch.DrawString(Font, "ИГРОК 1", new Vector2(10, 10), new Color(244, 121, 121, 255));
            spriteBatch.DrawString(H2Font, player.score.ToString(), new Vector2(10, 30), new Color(244, 121, 121, 255));
            spriteBatch.DrawString(Font, "ИГРОК 2", new Vector2((int)GraphicsDevice.PresentationParameters.BackBufferWidth - Font.MeasureString("ИГРОК 2").X-10, 10), new Color(129, 195, 241, 255));
            spriteBatch.DrawString(H2Font, player2.score.ToString(), new Vector2((int)GraphicsDevice.PresentationParameters.BackBufferWidth - Font.MeasureString(player2.score.ToString()).X-(player2.score>9? 80:40), 30), new Color(129, 195, 241, 255));
            if (mode)
            {
                spriteBatch.DrawString(Font, string.Concat(((int)timeleft).ToString(), " секунд"), new Vector2((int)GraphicsDevice.PresentationParameters.BackBufferWidth / 2 - Font.MeasureString(string.Concat(((int)timeleft).ToString(), " секунд")).X / 2, 90), darktheme ? new Color(51, 51, 51, 255) : new Color(235, 235, 235, 255));
            }
            if (player.win)
            {
                spriteBatch.DrawString(Font, "ПОБЕДИЛ ПЕРВЫЙ ИГРОК", new Vector2((int)GraphicsDevice.PresentationParameters.BackBufferWidth / 2 - Font.MeasureString("ПОБЕДИЛ ПЕРВЫЙ ИГРОК").X / 2, 130), new Color(244, 121, 121, 255));
            }
            else if (player2.win)
            {
                spriteBatch.DrawString(Font, "ПОБЕДИЛ ВТОРОЙ ИГРОК", new Vector2((int)GraphicsDevice.PresentationParameters.BackBufferWidth / 2 - Font.MeasureString("ПОБЕДИЛ ВТОРОЙ ИГРОК").X / 2, 130), new Color(129, 195, 241, 255));
            }
            else
            {
                spriteBatch.DrawString(Font, turn ? "ХОД ПЕРВОГО ИГРОКА" : "ХОД ВТОРОГО ИГРОКА", new Vector2((int)GraphicsDevice.PresentationParameters.BackBufferWidth / 2 - Font.MeasureString(turn ? "ХОД ПЕРВОГО ИГРОКА" : "ХОД ВТОРОГО ИГРОКА").X / 2, 130), turn ? new Color(244, 121, 121, 255) : new Color(129, 195, 241, 255));
            }
            MouseState State = Mouse.GetState();
            int X = x;
            int Y = y;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (darktheme)
                    {
                        Field[i][j].Draw(spriteBatch, new Color(51, 51, 51, 255), new Color(35, 35, 35, 255));
                    }
                    else
                    {
                        Field[i][j].Draw(spriteBatch, new Color(235, 235, 235, 255), new Color(215, 215, 215, 255));
                    }
                    if (Field[i][j].type == 1)
                    {
                        spriteBatch.Draw(Tiles[0], new Rectangle(0 + X, Y, Tiles[0].Width / 6, Tiles[0].Height / 6), new Color(244, 121, 121, 255));
                    }
                    else if (Field[i][j].type == 2)
                    {
                        spriteBatch.Draw(Tiles[1], new Rectangle(0 + X, Y, Tiles[1].Width / 6, Tiles[1].Height / 6), new Color(129, 195, 241, 255));
                    }
                    X += Tiles[2].Width / 6 + 10;
                }
                X = x;
                Y += Tiles[2].Height / 6 + 10;
                if (turn)
                {
                    player.Draw(spriteBatch, ref turn);
                }
                else
                {
                    player2.Draw(spriteBatch, ref turn);
                }

            }
            spriteBatch.DrawString(Font, "v.1.0", new Vector2(5, (int)GraphicsDevice.PresentationParameters.BackBufferHeight - 35), darktheme ? new Color(51, 51, 51, 255) : new Color(235, 235, 235, 255));
            spriteBatch.End();
            base.Draw(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            switch (_state)
            {
                case GameState.MainMenu:
                    MainMenuDraw(gameTime);
                    break;
                case GameState.HowToPlayMenu:
                    HowToPlayDraw(gameTime);
                    break;
                case GameState.Gameplay:
                    GameplayDraw(gameTime);
                    break;
            }
        }
        private void Check()
        {
            int temp1 = 0;
            int temp2 = 0;
            //строки
            for (int i = 0; i < 14; i++)
            {
                for (int j = 0; j < 14; j++)
                {
                    if (Field[i][j].type == 1)
                    {
                        temp1++;
                    }
                    else
                    {
                        temp1 = 0;
                    }
                    if (Field[i][j].type == 2)
                    {
                        temp2++;
                    }
                    else
                    {
                        temp2 = 0;
                    }
                    if (!mode)
                    {
                        if (temp1 >= 5)
                        {
                            player.win = true;
                            endgame = true;
                            break;
                        }
                        else if (temp2 >= 5)
                        {
                            player2.win = true;
                            endgame = true;
                            break;
                        }
                    }
                    else
                    {
                        if (timeleft > 0)
                        {
                            if (temp1 >= 5)
                            {
                                if (counter1 % 100 == 0)
                                {
                                    player.score += 5;
                                }
                                break;
                            }
                            else if (temp2 >= 5)
                            {
                                if (counter1 % 100 == 0)
                                {
                                    player2.score += 5;
                                }
                            }
                        }
                        else
                        {
                            if (player.score >= player2.score)
                            {
                                player.win = true;
                                endgame = true;
                                break;
                            }
                            else if (player2.score > player.score)
                            {
                                player2.win = true;
                                endgame = true;
                                break;
                            }
                        }
                    }
                }
            }
            //столбцы
            for (int j = 0; j < 14; j++)
            {
                for (int i = 0; i < 14; i++)
                {
                    if (Field[i][j].type == 1)
                    {
                        temp1++;
                    }
                    else
                    {
                        temp1 = 0;
                    }
                    if (Field[i][j].type == 2)
                    {
                        temp2++;
                    }
                    else
                    {
                        temp2 = 0;
                    }
                    if (!mode)
                    {
                        if (temp1 >= 5)
                        {
                            player.win = true;
                            endgame = true;
                            break;
                        }
                        else if (temp2 >= 5)
                        {
                            player2.win = true;
                            endgame = true;
                            break;
                        }
                    }
                    else
                    {
                        if (timeleft > 0)
                        {
                            if (temp1 >= 5)
                            {
                                if (counter1% 100 == 0)
                                {
                                    player.score += 5;
                                }
                                break;
                            }
                            else if (temp2 >= 5)
                            {
                                if (counter1 % 100 == 0)
                                {
                                    player2.score += 5;
                                }
                            }
                        }
                        else
                        {
                            if (player.score >= player2.score)
                            {
                                player.win = true;
                                endgame = true;
                                break;
                            }
                            else if (player2.score > player.score)
                            {
                                player2.win = true;
                                endgame = true;
                                break;
                            }
                        }
                    }
                }
            }
            //первая диагональ
            for (int j = 0; j < 14; j++)
            {
                for (int i = 0; i < 14; i++)
                {
                    if (i + j < 14)
                    {
                        if (Field[i][i + j].type == 1)
                        {
                            temp1++;
                        }
                        else
                        {
                            temp1 = 0;
                        }
                        if (Field[i][i + j].type == 2)
                        {
                            temp2++;
                        }
                        else
                        {
                            temp2 = 0;
                        }
                        if (!mode)
                        {
                            if (temp1 >= 5)
                            {
                                player.win = true;
                                endgame = true;
                                break;
                            }
                            else if (temp2 >= 5)
                            {
                                player2.win = true;
                                endgame = true;
                                break;
                            }
                        }
                        else
                        {
                            if (timeleft > 0)
                            {
                                if (temp1 >= 5)
                                {
                                    if (counter1 % 100 == 0)
                                    {
                                        player.score += 5;
                                    }
                                    break;
                                }
                                else if (temp2 >= 5)
                                {
                                    if (counter1 % 100 == 0)
                                    {
                                        player2.score += 5;
                                    }
                                }
                            }
                            else
                            {
                                if (player.score >= player2.score)
                                {
                                    player.win = true;
                                    endgame = true;
                                    break;
                                }
                                else if (player2.score > player.score)
                                {
                                    player2.win = true;
                                    endgame = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            //вторая диагональ
            for (int j = 13; j >= 0; j--)
            {
                for (int i = 13; i >= 0; i--)
                {
                    if (j - i > 0)
                    {
                        if (Field[i][j - i].type == 1)
                        {
                            temp1++;
                        }
                        else
                        {
                            temp1 = 0;
                        }
                        if (Field[i][j - i].type == 2)
                        {
                            temp2++;
                        }
                        else
                        {
                            temp2 = 0;
                        }
                        if (!mode)
                        {
                            if (temp1 >= 5)
                            {
                                player.win = true;
                                endgame = true;
                                break;
                            }
                            else if (temp2 >= 5)
                            {
                                player2.win = true;
                                endgame = true;
                                break;
                            }
                        }
                        else
                        {
                            if (timeleft > 0)
                            {
                                if (temp1 >= 5)
                                {
                                    if (counter1 % 100 == 0)
                                    {
                                        player.score += 5;
                                    }
                                    break;
                                }
                                else if (temp2 >= 5)
                                {
                                    if (counter1 % 100 == 0)
                                    {
                                        player2.score += 5;
                                    }
                                }
                            }
                            else
                            {
                                if (player.score >= player2.score)
                                {
                                    player.win = true;
                                    endgame = true;
                                    break;
                                }
                                else if (player2.score > player.score)
                                {
                                    player2.win = true;
                                    endgame = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
