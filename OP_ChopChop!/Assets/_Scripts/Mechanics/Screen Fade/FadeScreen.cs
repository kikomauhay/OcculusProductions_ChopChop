using System.Collections;
using UnityEngine;

public class FadeScreen : MonoBehaviour
{
    #region Members

    public float FadeDuration => _fadeDuration;

    [SerializeField] bool _fadeOnStart;
    [SerializeField] float _fadeDuration;
    [SerializeField] Color _fadeColor;

    Renderer _renderer;

    #endregion

    void Start()
    {
        _renderer = GetComponent<Renderer>();

        if (_fadeOnStart)
            FadeIn();
    }

    #region Fading

    public void FadeIn() => Fade(1f, 0f);
    public void FadeOut() => Fade(0f, 1f);

    void Fade(float alphaIn, float alphaOut) => 
        StartCoroutine(FadeRoutine(alphaIn, alphaOut));
    
    IEnumerator FadeRoutine(float alphaIn, float alphaOut)
    {
        float timer = 0f;

        while (timer <= _fadeDuration)
        {
            // slowly sets the fade-in color 
            Color newColor = _fadeColor;
            newColor.a = Mathf.Lerp(alphaIn, alphaOut, timer / _fadeDuration);
            _renderer.material.SetColor("_Color", newColor);

            // increases opacity per frame
            timer += Time.deltaTime;
            yield return null; 
        }

        // resets the transparency back to normal
        Color col = _fadeColor;
        col.a = alphaOut;
        _renderer.material.SetColor("_Color", col);
        this.gameObject.SetActive(false);
    } 

#endregion
}
