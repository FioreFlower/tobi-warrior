using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    public float scrollSpeed;
    public MeshRenderer meshRenderer;

    bool _isFlying = false;
    public void ScrollingBackground()
    {
        meshRenderer.material.mainTextureOffset += new Vector2(scrollSpeed*Time.deltaTime, 0);
    }

    
}
