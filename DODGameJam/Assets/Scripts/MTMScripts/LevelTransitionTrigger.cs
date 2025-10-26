using System;
using System.Collections;
using UnityEngine;

public class LevelTransitionTrigger : MonoBehaviour, ISelect
{
    [SerializeField] GameObject player;
    playerController playerController;
    [SerializeField] Transform transition, destination;
    //bool canMove;
    private void Awake()
    {
        playerController = GameManager.instance.playerScript;
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other == player) canMove = true;
    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other == player) canMove = false;
    //}
    private void Update()
    {
        //if (canMove && Input.GetMouseButtonDown(0)) StartCoroutine(Transition());
    }
    IEnumerator Transition()
    {
        playerController.enabled = false;
        yield return new WaitForEndOfFrame();
        player.transform.position = new Vector3(destination.position.x, destination.position.y, destination.position.z);
        playerController.enabled = true;
    }

    public void Selected()
    {
        if (playerController.teleport) StartCoroutine(Transition());
    }

}
