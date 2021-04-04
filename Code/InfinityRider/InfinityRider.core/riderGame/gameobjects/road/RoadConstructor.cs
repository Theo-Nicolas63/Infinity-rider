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

        public RoadConstructor(Level level, Game game, SpriteBatch spriteBatch) : base(game, spriteBatch)
        {
            _game = game;
            _device = game.GraphicsDevice;
            _level = level;
        }

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

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            var keyBoardState = Keyboard.GetState();

            if (keyBoardState.IsKeyDown(Keys.A))
            {
                applyPhysics(gameTime);
                SpeedMove = 500f;
            }
            else
            {
                SpeedMove = 0;
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

        public override void Draw(GameTime gameTime, SpriteBatch _spriteBatch)
        {
            Rectangle screenRectangle = new Rectangle(0, 0, _screenWidth, _screenHeight);

            if (_foregroundTexture != null)
            {
                _spriteBatch.Draw(_foregroundTexture, screenRectangle, Color.White);
            }

            base.Draw(gameTime);
        }

        public int getTerrainContour(int index)
        {
            return this.TerrainContour[index];
        }

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

        public void applyPhysics(GameTime gameTime)
        {
            _level.IsCollisionForward(gameTime);
        }

        public void ReinitializeRoad()
        {
            Road.ReinitializeRoad();
        }
    }
}
