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

    private List<float> originalOpacities;
    
    private Material[] mats;
    // Start is called before the first frame update
    void Start()
    {
        originalOpacities = new List<float>();
        mats = GetComponent<Renderer>().materials;
        foreach (Material mat in mats)
        {
            originalOpacities.Add(mat.color.a);
        }
    }

    // Using LateUpdate here to make sure the bDoFade bool is set from FadeCheck before this is run.
    void LateUpdate()
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
        foreach (Material mat in mats)
        {
            Color currentColor = mat.color;
            Color smoothColor = new Color(currentColor.r, currentColor.g, currentColor.b,
                Mathf.Lerp(currentColor.a, fadeAmount, fadeSpeed * Time.deltaTime));
            mat.color = smoothColor;
        }
    }

    void ResetOpacity()
    {
        for(int i = 0; i < mats.Length; i++)
        {
            Color currentColor = mats[i].color;
            Color smoothColor = new Color(currentColor.r, currentColor.g, currentColor.b,
                Mathf.Lerp(currentColor.a, originalOpacities[i], fadeSpeed * Time.deltaTime));
            mats[i].color = smoothColor;
        }
       
    }
}
