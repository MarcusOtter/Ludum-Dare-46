using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
	internal static PlayerInput Instance { get; private set; }

	internal event Action<PlayerInput> OnAttackKeyDown;
	internal event Action<PlayerInput> OnAttackKeyUp;

	internal float HorizontalAxis { get; private set; }
	internal float VerticalAxis { get; private set; }
	internal Vector3 MouseWorldPosition { get; private set; }

	[SerializeField] private KeyCode _attackKey = KeyCode.Mouse0;
	[SerializeField] private string _horizontalAxisName = "Horizontal";
	[SerializeField] private string _verticalAxisName = "Vertical";

	private Camera _mainCamera;

	private void Awake()
	{
		if (!IsAloneSingleton()) { return; }
		_mainCamera = Camera.main;
	}

	private void Update()
	{
		MouseWorldPosition = GetMouseWorldPosition();

		if (Input.GetKeyDown(_attackKey))
		{
			OnAttackKeyDown?.Invoke(this);
		}

		if (Input.GetKeyUp(_attackKey))
		{
			OnAttackKeyUp?.Invoke(this);
		}

		HorizontalAxis = Input.GetAxis(_horizontalAxisName);
		VerticalAxis = Input.GetAxis(_verticalAxisName);
	}

	private Vector3 GetMouseWorldPosition()
	{
		return _mainCamera.ScreenToWorldPoint(Input.mousePosition);
	}

	private bool IsAloneSingleton()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(transform.root.gameObject);
			return true;
		}

		Destroy(gameObject);
		return false;
	}
}
