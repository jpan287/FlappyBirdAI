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
        public Vector2 position;
        static float gravitySpeed = 0.75f;
        static float jumpSpeed = -9.8f;
        float ySpeed;
        public double fitness;
        public bool dead = false;
        public bool jump = false;

        public double xToPipe;
        public double yToPipe;

        public Bird(Texture2D Texture, Vector2 Position)
        {
            texture = Texture;
            position = Position;
            hitbox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }

        public void Update(GameTime gametime, KeyboardState keystate)
        {
            hitbox.X = (int)position.X;
            hitbox.Y = (int)position.Y;
            this.ks = keystate;

            if (jump)
            {
                ySpeed = jumpSpeed;
                jump = false;                
            }
            else
            {
                ySpeed += gravitySpeed;
            }
            position.Y += ySpeed;
            
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
            spritebatch.Draw(texture, position, Color.White);
        }
    }
}
