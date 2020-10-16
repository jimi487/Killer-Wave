﻿using UnityEngine.SceneManagement;
using UnityEngine;

public class TitleComponent : MonoBehaviour
{

    void Start() 
    { 
        if (GameManager.playerLives <= 2)
            GameManager.playerLives = 3; 
    }
    
    void Update()
  {
    if (Input.GetMouseButtonUp(0))
    {
      SceneManager.LoadScene("shop");
    }
  }
}