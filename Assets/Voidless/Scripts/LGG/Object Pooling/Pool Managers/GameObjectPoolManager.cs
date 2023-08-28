using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*===========================================================================
**
** Class:  GameObjectPoolManager
**
** Purpose: Singleton class that can contain many GameObject Pools.
** PoolObjectRequesters get their Pool-Objects from this class, so make sure
** both the manager and requesters reference the same type of Pool-Object.
**
**
** Author: Lîf Gwaethrakindo
**
===========================================================================*/
namespace LGG
{
    public class GameObjectPoolManager : MonoBehaviour
    {
        private static GameObjectPoolManager _Instance;

        [SerializeField] private GameObjectPool<PoolGameObject>[] _pools;
        private Dictionary<int, GameObjectPool<PoolGameObject>> _poolsMap;
        private Dictionary<int, PoolObjectRequester<PoolGameObject>> _poolRequesters;

        /// <summary>Gets and Sets Instance property.</summary>
        public static GameObjectPoolManager Instance
        {
            get { return _Instance; }
            private set { _Instance = value; }
        }

        /// <summary>Gets and Sets pools property.</summary>
        public GameObjectPool<PoolGameObject>[] pools
        {
            get { return _pools; }
            set { _pools = value; }
        }

        /// <summary>Gets and Sets poolsMap property.</summary>
        public Dictionary<int, GameObjectPool<PoolGameObject>> poolsMap
        {
            get { return _poolsMap; }
            set { _poolsMap = value; }
        }

        /// <summary>Gets and Sets poolRequesters property.</summary>
        public Dictionary<int, PoolObjectRequester<PoolGameObject>> poolRequesters
        {
            get { return _poolRequesters; }
            set { _poolRequesters = value; }
        }

        /// <summary>GameObjectPoolManager's instance initialization when loaded [Before scene loads].</summary>
        private void Awake()
        {
            /// \TODO Import Singleton<T> class and make this MonoBehaviour inherit from it.
            Instance = this;

            poolsMap = new Dictionary<int, GameObjectPool<PoolGameObject>>();
            poolRequesters = new Dictionary<int, PoolObjectRequester<PoolGameObject>>();

            foreach(GameObjectPool<PoolGameObject> pool in pools)
            {
                pool.Initialize();
                poolsMap.Add(pool.referenceObject.GetInstanceID(), pool);
            }
        }

        /// <summary>Tries to retrieve pool from manager.</summary>
        /// <param name="_reference">Reference Pool-Object [this object's Instance ID ought to be the key in the Dictionary].</param>
        public static GameObjectPool<PoolGameObject> GetPool(PoolGameObject _reference)
        {
            if(_reference == null) return null;

            int ID = _reference.GetInstanceID();
            GameObjectPool<PoolGameObject> pool = null;

            Instance.poolsMap.TryGetValue(ID, out pool);

            if(pool != null) Debug.Log("[GameObjectPoolManager] Current Pool's State: " + pool.ToString());
            return pool;
        }

        /// <summary>Adds PoolObjectRequester to this Manager.</summary>
        /// <param name="_requester">Requester to Add.</param>
        public static void AddRequester(PoolObjectRequester<PoolGameObject> _requester)
        {
            Dictionary<int, GameObjectPool<PoolGameObject>> poolDic = Instance.poolsMap;
            Dictionary<int, PoolObjectRequester<PoolGameObject>> requesterDic = Instance.poolRequesters;
            int poolObjectID = _requester.requestedPoolObject.GetInstanceID();
            int requesterID = _requester.GetInstanceID();

            if(!poolDic.ContainsKey(poolObjectID)) poolDic.Add(poolObjectID, new GameObjectPool<PoolGameObject>(_requester.requestedPoolObject));
            if(!requesterDic.ContainsKey(requesterID)) return;

            requesterDic.Add(requesterID, _requester);
        }

        /// <summary>Removes PoolObjectRequester to this Manager.</summary>
        /// <param name="_requester">Requester to Remove.</param>
        public static void RemoveRequester(PoolObjectRequester<PoolGameObject> _requester)
        {
            Dictionary<int, PoolObjectRequester<PoolGameObject>> requesterDic = Instance.poolRequesters;
            int requesterID = _requester.GetInstanceID();
            requesterDic.Remove(requesterID);
        }

        /// <summary>Adds Pool-Object to a Pool containing the same reproduction reference.</summary>
        /// <param name="_reference">Reproduction reference.</param>
        /// <param name="_instance">Reproduction's instance to add to pool.</param>
        /// <param name="_enqueue">Enqueue (deactivate it and register it to the vacant slots)? true by default, otherwise it will be activated and put into the occupied slots.</param>
        public static void AddPoolObject(PoolGameObject _reference, PoolGameObject _instance, bool _enqueue = true)
        {
            if(_reference == null) return;

            int ID = _reference.GetInstanceID();
            GameObjectPool<PoolGameObject> pool = null;

            if(Instance.poolsMap.TryGetValue(ID, out pool)) return;

            pool.Add(_instance, _enqueue);
        }
    }
}