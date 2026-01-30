using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private static CameraManager _instance;
    public static CameraManager Instance { get { return _instance; } }
    
    private void Awake()
    {
        if (_instance != null && _instance != this) Destroy(this.gameObject);
        else _instance = this;
    }

    public void SetCameraPosition(Vector3 position)
    {
        Camera.main.transform.position = position;
    }

    public IEnumerator MoveCameraTo(Vector3 newPos)
    {
        float elapsedTime = 0;
        Vector3 startingPos = Camera.main.transform.position;
        while (elapsedTime < .3f)
        {
            Camera.main.transform.position = Vector3.Lerp(startingPos, newPos, (elapsedTime / .3f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Camera.main.transform.position = newPos;
    }
}