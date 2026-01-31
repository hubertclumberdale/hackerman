using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mazza : MonoBehaviour
{

    public Player player;
    
    public float explosionForce = 850;
    public float explosionRadius = 5;
    public int computersFixed = 0;
    void OnCollisionEnter(Collision collision) {
        if(player.isAttacking){
            AudioManager.Instance.PlayHit();
            if(collision.gameObject.GetComponent<Rigidbody>() != null){
                Rigidbody collisionRigidbody = collision.gameObject.GetComponent<Rigidbody>();

                float lateralForce = Random.Range(8f, 15f);  
                float verticalForce = Random.Range(1f, 4f);
                float depthForce = Random.Range(-1f, 1f);

                Vector3 force = new Vector3(lateralForce, verticalForce, depthForce);

                // Flip direction depending on which side player is on
                force.x *= Mathf.Sign(collision.transform.position.x - transform.position.x);

                collisionRigidbody.AddForce(force, ForceMode.Impulse);
            }

            // Gestione Computer
            if (collision.gameObject.tag == "Computer" && !collision.gameObject.GetComponent<Computer>().repaired){
                Computer computer = collision.gameObject.GetComponent<Computer>();
                computer.SetRepaired(true);
                computersFixed++;
            }
            
            // Gestione Maschere
            MaskInteractable mask = collision.gameObject.GetComponent<MaskInteractable>();
            if (mask != null){
                Debug.Log("Mazza hit a mask");
                mask.OnMazzaHit();
            }
        }
    }
}
