using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using InfinityRider.core.RiderGame.GameObjects.Background;
using InfinityRider.core.RiderGame.GameObjects.Road;
using InfinityRider.core.RiderGame.GameObjects;
using InfinityRider.core.RiderGame.Menu;
using InfinityRider.core.RiderGame.Utils;

namespace InfinityRider.core.RiderGame
{
    public class Level : IDisposable
    {
        private RoadConstructor currentRoad;

        private Bike currentBike;

        public Background Background { get; private set; }

        private IList<GameObject> GameObjects { get; set; } = new List<GameObject>();

        public GraphicsDevice _device;

        private Texture2D testCollision;

        private Rectangle rect;

        private Game1 game;

        private MenuGame menu;

        public Level(Game game, SpriteBatch spriteBatch, GraphicsDevice device)
        {
            this.game = (Game1) game;

            Background = new Background(game, spriteBatch);
            GameObjects.Add(Background);
            currentRoad = new RoadConstructor(this, game, spriteBatch);
            GameObjects.Add(currentRoad);
            currentBike = new Bike(this, game, spriteBatch);
            GameObjects.Add(currentBike);
            //currentTerrain = currentRoad.getTerrainContour();
            _device = device;

            menu = new MenuGame(game, this);
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
            if(currentBike.GravityAcceleration < 500)
                currentBike.GravityAcceleration = currentBike.GravityAcceleration+20;
        }



        public void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            if(game.Status != GameStatus.PROCESSING)
            {
                menu.UpdateMenu();
            } else
            {
                foreach (var gameObject in GameObjects)
                {
                    gameObject.Update(gameTime);
                }
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

            if (game.Status != GameStatus.PROCESSING)
            {
                menu.DrawUI();
            }
            else
            {
                foreach (var gameObject in GameObjects)
                {
                    gameObject.Draw(gameTime, spriteBatch);
                }
            }

            //DrawRectangle(rect, Color.White, spriteBatch);
            spriteBatch.End();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void LaunchGame()
        {
            game.Status = GameStatus.PROCESSING;
            menu.UpdateStateMenu();
        }

        public void PauseGame()
        {
            if (game.Status == GameStatus.PROCESSING)
            {
                game.Status = GameStatus.PAUSED;
                menu.UpdateStateMenu();
            }
        }

        public void EndGame()
        {
            currentRoad.ReinitializeRoad();
            game.Status = GameStatus.FINISHED;
            menu.UpdateStateMenu();
        }

        public void ReLaunchGame()
        {
            currentRoad.ReinitializeRoad();
            currentBike.Position = new Vector2(currentBike.Position.X, 5);
            LaunchGame();
        }
    }
}
