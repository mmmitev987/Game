using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public GameObject aboveHeadIcon;
    public GameObject aboveHeadSceneIcon;

    public void NotifyPLayer(int index) 
    { 
        if (index == 0)
        {
            aboveHeadIcon.SetActive(true);
        }
        else
        {
            aboveHeadSceneIcon.SetActive(true);
        }
    }
    public void DeNotifyPLayer(int index)
    {
        if (index == 0)
        {
            aboveHeadIcon.SetActive(false);
        }
        else
        {
            aboveHeadSceneIcon.SetActive(false);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
