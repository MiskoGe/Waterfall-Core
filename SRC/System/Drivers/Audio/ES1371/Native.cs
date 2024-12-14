using System;

public class Native
{

	public static unsafe void Stosb(byte* p, byte value, ulong count)
	{
		if (count == 0) return;

		byte* block = stackalloc byte[8];
		for (int i = 0; i < 8; i++)
		{
			block[i] = value;
		}

		ulong blockSize = 8;
		ulong fullBlocks = count / blockSize;
		ulong remainingBytes = count % blockSize;

		for (ulong i = 0; i < fullBlocks; i++)
		{
			Buffer.MemoryCopy(block, p + (i * blockSize), blockSize, blockSize);
		}

		for (ulong i = 0; i < remainingBytes; i++)
		{
			p[(fullBlocks * blockSize) + i] = value;
		}
	}
	public static unsafe void Movsb(byte* source, byte* destination, int length)
	{
		byte* ps = source;
		byte* pd = destination;

		for (int i = 0; i < length; i++)
		{
			*pd++ = *ps++;
		}
	}

}