using System;
using System.Collections.Generic;
using System.Text;

namespace InfinityRider.core.riderGame
{
    class RoadManager
    {
        public int[] RoadOutline { get; private set; }
        public float Offset { get; private set; }
        public float PeakHeight { get; private set; }
        public float Flatness { get; private set; }
        public float MapPosition { get; private set; }
        public double[] Randoms { get; private set; }

        public RoadManager(int[] roadOutline, float offset, float peakHeight, float flatness, int mapPosition, double[] randoms)
        {
            RoadOutline = roadOutline;
            Offset = offset;
            PeakHeight = peakHeight;
            Flatness = flatness;
            MapPosition = mapPosition < 0 ? 0 : mapPosition;
            Randoms = randoms;
        }

        public RoadManager(int[] roadOutline, float offset, float peakHeight, float flatness, int mapPosition) : this(roadOutline, offset, peakHeight, flatness, 0, new double[3])
        {
            Random r = new Random();

            Randoms[0] = r.NextDouble() + 1;
            Randoms[1] = r.NextDouble() + 2;
            Randoms[2] = r.NextDouble() + 3;
        }

        public RoadManager(float offset, float peakHeight, float flatness) : this(new int[10000], offset, peakHeight, flatness, 0) { }

        public int[] GetCurrentRoad(int width)
        {
            var road = new int[width];

            int position = (int)MapPosition;
            for (int i = 0; i < width; i++)
            {
                if ((position + i) >= RoadOutline.Length) position = -i;
                road[i] = RoadOutline[position + i];
            }

            return road;
        }

        public void NextPosition(float i)
        {
            MapPosition += i;
            if (MapPosition >= RoadOutline.Length) MapPosition -= RoadOutline.Length;
        }

        public void NextPosition()
        {
            NextPosition(1);
        }

        public int[] CreateRoad(int width, float offset, float peakHeight, float flatness, double[] randoms)
        {
            int[] groundOutline = new int[width];

            //offset : 60
            //peakheight : 75
            //flatness : 200

            double height;
            for (int i = 0; i < width; i++)
            {
                height = offset;
                for (int d = 0; d < randoms.Length; d++)
                {
                    height += peakHeight / randoms[d] * Math.Sin(i / flatness * randoms[d] + randoms[d]);
                }

                groundOutline[i] = (int)height;
            }

            return groundOutline;
        }

        public void InitializeRoad()
        {
            RoadOutline = CreateRoad(RoadOutline.Length, Offset, PeakHeight, Flatness, Randoms);
        }
    }
}
