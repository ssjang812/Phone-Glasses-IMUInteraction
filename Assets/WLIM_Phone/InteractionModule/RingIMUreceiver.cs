using TMPro;
using UnityEngine;

public class RingIMUreceiver : MonoBehaviour
{
    private AndroidJavaClass UnityDotCommunication;
    private AndroidJavaObject _unityDotCommunicationInstance;

    // 테스트용 임시
    public TextMeshProUGUI sensorDataText;

    // Singleton instance
    private static RingIMUreceiver _instance;

    // Public property to access the singleton instance
    public static RingIMUreceiver Instance
    {
        get { return _instance; }
    }

    // Sensor data received from Android
    private RingIMU _ringIMU;

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        // Ensure only one instance of the class exists
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Keep the object alive between scene changes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    void Start()
    {
        // 플러그인의 UnidyDotCommucation을 intantiate하면, 거기서 Base를 instantiate, 하단의 ReceiveRingIMU를 호출함
        UnityDotCommunication = new AndroidJavaClass("com.example.xsensedot.UnityDotCommunication");
        _unityDotCommunicationInstance = UnityDotCommunication.CallStatic<AndroidJavaObject>("instnace");
    }

    // 안드로이드에서 전달된 센서 데이터를 처리하는 함수
    public void ReceiveRingIMU(string jsonData)
    {
        // 받은 데이터 처리
        Debug.Log("Received sensor data: " + jsonData);

        // JSON 형식의 데이터를 필요한 형태로 파싱하여 활용할 수 있습니다.
        // 예시: JSON 문자열을 다시 객체로 변환
        _ringIMU = JsonUtility.FromJson<RingIMU>(jsonData);
        sensorDataText.text = jsonData;
        // 여기에 데이터를 활용하는 코드 작성
    }

    // 외부에서 RingIMU에 접근할 수 있는 프로퍼티
    public RingIMU ringIMU
    {
        get { return _ringIMU; }
    }

    // 안드로이드에서 전송한 센서 데이터를 파싱하기 위한 클래스
    [System.Serializable]
    public class RingIMU
    {
        public float[] acc;
        public float[] gyr;
    }
}