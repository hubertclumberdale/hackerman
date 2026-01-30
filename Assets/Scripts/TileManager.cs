using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public RoomManager roomTutorialPrefab;
    public RoomManager[] roomPrefab;
    public List<RoomManager> rooms = new List<RoomManager>();
    private static TileManager _instance;
    public static TileManager Instance { get { return _instance; } }
    public RoomManager curRoom;




    private void Awake()
    {
        if (_instance != null && _instance != this) Destroy(this.gameObject);
        else _instance = this;
    }

    private void Start()
    {
        AddTile();
    }

    private void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //   AddTile();
    }

    public void AddTile()
    {
        RoomManager rm;
        if (rooms.Count==0)
         rm = Instantiate(roomTutorialPrefab);
        else
         rm = Instantiate(roomPrefab[Random.Range(0,roomPrefab.Length)]);
        if (rooms.Count > 0)
        {
            rm.transform.position = rooms[rooms.Count - 1].nextDoor.position;
        }
        else
        {
            rm.transform.position = Vector3.zero;
            CameraManager.Instance.SetCameraPosition(rm.camPos.position);
        }

        rm.transform.SetParent(transform);
        rooms.Add(rm);

        if (rooms.Count > 3)
        {
            Destroy(rooms[rooms.Count - 4].gameObject);
            rooms.RemoveAt(rooms.Count -4);
        }

        // se la stanza corrente era lo shop, ma noi abbiamo aggiunto ora un'altra stanza
            // se questa nuova stanaz è uno shop, non fare nulla
            // se questa nuova stanza è normale, far ripartire la musica metal 

        if(rm.isShop && !curRoom.isShop) 
        {
            AudioManager.Instance.PlayShopSong(); 
            TimeManager.Instance.PauseTimer();
        }
        else
        {
            if(curRoom != null && curRoom.isShop)
            {
                AudioManager.Instance.PlayMetalSong(); 
                TimeManager.Instance.ResumeTimer();
            }
        }
        curRoom = rm;
    }
}
