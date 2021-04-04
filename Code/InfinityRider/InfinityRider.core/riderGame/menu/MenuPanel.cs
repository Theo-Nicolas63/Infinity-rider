using Apos.Gui;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace InfinityRider.core.RiderGame.Menu
{
    /// <summary>
    /// A class of MenuPanel that inherit of ScreenPanel, and that can be drawn
    /// </summary>
    class MenuPanel : ScreenPanel
    {
        /// <summary>
        /// A constructor of the class MenuPanel
        /// </summary>
        public MenuPanel() { }

        /// <summary>
        /// A method to draw the MenuPanel
        /// </summary>
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
