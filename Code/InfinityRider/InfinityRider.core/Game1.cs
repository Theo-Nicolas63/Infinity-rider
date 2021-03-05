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
        private SpriteBatch _spriteBatch;
        private IList<GameObject> GameObjects { get; set; } = new List<GameObject>();
        private MainMenu _mainMenu;
        public GameStatus Status { get; private set; } = GameStatus.NOTSTART;

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
            RoadConstructor road = new RoadConstructor(this, _spriteBatch);
            GameObjects.Add(road);
            Bike bike = new Bike(this, _spriteBatch);
            GameObjects.Add(bike);
            _mainMenu = new MainMenu(this, _spriteBatch);
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

            switch (Status)
            {
                case GameStatus.NOTSTART:
                    _mainMenu.Update(gameTime);
                    break;
                case GameStatus.PROCESSING:
                    foreach (var gameObject in GameObjects)
                    {
                        gameObject.Update(gameTime);
                    }
                    break;
                case GameStatus.PAUSED:
                    //Menu Pause
                    break;
                case GameStatus.FINISHED:
                    //Menu fin
                    break;
            }



            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            switch (Status)
            {
                case GameStatus.NOTSTART:
                case GameStatus.PAUSED:
                case GameStatus.FINISHED:
                    _mainMenu.Draw(gameTime);
                    break;
                case GameStatus.PROCESSING:
                    _spriteBatch.Begin();
                    foreach (var gameObject in GameObjects)
                    {
                        gameObject.Draw(gameTime);
                    }
                    _spriteBatch.End();
                    break;
            }

            base.Draw(gameTime);
        }

        public void LaunchGame()
        {
            Status = GameStatus.PROCESSING;
        }

        public void PauseGame()
        {
            Status = GameStatus.PAUSED;
        }

        public void EndGame()
        {
            Status = GameStatus.FINISHED;
        }

        public void ReLaunchGame()
        {
            Status = GameStatus.NOTSTART;
        }
    }
}
