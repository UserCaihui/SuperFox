using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTips : MonoBehaviour
{
    public GameObject tip;
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            tip.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            tip.SetActive(false);
        }
    }
}
