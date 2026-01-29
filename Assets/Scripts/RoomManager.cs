using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public Transform nextDoor;
    public Computer[] computers;
    public Transform camPos;
    public GameObject door;
    private bool opened;
    public float scoreTimer;
    private Vector3 startRot;
    private void Start()
    {
        computers = GetComponentsInChildren<Computer>();
        startRot = door.transform.localEulerAngles ;
    }

    public void CheckForDoor()                                          //ogni volta che spacco computer chiamo sta funzione che se tutti i monitor sono distrutti apre porta
    {
        for (int i = 0; i < computers.Length; i++)
        {
            if (computers[i].repaired == false)
                return;
        }
        if (TileManager.Instance.rooms.Count == 1)
            GameManager.Instance.PlayMetalSong();   
            OpenDoor();
    }


    public void OpenDoor()                          //attivare animaazione porta e crea nuovo tile
    {
        if (!opened)
        {
            TimeManager.Instance.AddTimer(scoreTimer);                  //AGGIUNGI TEMPO
            TileManager.Instance.AddTile();
            StartCoroutine(ActiveDoorAnim());
            opened = true;
        }
    }


    IEnumerator ActiveDoorAnim()
    {
        float elapsedTime = 0;
        Vector3 startigRot = door.transform.localEulerAngles;
        while (elapsedTime < .3f)
        {
            door.transform.localEulerAngles = Vector3.Lerp(startigRot, new Vector3(0, -90, 0), (elapsedTime / .3f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        door.transform.localEulerAngles = new Vector3(0, -90, 0);
    }

    IEnumerator CloseDoorAnim()
    {
        float elapsedTime = 0;
        Vector3 startigRot = door.transform.localEulerAngles;
        while (elapsedTime < .6f)
        {
            door.transform.localEulerAngles = Vector3.Lerp(startigRot, startRot, (elapsedTime / .6f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        door.transform.localEulerAngles = startRot;
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
         
            TileManager.Instance.curRoom = this;
            StartCoroutine(TileManager.Instance.MoveCam(camPos.position));
        }
    }
}
