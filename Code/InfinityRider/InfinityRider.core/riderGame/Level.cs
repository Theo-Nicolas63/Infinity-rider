using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfinityRider.core.riderGame
{
    class Level : IDisposable
    {
        private RoadConstructor currentRoad => Utility.RoadConstructor;

        private Bike currentBike => Utility.Bike;
        private IList<GameObject> GameObjects { get; set; } = new List<GameObject>();

        public GraphicsDevice _device;

        private int[] currentTerrain;

        private Texture2D testCollision;

        private Rectangle rect;

        private Menu _menu => Utility.Menu;


        public Level(Game game, GraphicsDevice device)
        {
            Utility.Level = this;
            Utility.Background = new Background(game);
            GameObjects.Add(Utility.Background);
            Utility.RoadConstructor = new RoadConstructor(game);
            GameObjects.Add(currentRoad);
            Utility.Bike = new Bike(game);
            GameObjects.Add(currentBike);

            Utility.Menu = new Menu();

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

            switch (Utility.GameStatus)
            {
                case GameStatus.PROCESSING:
                    foreach (var gameObject in GameObjects)
                    {
                        gameObject.Update(gameTime);
                    }
                    break;
                default:
                    _menu.UpdateMenu();
                    break;
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

        public void Draw(GameTime gameTime)
        {
            // TODO: Add your drawing code here
            Utility.SpriteBatch.Begin();
            switch (Utility.GameStatus)
            {
                case GameStatus.PROCESSING:
                    foreach (var gameObject in GameObjects)
                    {
                        gameObject.Draw(gameTime);
                    }
                    break;
                default:
                    _menu.DrawUI();
                    break;
            }
            //DrawRectangle(rect, Color.White, spriteBatch);
            Utility.SpriteBatch.End();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void LaunchGame()
        {
            Utility.GameStatus = GameStatus.PROCESSING;
            _menu.UpdateStateMenu();
        }

        public void PauseGame()
        {
            if (Utility.GameStatus == GameStatus.PROCESSING)
            {
                Utility.GameStatus = GameStatus.PAUSED;
                _menu.UpdateStateMenu();
                //_mainMenu.wasEscapeKeyDownBefore = true;
            }
        }

        public void EndGame()
        {
            Utility.GameStatus = GameStatus.FINISHED;
            _menu.UpdateStateMenu();
        }

        public void ReLaunchGame()
        {
            LaunchGame();
            //TODO : Continue this method
            //_menu.UpdateStateMenu();
            //Utility.GameStatus = GameStatus.NOTSTART;
        }
    }
}
