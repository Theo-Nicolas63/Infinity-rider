using Apos.Gui;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Optional;
using System;

namespace InfinityRider.core.riderGame
{
    class Menu
    {
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
            menuSwitch.Add(MenuScreens.Pause, setupMainMenu());
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

        private Component setupMainMenu()
        {
            Panel p = new Panel();
            p.Layout = new LayoutVerticalCenter();
            p.AddHoverCondition(Default.ConditionMouseHover);
            p.AddAction(Default.IsScrolled, Default.ScrollVertically);

            p.Add(createTitle("Infinity Rider"));

            if(menuSwitch.Key.ValueOr(MenuScreens.Main) != MenuScreens.Finish)
            {
                p.Add(Default.CreateButton(menuSwitch.Key.ValueOr(MenuScreens.Main) == MenuScreens.Main ? "Start Game" : "Resume Game",
                    c => { Utility.Game.LaunchGame(); }, menuFocus.GrabFocus));
            }
            p.Add(Default.CreateButton("New Game", c => { Utility.Game.ReLaunchGame(); }, menuFocus.GrabFocus));
            p.Add(Default.CreateButton("Settings", c => { selectMenu(MenuScreens.Settings); }, menuFocus.GrabFocus));
            p.Add(Default.CreateButton("Quit", c => { selectMenu(MenuScreens.Quit); }, menuFocus.GrabFocus));

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
                selectMenu(MenuScreens.Background);
            }, menuFocus.GrabFocus));
            p.Add(Default.CreateButton("Graphics", c => {
                selectMenu(MenuScreens.Graphics);
            }, menuFocus.GrabFocus));
            p.Add(Default.CreateButton("Back", c => {
                selectMenu(MenuScreens.Main);
            }, menuFocus.GrabFocus));

            return p;
        }

        private Component setupBackgroundsMenu()
        {
            Panel p = new Panel();
            p.Layout = new LayoutVerticalCenter();
            p.AddHoverCondition(Default.ConditionMouseHover);
            p.AddAction(Default.IsScrolled, Default.ScrollVertically);

            p.Add(createTitle("Background Settings"));

            Component Component_BURNING_PLANET_RED = Default.CreateButton("Red burning planet", c =>
            {
                GuiHelper.NextLoopActions.Add(() => { Utility.Background.changeBackground(Background.BURNING_PLANET_RED); });
            }, menuFocus.GrabFocus);
            Component_BURNING_PLANET_RED.IsFocused = Utility.Background.BackgroundName == Background.BURNING_PLANET_RED ? true : false;
            p.Add(Component_BURNING_PLANET_RED);

            Component Component_EARTH_DOUBLE_LUNE = Default.CreateButton("Earth and double Lune", c =>
            {
                GuiHelper.NextLoopActions.Add(() => { Utility.Background.changeBackground(Background.EARTH_DOUBLE_LUNE); });
            }, menuFocus.GrabFocus);
            Component_EARTH_DOUBLE_LUNE.IsFocused = Utility.Background.BackgroundName == Background.EARTH_DOUBLE_LUNE ? true : false;
            p.Add(Component_EARTH_DOUBLE_LUNE);

            Component Component_EARTH_LUNE_BLUE = Default.CreateButton("Blue Earth and Lune", c => {
                GuiHelper.NextLoopActions.Add(() => { Utility.Background.changeBackground(Background.EARTH_LUNE_BLUE); });
            }, menuFocus.GrabFocus);
            Component_EARTH_LUNE_BLUE.IsFocused = Utility.Background.BackgroundName == Background.EARTH_LUNE_BLUE ? true : false;
            p.Add(Component_EARTH_LUNE_BLUE);

            Component Component_PLANET_BLUE = Default.CreateButton("Blue planet", c => {
                GuiHelper.NextLoopActions.Add(() => { Utility.Background.changeBackground(Background.PLANET_BLUE); });
            }, menuFocus.GrabFocus);
            Component_PLANET_BLUE.IsFocused = Utility.Background.BackgroundName == Background.PLANET_BLUE ? true : false;
            p.Add(Component_PLANET_BLUE);

            Component Component_PLANET_RED = Default.CreateButton("Red planet", c => {
                GuiHelper.NextLoopActions.Add(() => { Utility.Background.changeBackground(Background.PLANET_RED); });
            }, menuFocus.GrabFocus);
            Component_PLANET_RED.IsFocused = Utility.Background.BackgroundName == Background.PLANET_RED ? true : false;
            p.Add(Component_PLANET_RED);

            Component Component_SOLAR_SYSTEM = Default.CreateButton("Solar System", c => {
                GuiHelper.NextLoopActions.Add(() => { Utility.Background.changeBackground(Background.SOLAR_SYSTEM); });
            }, menuFocus.GrabFocus);
            Component_SOLAR_SYSTEM.IsFocused = Utility.Background.BackgroundName == Background.SOLAR_SYSTEM ? true : false;
            p.Add(Component_SOLAR_SYSTEM);

            p.Add(Default.CreateButton("Back", c => {
                selectMenu(MenuScreens.Settings);
            }, menuFocus.GrabFocus));

            return p;
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
            p.Add(Default.CreateButton(() => {
                return $"FullScreen: {(Utility.Settings.IsFullScreen ? " true" : "false")}";
            }, c => {
                GuiHelper.NextLoopActions.Add(() => { Utility.ToggleFullscreen(); });
            }, menuFocus.GrabFocus));
            p.Add(Default.CreateButton(() => {
                return $"Borderless: {(Utility.Settings.IsBorderless ? " true" : "false")}";
            }, c => {
                GuiHelper.NextLoopActions.Add(() => {
                    Utility.Settings.IsBorderless = !Utility.Settings.IsBorderless;
                    //Toggle twice to handle the borderless change.
                    Utility.ToggleFullscreen();
                    if (!Utility.Settings.IsFullScreen)
                    {
                        Utility.ToggleFullscreen();
                    }
                });
            }, menuFocus.GrabFocus));
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
                selectMenu(MenuScreens.Settings);
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
                selectMenu(MenuScreens.Main);
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

        private void selectMenu(MenuScreens key)
        {
            GuiHelper.NextLoopActions.Add(() => {
                menuSwitch.Key = Option.Some(key);
                menuFocus.Focus = menuSwitch;
            });
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
                    selectMenu(MenuScreens.Main);
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
