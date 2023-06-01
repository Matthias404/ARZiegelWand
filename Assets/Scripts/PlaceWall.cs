using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;

public class PlaceWall : MonoBehaviour
{
    //Make ArRaycast Manager and ArPlaneManager accessible and  object on click 

    [SerializeField]
    public TextMeshProUGUI text;

    bool objectIsPlaced = false;

    GameObject placedObject;

    //Make This Object change size with ARRaycastManager
    [SerializeField]
    GameObject m_ClickBoxPrefab;

    public GameObject ClickBoxPrefab
    {
        get => m_ClickBoxPrefab;
        set => m_ClickBoxPrefab = value;
    }

    // Removes all the anchors that have been created.
    public void RemoveAllAnchors()
    {
        foreach (var anchor in m_AnchorPoints)
        {
            Destroy(anchor);
        }
        m_AnchorPoints.Clear();
    }

    // On Awake(), we obtains a reference to all the required components.
    // The ARRaycastManager allows us to perform raycasts so that we know where to place an anchor.
    // The ARPlaneManager detects surfaces we can place our objects on.
    // The ARAnchorManager handles the processing of all anchors and updates their position and rotation.

    void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
        m_AnchorManager = GetComponent<ARAnchorManager>();
        m_PlaneManager = GetComponent<ARPlaneManager>();
        m_AnchorPoints = new List<ARAnchor>();
    }

    void Update()
    {
        // If there is no tap, then simply do nothing until the next call to Update().
        if (Input.touchCount == 0)
            return;

        var touch = Input.GetTouch(0);
        if (touch.phase != TouchPhase.Began)
            return;

        if (m_RaycastManager.Raycast(touch.position, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            // Raycast hits are sorted by distance, so the first one
            // will be the closest hit.
            var hitPose = s_Hits[0].pose;

            // Create a new anchor.
            var anchorObject = m_AnchorManager.AddAnchor(hitPose);

            

                placedObject = Instantiate(m_ClickBoxPrefab, hitPose.position, hitPose.rotation);

                // Make the new object a child of the anchor.
                placedObject.transform.parent = anchorObject.transform;

                // Record the anchor so we can remove it later.
                m_AnchorPoints.Add(anchorObject);
                
            

        }

    }

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    ARRaycastManager m_RaycastManager;

    ARAnchorManager m_AnchorManager;

    ARPlaneManager m_PlaneManager;

    List<ARAnchor> m_AnchorPoints;














}
