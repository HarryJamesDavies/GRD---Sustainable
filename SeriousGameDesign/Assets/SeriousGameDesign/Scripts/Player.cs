﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Pawn
{
    public float m_speedForce = 1.0f;
    public float m_rotationCoefficient = 1.0f;
    public float m_rotationMinimum = 10.0f;

    public float m_brakingCoefficient = 1.0f;

    private Vector3 m_force = Vector3.zero;
    public float m_torque = 0.0f;

    private Rigidbody2D m_rigidBody;

    public Vector2 m_forward = new Vector2(1.0f, 0.0f);

    private float m_defaultDrag = 0.1f;

    public float m_rotationSign = 0.0f;

    new
    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_defaultDrag = m_rigidBody.drag;

        m_forward = new Vector2(1.0f, 0.0f);

        base.Start();
    }

    void Update()
    {
        if (m_inputMap)
        {
            Movement();
        }
    }

    void Movement()
    {
        float deadzone = 0.25f;

        Vector2 stickInput = new Vector2(Input.GetAxis(m_inputMap.GetInput("Horizontal")), -Input.GetAxis(m_inputMap.GetInput("Vertical")));
        if (stickInput.magnitude < deadzone)
        {
            stickInput = Vector2.zero;
            m_rotationSign = 0.0f;
            m_torque = 0.0f;
        }
        else
        {
            stickInput = stickInput.normalized * ((stickInput.magnitude - deadzone) / (1 - deadzone));

            float currentAngle = gameObject.transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
            Vector2 currentDirection = Vector2.zero;
            currentDirection.x = Mathf.Cos(currentAngle);
            currentDirection.y = Mathf.Sin(currentAngle);

            Vector3 cross = Vector3.Cross(stickInput, currentDirection);
            m_rotationSign = Mathf.Sign(cross.z);

            float rotationAngle = Vector2.Angle(stickInput, currentDirection);
            rotationAngle *= m_rotationSign;

            m_torque = 0.0f;
            if (Mathf.Abs(rotationAngle) > m_rotationMinimum)
            {
                m_torque = -m_rotationSign * Mathf.Abs(rotationAngle) * m_rotationCoefficient;
            }
        }

        m_force = m_speedForce * m_forward * ((Input.GetAxis(m_inputMap.GetInput("Accelerate")) + 1.0f) * 0.5f);

        m_rigidBody.drag = (((Input.GetAxis(m_inputMap.GetInput("Deccelerate")) + 1.0f) * 0.5f) * m_brakingCoefficient) + m_defaultDrag;
    }

    void FixedUpdate()
    {
        m_rigidBody.AddForce(m_force, ForceMode2D.Impulse);
        m_rigidBody.AddTorque(m_torque);

        float currentAngle = gameObject.transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
        m_forward.x = Mathf.Cos(currentAngle);
        m_forward.y = Mathf.Sin(currentAngle);
        m_forward.Normalize();
    }
}
