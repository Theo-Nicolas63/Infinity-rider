using Apos.Gui;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfinityRider.core.riderGame
{
    public enum StatusMenu
    {
        MAIN,
        PAUSE,
        SETTINGS,
        QUIT
    }

    class MainMenu : GameObject
    {
        private IMGUI _ui;
        private int _width;
        private int _height;
        public StatusMenu Status { get; private set; } = StatusMenu.MAIN;
        private StatusMenu oldStatus = StatusMenu.MAIN;

        public MainMenu(Microsoft.Xna.Framework.Game game, SpriteBatch spriteBatch) : base(game, spriteBatch)
        {
        }

        protected override void LoadContent()
        {
            var fontSystem = FontSystemFactory.Create(GraphicsDevice, 2048, 2048);
            fontSystem.AddFont(TitleContainer.OpenStream($"{Game.Content.RootDirectory}/Fonts/Allura-Regular.otf"));

            GuiHelper.Setup(_game, fontSystem);

            _ui = new IMGUI();
            GuiHelper.CurrentIMGUI = _ui;
        }

        public override void Update(GameTime gametime)
        {
            // Call UpdateSetup at the start.
            GuiHelper.UpdateSetup();
            _ui.UpdateAll(gametime);

            _width = _game.GraphicsDevice.Viewport.Width;
            _height = _game.GraphicsDevice.Viewport.Height;

            // Create your UI.
            Panel.Put().XY = new Vector2(_width / 2, _height / 2);

            switch(Status)
            {
                case StatusMenu.MAIN:
                    updateButtonStart();
                    updateButtonSettings();
                    updateButtonQuit();
                    break;
                case StatusMenu.PAUSE:
                    resumeButtonBack();
                    updateButtonSettings();
                    updateButtonQuit();
                    break;
                case StatusMenu.SETTINGS:
                    updateButtonBack();
                    break;
                case StatusMenu.QUIT:
                    updateButtonQuit();
                    updateButtonBack();
                    break;
            }



            Panel.Pop();

            // Call UpdateCleanup at the end.
            GuiHelper.UpdateCleanup();
        }

        private void updateButtonStart()
        {
            Button startButton = Button.Put("Start");

            if(startButton.Clicked)
            {
                ((Game1)_game).LaunchGame();
            }
        }

        private void updateButtonQuit()
        {
            Button quitButton = Button.Put("Quit");

            if (quitButton.Clicked)
            {
                if (Status == StatusMenu.MAIN)
                {
                    oldStatus = Status;
                    Status = StatusMenu.QUIT;
                }
                else if(Status == StatusMenu.PAUSE)
                {
                    oldStatus = Status;
                    Status = StatusMenu.QUIT;
                }
                else if(Status == StatusMenu.QUIT)
                {
                    _game.Exit();
                }
            }
        }
        private void updateButtonSettings()
        {
            Button settingsButton = Button.Put("Settings");

            if (settingsButton.Clicked)
            {
                oldStatus = Status;
                Status = StatusMenu.SETTINGS;
            }
        }

        private void updateButtonBack()
        {
            Button backButton = Button.Put("Back");

            if (backButton.Clicked)
            {
                Status = oldStatus;
            }
        }

        private void resumeButtonBack()
        {
            Button resumeButton = Button.Put("Resume");

            if (resumeButton.Clicked)
            {
                ((Game1)_game).LaunchGame();
            }
        }


        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _ui.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}
