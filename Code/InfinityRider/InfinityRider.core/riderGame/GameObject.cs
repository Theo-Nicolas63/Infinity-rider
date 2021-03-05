using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace InfinityRider.core.riderGame
{
    public class GameObject : DrawableGameComponent
    {
        protected readonly SpriteBatch _spriteBatch;
        protected Game _game;
        protected GraphicsDevice _device;

        public GameObject(Microsoft.Xna.Framework.Game game, SpriteBatch spriteBatch) : base(game)
        {
            _game = game;
            _device = game.GraphicsDevice;
            _spriteBatch = spriteBatch;
            LoadContent();
        }
    }
}
