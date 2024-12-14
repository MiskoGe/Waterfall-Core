using System.Collections.Generic;
using Waterfall.System.Managment;

namespace Waterfall.System.Core.SystemInput.Keys
{
	public static class KeyHandler
	{
		public static Queue<KeyInfo> KeysToHandle = new Queue<KeyInfo>();
		public static bool KeyAvailable;
		public static bool CapsLock;
		public static bool Shift;
		public static bool Control;
		public static bool Alt;
		private static bool alreadyPressed;
		static ulong NextAcceleration;
		public static bool Accelerate = false;
		static KeyInfo LastKey;
		static int Phase = 0;
		public static void KeyboardAcceleration()
		{
			if (!Accelerate || LastKey == null)
			{
				Phase = 0;
				return;
			}

			if (RealTime.CPUUptime >= NextAcceleration)
			{
				if (Phase < 17)
				{
					Phase++;
					NextAcceleration = RealTime.CPUUptime + 133333333;
					return;
				}
				NextAcceleration = RealTime.CPUUptime + 133333333;
				AddKey(LastKey);
			}

		}
		public static void KeyFromPS2(byte key, bool released)
		{
			KeyInfo info = new KeyInfo();
			info.Key = (KeyboardKey)key - 1;
			switch (info.Key)
			{
				case KeyboardKey.LeftControl:
					Control = !released;
					LastKey = null;
					break;
				case KeyboardKey.LeftShift:
					Shift = !released;
					LastKey = null;
					break;
				case KeyboardKey.RightShift:
					Shift = !released;
					LastKey = null;
					break;
				case KeyboardKey.LeftAlt:
					Alt = !released;
					LastKey = null;
					break;
				case KeyboardKey.CapsLock:
					if (!released && !alreadyPressed)
					{
						CapsLock = !CapsLock;
					}
					if (released)
					{
						alreadyPressed = false;
					}
					LastKey = null;
					break;
				default:
					{
						if (!released)
						{
							if (Phase <= 3 || LastKey.Key != info.Key)
							{
								LastKey = info;
								Accelerate = true;
								info.Shift = Shift;
								info.Control = Control;
								info.Alt = Alt;
								info.CapsLock = CapsLock;
								AddKey(info);
								if (Phase == 0)
									Phase++;
							}
						}
						else
						{
							Accelerate = false;
							Phase = 0;
						}
					}
					break;
			}
		}
		public static void AddKey(KeyInfo key)
		{
			KeysToHandle.Enqueue(key);
			KeyAvailable = true;
		}
		public static KeyInfo ReadKey()
		{
			if (KeysToHandle.Count <= 1)
				KeyAvailable = false;
			return KeysToHandle.Dequeue();
		}
	}
	public class KeyInfo
	{
		public KeyboardKey Key;
		public bool Shift, Alt, Control, CapsLock;
	}
}
