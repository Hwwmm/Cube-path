using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBackground : MonoBehaviour {

    public static RandomBackground Instance;
    public GameObject BackgroundWithShader;
	// Use this for initialization
	void Awake () {
        if (Instance)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
      
        ChangeBackground();
    }
	
	public void ChangeBackground()
    {
        if (!BackgroundWithShader)
        {
            BackgroundWithShader = GameObject.Find("Main Camera/CameraBackground");
        }
        Renderer renderer = BackgroundWithShader.GetComponent<Renderer>();
        Color randomColor = getRandomBackgroundColor();
        //print(randomColor);

       
        renderer.material.SetVector("_ColorHigh", getRandomBackgroundColor());
        
    }

    public Color getRandomBackgroundColor()
    {
        Color[] colors = {  new Color(0.08235294f, 0.09927112f, 0.2235294f, 1.0f),
                            new Color(0.1824856f, 0.08235294f, 0.2235294f, 1.0f),
                            new Color(0.2235294f, 0.08235294f, 0.09215999f, 1.0f),
                            new Color(0.2235294f, 0.2087827f, 0.08235294f, 1.0f),
                            new Color(0.08235294f, 0.2235294f, 0.1012658f, 1.0f),
                            new Color(0.8235294f, 0.2235294f, 0.2235294f, 1.0f),
                            new Color(0.08235294f, 0.1851992f, 0.2235294f, 1.0f),
                            new Color(0.1365176f, 0.122597f, 0.2735849f, 1.0f),
                            new Color(0.04581544f, 0f, 0.3301887f, 1.0f),
                            new Color(0.3962264f, 0f, 0.1021801f, 1.0f),
                            new Color(0f, 0.2358491f, 0.1976704f, 1.0f),
        };
        int idx = Random.Range(0, colors.Length);
        return colors[idx];
    }
}
