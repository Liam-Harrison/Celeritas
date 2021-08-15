
using UnityEngine;

namespace Celeritas
{
	/// <summary>
	/// Extention for the sprite class.
	/// </summary>
	public static class SpriteExtentions
	{
        /// <summary>
        /// Converts a Sprite to a Texture2D.
        /// </summary>
        /// <param name="sprite">target sprite</param>
        /// <returns>A Texture2D based off of the given sprite</returns>
		public static Texture2D ToTexture2D(this Sprite sprite)
		{
			if (sprite.rect.width != sprite.texture.width)
			{
				Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height, TextureFormat.ARGB32, false);
				Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x,
															 (int)sprite.textureRect.y,
															 (int)sprite.textureRect.width,
															 (int)sprite.textureRect.height);
				newText.SetPixels(newColors);
				newText.filterMode = FilterMode.Point;
				newText.Apply();
				return newText;
			}
			else
			{
				return sprite.texture;
			}
		}
	}
}

