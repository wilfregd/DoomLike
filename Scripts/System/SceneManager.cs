using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
    [SerializeField] private Slider _loadingSlider;
    private AsyncOperation _async = null;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) //DEBUG REMOVE THIS
        {
            string currentLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            StartCoroutine(LoadLevelAsync(currentLevel));
        }
    }

    private IEnumerator LoadLevelAsync(string levelName)
    {
        _async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(levelName);

        while (!_async.isDone)
        {
            _loadingSlider.value = _async.progress;
            yield return null;
        }
        
    }
}
