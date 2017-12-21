using System;

namespace GameMain {
	/// <summary>
	/// Random Utility.
	/// </summary>
	public static class RandomUtility {
		/// <summary>
		/// Makes the seed.
		/// </summary>
		/// <returns>The seed.</returns>
		public static int MakeSeed () {
			return Environment.TickCount ^ Guid.NewGuid ().GetHashCode ();
		}

		/// <summary>
		/// Makes a system random object.
		/// </summary>
		/// <returns>The random object.</returns>
		/// <param name="seed">Seed.</param>
		public static Random MakeRandom (int? seed = null) {
			return new Random (seed.GetValueOrDefault (MakeSeed ()));
		}

	}
}
