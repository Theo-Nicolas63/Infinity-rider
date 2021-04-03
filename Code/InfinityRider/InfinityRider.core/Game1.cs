using Apos.Gui;
using FontStashSharp;
using InfinityRider.core.riderGame;
using InfinityRider.core.riderGame.gameobjects;
using InfinityRider.core.riderGame.gameobjects.background;
using InfinityRider.core.riderGame.utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace InfinityRider.core
{
    public class Game1 : Game
    {

        private Level level;
        private GraphicsDeviceManager _graphics;
        private IList<GameObject> GameObjects { get; set; } = new List<GameObject>();
        public Background GameBackground { get; private set; }
        public GraphicsDevice _device { get; private set; }

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            Utility.Settings = new Settings();
            Utility.Game = this;
            Utility.Graphics = _graphics;

            _graphics.PreferredBackBufferWidth = Utility.Settings.Width;
            _graphics.PreferredBackBufferHeight = Utility.Settings.Height;
            _graphics.ApplyChanges();

            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += WindowClientChanged;

            Utility.SpriteBatch = new SpriteBatch(GraphicsDevice);

            Utility.Level = new Level(this, _device);

            level = Utility.Level;

            _device = this.GraphicsDevice;
            base.Initialize();
        }

        private void WindowClientChanged(object sender, EventArgs e) { }

        protected override void LoadContent()
        {
            // TODO: use this.Content to load your game content here

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                level.PauseGame();

            // TODO: Add your update logic here

            level.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            level.Draw(gameTime);
            base.Draw(gameTime);
        }


    }
}
