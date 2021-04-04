using System;

namespace InfinityRider.core.RiderGame.GameObjects.Road
{
    /// <summary>
    /// A class that allow us to generate a road
    /// </summary>
    class RoadManager
    {
        /// <summary>
        /// The minimum size of the road generated
        /// </summary>
        public const int ROAD_MINIMUM_SIZE = 10000;
        /// <summary>
        /// The operator to know if something is descending, for instance, to change the Offset and the PeakHeight
        /// </summary>
        public const int OPERATOR_DESCENDING = -1;
        /// <summary>
        /// The operator to know if something is rising, for instance, to change the Offset and the PeakHeight
        /// </summary>
        public const int OPERATOR_RISING = 1;
        /// <summary>
        /// The position where we load the map further
        /// </summary>
        private long positionCreatingMap = ROAD_MINIMUM_SIZE;
        /// <summary>
        /// An array that contains the position of the current road, with the index for abscissa and the value for ordinate, but from the top of the screen
        /// </summary>
        public int[] RoadOutline { get; private set; }
        /// <summary>
        /// Sets the position of the midheight of the wave of the road
        /// </summary>
        public float Offset { get; private set; }
        /// <summary>
        /// The Offset that are changing to have a road that aren't the same within the time
        /// </summary>
        private float changingOffset;
        /// <summary>
        /// The movement that are currently being done on the offset : Descending or rising
        /// </summary>
        private int movementOffset;
        /// <summary>
        /// Defines how high the wave will be
        /// </summary>
        public float PeakHeight { get; private set; }
        /// <summary>
        /// The PeakHeight that are changing to have a road that aren't the same within the time
        /// </summary>
        private float changingPeakHeight;
        /// <summary>
        /// The movement that are currently being done on the PeakHeight : Descending or rising
        /// </summary>
        private int movementPeakHeight;
        /// <summary>
        /// Increases or decreases the wavelength of our wave
        /// </summary>
        public float Flatness { get; private set; }
        /// <summary>
        /// The position where we are in the map when she is moving
        /// </summary>
        public float MapPosition { get; private set; }
        /// <summary>
        /// An array that contains some random to randomize the generation of the road
        /// </summary>
        public double[] Randoms { get; private set; }

        /// <summary>
        /// A constructor of the class RoadManager
        /// </summary>
        /// <param name="roadOutline">The array from which we create the road</param>
        /// <param name="offset">Sets the position of the midheight of the wave of the road</param>
        /// <param name="peakHeight">Defines how high the wave will be</param>
        /// <param name="flatness">Increases or decreases the wavelength of our wave</param>
        /// <param name="mapPosition">The position where we are in the map when she is moving</param>
        /// <param name="randoms">An array that contains some random to randomize the generation of the road</param>
        public RoadManager(int[] roadOutline, float offset, float peakHeight, float flatness, int mapPosition, double[] randoms)
        {
            if (roadOutline == null || roadOutline.Length < ROAD_MINIMUM_SIZE) RoadOutline = new int[ROAD_MINIMUM_SIZE];
            else RoadOutline = roadOutline;

            Offset = offset;
            changingOffset = offset;
            movementOffset = OPERATOR_DESCENDING;

            PeakHeight = peakHeight;
            changingPeakHeight = peakHeight;
            movementPeakHeight = OPERATOR_DESCENDING;

            Flatness = flatness;

            MapPosition = mapPosition < 0 ? 0 : mapPosition;

            if (randoms == null || randoms.Length == 0) Randoms = LoadSomeRandomsDouble();
            else Randoms = randoms;

            InitializeRoad();
        }

        /// <summary>
        /// A constructor of the class RoadManager
        /// </summary>
        /// <param name="roadOutline">The array from which we create the road</param>
        /// <param name="offset">Sets the position of the midheight of the wave of the road</param>
        /// <param name="peakHeight">Defines how high the wave will be</param>
        /// <param name="flatness">Increases or decreases the wavelength of our wave</param>
        /// <param name="mapPosition">The position where we are in the map when she is moving</param>
        public RoadManager(int[] roadOutline, float offset, float peakHeight, float flatness, int mapPosition) : this(roadOutline, offset, peakHeight, flatness, 0, null) { }

        /// <summary>
        /// A constructor of the class RoadManager
        /// </summary>
        /// <param name="offset">Sets the position of the midheight of the wave of the road</param>
        /// <param name="peakHeight">Defines how high the wave will be</param>
        /// <param name="flatness">Increases or decreases the wavelength of our wave</param>
        public RoadManager(float offset, float peakHeight, float flatness) : this(new int[ROAD_MINIMUM_SIZE], offset, peakHeight, flatness, 0) { }

        /// <summary>
        /// A method to generate a new road that aren't the same of the precedent one
        /// </summary>
        public void ReinitializeRoad()
        {
            RoadOutline = new int[ROAD_MINIMUM_SIZE];
            
            changingOffset = Offset;
            movementOffset = OPERATOR_DESCENDING;

            changingPeakHeight = PeakHeight;
            movementPeakHeight = OPERATOR_DESCENDING;

            MapPosition = 0;

            Randoms = LoadSomeRandomsDouble();

            InitializeRoad();
        }

        /// <summary>
        /// A method that change the current Offset to have a road that aren't the same within the time
        /// </summary>
        /// <returns>The new Offset</returns>
        private float GetNextChangingOffset()
        {
            if(movementOffset == OPERATOR_DESCENDING)
            {
                if (changingOffset > Offset / 1.5) return changingOffset -= 0.5f;
                else
                {
                    movementOffset = OPERATOR_RISING;
                    return changingOffset += 0.5f;
                }
            } 
            else
            {
                if (changingOffset < Offset * 1.25) return changingOffset += 0.5f;
                else
                {
                    movementOffset = OPERATOR_DESCENDING;
                    return changingOffset -= 0.5f;
                }
            }
        }

        /// <summary>
        /// A method that change the current PeakHeight to have a road that aren't the same within the time
        /// </summary>
        /// <returns>The new PeakHeight</returns>
        private float GetNextChangingPeakHeight()
        {
            if (movementPeakHeight == OPERATOR_DESCENDING)
            {
                if (changingPeakHeight > PeakHeight / 2) return changingPeakHeight -= 0.5f;
                else
                {
                    movementPeakHeight = OPERATOR_RISING;
                    return changingPeakHeight += 0.5f;
                }
            }
            else
            {
                if (changingPeakHeight < PeakHeight * 1.5) return changingPeakHeight += 0.5f;
                else
                {
                    movementPeakHeight = OPERATOR_DESCENDING;
                    return changingPeakHeight -= 0.5f;
                }
            }
        }

        /// <summary>
        /// A method to get the current road according to the position in the map
        /// </summary>
        /// <param name="width">The width of the road we want</param>
        /// <returns>The current road according to the position in the map</returns>
        public int[] GetCurrentRoad(int width)
        {
            var road = new int[width];

            if (width + MapPosition >= RoadOutline.Length) LoadMoreRoad();

            int position = (int)MapPosition;
            for (int i = 0; i < width; i++)
            {
                if ((position + i) >= RoadOutline.Length) position -= i;
                road[i] = RoadOutline[position + i];
            }

            return road;
        }

        /// <summary>
        /// A method to change the position in the map
        /// </summary>
        /// <param name="i">The number we want to add in the position of the map</param>
        public void NextPosition(float i)
        {
            MapPosition += i;
        }

        /// <summary>
        /// A method to change the position in the map. It move of one forward
        /// </summary>
        public void NextPosition()
        {
            NextPosition(1);
        }

        /// <summary>
        /// A method to generate an array that contains the position of a road
        /// </summary>
        /// <param name="width">The width of the road we want to create</param>
        /// <param name="offset">Sets the position of the midheight of the wave of the road</param>
        /// <param name="peakHeight">Defines how high the wave will be</param>
        /// <param name="flatness">Increases or decreases the wavelength of our wave</param>
        /// <param name="randoms">An array that contains some random to randomize the generation of the road</param>
        /// <param name="position">The position where we are in the map when she is moving</param>
        /// <returns>An array that contains the position of a road, with the index for abscissa and the value for ordinate, but from the top of the screen</returns>
        public int[] CreateRoad(int width, float offset, float peakHeight, float flatness, double[] randoms, long position = 0)
        {
            int[] groundOutline = new int[width];

            if (randoms == null) return null;

            double height;
            for (int i = 0; i < width; i++)
            {
                height = offset;
                for (int d = 0; d < randoms.Length; d++)
                {
                    height += peakHeight / randoms[d] * Math.Sin((i + position) / flatness * randoms[d] + randoms[d]);
                }

                groundOutline[i] = (int)height;
            }

            return groundOutline;
        }

        /// <summary>
        /// A method to generate an array that contains the position of a road that changes within the time
        /// </summary>
        /// <param name="width">The width of the road we want to create</param>
        /// <param name="position">The position where we are in the map when she is moving</param>
        /// <returns>an array that contains the position of a road, with the index for abscissa and the value for ordinate, but from the top of the screen</returns>
        public int[] CreateMovingRoad(int width, long position = 0)
        {
            int widthBetweenMoves = 5;
            int[] groundOutline = new int[width];
            
            for(int i = 0; i < width / widthBetweenMoves; i ++)
            {
                CreateRoad(widthBetweenMoves, GetNextChangingOffset(), GetNextChangingPeakHeight(), Flatness, Randoms, i * widthBetweenMoves + position).CopyTo(groundOutline, i* widthBetweenMoves);
            }
            CreateRoad(width % widthBetweenMoves, GetNextChangingOffset(), GetNextChangingPeakHeight(), Flatness, Randoms, width - width % widthBetweenMoves + position)
                .CopyTo(groundOutline, width - width % widthBetweenMoves);

            return groundOutline;
        }

        /// <summary>
        /// A method to initialize the road contains in that instance of class
        /// </summary>
        private void InitializeRoad()
        {
            CreateMovingRoad(RoadOutline.Length, 0).CopyTo(RoadOutline, 0);
        }

        /// <summary>
        /// A method to generate the road forward, for instance, when we reach the end of the one load
        /// </summary>
        private void LoadMoreRoad()
        {
            int[] temp = new int[RoadOutline.Length];
            Array.Copy(RoadOutline, (int)MapPosition, temp, 0, RoadOutline.Length - (int)MapPosition);


            CreateMovingRoad((int)MapPosition, positionCreatingMap).CopyTo(temp, RoadOutline.Length - (int)MapPosition);
            positionCreatingMap += (long)MapPosition;

            temp.CopyTo(RoadOutline, 0);
            
            MapPosition = 0;
        }

        /// <summary>
        /// A method to generate an array of three randoms numbers
        /// </summary>
        /// <returns>An array of three randoms numbers</returns>
        public double[] LoadSomeRandomsDouble()
        {
            Random r = new Random();

            double[] rand = new double[3];

            rand[0] = r.NextDouble() + 1;
            rand[1] = r.NextDouble() + 2;
            rand[2] = r.NextDouble() + 3;

            return rand;
        }
    }
}
