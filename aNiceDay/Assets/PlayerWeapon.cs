using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    // Start is called before the first frame update
    private bool IsAttacking;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool GetIsAttacking() 
    {
        return IsAttacking;
    }

    public void SetIsAttacking(bool bIsAttacking)
    {
        IsAttacking = bIsAttacking;
    }
}
