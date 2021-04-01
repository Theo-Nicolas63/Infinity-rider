using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace InfinityRider.core.riderGame
{

    public class Background : GameObject
    {
        public class BackgroundImage
        {
            public const string BURNING_PLANET_RED = "BurningPlanetRed";
            public const string EARTH_DOUBLE_LUNE = "EarthDoubleLune";
            public const string EARTH_LUNE_BLUE = "EarthLuneBlue";
            public const string PLANET_BLUE = "PlanetBlue";
            public const string PLANET_RED = "PlanetRed";
            public const string SOLAR_SYSTEM = "SolarSystem";

            public static bool isExist(string background)
            {
                switch (background)
                {
                    case BURNING_PLANET_RED:
                    case EARTH_DOUBLE_LUNE:
                    case EARTH_LUNE_BLUE:
                    case PLANET_BLUE:
                    case PLANET_RED:
                    case SOLAR_SYSTEM:
                        return true;
                }
                return false;
            }

            public static string getHumanName(string background)
            {
                switch(background)
                {
                    case BURNING_PLANET_RED:
                        return "Red burning planet";
                    case EARTH_DOUBLE_LUNE:
                        return "Earth and double Lune";
                    case EARTH_LUNE_BLUE:
                        return "Blue Earth and Lune";
                    case PLANET_BLUE:
                        return "Blue planet";
                    case PLANET_RED:
                        return "Red planet";
                    case SOLAR_SYSTEM:
                        return "Solar System";
                }
                return "Not known";
            }
        }

        private const string DIRECTORY = "backgrounds/";

        private int _width;
        private int _height;
        public string BackgroundName { get; private set; }
        private Texture2D _imageBackground;


        public Background(Microsoft.Xna.Framework.Game game, string backgroundName = BackgroundImage.BURNING_PLANET_RED) : base(game)
        {
            BackgroundName = backgroundName;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            string filePath = Path.Combine(DIRECTORY, BackgroundName == null ? BackgroundImage.BURNING_PLANET_RED : BackgroundName);
            _imageBackground = Game.Content.Load<Texture2D>(filePath);
        }

        public void changeBackground(string background)
        {
            if (background == null || !BackgroundImage.isExist(background)) return;

            BackgroundName = background;

            _imageBackground = Game.Content.Load<Texture2D>(Path.Combine(DIRECTORY, BackgroundName));
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
