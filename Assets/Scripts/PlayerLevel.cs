using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    [SerializeField] private int Level = 1;
    [SerializeField] private int BaseXP = 500;

    private int XP = 0;
    private int xpsForNextLevel;

    // Start is called before the first frame update
    void Awake()
    {
        CalculateNextLevelXP();
    }

    public int GetLevel()
    {
        return Level;
    }

    public int GetXP()
    {
        return XP;
    }

    public void AddXP(int xp)
    {
        XP += xp;

        if (xp > xpsForNextLevel)
        {
            Level++;

            int xpsLeft = XP - xpsForNextLevel;
            XP = xpsLeft;

            CalculateNextLevelXP();
        }
    }

    private void CalculateNextLevelXP()
    {
        if (Level == 1) xpsForNextLevel = BaseXP;
        else xpsForNextLevel = BaseXP * (int)Mathf.Pow(Level, 1.25f);
    }
}
