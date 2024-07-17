using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameProject
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _idleTexture, _walkTexture;
        private int _currentFrame;
        private double _frameTimer;
        private const double FrameTime = 100;
        private Rectangle _sourceRectangle;
        private Vector2 _position;
        private float _speed = 2f;
        private bool _isMoving;
        private int _idleFrames = 5;
        private int _walkFrames = 6;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _currentFrame = 0;
            _frameTimer = FrameTime;
            _position = new Vector2(100, 100);
            _isMoving = false;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _idleTexture = Content.Load<Texture2D>("Hero/Idle");
            _walkTexture = Content.Load<Texture2D>("Hero/Walk");


            int frameWidth = _idleTexture.Width / _idleFrames;
            _sourceRectangle = new Rectangle(0, 0, frameWidth, _idleTexture.Height);
        }

        private SpriteEffects _spriteEffect = SpriteEffects.None;

        protected override void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();
            bool wasMoving = _isMoving;
            _isMoving = false;

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                _position.X -= _speed;
                _isMoving = true;
                _spriteEffect = SpriteEffects.FlipHorizontally;
            }
            else if (keyboardState.IsKeyDown(Keys.Right))
            {
                _position.X += _speed;
                _isMoving = true;
                _spriteEffect = SpriteEffects.None;
            }

            // Update animation only if moving
            if (_isMoving)
            {
                _frameTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
                if (_frameTimer <= 0)
                {
                    _frameTimer = FrameTime;
                    _currentFrame = (_currentFrame + 1) % _walkFrames; // Loop through walk frames
                    _sourceRectangle.X = _currentFrame * _sourceRectangle.Width;
                }
            }
            else
            {

                if (wasMoving)
                {
                    // Reset to the first frame of the idle animation when stopping
                    _currentFrame = 0;
                    _sourceRectangle.X = 0;

                }

                else
                {
                    _frameTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (_frameTimer <= 0)
                    {
                        _frameTimer = FrameTime;
                        _currentFrame = (_currentFrame + 1) % _idleFrames; // Loop through idle frames
                        _sourceRectangle.X = _currentFrame * (_idleTexture.Width / _idleFrames);
                    }
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            Texture2D currentTexture = _isMoving ? _walkTexture : _idleTexture;
            // Use _spriteEffect to flip the sprite based on direction
            _spriteBatch.Draw(currentTexture, _position, _sourceRectangle, Color.White, 0f, Vector2.Zero, 1f, _spriteEffect, 0f);
            _spriteBatch.End();

            base.Draw(gameTime);
        }


    }
}