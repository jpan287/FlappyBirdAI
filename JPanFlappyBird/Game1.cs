using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MachineLearning;
using System;

namespace JPanFlappyBird
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Bird player;
        Bird[] birds = new Bird[100];
        Pipe pipe1;
        Pipe pipe2;

        Random rand;

        double[][] inputs = new double[100][];

        MachineLearning.GeneticTrainer trainer = new MachineLearning.GeneticTrainer();
        MachineLearning.Network[] networks = new MachineLearning.Network[100]; //(a => 1 / (1 + Math.Exp(-a)), 1, 3, 3, 1);
        (Network, double)[] population = new (Network, double)[100];
    
        bool playing
        {
            get
            {
                var temp = false;
                for (int i = 0; i < birds.Length; i++)
                {
                    if (birds[i].dead == false)
                    {
                        temp = true;
                    }
                }
                return temp;
            }
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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
            graphics.PreferredBackBufferHeight = 800;
            graphics.PreferredBackBufferWidth = 1200;
            graphics.ApplyChanges();
            IsMouseVisible = true;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            rand = new Random();
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //player = new Bird(Content.Load<Texture2D>("flappyBird"), new Vector2(400, 400));
            for (int i = 0; i < networks.Length; i++)
            {
                birds[i] = new Bird(Content.Load<Texture2D>("flappyBird"), new Vector2(400, 400));
                inputs[i] = new double[] { birds[i].xToPipe, birds[i].yToPipe };
                networks[i] = new MachineLearning.Network(a => a < 0 ? 0 : 1, 2, 4, 1);
                networks[i].Randomize(rand);
                population[i] = (networks[i], birds[i].fitness);
            }
            pipe1 = new Pipe(Content.Load<Texture2D>("bottomPipe"), 1200);
            pipe2 = new Pipe(Content.Load<Texture2D>("bottomPipe"), 1600);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            pipe1.Update(gameTime);
            while (playing)
            {
                for (int i = 0; i < birds.Length; i++)
                {
                    birds[i].xToPipe = pipe1.topHitbox.X - birds[i].hitbox.X;
                    birds[i].yToPipe = pipe1.bottomHitbox.Y - birds[i].hitbox.Y;
                    if (birds[i].dead != true)
                    {
                        networks[i].Compute(inputs[i]);
                        birds[i].Update(gameTime, Keyboard.GetState());
                        if (networks[i].Outputs[0] == 1)
                        {
                            birds[i].jump = true;
                        }
                        if (birds[i].hitbox.Intersects(pipe1.bottomHitbox) || birds[i].hitbox.Intersects(pipe1.topHitbox))
                        {
                            birds[i].dead = true;
                        }
                    }
                }
                trainer.Train(population, rand, 0.5);
                base.Update(gameTime);
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            for (int i = 0; i < birds.Length; i++)
            {
                birds[i].Draw(spriteBatch);
            }
            //player.Draw(spriteBatch);
            pipe1.Draw(spriteBatch);
            //pipe2.Draw(spriteBatch);
            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
