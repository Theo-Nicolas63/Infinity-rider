using InfinityRider.core.Modele;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfinityRider.core.Content.Modele
{
    public class Moto : GameObject
    {
        private Texture2D _texture;

        public Vector2 Position { get; set; } = Vector2.One;

        private Vector2 Velocity { get; set; }
        public Moto(Game game, SpriteBatch spriteBatch, Vector2 velocity) : base(game, spriteBatch)
        {
            LoadContent();
            Velocity = velocity;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            _texture = Game.Content.Load<Texture2D>("Sprites/ball");
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            _spriteBatch.Draw(_texture, Position, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Position = Vector2.Add(Position, new Vector2(0, 100f * (float)gameTime.ElapsedGameTime.TotalSeconds));
        }
    }
}
