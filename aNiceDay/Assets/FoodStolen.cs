using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodStolen : MonoBehaviour
{
    private Interactable interactable;
    private bool isStolen = false;
    GameObject FishPlate;


    // Start is called before the first frame update
    void Start()
    {
        FishPlate = GameObject.Find("FishPlate");
    }

    public void StealingFood(bool something) 
    {
        isStolen = something;
        FishPlate.SetActive(something);
        
    }

    public bool GetStatusOfStolenFood() 
    {
        return isStolen;
    }


}

