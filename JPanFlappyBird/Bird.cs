using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JPanFlappyBird
{
    public class Bird
    {
        KeyboardState ks = new KeyboardState();
        Texture2D texture;
        public Rectangle hitbox;
        Vector2 position;
        static float gravitySpeed = 0.75f;
        static float jumpSpeed = -10.0f;
        float ySpeed;
        double fitness;
        public bool dead = false;

        public double xToPipe;
        public double yToPipe;

        MachineLearning.GeneticTrainer GeneticTrainer = new MachineLearning.GeneticTrainer();
        MachineLearning.Network[] networks = new MachineLearning.Network[1000]; //(a => 1 / (1 + Math.Exp(-a)), 1, 3, 3, 1);
        Random rand;
        double[] inputs;

        public Bird(Texture2D Texture, Vector2 Position)
        {
            rand = new Random();
            texture = Texture;
            position = Position;
            hitbox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            for (int i = 0; i < networks.Length; i++)
            {
                networks[i] = new MachineLearning.Network(a => a < 0 ? 0 : 1, 2, 4, 1);
                networks[i].Randomize(rand);
            }
        }

        public void Update(GameTime gametime, KeyboardState keystate)
        {
            hitbox.X = (int)position.X;
            hitbox.Y = (int)position.Y;
            this.ks = keystate;
            if (ks.IsKeyDown(Keys.Space))
            {
                ySpeed = jumpSpeed;
            }
            else
            {
                ySpeed += gravitySpeed;
            }

            position.Y += (float)ySpeed;
            
            if (position.Y > 800 || position.Y < -50)
            {
                dead = true;
            }
            
            if (dead != true)
            {
                fitness += gametime.ElapsedGameTime.TotalMilliseconds;
            }
        }

        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(texture, hitbox, Color.White);
        }
    }
}
