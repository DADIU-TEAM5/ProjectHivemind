using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Variables/Level Budget")]
public class levelBudget : ScriptableObject
{
    public int Value;
    public int usedBudget;

    bool givingBudget;


    public bool BudgetLeft()
    {
        return (usedBudget < Value);
    }

    public int GetBudget(int budgetToGet)
    {
        if (usedBudget < budgetToGet)
        {
            
            
            usedBudget += budgetToGet;


            return budgetToGet;

        }
        else
            return 0;
    }
}
