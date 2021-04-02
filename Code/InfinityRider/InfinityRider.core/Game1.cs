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

        private Level level;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public GraphicsDevice _device { get; private set; }
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
            _device = this.GraphicsDevice;
            this.level = new Level(this, _spriteBatch, _device);
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
                pauseGame();

            // TODO: Add your update logic here
            level.Update(gameTime);

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
                    _mainMenu.Update(gameTime);
                    break;
                case GameStatus.FINISHED:
                    _mainMenu.Update(gameTime);
                    break;
            }

            base.Update(gameTime);
        }

        private void pauseGame()
        {
            if(Status == GameStatus.PROCESSING)
            {
                Status = GameStatus.PAUSED;
                _mainMenu.Status = StatusMenu.PAUSE;
                _mainMenu.wasEscapeKeyDownBefore = true;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // TODO: Add your drawing code here
            level.Draw(gameTime, _spriteBatch);

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
