using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MachineLearning;
using System;
using System.Collections.Generic;

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
        Bird[] birds = new Bird[500];
        List<Pipe> pipes = new List<Pipe>();

        TimeSpan SpawnPipeTime = TimeSpan.FromMilliseconds(1200);
        TimeSpan elapsedSpawnPipeTime;

        Random rand;
        
        MachineLearning.GeneticTrainer trainer = new MachineLearning.GeneticTrainer();
        MachineLearning.Network[] networks = new MachineLearning.Network[500]; //(a => 1 / (1 + Math.Exp(-a)), 1, 3, 3, 1);
        (Network, double)[] population = new (Network, double)[500];
    
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
                        break;
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
                networks[i] = new MachineLearning.Network(a => a < 0 ? 0 : 1, 2, 4, 1);
                networks[i].Randomize(rand);
                population[i] = (networks[i], birds[i].fitness);
            }
            pipes.Add(new Pipe(Content.Load<Texture2D>("bottomPipe"), GraphicsDevice.Viewport.Width));
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
            for (int i = 0; i < pipes.Count; i++)
            {
                pipes[i].Update(gameTime);
            }
            if (playing)
            {
                for (int i = 0; i < pipes.Count; i++)
                {
                    if (pipes[i].bottomHitbox.X < 0)
                    {
                        pipes.RemoveAt(i);
                    }
                }

                elapsedSpawnPipeTime += gameTime.ElapsedGameTime;

                if (elapsedSpawnPipeTime >= SpawnPipeTime)
                {
                    elapsedSpawnPipeTime = TimeSpan.Zero;
                    pipes.Add(new Pipe(Content.Load<Texture2D>("bottomPipe"), GraphicsDevice.Viewport.Width));
                }
                for (int i = 0; i < birds.Length; i++)
                {
                    if (!birds[i].dead)
                    {
                        birds[i].xToPipe = pipes[0].topHitbox.X + pipes[0].bottomHitbox.Width / 2 - birds[i].hitbox.X;
                        birds[i].yToPipe = pipes[0].bottomHitbox.Y + 100 - birds[i].hitbox.Y;                        

                        networks[i].Compute(new double[] { birds[i].xToPipe, birds[i].yToPipe });
                        
                        if (networks[i].Outputs[0] == 1)
                        {
                            birds[i].jump = true;
                        }
                        if (birds[i].hitbox.Intersects(pipes[0].bottomHitbox) || birds[i].hitbox.Intersects(pipes[0].topHitbox))
                        {
                            birds[i].dead = true;
                        }

                        birds[i].Update(gameTime, Keyboard.GetState());
                    }
                }
            }
            else
            {
                trainer.Train(population, rand, 0.25);
                
                for (int i = 0; i < birds.Length; i++)
                {
                    birds[i].position.Y = 400;
                    birds[i].dead = false;
                    birds[i].fitness = 0;
                }
                pipes.Clear();
                pipes.Add(new Pipe(Content.Load<Texture2D>("bottomPipe"), GraphicsDevice.Viewport.Width));
                elapsedSpawnPipeTime = TimeSpan.Zero;
            }

            base.Update(gameTime);
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
            for (int i = 0; i < pipes.Count; i++)
            {
                pipes[i].Draw(spriteBatch);
            }
            //pipe2.Draw(spriteBatch);
            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
