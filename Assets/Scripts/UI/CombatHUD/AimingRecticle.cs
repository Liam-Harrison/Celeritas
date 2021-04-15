using Celeritas.Game.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AimingRecticle : MonoBehaviour
{
	private Color colour; // color of rendered aiming line

	private Material material;

	public Material Material { get => material; set => material = value; }

	public Color Colour { get => colour; set => colour = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void Draw(ShipEntity playerShip)
	{
		DrawLineBetweenTwoPoints(playerShip.transform.position, Mouse.current.position.ReadValue());
	}

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
	}
}
