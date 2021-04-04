namespace InfinityRider.core.RiderGame.GameObjects.Background
{
    /// <summary>
    /// A class for the image of the background that contain all the possibilities of images
    /// </summary>
    public class BackgroundImage
    {
        /// <summary>
        /// The name of an image for the background
        /// </summary>
        public const string BURNING_PLANET_RED = "BurningPlanetRed";
        /// <summary>
        /// The name of an image for the background
        /// </summary>
        public const string EARTH_DOUBLE_LUNE = "EarthDoubleLune";
        /// <summary>
        /// The name of an image for the background
        /// </summary>
        public const string EARTH_LUNE_BLUE = "EarthLuneBlue";
        /// <summary>
        /// The name of an image for the background
        /// </summary>
        public const string PLANET_BLUE = "PlanetBlue";
        /// <summary>
        /// The name of an image for the background
        /// </summary>
        public const string PLANET_RED = "PlanetRed";
        /// <summary>
        /// The name of an image for the background
        /// </summary>
        public const string SOLAR_SYSTEM = "SolarSystem";

        /// <summary>
        /// A method to know if the image is available
        /// </summary>
        /// <param name="background">The name of the background we want to test</param>
        /// <returns>True if the image is available, false otherwise</returns>
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

        /// <summary>
        /// A method to get a human readable string from the name of an image background
        /// </summary>
        /// <param name="background">The name of the image background</param>
        /// <returns>A human readable string</returns>
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
