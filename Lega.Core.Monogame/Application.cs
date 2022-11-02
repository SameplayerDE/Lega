using Lega.Core.Monogame.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Lega.Core.Monogame.Input;

namespace Lega.Core.Monogame
{
	public abstract class Application : Game
	{

		protected SpriteBatch SpriteBatch;
		protected ApplicationWindow ApplicationWindow;

		public bool IsFullScreen
		{
			get => ApplicationWindow.IsFullScreen;
			set => ApplicationWindow.IsFullScreen = value;
		}

		protected Application(string title = "", int width = 640, int height = 480, string contentPath = "Content")
		{
			ApplicationWindow = new ApplicationWindow(this, width, height);
			ApplicationWindow.Title = title;
			Content.RootDirectory = contentPath;

			IsFixedTimeStep = true;
			MaxElapsedTime = TimeSpan.FromSeconds(1);
			TargetElapsedTime = TimeSpan.FromSeconds(1d / 60d);

		}

		protected override void Initialize()
		{
			ApplicationWindow.Initialize();
			base.Initialize();
		}

		protected override void LoadContent()
		{
			SpriteBatch = new SpriteBatch(GraphicsDevice);
			base.LoadContent();
		}

		protected override void Update(GameTime gameTime)
		{
			SystemKeyboard.Update(gameTime);
			base.Update(gameTime);
		}

		public bool ToggleFullScreen()
		{
			ApplicationWindow.IsFullScreen = !ApplicationWindow.IsFullScreen;
			return ApplicationWindow.IsFullScreen;
		}

		public void SetFrameRate(int rate)
		{
			TargetElapsedTime = TimeSpan.FromSeconds(1d / rate);
		}
	}
}
