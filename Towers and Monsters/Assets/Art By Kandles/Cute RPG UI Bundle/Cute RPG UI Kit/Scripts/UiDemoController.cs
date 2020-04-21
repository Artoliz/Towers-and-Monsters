namespace ArtByKandles
{
	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine;
	using UnityEngine.UI;

	public sealed class UiDemoController : MonoBehaviour
	{
		[SerializeField] private RectTransform _container = null;

		[SerializeField] private Button _next = null;
		[SerializeField] private Button _previous = null;

		private int _panel;

		private void Start()
		{
			if(_container == null || _next == null || _previous == null)
			{
				Debug.LogError("Demo scene not configured");

				return;
			}

			_next.onClick.AddListener(OnNext);
			_previous.onClick.AddListener(OnPrevious);

			Refresh();		
		}

		private void Refresh()
		{
			for (int i = 0; i < _container.childCount; ++i)
				_container.GetChild(i).gameObject.SetActive(i == _panel); 
		}

		private void OnNext()
		{
			_panel = Mathf.Clamp(_panel + 1, 0, _container.childCount - 1);

			Refresh();
		}

		private void OnPrevious()
		{
			_panel = Mathf.Clamp(_panel - 1, 0, _container.childCount - 1);

			Refresh();
		}
		
		private void Update()
		{
		
		}
	}
}