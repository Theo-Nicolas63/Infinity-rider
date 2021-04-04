using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace InfinityRider.core.RiderGame.GameObjects.Background
{
    /// <summary>
    /// A class to setup a background to the game
    /// </summary>
    public class Background : GameObject
    {
        /// <summary>
        /// The directory that contains all backgrounds images
        /// </summary>
        private const string DIRECTORY = "backgrounds/";

        /// <summary>
        /// The width of the window, so of the background too
        /// </summary>
        private int _width;
        /// <summary>
        /// The height of the window, so of the background too
        /// </summary>
        private int _height;
        /// <summary>
        /// The name of the background that are being displaied
        /// </summary>
        public string BackgroundName { get; private set; }
        /// <summary>
        /// The Texture of the background that are being displaied
        /// </summary>
        private Texture2D _imageBackground;

        /// <summary>
        /// A constructor of the class Background
        /// </summary>
        /// <param name="game">The Game instance that contains all the game</param>
        /// <param name="spriteBatch">The spriteBatch instance of the game</param>
        /// <param name="backgroundName">The name of the background we want to display. BackgroundImage.BURNING_PLANET_RED by default</param>
        public Background(Microsoft.Xna.Framework.Game game, SpriteBatch spriteBatch, string backgroundName = BackgroundImage.BURNING_PLANET_RED) : base(game, spriteBatch)
        {
            BackgroundName = backgroundName;
        }

        /// <summary>
        /// A method that loads some contents for this class
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            string filePath = Path.Combine(DIRECTORY, BackgroundName == null ? BackgroundImage.BURNING_PLANET_RED : BackgroundName);
            _imageBackground = Game.Content.Load<Texture2D>(filePath);
        }

        /// <summary>
        /// A method that updates some contents for this class
        /// </summary>
        /// <param name="gameTime">The instance of GameTime of the game</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _width = ((Game1)_game).Width;
            //_width = _game.GraphicsDevice.Viewport.Width;
            //_width = _game.Window.ClientBounds.Width;
            _height = ((Game1)_game).Height;
            //_height = _game.GraphicsDevice.Viewport.Height;
            //_height = _game.Window.ClientBounds.Height;
        }

        /// <summary>
        /// A method that draws the background
        /// </summary>
        /// <param name="gameTime">The instance of GameTime of the game</param>
        /// <param name="spriteBatch">The spriteBatch instance of the game</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _spriteBatch.Draw(_imageBackground, new Rectangle(0,0, _width, _height), Color.White);

            base.Draw(gameTime);
        }

        /// <summary>
        /// A method to change the current background that are being displaied
        /// </summary>
        /// <param name="background">The name of the new background we want</param>
        public void ChangeBackground(string background)
        {
            if (background == null || !BackgroundImage.isExist(background)) return;

            BackgroundName = background;

            _imageBackground = Game.Content.Load<Texture2D>(Path.Combine(DIRECTORY, BackgroundName));
        }
    }
}
