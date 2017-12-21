using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

namespace GameMain {
	public static class StringUtility {
		
		/// <summary>
		/// Pad type.
		/// </summary>
		public enum PadType {
			/// <summary>
			/// Pad The left side.
			/// </summary>
			Left,
			/// <summary>
			/// Pad The right side.
			/// </summary>
			Right,
			/// <summary>
			/// Pad The both sids.
			/// </summary>
			Both
		}

		/// <summary>
		/// Padding the specified str.
		/// </summary>
		/// <returns>The result string.</returns>
		/// <param name="str">String to be padded.</param>
		/// <param name="length">Total Length.</param>
		/// <param name="padStr">Padding string, default is " ".</param>
		/// <param name="type">Pad Type,padleft ,padright or padboth.</param>
		public static string Pad (this string str, int length, string padStr = null, PadType type = PadType.Left) {
			if (str.Length >= length) {
				return str;
			}
			int needPading = length - str.Length;
			int leftPadding = 0, rightPadding = 0;
			switch (type) {
			case PadType.Left:
				leftPadding = needPading;
				break;
			case PadType.Right:
				rightPadding = needPading;
				break;
			case PadType.Both:
				leftPadding = Convert.ToInt32 (Math.Floor (needPading / 2f));
				rightPadding = Convert.ToInt32 (Math.Ceiling (needPading / 2f));
				break;
			}

			padStr = padStr == null || padStr.Length <= 0 ? " " : padStr;

			return Repeat (padStr, leftPadding / padStr.Length + 1).Substring (0, leftPadding)
				                + str
				                + Repeat (padStr, rightPadding / padStr.Length + 1).Substring (0, rightPadding);
		}

		/// <summary>
		/// Repeat the specified str in count times.
		/// </summary>
		/// <returns>The result.</returns>
		/// <param name="str">String to repeat.</param>
		/// <param name="count">Count times.</param>
		public static string Repeat (string str, int count) {
			if (str == null || count < 0) {
				throw new ArgumentException ();
			}

			if (count <= 1) {
				return str;
			}

			var sb = new StringBuilder ();
			for (int i = 0; i < count; ++i) {
				sb.Append (str);
			}
			return sb.ToString ();
		}

		/// <summary>
		/// Split the specified str into string array with a chunk length.
		/// </summary>
		/// <returns>The result array.</returns>
		/// <param name="str">String.</param>
		/// <param name="length">Chunk length.</param>
		public static string [] Split (this string str, int length = 1) {
			if (str == null || length <= 0) {
				throw new ArgumentException ();
			}

			var requested = new string [str.Length / length + (str.Length % length == 0 ? 0 : 1)];

            for (var i = 0; i<str.Length; i += length)
            {
                requested [i / length] = str.Substring(i, Math.Min(str.Length - i, length));
            }

            return requested;
		}

		/// <summary>
		/// Shuffle the specified string into random with seed.
		/// </summary>
		/// <returns>The shuffled result.</returns>
		/// <param name="str">String.</param>
		/// <param name="seed">Random Seed.</param>
		public static string Shuffle (this string str, int? seed = null) {
			if (str == null) {
				throw new ArgumentNullException ();
			}
			var arr = str.ToCharArray ();
			arr = ArrayUtility.Shuffle (arr, seed);
			return new string (arr);
		}

		/// <summary>
		/// Count the times that the subStr exists in the specified str.
		/// </summary>
		/// <returns>The count result.</returns>
		/// <param name="str">String.</param>
		/// <param name="subStr">Sub string.</param>
		/// <param name="start">Start index.</param>
		/// <param name="length">Length.</param>
		/// <param name="comparison">Comparison.</param>
		public static int Count (this string str, string subStr, int start = 0, int? length = null, StringComparison comparison = StringComparison.CurrentCultureIgnoreCase) {
			if (str == null || subStr == null || start < 0 || start >= str.Length) {
				throw new ArgumentNullException ();
			}

			length = length.HasValue ? Math.Min (str.Length - start, length.Value) : str.Length - start;

			int count = 0;
			while (length.Value > 0) {
				int index = str.IndexOf (subStr, start, length.Value, comparison);
				if (index < 0) {
					break;
				}
				++count;
				start = index + subStr.Length;
				length = Math.Min (str.Length - start, length.Value);
			}

			return count;
		}

		/// <summary>
		/// Reverse the specified str.
		/// </summary>
		/// <returns>The reverse result.</returns>
		/// <param name="str">String.</param>
		public static string Reverse (this string str) {
			if (str == null) {
				throw new ArgumentNullException ();
			}
			var arr = str.ToCharArray ();
			Array.Reverse (str.ToCharArray ());
			return new string (arr);
		}


	}
}
