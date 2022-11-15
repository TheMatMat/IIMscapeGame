using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardBuilder : MonoBehaviour
{
    public static BoardBuilder _instance;

    [SerializeField]
    private List<BoardSettings> boardSettings;

    public Board board;

    // Start is called before the first frame update
    void Start()
    {
        if (_instance == null)
            _instance = this;

        BoardSettings _selectedSetting;

        //load allboard settings
        foreach(BoardSettings _boardSetting in Resources.LoadAll<BoardSettings>("SO"))
        {
            boardSettings.Add(_boardSetting);
        }

        //select one
        if(boardSettings.Count > 0)
        {
            _selectedSetting = boardSettings[Random.Range(0, boardSettings.Count - 1)];

            if(_selectedSetting != null)
                InstantiateBoard(_selectedSetting);
        }
        else
        {
            Debug.Log("Cannot spawn Board");
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InstantiateBoard(BoardSettings _bs)
    {
        Board newBoardManager = Instantiate(board);
        newBoardManager.BS = _bs;
    }
}
