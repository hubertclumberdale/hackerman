using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : MonoBehaviour{
    public bool repaired;

    void Start(){
        GetComponent<ParticleSystem>().enableEmission = false;
    }
    
    public void SetRepaired(bool repaired){
        this.repaired = repaired;
        TileManager.Instance.curRoom.CheckForDoor();
        GetComponent<ParticleSystem>().enableEmission = true;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        GameManager.Instance.OnComputerRepaired();
    }

}
