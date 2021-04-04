using Apos.Gui;
using FontStashSharp;
using InfinityRider.core.riderGame.gameobjects.background;
using InfinityRider.core.riderGame.utils;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Optional;
using System;
using System.Collections.Generic;

namespace InfinityRider.core.riderGame.menu
{
    public class Menu
    {
        private Panel panelBackground;

        ComponentFocus menuFocus;
        Switcher<MenuScreens> menuSwitch;
        LinkedList<MenuScreens> oldMenu = new LinkedList<MenuScreens>();
        private Game1 game;
        private Level level;

        public Menu(Game game, Level level)
        {
            this.game = (Game1) game;
            this.level = level;
            
            SetUpFont();

            menuFocus = new ComponentFocus(Default.ConditionPrevFocus, Default.ConditionNextFocus);

            MenuPanel menuPanel = new MenuPanel();
            menuPanel.Layout = new LayoutVerticalCenter();
            menuSwitch = new Switcher<MenuScreens>();

            menuSwitch.Add(MenuScreens.Main, setupMainMenu());
            menuSwitch.Add(MenuScreens.Pause, setupPauseMenu());
            menuSwitch.Add(MenuScreens.Finish, setupFinishMenu());
            menuSwitch.Add(MenuScreens.Settings, setupSettingsMenu());
            menuSwitch.Add(MenuScreens.Background, setupBackgroundsMenu());
            menuSwitch.Add(MenuScreens.Graphics, setupGraphicsMenu());
            menuSwitch.Add(MenuScreens.Quit, setupQuitConfirm());


            menuPanel.Add(menuSwitch);

            menuFocus.Root = menuPanel;

            selectMenu(MenuScreens.Main);
        }

        public void UpdateStateMenu()
        {
            switch(game.Status)
            {
                case GameStatus.NOTSTART:
                    selectMenu(MenuScreens.Main);
                    break;
                case GameStatus.PROCESSING:
                case GameStatus.PAUSED:
                    selectMenu(MenuScreens.Pause);
                    break;
                case GameStatus.FINISHED:
                    selectMenu(MenuScreens.Finish);
                    break;
            }
        }

        public void updateCurrentMenu()
        {
            selectMenuWithoutSaveOld(menuSwitch.Key.ValueOr(MenuScreens.Main));
        }

        private void selectMenu(MenuScreens key)
        {
            oldMenu.AddFirst(menuSwitch.Key.ValueOr(MenuScreens.Main));
            selectMenuWithoutSaveOld(key);
        }

        private void selectMenuWithoutSaveOld(MenuScreens key)
        {
            GuiHelper.NextLoopActions.Add(() => {
                menuSwitch.Key = Option.Some(key);
                menuFocus.Focus = menuSwitch;
            });
        }

        private void selectOldMenu()
        {
            var oldMenuLinked = oldMenu.First;
            MenuScreens old = MenuScreens.Main;
            if(oldMenuLinked != null)
            {
                old = oldMenuLinked.Value;
                oldMenu.RemoveFirst();
            }
            selectMenuWithoutSaveOld(old);
        }

        public void UpdateSetup()
        {
            menuFocus.UpdateSetup();
        }

        public void UpdateInput()
        {
            if (Default.ConditionBackFocus())
            {
                if (menuSwitch.Key == Option.Some(MenuScreens.Main))
                {
                    selectMenu(MenuScreens.Quit);
                }
                else
                {
                    UpdateStateMenu();
                }
            }

            menuFocus.UpdateInput();
        }

        public void UpdateMenu()
        {
            GuiHelper.UpdateSetup();

            UpdateSetup();
            UpdateInput();
            Update();

            GuiHelper.UpdateCleanup();
        }

        public void SetUpFont(int width, int height, string path)
        {
            FontSystem fontSystem = FontSystemFactory.Create(game.GraphicsDevice, width, height);
            fontSystem.AddFont(TitleContainer.OpenStream(path));
            //fontSystem.AddFont(TitleContainer.OpenStream($"{Content.RootDirectory}/Fonts/Allura-Regular.otf"));

            GuiHelper.Setup(game, fontSystem);
        }

        public void SetUpFont()
        {
            SetUpFont(2048, 2048, $"{game.Content.RootDirectory}/Fonts/SIXTY.TTF");
        }

        public void Update()
        {
            menuFocus.Update();
        }

        public void DrawUI()
        {
            menuFocus.Draw();
        }

        private Component createTitle(string text)
        {
            Label l = new Label(text);
            Border border = new Border(l, 20, 20, 20, 50);

            return border;
        }

        private Component createLabelDynamic(Func<string> text)
        {
            LabelDynamic ld = new LabelDynamic(text);
            ld.ActiveColor = Color.White;
            ld.NormalColor = new Color(150, 150, 150);
            Border border = new Border(ld, 20, 20, 20, 20);

            return border;
        }

        private void setupButtonsNewGameSettingsQuit(Panel p)
        {
            p.Add(Default.CreateButton("New Game", c => { level.ReLaunchGame(); }, menuFocus.GrabFocus));
            p.Add(Default.CreateButton("Settings", c => { selectMenu(MenuScreens.Settings); }, menuFocus.GrabFocus));
            p.Add(Default.CreateButton("Quit", c => { selectMenu(MenuScreens.Quit); }, menuFocus.GrabFocus));
        }

        private Component setupMainMenu()
        {
            Panel p = new Panel();
            p.Layout = new LayoutVerticalCenter();
            p.AddHoverCondition(Default.ConditionMouseHover);
            p.AddAction(Default.IsScrolled, Default.ScrollVertically);

            p.Add(createTitle("Infinity Rider"));

            setupButtonsNewGameSettingsQuit(p);

            return p;
        }

        private Component setupPauseMenu()
        {
            Panel p = new Panel();
            p.Layout = new LayoutVerticalCenter();
            p.AddHoverCondition(Default.ConditionMouseHover);
            p.AddAction(Default.IsScrolled, Default.ScrollVertically);

            p.Add(createTitle("Infinity Rider"));
            p.Add(createTitle("Game paused"));

            p.Add(Default.CreateButton("Resume Game", c => { level.LaunchGame(); }, menuFocus.GrabFocus));
            setupButtonsNewGameSettingsQuit(p);

            return p;
        }

        private Component setupFinishMenu()
        {
            Panel p = new Panel();
            p.Layout = new LayoutVerticalCenter();
            p.AddHoverCondition(Default.ConditionMouseHover);
            p.AddAction(Default.IsScrolled, Default.ScrollVertically);

            p.Add(createTitle("Infinity Rider"));
            p.Add(createTitle("Game finished"));

            setupButtonsNewGameSettingsQuit(p);

            return p;
        }

        private Component setupSettingsMenu()
        {
            Panel p = new Panel();
            p.Layout = new LayoutVerticalCenter();
            p.AddHoverCondition(Default.ConditionMouseHover);
            p.AddAction(Default.IsScrolled, Default.ScrollVertically);

            p.Add(createTitle("Settings"));
            p.Add(Default.CreateButton("Background", c => {
                setupBackgroundsMenu();
                selectMenu(MenuScreens.Background);
            }, menuFocus.GrabFocus));
            p.Add(Default.CreateButton("Graphics", c => {
                selectMenu(MenuScreens.Graphics);
            }, menuFocus.GrabFocus));
            p.Add(Default.CreateButton("Back", c => {
                selectOldMenu();
            }, menuFocus.GrabFocus));

            return p;
        }

        private Component setupBackgroundsMenu()
        {
            panelBackground = new Panel();
            panelBackground.Layout = new LayoutVerticalCenter();
            panelBackground.AddHoverCondition(Default.ConditionMouseHover);
            panelBackground.AddAction(Default.IsScrolled, Default.ScrollVertically);

            panelBackground.Add(createTitle("Background Settings"));

            addButtonBackgroundImage(panelBackground, BackgroundImage.BURNING_PLANET_RED);
            addButtonBackgroundImage(panelBackground, BackgroundImage.EARTH_DOUBLE_LUNE);
            addButtonBackgroundImage(panelBackground, BackgroundImage.EARTH_LUNE_BLUE);
            addButtonBackgroundImage(panelBackground, BackgroundImage.PLANET_BLUE);
            addButtonBackgroundImage(panelBackground, BackgroundImage.PLANET_RED);
            addButtonBackgroundImage(panelBackground, BackgroundImage.SOLAR_SYSTEM);

            panelBackground.Add(Default.CreateButton("Back", c => {
                selectOldMenu();
            }, menuFocus.GrabFocus));

            return panelBackground;
        }
        private void addButtonBackgroundImage(Panel p, string background)
        {
            if (!BackgroundImage.isExist(background)) return;
            Component Component_background = Default.CreateButton(BackgroundImage.getHumanName(background), c =>
            {
                c.IsFocused = true;
                GuiHelper.NextLoopActions.Add(() => { level.Background.changeBackground(background); });
            }, menuFocus.GrabFocus);
            Component_background.IsFocused = level.Background.BackgroundName == background ? true : false;
            p.Add(Component_background);
        }

        private Component setupGraphicsMenu()
        {
            Panel p = new Panel();
            p.Layout = new LayoutVerticalCenter();
            p.AddHoverCondition(Default.ConditionMouseHover);
            p.AddAction(Default.IsScrolled, Default.ScrollVertically);

            p.Add(createTitle("Graphics Settings"));
            p.Add(createLabelDynamic(() => {
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
                selectOldMenu();
            }, menuFocus.GrabFocus));

            return p;
        }

        private Component setupQuitConfirm()
        {
            Panel p = new Panel();
            p.Layout = new LayoutVerticalCenter();
            p.AddHoverCondition(Default.ConditionMouseHover);
            p.AddAction(Default.IsScrolled, Default.ScrollVertically);

            p.Add(createTitle("Do you really want to quit?"));
            p.Add(Default.CreateButton("Yes", c => {
                game.Exit();
            }, menuFocus.GrabFocus));
            p.Add(Default.CreateButton("No", c => {
                selectOldMenu();
            }, menuFocus.GrabFocus));

            return p;
        }
    }
}
