using Cosmos.Core;
using MOOS.Misc;
using Waterfall.System.Boot;

namespace MOOS
{
	public unsafe class WavPlayer
	{
		static byte[] _pcm;
		static int _index;
		static WAV.Header _header;
		public static WavPlayer _player;
		public static string _song_name;

		public static bool playing;
		public WavPlayer()
		{
			_pcm = null;
			_index = 0;
			INTs.SetIntHandler(0x20, DoPlay);
			_player = this;
			_song_name = null;
			playing = false;
		}
		public void Play(byte[] wav, string name = "unknown")
		{
			_index = 0;
			WAV.Decode(wav, out var pcm, out var hdr);
			_pcm = pcm;
			_header = hdr;
			_song_name = name;

			playing = true;
			var context = new INTs.IRQContext();
			INTs.HandleInterrupt_20(ref context);
		}

		public static void DoPlay(ref INTs.IRQContext context)
		{
			WaterfallBoot.CLILogs.WriteInfo("DoPlay called");
			if (Audio.bytesWritten != 0) return;

			if (_index + Audio.CacheSize > _pcm.Length) _index = 0;

			fixed (byte* buffer = _pcm)
			{
				_index += Audio.CacheSize;
				Audio.snd_write(buffer + _index, Audio.CacheSize);
			}
		}
	}
}