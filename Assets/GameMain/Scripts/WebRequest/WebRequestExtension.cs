using System;
using GameFramework;
using UnityGameFramework.Runtime;
using UnityEngine;

namespace GameMain {
	public static class WebRequestExtension {

		public static int Get(this WebRequestComponent web, string url){
			return web.AddWebRequest (url);
		}

		public static int Get(this WebRequestComponent web, string url, object userData){
			return web.AddWebRequest (url, userData);
		}

		public static int Post(this WebRequestComponent web, string url, WWWForm form){
			return web.AddWebRequest (url, form);
		}

		public static int Post(this WebRequestComponent web, string url, WWWForm form, object userData){
			return web.AddWebRequest (url, form, userData);
		}

		public static int Post(this WebRequestComponent web, string url, byte[] datas){
			return web.AddWebRequest (url, datas);
		}

		public static int Post(this WebRequestComponent web, string url, byte[] datas, object userData){
			return web.AddWebRequest (url, datas, userData);
		}
	}
}
