
using UnityEngine;

public class Interactable : MonoBehaviour
{
    private bool IsInteracting = false;

    public void SetInteracting(bool bInteracting)
    {
        IsInteracting = bInteracting;
    }

    public bool GetIsInteracting()
    {
        return IsInteracting;
    }
}
