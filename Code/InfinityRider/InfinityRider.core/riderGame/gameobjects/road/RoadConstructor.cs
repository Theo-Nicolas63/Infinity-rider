using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace InfinityRider.core.RiderGame.GameObjects.Road
{
    /// <summary>
    /// A class to display a Road on a game
    /// </summary>
    class RoadConstructor : GameObject
    {
        /// <summary>
        /// The width of the window
        /// </summary>
        private int _screenWidth;
        /// <summary>
        /// The height of the window
        /// </summary>
        private int _screenHeight;
        /// <summary>
        /// The texture of the road currently displaied
        /// </summary>
        private Texture2D _foregroundTexture;
        /// <summary>
        /// The level that have created this instance of class
        /// </summary>
        Level _level;
        /// <summary>
        /// The speed of the road that are displaied, to move it
        /// </summary>
        private float SpeedMove { get; set; } = 0;
        /// <summary>
        /// The color of the road
        /// </summary>
        public Color MapColor { get; set; } = Color.Green;
        /// <summary>
        /// The instance of RoadManager that are being used to create the road
        /// </summary>
        private RoadManager Road { get; set; }
        /// <summary>
        /// An array that contains the position of the current road, with the index for abscissa and the value for ordinate, but from the top of the screen
        /// </summary>
        private int[] TerrainContour;

        /// <summary>
        /// A constructor of the class RoadConstructor
        /// </summary>
        /// <param name="level">The level that have created this instance of class</param>
        /// <param name="game">The Game instance that contains all the game</param>
        /// <param name="spriteBatch">The spriteBatch instance of the game</param>
        public RoadConstructor(Level level, Game game, SpriteBatch spriteBatch) : base(game, spriteBatch)
        {
            _game = game;
            _device = game.GraphicsDevice;
            _level = level;
        }

        /// <summary>
        /// A method that loads some contents for this class
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            if (_device != null)
            {
                _screenWidth = _device.Viewport.Width;
                _screenHeight = _device.Viewport.Height;

                GenerateTerrainContour();
                CreateForeGround();
            }
        }

        /// <summary>
        /// A method that updates some contents for this class
        /// </summary>
        /// <param name="gameTime">The instance of GameTime of the game</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            var keyBoardState = Keyboard.GetState();

            if (keyBoardState.IsKeyDown(Keys.A))
            {
                applyPhysics(gameTime);
                if (SpeedMove < 450f)
                    SpeedMove += 5;
            }
            else
            {
                applyPhysics(gameTime);
                if (SpeedMove > 5f)
                    SpeedMove -= 5f;
            }

            if (Road != null) Road.NextPosition(SpeedMove * (float)gameTime.ElapsedGameTime.TotalSeconds);

            if (_game != null)
            {
                _screenWidth = _device.Viewport.Width;
                _screenHeight = _device.Viewport.Height;

                GenerateTerrainContour();
                CreateForeGround();
            }
        }

        /// <summary>
        /// A method that draws the road
        /// </summary>
        /// <param name="gameTime">The instance of GameTime of the game</param>
        /// <param name="spriteBatch">The spriteBatch instance of the game</param>
        public override void Draw(GameTime gameTime, SpriteBatch _spriteBatch)
        {
            Rectangle screenRectangle = new Rectangle(0, 0, _screenWidth, _screenHeight);

            if (_foregroundTexture != null)
            {
                _spriteBatch.Draw(_foregroundTexture, screenRectangle, Color.White);
            }

            base.Draw(gameTime);
        }

        /// <summary>
        /// A method to get the value of the height of the road at a precise index
        /// </summary>
        /// <param name="index">The index for abscissa</param>
        /// <returns>The value for ordinate, but from the top of the screen</returns>
        public int getTerrainContour(int index)
        {
            return this.TerrainContour[index];
        }

        /// <summary>
        /// A method that create the road from the instance of RoadManager
        /// </summary>
        private void GenerateTerrainContour()
        {
            if (Road == null)
            {
                float offset = 600 * 5 / 8;
                float peakHeight = 75;
                float flatness = 200;
                Road = new RoadManager(offset, peakHeight, flatness);
            }
            TerrainContour = Road.GetCurrentRoad(_screenWidth);
        }

        /// <summary>
        /// A method that generate the texture of the road currently displaied
        /// </summary>
        private void CreateForeGround()
        {
            Color[] foreGroundColors = new Color[_screenWidth * _screenHeight];

            for (int x = 0; x < _screenWidth; x++)
            {
                for (int y = 0; y < _screenHeight; y++)
                {
                    if (y > TerrainContour[x] && y < TerrainContour[x] + 10)
                    {
                        foreGroundColors[x + y * _screenWidth] = MapColor;
                    }
                    else
                    {
                        foreGroundColors[x + y * _screenWidth] = Color.Transparent;
                    }

                    if (y >= TerrainContour[x] - 5 && y <= TerrainContour[x] || y >= TerrainContour[x] + 10 && y <= TerrainContour[x] + 15)
                    {
                        foreGroundColors[x + y * _screenWidth] = Color.FromNonPremultiplied(MapColor.R, MapColor.G, MapColor.B, 100);
                    }
                }
            }

            _foregroundTexture = new Texture2D(_device, _screenWidth, _screenHeight, false, SurfaceFormat.Color);
            _foregroundTexture.SetData(foreGroundColors);
        }

        /// <summary>
        /// A method that use the instance of Level within this class to know if there are a collision
        /// </summary>
        /// <param name="gameTime">The instance of GameTime of the game</param>
        public void applyPhysics(GameTime gameTime)
        {
            _level.IsCollisionForward(gameTime);
        }

        /// <summary>
        /// A method to reinitialize the road, for instance, for a new game
        /// </summary>
        public void ReinitializeRoad()
        {
            Road.ReinitializeRoad();
        }
    }
}
