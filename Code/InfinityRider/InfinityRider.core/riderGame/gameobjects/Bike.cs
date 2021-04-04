using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace InfinityRider.core.RiderGame.GameObjects
{
    /// <summary>
    /// A class to display a Bike within a game
    /// </summary>
    class Bike : GameObject
    {
        /// <summary>
        /// The texture of the bike
        /// </summary>
        private Texture2D _texture;
        /// <summary>
        /// The position of the bike
        /// </summary>
        public Vector2 Position { get; set; } = new Vector2(700, 200);
        /// <summary>
        /// The current speed of the bike
        /// </summary>
        private float SpeedMove { get; set; }
        /// <summary>
        /// The current rotation of the bike
        /// </summary>
        public float Rotation { get; set; }
        /// <summary>
        /// The current speed rotation of the bike
        /// </summary>
        private float SpeedRotation { get; set; }
        /// <summary>
        /// The current speed of gravity of the bike
        /// </summary>
        public int GravityAcceleration=200;
        /// <summary>
        /// The level that have created this instance of class
        /// </summary>
        private Level _level;
        /// <summary>
        /// The current Rectangle of hitbox of the bike
        /// </summary>
        public Rectangle BoundingRectangle =>
            new Rectangle((int)Position.X-(_texture.Width/2), (int)Position.Y-(_texture.Height/2), _texture.Width, _texture.Height);

        /// <summary>
        /// A constructor of the class Bike
        /// </summary>
        /// <param name="level">The level that have created this instance of class</param>
        /// <param name="game">The Game instance that contains all the game</param>
        /// <param name="spriteBatch">The spriteBatch instance of the game</param>
        public Bike(Level level, Game game, SpriteBatch spriteBatch) : base(game, spriteBatch)
        {
            SpeedMove = 500f;
            Rotation = 0f;
            SpeedRotation = 9f;
            _level = level;
            LoadContent();
        }

        /// <summary>
        /// A method that loads some contents for this class
        /// </summary>
        protected override void LoadContent()
        {
            _texture = Game.Content.Load<Texture2D>("bikes/Bike 1");
        }

        /// <summary>
        /// A method that updates some contents for this class
        /// </summary>
        /// <param name="gameTime">The instance of GameTime of the game</param>
        public override void Update(GameTime gameTime)
        {
            var keyBoardState = Keyboard.GetState();

            //if(keyBoardState.IsKeyDown(Keys.Space))
            //{
            //    Rotation -= SpeedRotation * (float)gameTime.ElapsedGameTime.TotalSeconds;
            //}

            if (keyBoardState.IsKeyDown(Keys.Up))
            {
                 Position = Vector2.Add(Position, new Vector2(0, SpeedMove * -1 * (float)gameTime.ElapsedGameTime.TotalSeconds));
            }
            if (keyBoardState.IsKeyDown(Keys.Down))
            {
                Vector2 futurePosition = Vector2.Add(Position, new Vector2(0, SpeedMove * (float)gameTime.ElapsedGameTime.TotalSeconds));
                if (!(_level.IsCollisionGravity(futurePosition)))
                    Position = futurePosition;
            }
            applyPhysics(gameTime);
        }

        /// <summary>
        /// A method that draws the bike
        /// </summary>
        /// <param name="gameTime">The instance of GameTime of the game</param>
        /// <param name="spriteBatch">The spriteBatch instance of the game</param>
        public override void Draw(GameTime gameTime, SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(_texture,                                  // Texture (Image)
                Position,                                                // Position of image
                null,                                                    // Area of image to display
                Color.White,                                             // Tint
                Rotation,                                                // Rotation (rad)
                new Vector2(_texture.Width / 2, _texture.Height / 2),    // Origin
                new Vector2(0.6f,0.6f),                                  // Scale
                SpriteEffects.None,                                      // Effect
                0f);                                                     // Depth
                base.Draw(gameTime);
        }

        /// <summary>
        /// A method that use the instance of Level within this class to know if there are a collision
        /// </summary>
        /// <param name="gameTime">The instance of GameTime of the game</param>
        public void applyPhysics(GameTime gameTime)
        {
            Vector2 futurePosition = Vector2.Add(Position, new Vector2(0, GravityAcceleration * (float)gameTime.ElapsedGameTime.TotalSeconds));
            if (!(_level.IsCollisionGravity(futurePosition)))
            {
                this.Position = futurePosition;
            }
        }

        /// <summary>
        /// A method to rotate the bike
        /// </summary>
        /// <param name="rotate">The degree of rotation we want, in rad</param>
        public void rotation(float rotate)
        {
            Rotation = rotate;
        }
    }
}
