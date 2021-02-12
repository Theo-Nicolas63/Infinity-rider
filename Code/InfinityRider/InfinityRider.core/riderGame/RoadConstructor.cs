using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

            double random1 = _randomizer.NextDouble() + 1;
            double random2 = _randomizer.NextDouble() + 2;
            double random3 = _randomizer.NextDouble() + 3;

            float offset = _screenHeight / 2;
            float peakheight = 100;
            float flatness = 70;

            for(int i = 0; i < _screenWidth; i++)
            {
                double height = peakheight / random1 * Math.Sin((float) i / flatness * random1 + random1);
                height += peakheight / random2 * Math.Sin((float)i / flatness * random2 + random2);
                height += peakheight / random3 * Math.Sin((float)i / flatness * random3 + random3);
                height += offset;

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
                    if(y > _terrainContour[x])
                    {
                        foreGroundColors[x + y * _screenWidth] = Color.Green;
                    } else
                    {
                        foreGroundColors[x + y * _screenWidth] = Color.Transparent;
                    }

                }
            }

            _foregroundTexture = new Texture2D(_device, _screenWidth, _screenHeight, false, SurfaceFormat.Color);
            _foregroundTexture.SetData(foreGroundColors);
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            if(_game != null)
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
