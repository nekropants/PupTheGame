using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] private DogConfiguration _configuration;
    [SerializeField] private Transform _playButton;
    private bool _starting = false;
    public void StartGame()
    {
        if (_starting == false)
        {
            _starting = true;
            StartCoroutine(IEStartGame());
        }
    }
    
    public IEnumerator IEStartGame()
    {
        _playButton.localScale *= 1.3f;
        yield return new WaitForSeconds(0.1f);
        SceneReference level = _configuration.levels[Random.Range(0, _configuration.levels.Length)];
        _configuration.activeLevel = level;
        SceneManager.LoadScene(level.SceneName);
    }
}
