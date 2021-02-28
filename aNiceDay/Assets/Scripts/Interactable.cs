
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float radius = 3f;
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
