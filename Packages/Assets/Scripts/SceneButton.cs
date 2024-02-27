using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Ivy.WeiChat
{
    public class SceneButton : MonoBehaviour
    {
        [SerializeField] private Button btn;
        [SerializeField] private string scenePath;
        //private AsyncOperationHandle _handle;

        //Addressables加载scene示例
        // internal static AsyncOperationHandle _handle;
        private void Start()
        {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() =>
            {
                //保存Addressables加载scene示例 方便之后回收
                // _handle = Addressables.LoadSceneAsync(scenePath);

                SceneManager.LoadSceneAsync(scenePath);

            });
        }

        private void OnDestroy()
        {
            //卸载主scene
            SceneManager.UnloadSceneAsync("Scenes/TopScene");
        }
    }
}


