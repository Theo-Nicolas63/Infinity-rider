using Apos.Gui;
using FontStashSharp;
using InfinityRider.core.riderGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace InfinityRider.core
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private IList<GameObject> GameObjects { get; set; } = new List<GameObject>();
        private Menu _menu;
        public Background GameBackground { get; private set; }

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

            Utility.Background = new Background(this, Utility.SpriteBatch);
            GameObjects.Add(Utility.Background);
            Utility.RoadConstructor = new RoadConstructor(this, Utility.SpriteBatch);
            GameObjects.Add(Utility.RoadConstructor);
            Utility.Bike = new Bike(this, Utility.SpriteBatch);
            GameObjects.Add(Utility.Bike);

            base.Initialize();
        }

        private void WindowClientChanged(object sender, EventArgs e) { }

        protected override void LoadContent()
        {
            Utility.SpriteBatch = new SpriteBatch(GraphicsDevice);

            FontSystem fontSystem = FontSystemFactory.Create(GraphicsDevice, 2048, 2048);
            fontSystem.AddFont(TitleContainer.OpenStream($"{Content.RootDirectory}/Fonts/Allura-Regular.otf"));

            GuiHelper.Setup(this, fontSystem);
            _menu = new Menu();

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                pauseGame();

            // TODO: Add your update logic here

            
            switch (Utility.GameStatus)
            {
                case GameStatus.NOTSTART:
                case GameStatus.PAUSED:
                case GameStatus.FINISHED:
                    UpdateMenu();
                    break;
                case GameStatus.PROCESSING:
                    foreach (var gameObject in GameObjects)
                    {
                        gameObject.Update(gameTime);
                    }
                    break;
            }

            base.Update(gameTime);
        }

        private void UpdateMenu()
        {
            GuiHelper.UpdateSetup();

            _menu.UpdateSetup();
            _menu.UpdateInput();
            _menu.Update();

            GuiHelper.UpdateCleanup();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            Utility.SpriteBatch.Begin();

            switch (Utility.GameStatus)
            {
                case GameStatus.NOTSTART:
                case GameStatus.PAUSED:
                case GameStatus.FINISHED:
                    _menu.DrawUI();
                    break;
                case GameStatus.PROCESSING:
                    foreach (var gameObject in GameObjects)
                    {
                        gameObject.Draw(gameTime);
                    }
                    break;
            }
            
            Utility.SpriteBatch.End();

            base.Draw(gameTime);
        }

        public void LaunchGame()
        {
            _menu.UpdateStateMenu();
            Utility.GameStatus = GameStatus.PROCESSING;
        }

        private void pauseGame()
        {
            if (Utility.GameStatus == GameStatus.PROCESSING)
            {
                _menu.UpdateStateMenu();
                Utility.GameStatus = GameStatus.PAUSED;
                //_mainMenu.wasEscapeKeyDownBefore = true;
            }
        }

        public void EndGame()
        {
            _menu.UpdateStateMenu();
            Utility.GameStatus = GameStatus.FINISHED;
        }

        public void ReLaunchGame()
        {
            //TODO : Continue this method
            _menu.UpdateStateMenu();
            Utility.GameStatus = GameStatus.NOTSTART;
        }
    }
}
