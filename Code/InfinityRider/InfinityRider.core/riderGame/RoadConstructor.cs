using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace InfinityRider.core.riderGame
{
    class RoadConstructor : GameObject
    {
        private Game _game;
        private GraphicsDevice _device;
        private int _screenWidth;
        private int _screenHeight;
        private Texture2D _foregroundTexture;
        private Random _randomizer = new Random();
        private readonly double[] randoms = new double[3];
        private float SpeedMove { get; set; } = 0;
        public Color MapColor { get; set; } = Color.Green;
        private RoadManager Road { get; set; }
        private int[] _terrainContour;

        public RoadConstructor(Microsoft.Xna.Framework.Game game, SpriteBatch spriteBatch) : base(game, spriteBatch)
        {
            _game = game;
            _device = game.GraphicsDevice;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        private void GenerateTerrainContour()
        {
            if (Road == null)
            {
                Road = new RoadManager(_screenHeight * 5 / 8, 75, 200);
                Road.InitializeRoad();
            }
            _terrainContour = Road.GetCurrentRoad(_screenWidth);
        }

        private void CreateForeGround()
        {
            Color[] foreGroundColors = new Color[_screenWidth * _screenHeight];

            for (int x = 0; x < _screenWidth; x++)
            {
                for (int y = 0; y < _screenHeight; y++)
                {
                    if (y > _terrainContour[x] && y < _terrainContour[x] + 10)
                    {
                        foreGroundColors[x + y * _screenWidth] = MapColor;
                    }
                    else
                    {
                        foreGroundColors[x + y * _screenWidth] = Color.Transparent;
                    }

                    if (y >= _terrainContour[x] - 5 && y <= _terrainContour[x] || y >= _terrainContour[x] + 10 && y <= _terrainContour[x] + 15)
                    {
                        foreGroundColors[x + y * _screenWidth] = Color.FromNonPremultiplied(MapColor.R, MapColor.G, MapColor.B, 100);
                    }
                }
            }

            _foregroundTexture = new Texture2D(_device, _screenWidth, _screenHeight, false, SurfaceFormat.Color);
            _foregroundTexture.SetData(foreGroundColors);
        }

        private void loadRandoms()
        {
            randoms[0] = _randomizer.NextDouble() + 1;
            randoms[1] = _randomizer.NextDouble() + 2;
            randoms[2] = _randomizer.NextDouble() + 3;
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            loadRandoms();

            if (_game != null)
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

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            Rectangle screenRectangle = new Rectangle(0, 0, _screenWidth, _screenHeight);

            if (_foregroundTexture != null)
            {
                _spriteBatch.Draw(_foregroundTexture, screenRectangle, Color.White);
            }
        }
    }
}
