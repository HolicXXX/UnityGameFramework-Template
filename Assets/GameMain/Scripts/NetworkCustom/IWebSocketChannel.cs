using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;

namespace GameMain {
	public interface IWebSocketChannel {

		bool Connected {
			get;
		}

		float HeartBeatInterval {
			get;
			set;
		}

		string Name {
			get;
		}

		string Host {
			get;
		}

		string RemoteAddress {
			get;
		}

		int RemotePort {
			get;
		}

		bool ResetHeartBeatElapseSecondsWhenReceivePacket {
			get;
			set;
		}

		void Close ();

		void Connect (string url, string userName = null, string passWord = null);

		void Connect (Uri uri, string userName = null, string passWord = null);

		void RegisterHandler (IProtoHandler handler);

		void RegisterHandler (int packetId, EventHandler<GameFramework.Event.GameEventArgs> handler);

		void UnRegisterHandler (IProtoHandler handler);

		void UnRegisterHandler (int packetId, EventHandler<GameFramework.Event.GameEventArgs> handler);

		void Send(object packet, int packetType, Action<bool> onComplete = null);

		void Send(byte[] buffer, Action<bool> onComplete = null);
	}
}
