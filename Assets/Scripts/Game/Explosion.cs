using Celeritas.Game;
using Celeritas.Game.Controllers;
using Celeritas.Game.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
	private bool exploding = false;
	private void Awake()
	{
		//exploding = false;
	}

	//blah blah add enemy options here
	//[SerializeField]
	//bool isEnemy;


	[SerializeField]
	protected GameObject smallExplosion;
	[SerializeField]
	protected GameObject bigExplosion;
	[SerializeField]
	protected GameObject[] shrapnel;
	[SerializeField]
	protected int shrapnelMin;
	[SerializeField]
	protected int shrapnelMax;
	[SerializeField]
	protected int[] explosionsPerStage;

	private ShipEntity ship;
	private Collider2D shipCollider;
	private float explosionStart;
	private GameObject tempObject;
	private int currentStage = 0;
	public void Explode(ShipEntity ship)
	{
		if (exploding == false)
		{
			exploding = true;
			explosionStart = Time.time;
			shipCollider = ship.GetComponentInChildren<Collider2D>();
			//disable wsad etc
		}
	
		Vector3 center = ship.Position;
		Vector3 rnd_point;
		int shrapnelPieces;
		int i;
		float timeCheck = Time.time - explosionStart;

		if (timeCheck > 1*currentStage)
		{
			if (currentStage < explosionsPerStage.Length)
			{
				for (i = 0; i < explosionsPerStage[currentStage]; i++)
				{
					rnd_point = getRandomPoint(shipCollider.bounds.min, shipCollider.bounds.max);
					tempObject = Instantiate(smallExplosion);
					tempObject.transform.position = rnd_point;
					tempObject.GetComponent<ParticleSystem>().Play(true);

				}
			}
			else if (currentStage == explosionsPerStage.Length)
			{
				tempObject = Instantiate(bigExplosion);
				tempObject.transform.position = center;
				tempObject.transform.position += new Vector3(0, 0, -5);
				tempObject.GetComponent<ParticleSystem>().Play(true);
				shrapnelPieces = UnityEngine.Random.Range(shrapnelMin, shrapnelMax);
				for (i = 0; i < shrapnelPieces; i++)
				{
					rnd_point = getRandomPoint(shipCollider.bounds.min, shipCollider.bounds.max);
					tempObject = Instantiate(shrapnel[UnityEngine.Random.Range(0,shrapnel.Length-1)]);
					tempObject.GetComponent<Rigidbody2D>().AddForce(UnityEngine.Random.insideUnitCircle *500);
				}
				ship.KillEntity();
			}
			else
			{
				exploding = false;
			}
			currentStage++;
		}

	}

	private void Update()
	{
		//Debug.Log("Is this thing on?");

		if (exploding)
		{
			Explode(ship);
		}
	}

	private Vector3 getRandomPoint(Vector3 min, Vector3 max)
	{
		float x = UnityEngine.Random.Range(min.x, max.x);
		float y = UnityEngine.Random.Range(min.y, max.y);
		float z = UnityEngine.Random.Range(min.z, max.z);

		return new Vector3(x, y, z-5);
	}
}