using Assets.Messages;
using Assets.Receivers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour, IGazeReceiver
{
    public float PickupDistance = 20f;
    private bool pickedUp = false;
    private GameObject pickedUpBy;

    public float moveForce = 250f;

    public void GazingUpon(GazingUponMessage message)
    {
        Debug.Log("Is gazing upon. Distance: " + message.Distance + " KeyCode.Mouse0: " + Input.GetKey(KeyCode.Mouse0) + " pickedUpBy: " + message.Gazer.name);
        if (ObjectIsGettingPickedUp(message))
        {
            PickupObject(message);
        }
    }

    private void PickupObject(GazingUponMessage message)
    {
        pickedUp = true;
        pickedUpBy = message.Gazer;
        Debug.Log("Is PickedUp");
        var rigidbody = GetComponent<Rigidbody>();
        rigidbody.useGravity = false;
        rigidbody.drag = 10;
        rigidbody.transform.parent = pickedUpBy.transform;
    }

    private bool ObjectIsGettingPickedUp(GazingUponMessage message)
    {
        return message.Distance <= PickupDistance && Input.GetKey(KeyCode.Mouse0) && !pickedUp;
    }

    public void NotGazingUpon()
    {
        Debug.Log("Is not gazing upon.");
        pickedUp = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse1) && pickedUp)
        {
            DropObject();
        }
        else if (pickedUp)
        {
            MovePickedUpObject();
        }
    }

    private void DropObject()
    {
        pickedUp = false;
        pickedUpBy = null;
        var rigidbody = GetComponent<Rigidbody>();
        rigidbody.useGravity = true;
        rigidbody.drag = 1;
        rigidbody.transform.parent = null;
    }

    private void MovePickedUpObject()
    {
        if (Vector3.Distance(transform.position, pickedUpBy.transform.position) > 0.1f)
        {
            var moveDirection = pickedUpBy.transform.position - transform.position;
            GetComponent<Rigidbody>().AddForce(moveDirection * moveForce);
        }
    }
}
