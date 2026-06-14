using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveOverWrite : MonoBehaviour
{
    void Awake()
    {
        OverWrite();
    }

    private void OverWrite()
    {
        if(GameSave.ShouldResetRunOnFirstLevel)
        {
            GameSave.ResetRunData(false);
        }
    }
}
