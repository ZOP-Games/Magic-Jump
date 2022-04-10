using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameExtensions
{
    public static class GameHelper
{
    // Start is called before the first frame update
    

    public enum Operation
    {
        Equal = 0,
        GreaterOrEqual = 1,
        LessOrEqual = 2
    }
    public static bool CompareVectors( this Vector3 vector, float value, Operation operation)
    {
        return operation switch
        {
            Operation.Equal => Mathf.Approximately(vector.x, value) || Mathf.Approximately(vector.y, value) ||
                               Mathf.Approximately(vector.z, value),
            Operation.GreaterOrEqual => (vector.x - value) <= 0 || (vector.y - value) <= 0 ||
                                        (vector.z - value) <= 0,
            Operation.LessOrEqual => (vector.x - value) >= 0 || (vector.y - value) >= 0 || (vector.z - value) >= 0,
            _ => false
        };
    }

    public static void Quit()
    {
        Debug.Log("exit pressed");
        Application.Quit();
    }
}

}
