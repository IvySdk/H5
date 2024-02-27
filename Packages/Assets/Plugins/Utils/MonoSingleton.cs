using System;
using UnityEngine;

namespace Ivy.Utils
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _sInst;

        public static T I
        {
            get
            {
                if (_sInst != null) return _sInst;
                var holderName = typeof(T).FullName + "_holder";
                var holder = GameObject.Find(holderName);
                if (holder == null)
                {
                    holder = new GameObject(holderName);
                    DontDestroyOnLoad(holder);
                }

                _sInst = holder.GetComponent<T>();
                if (_sInst == null)
                {
                    _sInst = holder.AddComponent<T>();
                }

                return _sInst;
            }
        }

        private void Start()
        {
            Init();
        }

        private void OnDestroy()
        {
            DeInit();
        }

        public abstract void Init();

        public virtual void DeInit() { }

        public virtual void OnUpdate() { }
    }   
}
