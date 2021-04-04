using System;
using System.Collections.Generic;
using System.Text;

namespace InfinityRider.core.riderGame.gameobjects.road
{
    class RoadManager
    {
        public const int ROAD_MINIMUM_SIZE = 10000;
        public const int OPERATOR_DESCENDING = -1;
        public const int OPERATOR_RISING = 1;
        public const int MINIMUM_SIZE_LOAD_BEHIND = 500;
        private long positionCreatingMap = ROAD_MINIMUM_SIZE;
        public int[] RoadOutline { get; private set; }
        public float Offset { get; private set; }
        private float changingOffset;
        private int movementOffset;
        public float PeakHeight { get; private set; }
        private float changingPeakHeight;
        private int movementPeakHeight;
        public float Flatness { get; private set; }
        public float MapPosition { get; private set; }
        public double[] Randoms { get; private set; }

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

        public RoadManager(int[] roadOutline, float offset, float peakHeight, float flatness, int mapPosition) : this(roadOutline, offset, peakHeight, flatness, 0, null) { }

        public RoadManager(float offset, float peakHeight, float flatness) : this(new int[ROAD_MINIMUM_SIZE], offset, peakHeight, flatness, 0) { }

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

        public void NextPosition(float i)
        {
            MapPosition += i;
        }

        public void NextPosition()
        {
            NextPosition(1);
        }

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

        private void InitializeRoad()
        {
            CreateMovingRoad(RoadOutline.Length, 0).CopyTo(RoadOutline, 0);
        }

        private void LoadMoreRoad()
        {
            int[] temp = new int[RoadOutline.Length];
            Array.Copy(RoadOutline, (int)MapPosition, temp, 0, RoadOutline.Length - (int)MapPosition);


            CreateMovingRoad((int)MapPosition, positionCreatingMap).CopyTo(temp, RoadOutline.Length - (int)MapPosition);
            positionCreatingMap += (long)MapPosition;

            temp.CopyTo(RoadOutline, 0);
            
            MapPosition = 0;
        }

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
