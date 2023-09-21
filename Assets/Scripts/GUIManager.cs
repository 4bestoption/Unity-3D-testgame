using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIManager : MonoBehaviour
{


    public GameObject backgroundImage = null;
    public GameObject gameOverText = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowGameOver ()
    {
        backgroundImage.SetActive(true);
        gameOverText.SetActive(true);

        
        
    }

}
