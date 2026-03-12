using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool OllieSaved {get; set;}
    public bool BaileySaved {get; set;}
    public bool isKai {get; set;}
    public bool isOllie {get; set;}
    public bool isBailey{get; set;}

    private void Awake()
    {
        if (instance == null)
        instance = this;
        OllieSaved = false;
        BaileySaved = false;
        isKai = true;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
