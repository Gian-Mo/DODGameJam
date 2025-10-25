using System;
using System.Collections;
using UnityEngine;

public class LevelTransitionTrigger : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Transform transition, destination;
    bool canMove;
    private void OnTriggerEnter(Collider other)
    {
        if (other == player) canMove = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other == player) canMove = false;
    }
    private void Update()
    {
        if (canMove && Input.GetMouseButtonDown(0)) StartCoroutine(Transition());
    }
    IEnumerator Transition()
    {
            yield return new WaitForEndOfFrame();
            player.transform.position = new Vector3(destination.position.x, destination.position.y, destination.position.z);
    }
}
