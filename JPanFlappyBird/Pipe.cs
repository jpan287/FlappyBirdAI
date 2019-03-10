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
        int heightOfHole;
        int xSpeed = 5;

        public Pipe(Texture2D Texture)
        {
            texture = Texture;
            rand = new Random();
            heightOfHole = rand.Next(100, 700);

            topHitbox = new Rectangle(1200, rand.Next(0, heightOfHole), texture.Width, texture.Height);
            bottomHitbox = new Rectangle(1200, rand.Next(heightOfHole + 150, 800), texture.Width, texture.Height);
        }

        public void Update(GameTime gametime)
        {
            bottomHitbox.X -= xSpeed;
            topHitbox.X -= xSpeed;
            if (bottomHitbox.X < 0)
            {
                heightOfHole = rand.Next(100, 600);

                topHitbox = new Rectangle(1200, rand.Next(0, heightOfHole), texture.Width, texture.Height);
                bottomHitbox = new Rectangle(1200, rand.Next(heightOfHole + 150, 800), texture.Width, texture.Height);
            }
        }

        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(texture, destinationRectangle: topHitbox, color: Color.White, effects: SpriteEffects.FlipVertically);
            spritebatch.Draw(texture, bottomHitbox, Color.White);
        }
    }
}
