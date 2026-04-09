using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public RoomManager roomTutorialPrefab;
    public RoomManager creditsPrefab;
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
        {
            rm = Instantiate(roomTutorialPrefab);
            
            // Spawna anche la stanza credits a sinistra del tutorial
            if (creditsPrefab != null && rm.leftDoor != null)
            {
                RoomManager creditsRoom = Instantiate(creditsPrefab);
                
                // Sposto la stanza credits più a sinistra (assumo larghezza stanza ~65 unità)
                Vector3 targetPosition = rm.leftDoor.position;
                targetPosition.x -= 20f; // Metto la stanza a sinistra con gap
                targetPosition.y -= 1.8f;
                targetPosition.z -= 3.6f;
                creditsRoom.transform.position = targetPosition;
                
                creditsRoom.transform.SetParent(transform);
                
                // Debug per verificare separazione
                Debug.Log("Tutorial position: " + rm.transform.position);
                Debug.Log("Credits position: " + creditsRoom.transform.position);
            }
        }
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

        if (rooms.Count > 4)
        {
            // Prima distruggi la stanza più vecchia (sempre la prima nella lista)
            Destroy(rooms[0].gameObject);
            rooms.RemoveAt(0);
            
            // Poi chiudi la porta della nuova "prima" stanza (quella che sarà distrutta dopo)
            if (rooms.Count > 0)
            {
                rooms[0].CloseDoor();
            }
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
