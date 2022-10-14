using System;
using OpenRA.Primitives;

namespace OpenRA.Platforms.Headless
{
	sealed class DummyPlatformWindow : IPlatformWindow
	{
		public bool IsNullRenderer => true;

		public IGraphicsContext Context => throw new NotImplementedException();

		public Size NativeWindowSize => throw new NotImplementedException();

		public Size EffectiveWindowSize => throw new NotImplementedException();

		public float NativeWindowScale => throw new NotImplementedException();

		public float EffectiveWindowScale => throw new NotImplementedException();

		public Size SurfaceSize => throw new NotImplementedException();

		public int DisplayCount => throw new NotImplementedException();

		public int CurrentDisplay => throw new NotImplementedException();

		public bool HasInputFocus => throw new NotImplementedException();

		public bool IsSuspended => throw new NotImplementedException();

		public GLProfile GLProfile => throw new NotImplementedException();

		public GLProfile[] SupportedGLProfiles => throw new NotImplementedException();

		public event Action<float, float, float, float> OnWindowScaleChanged;

		public IHardwareCursor CreateHardwareCursor(string name, Size size, byte[] data, int2 hotspot, bool pixelDouble)
		{
			return null;
		}

		public void Dispose() { }

		public string GetClipboardText()
		{
			return null;
		}

		public void GrabWindowMouseFocus() { }

		public void PumpInput(IInputHandler inputHandler)
		{
			/* TODO: This should probably be implemented, a queue of some sort */
		}

		public void ReleaseWindowMouseFocus() { }

		public bool SetClipboardText(string text) { return false; }

		public void SetHardwareCursor(IHardwareCursor cursor) { }

		public void SetRelativeMouseMode(bool mode) { }

		public void SetScaleModifier(float scale) { }

		public void SetWindowTitle(string title)
		{
			Console.Title = title;
		}
	}
}
