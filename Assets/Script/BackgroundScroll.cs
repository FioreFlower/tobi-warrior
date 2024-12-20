using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    public float scrollSpeed;
    public MeshRenderer meshRenderer;

    bool isFlying = false;
    void ScrollingBackground()
    {
        meshRenderer.material.mainTextureOffset += new Vector2(scrollSpeed*Time.deltaTime, 0);
    }

    void Update()
    {
        if (isFlying)
        {
            ScrollingBackground();
        }
    }
}
