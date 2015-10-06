using Microsoft.Xna.Framework.Graphics;
using RogueSharp.DiceNotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FateAscension
{
    public class Figure
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Texture2D Sprite { get; set; }

        // Roll a 20-sided die and add this value when making an attack
        public int AttackBonus { get; set; }
        // An attack must meet or exceed this value to hit
        public int ArmorClass { get; set; }
        // Roll these dice to determine how much damage was dealt after a hit
        public DiceExpression Damage { get; set; }
        // How many points of damage the figure can withstand before dying
        public int Health { get; set; }
        // The name of the figure, used for attack messages
        public string Name { get; set; }

    }
}
