using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneTransitionSystem
{
public class  STSSceneManager : MonoBehaviour
{
    public static void LoadScene(string _sceneName)
    {
        if (SceneManager.GetSceneByName(_sceneName) != null)
        {
            Debug.Log("loading scene: " + _sceneName);
            SceneManager.LoadScene(_sceneName);
        }
    }
}
}
