using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _instance;                          /// <summary>T Instance reference.</summary>
	
   [Header("Singleton's Attributes:")]
   [SerializeField] private bool _dontDestroyOnLoad;    /// <summary>Don't Destroy On Load?.</summary>

   /// <summary>Gets and Sets dontDestroyOnLoad property.</summary>
   public bool dontDestroyOnLoad
   {
      get { return _dontDestroyOnLoad; }
      set { _dontDestroyOnLoad = value; }
   }

	/// <summary>Gets and Sets Instance property.</summary>
	public static T Instance
	{
		get
      {
         if(_instance == null)
         {
            _instance = (T)FindObjectOfType(typeof(T));
 
            /*if (_instance == null)
            Debug.LogError("An Instance of " + typeof(T) + " is needed in the scene, but there is none.");*/
         }
 
         return _instance;
      }
	}

   /// <summary>Callback invoked when the MonoBehaviour is instantiated.</summary>
   private void Awake()
   {
      if(Instance != this) Destroy(gameObject);
      else
      {
         if(dontDestroyOnLoad) DontDestroyOnLoad(gameObject);
         OnAwake();
      }
   }

   /// <summary>Callback called on Awake if this Object is the Singleton's Instance.</summary>
   protected virtual void OnAwake() { /*...*/ }
}
}