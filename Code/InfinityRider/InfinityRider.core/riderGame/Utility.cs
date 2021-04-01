using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace InfinityRider.core.riderGame
{
    static class Utility
    {
        public static SpriteBatch SpriteBatch { get; set; }
        public static Settings Settings { get; set; }
        public static Background Background { get; set; }
        public static RoadConstructor RoadConstructor { get; set; }
        public static Bike Bike { get; set; }
        public static Game1 Game { get; set; }
        public static Level Level { get; set; }
        public static Menu Menu { get; set; }
        public static GameStatus GameStatus { get; set; } = GameStatus.NOTSTART;

        public static GameWindow Window => Game.Window;
        public static GraphicsDeviceManager Graphics { get; set; }

        public static int WindowWidth => Window.ClientBounds.Width;
        public static int WindowHeight => Window.ClientBounds.Height;

        public static bool ShowLine { get; set; } = false;
    }
}
