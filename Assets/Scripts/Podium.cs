using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Podium : MonoBehaviour
{
    [SerializeField] private DogConfiguration _configuration;

    private float timeStamp = 0;
    void Start()
    {
        timeStamp = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - timeStamp > _configuration.prizeGivingDuration)
        {
            SceneManager.LoadScene("Scene_Menu");
        }
    }
}
