using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _backgroundRenderer;
    [SerializeField] private SpriteRenderer _fillRenderer;

    [SerializeField] private float _healthPerUnit = 10f;

    private float _maxHealth;
    private float _fillAmount;
    private bool _visible = true;

    internal void SetVisibility(bool visible)
    {
        _visible = visible;
        Render();
    }

    internal void SetHealth(float newHealth, float maxHealth)
    {
        _fillAmount = Mathf.Clamp01(newHealth / maxHealth);
        _maxHealth = maxHealth;
        Render();
    }

    private void Render()
    {
        _backgroundRenderer.enabled = _visible;
        _fillRenderer.enabled = _visible;

        if (!_visible) { return; }

        var backgroundScaleX = _maxHealth / _healthPerUnit;
        _backgroundRenderer.transform.localScale = _backgroundRenderer.transform.localScale.With(x: backgroundScaleX);

        var fillScaleX = _fillAmount * backgroundScaleX;
        var newFillPositionX = (backgroundScaleX - fillScaleX) / -2f;

        _fillRenderer.transform.localScale = _fillRenderer.transform.localScale.With(x: fillScaleX);
        _fillRenderer.transform.localPosition = _fillRenderer.transform.localPosition.With(x: newFillPositionX);
    }
}
