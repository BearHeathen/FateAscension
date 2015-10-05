#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueSharp;

#endregion

namespace FateAscension
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class FateAscensionRoguelike : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Texture2D _floor;
        private Texture2D _wall;
        private IMap _map;
        private Player _player;
        private AggressiveEnemy _aggressiveEnemy;
        private InputState _inputState;

        public FateAscensionRoguelike()
           : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _inputState = new InputState();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Global.Camera.ViewportWidth = graphics.GraphicsDevice.Viewport.Width;
            Global.Camera.ViewportHeight = graphics.GraphicsDevice.Viewport.Height;
            IMapCreationStrategy<Map> mapCreationStrategy = new RandomRoomsMapCreationStrategy<Map>(Global.MapWidth, Global.MapHeight, 100, 7, 3);
            _map = Map.Create(mapCreationStrategy);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _floor = Content.Load<Texture2D>("Floor");
            _wall = Content.Load<Texture2D>("Wall");
            Cell startingCell = GetRandomEmptyCell();
            _player = new Player
            {
                X = startingCell.X,
                Y = startingCell.Y,
                Sprite = Content.Load<Texture2D>("Player")
            };
            UpdatePlayerFieldOfView();
            Global.Camera.CenterOn(startingCell);
            startingCell = GetRandomEmptyCell();
            var pathFromAggressiveEnemy = new PathToPlayer(_player, _map, Content.Load<Texture2D>("White"));
            pathFromAggressiveEnemy.CreateFrom(startingCell.X, startingCell.Y);
            _aggressiveEnemy = new AggressiveEnemy(pathFromAggressiveEnemy)
            {
                X = startingCell.X,
                Y = startingCell.Y,
                Sprite = Content.Load<Texture2D>("Hound")
            };
            Global.GameState = GameStates.PlayerTurn;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            _inputState.Update();
            Global.Camera.HandleInput(_inputState, PlayerIndex.One);
            if (_inputState.IsExitGame(PlayerIndex.One))
            {
                Exit();
            }
            else if (_inputState.IsSpace(PlayerIndex.One))
            {
                if (Global.GameState == GameStates.PlayerTurn)
                {
                    Global.GameState = GameStates.Debugging;
                }
                else if (Global.GameState == GameStates.Debugging)
                {
                    Global.GameState = GameStates.PlayerTurn;
                }
            }
            else
            {
                if (Global.GameState == GameStates.PlayerTurn
                   && _player.HandleInput(_inputState, _map))
                {
                    UpdatePlayerFieldOfView();
                    Global.Camera.CenterOn(_map.GetCell(_player.X, _player.Y));
                    Global.GameState = GameStates.EnemyTurn;
                }
                if (Global.GameState == GameStates.EnemyTurn)
                {
                    _aggressiveEnemy.Update();
                    Global.GameState = GameStates.PlayerTurn;
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, Global.Camera.TranslationMatrix);

            foreach (Cell cell in _map.GetAllCells())
            {
                var position = new Vector2(cell.X * Global.SpriteWidth, cell.Y * Global.SpriteHeight);
                if (!cell.IsExplored && Global.GameState != GameStates.Debugging)
                {
                    continue;
                }
                Color tint = Color.White;
                if (!cell.IsInFov && Global.GameState != GameStates.Debugging)
                {
                    tint = Color.Gray;
                }
                if (cell.IsWalkable)
                {
                    spriteBatch.Draw(_floor, position, null, null, null, 0.0f, Vector2.One, tint, SpriteEffects.None, LayerDepth.Cells);
                }
                else
                {
                    spriteBatch.Draw(_wall, position, null, null, null, 0.0f, Vector2.One, tint, SpriteEffects.None, LayerDepth.Cells);
                }
            }

            _player.Draw(spriteBatch);
            if (Global.GameState == GameStates.Debugging || _map.IsInFov(_aggressiveEnemy.X, _aggressiveEnemy.Y))
            {
                _aggressiveEnemy.Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void UpdatePlayerFieldOfView()
        {
            _map.ComputeFov(_player.X, _player.Y, 30, true);
            foreach (Cell cell in _map.GetAllCells())
            {
                if (_map.IsInFov(cell.X, cell.Y))
                {
                    _map.SetCellProperties(cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable, true);
                }
            }
        }

        private Cell GetRandomEmptyCell()
        {
            while (true)
            {
                int x = Global.Random.Next(49);
                int y = Global.Random.Next(29);
                if (_map.IsWalkable(x, y))
                {
                    return _map.GetCell(x, y);
                }
            }
        }
    }
}