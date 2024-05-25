using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LetterGrid : MonoBehaviour
{
    // SerializeField for different types of letter tiles
    [SerializeField]
    private GameObject letterTileBronze;

    [SerializeField]
    private GameObject letterTileSilver;

    [SerializeField]
    private GameObject letterTileGold;

    // SerializeField for different types of letter tile sprites
    [SerializeField]
    Sprite bronzeTile;

    [SerializeField]
    Sprite silverTile;

    [SerializeField]
    Sprite goldTile;

    // Size of the grid
    [SerializeField]
    private int gridSize = 4;

    // Background image for the grid
    [SerializeField]
    private GameObject backgroundImage;

    // Attack button
    [SerializeField]
    private Button attackButton;

    // Singleton instance
    private static LetterGrid instance;

    // List of letter tiles
    private List<GameObject> letterTiles;

    // List of selected tiles
    private List<GameObject> selectedTiles;

    // GameObject for selected tiles container
    private GameObject selectedContainer;

    // Dictionary to store original tile positions
    private Dictionary<GameObject, Vector3> originalTilePositions;

    // Main camera
    private Camera mainCamera;

    // Audio clip when a tile is selected
    [SerializeField]
    private AudioClip tileSelectSound;

    // Audio clip for valid word
    [SerializeField]
    private AudioClip wordValidSound;

    // Audio source for playing audio clips
    [SerializeField]
    private AudioSource audioSource;

    // Dictionary to store letter values
    private Dictionary<char, double> letterValues = new Dictionary<char, double>()
    {
        // Bronze value
        { 'A', 1 },
        { 'D', 1 },
        { 'E', 1 },
        { 'G', 1 },
        { 'I', 1 },
        { 'L', 1 },
        { 'N', 1 },
        { 'O', 1 },
        { 'R', 1 },
        { 'S', 1 },
        { 'T', 1 },
        { 'U', 1 },
        { 'B', 1.25 },
        { 'C', 1.25 },
        { 'F', 1.25 },
        { 'H', 1.25 },
        { 'M', 1.25 },
        { 'P', 1.25 },
        // Silver value
        { 'V', 1.5 },
        { 'W', 1.5 },
        { 'Y', 1.5 },
        { 'J', 1.75 },
        { 'K', 1.75 },
        { 'Q', 1.75 },
        //Gold value
        { 'X', 2 },
        { 'Z', 2 },
    };

    // Letter pool for better distribution of letters
    private List<string> LetterPool = new List<string>
    {
        "E",
        "E",
        "E",
        "E",
        "E",
        "E",
        "E",
        "E",
        "E",
        "E",
        "E",
        "E",
        "A",
        "A",
        "A",
        "A",
        "A",
        "A",
        "A",
        "A",
        "A",
        "I",
        "I",
        "I",
        "I",
        "I",
        "I",
        "I",
        "I",
        "I",
        "O",
        "O",
        "O",
        "O",
        "O",
        "O",
        "O",
        "O",
        "N",
        "N",
        "N",
        "N",
        "N",
        "N",
        "N",
        "N",
        "R",
        "R",
        "R",
        "R",
        "R",
        "R",
        "R",
        "T",
        "T",
        "T",
        "T",
        "T",
        "T",
        "L",
        "L",
        "L",
        "L",
        "S",
        "S",
        "S",
        "S",
        "U",
        "U",
        "U",
        "D",
        "D",
        "D",
        "G",
        "G",
        "B",
        "B",
        "C",
        "C",
        "M",
        "M",
        "P",
        "P",
        "F",
        "F",
        "H",
        "H",
        "V",
        "V",
        "W",
        "W",
        "Y",
        "Y",
        "K",
        "J",
        "X",
        "Q",
        "Z"
    };

    // Get the instance of the LetterGrid
    public static LetterGrid Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LetterGrid>();
                if (instance == null)
                {
                    Debug.LogError("No LetterGrid found in scene. Creating instance.");
                    instance = new LetterGrid();
                }
            }
            return instance;
        }
    }

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        if (instance != null && instance != this)
        {
            DestroyImmediate(gameObject); // Destroy duplicate if it exists
            return;
        }

        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Setup();
        GenerateLetterGrid();
    }

    // Raycast to check if a tile is clicked
    private void Update()
    {
        // Check if the left mouse button is clicked
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitObject = hit.collider.gameObject;
                if (hitObject.CompareTag("LetterTile"))
                {
                    // Debug.Log("Hit something");
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

    // Get all required components and setup the game
    void Setup()
    {
        letterTiles = new List<GameObject>();
        selectedTiles = new List<GameObject>();
        originalTilePositions = new Dictionary<GameObject, Vector3>();
        attackButton = GameObject.Find("AttackButton").GetComponent<UnityEngine.UI.Button>();
        selectedContainer = GameObject.Find("SelectedContainer");
        backgroundImage.SetActive(true);
        mainCamera = Camera.main;
        audioSource = GetComponent<AudioSource>();
    }

    // Generate a grid of letter tiles.
    void GenerateLetterGrid()
    {
        // This variable is used to adjust the spacing between rows.
        float rowSpace = -0.6f;

        // Loop over each row in the grid.
        for (int row = 0; row < gridSize; row++)
        {
            // Increase the row space by 0.15 for each new row.
            rowSpace = rowSpace + .15f;

            // Loop over each column in the grid.
            for (int col = 0; col < gridSize; col++)
            {
                // Create a new tile and add it to the list of letter tiles.
                GameObject newTile = CreateNewTile();
                letterTiles.Add(newTile);

                // Set the parent of the new tile to be this object (the grid).
                newTile.transform.SetParent(transform);

                // Set the text of the tile to be its name.
                newTile.GetComponentInChildren<TMP_Text>().text = newTile.name;

                // Set the local position of the tile in the grid.
                newTile.transform.localPosition = new Vector3(col + col * .15f, row + rowSpace, 0);

                // Set the name of the tile to be its text.
                //newTile.name = newTile.GetComponentInChildren<TMP_Text>().text;
            }
        }

        // Store the original positions of all the tiles.
        foreach (GameObject tile in letterTiles)
        {
            originalTilePositions.Add(tile, tile.transform.localPosition);
        }

        // Check if the letter grid is valid.
        if (IsLetterGridValid())
        {
            // If it is, log the list of letters.
            Debug.Log("Letter list: " + GetLetterList());
        }
        else
        {
            // If it's not, scramble the letters.
            ScrambleLetter();
        }
    }

    // Create a new tile with a random letter.
    GameObject CreateNewTile()
    {
        // Generate a random letter.
        string l = GetRandomLetter();

        // Calculate the value of the letter.
        double charValue = GetCharValue(l);

        // Declare a new GameObject to hold the new tile.
        GameObject newTile;

        // If the value of the letter is less than 1.5, create a bronze tile.
        if (charValue < 1.5f)
        {
            newTile = Instantiate(letterTileBronze);
        }
        // If the value of the letter is less than 2, create a silver tile.
        else if (charValue < 2f)
        {
            newTile = Instantiate(letterTileSilver);
        }
        // Otherwise, create a gold tile.
        else
        {
            newTile = Instantiate(letterTileGold);
        }

        // Return the new tile.
        return newTile;
    }

    // Generate a random letter.
    string GetRandomLetter()
    {
        // char randomLetter = (char)UnityEngine.Random.Range(65, 91);

        // return randomLetter.ToString();

        string randomLetter = LetterPool[UnityEngine.Random.Range(0, LetterPool.Count)];

        return randomLetter;
    }

    // Get the value of a character.
    double GetCharValue(string s)
    {
        char character = s.ToUpper().ToCharArray()[0];
        if (letterValues.ContainsKey(character))
            return 0 + letterValues[character];
        else
            return 0f;
    }

    // Get the value of a word
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

    // Get the value of the selected word
    public int GetSelectedWordValue()
    {
        string selectedWord = BuildSeletedWord();

        if (selectedWord.Length > 0)
        {
            if (WordChecker.Instance.IsValidWord(selectedWord))
            {
                return GetWordValue(selectedWord);
            }
            else
            {
                return 0;
            }
        }
        else
            return 0;
    }

    // Check if a tile is selected
    bool IsTileSelected(GameObject tile)
    {
        return selectedTiles.Contains(tile);
    }

    // Deselect a tile.
    void DeselectTile(GameObject tile)
    {
        // Play the tile selection sound.
        SoundManager.Instance.PlaySound(audioSource, tileSelectSound);

        // Get the index of the tile in the selected tiles list.
        int tileIndex = selectedTiles.IndexOf(tile);

        // Remove the tile from the selected tiles list.
        selectedTiles.Remove(tile);

        // If the original position of the tile is known...
        if (originalTilePositions.ContainsKey(tile))
        {
            // ...move the tile back to its original position in the grid.
            tile.transform.SetParent(transform);
            tile.transform.localPosition = originalTilePositions[tile];
        }
        else
        {
            // If the original position of the tile is not known, log an error.
            Debug.LogError("Original position not found for tile: " + tile.name);
        }

        // If there are tiles to the right of the deselected tile in the selected tiles list...
        if (tileIndex < selectedTiles.Count)
        {
            // ...loop over each of these tiles.
            for (int i = tileIndex; i < selectedTiles.Count; i++)
            {
                // Get the tile to move.
                GameObject tileToMove = selectedTiles[i];

                // Remove the tile from the selected tiles list.
                selectedTiles.RemoveAt(i);

                // If the original position of the tile is known...
                if (originalTilePositions.ContainsKey(tileToMove))
                {
                    // ...move the tile back to its original position in the grid.
                    tileToMove.transform.SetParent(transform);
                    tileToMove.transform.localPosition = originalTilePositions[tileToMove];
                }
                else
                {
                    // If the original position of the tile is not known, log an error.
                    Debug.LogError("Original position not found for tile: " + tileToMove.name);
                }

                // Decrement i by 1 to account for the removed item at index i during the loop iteration.
                i--;
            }
        }

        // Check if the selected word is valid.
        CheckWordValidity();
    }

    // Select a tile.
    void SelectTile(GameObject tile)
    {
        // Play the tile selection sound.
        SoundManager.Instance.PlaySound(audioSource, tileSelectSound);

        // Add the tile to the selected tiles list.
        selectedTiles.Add(tile);

        // Move the tile to the selected container.
        tile.transform.SetParent(selectedContainer.transform);

        // If this is the first tile in the selected tiles list...
        if (selectedTiles.Count == 1)
        {
            // ...set the position of the tile to the position of the selected container.
            tile.transform.localPosition = selectedContainer.transform.position;
        }
        else
        {
            // If this is not the first tile in the selected tiles list...

            // Get the previous tile in the selected tiles list.
            int previousTileIndex = selectedTiles.Count - 2;
            GameObject previousTile = selectedTiles[previousTileIndex];

            // Calculate the x position of the tile based on the position of the previous tile and the tile width.
            float xPos = previousTile.transform.localPosition.x;

            // Set the position of the tile to the right of the previous tile.
            tile.transform.localPosition = new Vector3(
                xPos + 1, // Add 1 to the x position of the previous tile.
                selectedContainer.transform.position.y, // The y position is the same as the selected container.
                0 // The z position is 0.
            );
        }

        // Check if the selected word is valid.
        CheckWordValidity();
    }

    // Build the selected word from the selected tiles.
    string BuildSeletedWord()
    {
        string selectedWord = "";

        foreach (GameObject tile in selectedTiles)
        {
            selectedWord += tile.GetComponentInChildren<TMP_Text>().text;
        }

        return selectedWord;
    }

    // Get the selected word
    public string GetSelectedWord()
    {
        return BuildSeletedWord();
    }

    // Check the validity of the selected word.
    void CheckWordValidity()
    {
        // Build the selected word from the selected tiles, convert it to lower case and trim any leading or trailing whitespace.
        string selectedWord = BuildSeletedWord().ToLower().Trim();

        // Get the color block of the attack button.
        ColorBlock attackButtonCB = attackButton.colors;

        // If the selected word is a valid word according to the WordChecker...
        if (WordChecker.Instance.IsValidWord(selectedWord))
        {
            // ...play the word valid sound.
            SoundManager.Instance.PlaySound(audioSource, wordValidSound);

            // Log that the word is valid.
            Debug.Log("Valid word: " + selectedWord);

            // Enable the attack button.
            attackButton.enabled = true;

            // Increase the color multiplier of the attack button to make it more visible.
            attackButtonCB.colorMultiplier = 2;

            // Apply the updated color block to the attack button.
            attackButton.colors = attackButtonCB;
        }
        else
        {
            // If the selected word is not valid...

            // Disable the attack button.
            attackButton.enabled = false;

            // Reset the color multiplier of the attack button.
            attackButtonCB.colorMultiplier = 1;

            // Apply the updated color block to the attack button.
            attackButton.colors = attackButtonCB;
        }
    }

    // Check if the letter grid contains at least one vowel
    bool IsLetterGridValid()
    {
        string letterList = GetLetterList();
        bool hasVowel = false;

        foreach (char letter in letterList)
        {
            if (letter == 'A' || letter == 'E' || letter == 'I' || letter == 'O' || letter == 'U')
            {
                hasVowel = true;
                break; // Exit the loop once a vowel is found
            }
        }

        return hasVowel;
    }

    // Reset the tile
    private void ResetTile(GameObject tile)
    {
        // Generate a random letter
        string letter = GetRandomLetter();

        // Get the value of the generated letter
        double letterValue = GetCharValue(letter);

        // Set the text of the tile to the generated letter
        tile.GetComponentInChildren<TMP_Text>().text = letter;

        // Set the name of the tile to the generated letter
        tile.name = letter;

        // Get the SpriteRenderer component of the tile
        SpriteRenderer spriteRenderer = tile.GetComponentInChildren<SpriteRenderer>();

        // Change the sprite of the tile based on the value of the letter
        if (letterValue < 1.5f)
        {
            // If the letter value is less than 1.5, set the sprite to bronzeTile
            spriteRenderer.sprite = bronzeTile;
        }
        else if (letterValue < 2f)
        {
            // If the letter value is less than 2 but not less than 1.5, set the sprite to silverTile
            spriteRenderer.sprite = silverTile;
        }
        else
        {
            // If the letter value is 2 or more, set the sprite to goldTile
            spriteRenderer.sprite = goldTile;
        }
    }

    // Scramble the letters in the grid.
    public void ScrambleLetter()
    {
        // If there are any selected tiles...
        if (selectedTiles.Count != 0)
        {
            // ...deselect the first tile in the selected tiles list.
            DeselectTile(selectedTiles[0]);
        }

        // Loop over each tile in the grid.
        foreach (GameObject tile in letterTiles)
        {
            // Reset the tile.
            ResetTile(tile);
        }

        // If the letter grid is valid...
        if (IsLetterGridValid())
        {
            // ...log the list of letters in the grid.
            Debug.Log("Letter list: " + GetLetterList());
        }
        else
        {
            // If the letter grid is not valid, scramble the letters again.
            ScrambleLetter();
        }
    }

    // This method resets the selected tiles in the grid.
    public void ResetSelectedTiles()
    {
        // If there are any selected tiles...
        if (selectedTiles.Count != 0)
        {
            // ...loop over each selected tile.
            foreach (GameObject tile in selectedTiles)
            {
                // Reset the tile.
                ResetTile(tile);
            }

            // Deselect the first tile in the selected tiles list.
            DeselectTile(selectedTiles[0]);
        }

        // If the letter grid is valid...
        if (IsLetterGridValid())
        {
            // ...log the list of letters in the grid.
            Debug.Log("Letter list: " + GetLetterList());
        }
        else
        {
            // If the letter grid is not valid, scramble the letters again.
            ScrambleLetter();
        }
    }

    string GetLetterList()
    {
        string letters = "";
        foreach (GameObject tile in letterTiles)
        {
            letters += tile.name;
        }
        return letters;
    }
}
