using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class AutoCharacterPlacer : MonoBehaviour
{
    public GameObject characterPrefab;
    private GameObject spawnedCharacter;

    private ARRaycastManager raycastManager;
    private bool hasPlaced = false;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Awake()
    {
        raycastManager = FindObjectOfType<ARRaycastManager>();
    }

    void Update()
    {
        if (hasPlaced)
            return;

        if (raycastManager.Raycast(new Vector2(Screen.width / 2f, Screen.height / 2f), hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;

            spawnedCharacter = Instantiate(characterPrefab, hitPose.position, hitPose.rotation);
            hasPlaced = true;
                        
            //ancla el pj , revisar
            var anchor = spawnedCharacter.AddComponent<ARAnchor>();
            Debug.Log("character placed and anchored");//test

            //ocultar planos y desactivar plane manager revisar
            var planeManager = FindObjectOfType<ARPlaneManager>();
            if (planeManager != null)
            {
                planeManager.enabled = false;

                foreach (var plane in planeManager.trackables)
                {
                    plane.gameObject.SetActive(false);
                }
            }
        }
    }
}
