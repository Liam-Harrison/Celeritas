using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleSlam : MonoBehaviour
{
	[SerializeField]
	float ParticleDelay;
	bool IsActive = false;
	float TimeCheck;
    void Update()
    {
		TimeCheck = Time.time - InitialTime;
        if (IsActive && TimeCheck >= ParticleDelay)
		{
			Slam();
		}
    }

	[SerializeField]
	ParticleSystem OriginParticles;
	float InitialTime;
	Vector2 location;
	public void SpawnTentacle(Vector2 loc)
	{
		IsActive = true;
		InitialTime = Time.time;
		location = loc;
		Instantiate(OriginParticles, location, Quaternion.identity);
	}

	[SerializeField]
	GameObject Tentacle;
	private void Slam()
	{
		Instantiate(Tentacle, location, Quaternion.identity);
		//add animation trigger here! Once you figure out how to get at it!!!
	}
}
