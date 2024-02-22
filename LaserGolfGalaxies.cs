using LaserGolf.Components;
using LaserGolf.Components.Obstacles;
using LaserGolf.ConfigClasses;
using LaserGolf.Maps;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace LaserGolf
{
    public class LaserGolfGalaxies : Game
    {
        // Manager objects
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Number of players
        private int _numPlayers;

        // Map elements
        private List<LaserGolfObstacle> _obstacles;
        private Ball[] _playBalls;
        private Point _startLoc;
        private PlayScreen _screen;
        private GolfHole _hole;

        // Text
        private SpriteFont dfont;
        private int textY;

        // Sound
        private SoundEffect _holeReachedSound;
        private Song _backgroundMusic;

        /// <summary>
        /// Percentage to shrink velocity by every second 
        /// </summary>
        public readonly double FRICTION_COEFFICENT = 0.75;

        // Used for buffering input to add players
        private bool bufferAdd = true;

        // Used for tracking when to end the game
        private double endTime = -1f;

        // Function to draw a new map to the screen  and return the created map object for later use
        private Map drawMap(int? mapSelector)
        {
            Map m;
            // Create a randomly generated map
            if (mapSelector == null)
            {
                m = new Map(this, 3.0 / 4.0);
            }
            // Get a preset map
            else
            {
                m = new Map(this, (int) mapSelector, 3.0 / 4.0);
            }

            // Convert the MapElements to drawable MonoGame Objects
            for (int i = 0; i < m.Obstacles.Count; i++)
            {
                LaserGolfObstacle currObs = m.Obstacles[i];
                Components.Add(currObs);
                _obstacles.Add(currObs);
            }

            Components.Add(m.Hole);
            _hole = m.Hole;

            return m;

        }

        public LaserGolfGalaxies()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;


            _numPlayers = 1;
        }

        protected override void Initialize()
        {


            TextureContainer textureCon = new TextureContainer(this);
            Services.AddService(typeof(TextureContainer), textureCon);
            Components.Add(textureCon);

            // Create the Map to play the Game on
            _obstacles = new List<LaserGolfObstacle>();
            Map m = drawMap(null);


            // Add a ball for every player 
            _playBalls = new Ball[4];
            for(int i=0; i < 4; i++)
            {
                Ball temp = new Ball(this, m.BallPos, i);
                temp.ScaleX = 100;
                temp.ScaleY = 75;

                Components.Add(temp);
                _playBalls[i] = temp;
            }



            //  Create object to track game state elements
            // Such as player scores and the current players turn
            StateTracker tracker = new StateTracker();
            Services.AddService(typeof(StateTracker), tracker);

            _screen = new PlayScreen(1080, 810);
            Services.AddService(typeof(PlayScreen), _screen);


            _graphics.PreferredBackBufferWidth = 1080;
            _graphics.PreferredBackBufferHeight = 1010;
            _graphics.ApplyChanges();

            textY = 900;

            Window.Title = "Laser Golf Galaxies";
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);


            Services.AddService(typeof(SpriteBatch), _spriteBatch);
            base.LoadContent();

            dfont = Content.Load<SpriteFont>("posdisplayfont");

            //Set the start loc 
            _startLoc = new Point((int)System.Math.Round(_playBalls[0].Position.X), (int)System.Math.Round(_playBalls[0].Position.Y));

            // Load sounds
            _holeReachedSound = Content.Load<SoundEffect>("holeReachedSound");
            _backgroundMusic = Content.Load<Song>("backgroundMusic");
            MediaPlayer.Play(_backgroundMusic);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.3f;

        }

        protected override void Update(GameTime gameTime)
        {

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
           

            base.Update(gameTime);

            // On the first turn allow for adding players
            bool isFirst = ((StateTracker)Services.GetService(typeof(StateTracker))).isFirst;

            if (isFirst && Keyboard.GetState().IsKeyDown(Keys.X) && _numPlayers < 4 && bufferAdd)
            {
                bool[] temp =  ((StateTracker)Services.GetService(typeof(StateTracker))).PlayersActive;
                temp[_numPlayers] = true;
                ((StateTracker)Services.GetService(typeof(StateTracker))).PlayersActive = temp;
                _numPlayers += 1;
                bufferAdd = false;
            }

            if(!bufferAdd && Keyboard.GetState().IsKeyUp(Keys.X))
            {
                bufferAdd = true;
            }

            //Check collision for only the current player's  ball and the obstacles
            int currPlayer = ((StateTracker)Services.GetService(typeof(StateTracker))).CurrentPlayer;
            Ball currBall = _playBalls[currPlayer];
            for(int i = 0;i < _obstacles.Count; i++)
            {
                LaserGolfObstacle currOb = _obstacles[i];
                if (currOb.checkCollides(currBall))
                {

                    currBall.Velocity = currOb.collideWith(currBall, gameTime);
                }
            }
            _playBalls[currPlayer] = currBall;

            // Check for collision with the hole 
            if (_hole.checkWin(_playBalls[currPlayer]))
            {
                _playBalls[currPlayer].Velocity = new Vector2(0, 0);
                // Remove this player's ball
                bool[] players = ((StateTracker)Services.GetService(typeof(StateTracker))).PlayersActive;
                players[currPlayer] = false;
                ((StateTracker)Services.GetService(typeof(StateTracker))).PlayersActive = players;

                bool activeFound = false;
                // Check if all player's balls are removed
                for (int i=0; i < players.Length; i++)
                {
                    if (players[i])
                    {
                        activeFound = true;
                    }
                }

                // Play sound
                _holeReachedSound.Play();

                // If all player's set the flag to end the game
                if(activeFound == false)
                {

                    Exit();
                }

            }


        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(SpriteSortMode.BackToFront);
            // Draw the starting rectangle for the golf ball
            int rectWidth = (int) System.Math.Round((_screen.Width / 40) * 1.5);
            int rectHeight = 4;
            int rectX = (int)System.Math.Round(_startLoc.X - ((_screen.Width / 40) * 0.25));
            int rectY = (int)System.Math.Round(_startLoc.Y + ((_screen.Width / 40) * 1.1));
            Rectangle destRect = new Rectangle(rectX, rectY, rectWidth, rectHeight);




            for(int i=0; i < _numPlayers; i++)
            {
                int[] playerScores = ((StateTracker)Services.GetService(typeof(StateTracker))).Score;
                Vector2 textPos = new Vector2(50 + (i * 250), textY);

                Color c = Color.White;
                // Draw the current players score in yellow 
                if(i == ((StateTracker)Services.GetService(typeof(StateTracker))).CurrentPlayer){

                    c = Color.Yellow;
                }

                _spriteBatch.DrawString(dfont, "Player " + (i + 1) + " Score: " + playerScores[i], textPos, c);
            }

            

            _spriteBatch.Draw(((TextureContainer)Services.GetService(typeof(TextureContainer))).ColorStrip, destRect, new Rectangle(StripColors.YELLOW, 0, 1, 1), Color.Yellow);

            base.Draw(gameTime);
            _spriteBatch.End();
        }
    }
}
