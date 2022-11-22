using Fusion;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccesTitan : MonoBehaviour
{
    public NetworkObject titanObject;
    public EnterVanguardTitan titanScript;
 
    void Update()
    {
        StartTitanFall();
        EmbarkWithTitan();
    }
 
    void StartTitanFall()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            Vector3 position = gameObject.transform.position;
            position.y += 178;
            titanObject.transform.position = position;
            titanScript.StartFall();
        }
    }
 
    void EmbarkWithTitan()
    {
        if (Input.GetKeyDown(KeyCode.F) && titanScript.inRangeForEmbark)
        {
            StartCoroutine(titanScript.Embark());
        }
    }

}
