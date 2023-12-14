using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    [SerializeField] private int Level = 1;
    [SerializeField] private int XP = 0;

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
        xpsForNextLevel = Level ^ 2;

        if (Level >= 20)
        {
            xpsForNextLevel = 10000;
        }
    }
}
