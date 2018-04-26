using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public enum SignType
{
    Rubbish,
    Recycling,
    Sorted,
    Null
}

public class Sign
{
    public Text m_sign;
    public int m_itemMax = 0;

    public Sign(Text _sign, int _itemMax)
    {
        m_sign = _sign;
        m_itemMax = _itemMax;
    }
}

public enum GroupSize
{
    Null,
    House,
    Street,
    Town,
    City,
    Country
}

public class OutcomeManager : MonoBehaviour
{
    public static OutcomeManager Instance = null;

    [Header("Animation")]
    public Animator m_truck;
    public Animator m_piston;
    public Animator m_bin;

    [Header("Spawning")]
    public Transform m_rubbishSpawnPoint;
    private float m_remainingTime = 0.0f;
    private float m_spawnLength = 0.0f;
    private float m_totalSpawnLength = 0.0f;

    public List<GameObject> m_totalObjects = new List<GameObject>();
    public List<GameObject> m_currentRubbish = new List<GameObject>();
    public List<WeightObjectResult> m_weightObjectResults = new List<WeightObjectResult>();
    public GameObject m_moneyBag;
    public Transform m_rubbishHolder;

    public List<GameObject> m_sortedMoneyBags = new List<GameObject>();

    private bool m_spawnObjects = false;
    private bool m_spawnRubbish = false;
    private bool m_spawnRecycling = false;
    private bool m_spawnMoney = false;

    private bool m_rubbishSpawned = false;
    private bool m_showOutcome = false;
    private bool m_outcomeShown = false;

    private int m_rubbishPerRound;
    public int m_maxDumpPerRound = 10;
    private int m_spawnedRubbish = 0;

    private Dictionary<GroupSize, int> m_groupToPopulation = new Dictionary<GroupSize, int>();

    [Header("UI")]
    public SignType m_signType = SignType.Rubbish;
    
    public Text m_rubbishSign;
    public Text m_recyclingSign;
    public Text m_sortedSign;
    public Text m_weightSign;
    public Text m_moneySign;

    private int m_currentRubbishCount = 0;
    private int m_currentRecyclingCount = 0;
    private int m_currentSortCount = 0;
    private float m_currentWeight = 0.0f;

    public Canvas m_calculationUI;
    public Text m_peopleInGroupText;
    public Text m_peoplePerGroupText;
    public Text m_weightPerPersonText;
    public Text m_weightPerStreetText;
    public Dropdown m_peoplePerStreetField;
    public Button m_beginOutcomeButton;
    public Button m_endOutcomeButton;
    public GameObject m_objectBase;
    public Text m_remainWeightText;
    public GameObject m_infoText;

    [Header("Stats")]
    public float m_weightPerPerson = 0.0f;
    public float m_weightPerStreet = 0.0f;
    public int m_peoplePerStreet = 0;

    public float m_poundPerKg = 0.15f;

    void Awake()
    {
        CreateInstance();
    }

    void Start()
    {
        m_peoplePerStreetField.onValueChanged.AddListener(OnPeoplePerStreetChange);
        m_beginOutcomeButton.onClick.AddListener(BeginOutcome);
       
        List<Dropdown.OptionData> fileData = new List<Dropdown.OptionData>();
        fileData.Add(new Dropdown.OptionData(""));
        m_groupToPopulation.Add(GroupSize.Null, 0);
        fileData.Add(new Dropdown.OptionData(GroupSize.House.ToString()));
        m_groupToPopulation.Add(GroupSize.House, 1);
        fileData.Add(new Dropdown.OptionData(GroupSize.Street.ToString()));
        m_groupToPopulation.Add(GroupSize.Street, 100);
        fileData.Add(new Dropdown.OptionData(GroupSize.Town.ToString()));
        m_groupToPopulation.Add(GroupSize.Town, 1000);
        //fileData.Add(new Dropdown.OptionData(GroupSize.City.ToString()));
        //m_groupToPopulation.Add(GroupSize.City, 400000);
        //fileData.Add(new Dropdown.OptionData(GroupSize.Country.ToString()));
        //m_groupToPopulation.Add(GroupSize.Country, 65000000);
        m_peoplePerStreetField.AddOptions(fileData);
    }

    private void CreateInstance()
    {
        if(Instance)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void Begin()
    {
        m_totalObjects.AddRange(m_currentRubbish);
        m_totalObjects.AddRange(GameManager.Instance.m_landFilledRubbish);
        m_totalObjects.AddRange(GameManager.Instance.m_sortedRubbish);
        m_totalObjects.AddRange(GameManager.Instance.m_landFilledRecycling);
        m_totalObjects.AddRange(GameManager.Instance.m_sortedRecycling);

        foreach (GameObject @object in GameManager.Instance.m_sortedRubbish)
        {
            m_sortedMoneyBags.Add(m_moneyBag);
            m_totalObjects.Add(m_moneyBag);
        }

        foreach (GameObject @object in GameManager.Instance.m_sortedRecycling)
        {
            m_sortedMoneyBags.Add(m_moneyBag);
            m_totalObjects.Add(m_moneyBag);
        }

        m_currentRubbish.AddRange(GameManager.Instance.m_landFilledRubbish);
        m_currentRubbish.AddRange(GameManager.Instance.m_sortedRubbish);

        if (m_totalObjects.Count > m_maxDumpPerRound)
        {
            m_rubbishPerRound = Mathf.CeilToInt(m_totalObjects.Count / Mathf.CeilToInt((float)m_totalObjects.Count / m_maxDumpPerRound));
        }
        m_truck.Play("MoveTruckIn");
    }
	
	public void OnTruckInFinish()
    {
        m_piston.Play("MovePistonUp");
    }

    public void OnPistonUpFinish()
    {
        m_bin.Play("MoveBinIn");
    }

    public void OnBinIn()
    {
        m_bin.Play("DumpLoad");
        m_spawnRubbish = true;
    }

    public void OnDumpBegin(float _length)
    {
        if (!m_spawnObjects)
        {
            m_totalSpawnLength = _length;
            if (m_currentRubbish.Count == 0)
            {
                m_remainingTime = m_totalSpawnLength + 1.0f;
            }
            else if (m_currentRubbish.Count > m_maxDumpPerRound)
            {
                m_spawnLength = m_totalSpawnLength / m_rubbishPerRound;
                m_remainingTime = 0.0f;
            }
            else
            {
                m_spawnLength = m_totalSpawnLength / m_currentRubbish.Count;
                m_remainingTime = 0.0f;
            }
            m_spawnObjects = true;
        }
    }

    public void OnDumpEnd()
    {
        if(m_spawnedRubbish < m_totalObjects.Count)
        {
            m_bin.Play("DumpLoad", -1, 0.0f);
            m_bin.Play("DumpLoad");
        }
        else
        {
            m_spawnObjects = false;
            m_bin.Play("MoveBinOut");
        }
    }

    public void OnBinOut()
    {
        m_piston.Play("MovePistonDown");
        InitialiseOutcome();
    }

    void Update()
    {
        if (m_spawnObjects)
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                m_spawnRubbish = false;
                m_spawnRecycling = false;
                m_spawnMoney = false;
                m_spawnObjects = false;

                m_rubbishSign.text = "= " + (GameManager.Instance.m_landFilledRubbish.Count
                    + GameManager.Instance.m_sortedRubbish.Count).ToString();
                m_recyclingSign.text = "= " + (GameManager.Instance.m_landFilledRecycling.Count +
                    GameManager.Instance.m_sortedRecycling.Count).ToString();
                m_moneySign.text = (m_currentSortCount * m_poundPerKg).ToString("F2");
                m_sortedSign.text = "= " + (m_sortedMoneyBags.Count).ToString();

                m_remainingTime = 0.0f;
            }
            else
            {
                SpawnRubbish();
            }
        }

        if (m_showOutcome)
        {
            if (m_currentWeight >= m_weightPerStreet)
            {
                if (!m_endOutcomeButton.gameObject.activeInHierarchy)
                {
                    m_endOutcomeButton.gameObject.SetActive(true);
                    m_objectBase.SetActive(false);
                }
            }
        }
    }

    public void EndLevel()
    {
        HandleOutcome();
    }

    public void SpawnRubbish()
    {
        m_remainingTime -= Time.deltaTime;
        if (m_remainingTime <= 0.0f && m_currentRubbish.Count != 0)
        {
            GameObject rubbish = Instantiate(m_currentRubbish[0], m_rubbishSpawnPoint.position, Quaternion.identity, m_rubbishHolder);
            Vector3 normalScale = rubbish.transform.localScale;
            rubbish.transform.localScale = normalScale * 2.0f;
            rubbish.GetComponent<Rigidbody>().useGravity = true;
            m_currentRubbish.RemoveAt(0);
            m_spawnedRubbish++;

            if (m_spawnRubbish)
            {
                m_currentRubbishCount++;
                m_rubbishSign.text = "= " + m_currentRubbishCount.ToString();

                m_currentWeight += rubbish.GetComponent<Rubbish>().m_weight;
                m_weightSign.text = m_currentWeight.ToString("F2");
            }
            else if (m_spawnRecycling)
            {
                m_currentRecyclingCount++;
                m_recyclingSign.text = "= " + m_currentRecyclingCount.ToString();

                m_currentWeight += rubbish.GetComponent<Rubbish>().m_weight;
                m_weightSign.text = m_currentWeight.ToString("F2");
            }
            else if (m_spawnMoney)
            {
                m_currentSortCount++;
                m_sortedSign.text = "= " + (m_currentSortCount).ToString();

                m_moneySign.text = (m_currentSortCount * m_poundPerKg).ToString("F2");
            }
            else
            {
                m_remainingTime = m_spawnLength;
            }
        }

        if (m_currentRubbish.Count == 0)
        {
            if (m_spawnRubbish)
            {
                m_spawnRubbish = false;
                m_spawnRecycling = true;

                m_currentRubbish.AddRange(GameManager.Instance.m_landFilledRecycling);
                m_currentRubbish.AddRange(GameManager.Instance.m_sortedRecycling);

                m_remainingTime = m_spawnLength;
            }
            else if (m_spawnRecycling)
            {
                m_spawnRecycling = false;
                m_spawnMoney = true;

                m_currentRubbish.AddRange(m_sortedMoneyBags);

                m_remainingTime = m_spawnLength;
            }
            else if (m_spawnMoney)
            {
                m_spawnMoney = false;
                m_spawnObjects = false;
            }
        }
    }

    private void InitialiseOutcome()
    {
        m_weightPerPerson = m_currentWeight;
        m_weightPerPersonText.text = m_weightPerPerson.ToString("F2");
        m_weightPerStreet = 0.0f;
        m_weightPerStreetText.text = m_weightPerStreet.ToString("F2");
        m_peoplePerStreet = 0;
        m_calculationUI.gameObject.SetActive(true);
    }

    public void OnPeoplePerStreetChange(int _input)
    {
        m_peoplePerStreet = m_groupToPopulation[(GroupSize)_input];
        m_peopleInGroupText.text = m_peoplePerStreet.ToString();
        m_peoplePerGroupText.text = m_peoplePerStreet.ToString();
        m_weightPerStreet = m_peoplePerStreet * m_weightPerPerson;
        m_weightPerStreetText.text = m_weightPerStreet.ToString();
        if (m_peoplePerStreet != 1)
        {
            m_beginOutcomeButton.gameObject.SetActive(true);
            m_infoText.SetActive(false);
        }
        else
        {
            m_beginOutcomeButton.gameObject.SetActive(false);
            m_infoText.SetActive(true);
        }
    }

    //public void OnPeoplePerStreetChange(string _input)
    //{
    //    m_peoplePerStreet = int.Parse(_input);
    //    m_weightPerStreet = m_peoplePerStreet * m_weightPerPerson;
    //    m_weightPerStreetText.text = m_weightPerStreet.ToString();
    //    if (m_peoplePerStreet != 0)
    //    {
    //        m_beginOutcomeButton.gameObject.SetActive(true);
    //    }
    //    else
    //    {
    //        m_beginOutcomeButton.gameObject.SetActive(false);
    //    }
    //}

    public void BeginOutcome()
    {
        m_calculationUI.gameObject.SetActive(false);        
        Destroy(m_rubbishHolder.gameObject);
        m_rubbishHolder = null;
        m_currentRubbish.Clear();
        m_currentWeight = 0.0f;
        m_weightSign.text = m_currentWeight.ToString("F2");
        m_remainWeightText.text = m_weightPerStreet.ToString("F2");
        m_objectBase.SetActive(true);
        m_showOutcome = true;
    }

    private void HandleOutcome()
    {
        m_outcomeShown = true;
        CreateWeekResultsData();
        SceneChanger.TransitionScene("Results");
    }

    private void CreateWeekResultsData()
    {
        ResultData data = new GameObject("ResultsData").AddComponent<ResultData>();

        data.Initalise(m_weightPerPerson, m_weightPerStreet, m_currentRubbishCount,
            m_currentRecyclingCount, m_currentSortCount,
            m_weightObjectResults, m_currentSortCount * m_poundPerKg, Time.time - GameManager.Instance.m_startTime);
        
        DataHandler.SaveCategoricData("WeekResults", GameManager.CurrentOutFolderPath, GenerateWeekData(data));
        DataHandler.SaveCategoricData("WeekResults", GameManager.CurrentInFolderPath, GenerateWeekData(data));
    }

    private List<CategoricData.CategoricPair> GenerateWeekData(ResultData _data)
    {
        List<CategoricData.CategoricPair> data = new List<CategoricData.CategoricPair>();
        data.Add(new CategoricData.CategoricPair("WeightPerPersonWeek", _data.m_weightPerPerson));
        data.Add(new CategoricData.CategoricPair("WeightPerStreetWeek", _data.m_weightPerStreet));

        data.Add(new CategoricData.CategoricPair("LandfilledRubbishWeek", _data.m_landfileldRubbish));
        data.Add(new CategoricData.CategoricPair("LandfilledRecyclingWeek", _data.m_landfilledRecycling));
        data.Add(new CategoricData.CategoricPair("SortedRubbishWeek", _data.m_sortedRubbish));
        data.Add(new CategoricData.CategoricPair("MoneySpentWeek", _data.m_moneySpent));

        data.Add(new CategoricData.CategoricPair("PlaySessionLength", _data.m_playSessionLength));
        return data;
    }

    public void AddWeightResult(Sprite _icon, float _weight)
    {
        m_currentWeight += _weight;
        m_weightSign.text = m_currentWeight.ToString("F2");
        m_remainWeightText.text = (m_weightPerStreet - m_currentWeight).ToString("F2");

        foreach (WeightObjectResult result in m_weightObjectResults)
        {
            if(result.m_icon == _icon)
            {
                result.m_count++;
                return;
            }
        }
        m_weightObjectResults.Add(new WeightObjectResult(_icon));
    }
}
