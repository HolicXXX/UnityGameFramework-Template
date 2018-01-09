using System;

namespace GameMain {
	/// <summary>
	/// Random Utility.
	/// </summary>
	public static class RandomUtility {

		private static Random _inst = null;
		/// <summary>
		/// The Random instance.
		/// </summary>
		public static Random RandomInst{
			get {
				if (_inst == null) {
					_inst = MakeRandom ();
				}
				return _inst;
			}
		}

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
