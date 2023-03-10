using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Space_Game.Core;
using Space_Game.Core.InputManagement;
using Space_Game.Core.LayerManagement;
using Space_Game.Core.SoundManagement;
using Space_Game.Core.TextureManagement;
using System;
using System.Collections.Generic;

namespace Space_Game
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager mGraphicsDeviceManager;
        private SpriteBatch mSpriteBatch;
        private InputManager mInputManager;
        private SoundManager mSoundManager;
        private LayerManager mLayerManager;
        private TextureManager mTextureManager;

        private int mWidth;
        private int mHeight;
        private bool mIsFullScreen;

        private bool mResulutionWasResized;

        public Game1()
        {
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            mGraphicsDeviceManager = new GraphicsDeviceManager(this);
            mInputManager = new InputManager();
            mSoundManager = new SoundManager();
            mTextureManager = TextureManager.GetInstance();
            Window.ClientSizeChanged += delegate { mResulutionWasResized = true; };
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Globals.mGraphicsDevice = GraphicsDevice;
            Globals.mContentManager = Content;
            Globals.mSoundManager = mSoundManager;
            Globals.mRandom = new Random();
            base.Initialize();
            MouseCursor cursor = MouseCursor.FromTexture2D(Content.Load<Texture2D>("cursor"), 0, 0);
            Mouse.SetCursor(cursor);
        }

        protected override void LoadContent()
        {
            Globals.mSpriteBatch = mSpriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here

            // setup texture manager
            mTextureManager.SetContentManager(Content);
            mTextureManager.SetSpriteBatch(mSpriteBatch);

            mSoundManager.LoadContent(Content, new List<string> { "hit", "engineSound" });

            // Load Star Textures
            mTextureManager.LoadTexture("sunTypeB", "GameObjects/Stars/sunTypeB");
            mTextureManager.LoadTexture("sunTypeF", "GameObjects/Stars/sunTypeF");
            mTextureManager.LoadTexture("sunTypeG", "GameObjects/Stars/sunTypeG");
            mTextureManager.LoadTexture("sunTypeK", "GameObjects/Stars/sunTypeK");
            mTextureManager.LoadTexture("sunTypeM", "GameObjects/Stars/sunTypeM");

            // Load Planet Textures
            mTextureManager.LoadTexture("planetTypeH", "GameObjects/Planets/planetTypeH");
            mTextureManager.LoadTexture("planetTypeJ", "GameObjects/Planets/planetTypeJ");
            mTextureManager.LoadTexture("planetTypeM", "GameObjects/Planets/planetTypeM");
            mTextureManager.LoadTexture("planetTypeY", "GameObjects/Planets/planetTypeY");

            // Load CrossHair
            mTextureManager.LoadTexture("crossHair1", "GameObjects/crossHair/crossHair1");
            mTextureManager.LoadTexture("crossHair2", "GameObjects/crossHair/crossHair2");

            // Load other STuff
            mTextureManager.LoadTexture("gameBackground", "gameBackground");
            mTextureManager.LoadTexture("gameBackgroundParlax", "gameBackgroundParlax");
            mTextureManager.LoadTexture("ship", "GameObjects/Ships/ship");
            mTextureManager.LoadTexture("warpAnimation", "GameObjects/Ships/warpAnimation");
            mTextureManager.LoadTexture("shipHover", "GameObjects/Ships/shipHover");
            mTextureManager.LoadTexture("systemState", "GameObjects/systemState");
            mTextureManager.LoadTexture("cursor", "cursor");
            mTextureManager.LoadTexture("transparent", "GameObjects/transparent");
            mTextureManager.LoadTexture("projectile", "GameObjects/projectile");
            mTextureManager.LoadTexture("spaceStation", "GameObjects/spaceStation");

            // game fonts
            mTextureManager.LoadSpriteTexture("text", "fonts/text");
            mTextureManager.LoadSpriteTexture("title", "fonts/title");
            mTextureManager.LoadSpriteTexture("smal", "fonts/smal");

            Globals.mLayerManager = mLayerManager = new LayerManager(this, GraphicsDevice, mSpriteBatch, Content, mSoundManager);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                Exit();
            }
            // TODO: Add your update logic here
            // handle window resize
            if (mResulutionWasResized)
            {
                mLayerManager.OnResolutionChanged();
            }
            InputState inputState = mInputManager.Update(gameTime);
            mLayerManager.Update(gameTime, inputState, Window, mGraphicsDeviceManager);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            // TODO: Add your drawing code here
            mLayerManager.Draw();
            base.Draw(gameTime);
        }

        // Some Stuff
        public void ToggleFullscreen()
        {
            if (mIsFullScreen)
            {
                UnSetFullscreen();
            }
            else
            {
                SetFullscreen();
            }
            mIsFullScreen = !mIsFullScreen;
        }

        private void SetFullscreen()
        {
            mWidth = Window.ClientBounds.Width;
            mHeight = Window.ClientBounds.Height;

            mGraphicsDeviceManager.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            mGraphicsDeviceManager.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            mGraphicsDeviceManager.IsFullScreen = true;
            mGraphicsDeviceManager.ApplyChanges();
        }

        private void UnSetFullscreen()
        {
            mGraphicsDeviceManager.PreferredBackBufferWidth = mWidth;
            mGraphicsDeviceManager.PreferredBackBufferHeight = mHeight;
            mGraphicsDeviceManager.IsFullScreen = false;
            mGraphicsDeviceManager.ApplyChanges();
        }
    }
}