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
        /// <summary>
        /// road of the game
        /// </summary>
        private RoadConstructor currentRoad;

        /// <summary>
        /// instance of the bike
        /// </summary>
        private Bike currentBike;
        /// <summary>
        /// Background of the game
        /// </summary>
        public Background Background { get; private set; }
        /// <summary>
        /// list of gameObjects
        /// </summary>
        private IList<GameObject> GameObjects { get; set; } = new List<GameObject>();

        /// <summary>
        /// to get the size of the screen
        /// </summary>
        public GraphicsDevice _device;

        private Texture2D testCollision;

        private Rectangle rect;

        /// <summary>
        /// current game
        /// </summary>
        private Game1 game;

        /// <summary>
        /// Menu of the game
        /// </summary>
        private MenuGame menu;

        /// <summary>
        /// score of the game
        /// </summary>
        public int Score { get; private set; } = 0;

        /// <summary>
        /// font
        /// </summary>
        private SpriteFont _font;

        /// <summary>
        /// biggest jump of the current game
        /// </summary>
        public int MaxJump { get; private set;} = 0;

        /// <summary>
        /// timer of the game (30 sec)
        /// </summary>
        private float Timer;
        private int TimeCompteur = 30;

        public Level(Game game, SpriteBatch spriteBatch, GraphicsDevice device, SpriteFont font)
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
            _font = font;
            menu = new MenuGame(game, this);
        }

        /// <summary>
        /// allows to verify collsions when the bike falls 
        /// </summary>
        /// <param name="futurPosition"> future position of the bike if there is no collision</param>
        /// <returns>return true if there is collision and false if there is one</returns>
        public bool IsCollisionGravity(Vector2 futurPosition)
        {
            Rectangle terrain = new Rectangle((int)futurPosition.X, currentRoad.getTerrainContour((int)futurPosition.X), 10, 20);
            rect = terrain;
            if (currentBike.BoundingRectangle.Intersects(terrain))
            {
                if (MaxJump < Score && TimeCompteur < 29)
                    MaxJump = Score;
                Score = 0;
                return true;
            }
            else
            {
                Score++;
                return false;
            }
                
        }

        /// <summary>
        /// allows to verifiy if the road mounts and allows the bike to go up
        /// </summary>
        /// <param name="gameTime">recipes the gameTime of the game</param>
        public void IsCollisionForward(GameTime gameTime)
        {
            Rectangle terrain = new Rectangle((int)currentBike.Position.X, currentRoad.getTerrainContour((int)currentBike.Position.X), 10, 20);
            rect = terrain;
            while (currentBike.BoundingRectangle.Intersects(terrain))
            {
                if (currentRoad.getTerrainContour((int)currentBike.Position.X + 10) < currentRoad.getTerrainContour((int)currentBike.Position.X) - 5)
                {
                    if(currentBike.GravityAcceleration > -1000)
                    currentBike.GravityAcceleration -= 50;
                }
                 else currentBike.GravityAcceleration = -10;
                
                currentBike.Position = Vector2.Add(currentBike.Position, new Vector2(0, currentBike.GravityAcceleration * (float)gameTime.ElapsedGameTime.TotalSeconds));
            }
            if(currentBike.GravityAcceleration < 500)
                currentBike.GravityAcceleration = currentBike.GravityAcceleration+20;
        }


        /// <summary>
        /// update of the game which allows to manage actions of the game
        /// </summary>
        /// <param name="gameTime"></param>
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
            if(game.Status == GameStatus.PROCESSING)
            { 
                Timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                TimeCompteur -= (int)Timer;
                if (Timer >= 1.0F) Timer = 0F;
            }

            if(TimeCompteur == 0)
            {
                EndGame();
            }

           
        }
        /// <summary>
        ///  draw of a rectangle, used for testing collisions
        /// </summary>
         public void DrawRectangle(Rectangle coords, Color color, SpriteBatch spriteBatch)
        {
            if (testCollision == null)
            {
                testCollision = new Texture2D(_device, 1, 1);
                testCollision.SetData(new[] { Color.White });
            }
            spriteBatch.Draw(testCollision, coords, color);
        }

        /// <summary>
        /// allows to display elements of the game
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
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
                spriteBatch.DrawString(_font, "Score : " + Score, new Vector2(100, 100), Color.White);
                spriteBatch.DrawString(_font, "Biggest jump :" + MaxJump, new Vector2(100, 135), Color.White);
                spriteBatch.DrawString(_font, "Time : " + TimeCompteur, new Vector2(_device.Viewport.Bounds.Width/2, 120), Color.White);
            }

            
            //DrawRectangle(rect, Color.White, spriteBatch);
            spriteBatch.End();
        }

        
        public void Dispose()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        ///allows to launch a game
        /// </summary>
        public void LaunchGame()
        {
            game.Status = GameStatus.PROCESSING;
            menu.UpdateStateMenu();
        }
        /// <summary>
        /// allows to paude a game (not used)
        /// </summary>
        public void PauseGame()
        {
            if (game.Status == GameStatus.PROCESSING)
            {
                game.Status = GameStatus.PAUSED;
                menu.UpdateStateMenu();
            }
        }
        /// <summary>
        /// allows to end the game and to call the end game menu
        /// </summary>
        public void EndGame()
        {
            currentRoad.ReinitializeRoad();
            game.Status = GameStatus.FINISHED;
            menu.UpdateStateMenu();
        }
        /// <summary>
        /// allows to launch a new game and to reinitialize elements
        /// </summary>
        public void ReLaunchGame()
        {
            currentRoad.ReinitializeRoad();
            currentBike.Position = new Vector2(currentBike.Position.X, 5);
            Score = 0;
            MaxJump = 0;
            TimeCompteur = 30;
            LaunchGame();
        }


    }
}
