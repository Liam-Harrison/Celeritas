using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
/// <summary>
/// Screen shake effect.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class ScreenShake : MonoBehaviour
{
	private CinemachineVirtualCamera camera;
	private CinemachineBasicMultiChannelPerlin perlin;
	private AudioSource source;

	[SerializeField]
	private AudioClip shakeSfx;


	private void Awake()
	{
		camera = GetComponent<CinemachineVirtualCamera>();
		perlin = camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
		source = GetComponent<AudioSource>();
	}

	[SerializeField]
	private float intensity;
	private void shake(float intensity)
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
				shake(intensity);
			}
			else
			{
				shake(0f);
				isShaking = false;
			}
		}

	}

	[SerializeField]
	private float time;
	public void triggerShake(float shieldValue)
	{
		if (shieldValue == 0)
		{
			isShaking = true;
			shakeTime = time;
			source.PlayOneShot(shakeSfx);
		}
	}
}
