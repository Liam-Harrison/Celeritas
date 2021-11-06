using Celeritas.Game.Entities;
using UnityEngine;

public class Explosion : MonoBehaviour
{
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

	[SerializeField]
	protected Collider2D shipCollider;

	[SerializeField]
	protected ShipEntity ship;

	private float explosionStart;
	private GameObject tempObject;
	private int currentStage = 0;
	private bool exploding = false;

	public void Explode()
	{
		if (exploding == false)
		{
			exploding = true;
			explosionStart = Time.time;
			ship.IsStationary = true;
			currentStage = 0;
		}

		Vector3 center = ship.Position;
		Vector3 rnd_point;
		int shrapnelPieces;
		int i;
		float timeCheck = Time.time - explosionStart;

		if (timeCheck > 1 * currentStage)
		{
			if (currentStage < explosionsPerStage.Length)
			{
				for (i = 0; i < explosionsPerStage[currentStage]; i++)
				{
					rnd_point = getRandomPoint(shipCollider.bounds.min, shipCollider.bounds.max);
					tempObject = Instantiate(smallExplosion);
					Destroy(tempObject, 4f);
					tempObject.transform.position = rnd_point;
					tempObject.GetComponent<ParticleSystem>().Play(true);
				}
			}
			else if (currentStage == explosionsPerStage.Length)
			{
				tempObject = Instantiate(bigExplosion);
				Destroy(tempObject, 4f);
				tempObject.transform.position = center;
				tempObject.transform.position += new Vector3(0, 0, -5);
				tempObject.GetComponent<ParticleSystem>().Play(true);
				shrapnelPieces = UnityEngine.Random.Range(shrapnelMin, shrapnelMax);
				for (i = 0; i < shrapnelPieces; i++)
				{
					rnd_point = getRandomPoint(shipCollider.bounds.min, shipCollider.bounds.max);
					tempObject = Instantiate(shrapnel[UnityEngine.Random.Range(0, shrapnel.Length - 1)]);
					Destroy(tempObject, 4f);
					tempObject.GetComponent<Rigidbody2D>().AddForce(UnityEngine.Random.insideUnitCircle * 500);
				}
				ship.KillEntity();
			}
			currentStage++;
		}

	}

	public void Initalize()
	{
		currentStage = 0;
		exploding = false;
	}

	private void Update()
	{
		if (exploding)
		{
			Explode();
		}
	}

	private Vector3 getRandomPoint(Vector3 min, Vector3 max)
	{
		float x = UnityEngine.Random.Range(min.x, max.x);
		float y = UnityEngine.Random.Range(min.y, max.y);
		float z = UnityEngine.Random.Range(min.z, max.z);

		return new Vector3(x, y, z - 5);
	}
}