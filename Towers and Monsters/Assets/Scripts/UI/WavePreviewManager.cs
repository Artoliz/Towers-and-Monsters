using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavePreviewManager : MonoBehaviour
{
    
    public Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Open()
    {
        animator.SetTrigger("open");
    }

    public void Close()
    {
        animator.SetTrigger("close");
        
    }
}
