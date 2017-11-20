using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMain {
	public abstract class CharacterControllerBase : MonoBehaviour {
		
		protected Dictionary<Type, ICharacterManager> m_managerDict;

		protected virtual void Awake(){
			m_managerDict = new Dictionary<Type, ICharacterManager> ();
		}

		protected virtual void Start () {
			//Leave this to impletment
//			m_managerDict.Add (Type, GameEntry.Character.CreateCharacterManager (Type, this, states));
		}

		protected virtual void Update () {
			foreach (var pair in m_managerDict) {
				if (pair.Value.Enable) {
					pair.Value.Update (Time.deltaTime, Time.unscaledDeltaTime);
				}
			}
		}

		protected virtual void OnDestroy(){
			foreach (var pair in m_managerDict) {
				GameEntry.Character.RemoveCharacterManager (pair.Key, this.name);
			}
		}
	}
}
