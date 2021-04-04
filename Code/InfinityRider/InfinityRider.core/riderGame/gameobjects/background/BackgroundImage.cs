using System;
using System.Collections.Generic;
using System.Text;

namespace InfinityRider.core.riderGame.gameobjects.background
{
    public class BackgroundImage
    {
        public const string BURNING_PLANET_RED = "BurningPlanetRed";
        public const string EARTH_DOUBLE_LUNE = "EarthDoubleLune";
        public const string EARTH_LUNE_BLUE = "EarthLuneBlue";
        public const string PLANET_BLUE = "PlanetBlue";
        public const string PLANET_RED = "PlanetRed";
        public const string SOLAR_SYSTEM = "SolarSystem";

        public static bool isExist(string background)
        {
            switch (background)
            {
                case BURNING_PLANET_RED:
                case EARTH_DOUBLE_LUNE:
                case EARTH_LUNE_BLUE:
                case PLANET_BLUE:
                case PLANET_RED:
                case SOLAR_SYSTEM:
                    return true;
            }
            return false;
        }

        public static string getHumanName(string background)
        {
            switch (background)
            {
                case BURNING_PLANET_RED:
                    return "Red burning planet";
                case EARTH_DOUBLE_LUNE:
                    return "Earth and double Lune";
                case EARTH_LUNE_BLUE:
                    return "Blue Earth and Lune";
                case PLANET_BLUE:
                    return "Blue planet";
                case PLANET_RED:
                    return "Red planet";
                case SOLAR_SYSTEM:
                    return "Solar System";
            }
            return "Not known";
        }
    }
}
