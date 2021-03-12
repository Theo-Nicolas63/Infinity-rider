using System;
using System.Collections.Generic;
using System.Text;

namespace InfinityRider.core.riderGame
{
    public class Settings
    {
        public bool IsFullScreen
        {
            get;
            set;
        } = false;
        public bool IsBorderless
        {
            get;
            set;
        } = true;
        public int Width
        {
            get;
            set;
        } = 1700;
        public int Height
        {
            get;
            set;
        } = 900;
    }
}
