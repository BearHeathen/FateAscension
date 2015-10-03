using RogueSharp.Random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FateAscension
{
    public enum GameStates
    {
        None = 0,
        PlayerTurn = 1,
        EnemyTurn = 2,
        Debugging = 3
    }
    public class Global
    {
        public static GameStates GameState { get; set; }
        public static readonly IRandom Random = new DotNetRandom();
        
    }
}
