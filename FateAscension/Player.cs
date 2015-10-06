using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueSharp;

namespace FateAscension
{
    public class Player : Figure
    {
        
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
                    // Check to see if there is an enemy at the location
                    // that the player is attempting to move into
                    var enemy = Global.CombatManager.EnemyAt(tempX, Y);
                    if (enemy == null)
                    {
                        // When there is not an enemy, move as normal
                        X = tempX;
                    }
                    else
                    {
                        // When there is an enemy in the cell, make an
                        // attack against them by using the CombatManager
                        Global.CombatManager.Attack(this, enemy);
                    }
                    return true;
                }
            }
            else if (inputState.IsRight(PlayerIndex.One))
            {
                int tempX = X + 1;
                if (map.IsWalkable(tempX, Y))
                {
                    // Check to see if there is an enemy at the location
                    // that the player is attempting to move into
                    var enemy = Global.CombatManager.EnemyAt(tempX, Y);
                    if (enemy == null)
                    {
                        // When there is not an enemy, move as normal
                        X = tempX;
                    }
                    else
                    {
                        // When there is an enemy in the cell, make an
                        // attack against them by using the CombatManager
                        Global.CombatManager.Attack(this, enemy);
                    }
                    return true;
                }
            }
            else if (inputState.IsUp(PlayerIndex.One))
            {
                int tempY = Y - 1;
                if (map.IsWalkable(X, tempY))
                {
                    // Check to see if there is an enemy at the location
                    // that the player is attempting to move into
                    var enemy = Global.CombatManager.EnemyAt(X, tempY);
                    if (enemy == null)
                    {
                        // When there is not an enemy, move as normal
                        Y = tempY;
                    }
                    else
                    {
                        // When there is an enemy in the cell, make an
                        // attack against them by using the CombatManager
                        Global.CombatManager.Attack(this, enemy);
                    }
                    return true;
                }
            }
            else if (inputState.IsDown(PlayerIndex.One))
            {
                int tempY = Y + 1;
                if (map.IsWalkable(X, tempY))
                {
                    // Check to see if there is an enemy at the location
                    // that the player is attempting to move into
                    var enemy = Global.CombatManager.EnemyAt(X, tempY);
                    if (enemy == null)
                    {
                        // When there is not an enemy, move as normal
                        Y = tempY;
                    }
                    else
                    {
                        // When there is an enemy in the cell, make an
                        // attack against them by using the CombatManager
                        Global.CombatManager.Attack(this, enemy);
                    }
                    return true;
                }
            }
            return false;
        }
    }
}