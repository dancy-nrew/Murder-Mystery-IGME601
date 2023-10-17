using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Adapted from https://www.youtube.com/watch?v=mOqHVMS7-Nw
public class ObjectFader : MonoBehaviour
{
    public bool bDoFade;

    [SerializeField]
    private float fadeSpeed = 10f;
    [SerializeField]
    private float fadeAmount = 0.5f;

    private float originalOpacity;
    
    private Material material;
    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<Renderer>().material;
        originalOpacity = material.color.a;
    }

    // Update is called once per frame
    void Update()
    {
        if(bDoFade)
        {
            FadeObject();
        }
        else
        {
            ResetOpacity();
        }
    }

    void FadeObject()
    {
        Color currentColor = material.color;
        Color smoothColor = new Color(currentColor.r, currentColor.g, currentColor.b, 
            Mathf.Lerp(currentColor.a, fadeAmount, fadeSpeed * Time.deltaTime));
        material.color = smoothColor;
    }

    void ResetOpacity()
    {
        Color currentColor = material.color;
        Color smoothColor = new Color(currentColor.r, currentColor.g, currentColor.b, 
            Mathf.Lerp(currentColor.a, originalOpacity, fadeSpeed * Time.deltaTime));
        material.color = smoothColor;
    }
}
