using Apos.Gui;
using FontStashSharp;
using InfinityRider.core.RiderGame.GameObjects.Background;
using InfinityRider.core.RiderGame.Utils;
using Microsoft.Xna.Framework;
using Optional;
using System;
using System.Collections.Generic;

namespace InfinityRider.core.RiderGame.Menu
{
    /// <summary>
    /// A class that create and display a Menu
    /// </summary>
    public class MenuGame
    {
        /// <summary>
        /// The instance of ComponentFocus of the Menu
        /// </summary>
        private readonly ComponentFocus menuFocus;
        /// <summary>
        /// The instance of Switcher of the Menu to navigate from a Menu to another
        /// </summary>
        private readonly Switcher<MenuScreens> menuSwitch;
        /// <summary>
        /// A list of the old Menu by which we've been through, allowing us to go back
        /// </summary>
        private readonly LinkedList<MenuScreens> oldMenu = new LinkedList<MenuScreens>();
        /// <summary>
        /// The Game instance that contains all the game
        /// </summary>
        private readonly Game1 game;
        /// <summary>
        /// The level that have created this instance of class
        /// </summary>
        private readonly Level level;

        /// <summary>
        /// A constructor of the class MenuGame
        /// </summary>
        /// <param name="game">The Game instance that contains all the game</param>
        /// <param name="level">The level that have created this instance of class</param>
        public MenuGame(Game game, Level level)
        {
            this.game = (Game1) game;
            this.level = level;
            
            SetUpFont();

            menuFocus = new ComponentFocus(Default.ConditionPrevFocus, Default.ConditionNextFocus);

            MenuPanel menuPanel = new MenuPanel();
            menuPanel.Layout = new LayoutVerticalCenter();
            menuSwitch = new Switcher<MenuScreens>();

            menuSwitch.Add(MenuScreens.Main, SetupMainMenu());
            menuSwitch.Add(MenuScreens.Pause, SetupPauseMenu());
            menuSwitch.Add(MenuScreens.Finish, SetupFinishMenu());
            menuSwitch.Add(MenuScreens.Settings, SetupSettingsMenu());
            menuSwitch.Add(MenuScreens.Background, SetupBackgroundsMenu());
            menuSwitch.Add(MenuScreens.Graphics, SetupGraphicsMenu());
            menuSwitch.Add(MenuScreens.Quit, SetupQuitConfirm());


            menuPanel.Add(menuSwitch);

            menuFocus.Root = menuPanel;

            SelectMenu(MenuScreens.Main);
        }

        /// <summary>
        /// A method that update the state of the menu from the game.Status
        /// </summary>
        public void UpdateStateMenu()
        {
            switch(game.Status)
            {
                case GameStatus.NOTSTART:
                    SelectMenu(MenuScreens.Main);
                    break;
                case GameStatus.PROCESSING:
                case GameStatus.PAUSED:
                    SelectMenu(MenuScreens.Pause);
                    break;
                case GameStatus.FINISHED:
                    SelectMenu(MenuScreens.Finish);
                    break;
            }
        }

        /// <summary>
        /// A method to select the new current menu
        /// </summary>
        /// <param name="key">The instance of MenuScreens we want for the Menu</param>
        private void SelectMenu(MenuScreens key)
        {
            oldMenu.AddFirst(menuSwitch.Key.ValueOr(MenuScreens.Main));
            SelectMenuWithoutSaveOld(key);
        }

        /// <summary>
        /// A method to select the new current menu without saving whtin the list of oldMenu
        /// </summary>
        /// <param name="key">The instance of MenuScreens we want for the Menu</param>
        private void SelectMenuWithoutSaveOld(MenuScreens key)
        {
            GuiHelper.NextLoopActions.Add(() => {
                menuSwitch.Key = Option.Some(key);
                menuFocus.Focus = menuSwitch;
            });
        }

        /// <summary>
        /// A method to select the precedent Menu
        /// </summary>
        private void SelectOldMenu()
        {
            var oldMenuLinked = oldMenu.First;
            MenuScreens old = MenuScreens.Main;
            if(oldMenuLinked != null)
            {
                old = oldMenuLinked.Value;
                oldMenu.RemoveFirst();
            }
            SelectMenuWithoutSaveOld(old);
        }

        /// <summary>
        /// The method to update the wall Menu
        /// </summary>
        public void UpdateMenu()
        {
            GuiHelper.UpdateSetup();

            menuFocus.UpdateSetup();
            
            if (Default.ConditionBackFocus())
            {
                if (menuSwitch.Key == Option.Some(MenuScreens.Main))
                {
                    SelectMenu(MenuScreens.Quit);
                }
                else
                {
                    UpdateStateMenu();
                }
            }

            menuFocus.UpdateInput();

            menuFocus.Update();

            GuiHelper.UpdateCleanup();
        }

        /// <summary>
        /// A method to setup a font for the menu
        /// </summary>
        /// <param name="width">The width we want for the FonstSystem</param>
        /// <param name="height">The height we want for the FonstSystem</param>
        /// <param name="path">The path where to find the font we want</param>
        public void SetUpFont(int width, int height, string path)
        {
            FontSystem fontSystem = FontSystemFactory.Create(game.GraphicsDevice, width, height);
            fontSystem.AddFont(TitleContainer.OpenStream(path));

            GuiHelper.Setup(game, fontSystem);
        }

        /// <summary>
        /// A method to setup a default font for the menu
        /// </summary>
        public void SetUpFont()
        {
            SetUpFont(2048, 2048, $"{game.Content.RootDirectory}/Fonts/SIXTY.TTF");
        }

        /// <summary>
        /// The method to draw the menu
        /// </summary>
        public void DrawUI()
        {
            menuFocus.Draw();
        }

        /// <summary>
        /// A method to create a title on the menu
        /// </summary>
        /// <param name="text">The text we want in</param>
        /// <returns>The Component of the title created</returns>
        private Component CreateTitle(string text)
        {
            Label l = new Label(text);
            Border border = new Border(l, 20, 20, 20, 50);

            return border;
        }

        /// <summary>
        /// A method to generate a Label dynamic, that can change in time
        /// </summary>
        /// <param name="text">The function that generate the dynamic text</param>
        /// <returns>A Label dynamic, that can change in time</returns>
        private Component CreateLabelDynamic(Func<string> text)
        {
            LabelDynamic ld = new LabelDynamic(text);
            ld.ActiveColor = Color.White;
            ld.NormalColor = new Color(150, 150, 150);
            Border border = new Border(ld, 20, 20, 20, 20);

            return border;
        }

        /// <summary>
        /// A method that create buttons for 'New Game', 'Settings' and 'Quit' in a panel
        /// </summary>
        /// <param name="p">The Panel where we want to create those</param>
        private void SetupButtonsNewGameSettingsQuit(Panel p)
        {
            p.Add(Default.CreateButton("New Game", c => { level.ReLaunchGame(); }, menuFocus.GrabFocus));
            p.Add(Default.CreateButton("Settings", c => { SelectMenu(MenuScreens.Settings); }, menuFocus.GrabFocus));
            p.Add(Default.CreateButton("Quit", c => { SelectMenu(MenuScreens.Quit); }, menuFocus.GrabFocus));
        }

        /// <summary>
        /// A method that create a Component for the main menu
        /// </summary>
        /// <returns>A Component for the main menu</returns>
        private Component SetupMainMenu()
        {
            Panel p = new Panel();
            p.Layout = new LayoutVerticalCenter();
            p.AddHoverCondition(Default.ConditionMouseHover);
            p.AddAction(Default.IsScrolled, Default.ScrollVertically);

            p.Add(CreateTitle("Infinity Rider"));

            SetupButtonsNewGameSettingsQuit(p);

            return p;
        }

        /// <summary>
        /// A method that create a Component for the pause menu
        /// </summary>
        /// <returns>A Component for the pause menu</returns>
        private Component SetupPauseMenu()
        {
            Panel p = new Panel();
            p.Layout = new LayoutVerticalCenter();
            p.AddHoverCondition(Default.ConditionMouseHover);
            p.AddAction(Default.IsScrolled, Default.ScrollVertically);

            p.Add(CreateTitle("Infinity Rider"));
            p.Add(CreateTitle("Game paused"));

            p.Add(Default.CreateButton("Resume Game", c => { level.LaunchGame(); }, menuFocus.GrabFocus));
            SetupButtonsNewGameSettingsQuit(p);

            return p;
        }

        /// <summary>
        /// A method that create a Component for the finish menu
        /// </summary>
        /// <returns>A Component for the finish menu</returns>
        private Component SetupFinishMenu()
        {
            Panel p = new Panel();
            p.Layout = new LayoutVerticalCenter();
            p.AddHoverCondition(Default.ConditionMouseHover);
            p.AddAction(Default.IsScrolled, Default.ScrollVertically);

            p.Add(CreateTitle("Infinity Rider"));
            p.Add(CreateTitle("Game finished"));

            SetupButtonsNewGameSettingsQuit(p);

            return p;
        }

        /// <summary>
        /// A method that create a Component for the settings menu
        /// </summary>
        /// <returns>A Component for the settings menu</returns>
        private Component SetupSettingsMenu()
        {
            Panel p = new Panel();
            p.Layout = new LayoutVerticalCenter();
            p.AddHoverCondition(Default.ConditionMouseHover);
            p.AddAction(Default.IsScrolled, Default.ScrollVertically);

            p.Add(CreateTitle("Settings"));
            p.Add(Default.CreateButton("Background", c => {
                SetupBackgroundsMenu();
                SelectMenu(MenuScreens.Background);
            }, menuFocus.GrabFocus));
            p.Add(Default.CreateButton("Graphics", c => {
                SelectMenu(MenuScreens.Graphics);
            }, menuFocus.GrabFocus));
            p.Add(Default.CreateButton("Back", c => {
                SelectOldMenu();
            }, menuFocus.GrabFocus));

            return p;
        }

        /// <summary>
        /// A method that create a Component for the settings of background menu
        /// </summary>
        /// <returns>A Component for the settings of background menu</returns>
        private Component SetupBackgroundsMenu()
        {
            Panel p = new Panel();
            p.Layout = new LayoutVerticalCenter();
            p.AddHoverCondition(Default.ConditionMouseHover);
            p.AddAction(Default.IsScrolled, Default.ScrollVertically);

            p.Add(CreateTitle("Background Settings"));

            p.Add(CreateLabelDynamic(() => {
                return "Current background: " + level.Background.BackgroundName;
            }));

            AddButtonBackgroundImage(p, BackgroundImage.BURNING_PLANET_RED);
            AddButtonBackgroundImage(p, BackgroundImage.EARTH_DOUBLE_LUNE);
            AddButtonBackgroundImage(p, BackgroundImage.EARTH_LUNE_BLUE);
            AddButtonBackgroundImage(p, BackgroundImage.PLANET_BLUE);
            AddButtonBackgroundImage(p, BackgroundImage.PLANET_RED);
            AddButtonBackgroundImage(p, BackgroundImage.SOLAR_SYSTEM);

            p.Add(Default.CreateButton("Back", c => {
                SelectOldMenu();
            }, menuFocus.GrabFocus));

            return p;
        }

        /// <summary>
        /// A method that create a button to change the backgroundImage of the game
        /// </summary>
        /// <param name="p">The Panel where we want to create those</param>
        /// <param name="background">The name of the background image to change</param>
        private void AddButtonBackgroundImage(Panel p, string background)
        {
            if (!BackgroundImage.isExist(background)) return;
            Component Component_background = Default.CreateButton(BackgroundImage.getHumanName(background), c =>
            {
                c.IsFocused = true;
                GuiHelper.NextLoopActions.Add(() => { level.Background.ChangeBackground(background); });
            }, menuFocus.GrabFocus);
            Component_background.IsFocused = level.Background.BackgroundName == background ? true : false;
            p.Add(Component_background);
        }

        /// <summary>
        /// A method that create a Component for the settings of graphics menu
        /// </summary>
        /// <returns>A Component for the settings of graphics menu</returns>
        private Component SetupGraphicsMenu()
        {
            Panel p = new Panel();
            p.Layout = new LayoutVerticalCenter();
            p.AddHoverCondition(Default.ConditionMouseHover);
            p.AddAction(Default.IsScrolled, Default.ScrollVertically);

            p.Add(CreateTitle("Graphics Settings"));
            p.Add(CreateLabelDynamic(() => {
                return "[Current font scale: " + GuiHelper.Scale + "x]";
            }));
            p.Add(Default.CreateButton("font Scale 1x", c => {
                GuiHelper.NextLoopActions.Add(() => { GuiHelper.Scale = 1f; });
            }, menuFocus.GrabFocus));
            p.Add(Default.CreateButton("font Scale 2x", c => {
                GuiHelper.NextLoopActions.Add(() => { GuiHelper.Scale = 2f; });
            }, menuFocus.GrabFocus));
            p.Add(Default.CreateButton("font Scale 3x", c => {
                GuiHelper.NextLoopActions.Add(() => { GuiHelper.Scale = 3f; });
            }, menuFocus.GrabFocus));
            p.Add(Default.CreateButton("font Scale 4x", c => {
                GuiHelper.NextLoopActions.Add(() => { GuiHelper.Scale = 4f; });
            }, menuFocus.GrabFocus));
            p.Add(Default.CreateButton("Back", c => {
                SelectOldMenu();
            }, menuFocus.GrabFocus));

            return p;
        }

        /// <summary>
        /// A method that create a Component for the menu for confirm to quit 
        /// </summary>
        /// <returns>A Component for the menu for confirm to quit</returns>
        private Component SetupQuitConfirm()
        {
            Panel p = new Panel();
            p.Layout = new LayoutVerticalCenter();
            p.AddHoverCondition(Default.ConditionMouseHover);
            p.AddAction(Default.IsScrolled, Default.ScrollVertically);

            p.Add(CreateTitle("Do you really want to quit?"));
            p.Add(Default.CreateButton("Yes", c => {
                game.Exit();
            }, menuFocus.GrabFocus));
            p.Add(Default.CreateButton("No", c => {
                SelectOldMenu();
            }, menuFocus.GrabFocus));

            return p;
        }
    }
}
