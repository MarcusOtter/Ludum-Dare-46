
using UnityEngine;

public interface ICircleOnHover
{
    float GetRadius();
    Color GetColour();
}

public class CircleOnMouseOver : MonoBehaviour
{
    private ICircleOnHover _circle;
    [SerializeField] private Transform _circlePrefab;
    private Transform _spawnedCircle;
    private void Start()
    {
        _circle = transform.root.GetComponentInChildren<ICircleOnHover>();
        _spawnedCircle = Instantiate(_circlePrefab, transform);
        _spawnedCircle.localScale = new Vector2(0f, 0f);
    }

    private void OnMouseEnter()
    {
    //    LeanTween.cancel(gameObject);
        LeanTween.scale(_spawnedCircle.gameObject, new Vector2(1f, 1f) * _circle.GetRadius() * 2, 0.1f);
    }

    private void OnMouseExit()
    {
    //    LeanTween.cancel(gameObject);

        LeanTween.scale(_spawnedCircle.gameObject, Vector2.zero, 0.1f);
    }
}
