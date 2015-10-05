using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueSharp;

namespace FateAscension
{
    public class Player
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Texture2D Sprite { get; set; }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, new Vector2(X * Sprite.Width, Y * Sprite.Height), null, null, null, 0.0f, Vector2.One, Color.White, SpriteEffects.None, LayerDepth.Figures);
        }
        public bool HandleInput(InputState inputState, IMap map)
        {
            if (inputState.IsLeft(PlayerIndex.One))
            {
                int tempX = X - 1;
                if (map.IsWalkable(tempX, Y))
                {
                    X = tempX;
                    return true;
                }
            }
            else if (inputState.IsRight(PlayerIndex.One))
            {
                int tempX = X + 1;
                if (map.IsWalkable(tempX, Y))
                {
                    X = tempX;
                    return true;
                }
            }
            else if (inputState.IsUp(PlayerIndex.One))
            {
                int tempY = Y - 1;
                if (map.IsWalkable(X, tempY))
                {
                    Y = tempY;
                    return true;
                }
            }
            else if (inputState.IsDown(PlayerIndex.One))
            {
                int tempY = Y + 1;
                if (map.IsWalkable(X, tempY))
                {
                    Y = tempY;
                    return true;
                }
            }
            return false;
        }
    }
}