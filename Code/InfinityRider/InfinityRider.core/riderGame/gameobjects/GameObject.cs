using InfinityRider.core.riderGame.utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace InfinityRider.core.riderGame.gameobjects
{
    public class GameObject : DrawableGameComponent
    {
        protected SpriteBatch _spriteBatch;
        protected Game _game;
        protected GraphicsDevice _device;

        public GameObject(Microsoft.Xna.Framework.Game game) : base(game)
        {
            _game = game;
            _device = game.GraphicsDevice;
            LoadContent();
            _spriteBatch = Utility.SpriteBatch;
        }

        public virtual void Draw(GameTime gameTime)
        {

        }
    }
}
