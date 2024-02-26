using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Ivy.WeiChat
{
    public class BackButton : MonoBehaviour
    {

        [SerializeField] private Button btn;
        private const string sceneName = "Scenes/TopScene";
        private void Start()
        {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() =>
            {
                // StartCoroutine(LoadAsyncScene());
            });
        }

        // private IEnumerator LoadAsyncScene()
        // {
        // var asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        // asyncLoad.completed += op =>
        // {
        //     if (SceneButton._handle.IsValid())
        //     {
        //         Addressables.UnloadSceneAsync(SceneButton._handle);
        //     }
        // };
        // yield return asyncLoad;
        // }

    }
}


