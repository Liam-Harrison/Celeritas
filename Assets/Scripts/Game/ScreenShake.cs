using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ScreenShake : MonoBehaviour
{
	private CinemachineVirtualCamera camera;
	private CinemachineBasicMultiChannelPerlin perlin;

	private void Awake()
	{
		camera = GetComponent<CinemachineVirtualCamera>();
		perlin = camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
	}

	public float intensity;
	private void shakeThatScreen(float intensity)
	{

		perlin.m_AmplitudeGain = intensity;
	}


	private bool isShaking;
	private float shakeTime;
	private void Update()
	{

		if (isShaking)
		{
			shakeTime -= Time.deltaTime;

			if (shakeTime > 0)
			{
				shakeThatScreen(intensity);
			}
			else
			{
				shakeThatScreen(0f);
				isShaking = false;
			}
		}

	}

	public float time;
	public void triggerShake(float shieldValue)
	{
		if (shieldValue == 0)
		{
			isShaking = true;
			shakeTime = time;
		}
	}
}
