using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfinityRider.core.riderGame
{
    class RoadConstructor : GameObject
    {
        private Game _game;
        private GraphicsDevice _device;
        private int _screenWidth;
        private int _screenHeight;
        private int[] _terrainContour;
        private Texture2D _foregroundTexture;
        private Random _randomizer = new Random();
        private readonly double[] randoms = new double[3];
        private float SpeedMove { get; set; } = 0;
        private float MapPosition { get; set; } = 0;
        public Color MapColor { get; set; } = Color.Green;

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
            _terrainContour = new int[_screenWidth];

            float offset = _screenHeight * 5 / 8;
            float peakheight = 75;
            float flatness = 200;

            for(int i = 0; i < _screenWidth; i++)
            {
                double height = offset;
                for(int d = 0; d < randoms.Length; d++)
                {
                    height += peakheight / randoms[d] * Math.Sin((float) (i + MapPosition) / flatness * randoms[d] + randoms[d]);
                }

                _terrainContour[i] = (int)height;
            }
        }

        private void CreateForeGround()
        {
            Color[] foreGroundColors = new Color[_screenWidth * _screenHeight];

            for(int x = 0; x < _screenWidth; x++)
            {
                for(int y = 0; y < _screenHeight; y++)
                {
                    if(y > _terrainContour[x] && y < _terrainContour[x] + 10)
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
            } else
            {
                SpeedMove = 0;
            }

            MapPosition += SpeedMove * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(MapPosition != 0 && MapPosition % 2 == 0)
            {
                loadRandoms();
            }

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

            if(_foregroundTexture != null)
            {
                _spriteBatch.Draw(_foregroundTexture, screenRectangle, Color.White);
            }
        }
    }
}
