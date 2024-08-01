using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_ExtraGenerationBugFix : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(DeleteExtras());
    }

    IEnumerator DeleteExtras()
    {
        yield return new WaitForSeconds(.1f);
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i < 6) continue;
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
