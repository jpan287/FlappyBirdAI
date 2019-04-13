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
        public Vector2 position;
        Random rand;
        int height;
        int xSpeed = 7;

        public Pipe(Texture2D Texture, int initialX)
        {
            texture = Texture;
            rand = new Random();
            height = rand.Next(-650, -300);
            position.X = initialX;
            position.Y = height + texture.Height + 100;

            topHitbox = new Rectangle((int)position.X, height, texture.Width, texture.Height);
            bottomHitbox = new Rectangle((int)position.X, height + texture.Height + 200, texture.Width, texture.Height);
        }

        public void Update(GameTime gametime)
        {
            bottomHitbox.X = (int)position.X;
            topHitbox.X = (int)position.X;
            position.X -= xSpeed;
        }

        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(texture, topHitbox, null, Color.White,0, Vector2.Zero, SpriteEffects.FlipVertically, 0);
            spritebatch.Draw(texture, bottomHitbox, Color.White);
        }
    }
}
