namespace Celeritas
{
	/// <summary>
	/// Extensions for the string type.
	/// </summary>
	public static class StringExtensions
	{
		/// <summary>
		/// Returns the remainder of a string after the last specified character.
		/// </summary>
		/// <param name="source">The target string.</param>
		/// <param name="delimiter">The delimiter that marks where the string should be split.</param>
		/// <returns>Returns a string that has the start cutoff up to the delimiter.</returns>
		public static string AfterCharacter(this string source, char delimiter)
		{
			if (!source.Contains(delimiter.ToString())) return source;

			int index = source.LastIndexOf(delimiter);
			return source.Substring(index + 1);
		}

		/// <summary>
		/// Returns the remainder of a string before the first specified character.
		/// </summary>
		/// <param name="source">The target string.</param>
		/// <param name="delimiter">The delimiter that marks where the string should be split.</param>
		/// <returns>Returns a string up to the delimiter.</returns>
		public static string BeforeCharacter(this string source, char delimiter)
		{
			if (!source.Contains(delimiter.ToString())) return source;

			int index = source.IndexOf(delimiter);
			return source.Substring(0, index);
		}

		/// <summary>
		/// Formats the given string to use Unity's rules of string assembly for editor display.
		/// </summary>
		/// <param name="source">The source string.</param>
		/// <returns>The display version of the string.</returns>
		public static string AsDisplayString(this string source)
		{
			if (source.Length == 0)
				return source;

			string result = source[0].ToString();
			for (int i = 1; i < source.Length; i++)
			{
				if (char.IsUpper(source[i]))
					result += " ";
				result += source[i].ToString();
			}

			return result;
		}
	}
}
