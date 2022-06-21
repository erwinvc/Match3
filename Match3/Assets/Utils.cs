using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils  {
    public static bool Within(int value, int min, int max) {
        return value >= min && value < max;
    }
}
