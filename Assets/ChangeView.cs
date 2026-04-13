using UnityEngine;

public class ChangeView : MonoBehaviour
{
    public Camera[] cameras;
    private int currentIndex = 0;

    void Start()
    {
        ActivateCamera(currentIndex);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            currentIndex++;
            if (currentIndex >= cameras.Length)
            {
                currentIndex = 0;
            }

            ActivateCamera(currentIndex);
        }
    }

    void ActivateCamera(int index)
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(i == index);
        }
    }
}
