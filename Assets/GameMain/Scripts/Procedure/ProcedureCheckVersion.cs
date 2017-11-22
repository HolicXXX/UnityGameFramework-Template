using UnityEngine;
using GameFramework;
using GameFramework.Event;
using GameFramework.Resource;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace GameMain {
	public class ProcedureCheckVersion : ProcedureBase {
		public override bool UseNativeDialog {
			get {
				return true;
			}
		}

		private bool m_LatestVersionComplete = false;
		private VersionInfo m_VersionInfo = null;

		protected override void OnInit (ProcedureOwner procedureOwner)
		{
			base.OnInit (procedureOwner);
		}

		protected override void OnEnter (ProcedureOwner procedureOwner)
		{
			base.OnEnter (procedureOwner);

			m_LatestVersionComplete = false;

			RequestVersion ();
		}

		protected override void OnUpdate (ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
		{
			base.OnUpdate (procedureOwner, elapseSeconds, realElapseSeconds);

			if (!m_LatestVersionComplete) {
				return;
			}

			ChangeState<ProcedureUpdateResource> (procedureOwner);
		}

		protected override void OnLeave (ProcedureOwner procedureOwner, bool isShutdown)
		{
			base.OnLeave (procedureOwner, isShutdown);
		}

		protected override void OnDestroy (ProcedureOwner procedureOwner)
		{
			base.OnDestroy (procedureOwner);
		}

		private void RequestVersion(){
			string deviceId = SystemInfo.deviceUniqueIdentifier;
			string deviceName = SystemInfo.deviceName;
			string deviceModel = SystemInfo.deviceModel;
			string processorType = SystemInfo.processorType;
			string processorCount = SystemInfo.processorCount.ToString();
			string memorySize = SystemInfo.systemMemorySize.ToString();
			string operatingSystem = SystemInfo.operatingSystem;
			string iOSGeneration = string.Empty;
			string iOSSystemVersion = string.Empty;
			string iOSVendorIdentifier = string.Empty;
			#if UNITY_IOS && !UNITY_EDITOR
			iOSGeneration = UnityEngine.iOS.Device.generation.ToString();
			iOSSystemVersion = UnityEngine.iOS.Device.systemVersion;
			iOSVendorIdentifier = UnityEngine.iOS.Device.vendorIdentifier ?? string.Empty;
			#endif
			string gameVersion = GameEntry.Base.GameVersion;
			string platform = Application.platform.ToString();
			string language = GameEntry.Localization.Language.ToString();
			string unityVersion = Application.unityVersion;
			string installMode = Application.installMode.ToString();
			string sandboxType = Application.sandboxType.ToString();
			string screenWidth = Screen.width.ToString();
			string screenHeight = Screen.height.ToString();
			string screenDpi = Screen.dpi.ToString();
			string screenOrientation = Screen.orientation.ToString();
			string screenResolution = string.Format("{0} x {1} @ {2}Hz", Screen.currentResolution.width.ToString(), Screen.currentResolution.height.ToString(), Screen.currentResolution.refreshRate.ToString());
			string useWifi = (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork).ToString();

			WWWForm wwwForm = new WWWForm();
			wwwForm.AddField("DeviceId", WebUtility.EscapeString(deviceId));
			wwwForm.AddField("DeviceName", WebUtility.EscapeString(deviceName));
			wwwForm.AddField("DeviceModel", WebUtility.EscapeString(deviceModel));
			wwwForm.AddField("ProcessorType", WebUtility.EscapeString(processorType));
			wwwForm.AddField("ProcessorCount", WebUtility.EscapeString(processorCount));
			wwwForm.AddField("MemorySize", WebUtility.EscapeString(memorySize));
			wwwForm.AddField("OperatingSystem", WebUtility.EscapeString(operatingSystem));
			wwwForm.AddField("IOSGeneration", WebUtility.EscapeString(iOSGeneration));
			wwwForm.AddField("IOSSystemVersion", WebUtility.EscapeString(iOSSystemVersion));
			wwwForm.AddField("IOSVendorIdentifier", WebUtility.EscapeString(iOSVendorIdentifier));
			wwwForm.AddField("GameVersion", WebUtility.EscapeString(gameVersion));
			wwwForm.AddField("Platform", WebUtility.EscapeString(platform));
			wwwForm.AddField("Language", WebUtility.EscapeString(language));
			wwwForm.AddField("UnityVersion", WebUtility.EscapeString(unityVersion));
			wwwForm.AddField("InstallMode", WebUtility.EscapeString(installMode));
			wwwForm.AddField("SandboxType", WebUtility.EscapeString(sandboxType));
			wwwForm.AddField("ScreenWidth", WebUtility.EscapeString(screenWidth));
			wwwForm.AddField("ScreenHeight", WebUtility.EscapeString(screenHeight));
			wwwForm.AddField("ScreenDPI", WebUtility.EscapeString(screenDpi));
			wwwForm.AddField("ScreenOrientation", WebUtility.EscapeString(screenOrientation));
			wwwForm.AddField("ScreenResolution", WebUtility.EscapeString(screenResolution));
			wwwForm.AddField("UseWifi", WebUtility.EscapeString(useWifi));

			GameEntry.WebRequest.AddWebRequest(GameEntry.Config.BuildInfo.CheckVersionUrl, wwwForm, this);
		}

		private void GotoUpdateApp(object userData)
		{
			string url = null;
			#if UNITY_EDITOR
			url = GameEntry.Config.BuildInfo.StandaloneAppUrl;
			#elif UNITY_IOS
			url = GameEntry.Config.BuildInfo.IosAppUrl;
			#elif UNITY_ANDROID
			url = GameEntry.Config.BuildInfo.AndroidAppUrl;
			#else
			url = GameEntry.Config.BuildInfo.StandaloneAppUrl;
			#endif
			Application.OpenURL(url);
			}

		private void UpdateVersion()
		{
			if (GameEntry.Resource.CheckVersionList(m_VersionInfo.InternalResourceVersion) == CheckVersionListResult.Updated)
			{
				m_LatestVersionComplete = true;
			}
			else
			{
				GameEntry.Resource.UpdateVersionList(m_VersionInfo.VersionListLength, m_VersionInfo.VersionListHashCode, m_VersionInfo.VersionListZipLength, m_VersionInfo.VersionListZipHashCode);
			}
		}

		private void OnWebRequestSuccess(object sender, GameEventArgs e)
		{
			WebRequestSuccessEventArgs ne = (WebRequestSuccessEventArgs)e;
			if (ne.UserData != this)
			{
				return;
			}

			m_VersionInfo = Utility.Json.ToObject<VersionInfo>(ne.GetWebResponseBytes());
			if (m_VersionInfo == null)
			{
				Log.Error("Parse VersionInfo failure.");
				return;
			}

			Log.Info("Latest game version is '{0}', local game version is '{1}'.", m_VersionInfo.LatestGameVersion, GameEntry.Base.GameVersion);

			if (m_VersionInfo.ForceGameUpdate)
			{
				GameEntry.UI.OpenDialog(new DialogParams
					{
						Mode = 2,
						Title = GameEntry.Localization.GetString("ForceUpdate.Title"),
						Message = GameEntry.Localization.GetString("ForceUpdate.Message"),
						ConfirmText = GameEntry.Localization.GetString("ForceUpdate.UpdateButton"),
						OnClickConfirm = GotoUpdateApp,
						CancelText = GameEntry.Localization.GetString("ForceUpdate.QuitButton"),
						OnClickCancel = delegate (object userData) { UnityGameFramework.Runtime.GameEntry.Shutdown(ShutdownType.Quit); },
					}
				);

				return;
			}

			GameEntry.Resource.UpdatePrefixUri = Utility.Path.GetCombinePath(m_VersionInfo.GameUpdateUrl, GetResourceVersionName(), GetPlatformPath());

			UpdateVersion();
		}
		
		private void OnWebRequestFailure(object sender, GameEventArgs e)
		{
			WebRequestFailureEventArgs ne = (WebRequestFailureEventArgs)e;
			if (ne.UserData != this)
			{
				return;
			}

			Log.Warning("Check version failure, error message '{0}'.", ne.ErrorMessage);
		}

		private void OnVersionListUpdateSuccess(object sender, GameEventArgs e)
		{
			UnityGameFramework.Runtime.VersionListUpdateSuccessEventArgs ne = (UnityGameFramework.Runtime.VersionListUpdateSuccessEventArgs)e;
			m_LatestVersionComplete = true;
			Log.Info("Download latest resource version list from '{0}' success.", ne.DownloadUri);
		}

		private void OnVersionListUpdateFailure(object sender, GameEventArgs e)
		{
			UnityGameFramework.Runtime.VersionListUpdateFailureEventArgs ne = (UnityGameFramework.Runtime.VersionListUpdateFailureEventArgs)e;
			Log.Warning("Download latest resource version list from '{0}' failure, error message '{1}'.", ne.DownloadUri, ne.ErrorMessage);
		}

		private string GetResourceVersionName()
		{
			string[] splitApplicableGameVersion = GameEntry.Base.GameVersion.Split('.');
			if (splitApplicableGameVersion.Length != 3)
			{
				return string.Empty;
			}

			return string.Format("{0}_{1}_{2}_{3}", splitApplicableGameVersion[0], splitApplicableGameVersion[1], splitApplicableGameVersion[2], m_VersionInfo.InternalResourceVersion.ToString());
		}

		private string GetPlatformPath()
		{
			switch (Application.platform)
			{
			case RuntimePlatform.WindowsEditor:
			case RuntimePlatform.WindowsPlayer:
				return "windows";
			case RuntimePlatform.OSXEditor:
			case RuntimePlatform.OSXPlayer:
				return "osx";
			case RuntimePlatform.IPhonePlayer:
				return "ios";
			case RuntimePlatform.Android:
				return "android";
			case RuntimePlatform.WSAPlayerX86:
			case RuntimePlatform.WSAPlayerX64:
			case RuntimePlatform.WSAPlayerARM:
				return "winstore";
			default:
				return string.Empty;
			}
		}
	}
}
