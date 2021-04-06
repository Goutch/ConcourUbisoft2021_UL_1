using System;
using System.Collections;
using System.Collections.Generic;
using Arm;
using UnityEngine;

public class MagnetIndicator : MonoBehaviour
{
	[SerializeField] private float emmisionIntensity = 10.0f;
	[SerializeField] private Color grabbedColor = Color.green;
	[SerializeField] private Color magnetActiveColor = Color.red;
	[SerializeField] private Color magnetInactiveColor = Color.black;
	[SerializeField] private MagnetController magnet;
	private Renderer _renderer;

	private void Start()
	{
		_renderer = GetComponent<Renderer>();
	}

	private void OnEnable()
	{
		magnet.OnMagnetActiveChange += HandleMagnetStateChange;
		magnet.OnGrabStateChange += HandleMagnetStateChange;
	}

	private void OnDisable()
	{
		magnet.OnMagnetActiveChange -= HandleMagnetStateChange;
		magnet.OnGrabStateChange -= HandleMagnetStateChange;
	}

	private void HandleMagnetStateChange()
	{
		if (magnet.MagnetActive)
		{
			if (magnet.Grabbed)
			{
				_renderer.material.color = grabbedColor;
				_renderer.material.SetColor("_EmissionColor", grabbedColor * emmisionIntensity);
				_renderer.material.EnableKeyword("_EMISSION");
			}
			else
			{
				_renderer.material.color = magnetActiveColor;
				_renderer.material.SetColor("_EmissionColor", magnetActiveColor * emmisionIntensity);
				_renderer.material.EnableKeyword("_EMISSION");
			}
		}
		else
		{
			_renderer.material.color = magnetInactiveColor;
			_renderer.material.SetColor("_EmissionColor", magnetInactiveColor * emmisionIntensity);
			_renderer.material.DisableKeyword("_EMISSION");
		}
	}
}