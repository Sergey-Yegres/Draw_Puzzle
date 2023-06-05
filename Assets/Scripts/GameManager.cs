using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private List<CharacterLogic> _allCharacters = new List<CharacterLogic>();

    [SerializeField]
    private GameObject _loseScreen, _winScreen;
    [Range(1f, 10f)]
    public float MoveSpeed;
    public void AddNewCharacterInList(CharacterLogic newCharacter)
    {
        if (!_allCharacters.Contains(newCharacter))
            _allCharacters.Add(newCharacter);
    }
    public void OpenLoseScreen()
    {
        _loseScreen.SetActive(true);
    }
    public void OpenWinScreen()
    {
        _winScreen.SetActive(true);
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
    public void CheckAllCharactersForPossibilityMove()
    {
        bool _canMoveAllCharacters = true;
        foreach (CharacterLogic character in _allCharacters)
        {
            if (!character.LineAlreadyDrawn)
            {
                _canMoveAllCharacters = false;
                break;
            }
        }

        if (_canMoveAllCharacters)
        {
            foreach (CharacterLogic character in _allCharacters)
                character.StartMove();
        }
    }
    public void CheckAllCharactersForWin()
    {
        bool _canWinAllCharacters = true;
        foreach (CharacterLogic character in _allCharacters)
        {
            if (!character.CharacterWin)
            {
                _canWinAllCharacters = false;
                break;
            }
        }

        if (_canWinAllCharacters)
            OpenWinScreen();
    }
    private void Awake()
    {
        if (Instance != null)
            Destroy(this);
        Instance = this;
    }
}
