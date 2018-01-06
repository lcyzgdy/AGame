using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeControl : MonoBehaviour
{
	[SerializeField] private GameObject cubePrefab;
	private NextDirection nextDirection;

	// Use this for initialization
	void Start()
	{
		CSharpCodeProvider a = new CSharpCodeProvider();
	}

	// Update is called once per frame
	void Update()
	{
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (Random.Range(1f, 100f) > 50f)
		{
			nextDirection = NextDirection.Left;
		}
		else
		{
			nextDirection = NextDirection.Up;
		}

	}

	enum NextDirection
	{
		Left,
		Up
	};
}
