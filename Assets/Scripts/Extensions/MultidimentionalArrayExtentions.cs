using System;
using System.Collections.Generic;
using Celeritas.Scriptables;

namespace Celeritas.Extensions
{
	/// <summary>
	/// Extention for any multidimensional array types.
	/// </summary>
	public static class MultidimentionalArrayExtentions
	{
		private readonly static Dictionary<TetrisShape, bool[,]> ModuleShapes = new Dictionary<TetrisShape, bool[,]>
		{
			{
				TetrisShape.Single, new bool[,]
			{
				{ true, false, false, },
				{ false, false, false, },
				{ false, false, false} }
			},

			{
				TetrisShape.SmallL, new bool[,]
			{
				{ true, true, false, },
				{ true, false, false, },
				{ false, false, false} }
			},

			{
				TetrisShape.LargeL, new bool[,]
			{
				{ true, true, false, },
				{ true, false, false, },
				{ true, false, false} }
			},

			{
				TetrisShape.SmallLine, new bool[,]
			{
				{ false, false, false, },
				{ true, false, false, },
				{ true, false, false} }
			},

			{
				TetrisShape.LargeLine, new bool[,]
			{
				{ true, false, false, },
				{ true, false, false, },
				{ true, false, false} }
			},

			{
				TetrisShape.SmallSquare, new bool[,]
			{
				{ true, true, false, },
				{ true, true, false, },
				{ false, false, false} }
			},

			{
				TetrisShape.LargeSquare, new bool[,]
			{
				{ true, true, true, },
				{ true, true, true, },
				{ true, true, true} }
			},

			{
				TetrisShape.T, new bool[,]
			{
				{ true, true, true, },
				{ false, true, false },
				{ false, false, false} }
			},

			{
				TetrisShape.None, new bool[,]
			{
				{ false, false, false, },
				{ false, false, false, },
				{ false, false, false} }
			},
		};

		public static bool[,] ModuleShape(this TetrisShape shape)
		{
			return ModuleShapes[shape];
		}

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