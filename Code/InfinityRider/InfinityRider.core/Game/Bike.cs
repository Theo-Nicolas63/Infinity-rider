using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfinityRider.core.game
{
    class Bike : GameObject
    {
        private Texture2D _texture;
        private Vector2 Position { get; set; }
        private float Speed { get; set; }
        public Bike(Microsoft.Xna.Framework.Game game, SpriteBatch spriteBatch) : base(game, spriteBatch)
        {
            Speed = 500f;
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

            if(keyBoardState.IsKeyDown(Keys.Left))
            {
                Position = Vector2.Add(Position, new Vector2(Speed * -1 * (float)gameTime.ElapsedGameTime.TotalSeconds, 0));
            }
            if (keyBoardState.IsKeyDown(Keys.Right))
            {
                Position = Vector2.Add(Position, new Vector2(Speed * (float)gameTime.ElapsedGameTime.TotalSeconds, 0));
            }
            if (keyBoardState.IsKeyDown(Keys.Up))
            {
                Position = Vector2.Add(Position, new Vector2(0, Speed * -1 * (float)gameTime.ElapsedGameTime.TotalSeconds));
            }
            if (keyBoardState.IsKeyDown(Keys.Down))
            {
                Position = Vector2.Add(Position, new Vector2(0, Speed * (float)gameTime.ElapsedGameTime.TotalSeconds));
            }
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Draw(_texture, Position, null, Color.White, 0f, new Vector2(_texture.Width / 2, _texture.Height / 2), Vector2.One, SpriteEffects.None, 0f);

            base.Draw(gameTime);
        }
    }
}
