using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Room : MonoBehaviour
{
    public GameObject vcam;
    [SerializeField]
    private CamFollow camFollow;

    private Player player;

    private void Awake()
    {
        camFollow = GameObject.FindGameObjectWithTag("CamFollow").GetComponent<CamFollow>();
        vcam.GetComponent<CinemachineVirtualCamera>().Follow = camFollow.transform;
        player= GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Room"))
        {
            playerYSpeed = player.Core.Movement.CurrentVelocity.y;
            StartCoroutine(RoomTransition(Camera.main.GetComponent<CinemachineBrain>().m_DefaultBlend.BlendTime));
            vcam.SetActive(false);
        }
    }

    [SerializeField] private CamType camType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Room"))
        {
            vcam.SetActive(true);

            camFollow.camType = this.camType;
        }
    }

    float playerYSpeed;

    IEnumerator RoomTransition(float time)
    {
        player.Anim.enabled = false;
        //PlayerInControlHandler.SetPlayerControl(false);
        yield return new WaitForSeconds(time);
        player.Anim.enabled = true;
        //PlayerInControlHandler.SetPlayerControl(true);
    }
}
