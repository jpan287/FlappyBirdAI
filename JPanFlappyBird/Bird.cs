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
        Rectangle hitbox;
        Vector2 position;
        static double gravitySpeed = 2.0;
        static double jumpSpeed = 20.0;
        double ySpeed;
        public bool dead = false;

        public Bird(Texture2D Texture, Vector2 Position)
        {
            texture = Texture;
            position = Position;
            hitbox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }

        public void Update(GameTime gametime, KeyboardState keystate)
        {
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
            
            if (position.Y > 800)
            {
                dead = true;
            }
        }

        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(texture, hitbox, Color.White);
        }
    }
}
