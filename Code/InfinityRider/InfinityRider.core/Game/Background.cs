using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfinityRider.core.game
{
    class Background : GameObject
    {
        private Texture2D ImageBackground { get; set; }

        public Background(Microsoft.Xna.Framework.Game game, SpriteBatch spriteBatch) : base(game, spriteBatch)
        {

        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            ImageBackground = Game.Content.Load<Texture2D>("background/stars");

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            float width;
            float heigth;
            _spriteBatch.Draw(ImageBackground, new Rectangle(0,0,800,480), Color.White);
            base.Draw(gameTime);
        }


    }
}
