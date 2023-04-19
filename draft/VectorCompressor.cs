class VectorCompressor
{
	/**
	 *
	 * X / Y / Z Meta verilerini barındırır.
	 *
	 * @author Ismail <ismaiil_0234@hotmail.com>
	 *
	 */
	public enum Metadata
	{
		None = 0x0000000,
		X    = 0x0000001,
		Y    = 0x0000002,
		Z    = 0x0000004
	}

	/**
	 *
	 * Büyük sayıyı barındırır.
	 *
	 * @author Ismail <ismaiil_0234@hotmail.com>
	 *
	 */
	private static long BigNumber = 1000000L * 1000000L * 1000000L;

	/**
	 *
	 * Vector3'ü 12 bayttan 8 bayta sıkıştırır.
	 *
	 * @author Ismail <ismaiil_0234@hotmail.com>
	 *
	 */
	public static long Compress(float x, float y, float z)
	{
		var qData = Metadata.None;
		if (x < 0)
		{
			qData |= Metadata.X;
		}
		
		if (y < 0)
		{
			qData |= Metadata.Y;
		}
		
		if (z < 0)
		{
			qData |= Metadata.Z;
		}
		
		var xData = (long)(Math.Abs(x) * 100);
		var yData = (long)(Math.Abs(y) * 100) * 1000000;
		var zData = (long)(Math.Abs(z) * 100) * 1000000 * 1000000;
		
		return (1000000000000000000 * (long) qData) + (xData + yData + zData);
	}
	
	/**
	 *
	 * Sıkıştırılmış Vector3'ü normal haline getirir.
	 *
	 * @author Ismail <ismaiil_0234@hotmail.com>
	 *
	 */
	public static Vector3 Decompress(long longNumber)
	{		
		var flag = (byte) (longNumber / BigNumber);
		longNumber -= BigNumber * flag;

		var zData = longNumber / (1000000L * 1000000L);
		longNumber -= (1000000L * 1000000L) * zData;

		var yData = longNumber / (1000000L);
		longNumber -= (1000000L) * yData;

		if ((flag & 0x0000001) == 0x0000001)
		{
			longNumber *= -1;
		}

		if ((flag & 0x0000002) == 0x0000002)
		{
			yData *= -1;
		}

		if ((flag & 0x0000004) == 0x0000004)
		{
			zData *= -1;
		}
		
		return new Vector3(longNumber / 100f, yData / 100f, zData / 100f);
	}
}

/*
Performance Test
-----------------------------------------------------
One Million Compression: 10-15 ms
One Million Decompression: 45-50 ms

Old Size: 12 + 3 byte
New Size: 8 + 1 byte

Compression Rate: 40%

Min/Max Values (X / Y / Z)
-----------------------------------------------------
Min Values: -9999.99f / -9999.99f / -9999.99f
Max Values: 9999.99f / 9999.99f / 9999.99f
*/
