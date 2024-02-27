using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Ivy.WeiChat{
    
    public class RotateBehaviour : MonoBehaviour
    {
        private readonly Vector3 axis = new ()
        {
            x = 0,
            y = 1,
            z = 0
        };
        
        void Update()
        {
            transform.Rotate(axis, Time.deltaTime*60);
        }

        private void OnEnable()
        {
            Debug.Log($"RotateBehaviour OnEnable");
        }

        private void OnDisable()
        {
            Debug.Log($"RotateBehaviour OnDisable");
        }
    }
}

