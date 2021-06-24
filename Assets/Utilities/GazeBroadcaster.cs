using Assets.Messages;
using System;
using UnityEngine;

namespace Assets.Utilities
{
    public class GazeBroadcaster : MonoBehaviour
    {
        public Camera gazingCamera;

        private GameObject lastGazedUpon;

        void Update()
        {
            CheckGaze();
        }

        private void CheckGaze()
        {
            var gazeRay = new Ray(gazingCamera.transform.position, gazingCamera.transform.rotation * Vector3.forward);

            var wasHit = Physics.Raycast(gazeRay, out var hit, Mathf.Infinity);
            if (wasHit)
            {
                var gazingUponMessage = new GazingUponMessage
                {
                    Gazer = gazingCamera.gameObject,
                    Distance = hit.distance,
                };

                hit.transform.SendMessage("GazingUpon", gazingUponMessage, SendMessageOptions.DontRequireReceiver);
                lastGazedUpon = hit.transform.gameObject;
            }

            if (lastGazedUpon && (!wasHit || hit.transform.gameObject != lastGazedUpon))
            {
                lastGazedUpon.SendMessage("NotGazingUpon", SendMessageOptions.DontRequireReceiver);
                lastGazedUpon = null;
            }
        }
    }
}
