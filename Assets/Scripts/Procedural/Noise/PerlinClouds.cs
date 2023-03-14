using UnityEngine;
using System.Collections;

// Create a texture and fill it with Perlin noise.
// Try varying the xOrg, yOrg and scale values in the inspector
// while in Play mode to see the effect they have on the noise.

public class PerlinClouds : MonoBehaviour
{
    public float moveMentSpeed;
    // Width and height of the texture in pixels.
    public int pixWidth;
    public int pixHeight;

    // The origin of the sampled area in the plane.
    public float xOrg;
    public float yOrg;
    public float zOrg;

    public float increment, zIncrement;
    // The number of cycles of the basic noise pattern that are repeated
    // over the width and height of the texture.
    public float scale = 1.0F;

    private Texture2D noiseTex;
    private Color[] pix;
    private Renderer rend;
    public float alpha;
    void Start()
    {
        rend = GetComponent<Renderer>();

        // Set up the texture and a Color array to hold pixels during processing.
        noiseTex = new Texture2D(pixWidth, pixHeight);
        pix = new Color[noiseTex.width * noiseTex.height];
        rend.material.mainTexture = noiseTex;


    }

    void calcNoiseNew() 
    {
         xOrg = 0.0f; // Start xoff at 0

        for (int x = 0; x < noiseTex.width; x++)
        {
            xOrg += increment;   // Increment xoff 
            yOrg = 0.0f;   // For every xoff, start yoff at 0
            for (int y = 0; y < noiseTex.height; y++)
            {
                yOrg += increment; // Increment yoff
                float sample = Perlin.Noise(yOrg, xOrg, zOrg);

                pix[x + y * noiseTex.width] = new Color(0, 0, 0, sample);
            }
        }
        noiseTex.SetPixels(pix);
        noiseTex.Apply();
        zOrg += zIncrement; // Increment zoff
    }


    void Update()
    {

        calcNoiseNew();
    }


}
