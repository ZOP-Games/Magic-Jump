using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameExtensions
{
    //this is a class that does things we want to use in a lot of places, extending the base unity api 
    public static class GameHelper
    {
        //enum for vector comparing
        public enum Operation
        {
            Equal = 0,
            GreaterOrEqual = 1,
            LessOrEqual = 2
        }

        //vector comparer, decides whether all axes of the vector are <Operation> to the specified value
        //this an extension method, you can call it by <A vector3 object>.CompareVectors(...)
        public static bool CompareVectors(this Vector3 vector, float value, Operation operation)
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

        //exit action, useful as we use exiting a lot
        public static void Quit()
        {
            Debug.Log("exit pressed");
            Application.Quit();
        }
    }
}