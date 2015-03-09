using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace NNetTut
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;

        internal static KeyboardState keyboardState;
        internal static GameSpeed gameSpeed = new GameSpeed(5);

        int WINDOW_WIDTH = 1200;
        int WINDOW_HEIGHT = 600;

        int NUMBER_OF_SMART_TANKS = 50;
        List<SmartTank> SmartTanks= new List<SmartTank>();
        Sprite smartTankSprite;

        int NUMBER_OF_MINES = 15;
        List<Mine> Mines = new List<Mine>();
        Sprite mineSprite;

        int EXTINCTION_WAIT_TIME { get { return 240000 / gameSpeed.Speed; } }
        int EXTINCTION_COUNT_UP = 0;
        int generationNumber = 1;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //Set resolution and make mouse visible
            graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.IsMouseVisible = true;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            smartTankSprite = new Sprite(Content, "tank");
            for (int tankNum = 0; tankNum < NUMBER_OF_SMART_TANKS; tankNum++)
            {
                SmartTanks.Add(new SmartTank(new Tank(m.random.Next(smartTankSprite.Width / 2, WINDOW_WIDTH - smartTankSprite.Width / 2), m.random.Next(smartTankSprite.Height / 2, WINDOW_HEIGHT - smartTankSprite.Height / 2), 0, smartTankSprite, WINDOW_WIDTH, WINDOW_HEIGHT), new NeuralNet(4,2, new List<int>() { 3 }), 2, 2));
            }

            mineSprite = new Sprite(Content, "mine");
            for (int mineNum = 0; mineNum < NUMBER_OF_MINES; mineNum++)
            {
                Mines.Add(new Mine(m.random.Next(mineSprite.Width / 2, WINDOW_WIDTH - mineSprite.Width / 2), m.random.Next(mineSprite.Height / 2, WINDOW_HEIGHT - mineSprite.Height / 2), mineSprite, WINDOW_WIDTH, WINDOW_HEIGHT));
            }

            spriteFont = Content.Load<SpriteFont>("Arial");
            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            EXTINCTION_COUNT_UP += gameTime.ElapsedGameTime.Milliseconds;
            if (EXTINCTION_COUNT_UP > EXTINCTION_WAIT_TIME)
            {
                List<Genome> currentPopulation = new List<Genome>();
                foreach (SmartTank smartTank in SmartTanks)
                {
                    currentPopulation.Add(smartTank.Genome);
                }
                List<Genome> updatedPopulation = m.GeneticAlgorithm(currentPopulation);
                for(int i=0; i < SmartTanks.Count; i++)
                {
                    SmartTanks[i].Genome = updatedPopulation[i];
                }
                EXTINCTION_COUNT_UP -= EXTINCTION_WAIT_TIME;
                generationNumber++;
            }

            keyboardState = Keyboard.GetState();
            gameSpeed.Update(keyboardState);

            foreach (SmartTank smartTank in SmartTanks)
            {
                smartTank.Update(gameTime, Mines);
            }

            foreach (Mine mine in Mines)
            {
                mine.Update(gameTime, SmartTanks, Mines);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.OliveDrab);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            foreach (Mine mine in Mines)
            {
                mine.Draw(spriteBatch);
            }

            foreach (SmartTank smartTank in SmartTanks)
            {
                smartTank.Draw(spriteBatch);
            }

            spriteBatch.DrawString(spriteFont, String.Format("Generation: {0}", generationNumber), new Vector2((float)(WINDOW_WIDTH * (0f / 6f)), (float)(WINDOW_HEIGHT * (0f / 6f))), Color.White);
            spriteBatch.DrawString(spriteFont, String.Format("Game Speed: {0}", gameSpeed.Speed), new Vector2((float)(WINDOW_WIDTH * (5f / 6f)), (float)(WINDOW_HEIGHT * (0f / 6f))), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
