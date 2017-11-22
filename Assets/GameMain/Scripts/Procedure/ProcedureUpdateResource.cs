using GameFramework;
using GameFramework.Event;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace GameMain {
	public class ProcedureUpdateResource : ProcedureBase
	{
		private bool m_UpdateAllComplete = false;
		private int m_UpdateCount = 0;
		private int m_UpdateTotalZipLength = 0;
		private int m_UpdateSuccessCount = 0;
		private List<UpdateLengthData> m_UpdateLengthData = new List<UpdateLengthData>();
		//Cached Update UI Form

		public override bool UseNativeDialog
		{
			get
			{
				return true;
			}
		}

		protected override void OnEnter(ProcedureOwner procedureOwner)
		{
			base.OnEnter(procedureOwner);

			m_UpdateAllComplete = false;
			m_UpdateCount = 0;
			m_UpdateTotalZipLength = 0;
			m_UpdateSuccessCount = 0;
			m_UpdateLengthData.Clear();

			GameEntry.Event.Subscribe(ResourceCheckCompleteEventArgs.EventId, OnResourceCheckComplete);
			GameEntry.Event.Subscribe(ResourceUpdateStartEventArgs.EventId, OnResourceUpdateStart);
			GameEntry.Event.Subscribe(ResourceUpdateChangedEventArgs.EventId, OnResourceUpdateChanged);
			GameEntry.Event.Subscribe(ResourceUpdateSuccessEventArgs.EventId, OnResourceUpdateSuccess);
			GameEntry.Event.Subscribe(ResourceUpdateFailureEventArgs.EventId, OnResourceUpdateFailure);
			GameEntry.Event.Subscribe(ResourceUpdateAllCompleteEventArgs.EventId, OnResourceUpdateAllComplete);

			GameEntry.Resource.CheckResources();
		}

		protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
		{
			//Destroy Update UI Form

			GameEntry.Event.Unsubscribe(ResourceCheckCompleteEventArgs.EventId, OnResourceCheckComplete);
			GameEntry.Event.Unsubscribe(ResourceUpdateStartEventArgs.EventId, OnResourceUpdateStart);
			GameEntry.Event.Unsubscribe(ResourceUpdateChangedEventArgs.EventId, OnResourceUpdateChanged);
			GameEntry.Event.Unsubscribe(ResourceUpdateSuccessEventArgs.EventId, OnResourceUpdateSuccess);
			GameEntry.Event.Unsubscribe(ResourceUpdateFailureEventArgs.EventId, OnResourceUpdateFailure);
			GameEntry.Event.Unsubscribe(ResourceUpdateAllCompleteEventArgs.EventId, OnResourceUpdateAllComplete);

			base.OnLeave(procedureOwner, isShutdown);
		}

		protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
		{
			base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

			if (!m_UpdateAllComplete)
			{
				return;
			}

			ChangeState<ProcedurePreload>(procedureOwner);
		}

		private void StartUpdateResources(object userData)
		{
			//Show Update UI Form

			GameEntry.Resource.UpdateResources();
			Log.Info("Start update resources...");
		}

		private void OnResourceCheckComplete(object sender, GameEventArgs e)
		{
			ResourceCheckCompleteEventArgs ne = (ResourceCheckCompleteEventArgs)e;
			Log.Info("Check resource complete, '{0}' resources need to update, zip length is '{1}', unzip length is '{2}'.", ne.UpdateCount.ToString(), ne.UpdateTotalZipLength.ToString(), ne.UpdateTotalLength.ToString());

			m_UpdateCount = ne.UpdateCount;
			m_UpdateTotalZipLength = ne.UpdateTotalZipLength;
			if (m_UpdateCount <= 0)
			{
				ProcessUpdateAllComplete();
				return;
			}

			if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
			{
				GameEntry.UI.OpenDialog(new DialogParams
					{
						Mode = 2,
						Title = GameEntry.Localization.GetString("UpdateResourceViaCarrierDataNetwork.Title"),
						Message = GameEntry.Localization.GetString("UpdateResourceViaCarrierDataNetwork.Message"),
						ConfirmText = GameEntry.Localization.GetString("UpdateResourceViaCarrierDataNetwork.UpdateButton"),
						OnClickConfirm = StartUpdateResources,
						CancelText = GameEntry.Localization.GetString("UpdateResourceViaCarrierDataNetwork.QuitButton"),
						OnClickCancel = delegate (object userData) { UnityGameFramework.Runtime.GameEntry.Shutdown(ShutdownType.Quit); },
					});

				return;
			}

			StartUpdateResources(null);
		}

		private void OnResourceUpdateStart(object sender, GameEventArgs e)
		{
			ResourceUpdateStartEventArgs ne = (ResourceUpdateStartEventArgs)e;

			for (int i = 0; i < m_UpdateLengthData.Count; i++)
			{
				if (m_UpdateLengthData[i].Name == ne.Name)
				{
					Log.Warning("Update resource '{0}' is invalid.", ne.Name);
					m_UpdateLengthData[i].Length = 0;
					RefreshProgress();
					return;
				}
			}

			m_UpdateLengthData.Add(new UpdateLengthData(ne.Name));
		}

		private void OnResourceUpdateChanged(object sender, GameEventArgs e)
		{
			ResourceUpdateChangedEventArgs ne = (ResourceUpdateChangedEventArgs)e;

			for (int i = 0; i < m_UpdateLengthData.Count; i++)
			{
				if (m_UpdateLengthData[i].Name == ne.Name)
				{
					m_UpdateLengthData[i].Length = ne.CurrentLength;
					RefreshProgress();
					return;
				}
			}

			Log.Warning("Update resource '{0}' is invalid.", ne.Name);
		}

		private void OnResourceUpdateSuccess(object sender, GameEventArgs e)
		{
			ResourceUpdateSuccessEventArgs ne = (ResourceUpdateSuccessEventArgs)e;
			Log.Info("Update resource '{0}' success.", ne.Name);

			for (int i = 0; i < m_UpdateLengthData.Count; i++)
			{
				if (m_UpdateLengthData[i].Name == ne.Name)
				{
					m_UpdateLengthData[i].Length = ne.ZipLength;
					m_UpdateSuccessCount++;
					RefreshProgress();
					return;
				}
			}

			Log.Warning("Update resource '{0}' is invalid.", ne.Name);
		}

		private void OnResourceUpdateFailure(object sender, GameEventArgs e)
		{
			ResourceUpdateFailureEventArgs ne = (ResourceUpdateFailureEventArgs)e;
			if (ne.RetryCount >= ne.TotalRetryCount)
			{
				Log.Error("Update resource '{0}' failure from '{1}' with error message '{2}', retry count '{3}'.", ne.Name, ne.DownloadUri, ne.ErrorMessage, ne.RetryCount.ToString());
				return;
			}
			else
			{
				Log.Info("Update resource '{0}' failure from '{1}' with error message '{2}', retry count '{3}'.", ne.Name, ne.DownloadUri, ne.ErrorMessage, ne.RetryCount.ToString());
			}

			for (int i = 0; i < m_UpdateLengthData.Count; i++)
			{
				if (m_UpdateLengthData[i].Name == ne.Name)
				{
					m_UpdateLengthData.Remove(m_UpdateLengthData[i]);
					RefreshProgress();
					return;
				}
			}

			Log.Warning("Update resource '{0}' is invalid.", ne.Name);
		}

		private void OnResourceUpdateAllComplete(object sender, GameEventArgs e)
		{
			Log.Info("All resources update complete.");
			ProcessUpdateAllComplete();
		}

		private void ProcessUpdateAllComplete()
		{
			m_UpdateAllComplete = true;
		}

		private void RefreshProgress()
		{
			int currentTotalUpdateLength = 0;
			for (int i = 0; i < m_UpdateLengthData.Count; i++)
			{
				currentTotalUpdateLength += m_UpdateLengthData[i].Length;
			}

			float progressTotal = (float)currentTotalUpdateLength / m_UpdateTotalZipLength;
			string descriptionText = GameEntry.Localization.GetString("UpdateResource.Tips", m_UpdateSuccessCount.ToString(), m_UpdateCount.ToString(), GetLengthString(currentTotalUpdateLength), GetLengthString(m_UpdateTotalZipLength), progressTotal, GetLengthString((int)GameEntry.Download.CurrentSpeed));
			Log.Debug ("UpdateResource: {0} {1}%", descriptionText, progressTotal * 100f);
			//Update UI Form's Progress
		}

		private string GetLengthString(int length)
		{
			if (length < 1024)
			{
				return string.Format("{0} Bytes", length.ToString());
			}

			if (length < 1024 * 1024)
			{
				return string.Format("{0} KB", (length / 1024f).ToString("F2"));
			}

			if (length < 1024 * 1024 * 1024)
			{
				return string.Format("{0} MB", (length / 1024f / 1024f).ToString("F2"));
			}

			return string.Format("{0} GB", (length / 1024f / 1024f / 1024f).ToString("F2"));
		}

		private class UpdateLengthData
		{
			private readonly string m_Name;

			public UpdateLengthData(string name)
			{
				m_Name = name;
			}

			public string Name
			{
				get
				{
					return m_Name;
				}
			}

			public int Length
			{
				get;
				set;
			}
		}
	}
}
