using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyInteraction : MonoBehaviour
{
    private Interactable interactable;
    private bool isStolen = false;
    GameObject MoneyBag;

    // Start is called before the first frame update
    void Start()
    {
        MoneyBag = GameObject.Find("MoneyBag");
    }

    public void StealingMoney(bool something)
    {
        isStolen = something;
        MoneyBag.SetActive(something);

    }

    public bool GetStatusOfStolenMoney()
    {
        return isStolen;
    }
}
