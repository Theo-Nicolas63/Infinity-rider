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
        private IList<GameObject> GameObjects { get; set; } = new List<GameObject>();

        public GraphicsDevice _device;

        private int[] currentTerrain;

        public Level(Game game, SpriteBatch spriteBatch, GraphicsDevice device)
        {
            Background background = new Background(game, spriteBatch);
            GameObjects.Add(background);
            currentRoad = new RoadConstructor(this, game, spriteBatch);
            GameObjects.Add(currentRoad);
            currentBike = new Bike(this, game, spriteBatch);
            GameObjects.Add(currentBike);
            //currentTerrain = currentRoad.getTerrainContour();
            _device = device;
        }

        public bool IsCollision(Vector2 futurPosition)
        {
            Rectangle terrain = new Rectangle((int)futurPosition.X, _device.Viewport.Height - currentRoad.getTerrainContour((int)futurPosition.Y), 1, 1);
            if (currentBike.BoundingRectangle.Intersects(terrain))
                return true;
            else return false;
        }

      

        public void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            foreach (var gameObject in GameObjects)
            {
                gameObject.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // TODO: Add your drawing code here
            spriteBatch.Begin();
            foreach (var gameObject in GameObjects)
            {
                gameObject.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
