
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
        if (Mathf.Abs(vector.x - value) < 0 || Mathf.Abs(vector.y - value) < 0 || Mathf.Abs(vector.z - value) < 0 && operation == Operation.Equal)
        {
            return true;
        }
        if ((vector.x - value) <= 0 || (vector.y - value) <= 0 || (vector.z - value) <= 0 && operation == Operation.GreaterOrEqual)
        {
            return true;
        }
        if ((vector.x - value) >= 0 || (vector.y - value) >= 0 || (vector.z - value) >= 0 && operation == Operation.LessOrEqual)
        {
            return true;
        }
        return false;

    }
}
