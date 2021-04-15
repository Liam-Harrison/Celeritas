using Celeritas.Game.Controllers;
using Celeritas.Game.Entities;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Draws a line between the player ship and the mouse.
/// </summary>
public class AimingLine : MonoBehaviour
{
	private LineRenderer line;

	[SerializeField, Title("To allow line to intersect centre of cursor")]
	private float xDisplacementFromCursorCentre;

	[SerializeField]
	private float yDisplacementFromCursonCentre;

    // Start is called before the first frame update
    void Start()
    {
		line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
		if (PlayerController.Instance!=null)
			Draw(PlayerController.Instance.ShipEntity);
    }

	public void Draw(ShipEntity playerShip)
	{
		// draw using line renderer
		line.SetPosition(0, playerShip.transform.position);
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
		mousePosition.z = line.transform.position.z;
		// these additions are for the aiming sprite that is being used.
		mousePosition.x += xDisplacementFromCursorCentre;
		mousePosition.y += yDisplacementFromCursonCentre;
		line.SetPosition(1, mousePosition);
		//DrawLineBetweenTwoPoints(playerShip.transform.position, Mouse.current.position.ReadValue());
	}

	/*
	private void DrawLineBetweenTwoPoints(Vector3 start, Vector3 end)
	{
		// From https://docs.unity3d.com/ScriptReference/GL.LINES.html

		GL.PushMatrix();
		material.SetPass(0);
		GL.LoadOrtho();

		GL.Begin(GL.LINES);
		GL.Color(colour);
		GL.Vertex(start);
		GL.Vertex(new Vector3(end.x / Screen.width, end.y / Screen.height, 0));
		GL.End();

		GL.PopMatrix();
	}*/
}
