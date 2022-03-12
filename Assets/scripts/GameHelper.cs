using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHelper : ScriptableObject
{
    // Start is called before the first frame update
    public delegate void PassMethod();
    public static void Wait(int seconds,PassMethod method, MonoBehaviour starter)
    {
        IEnumerator Waiting()
        {
            yield return new WaitForSeconds(seconds);
            method();

        }

        starter.StartCoroutine(Waiting());

    }

    public enum Operation
    {
        Equal = 0,
        GreaterOrEqual = 1,
        LessOrEqual = 2
    }
    public static bool CompareVectors(Vector3 vector, float value, Operation operation)
    {
        var vector3 = vector;//new Vector3(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
        return operation switch
        {
            Operation.Equal => Mathf.Approximately(vector3.x, value) || Mathf.Approximately(vector3.y, value) ||
                               Mathf.Approximately(vector3.z, value),
            Operation.GreaterOrEqual => (vector3.x - value) <= 0 || (vector3.y - value) <= 0 ||
                                        (vector3.z - value) <= 0,
            Operation.LessOrEqual => (vector3.x - value) >= 0 || (vector3.y - value) >= 0 || (vector3.z - value) >= 0,
            _ => false
        };
    }
}
