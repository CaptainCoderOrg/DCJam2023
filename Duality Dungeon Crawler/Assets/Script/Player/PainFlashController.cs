using UnityEngine;
using System.Collections;

public class PainFlashController : MonoBehaviour
{
    SpriteRenderer _renderer;

    public void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.enabled = false;
    }

    public void ShowPain(Color fromColor, Color toColor)
    {
        if (_isFlashing) { return; }
        StartCoroutine(CoPain(fromColor, toColor));
    }

    private bool _isFlashing = false;

    public void OnEnable()
    {
        _isFlashing = false;
    }

    private IEnumerator CoPain(Color fromColor, Color toColor)
    {
        fromColor.a = 0;
        toColor.a = .75f;
        _isFlashing = true;
        float t = 0;
        _renderer.enabled = true;
        while (t < 1)
        {
            t += .15f;
            _renderer.color = Color.Lerp(fromColor, toColor, t);
            yield return new WaitForSeconds(0.01f);
        }
        while (t > 0)
        {
            t -= .15f;
            _renderer.color = Color.Lerp(fromColor, toColor, t);
            yield return new WaitForSeconds(0.01f);
        }
        _renderer.enabled = false;
        _isFlashing = false;
    }

}