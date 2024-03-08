using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class LetterGrid : MonoBehaviour
{
    [SerializeField] private GameObject letterTilePrefab;
    [SerializeField] private int gridSize = 4;

    private static LetterGrid _instance;

    private List<GameObject> letterTiles;
    private List<GameObject> selectedTiles;
    private GameObject selectedContainer;
    private Dictionary<GameObject, Vector3> originalTilePositions;

    private Camera mainCamera;

    private Dictionary<char, double> letterValues = new Dictionary<char, double>()
    {
    // Bronze value
    {'A', 1}, {'D', 1}, {'E', 1}, {'G', 1}, {'I', 1}, {'L', 1}, {'N', 1}, {'O', 1}, {'R', 1}, {'S', 1}, {'T', 1}, {'U', 1},
    {'B', 1.25}, {'C', 1.25}, {'F', 1.25}, {'H', 1.25}, {'M', 1.25}, {'P', 1.25},

    // Silver value
    {'V', 1.5}, {'W', 1.5}, {'Y', 1.5},
    {'J', 1.75}, {'K', 1.75}, {'Q', 1.75},

    //Gold value
    {'X', 2}, {'Z', 2},

    };

    public static LetterGrid Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<LetterGrid>();
                if (_instance == null)
                {
                    Debug.LogError("No LetterGrid found in scene. Creating instance.");
                    _instance = new LetterGrid();
                }
            }
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            DestroyImmediate(gameObject); // Destroy duplicate if it exists
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject); // Persist across scenes (optional)
    }

    // Start is called before the first frame update
    void Start()
    {
        letterTiles = new List<GameObject>();
        selectedTiles = new List<GameObject>();
        originalTilePositions = new Dictionary<GameObject, Vector3>();
        selectedContainer = GameObject.Find("SelectedContainer");
        mainCamera = Camera.main;
        GenerateLetterGrid();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitObject = hit.collider.gameObject;
                if (hitObject.CompareTag("LetterTile"))
                {
                    //Debug.Log("Hit something");
                    if (IsTileSelected(hitObject))
                    {
                        DeselectTile(hitObject);
                    }
                    else
                    {
                        SelectTile(hitObject);
                    }
                }
            }
        }
    }

    void GenerateLetterGrid()
    {
        for (int row = 0; row < gridSize; row++)
        {
            for (int col = 0; col < gridSize; col++)
            {
                GameObject newTile = Instantiate(letterTilePrefab, transform.position, Quaternion.identity);
                letterTiles.Add(newTile);
                newTile.transform.SetParent(transform);
                newTile.GetComponentInChildren<TMP_Text>().text = GetRandomLetter();
                newTile.transform.localPosition = new Vector3(col, row, 0);
                newTile.name = newTile.GetComponentInChildren<TMP_Text>().text;
            }
        }

        foreach (GameObject tile in letterTiles)
        {
            originalTilePositions.Add(tile, tile.transform.localPosition);
        }
    }

    string GetRandomLetter()
    {
        char randomLetter = (char)UnityEngine.Random.Range(65, 91);

        return randomLetter.ToString();
    }

    int GetWordValue(string word)
    {
        double totalValue = 0;

        foreach (char letter in word.ToUpper())
        {
            if (letterValues.TryGetValue(letter, out double value))
            {
                totalValue += value;
            }
            else
            {
                // Handle unknown letters (optional)
                // You can throw an exception, log a warning, or ignore them
                Debug.LogWarning("Unknown letter encountered: " + letter);
            }
        }

        return (int)Math.Round(totalValue);
    }

    public int GetSelectedWordValue()
    {
        string selectedWord = BuildSeletedWord();

        if (selectedWord.Length > 0)
        {
            if (WordChecker.Instance.IsValidWord(selectedWord))
            {
                return GetWordValue(selectedWord);
            }
            else { return 0; }
        }
        else return 0;

    }

    bool IsTileSelected(GameObject tile)
    {
        return selectedTiles.Contains(tile);
    }

    void DeselectTile(GameObject tile)
    {
        int tileIndex = selectedTiles.IndexOf(tile);
        selectedTiles.Remove(tile);
        Debug.Log("Removed: " + tile.name);

        // Move tile back to its original position in the grid (same logic as before)
        if (originalTilePositions.ContainsKey(tile))
        {
            tile.transform.SetParent(transform);
            tile.transform.localPosition = originalTilePositions[tile];
        }
        else
        {
            Debug.LogError("Original position not found for tile: " + tile.name);
        }

        // Remove and move all tiles to the right (> index) in selectedTiles
        if (tileIndex < selectedTiles.Count)
        {
            for (int i = tileIndex; i < selectedTiles.Count; i++)
            {
                GameObject tileToMove = selectedTiles[i];
                selectedTiles.RemoveAt(i); // Remove from selected list before moving

                // Move tile back to its original position (same logic as before)
                if (originalTilePositions.ContainsKey(tileToMove))
                {
                    tileToMove.transform.SetParent(transform);
                    tileToMove.transform.localPosition = originalTilePositions[tileToMove];
                }
                else
                {
                    Debug.LogError("Original position not found for tile: " + tileToMove.name);
                }

                // Decrement i by 1 to account for removed item at index i during loop iteration
                i--;
            }
        }

        CheckWordValidity();
    }

    void SelectTile(GameObject tile)
    {
        selectedTiles.Add(tile);
        Debug.Log("Added: " + tile.name);

        // Move tile to the selected container
        tile.transform.SetParent(selectedContainer.transform);

        // Set initial position based on selectedTiles count
        if (selectedTiles.Count == 1)
        {
            // First selected tile, set position to container"s position
            tile.transform.localPosition = selectedContainer.transform.position;
        }
        else
        {
            // Subsequent tiles, position them to the right of the previous tile with spacing
            int previousTileIndex = selectedTiles.Count - 2;
            GameObject previousTile = selectedTiles[previousTileIndex];

            // Calculate offset based on previous tile position and tile width
            float xPos = previousTile.transform.localPosition.x; // Replace spacing with your desired spacing
            tile.transform.localPosition = new Vector3(xPos + 1, selectedContainer.transform.position.y, 0); // Assuming only X position needs adjustment
        }

        CheckWordValidity();
    }

    string BuildSeletedWord()
    {
        string selectedWord = "";

        foreach (GameObject tile in selectedTiles)
        {
            selectedWord += tile.GetComponentInChildren<TMP_Text>().text;
        }

        return selectedWord;
    }

    void CheckWordValidity()
    {
        string selectedWord = BuildSeletedWord().ToLower().Trim();

        if (selectedWord.Length > 0)
        {
            if (WordChecker.Instance.IsValidWord(selectedWord))
            {
                Debug.Log("Valid word: " + selectedWord);
            }
            else
            {
                Debug.Log("Invalid word: " + selectedWord);
            }
        }
        else
        {
            Debug.Log("Nothing has been selected");
        }

    }

    public void ScrambleLetter()
    {
        // Remove all the selected tiles
        if (selectedTiles.Count != 0)
        {
            DeselectTile(selectedTiles[0]);
        }

        // Scramble every tiles in the grid
        foreach (GameObject tile in letterTiles)
        {
            tile.GetComponentInChildren<TMP_Text>().text = GetRandomLetter();
            tile.name = tile.GetComponentInChildren<TMP_Text>().text;
        }
    }


}