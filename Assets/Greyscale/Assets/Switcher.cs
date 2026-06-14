using UnityEngine;

public class GrayscaleToggle : MonoBehaviour
{
    [SerializeField] private Material grayscaleMaterial; // the Fullscreen SG material
    [SerializeField] private float    fadeSpeed = 8f;    // set high for instant, low for a fade
    private                  bool     isGray    = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
            isGray = !isGray;

        float current = grayscaleMaterial.GetFloat("_Blend");
        float target  = isGray ? 1f : 0f;
        grayscaleMaterial.SetFloat("_Blend", Mathf.MoveTowards(current, target, fadeSpeed * Time.unscaledDeltaTime));
    }
}