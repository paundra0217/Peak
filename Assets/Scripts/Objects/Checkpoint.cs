using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool checkpointChecked;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !checkpointChecked)
        {
            checkpointChecked = true;
            GameManager.Instance.SetSpawnPoint(transform.position.x, transform.position.y);
            //ChangeAppearance();
        }
    }

    private void ChangeAppearance()
    {
        GetComponent<SpriteRenderer>().color = Color.green;
    }
}
