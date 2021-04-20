//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#if UNITY_EDITOR
#pragma warning disable

using System;
using System.Collections.Generic;
using System.Text;

namespace AssetIcons.Editors.Internal.Expresser
{
	/// <summary>
	/// <para>A mathematical expression in a compiled format.</para>
	/// </summary>
	internal class ExpressionSyntax
	{
		private readonly string expression;
		private readonly string[] terms;
		private readonly ExpressionToken[] tokens;

		private enum SpanClassifier : byte
		{
			None,
			Numeric,
			Operator,
			String,
			Space,
			Structure
		}

		private struct CharacterDescriptor
		{
			public char Character;
			public short Index;
			private readonly string sourceString;

			public bool IsOperator
			{
				get
				{
					return Character == '+' ||
						Character == '-' ||
						Character == '*' ||
						Character == '/' ||
						Character == '^' ||
						Character == '&' ||
						Character == '%' ||
						Character == '|' ||
						Character == '=' ||
						Character == '!' ||
						Character == '>' ||
						Character == '<';
				}
			}

			public bool IsStructure
			{
				get
				{
					return Character == '(' ||
					   Character == ')' ||
					   Character == ',';
				}
			}

			public SyntaxTokenKind SelfTokenKind
			{
				get
				{
					switch (Character)
					{
						case '+':
							return SyntaxTokenKind.Plus;
						case '-':
							return SyntaxTokenKind.Minus;
						case '*':
							return SyntaxTokenKind.Multiply;
						case '/':
							return SyntaxTokenKind.Divide;
						case '^':
							return SyntaxTokenKind.Power;
						case '%':
							return SyntaxTokenKind.Percentage;
						case '(':
							return SyntaxTokenKind.OpenParentheses;
						case ')':
							return SyntaxTokenKind.CloseParentheses;
						case ',':
							return SyntaxTokenKind.Comma;
						case '>':
							return SyntaxTokenKind.GreaterThan;
						case '<':
							return SyntaxTokenKind.LessThan;
						case '!':
							return SyntaxTokenKind.Not;
						default:
							return SyntaxTokenKind.None;
					}
				}
			}

			public SpanClassifier Type
			{
				get
				{
					if (Index < 0 || char.IsWhiteSpace(Character))
					{
						return SpanClassifier.Space;
					}
					else if (char.IsDigit(Character) || Character == '.')
					{
						return SpanClassifier.Numeric;
					}
					else if (char.IsLetter(Character))
					{
						return SpanClassifier.String;
					}
					else if (IsStructure)
					{
						return SpanClassifier.Structure;
					}
					else if (IsOperator)
					{
						return SpanClassifier.Operator;
					}
					else
					{
						throw new InvalidOperationException(string.Format("Character \"{0}\" could not be classified", Character));
					}
				}
			}

			public CharacterDescriptor(string sourceString, short index)
			{
				this.sourceString = sourceString;
				Index = index;
				if (Index >= 0)
				{
					if (index >= sourceString.Length)
					{
						Index = -2;
						Character = new char();
					}
					else
					{
						Character = sourceString[index];
					}
				}
				else
				{
					Index = -1;
					Character = new char();
				}
			}

			public static CharacterDescriptor End(string sourceString)
			{
				return new CharacterDescriptor(sourceString, -2);
			}

			public static CharacterDescriptor Start(string sourceString)
			{
				return new CharacterDescriptor(sourceString, -1);
			}

			public CharacterDescriptor Next()
			{
				if (Index == -2)
				{
					return this;
				}

				return new CharacterDescriptor(sourceString, (short)(Index + 1));
			}

			public override string ToString()
			{
				if (Index == -1)
				{
					return "Start";
				}

				if (Index == -2)
				{
					return "End";
				}

				return string.Format("\"{0}\" at {1}", Character, Index);
			}
		}

		/// <summary>
		/// <para></para>
		/// </summary>
		public string Expression
		{
			get
			{
				return expression;
			}
		}

		public string[] Terms
		{
			get
			{
				return terms;
			}
		}

		public ExpressionToken[] Tokens
		{
			get
			{
				return tokens;
			}
		}

		/// <summary>
		/// <para></para>
		/// </summary>
		/// <param name="expression"></param>
		public ExpressionSyntax(string expression)
		{
			this.expression = expression;

			ExpressionToken[] tokens;
			string[] terms;

			ParseText(expression, out tokens, out terms);

			this.tokens = tokens;
			this.terms = terms;
		}

		public override string ToString()
		{
			var sb = new StringBuilder();

			var lastToken = Tokens[0];
			for (int i = 1; i < Tokens.Length; i++)
			{
				var token = Tokens[i];
				sb.Append(lastToken);
				if (lastToken.Operation != SyntaxTokenKind.OpenParentheses
					&& token.Operation != SyntaxTokenKind.CloseParentheses
					&& token.Operation != SyntaxTokenKind.Percentage
					&& token.Operation != SyntaxTokenKind.Comma
					&& lastToken.Operation != SyntaxTokenKind.Not)
				{
					sb.Append(' ');
				}
				lastToken = token;
			}
			sb.Append(Tokens[Tokens.Length - 1]);
			return sb.ToString();
		}

		private static void ParseText(string expression, out ExpressionToken[] tokens, out string[] sources)
		{
			var foundTokens = new List<ExpressionToken>();
			var foundSources = new List<string>();

			int currentSpanStart = 0;
			int currentSpanLength = 0;
			var currentSpanClass = SpanClassifier.None;
			var characterClass = SpanClassifier.None;

			var lastToken = ExpressionToken.None;

			var headCharacter = CharacterDescriptor.Start(expression);
			var lastCharacter = headCharacter;

			while (true)
			{
				headCharacter = headCharacter.Next();

				if (headCharacter.Index == -2 && lastCharacter.Index == -2)
				{
					break;
				}

				characterClass = headCharacter.Type;

				if (headCharacter.Character == '-' && (lastToken.IsOperator || lastToken.Operation == SyntaxTokenKind.None))
				{
					characterClass = SpanClassifier.Numeric;
				}

				if (characterClass != currentSpanClass || characterClass == SpanClassifier.Structure)
				{
					if (currentSpanClass == SpanClassifier.None && characterClass != SpanClassifier.Structure
						|| currentSpanClass == SpanClassifier.Numeric
						&& currentSpanLength == 1
						&& lastCharacter.Character == '-')
					{
						currentSpanClass = characterClass;
					}
					else
					{
						switch (currentSpanClass)
						{
							case SpanClassifier.String:

								bool isNegative = expression[currentSpanStart] == '-';
								int offset = isNegative ? 1 : 0;
#if USE_SPANS
								var spanContent = expression.AsSpan().Slice(currentSpanStart, currentSpanLength));
#else
								string spanContent = expression.Substring(currentSpanStart + offset, currentSpanLength - offset);
#endif

								if (spanContent.Equals("true", StringComparison.OrdinalIgnoreCase))
								{
									lastToken = ExpressionToken.StaticValue(new MathValue(true));
									foundTokens.Add(lastToken);
								}
								else if (spanContent.Equals("false", StringComparison.OrdinalIgnoreCase))
								{
									lastToken = ExpressionToken.StaticValue(new MathValue(false));
									foundTokens.Add(lastToken);
								}
								else
								{
									int sourceIndex = foundSources.IndexOf(spanContent);
									if (sourceIndex == -1)
									{
										sourceIndex = foundSources.Count;
										foundSources.Add(spanContent);
									}

									lastToken = ExpressionToken.ReadSource((byte)sourceIndex, isNegative);
									foundTokens.Add(lastToken);
								}
								break;

							case SpanClassifier.Numeric:
#if USE_SPANS
								spanContent = expression.AsSpan().Slice(currentSpanStart, currentSpanLength));
#else
								spanContent = expression.Substring(currentSpanStart, currentSpanLength);
#endif

								float numericFloat = float.Parse(spanContent);

								lastToken = ExpressionToken.StaticValue(new MathValue(numericFloat, false));
								foundTokens.Add(lastToken);

								break;

							case SpanClassifier.Operator:
							case SpanClassifier.Structure:
								if (currentSpanLength == 1)
								{
									lastToken = ExpressionToken.Operator(lastCharacter.SelfTokenKind);
								}
								else
								{
									spanContent = expression.Substring(currentSpanStart, currentSpanLength);

									SyntaxTokenKind operation;
									switch (spanContent)
									{
										case "==":
											operation = SyntaxTokenKind.Equal;
											break;
										case "!=":
											operation = SyntaxTokenKind.NotEqual;
											break;
										case ">=":
											operation = SyntaxTokenKind.GreaterThanOrEqual;
											break;
										case "<=":
											operation = SyntaxTokenKind.LessThanOrEqual;
											break;
										case "&&":
											operation = SyntaxTokenKind.And;
											break;
										case "||":
											operation = SyntaxTokenKind.Or;
											break;
										default:
											operation = SyntaxTokenKind.None;
											break;
									}
									lastToken = ExpressionToken.Operator(operation);
								}
								if (lastToken.Operation == SyntaxTokenKind.None)
								{
									throw new InvalidOperationException(string.Format("Unrecognised Operator Sequence \"{0}\"", expression.Substring(currentSpanStart, currentSpanLength)));
								}
								foundTokens.Add(lastToken);
								break;
						}

						currentSpanStart = headCharacter.Index;
						currentSpanLength = 0;
						currentSpanClass = characterClass;
					}
				}

				currentSpanLength++;

				lastCharacter = headCharacter;
			}

			tokens = foundTokens.ToArray();
			sources = foundSources.ToArray();
		}
	}
}

#pragma warning restore
#endif
