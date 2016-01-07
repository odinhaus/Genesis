using Genesis.XNA.Cameras;
using Genesis.XNA.Cursors;
using Genesis.XNA.Diagnostics;
using Genesis.XNA.Players;
using Genesis.XNA.Primitives;
using Genesis.XNA.Shapes._2D;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Genesis.UAP
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        const float PI2 = (float)(Math.PI * 2f);

        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        SpriteFont _font;
        Genesoid[] _players;
        Camera _gameCamera;
        KeyboardState _keyboardState;
        MouseState _mouseState;
        Label _label;
        Label _label2;
        Label _label3;
        Label _label4;
        Label _label5;
        Label _label6;
        Cursor _cursor;
        float _zoom = 100f;
        Matrix _currentProjection;

        RoundedLine _line;

        const float worldSize = 100f;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            this.TargetElapsedTime = TimeSpan.FromTicks((long)10000000 / (long)120);
            _graphics.SynchronizeWithVerticalRetrace = true;
            //this.IsFixedTimeStep = true;
            _graphics.ApplyChanges();
            _gameCamera = new Camera(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 0.01f, worldSize * 8);
            _gameCamera.ZoomOut(_zoom);
            _gameCamera.Range.Position.Max = new Vector3(worldSize, worldSize * 4, worldSize);
            _gameCamera.Range.Position.Min = new Vector3(0, 10, 0);
            _currentProjection = _gameCamera.Projection;

            //_gameCamera.View = Matrix.CreateLookAt(_gameCamera.Position, Vector3.Zero, Vector3.Up);

            _keyboardState = Keyboard.GetState();
            _mouseState = Mouse.GetState();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            BasicEffect be = new BasicEffect(GraphicsDevice);
            be.VertexColorEnabled = true;

            Random random = new Random();
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _players = new Genesoid[10];
            for (int p = 0; p < _players.Length; p++)
            {
                Genesoid player = new Genesoid(this.GraphicsDevice, 1.5f, Color.White);
                player.Position = new Vector3(RandomFloat(random, -10f, 10f), 0f, RandomFloat(random, -10f, 10f));
                player.Velocity = new Vector3(
                    RandomFloat(random, -20f, 20f),
                    0f,
                    RandomFloat(random, -20f, 20f));
                _players[p] = player;
            }

            _font = Content.Load<SpriteFont>(@"Fonts/Font");
            FrameRate fr = new FrameRate(this, _spriteBatch, _font);
            Components.Add(fr);

            _label = new Label(this, _spriteBatch, _font);
            _label.Position = new Vector2(0, 20);
            Components.Add(_label);

            _label2 = new Label(this, _spriteBatch, _font);
            _label2.Position = new Vector2(0, 40);
            Components.Add(_label2);
            _label3 = new Label(this, _spriteBatch, _font);
            _label3.Position = new Vector2(0, 60);
            Components.Add(_label3);
            _label4 = new Label(this, _spriteBatch, _font);
            _label4.Position = new Vector2(0, 80);
            Components.Add(_label4);
            _label5 = new Label(this, _spriteBatch, _font);
            _label5.Position = new Vector2(0, 100);
            Components.Add(_label5);
            _label6 = new Label(this, _spriteBatch, _font);
            _label6.Position = new Vector2(0, 120);
            Components.Add(_label6);

            _cursor = new Cursor(this, _gameCamera);

            _label.Caption = "Camera";
            _label.Value = _gameCamera.Position.ToString();
            _label2.Caption = "Up";
            _label2.Value = _gameCamera.Up.ToString();
            _label3.Caption = "Theta";
            _label3.Value = _gameCamera.Theta.ToString();
            _label4.Caption = "Target";
            _label4.Value = _gameCamera.Target.ToString();
            _label5.Caption = "Mouse";
            _label5.Value = _cursor.Position.ToString();
            _label6.Caption = "ViewPort";
            _label6.Value = GraphicsDevice.Viewport.ToString();


            _line = new RoundedLine(GraphicsDevice, 3f, 20f, 20, Color.Yellow);
            _line.Position = Vector3.Zero;
            _line.Velocity = Vector3.Zero;
            _line.Rotation = new Vector3(0, 0, 0);
        }

        private static float RandomFloat(Random random, float min, float max)
        {
            return (float)random.NextDouble() * (max - min) + min;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            foreach (Genesoid player in _players)
            {
                BounceSphereInWorld(player, gameTime);
                player.Update(gameTime);
            }

            var rotation = (float)(gameTime.TotalGameTime.TotalSeconds % 1) * PI2;
            _line.Rotation = new Vector3(0, -rotation, 0);
            _line.Update(gameTime);

            UpdateKeyboardState(gameTime);
            UpdateMouseState(gameTime);
            UpdateWindowState(gameTime);


            _gameCamera.Update(gameTime);
            _cursor.Update(gameTime);
            base.Update(gameTime);
        }

        Viewport _lastViewPort;
        private void UpdateWindowState(GameTime gameTime)
        {
            _label6.Caption = "ViewPort";
            _label6.Value = GraphicsDevice.Viewport.ToString();
            if (GraphicsDevice.Viewport.Width != _lastViewPort.Width
                || GraphicsDevice.Viewport.Height != _lastViewPort.Height)
            {
                _graphics.PreferredBackBufferHeight = GraphicsDevice.Viewport.Height;
                _graphics.PreferredBackBufferWidth = GraphicsDevice.Viewport.Width;
                _graphics.ApplyChanges();
                _lastViewPort = GraphicsDevice.Viewport;
                _currentProjection = GetCurrentProjection();

                //_gameCamera.z
            }
        }

        private void UpdateMouseState(GameTime gameTime)
        {
            MouseState state = Mouse.GetState();
            try
            {
                int wheel = state.ScrollWheelValue - _mouseState.ScrollWheelValue;
                if (wheel != 0)
                {
                    if (wheel > 0)
                    {
                        // Zoom in 
                        _gameCamera.ZoomIn(2f);
                    }
                    else
                    {
                        // Zoom out 
                        _gameCamera.ZoomOut(2f);
                    }
                    _label.Caption = "Camera";
                    _label.Value = _gameCamera.Position.ToString();
                    _label2.Caption = "Up";
                    _label2.Value = _gameCamera.Up.ToString();
                    _label3.Caption = "Theta";
                    _label3.Value = _gameCamera.Theta.ToString();
                    _label4.Caption = "Target";
                    _label4.Value = _gameCamera.Target.ToString();
                }
            }
            finally
            {
                _label5.Caption = "Mouse";
                _label5.Value = state.Position.ToString();
                _mouseState = state;
            }
        }

        private void UpdateKeyboardState(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();
            try
            {
                Vector3 camPos = _gameCamera.Position;
                Vector3 playPos = Vector3.Zero;
                Vector3 centered = camPos - playPos;
                if (state.IsKeyDown(Keys.Down))
                {
                    _gameCamera.OrbitVertically(0.5f);
                }
                else if (state.IsKeyDown(Keys.Up))
                {
                    _gameCamera.OrbitVertically(-0.5f);
                }
                if (state.IsKeyDown(Keys.Left))
                {
                    _gameCamera.OrbitHorizontally(-0.5f);
                }
                else if (state.IsKeyDown(Keys.Right))
                {
                    _gameCamera.OrbitHorizontally(0.5f);
                }
                if (state.IsKeyDown(Keys.OemMinus))
                {
                    _gameCamera.ZoomOut(2f);
                }
                else if (state.IsKeyDown(Keys.OemPlus))
                {
                    _gameCamera.ZoomIn(2f);
                }
                if (state.IsKeyDown(Keys.OemComma))
                {
                    _gameCamera.Roll(-0.5f);
                }
                else if (state.IsKeyDown(Keys.OemPeriod))
                {
                    _gameCamera.Roll(0.5f);
                }
                if (state.IsKeyDown(Keys.A))
                {
                    _gameCamera.PanLeft(0.25f);
                }
                else if (state.IsKeyDown(Keys.D))
                {
                    _gameCamera.PanRight(0.25f);
                }
                if (state.IsKeyDown(Keys.W))
                {
                    _gameCamera.PanUp(0.25f);
                }
                else if (state.IsKeyDown(Keys.S))
                {
                    _gameCamera.PanDown(0.25f);
                }
                if (state.GetPressedKeys().Length > 0)
                {
                    _label.Caption = "Camera";
                    _label.Value = _gameCamera.Position.ToString();
                    _label2.Caption = "Up";
                    _label2.Value = _gameCamera.Up.ToString();
                    _label3.Caption = "Theta";
                    _label3.Value = _gameCamera.Theta.ToString();
                    _label4.Caption = "Target";
                    _label4.Value = _gameCamera.Target.ToString();
                }
            }
            finally
            {
                _keyboardState = state;
            }
        }

        private static void BounceSphereInWorld(IMovable3 s, GameTime gameTime)
        {
            // vy = ay * dt + v0;
            Vector3 velocity = s.Velocity;
            Vector3 position = s.Position;
            if (velocity.Y > 0)
                velocity.Y = velocity.Y - (float)(2f * 9.8f * gameTime.ElapsedGameTime.TotalSeconds);
            // First test along the X axis, flipping the velocity if a collision occurs.
            if (s.Position.X < -worldSize + s.Radius)
            {
                position.X = -worldSize + s.Radius;
                if (s.Velocity.X < 0f)
                    velocity.X *= -1f;
            }
            else if (s.Position.X > worldSize - s.Radius)
            {
                position.X = worldSize - s.Radius;
                if (s.Velocity.X > 0f)
                    velocity.X *= -1f;
            }

            // Then we test the Y axis
            if (s.Position.Y < s.Radius)
            {
                position.Y = s.Radius;
                if (s.Velocity.Y < 0f)
                    velocity.Y *= -1f;
            }
            else if (s.Position.Y > worldSize - s.Radius)
            {
                position.Y = worldSize - s.Radius;
                if (s.Velocity.Y > 0f)
                    velocity.Y *= -1f;
            }

            // And lastly the Z axis
            if (s.Position.Z < -worldSize + s.Radius)
            {
                position.Z = -worldSize + s.Radius;
                if (s.Velocity.Z < 0f)
                    velocity.Z *= -1f;
            }
            else if (s.Position.Z > worldSize - s.Radius)
            {
                position.Z = worldSize - s.Radius;
                if (s.Velocity.Z > 0f)
                    velocity.Z *= -1f;
            }
            s.Position = position;
            s.Velocity = velocity;
            s.Rotation = new Vector3(0, PI2 * (float)(gameTime.TotalGameTime.TotalSeconds % 1), 0);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            // Create a view and projection matrix for our camera
            foreach (Genesoid player in _players)
                player.Draw(_gameCamera.View, _currentProjection);
            _cursor.Draw(_gameCamera.View, _currentProjection);
            _line.Draw(_gameCamera.View, _currentProjection);

            base.Draw(gameTime);
        }

        private Matrix GetCurrentProjection()
        {
            return Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 0.01f, worldSize * 8);
        }
    }
}
