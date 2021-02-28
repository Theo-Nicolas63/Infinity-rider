using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfinityRider.core.riderGame
{
    class Level : IDisposable
    {
        private RoadConstructor currentRoad;

        private Bike currentBike; 

        public void Update(Bike bike, RoadConstructor road)
        {
            currentBike = bike;
            currentRoad = road;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
