
using System;
using Celeritas.Scriptables;

namespace Celeritas.Extensions
{
	/// <summary>
	/// Extention for any multidimensional array types.
	/// </summary>
	public static class MultidimentionalArrayExtentions
	{
		/// <summary>
		/// provides x and y iterator values for bool 2D arrays.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="run">call back lambda function</param>
		public static void ForEach(this bool[,] source, Action<int, int> run)
		{
			if (source != null)
			{
				for (int x = 0; x < source.GetLength(0); x++)
				{
					for (int y = 0; y < source.GetLength(1); y++)
					{
						run(x, y);
					}
				}
			}
		}

				/// <summary>
		/// provides x and y iterator values for HullData 2D arrays.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="run">call back lambda function</param>
		public static void ForEach(this ModuleData[,] source, Action<int, int> run)
		{
			if (source != null)
			{
				for (int x = 0; x < source.GetLength(0); x++)
				{
					for (int y = 0; y < source.GetLength(1); y++)
					{
						run(x, y);
					}
				}
			}
		}
	}
}