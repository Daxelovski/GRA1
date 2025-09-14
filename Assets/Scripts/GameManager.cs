using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private AudioSystem audioSystem;
    [SerializeField] private MoneySystem money;
    [SerializeField] private Score score;
    [SerializeField] private float timer;
    [SerializeField] private PlayerStats player;

    public bool nearPortal;

    [SerializeField] private GameObject portalWindow;
    [SerializeField] private Crystal[] crystals;
    [SerializeField] private bool holdKey;
    [SerializeField] private string nextScene;

    void Start()
    {
        Find();
        SetEvents();
    }

    void Update()
    {
        CheckPortal();
    }


    private void CheckPortal()
    {
        if(nearPortal == true)
        {
            if(portalWindow.activeSelf == false)
            {
                portalWindow.SetActive(true);
            }

            if(Input.GetKeyDown("e"))
            {
                holdKey = true;
            }

            if(Input.GetKeyUp("e"))
            {
                holdKey = false;
            }


        }
        else if(portalWindow.activeSelf == true)
        {
            portalWindow.SetActive(false);
        }

        if(holdKey == false)
        {
            timer = 0;
        }
        else
        {
            timer += Time.deltaTime;
        }

        if(timer >= 3)
        {
            player.Save();
            score.Save();
            money.Save();
            SceneManager.LoadScene(nextScene);
        }
    }

    public void Find()
    {
        crystals = GameObject.FindObjectsOfType<Crystal>();
    }

    public void SetEvents()
    {
        foreach(var item in crystals)
        {
            item.pickupEvent += AddScorceAndCurrency;
        }
    }

    private void AddScorceAndCurrency()
    {
        score.AddScore(50);
        money.AddCrystals(5);
        audioSystem.PlayAudio(1, 1);
    }

    public void CollectPotion()
    {
        audioSystem.PlayAudio(2, 1);
    }
}
