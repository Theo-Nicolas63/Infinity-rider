using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace InfinityRider.core.riderGame
{
    class Background : GameObject
    {
        public const string BURNING_PLANET_RED = "BurningPlanetRed";
        public const string EARTH_DOUBLE_LUNE = "EarthDoubleLune";
        public const string EARTH_LUNE_BLUE = "EarthLuneBlue";
        public const string PLANET_BLUE = "PlanetBlue";
        public const string PLANET_RED = "PlanetRed";
        public const string SOLAR_SYSTEM = "SolarSystem";

        private Game _game;
        private int _width;
        private int _height;
        private string _backgroundName;
        private Texture2D _imageBackground;


        public Background(Microsoft.Xna.Framework.Game game, SpriteBatch spriteBatch, string backgroundName = BURNING_PLANET_RED) : base(game, spriteBatch)
        {
            _game = game;
            _backgroundName = backgroundName;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            string directoy = "backgrounds/";
            string filePath = Path.Combine(directoy, _backgroundName == null ? BURNING_PLANET_RED : _backgroundName);
            _imageBackground = Game.Content.Load<Texture2D>(filePath);
            
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _width = _game.GraphicsDevice.Viewport.Width;
            //_width = _game.Window.ClientBounds.Width;
            _height = _game.GraphicsDevice.Viewport.Height;
            //_height = _game.Window.ClientBounds.Height;
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Draw(_imageBackground, new Rectangle(0,0, _width, _height), Color.White);

            base.Draw(gameTime);
        }


    }
}
