using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RogueSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FateAscension
{
    public class Player
    {
        public int X { get; set; }
        public int Y { get; set; }
        public float Scale { get; set; }
        public Texture2D Sprite { get; set; }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            float multiplier = Scale * (Sprite.Width);
            spriteBatch.Draw(Sprite, new Vector2(X  * multiplier, Y  * multiplier), 
                null, null, null, 0.0f, new Vector2(Scale, Scale), 
                Color.White, SpriteEffects.None, 0.5f);
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
