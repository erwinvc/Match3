using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Match3 : MonoBehaviour {
    public GameObject elementSlotWhitePrefab;
    public GameObject elementSlotDarkPrefab;
    public GameObject elementPrefab;
    public Transform gridPanel;
    private static int gridWidth = 8;
    private static int gridHeight = 8;
    public static int elementSize = 100;

    private static Match3 _Instance;

    public static Color ElementTypeToColor(ElementType type) {
        switch (type) {
            case ElementType.WHITE: return Color.white;
            case ElementType.YELLOW: return Color.yellow;
            case ElementType.GREEN: return Color.green;
            case ElementType.ORANGE: return new Color(1.0f, 0.65f, 0.0f);
            case ElementType.RED: return Color.red;
            case ElementType.PURPLE: return Color.magenta;
            case ElementType.BLUE: return Color.blue;
        }
        return Color.white;
    }

    int totalWidth;
    int halfTotalWidth;
    int totalHeight;
    int halfTotalHeight;
    int halfElementSize;

    private Element[,] elements;
    private GameObject[,] grid;
    private List<Element> elementsToRemove = new List<Element>();
    HashSet<int> elementsAlreadyInGroup = new HashSet<int>();

    bool shouldCheck = true;
    bool isAcceptingInput = true;

    //Gets called after the board has been simulated and all elements have stopped falling
    Action onAfterBoardSimulation = null;

    void Start() {
        _Instance = this;
        totalWidth = gridWidth * elementSize;
        halfTotalWidth = totalWidth / 2;
        totalHeight = gridHeight * elementSize;
        halfTotalHeight = totalHeight / 2;
        halfElementSize = elementSize / 2;

        gridPanel.transform.localPosition = new Vector3(-totalWidth / 2 + halfElementSize, -totalHeight / 2 + halfElementSize);
        if (elements != null) {
            foreach (Element element in elements) if (element != null) Destroy(element.gameObject);
        }
        if (grid != null) {
            foreach (GameObject obj in grid) Destroy(obj);
        }
        elements = new Element[gridWidth, gridHeight];
        grid = new GameObject[gridWidth, gridHeight];
        for (int x = 0; x < gridWidth; x++) {
            for (int y = 0; y < gridHeight; y++) {
                bool odd = (x + (y * gridHeight) + y) % 2 == 0;
                RectTransform obj = Instantiate(odd ? elementSlotWhitePrefab : elementSlotDarkPrefab, gridPanel).transform as RectTransform;
                obj.localPosition = new Vector2(x * elementSize, y * elementSize);
                grid[x, y] = obj.gameObject;
            }
        }

        SimulateBoard();
        shouldCheck = true;
    }

    void Update() {
        bool canCheck = !IsBoardInMotion();
        if (canCheck) {
            if (shouldCheck) SimulateBoard();
            else {
                onAfterBoardSimulation?.Invoke();
                onAfterBoardSimulation = null;
                isAcceptingInput = true;

            }
        }
    }

    private bool IsBoardInMotion() {
        foreach (Element element in elements) {
            if (element == null) continue;
            if (!element.frozen) return true;
        }
        return false;
    }

    private Element GetElement(int x, int y) {
        if (Utils.Within(x, 0, gridWidth) && Utils.Within(y, 0, gridHeight)) {
            return elements[x, y];
        }
        return null;
    }

    private void CreateNewElement(int x, int y, int spawnHeight) {
        Element element = Instantiate(elementPrefab, gridPanel).GetComponent<Element>();
        element.transform.localPosition = new Vector2(x * elementSize, (spawnHeight) * elementSize + totalHeight);
        element.SetTargetPosition(x, y, 0);
        elements[x, y] = element;
        element.onDrag = ElementOnDrag;
        Button button = element.gameObject.AddComponent<Button>();
        //button.onClick.AddListener(() => {
        //    for(int y = element.gridY; y < totalHeight; y++) {
        //        Element e = GetElement(element.gridX, y);
        //        if (e == null) return;
        //        e.frozen = false;
        //        e.velocity = 1000;
        //    }
        //});
    }
    private void RegisterElementForRemoval(int x, int y) {
        elementsAlreadyInGroup.Add(x + y * gridWidth);
        elementsToRemove.Add(GetElement(x, y));
    }

    private void RemoveElement(Element element) {
        elements[element.gridX, element.gridY] = null;
        element.gameObject.transform.DOScale(Vector3.zero, 0.2f);
        Destroy(element.gameObject, 0.2f);
    }



    /*Returns the Y position of the first element above given Y that's not null*/
    public int GetFirstElementYAbove(int x, int y) {
        for (int yy = y; yy < gridHeight; yy++) {
            if (GetElement(x, yy) != null) return yy;
        }
        return -1;
    }

    /*Simulates the board by checking for matches, unfreezing falling elements and generating new elements*/
    private void SimulateBoard() {
        isAcceptingInput = false;
        elementsAlreadyInGroup.Clear();
        if (!CheckMatches()) shouldCheck = false;

        RemoveRegisteredElements();
        UpdateFallingElements();
        CreateNewElements();
    }

    private void RemoveRegisteredElements() {
        foreach (Element element in elementsToRemove) RemoveElement(element);
        elementsToRemove.Clear();
    }

    private void UpdateFallingElements() {
        for (int x = 0; x < gridWidth; x++) {
            for (int y = 0; y < gridHeight; y++) {
                if (elements[x, y] == null) {
                    int firstElementYAbove = GetFirstElementYAbove(x, y);
                    if (firstElementYAbove == -1) continue;
                    Element firstElementAbove = elements[x, firstElementYAbove];
                    firstElementAbove.SetTargetPosition(x, y);
                    elements[x, firstElementYAbove] = null;
                    elements[x, y] = firstElementAbove;
                }
            }
        }
    }

    private void CreateNewElements() {
        List<int>[] newElementsPerRow = new List<int>[gridWidth];
        //Check for empty slots
        for (int x = 0; x < gridWidth; x++) {
            for (int y = 0; y < gridHeight; y++) {
                if (elements[x, y] == null) {
                    if (newElementsPerRow[x] == null) newElementsPerRow[x] = new List<int>();
                    int newY = y;
                    for (int yy = y; yy < gridHeight; yy++) {
                        if (elements[x, yy] != null) newY++;
                    }
                    newElementsPerRow[x].Add(newY);
                }
            }
        }

        //Create elements for empty slots and spawn them above the board
        for (int x = 0; x < gridWidth; x++) {
            if (newElementsPerRow[x] == null) continue;
            for (int y = 0; y < newElementsPerRow[x].Count; y++) {
                CreateNewElement(x, newElementsPerRow[x][y], y);
            }
        }
    }

    private bool CompareElementTypeAndGroup(int x, int y, ElementType type) {
        Element element = GetElement(x, y);
        if (element != null) {
            return !elementsAlreadyInGroup.Contains(x + y * gridWidth) && element.type == type;
        }
        return false;
    }

    /*Scan the board for matching pairs of 3, 4 and 5 in a horizontal and vertical pass*/
    private bool CheckMatches() {
        bool foundMatch = false;

        //Horizontal
        for (int x = 0; x < gridWidth; x++) {
            for (int y = 0; y < gridHeight; y++) {
                Element element = GetElement(x, y);
                if (element == null) continue;
                if (CompareElementTypeAndGroup(x + 1, y, element.type)
                    && CompareElementTypeAndGroup(x + 2, y, element.type)) {
                    if (CompareElementTypeAndGroup(x + 3, y, element.type)) {
                        if (CompareElementTypeAndGroup(x + 4, y, element.type)) {
                            //5
                            Debugger.AddScore(5);
                            for (int i = 0; i < 5; i++) RegisterElementForRemoval(x + i, y);
                            foundMatch = true;
                        } else {
                            //4
                            Debugger.AddScore(4);
                            for (int i = 0; i < 4; i++) RegisterElementForRemoval(x + i, y);
                            foundMatch = true;
                        }
                    } else {
                        //3
                        Debugger.AddScore(3);
                        for (int i = 0; i < 3; i++) RegisterElementForRemoval(x + i, y);
                        foundMatch = true;
                    }
                }
            }
        }
        //Vertical
        elementsAlreadyInGroup.Clear();
        for (int x = 0; x < gridWidth; x++) {
            for (int y = 0; y < gridHeight; y++) {
                Element element = GetElement(x, y);
                if (element == null) continue;
                if (CompareElementTypeAndGroup(x, y + 1, element.type)
                    && CompareElementTypeAndGroup(x, y + 2, element.type)) {
                    if (CompareElementTypeAndGroup(x, y + 3, element.type)) {
                        if (CompareElementTypeAndGroup(x, y + 4, element.type)) {
                            //5
                            Debugger.AddScore(5);
                            for (int i = 0; i < 5; i++) RegisterElementForRemoval(x, y + i);
                            foundMatch = true;
                        } else {
                            //4
                            Debugger.AddScore(4);
                            for (int i = 0; i < 4; i++) RegisterElementForRemoval(x, y + i);
                            foundMatch = true;
                        }
                    } else {
                        //3
                        Debugger.AddScore(3);
                        for (int i = 0; i < 3; i++) RegisterElementForRemoval(x, y + i);
                        foundMatch = true;
                    }
                }
            }
        }
        return foundMatch;
    }

    private void ElementOnDrag(Element element, int directionX, int directionY) {
        if (isAcceptingInput && swapTween == null) {
            SwapElement(element, directionX, directionY, true);
            element.isDragging = false;
        }
    }

    Tween swapTween = null;
    public void SwapElement(Element element, int directionX, int directionY, bool shouldAutomaticallReturn) {
        bool withinX = element.gridX + directionX >= 0 && element.gridX + directionX < gridWidth;
        bool withinY = element.gridY + directionY >= 0 && element.gridY + directionY < gridHeight;
        if (withinX && withinY) {
            int x = element.gridX;
            int y = element.gridY;
            Element toSwap = elements[x + directionX, y + directionY];
            elements[x + directionX, y + directionY] = element;
            elements[x, y] = toSwap;
            element.gridX = x + directionX;
            element.gridY = y + directionY;
            toSwap.gridX = x;
            toSwap.gridY = y;

            swapTween = element.transform.DOLocalMove(new Vector3((x + directionX) * elementSize, (y + directionY) * elementSize, 0.0f), 0.5f).OnComplete(() => {
                swapTween = null;
                shouldCheck = true;
                if (shouldAutomaticallReturn)
                    onAfterBoardSimulation = () => {
                        if (element != null)
                            SwapElement(element, -directionX, -directionY, false);
                    };
            }).SetEase(Ease.Linear);

            toSwap.transform.DOLocalMove(new Vector3(x * elementSize, y * elementSize, 0.0f), 0.5f).SetEase(Ease.Linear);
        }
    }
}
