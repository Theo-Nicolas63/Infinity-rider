using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace InfinityRider.core.riderGame
{
    class Background : GameObject
    {
        private Game _game;
        private int _width;
        private int _height;
        private Texture2D _imageBackground;

        public Background(Microsoft.Xna.Framework.Game game, SpriteBatch spriteBatch) : base(game, spriteBatch)
        {
            _game = game;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            _imageBackground = Game.Content.Load<Texture2D>("background/stars");

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
            _spriteBatch.Draw(_imageBackground, new Rectangle(0,0,800,480), Color.White);

            if(_width > 800)
            {
                _spriteBatch.Draw(_imageBackground, new Rectangle(800, 0, 800, 480), Color.White);
            }
            if (_height > 480)
            {
                _spriteBatch.Draw(_imageBackground, new Rectangle(0, 480, 800, 480), Color.White);
            }
            if (_height > 480 && _width > 800)
            {
                _spriteBatch.Draw(_imageBackground, new Rectangle(800, 480, 800, 480), Color.White);
            }

            //We can make the things up more extandable (by calculate modulo of width...
            //Or do that :
            //_spriteBatch.Draw(_imageBackground, new Rectangle(0, 0, _width, _height), Color.White);


            base.Draw(gameTime);
        }


    }
}
