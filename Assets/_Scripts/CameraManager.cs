using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] GameObject mainCam;
    [SerializeField] GameObject sideCam;
    public static CameraManager i;

    private void Awake()
    {
        i = this;
    }

    private void Start()
    {
        mainCam.GetComponent<CinemachineVirtualCamera>().Follow =Ambulance.i.transform;
        mainCam.GetComponent<CinemachineVirtualCamera>().LookAt =Ambulance.i.transform;

        sideCam.GetComponent<CinemachineVirtualCamera>().Follow =Ambulance.i.transform;
        sideCam.GetComponent<CinemachineVirtualCamera>().LookAt =Ambulance.i.transform;
    }

    public void SwitchCamera()
    {
        if (!sideCam.activeSelf)
        {
            sideCam.SetActive(true);
        }
        else
        {
            sideCam.SetActive(false);
        }
    }
}
