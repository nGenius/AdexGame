using UnityEngine;


public class GameStarter : MonoBehaviour
{
    private ContentsData data = new ContentsData();

    private void Start()
    {
        data.LoadData();
    }
}