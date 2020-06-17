﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavePreviewManager : MonoBehaviour
{
    #region Public Variables

    public float moduleSize = 175.0f;
    public float offset = -15.0f;
    
    #endregion

    
    #region Serializable Variables

    [SerializeField] private Animator animator;
    [SerializeField] private RectTransform content;
    [SerializeField] private List<GameObject> modules = new List<GameObject>();
    [SerializeField] private List<int> waves = new List<int>();
    
    #endregion
    
    
    #region Private Variables
    #endregion

    
    #region MonoBeaviour
    
    void Start()
    {
        
        foreach (GameObject module in modules)
        {
            module.SetActive(false);
        }

        NextWave(15);
    }

    void Update()
    {
        
    }

    #endregion
    
    
    #region Public Methods
    public void Open()
    {
        animator.SetTrigger("open");
    }

    public void Close()
    {
        animator.SetTrigger("close");
        
    }

    public void NextWave(int wave)
    {

        for (int i = 0; i < modules.Count && i < waves.Count; i++)
        {
            if (waves[i] > wave)
                break;
            
            content.sizeDelta = new Vector2(600, (moduleSize + 5) * (i + 1) + 10); 
            
            modules[i].SetActive(true);
            Debug.Log("before : " + modules[i].transform.localPosition);
            modules[i].transform.localPosition = new Vector3(modules[i].transform.localPosition.x, offset - moduleSize * i , modules[i].transform.localPosition.z);
            Debug.Log("after : " + modules[i].transform.localPosition);
        }
        
    }

    #endregion
    
}
