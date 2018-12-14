using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour {

    public float disarmingTime;
    private TextMesh timerText;
    private bool isGettingDisarmed;
    public GameObject[] disarmingAgents;

    public bool IsGettingDisarmed
    {
        get
        {
            return isGettingDisarmed;
        }

        set
        {
            isGettingDisarmed = value;
        }
    }

    void Start () {
        timerText = GetComponentInChildren<TextMesh>();
        disarmingAgents = new GameObject[3];
    }
	
	void Update () {

        CheckDisarmingAgents();

        if (IsGettingDisarmed)
        {
            if (disarmingTime >= 0f)
            {
                for (int i = 0; i < disarmingAgents.Length; ++i)
                {
                    if (disarmingAgents[i] != null)
                    {
                        disarmingTime -= Time.deltaTime;
                    }
                }
                timerText.text = "disarming..." + ((int)disarmingTime).ToString();
            }
            else
            {
                Destroy(gameObject);
                for (int i = 0; i < disarmingAgents.Length; ++i)
                {
                    if (disarmingAgents[i] != null)
                    {
                        disarmingAgents[i].GetComponent<Agent>().DisarmedSpike();
                    }
                }
            }
        }
        else
        {
            timerText.text = ((int)disarmingTime).ToString();
        }
	}

    public void GetSteppedByVip()
    {
        for (int i = 0; i < disarmingAgents.Length; ++i)
        {
            if (disarmingAgents[i] != null)
            {
                disarmingAgents[i].GetComponent<Agent>().DisarmedSpike();
            }
        }
    }

    public void GettingDisarmed(GameObject agent)
    {
        for (int i = 0; i < disarmingAgents.Length; ++i)
        {
            if (disarmingAgents[i] != null && disarmingAgents[i] == agent)
            {
                return;
            }
        }
        for (int i = 0; i < disarmingAgents.Length; ++i)
        {
            if (disarmingAgents[i] == null)
            {
                disarmingAgents[i] = agent;
                break;
            }
        }
    }
    public void CheckDisarmingAgents()
    {
        for (int i = 0; i < disarmingAgents.Length; ++i)
        {
            if (disarmingAgents[i] != null)
            {
                IsGettingDisarmed = true;
                return;
            }
        }
        IsGettingDisarmed = false;
    }
    public void RemoveDisarmingAgent(GameObject agent)
    {
        for (int i = 0; i < disarmingAgents.Length; ++i)
        {
            if (disarmingAgents[i] == agent)
            {
                disarmingAgents[i] = null;
                break;
            }
        }
    }
}
