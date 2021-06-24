using Assets.Interfaces;
using UnityEngine;

public class Pickup : MonoBehaviour, IPickup
{
    public float PickupDistance = 20f;
    public float MoveForce = 250f;
    public float ThrowForce = 250f;
    private GameObject pickedUpObject;

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Mouse0) && pickedUpObject == null && HasPickupableObjectWithinRange(out var objectToPickup))
        {
            PickupObject(objectToPickup);
        }
        else if (Input.GetKey(KeyCode.Mouse1) && pickedUpObject != null)
        {
            DropObject(pickedUpObject);
        } 
        else if (Input.GetKey(KeyCode.E) && pickedUpObject != null)
        {
            ThrowObject();
        }
        else if (pickedUpObject != null)
        {
            MovePickedUpObject();
        }
    }

    private void PickupObject(GameObject objectToPickup)
    {
        var rigidbody = objectToPickup.GetComponent<Rigidbody>();
        rigidbody.useGravity = false;
        rigidbody.drag = 10;
        rigidbody.transform.parent = transform;
        pickedUpObject = objectToPickup;
    }

    private bool HasPickupableObjectWithinRange(out GameObject pickupableObject)
    {
        pickupableObject = null;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out var hit, PickupDistance) && 
            hit.transform.gameObject.GetComponent<Rigidbody>() != null)
        {
            pickupableObject = hit.transform.gameObject;
            return true;
        }

        return false;
    }

    private void ThrowObject()
    {
        var throwVector = transform.forward * ThrowForce;
        pickedUpObject.GetComponent<Rigidbody>().AddForce(throwVector);
        DropObject(pickedUpObject);
    }

    private void DropObject(GameObject obj)
    {
        var rigidbody = obj.GetComponent<Rigidbody>();
        rigidbody.useGravity = true;
        rigidbody.drag = 1;
        rigidbody.transform.parent = null;
        pickedUpObject = null;
    }

    private void MovePickedUpObject()
    {
        var pointInFrontOfHolder = transform.position + transform.forward * PickupDistance;

        if (Vector3.Distance(pickedUpObject.transform.position, pointInFrontOfHolder) > 0.1f)
        {
            var moveDirection = pointInFrontOfHolder - pickedUpObject.transform.position;
            pickedUpObject.GetComponent<Rigidbody>().AddForce(moveDirection * MoveForce);
        }
    }

    public bool IsHoldingObject()
    {
        return pickedUpObject != null;
    }
}
