using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class NPCController : MonoBehaviour
{
    private static NPCController _instance;
    public static NPCController Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("NPC Controller is null");
            }

            return _instance;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        _instance = this;
    }

    private GameObject FindNPC(string name)
    {
        GameObject npc = transform.Find(name).gameObject;
        if (npc == null)
        {
            Debug.LogErrorFormat("NPC {0} is not valid", name);
            return null;
        }

        return npc;
    }

    public void MoveNPC(string name, Vector2 newLocation, bool flipSprite)
    {
        GameObject npc = FindNPC(name);
        if (npc == null) return;

        npc.transform.position = newLocation;
        npc.GetComponent<SpriteRenderer>().flipX = flipSprite;
    }

    public void ToggleNPC(string name, bool active)
    {
        GameObject npc = FindNPC(name);

        Color color = npc.GetComponent<SpriteRenderer>().color;
        npc.GetComponent<SpriteRenderer>().color = active ?
            new Color(color.r, color.g, color.b, 1f) :
            new Color(color.r, color.g, color.b, 0f);
    }

    #region for unity events
    
    public void MoveToCreditScene(string name)
    {
        ToggleNPC(name, true);
        MoveNPC(name, new Vector2(370.5f, 395.66f), false);
    }

    public void HideNPC(string name)
    {
        ToggleNPC(name, false);
    }

    public void ShowNPC(string name)
    {
        ToggleNPC(name, true);
    }

    #endregion
}
