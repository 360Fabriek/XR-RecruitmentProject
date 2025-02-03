using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;

public class StartGame : MonoBehaviour
{
    [SerializeField] private GameObject ballHolder;
    public void StartButtonClicked()
    {
        StartCoroutine(SpawnBalls());
    }

    IEnumerator SpawnBalls()
    {
        // Loop through all of the pool balls in the scene
        for (int i = 0; i <= ballHolder.transform.childCount -1; i++)
        {
            // One by one, set them to active
            ballHolder.transform.GetChild(i).gameObject.SetActive(true);
            yield return new WaitForSeconds(0.2f);
        }
        for (int i = 0; i < ballHolder.transform.childCount -1; i++)
        {
            // After every ball has been spawned in, turn of kinematic (They are spawned as kinematic so that they will not move around by accident while they are spawning in)
            ballHolder.transform.GetChild(i).gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}
