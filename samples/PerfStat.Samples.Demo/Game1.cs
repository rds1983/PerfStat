using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NvgSharp;

namespace PerfStat.Samples.Demo
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game
	{
		GraphicsDeviceManager _graphics;

		private SpriteBatch _spriteBatch;
		private Demo _demo;
		private NvgContext _context;
		private readonly PerfGraphWidget _perfGraph = new PerfGraphWidget();

		public Game1()
		{
			_graphics = new GraphicsDeviceManager(this)
			{
				PreferredBackBufferWidth = 1200,
				PreferredBackBufferHeight = 800,
				PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8
			};

			Content.RootDirectory = "Content";
			IsMouseVisible = true;
			Window.AllowUserResizing = true;
			IsFixedTimeStep = false;
			_graphics.SynchronizeWithVerticalRetrace = false;
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			_context = new NvgContext(GraphicsDevice);

			_demo = new Demo(_context);

			_spriteBatch = new SpriteBatch(GraphicsDevice);
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// game-specific content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		protected override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			_perfGraph.Update(gameTime.ElapsedGameTime.TotalSeconds);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(new Color(0.3f, 0.3f, 0.3f));

			if (_graphics.PreferredBackBufferWidth != Window.ClientBounds.Width ||
				_graphics.PreferredBackBufferHeight != Window.ClientBounds.Height)
			{
				_graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
				_graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
				_graphics.ApplyChanges();
			}

			// TODO: Add your drawing code here

			var mouseState = Mouse.GetState();

			_context.BeginFrame(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight, 1.0f);

			var t = (float)gameTime.TotalGameTime.TotalSeconds;
			_demo.renderDemo(_context, 
				mouseState.X, 
				mouseState.Y, 
				_graphics.PreferredBackBufferWidth, 
				_graphics.PreferredBackBufferHeight, 
				t, 
				false);

			_perfGraph.Render(_context, 5, 5);

			_context.EndFrame();

			base.Draw(gameTime);
		}
	}
}