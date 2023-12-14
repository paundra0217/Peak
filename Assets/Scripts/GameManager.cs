using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float gravityScale;
    private void Awake()
    {
        Physics2D.gravity = new Vector2(0, -gravityScale);
    }
}
