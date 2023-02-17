using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public KeyCode resetKey;
    private Vector3 startPos;
    private CharacterController c;

    private void Awake()
    {
        c = GetComponent<CharacterController>();
    }

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(resetKey))
        {
            c.enabled = false;
            transform.position = startPos;
            c.enabled = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        transform.parent = collision.gameObject.transform;
    }

    private void OnCollisionExit(Collision other)
    {
        transform.parent = null;

    }
}
