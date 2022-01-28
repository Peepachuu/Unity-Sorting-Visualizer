using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SortingVisualizer : MonoBehaviour
{
    [Range(5, 100)] [SerializeField] private int numberOfBars;
    [SerializeField] private int lowerBound;
    [SerializeField] private int upperBound;

    private int _maxValue = int.MinValue;
    private int _minValue = int.MaxValue;

    private readonly List<int> _lst = new List<int>();

    private const float DrawableAreaWidth = 20;
    private const float DrawableAreaHeight = 20;

    private const float StartingX = -10f;

    [SerializeField] private GameObject barPrefab;
    private readonly List<GameObject> _bars = new List<GameObject>();

    private readonly Color[] _colors =
    {
        new Color(0.5f, 0.5f, 0.5f, 1),
        new Color(0.625f, 0.625f, 0.625f, 1),
        new Color(0.75f, 0.75f, 0.75f, 1)
    };

    private const float TimeDelay = 0.02f;
    private bool _ascending = true;
    private bool _sorting;

    void Start()
    {
        SetUp();
    }

    void SetUp()
    {
        GenerateBars();
        GenerateList();
        DrawList();
    }

    void GenerateBars()
    {
        foreach (GameObject bar in _bars)
            Destroy(bar);
        
        _bars.Clear();
        
        for (int i = 0; i < numberOfBars; ++i)
        {
            _bars.Add(Instantiate(barPrefab));
            _bars[i].GetComponent<SpriteRenderer>().color = _colors[i % 3];
        }
    }
    void GenerateList()
    {
        _lst.Clear();
        for (int i = 0; i < numberOfBars; ++i)
        {
            _lst.Add(Random.Range(lowerBound, upperBound));
            _maxValue = Mathf.Max(_maxValue, _lst[i]);
            _minValue = Mathf.Min(_minValue, _lst[i]);
        }
    }
    
    void DrawList()
    {
        float width = DrawableAreaWidth / numberOfBars;
        float unitHeight = DrawableAreaHeight / (_maxValue - _minValue);
        for (int i = 0; i < numberOfBars; ++i)
        {
            _bars[i].transform.localScale = new Vector3(width, unitHeight * (_lst[i] - _minValue), 0);
            _bars[i].transform.position = new Vector3(StartingX + width/2 + width*i, 0, 0);
        }
    }

    void SwitchBars(int i, int j)
    {
        (_bars[i].transform.localScale, _bars[j].transform.localScale) =
            (_bars[j].transform.localScale, _bars[i].transform.localScale);

        (_bars[i].GetComponent<SpriteRenderer>().color, _bars[j].GetComponent<SpriteRenderer>().color) = 
            (_bars[j].GetComponent<SpriteRenderer>().color, _bars[i].GetComponent<SpriteRenderer>().color);
    }

    void Update()
    {
        GetInput();
    } 

    void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StopAllCoroutines();
            SetUp();
            _sorting = false;
        }
        else if (Input.GetKeyDown(KeyCode.A) && !_sorting)
            _ascending = true;
        else if (Input.GetKeyDown(KeyCode.D) && !_sorting)
            _ascending = false;
        else if (Input.GetKeyDown(KeyCode.B) && !_sorting)
        {
            _sorting = true;
            StartCoroutine(BubbleSort());
        }
        else if (Input.GetKeyDown(KeyCode.I) && !_sorting)
        {
            _sorting = true;
            StartCoroutine(InsertionSort());
        }
    }

    IEnumerator BubbleSort()
    {
        for (int i = 0; i < numberOfBars - 1; ++i)
        {
            for (int j = 0; j < numberOfBars - 1 - i; ++j)
            {
                if ((_lst[j] < _lst[j + 1] || !_ascending) && (_lst[j] > _lst[j + 1] || _ascending)) 
                    continue;
                
                (_lst[j], _lst[j + 1]) = (_lst[j + 1], _lst[j]);
                SwitchBars(j, j + 1);
                yield return new WaitForSeconds(TimeDelay);
            }
        }
    }

    IEnumerator InsertionSort()
    {
        for (int i = 1; i < numberOfBars; ++i)
        {
            int current = _lst[i];
            while (true)
            {
                bool ascendingSort = i > 0 && _lst[i - 1] > current && _ascending;
                bool descendingSort = i > 0 && _lst[i - 1] < current && !_ascending;

                if (!ascendingSort && !descendingSort)
                    break;

                _lst[i] = _lst[i - 1];
                --i;
                _lst[i] = current;
                SwitchBars(i, i + 1);
                yield return new WaitForSeconds(TimeDelay);
            }
        }
    }
    
}
