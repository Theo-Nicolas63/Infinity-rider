using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace InfinityRider.core.RiderGame.GameObjects
{
    /// <summary>
    /// A class to generalize the concept of a gameObject
    /// </summary>
    public abstract class GameObject : DrawableGameComponent
    {
        /// <summary>
        /// The SpriteBatch instance of the game
        /// </summary>
        protected readonly SpriteBatch _spriteBatch;
        /// <summary>
        /// The Game instance that contains all the game
        /// </summary>
        protected Game _game;
        /// <summary>
        /// The GraphicsDevice instance of the game
        /// </summary>
        protected GraphicsDevice _device;

        /// <summary>
        /// A constructor for this class
        /// </summary>
        /// <param name="game">The Game instance that contains all the game</param>
        /// <param name="spriteBatch">The spriteBatch instance of the game</param>
        public GameObject(Microsoft.Xna.Framework.Game game, SpriteBatch spriteBatch) : base(game)
        {
            _game = game;
            _device = game.GraphicsDevice;
            _spriteBatch = spriteBatch;
            LoadContent();
        }

        /// <summary>
        /// A method to generalize the concept to Draw within a GameObject
        /// </summary>
        /// <param name="gameTime">The instance of GameTime of the game</param>
        /// <param name="spriteBatch">The spriteBatch instance of the game</param>
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

        }
    }
}
