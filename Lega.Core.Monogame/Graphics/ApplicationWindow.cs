using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
namespace Lega.Core.Monogame.Graphics
{
	public class ApplicationWindow
	{

		private int _width;
		private int _height;

		public Application Application { get; protected set; }
		public GraphicsDeviceManager GraphicsDeviceManager { get; protected set; }

		public string Title { get; set; }

		public bool IsFullScreen
		{
			get => GraphicsDeviceManager.IsFullScreen;
			set
			{
				GraphicsDeviceManager.IsFullScreen = value;
				GraphicsDeviceManager.ApplyChanges();
			}
		}

		public ApplicationWindow(Application application, int width, int height)
		{
			_width = width;
			_height = height;
			Application = application;
			GraphicsDeviceManager = new GraphicsDeviceManager(application);
		}

		public virtual void Initialize()
		{

			GraphicsDeviceManager.PreferredBackBufferWidth = _width;
			GraphicsDeviceManager.PreferredBackBufferHeight = _height;
			GraphicsDeviceManager.PreferredBackBufferFormat = SurfaceFormat.Color;
			GraphicsDeviceManager.PreferredDepthStencilFormat = DepthFormat.Depth24; // <-- set depth here

			GraphicsDeviceManager.HardwareModeSwitch = false;
			GraphicsDeviceManager.PreferMultiSampling = false;
			GraphicsDeviceManager.IsFullScreen = false;
			GraphicsDeviceManager.ApplyChanges();

			Application.Window.Title = Title;
		}
	}
}
