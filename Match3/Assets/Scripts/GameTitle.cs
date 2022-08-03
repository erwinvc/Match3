using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameTitle : WorldGenerationObject
{
    public TextMeshProUGUI foreground;
    public TextMeshProUGUI backgroundPrimary;
    public TextMeshProUGUI backgroundSecondary;

    public override void Generate(WorldGenerationData data, bool isActive, bool generateNewMaterials) {
        backgroundPrimary.color = data.colorPrimary;
        backgroundSecondary.color = data.colorSecondary;
    }

    public override bool ShouldGenerate(WorldGenerationData data) {
        return true;
    }
}
