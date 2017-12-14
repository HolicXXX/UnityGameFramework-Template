
namespace GameMain {
	public interface IHotFixHelper {
		bool Enable { get; }
		void Initialize ();
		void Update (float elapsedTime, float unscaledElapsedTime);
		void ShutDown ();
	}
}
