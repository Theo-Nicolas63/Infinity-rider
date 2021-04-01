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

        private Texture2D testCollision;

        private Rectangle rect;

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

        public bool IsCollisionGravity(Vector2 futurPosition)
        {
            Rectangle terrain = new Rectangle((int)futurPosition.X, currentRoad.getTerrainContour((int)futurPosition.X), 10, 20);
            rect = terrain;
            if (currentBike.BoundingRectangle.Intersects(terrain))
            {
                //currentBike.rotation(0);
                return true;
            }
            else return false;
        }

        public void IsCollisionForward(GameTime gameTime)
        {
            Rectangle terrain = new Rectangle((int)currentBike.Position.X, currentRoad.getTerrainContour((int)currentBike.Position.X), 10, 20);
            rect = terrain;
            while (currentBike.BoundingRectangle.Intersects(terrain))
            {
                if (currentRoad.getTerrainContour((int)currentBike.Position.X + 10) < currentRoad.getTerrainContour((int)currentBike.Position.X))
                    currentBike.GravityAcceleration = currentBike.GravityAcceleration - 50;
                else 
                    currentBike.GravityAcceleration = -10;
                
                currentBike.Position = Vector2.Add(currentBike.Position, new Vector2(0, currentBike.GravityAcceleration * (float)gameTime.ElapsedGameTime.TotalSeconds));
            }            
            currentBike.GravityAcceleration = currentBike.GravityAcceleration+20;
        }



        public void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            foreach (var gameObject in GameObjects)
            {
                gameObject.Update(gameTime);
            }
        }

        public void DrawRectangle(Rectangle coords, Color color, SpriteBatch spriteBatch)
        {
            if (testCollision == null)
            {
                testCollision = new Texture2D(_device, 1, 1);
                testCollision.SetData(new[] { Color.White });
            }
            spriteBatch.Draw(testCollision, coords, color);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // TODO: Add your drawing code here
            spriteBatch.Begin();
            foreach (var gameObject in GameObjects)
            {
                gameObject.Draw(gameTime, spriteBatch);
            }
            //DrawRectangle(rect, Color.White, spriteBatch);
            spriteBatch.End();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
