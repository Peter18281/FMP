using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CameraFollowScript : NetworkBehaviour
{
    float followTimeDelta = 0.8f;
    [SerializeField]
    RoundManager roundManager;
    public float minSizeY = 5f;
    public Camera cam;
    public SpriteRenderer boundingBox;

    void SetCameraSize()
    {
        float minSizeX = minSizeY * Screen.width / Screen.height;

        float width = Mathf.Abs(roundManager.players[0].transform.position.x - roundManager.players[1].transform.position.x) * 0.5f;
        float height = Mathf.Abs(roundManager.players[0].transform.position.y - roundManager.players[1].transform.position.y) * 0.5f;

        cam.orthographicSize = Mathf.Max(height,
            Mathf.Max(width, minSizeX) * Screen.height / Screen.width, minSizeY);
    }

    void SetCameraPos()
    {
        Vector3 middle = (roundManager.players[0].transform.position + roundManager.players[1].transform.position) * 0.5f;

        transform.position = new Vector3(
            middle.x,
            transform.position.y,
            transform.position.z
        );

        float camVertExtent = cam.orthographicSize;
        float camHorzExtent = cam.aspect * camVertExtent;

        float leftBound = boundingBox.bounds.min.x + camHorzExtent;
        float rightBound = boundingBox.bounds.max.x - camHorzExtent;
        float bottomBound = boundingBox.bounds.min.y + camVertExtent;
        float topBound = boundingBox.bounds.max.y - camVertExtent;

        float camX = Mathf.Clamp(middle.x, leftBound, rightBound);
        float camY = Mathf.Clamp(middle.y, bottomBound, topBound);

        cam.transform.position = new Vector3(camX, camY, cam.transform.position.z);
    }

    void Update()
    {
        if (roundManager.players.Length == 2)
        {
            SetCameraPos();
            SetCameraSize();
        }
    }
}

