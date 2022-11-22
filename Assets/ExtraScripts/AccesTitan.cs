using Fusion;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccesTitan : NetworkBehaviour
{
    [Networked]
    public NetworkObject TitanObject { get; set; }
    
    [Networked]
    public EnterVanguardTitan TitanScript { get; set; }
 
    void Update()
    {
        StartTitanFall();
        EmbarkWithTitan();
    }
 
    void StartTitanFall()
    {
        if (!HasInputAuthority) return;
        
        if (Input.GetKeyDown(KeyCode.V))
        {
            Vector3 position = gameObject.transform.position;
            position.y += 178;
            TitanObject.transform.position = position;
            TitanScript.StartFall();
        }
    }
 
    void EmbarkWithTitan()
    {
        if (!HasInputAuthority) return;
        
        if (Input.GetKeyDown(KeyCode.F) && TitanScript.inRangeForEmbark)
        {
            StartCoroutine(TitanScript.Embark());
        }
    }

}
