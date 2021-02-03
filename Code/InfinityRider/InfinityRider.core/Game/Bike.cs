using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfinityRider.core.Game
{
    class Bike : GameObject
    {
        private Texture2D _texture;
        public Vector2 Position { get; private set; }

        public Bike(Microsoft.Xna.Framework.Game game, SpriteBatch spriteBatch) : base(game, spriteBatch)
        {

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
                Position = Vector2.Add(Position, new Vector2(-1, 0));
            }
            if (keyBoardState.IsKeyDown(Keys.Right))
            {
                Position = Vector2.Add(Position, new Vector2(1, 0));
            }
            if (keyBoardState.IsKeyDown(Keys.Up))
            {
                Position = Vector2.Add(Position, new Vector2(0, 1));
            }
            if (keyBoardState.IsKeyDown(Keys.Down))
            {
                Position = Vector2.Add(Position, new Vector2(0, -1));
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
