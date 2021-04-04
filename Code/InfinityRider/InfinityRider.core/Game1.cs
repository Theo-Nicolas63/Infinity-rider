using InfinityRider.core.RiderGame;
using InfinityRider.core.RiderGame.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace InfinityRider.core
{
    public class Game1 : Game
    {
        /// <summary>
        /// Level which allows to manage elements of the game
        /// </summary>
        private Level level;
    
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public GraphicsDevice _device { get; private set; }

        /// <summary>
        /// Status of the game
        /// </summary>
        public GameStatus Status { get; set; } = GameStatus.NOTSTART;

        public int Width { get; set; } = 1700;
        public int Height { get; set; } = 900;

        private SpriteFont font;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        /// <summary>
        /// allows to initialize 
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();

            _graphics.PreferredBackBufferWidth = Width;
            _graphics.PreferredBackBufferHeight = Height;
            _graphics.ApplyChanges();

            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += WindowClientChanged;

            _device = this.GraphicsDevice;
            this.level = new Level(this, _spriteBatch, _device, font);
        }

        private void WindowClientChanged(object sender, EventArgs e) 
        {
            Width = GraphicsDevice.Viewport.Width;
            Height = GraphicsDevice.Viewport.Height;
        }

        /// <summary>
        /// allows to load contents 
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Score");
            // TODO: use this.Content to load your game content here
        }


        /// <summary>
        /// main update  
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                level.PauseGame();

            // TODO: Add your update logic here
            level.Update(gameTime);
            
            base.Update(gameTime);
        }

        /// <summary>
        /// main draw
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // TODO: Add your drawing code here
            level.Draw(gameTime, _spriteBatch);

            base.Draw(gameTime);
        }
    }
}
