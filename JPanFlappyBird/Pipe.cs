using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JPanFlappyBird
{
    public class Pipe
    {
        Texture2D texture;
        public Rectangle topHitbox;
        public Rectangle bottomHitbox;
        Random rand;
        int height;
        int xSpeed = 5;
        int points = 0;

        public Pipe(Texture2D Texture)
        {
            texture = Texture;
            rand = new Random();
            height = rand.Next(-650, -300);

            topHitbox = new Rectangle(1200, height, texture.Width, texture.Height);
            bottomHitbox = new Rectangle(1200, height + texture.Height + 200, texture.Width, texture.Height);
        }

        public void Update(GameTime gametime)
        {
            bottomHitbox.X -= xSpeed;
            topHitbox.X -= xSpeed;
            if (bottomHitbox.X < 0)
            {
                height = rand.Next(-650, -300);

                topHitbox = new Rectangle(1200, height, texture.Width, texture.Height);
                bottomHitbox = new Rectangle(1200, height + texture.Height + 200, texture.Width, texture.Height);
                points++;
            }
        }

        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(texture, topHitbox, null, Color.White,0, Vector2.Zero, SpriteEffects.FlipVertically, 0);
            spritebatch.Draw(texture, bottomHitbox, Color.White);
        }
    }
}
