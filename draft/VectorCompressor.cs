class VectorCompressor
{
	public enum Metadata
	{
		None = 0x0000000,
		X    = 0x0000001,
		Y    = 0x0000002,
		Z    = 0x0000004
	}
	
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
}
