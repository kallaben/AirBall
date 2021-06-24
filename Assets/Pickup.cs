using Assets.Messages;
using Assets.Receivers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour, IGazeReceiver
{
    public float PickupDistance = 20f;
    private bool pickedUp = false;
    private GameObject pickedUpBy;
    public float MoveForce = 250f;
    public float ThrowForce = 250f;

    public void GazingUpon(GazingUponMessage message)
    {
        if (ObjectIsGettingPickedUp(message))
        {
            PickupObject(message);
        }
    }

    private void PickupObject(GazingUponMessage message)
    {
        pickedUp = true;
        pickedUpBy = message.Gazer;

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
        pickedUp = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse1) && pickedUp)
        {
            DropObject();
        } 
        else if (Input.GetKey(KeyCode.E) && pickedUp)
        {
            ThrowObject();
        }
        else if (pickedUp)
        {
            MovePickedUpObject();
        }
    }

    private void ThrowObject()
    {
        var throwVector = pickedUpBy.transform.forward * ThrowForce;
        GetComponent<Rigidbody>().AddForce(throwVector);
        DropObject();
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
        var pointInFrontOfHolder = pickedUpBy.transform.position + pickedUpBy.transform.forward * PickupDistance;

        if (Vector3.Distance(transform.position, pointInFrontOfHolder) > 0.1f)
        {
            var moveDirection = pointInFrontOfHolder - transform.position;
            GetComponent<Rigidbody>().AddForce(moveDirection * MoveForce);
        }
    }
}
