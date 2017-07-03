using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyControlCarWithWheel : MonoBehaviour
{
	[SerializeField] private WheelJoint2D[] wheels;
	[SerializeField] private float maxSpeed;
	[SerializeField] private float acceleration;
	private Vector2 groundDirection;
	private new Rigidbody2D[] rigidbody;
	// Use this for initialization
	void Start()
	{
		rigidbody = new Rigidbody2D[wheels.Length];
		for (var i = 0; i < wheels.Length; i++)
		{
			rigidbody[i] = wheels[i].GetComponent<Rigidbody2D>();
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.anyKeyDown)
		{
			if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				for (var i = 0; i < wheels.Length; i++)
				{
					wheels[i].useMotor = true;
					wheels[i].motor = new JointMotor2D()
					{
						motorSpeed = -rigidbody[i].angularVelocity,
						maxMotorTorque = wheels[i].motor.maxMotorTorque
					};
				}
			}
			if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				for (var i = 0; i < wheels.Length; i++)
				{
					wheels[i].useMotor = true;
					wheels[i].motor = new JointMotor2D()
					{
						motorSpeed = -rigidbody[i].angularVelocity,
						maxMotorTorque = wheels[i].motor.maxMotorTorque
					};
				}
			}
		}
		if (Input.anyKey)
		{
			if (Input.GetKey(KeyCode.LeftArrow))
			{
				foreach (var i in wheels)
				{
					i.useMotor = true;
					i.motor = new JointMotor2D()
					{
						motorSpeed = (i.motor.motorSpeed - acceleration) > -maxSpeed ? i.motor.motorSpeed - acceleration : -maxSpeed,
						maxMotorTorque = i.motor.maxMotorTorque
					};
				}
			}
			if (Input.GetKey(KeyCode.RightArrow))
			{
				foreach (var i in wheels)
				{
					i.useMotor = true;
					i.motor = new JointMotor2D()
					{
						motorSpeed = (i.motor.motorSpeed + acceleration) < maxSpeed ? i.motor.motorSpeed + acceleration : maxSpeed,
						maxMotorTorque = i.motor.maxMotorTorque
					};
				}
			}
		}
		else
		{
			foreach (var i in wheels)
			{
				i.useMotor = false;
				if (i.motor.motorSpeed > 0)
				{
					i.motor = new JointMotor2D()
					{
						motorSpeed = (i.motor.motorSpeed - acceleration / 2) > 0f ? i.motor.motorSpeed - acceleration / 2 : 0f,
						maxMotorTorque = i.motor.maxMotorTorque
					};
				}
				else if (i.motor.motorSpeed < 0)
				{
					i.motor = new JointMotor2D()
					{
						motorSpeed = (i.motor.motorSpeed + acceleration / 2) < 0f ? i.motor.motorSpeed + acceleration / 2 : 0f,
						maxMotorTorque = i.motor.maxMotorTorque
					};
				}
			}
		}
	}
}
