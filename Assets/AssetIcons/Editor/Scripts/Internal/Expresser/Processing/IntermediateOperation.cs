//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#if UNITY_EDITOR
#pragma warning disable

using System.Linq;

namespace AssetIcons.Editors.Internal.Expresser.Processing
{
	/// <summary>
	/// <para>An operation within an <see cref="IntermediateExpression"/></para>
	/// </summary>
	internal struct IntermediateOperation
	{
		/// <summary>
		/// <para>A location to store the output of this operation.</para>
		/// </summary>
		public byte DistIndex;

		/// <summary>
		/// <para>A code representing the type of operation this represents.</para>
		/// </summary>
		public IntermediateOperationCode OperationCode;

		/// <summary>
		/// <para>An array of parameters used when invoking this operation.</para>
		/// </summary>
		public IntermediateParameter[] Parameters;

		/// <summary>
		/// <para>Constructs a new instance of the <see cref="IntermediateOperation"/> struct.</para>
		/// </summary>
		/// <param name="distIndex">A location to store the output of this operation.</param>
		/// <param name="operationCode">A code representing the type of operation this represents.</param>
		/// <param name="parameters">An array of parameters used when invoking this operation.</param>
		public IntermediateOperation(byte distIndex, IntermediateOperationCode operationCode, IntermediateParameter[] parameters)
		{
			DistIndex = distIndex;
			OperationCode = operationCode;
			Parameters = parameters;
		}

		/// <summary>
		/// <para>Converts this <see cref="IntermediateOperation"/> into a string representation.</para>
		/// </summary>
		/// <returns>
		/// <para>A string representation of this <see cref="IntermediateOperation"/>.</para>
		/// </returns>
		public override string ToString()
		{
			return string.Format("Output[{0}] = {1}: {2}", DistIndex, OperationCode, string.Join(", ", Parameters.Select(p => p.ToString()).ToArray()));
		}
	}
}

#pragma warning restore
#endif
