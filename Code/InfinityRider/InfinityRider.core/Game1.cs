using InfinityRider.core.riderGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace InfinityRider.core
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private IList<GameObject> GameObjects { get; set; } = new List<GameObject>();

        private Level level = new Level();
        private RoadConstructor road;
        private Bike bike;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Background background = new Background(this, _spriteBatch);
            GameObjects.Add(background);
            road = new RoadConstructor(this, _spriteBatch);
            GameObjects.Add(road);
            bike = new Bike(this, _spriteBatch);
            GameObjects.Add(bike);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            foreach (var gameObject in GameObjects)
            {
                gameObject.Update(gameTime);
            }

            base.Update(gameTime);
            level.Update(bike, road);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            foreach(var gameObject in GameObjects)
            {
                gameObject.Draw(gameTime);
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
