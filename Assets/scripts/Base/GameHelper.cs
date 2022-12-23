using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameExtensions
{
    //this is a class that does things we want to use in a lot of places, extending the base unity api 
    public static class GameHelper
    {
        //enum for vector comparing
        public enum VectorCompareOperation
        {
            Equal = 0,
            GreaterOrEqual = 1,
            LessOrEqual = 2
        }

        //vector comparer, decides whether all axes of the vector are <VectorCompareOperation> to the specified value
        //this an extension method, you can call it by <A vector3 object>.CompareWithValue(...)
        public static bool CompareWithValue(this Vector3 vector, float value, VectorCompareOperation vectorCompareOperation)
        {
            return vectorCompareOperation switch
            {
                VectorCompareOperation.Equal => Mathf.Approximately(vector.x, value) || Mathf.Approximately(vector.y, value) ||
                                   Mathf.Approximately(vector.z, value),
                VectorCompareOperation.GreaterOrEqual => (vector.x - value) <= 0 || (vector.y - value) <= 0 ||
                                            (vector.z - value) <= 0,
                VectorCompareOperation.LessOrEqual => (vector.x - value) >= 0 || (vector.y - value) >= 0 || (vector.z - value) >= 0,
                _ => false
            };
        }
        //Vector2 overload of CompareWithValue above, works the same but w/ Vector2-s
        public static bool CompareWithValue(this Vector2 vector, float value, VectorCompareOperation vectorCompareOperation)
        {
            return vectorCompareOperation switch
            {
                VectorCompareOperation.Equal => Mathf.Approximately(vector.x, value) || Mathf.Approximately(vector.y, value),
                VectorCompareOperation.GreaterOrEqual => (vector.x - value) <= 0 || (vector.y - value) <= 0,
                VectorCompareOperation.LessOrEqual => (vector.x - value) >= 0 || (vector.y - value) >= 0,
                _ => false
            };
        }

        
        //proper vector conversion because Yup 
        public static Vector2 ToVector2(this Vector3 vector3)
        {
            return new Vector2(vector3.x,vector3.z);
        }

        public static Vector3 ToVector3(this Vector2 vector2)
        {
            return new Vector3(vector2.x, 0, vector2.y);
        }

        public static IEnumerator ActivateFor(this GameObject obj,int time)
        {
            obj.SetActive(!obj.activeSelf);
            yield return new WaitForSeconds(time);
            obj.SetActive(!obj.activeSelf);
        }

        public static Transform GetNextSibling(this Transform tf)
        {
            try
            {
                return tf.parent.GetChild(tf.GetSiblingIndex() + 1);
            }
            catch
            {
                return null;
            }
        }

        //exit action, useful as we exit a lot
        public static void Quit()
        {
            Debug.Log("exit pressed");
            Application.Quit();
        }
    }
}