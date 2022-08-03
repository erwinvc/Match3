using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils  {
    #region ObjectFinding
    private static T CheckObjects<T>(T[] collection, string name, bool errorWhenNoneFound = true) where T : class {
        if (errorWhenNoneFound && collection.Length == 0) Debug.LogError($"No instances of unique type {name} could be found. This will probably break a lot of stuff!");
        if (collection.Length == 1) return collection[0];
        if (collection.Length > 1) Debug.LogError($"Multiple instances of unique type {name} could be found. This will probably break a lot of stuff!");
        return null;
    }

    private static T CheckTransforms<T>(T[] collection, string transformName, string name, bool errorWhenNoneFound = true) where T : class {
        if (errorWhenNoneFound && collection.Length == 0) Debug.LogError($"No children of {transformName} named {name} could be found. This will probably break a lot of stuff!");
        if (collection.Length == 1) return collection[0];
        if (collection.Length > 1) Debug.LogError($"Multiple children of {transformName} named {name} could be found. This will probably break a lot of stuff!");
        return null;
    }

    public static bool FindUniqueObjectInChildren<T>(GameObject parent, out T obj, bool errorWhenNoneFound = true) where T : Object {
        obj = CheckObjects(parent.GetComponentsInChildren<T>(true), typeof(T).Name, errorWhenNoneFound);
        return obj != null;
    }
    public static bool FindUniqueObject<T>(out T obj, bool errorWhenNoneFound = true) where T : Object {
        obj = CheckObjects(GameObject.FindObjectsOfType<T>(true), typeof(T).Name, errorWhenNoneFound);
        return obj != null;
    }

    public static T FindUniqueObject<T>() where T : Object {
        return CheckObjects(GameObject.FindObjectsOfType<T>(), typeof(T).Name);
    }

    public static GameObject FindUniqueGameObjectWithTag(string tag) {
        return CheckObjects(GameObject.FindGameObjectsWithTag(tag), tag);
    }

    public static T FindUniqueObjectWithTag<T>(string tag) where T : Component {
        return FindUniqueGameObjectWithTag(tag)?.GetComponent<T>();
    }

    /*Logs an error when les or more than one instance of the type is found in the scene*/
    public static void EnsureOnlyOneInstanceInScene<T>() where T : Object {
        FindUniqueObject<T>();
    }

    #endregion

    public static Color ColorWithAlpha(Color color, float alpha) {
        return new Color(color.r, color.g, color.b, alpha);
    }

    public static Sequence DelayedAction(float delay, System.Action action) {
        return DOTween.Sequence().AppendInterval(delay).AppendCallback(() => action());
    }

    public struct Cooldown {
        public float time;

        public bool Ready(float time) {
            bool ready = this.time + time <= Time.time;
            if (ready) this.time = Time.time;
            return ready;
        }
    }

    /*Maps a float from one range to another*/
    public static float Remap(float val, float minIn, float maxIn, float minOut, float maxOut) {
        return minOut + (val - minIn) * (maxOut - minOut) / (maxIn - minIn);
    }

    public static bool Within(int value, int min, int max) {
        return value >= min && value < max;
    }

    public static bool RandomBool() {
        return Random.value > 0.5f;
    }

    public static T RandomEnumValue<T>() where T : System.Enum{
        System.Array values = System.Enum.GetValues(typeof(T));
        return (T)values.GetValue(Random.Range(0, values.Length));
    } 
}
