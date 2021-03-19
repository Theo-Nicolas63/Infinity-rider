using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace InfinityRider.core.riderGame
{
    static class Utility
    {
        public static SpriteBatch SpriteBatch;
        public static Settings Settings;
        public static Background Background;
        public static RoadConstructor RoadConstructor;
        public static Bike Bike;
        public static Game1 Game;
        public static GameStatus GameStatus { get; set; } = GameStatus.NOTSTART;

        public static GameWindow Window => Game.Window;
        public static GraphicsDeviceManager Graphics { get; set; }

        public static int WindowWidth => Window.ClientBounds.Width;
        public static int WindowHeight => Window.ClientBounds.Height;

        public static bool ShowLine = false;
    }
}
