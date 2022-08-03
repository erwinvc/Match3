using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Debugger : MonoBehaviour {
    public TextMeshProUGUI text3;
    public TextMeshProUGUI text4;
    public TextMeshProUGUI text5;
    private static Debugger _Instance;
    void Start() {
        if (!Application.isEditor) enabled = false;
        _Instance = this;
    }

    int score3 = 0;
    int score4 = 0;
    int score5 = 0;

    public void Update() {
        if (Input.GetKeyDown(KeyCode.Equals)) Time.timeScale++;
        if (Input.GetKeyDown(KeyCode.Minus)) Time.timeScale--;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0, 10000);
    }

    public static void AddScore(int amount) {
        _Instance._AddScore(amount);
    }

    public void _AddScore(int amount) {
        switch (amount) {
            case 3: score3++; break;
            case 4: score4++; break;
            case 5: score5++; break;
        }

        text3.text = $"3: {score3}";
        text4.text = $"4: {score4}";
        text5.text = $"5: {score5}";
    }
}
