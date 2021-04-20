//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#if UNITY_EDITOR
#pragma warning disable

using System;
using System.Text;
using UnityEngine;

namespace AssetIcons.Editors.Internal.Product
{
	/// <summary>
	/// <para>Provides global access to standard information about AssetIcons.</para>
	/// </summary>
	/// <seealso cref="ProductInformation"/>
	internal static class EditorProductInformation
	{
		/// <summary>
		/// <para>A type of issue reported in the Issue Tracker.</para>
		/// </summary>
		public enum IssueType
		{
			/// <summary>
			/// <para>Represents no type of issue in the Issue Tracker.</para>
			/// </summary>
			None,
			/// <summary>
			/// <para>Represents a bug in the Issue Tracker.</para>
			/// </summary>
			Bug,
			/// <summary>
			/// <para>Represents a feature request in the Issue Tracker.</para>
			/// </summary>
			Feature,
			/// <summary>
			/// <para>Represents a question in the Issue Tracker.</para>
			/// </summary>
			Question
		}

		/// <summary>
		/// <para>Engages the user-flow for submitting an issue to the issue tracker.</para>
		/// </summary>
		/// <param name="issueType">The issue type for this issue.</param>
		/// <param name="title">The title of the issue.</param>
		/// <param name="body">The body of the issue.</param>
		public static void SubmitIssue(IssueType issueType = IssueType.None, string title = "", string body = "")
		{
			bool firstParamter = true;

			var urlBuilder = new StringBuilder();
			urlBuilder.Append("https://github.com/Fydar/AssetIcons/issues/new?");

			string issueTypeParameter = null;
			switch (issueType)
			{
				case IssueType.Bug:
					issueTypeParameter = "labels=%F0%9F%90%9B+bug&template=bug-report.md";
					break;

				case IssueType.Feature:
					issueTypeParameter = "labels=%E2%9C%A8+enhancement&template=feature-request.md";
					break;

				case IssueType.Question:
					issueTypeParameter = "labels=%E2%9D%93+question&template=ask-a-question.md";
					break;
			}
			if (issueTypeParameter != null)
			{
				urlBuilder.Append(issueTypeParameter);
				firstParamter = false;
			}

			if (!string.IsNullOrEmpty(title))
			{
				if (!firstParamter)
				{
					urlBuilder.Append('&');
				}
				urlBuilder.Append("title=");
				urlBuilder.Append(Uri.EscapeDataString(title));
				firstParamter = false;
			}

			if (!string.IsNullOrEmpty(body))
			{
				if (!firstParamter)
				{
					urlBuilder.Append('&');
				}
				urlBuilder.Append("body=");
				urlBuilder.Append(Uri.EscapeDataString(body));
			}

			Application.OpenURL(urlBuilder.ToString());
		}

		/// <summary>
		/// <para>Engages the user-flow for writing a review for the asset.</para>
		/// </summary>
		/// <param name="reviewStars">The user star-rating for the review.</param>
		/// <param name="reviewHeader">The title of the review.</param>
		/// <param name="reviewBody">The body of the review.</param>
		public static void WriteReview(int reviewStars, string reviewHeader = "", string reviewBody = "")
		{
			Application.OpenURL("https://assetstore.unity.com/packages/slug/100547#reviews");
		}
	}
}

#pragma warning restore
#endif
