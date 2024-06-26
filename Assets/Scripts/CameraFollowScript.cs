using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

//Script for setting the Camera's position and size according to the spacing of the players.
public class CameraFollowScript : NetworkBehaviour
{
    float followTimeDelta = 0.8f;
    [SerializeField] RoundManager roundManager;

    public float minSizeY = 5f;
    public Camera cam;
    public SpriteRenderer boundingBox;

    void SetCameraSize()
    {
        //Calculate minimum width from minimum height.
        float minSizeX = minSizeY * Screen.width / Screen.height;

        //Get the midpoint of the 2 players.
        float width = Mathf.Abs(roundManager.players[0].transform.position.x - roundManager.players[1].transform.position.x) * 0.5f;
        float height = Mathf.Abs(roundManager.players[0].transform.position.y - roundManager.players[1].transform.position.y) * 0.5f;
        
        //Set the camera's size, based on the minimum size, or the distance between players, whichever is larger.
        cam.orthographicSize = Mathf.Max(height,
            Mathf.Max(width, minSizeX) * Screen.height / Screen.width, minSizeY);
    }

    void SetCameraPos()
    {
        //Get the midpoint of the players.
        Vector3 middle = (roundManager.players[0].transform.position + roundManager.players[1].transform.position) * 0.5f;
        
        //Set the camera's x value to this midpoint.
        transform.position = new Vector3(
            middle.x,
            transform.position.y,
            transform.position.z
        );
        
        //Get the bounds of the Camera;
        float camVertExtent = cam.orthographicSize;
        float camHorzExtent = cam.aspect * camVertExtent;
        
        //Calculate the bounds based on a bounding box in the scene.
        float leftBound = boundingBox.bounds.min.x + camHorzExtent;
        float rightBound = boundingBox.bounds.max.x - camHorzExtent;
        float bottomBound = boundingBox.bounds.min.y + camVertExtent;
        float topBound = boundingBox.bounds.max.y - camVertExtent;
        
        //Clamp the camera if it exceeds the bounds.
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

