//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#if UNITY_EDITOR
#pragma warning disable

using AssetIcons.Editors.Internal.Drawing;
using AssetIcons.Editors.Internal.Expresser;
using AssetIcons.Editors.Internal.Expresser.Input;
using UnityEngine;

namespace AssetIcons.Editors
{
	/// <summary>
	/// <para>A formatted representation of the <see cref="AssetIconsStyle"/> for drawing graphics in the <see cref="AssetIconsGUI"/>.</para>
	/// </summary>
	/// <seealso cref="AssetIconsStyle"/>
	/// <seealso cref="AssetIconsGUI"/>
	public sealed class AssetIconsCompiledStyle
	{
		private static readonly AssetIconsCompiledStyle defaultStyle = new AssetIconsCompiledStyle(new AssetIconsStyle());

		/// <summary>
		/// <para>A <see cref="AssetIconsCompiledStyle"/> for the default constructor of <see cref="AssetIconsStyle"/>.</para>
		/// </summary>
		/// <seealso cref="AssetIconsStyle"/>
		public static AssetIconsCompiledStyle Default
		{
			get
			{
				return defaultStyle;
			}
		}

		private class PositioningProcessor
		{
			private float lastWidth;
			private float lastHeight;

			private float cachedWidth;
			private float cachedHeight;
			private float cachedX;
			private float cachedY;
			private bool cachedDisplay;

			private readonly CompiledExpression compiledWidth;
			private readonly CompiledExpression compiledHeight;
			private readonly CompiledExpression compiledX;
			private readonly CompiledExpression compiledY;
			private readonly CompiledExpression compiledDisplay;

			private readonly StaticValueProvider widthProvider;
			private readonly StaticValueProvider heightProvider;

			private readonly Vector2 styleAnchoring;

			public PositioningProcessor(AssetIconsCompiledStyle compiledStyleDefinition, AssetIconsStyle style)
			{
				widthProvider = new StaticValueProvider(new MathValue(1.0f, true));
				heightProvider = new StaticValueProvider(new MathValue(1.0f, true));

				var horizontalContext = new MathContextBuilder()
					.WithTerm("width", widthProvider)
					.WithTerm("height", heightProvider)
					.ImplicitlyReferences(widthProvider)
					.Build();

				var verticalContext = new MathContextBuilder()
					.WithTerm("width", widthProvider)
					.WithTerm("height", heightProvider)
					.ImplicitlyReferences(heightProvider)
					.Build();

				compiledWidth = CompiledExpression.Compile(style.Width, horizontalContext);
				compiledHeight = CompiledExpression.Compile(style.Height, verticalContext);
				compiledX = CompiledExpression.Compile(style.X, horizontalContext);
				compiledY = CompiledExpression.Compile(style.Y, verticalContext);
				compiledDisplay = CompiledExpression.Compile(style.Display, horizontalContext);

				styleAnchoring = compiledStyleDefinition.Anchor;
			}

			/// <summary>
			/// <para>Applies this <see cref="PositioningProcessor"/> to a <see cref="Rect"/>.</para>
			/// </summary>
			/// <param name="rect">A source <see cref="Rect"/> to apply position modifications to.</param>
			/// <param name="display">A Boolean value indicating whether the <see cref="Rect"/> should be displayed.</param>
			/// <returns>
			/// <para>A rect that has been modified by this <see cref="PositioningProcessor"/>.</para>
			/// </returns>
			public Rect Filter(Rect rect, out bool display)
			{
				if (Mathf.Abs(rect.width - lastWidth) > 0.01f
					|| Mathf.Abs(rect.height - lastHeight) > 0.01f)
				{
					lastWidth = rect.width;
					lastHeight = rect.height;

					widthProvider.Value = new MathValue(lastWidth, false);
					heightProvider.Value = new MathValue(lastHeight, false);

					if (compiledDisplay != null)
					{
						var newDisplayValue = compiledDisplay.Evaluate();
						if (newDisplayValue.ValueClass == ValueClassifier.Boolean)
						{
							cachedDisplay = newDisplayValue.BoolValue;

							if (!cachedDisplay)
							{
								display = false;
								return rect;
							}
						}
					}
					else
					{
						cachedDisplay = true;
					}

					if (compiledX != null)
					{
						var xOffsetValue = compiledX.Evaluate();
						if (xOffsetValue.ValueClass == ValueClassifier.Float)
						{
							cachedX = xOffsetValue.FloatValue;
						}
						else if (xOffsetValue.ValueClass == ValueClassifier.FloatFractional)
						{
							cachedX = xOffsetValue.FloatValue * rect.width;
						}
					}

					if (compiledY != null)
					{
						var yOffsetValue = compiledY.Evaluate();
						if (yOffsetValue.ValueClass == ValueClassifier.Float)
						{
							cachedY = yOffsetValue.FloatValue;
						}
						else if (yOffsetValue.ValueClass == ValueClassifier.FloatFractional)
						{
							cachedY = yOffsetValue.FloatValue * rect.height;
						}
					}

					if (compiledWidth != null)
					{
						var newWidthValue = compiledWidth.Evaluate();
						if (newWidthValue.ValueClass == ValueClassifier.Float)
						{
							cachedWidth = newWidthValue.FloatValue;
						}
						else if (newWidthValue.ValueClass == ValueClassifier.FloatFractional)
						{
							cachedWidth = newWidthValue.FloatValue * rect.width;
						}
					}
					else
					{
						cachedWidth = rect.width;
					}

					if (compiledHeight != null)
					{
						var newHeightValue = compiledHeight.Evaluate();
						if (newHeightValue.ValueClass == ValueClassifier.Float)
						{
							cachedHeight = newHeightValue.FloatValue;
						}
						else if (newHeightValue.ValueClass == ValueClassifier.FloatFractional)
						{
							cachedHeight = newHeightValue.FloatValue * rect.height;
						}
					}
					else
					{
						cachedHeight = rect.height;
					}
				}

				if (!cachedDisplay)
				{
					display = false;
					return rect;
				}

				rect.x += cachedX + ((rect.width - cachedWidth) * styleAnchoring.x);
				rect.y += (-cachedY) + ((rect.height - cachedHeight) * (1.0f - styleAnchoring.y));
				rect.width = cachedWidth;
				rect.height = cachedHeight;

				display = true;
				return rect;
			}
		}

		/// <summary>
		/// <para>A value used to determine the max size of the icon.</para>
		/// </summary>
		public int MaxSize { get; set; }

		/// <summary>
		/// <para>A value used to determine the aspect of the icon.</para>
		/// </summary>
		public IconAspect Aspect { get; set; }

		/// <summary>
		/// <para>An anchor that all difference in scale is orientated around.</para>
		/// </summary>
		public Vector2 Anchor { get; set; }

		/// <summary>
		/// <para>A tint to apply to the icon.</para>
		/// </summary>
		public Color Tint { get; set; }

		/// <summary>
		/// <para>A value used to determine the layer of the icon.</para>
		/// </summary>
		public int Layer { get; set; }

		/// <summary>
		/// <para>A font style to use on all rendered text.</para>
		/// </summary>
		public FontStyle FontStyle { get; set; }

		/// <summary>
		/// <para>An anchor for all rendered text.</para>
		/// </summary>
		public TextAnchor TextAnchor { get; set; }

		/// <summary>
		/// <para>A camera projection for all rendered Prefabs.</para>
		/// </summary>
		public IconProjection Projection { get; set; }

		/// <summary>
		/// <para>A compiled representation of the positioning expressions.</para>
		/// </summary>
		private PositioningProcessor Positioning { get; set; }

		/// <summary>
		/// <para>Constructs a new instance of the <see cref="AssetIconsCompiledStyle"/> from a <see cref="AssetIconsStyle"/>.</para>
		/// </summary>
		public AssetIconsCompiledStyle(AssetIconsStyle style)
		{
			MaxSize = style.MaxSize;
			Aspect = style.Aspect;
			Anchor = AnchorToVector(style.Anchor);
			Layer = style.Layer;
			Tint = ExpressionParser.ParseHexColor(style.Tint);
			FontStyle = style.FontStyle;
			TextAnchor = ToTextAnchor(style.TextAnchor);
			Projection = style.Projection;

			Positioning = new PositioningProcessor(this, style);
		}

		/// <summary>
		/// <para>Applies this <see cref="AssetIconsCompiledStyle"/> to a <see cref="Rect"/>.</para>
		/// </summary>
		/// <param name="rect">A source <see cref="Rect"/> to apply position modifications to.</param>
		/// <param name="display">A Boolean value indicating whether the <see cref="Rect"/> should be displayed.</param>
		/// <returns>
		/// <para>A <see cref="Rect"/> that has been modified by this <see cref="AssetIconsCompiledStyle"/>.</para>
		/// </returns>
		public Rect Filter(Rect rect, out bool display)
		{
			return Positioning.Filter(rect, out display);
		}

		private static TextAnchor ToTextAnchor(IconAnchor anchor)
		{
			switch (anchor)
			{
				default:
				case IconAnchor.Center:
					return TextAnchor.MiddleCenter;
				case IconAnchor.Top:
					return TextAnchor.UpperCenter;
				case IconAnchor.Bottom:
					return TextAnchor.LowerCenter;
				case IconAnchor.Left:
					return TextAnchor.MiddleLeft;
				case IconAnchor.Right:
					return TextAnchor.MiddleRight;
				case IconAnchor.TopLeft:
					return TextAnchor.UpperLeft;
				case IconAnchor.TopRight:
					return TextAnchor.UpperRight;
				case IconAnchor.BottomLeft:
					return TextAnchor.LowerLeft;
				case IconAnchor.BottomRight:
					return TextAnchor.LowerRight;
			}
		}

		private static Vector2 AnchorToVector(IconAnchor anchor)
		{
			switch (anchor)
			{
				default:
				case IconAnchor.Center:
					return new Vector2(0.5f, 0.5f);
				case IconAnchor.Top:
					return new Vector2(0.5f, 1.0f);
				case IconAnchor.Bottom:
					return new Vector2(0.5f, 0.0f);
				case IconAnchor.Left:
					return new Vector2(0.0f, 0.5f);
				case IconAnchor.Right:
					return new Vector2(1.0f, 0.5f);
				case IconAnchor.TopLeft:
					return new Vector2(0.0f, 1.0f);
				case IconAnchor.TopRight:
					return new Vector2(1.0f, 1.0f);
				case IconAnchor.BottomLeft:
					return new Vector2(0.0f, 0.0f);
				case IconAnchor.BottomRight:
					return new Vector2(1.0f, 0.0f);

			}
		}
	}
}

#pragma warning restore
#endif
