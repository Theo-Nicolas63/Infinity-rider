using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace InfinityRider.core.riderGame
{
    class Bike : GameObject
    {
        private Texture2D _texture;
        private Vector2 Position { get; set; } = Vector2.One;
        private float SpeedMove { get; set; }
        private float Rotation { get; set; }
        private float SpeedRotation { get; set; }
        
        private Vector2 Velocity { get; set; }

        private Rectangle BoundingRectangle =>
            new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
        public Bike(Microsoft.Xna.Framework.Game game, SpriteBatch spriteBatch) : base(game, spriteBatch)
        {
            SpeedMove = 500f;
            Rotation = 0f;
            SpeedRotation = 9f;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            _texture = Game.Content.Load<Texture2D>("bikes/Bike 1");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            var keyBoardState = Keyboard.GetState();

            if(keyBoardState.IsKeyDown(Keys.Space))
            {
                Rotation -= SpeedRotation * (float)gameTime.ElapsedGameTime.TotalSeconds;
                //TODO : Add the forward movement for when the colision manage will be there, with the world/road
            }

            //This is to test the keyboard and the bike, but in the game it will not be in the code
            if(keyBoardState.IsKeyDown(Keys.Left))
            {
                Position = Vector2.Add(Position, new Vector2(SpeedMove * -1 * (float)gameTime.ElapsedGameTime.TotalSeconds, 0));
            }
            if (keyBoardState.IsKeyDown(Keys.Right))
            {
                Position = Vector2.Add(Position, new Vector2(SpeedMove * (float)gameTime.ElapsedGameTime.TotalSeconds, 0));
            }
            if (keyBoardState.IsKeyDown(Keys.Up))
            {
                Position = Vector2.Add(Position, new Vector2(0, SpeedMove * -1 * (float)gameTime.ElapsedGameTime.TotalSeconds));
            }
            if (keyBoardState.IsKeyDown(Keys.Down))
            {
                Position = Vector2.Add(Position, new Vector2(0, SpeedMove * (float)gameTime.ElapsedGameTime.TotalSeconds));
            }
            Position = Vector2.Add(Position, new Vector2(0, 200f * (float)gameTime.ElapsedGameTime.TotalSeconds));
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Draw(_texture,                                  // Texture (Image)
                Position,                                                // Position de l'image
                null,                                                    // Zone de l'image à afficher
                Color.White,                                             // Teinte
                Rotation,                                                // Rotation (en rad)
                new Vector2(_texture.Width / 2, _texture.Height / 2),    // Origine
                new Vector2(0.6f,0.6f),                                  // Echelle
                SpriteEffects.None,                                      // Effet
                0f);                                                     // Profondeur

            base.Draw(gameTime);
        }
    }
}
