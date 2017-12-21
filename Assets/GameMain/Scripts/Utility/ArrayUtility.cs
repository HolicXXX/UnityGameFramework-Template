using System.Collections;
using System.Collections.Generic;
using System;

namespace GameMain {
	
	/// <summary>
	/// Array Utility.
	/// </summary>
	public static class ArrayUtility {

		/// <summary>
		/// Concat the specified arr1 and arr2.
		/// </summary>
		/// <returns>The concat result.</returns>
		/// <param name="arr1">first Array.</param>
		/// <param name="arr2">second Array.</param>
		/// <typeparam name="T">The type parameter.</typeparam>
		public static T [] Concat<T> (T [] arr1, T [] arr2) {
			if (arr1 == null || arr2 == null) {
				throw new ArgumentNullException ();
			}
			var ret = new T [arr1.Length + arr2.Length];
			Array.Copy (arr1, 0, ret, 0, arr1.Length);
			Array.Copy (arr2, 0, ret, arr1.Length, arr2.Length);
			return ret;
		}

		/// <summary>
		/// Merge the specified sources array.
		/// </summary>
		/// <returns>The merge result.</returns>
		/// <param name="sources">Sources Array.</param>
		/// <typeparam name="T">The type parameter.</typeparam>
		public static T [] Merge<T> (params T [] [] sources) {
			if (sources == null) {
				throw new ArgumentNullException ();
			}
			int length = 0;
			foreach (var source in sources) {
				length += source.Length;
			}
			T [] ret = new T [length];
			int offset = 0;
			foreach (var source in sources) {
				Array.Copy (source, 0, ret, offset, source.Length);
				offset += source.Length;
			}
			return ret;
		}

		/// <summary>
		/// Whether the specified source has a element matches the predicate method.
		/// </summary>
		/// <returns>Result.</returns>
		/// <param name="source">Source Array.</param>
		/// <param name="predicate">Predicate Method.</param>
		/// <typeparam name="T">The type parameter.</typeparam>
		public static bool Some<T> (T [] source, Predicate<T> predicate) {
			if (source == null || predicate == null) {
				return false;
			}
			return Array.Exists (source, predicate);
		}

		/// <summary>
		/// If all elements in the specified source match the predicate method.
		/// </summary>
		/// <returns>The Result.</returns>
		/// <param name="source">Source Array.</param>
		/// <param name="predicate">Predicate Method.</param>
		/// <typeparam name="T">The type parameter.</typeparam>
		public static bool All<T> (T [] source, Predicate<T> predicate) {
			if (source == null || predicate == null) {
				return false;
			}
			return Array.TrueForAll (source, predicate);
		}

		/// <summary>
		/// Filter the specified source with a predicate method.
		/// </summary>
		/// <returns>Result Array.</returns>
		/// <param name="source">Source Array.</param>
		/// <param name="predicate">Predicate Method.</param>
		/// <typeparam name="T">The type parameter.</typeparam>
		public static T [] Filter<T> (T [] source, Predicate<T> predicate) {
			if (source == null || predicate == null) {
				throw new ArgumentNullException ();
			}

			return Array.FindAll (source, predicate);
		}

		/// <summary>
		/// Turn the specified source's all element with a transfer operation method.
		/// </summary>
		/// <returns>The Result.</returns>
		/// <param name="source">Source Array.</param>
		/// <param name="operation">Operation Method.</param>
		/// <typeparam name="T1">The origin type parameter.</typeparam>
		/// <typeparam name="T2">The result type parameter.</typeparam>
		public static T2 [] Map<T1, T2> (T1[] source, Func<T1, T2> operation) {
			if (source == null || operation == null) {
				throw new ArgumentNullException ();
			}

			T2 [] ret = new T2 [source.Length];
			for (int i = 0; i < source.Length; ++i) {
				ret [i] = operation (source [i]);
			}
			return ret;
		}

		/// <summary>
		/// Pop a element form the specified source'end.
		/// </summary>
		/// <returns>The End element.</returns>
		/// <param name="source">Source Array.</param>
		/// <typeparam name="T">The type parameter.</typeparam>
		public static T Pop<T> (ref T [] source) {
			if (source == null || source.Length == 0) {
				throw new ArgumentNullException ();
			}
			T ret = source [source.Length - 1];
			Array.Resize (ref source, source.Length - 1);
			return ret;
		}

		/// <summary>
		/// Push the elements into the specified source's head.
		/// </summary>
		/// <returns>The length of the result array.</returns>
		/// <param name="source">Source array.</param>
		/// <param name="elements">Elements to be pushed.</param>
		/// <typeparam name="T">The type parameter.</typeparam>
		public static int Push<T> (ref T [] source, params T [] elements) {
			if (source == null || elements == null) {
				throw new ArgumentNullException ();
			}

			Array.Resize (ref source, source.Length + elements.Length);
			Array.Copy (elements, 0, source, source.Length - elements.Length, elements.Length);
			return source.Length;
		}

		/// <summary>
		/// Shift an element out from the specified source's head;
		/// </summary>
		/// <returns>The shifted element.</returns>
		/// <param name="source">Source array.</param>
		/// <typeparam name="T">The type parameter.</typeparam>
		public static T Shift<T> (ref T [] source) {
			if (source == null || source.Length == 0) {
				throw new ArgumentNullException ();
			}

			T ret = source [0];
			T [] newSource = new T [source.Length - 1];
			Array.Copy (source, 1, newSource, 0, source.Length - 1);
			source = newSource;
			return ret;
		}

		/// <summary>
		/// Insert elements into the specified source's head.
		/// </summary>
		/// <returns>The length of the result array.</returns>
		/// <param name="source">Source array.</param>
		/// <param name="elements">Elements to be inserted.</param>
		/// <typeparam name="T">The type parameter.</typeparam>
		public static int Unshift<T> (ref T [] source, params T [] elements) {
			if (source == null || elements == null) {
				throw new ArgumentNullException ();
			}

			T [] newSource = new T [source.Length + elements.Length];
			Array.Copy (elements, 0, newSource, 0, elements.Length);
			Array.Copy (source, 0, newSource, elements.Length, source.Length);
			source = newSource;
			return source.Length;
		}

		/// <summary>
		/// To split up the specified source into some arrays which's length is chunkSize 
		/// </summary>
		/// <returns>The Result Array.</returns>
		/// <param name="source">Source Array.</param>
		/// <param name="chunkSize">Chunk size.</param>
		/// <typeparam name="T">The type parameter.</typeparam>
		public static T [] [] Chunk<T> (T [] source, int chunkSize) {
			if (source == null || chunkSize <= 0) {
				throw new ArgumentException ();
			}

			T [] [] ret = new T [source.Length / chunkSize + (source.Length % chunkSize == 0 ? 0 : 1)] [];
			T [] chunk = null;
			for (int i = 0; i < source.Length; ++i) {
				if (i % chunkSize == 0) {
					ret [i / chunkSize] = chunk = new T [i + chunkSize <= source.Length ? chunkSize : source.Length - i];
				}
				chunk [i % chunkSize] = source [i];
			}
			return ret;
		}

		/// <summary>
		/// Remove some elments form the specified source, and insert some into the same place where elements are removed.
		/// </summary>
		/// <returns>The removed elements.</returns>
		/// <param name="source">Source array.</param>
		/// <param name="start">Start index.</param>
		/// <param name="length">Length to remove.</param>
		/// <param name="insertSource">Insert element array.</param>
		/// <typeparam name="T">The type parameter.</typeparam>
		public static T [] Splice<T> (ref T [] source, int start, int? length = null, T [] insertSource = null) {
			if (source == null || start < 0 || start >= source.Length) {
				throw new ArgumentException ();
			}

			length = length.HasValue ? Math.Min (source.Length - start, length.Value) : source.Length - start;
			T [] ret = new T [length.Value];
			Array.Copy (source, start, ret, 0, length.Value);
			int newLength = source.Length - length.Value + (insertSource == null ? 0 : insertSource.Length);
			T [] newSource = new T [newLength];
			Array.Copy (source, 0, newSource, 0, start);
			if (insertSource != null) {
				Array.Copy (insertSource, 0, newSource, start, insertSource.Length);
			}
			if (start + length.Value < source.Length) {
				Array.Copy (source, length.Value + start, newSource, start + (insertSource == null ? 0 : insertSource.Length), source.Length - (start + length.Value));
			}

			return ret;
		}

		/// <summary>
		/// Slice some elements form the specified source.
		/// </summary>
		/// <returns>The elements sliced from origin array.</returns>
		/// <param name="source">Source array.</param>
		/// <param name="start">Start index.</param>
		/// <param name="length">Length number.</param>
		/// <typeparam name="T">The type parameter.</typeparam>
		public static T [] Slice<T> (T [] source, int start, int? length = null) {
			if (source == null || start < 0 || start >= source.Length) {
				throw new ArgumentException ();
			}

			length = length.HasValue ? Math.Min (source.Length - start, length.Value) : source.Length - start;
			T [] ret = new T [length.Value];
			Array.Copy (source, start, ret, 0, length.Value);
			return ret;
		}

		/// <summary>
		/// Difference the specified arr1 and arr2, and return the elements that are contained in first array, and not in second array.
		/// </summary>
		/// <returns>The difference elements array.</returns>
		/// <param name="arr1">First Array.</param>
		/// <param name="arr2">Second Array.</param>
		/// <typeparam name="T">The type parameter.</typeparam>
		public static T [] Difference<T> (T [] arr1, T [] arr2) {
			if (arr1 == null || arr2 == null) {
				throw new ArgumentNullException ();
			}

			return Filter (arr1, t => {
				foreach (var t2 in arr2) {
					if (t2.Equals (t)) {
						return false;
					}
				}
				return true;
			});
		}

		/// <summary>
		/// Reverse the specified source's part or whole array.
		/// </summary>
		/// <returns>The reverse result array.</returns>
		/// <param name="source">Source array.</param>
		/// <param name="start">Start index.</param>
		/// <param name="length">Length.</param>
		/// <typeparam name="T">The type parameter.</typeparam>
		public static T [] Reverse<T> (T [] source, int start = 0, int? length = null) {
			if (source == null || start < 0 || start >= source.Length) {
				throw new ArgumentException ();
			}

			length = length.HasValue ? Math.Min (source.Length - start, length.Value) : source.Length - start;
			T [] ret = new T [length.Value];
			Array.Copy (source, start, ret, 0, length.Value);
			Array.Reverse (ret, 0, length.Value);
			return ret;
		}

		/// <summary>
		/// Shuffle the specified source into random with a seed.
		/// </summary>
		/// <returns>The shuffle result array.</returns>
		/// <param name="source">Source array.</param>
		/// <param name="seed">Seed.</param>
		/// <typeparam name="T">The type parameter.</typeparam>
		public static T [] Shuffle<T> (T [] source, int? seed = null) {
			if (source == null) {
				throw new ArgumentNullException ();
			}

			var random = RandomUtility.MakeRandom (seed);
			Array.Sort (source, (a, b) => random.Next (0, 100) > 50 ? 1 : -1);
			return source;
		}
	}
}
