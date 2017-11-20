using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using GameFramework.Fsm;
using UnityGameFramework.Runtime;

namespace GameMain {
	public class CharacterComponent : GameFrameworkComponent {

		[SerializeField]
		private List<string> m_CharacterStateTypes;

		private Dictionary<string,Dictionary<string,ICharacterManager>> m_CharacterInstancesDict;

		protected override void Awake(){
			base.Awake ();

			InitStateDictionary ();
		}

		void Start () {
			
		}

		void OnDestroy() {
			m_CharacterStateTypes.Clear ();
			foreach (var pair in m_CharacterInstancesDict) {
				pair.Value.Clear ();
			}
			m_CharacterInstancesDict.Clear ();
		}

		void Update () {
			
		}

		public bool HasCharacterManager<T>(string cname) where T : ICharacterManager{
			Type t = typeof(T);
			return HasCharacterManager (t, cname);
		}

		public bool HasCharacterManager(Type cmType, string cname){
			if (string.IsNullOrEmpty (cname)) {
				throw new GameFrameworkException ("Invalid Name to check CharacterManager");
			}

			string tName = cmType.FullName;

			if (cmType.GetInterface ("ICharacterManager") == null) {
				throw new GameFrameworkException (string.Format ("Invalid Type {0} to check CharacterManager.", tName));
			}

			return m_CharacterInstancesDict.ContainsKey (tName) && m_CharacterInstancesDict [tName].ContainsKey (string.Format ("{0}.{1}", cname, cmType.Name));
		}

		public ICharacterManager CreateCharacterManager<T>(CharacterControllerBase owner, params CharacterStateBase[] states) where T : ICharacterManager{
			Type t = typeof(T);
			return CreateCharacterManager (t, owner, states);
		}

		public ICharacterManager CreateCharacterManager(Type cmType, CharacterControllerBase owner, params CharacterStateBase[] states){
			if (owner == null) {
				throw new GameFrameworkException ("Invalid Owner to create CharacterManager");
			}

			if (states.Length < 1) {
				throw new GameFrameworkException ("Must has at least One State to create a CharacterManager");
			}

			string tName = cmType.FullName;

			if (cmType.GetInterface ("ICharacterManager") == null) {
				throw new GameFrameworkException (string.Format ("Invalid Type {0} to create CharacterManager.", tName));
			}

			ICharacterManager ret = null;
			Dictionary<string, ICharacterManager> dict = null;
			if (m_CharacterInstancesDict.TryGetValue (tName, out dict)) {
				string instName = string.Format ("{0}.{1}", owner.name, cmType.Name);
				if (dict.ContainsKey(instName)) {
					Log.Warning ("{0} already Exists, will be replaced", instName);
				}
				ret = (ICharacterManager)Activator.CreateInstance (cmType);
				ret.Initialize (GameFrameworkEntry.GetModule<IFsmManager>(), owner, states);
				dict [instName] = ret;
			} else {
				throw new GameFrameworkException (string.Format ("Invalid Type {0} to create CharacterManager.", tName));
			}

			return ret;
		}

		public ICharacterManager GetCharacterManager<T>(string cname) where T : ICharacterManager{
			Type t = typeof(T);
			return GetCharacterManager (t, cname);
		}

		public ICharacterManager GetCharacterManager(Type cmType, string cname){
			if (string.IsNullOrEmpty (cname)) {
				throw new GameFrameworkException ("Invalid Name to get CharacterManager");
			}

			string tName = cmType.FullName;

			if (cmType.GetInterface ("ICharacterManager") == null) {
				throw new GameFrameworkException (string.Format ("Invalid Type {0} to get CharacterManager.", tName));
			}

			ICharacterManager ret = null;
			Dictionary<string, ICharacterManager> dict = null;
			if (m_CharacterInstancesDict.TryGetValue (tName, out dict)) {
				string instName = string.Format ("{0}.{1}", cname, cmType.Name);
				dict.TryGetValue (instName, out ret);
			} else {
				throw new GameFrameworkException (string.Format ("Invalid Type {0} to create CharacterManager.", tName));
			}

			return ret;
		}

		public void RemoveCharacterManager<T>(string cname){
			Type t = typeof(T);
			RemoveCharacterManager (t, cname);
		}

		public void RemoveCharacterManager(Type cmType, string cname){
			if (string.IsNullOrEmpty (cname)) {
				throw new GameFrameworkException ("Invalid Name to remove CharacterManager");
			}

			string tName = cmType.FullName;

			if (cmType.GetInterface ("ICharacterManager") == null) {
				throw new GameFrameworkException (string.Format ("Invalid Type {0} to remove CharacterManager.", tName));
			}

			Dictionary<string, ICharacterManager> dict = null;
			if (m_CharacterInstancesDict.TryGetValue (tName, out dict)) {
				string instName = string.Format ("{0}.{1}", cname, cmType.Name);
				if (dict.ContainsKey (instName)) {
					dict.Remove (instName);
				}
			} else {
				throw new GameFrameworkException (string.Format ("Invalid Type {0} to create CharacterManager.", tName));
			}
		}

		private void InitStateDictionary(){
			m_CharacterInstancesDict = new Dictionary<string, Dictionary<string, ICharacterManager>> ();
			m_CharacterStateTypes = new List<string> ();

			Assembly assembly = Assembly.GetExecutingAssembly();
			Type[] types = assembly.GetTypes();

			for (int i = 0; i < types.Length; i++)
			{
				if (!types[i].IsClass || types[i].IsAbstract)
				{
					continue;
				}

				if (types[i].GetInterface("ICharacterManager") != null)
				{
					Dictionary<string, ICharacterManager> dict = null;
					string tName = types [i].FullName;
					if (!m_CharacterInstancesDict.TryGetValue (tName, out dict))
					{
						dict = new Dictionary<string, ICharacterManager> ();
						m_CharacterInstancesDict.Add (tName, dict);
						m_CharacterStateTypes.Add (tName);
					}
				}
			}

		}
	}
}
