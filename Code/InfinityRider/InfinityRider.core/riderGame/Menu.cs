using Apos.Gui;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Optional;
using System;
using System.Collections.Generic;
using static InfinityRider.core.riderGame.Background;

namespace InfinityRider.core.riderGame
{
    class Menu
    {
        private Panel panelBackground;
        enum MenuScreens
        {
            Main,
            Pause,
            Finish,
            Settings,
            Background,
            Graphics,
            Quit
        }

        public Menu()
        {
            menuFocus = new ComponentFocus(Default.ConditionPrevFocus, Default.ConditionNextFocus);

            MenuPanel mp = new MenuPanel();
            mp.Layout = new LayoutVerticalCenter();
            menuSwitch = new Switcher<MenuScreens>();

            menuSwitch.Add(MenuScreens.Main, setupMainMenu());
            menuSwitch.Add(MenuScreens.Pause, setupPauseMenu());
            menuSwitch.Add(MenuScreens.Finish, setupFinishMenu());
            menuSwitch.Add(MenuScreens.Settings, setupSettingsMenu());
            menuSwitch.Add(MenuScreens.Background, setupBackgroundsMenu());
            menuSwitch.Add(MenuScreens.Graphics, setupGraphicsMenu());
            menuSwitch.Add(MenuScreens.Quit, setupQuitConfirm());
            

            mp.Add(menuSwitch);

            menuFocus.Root = mp;

            selectMenu(MenuScreens.Main);
        }
        ComponentFocus menuFocus;
        Switcher<MenuScreens> menuSwitch;
        LinkedList<MenuScreens> oldMenu = new LinkedList<MenuScreens>();

        private void setupButtonsNewGameSettingsQuit(Panel p)
        {
            p.Add(Default.CreateButton("New Game", c => { Utility.Game.ReLaunchGame(); }, menuFocus.GrabFocus));
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

            p.Add(Default.CreateButton("Resume Game", c => { Utility.Game.LaunchGame(); }, menuFocus.GrabFocus));
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

        private void addButtonBackgroundImage(Panel p, string background)
        {
            if (!BackgroundImage.isExist(background)) return;
            Component Component_background = Default.CreateButton(BackgroundImage.getHumanName(background), c =>
            {
                c.IsFocused = true;
                GuiHelper.NextLoopActions.Add(() => { Utility.Background.changeBackground(background); });
            }, menuFocus.GrabFocus);
            Component_background.IsFocused = Utility.Background.BackgroundName == background ? true : false;
            p.Add(Component_background);
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
                Utility.Game.Exit();
            }, menuFocus.GrabFocus));
            p.Add(Default.CreateButton("No", c => {
                selectOldMenu();
            }, menuFocus.GrabFocus));

            return p;
        }

        public void UpdateStateMenu()
        {
            switch(Utility.GameStatus)
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

        private class MenuPanel : ScreenPanel
        {
            public MenuPanel() { }

            public override void Draw()
            {
                SetScissor();
                _s.FillRectangle(BoundingRect, Color.Black * 0.6f);

                _s.DrawLine(Left, Top, Right, Top, Color.Black, 2);
                _s.DrawLine(Right, Top, Right, Bottom, Color.Black, 2);
                _s.DrawLine(Left, Bottom, Right, Bottom, Color.Black, 2);
                _s.DrawLine(Left, Top, Left, Bottom, Color.Black, 2);

                base.Draw();
                ResetScissor();
            }
        }
    }
}
