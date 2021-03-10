using Apos.Gui;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        public StatusMenu Status { get; set; } = StatusMenu.MAIN;
        private StatusMenu oldStatus = StatusMenu.MAIN;
        public bool wasEscapeKeyDownBefore = false;

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
                    updateMain();
                    break;
                case StatusMenu.PAUSE:
                    updatePause();
                    break;
                case StatusMenu.SETTINGS:
                    updateSettings(gametime);
                    break;
                case StatusMenu.QUIT:
                    updateQuit();
                    break;
            }

            Panel.Pop();

            // Call UpdateCleanup at the end.
            GuiHelper.UpdateCleanup();
            base.Update(gametime);
        }

        private void updateMain()
        {
            updateButtonStart();
            updateButtonSettings();
            updateButtonQuit();

            var keyBoardState = Keyboard.GetState();

            if (keyBoardState.IsKeyDown(Keys.Escape))
            {
                if(!wasEscapeKeyDownBefore)
                {
                    oldStatus = Status;
                    Status = StatusMenu.QUIT;
                    wasEscapeKeyDownBefore = true;
                }
            }
            else
            {
                wasEscapeKeyDownBefore = false;
            }
        }

        private void updatePause()
        {
            updateButtonResume();
            updateButtonSettings();
            updateButtonQuit();

            var keyBoardState = Keyboard.GetState();

            if (keyBoardState.IsKeyDown(Keys.Escape))
            {
                if (!wasEscapeKeyDownBefore)
                {
                    oldStatus = Status;
                    Status = StatusMenu.QUIT;
                    wasEscapeKeyDownBefore = true;
                }
            }
            else
            {
                wasEscapeKeyDownBefore = false;
            }
        }

        private void updateSettings(GameTime gametime)
        {
            updateLabelBackground(gametime);
            updateButtonBack();

            var keyBoardState = Keyboard.GetState();

            if (keyBoardState.IsKeyDown(Keys.Escape))
            {
                if (!wasEscapeKeyDownBefore)
                {
                    Status = oldStatus;
                    wasEscapeKeyDownBefore = true;
                }
            }
            else
            {
                wasEscapeKeyDownBefore = false;
            }
        }

        private void updateQuit()
        {
            updateButtonQuit();
            updateButtonBack();

            var keyBoardState = Keyboard.GetState();

            if (keyBoardState.IsKeyDown(Keys.Escape))
            {
                if (!wasEscapeKeyDownBefore)
                {
                    _game.Exit();
                }
            } 
            else
            {
                wasEscapeKeyDownBefore = false;
            }
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
                if (Status == StatusMenu.MAIN || Status == StatusMenu.PAUSE)
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

        private void updateButtonResume()
        {
            Button resumeButton = Button.Put("Resume");

            if (resumeButton.Clicked)
            {
                ((Game1)_game).LaunchGame();
            }
        }

        private void updateLabelBackground(GameTime gametime)
        {
            Label backgroundLabel = Label.Put("Background");

            Button burningPlanetRed = Button.Put("Burning Planet Red", 66);
            burningPlanetRed.XY = new Vector2(10, 10);
            burningPlanetRed.Update(gametime);

            Button earthDoubleLune = Button.Put("Earth Double Lune");

            Button earthLuneBlue = Button.Put("Earth Lune Blue");
        }


        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _ui.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}
