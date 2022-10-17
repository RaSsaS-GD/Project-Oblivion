using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    private Transform player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    [HideInInspector] public CamType camType;

    private void Update()
    {
        switch (camType)
        {
            case CamType.MultiAxis:
                transform.position = player.position;
                break;
            case CamType.Vertical:
                transform.position = new Vector3(transform.position.x, player.position.y, player.position.z);
                break;
            case CamType.Horizontal:
                transform.position = new Vector3(player.position.x, transform.position.y, player.position.z);
                break;
        }
    }
}

public enum CamType
{
    Static,
    Horizontal,
    Vertical,
    MultiAxis
}
