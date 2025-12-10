// RingIMUreceiver.cs
// Brief: Receives IMU data from an XSENS DOT ring via an Android native plugin and exposes it to Unity scripts.
//
// This project packages the XSENS DOT Android native API into a Unity-friendly plugin so the sensor can be
// used from within Unity on Android devices. If you need to use the native Android SDK directly or require
// detailed sensor and SDK documentation, see the official XSENS DOT page:
// https://base.movella.com/s/xsens-dot-landing-page?language=en_US
//
// Usage notes:
// - The Android plugin class `com.example.xsensedot.UnityDotCommunication` is expected to provide a
// static instance and call `ReceiveRingIMU(string jsonData)` to deliver sensor payloads.
// - `ReceiveRingIMU` receives a JSON string that is deserialized into the nested `RingIMU` type
// (fields: `acc` and `gyr` arrays). Update parsing or schema if the plugin payload changes.
// - Attach this component to a GameObject in a scene on the Android build; the instance is a singleton
// and persists across scene loads.

using TMPro;
using UnityEngine;

public class RingIMUreceiver : MonoBehaviour
{
    private AndroidJavaClass UnityDotCommunication;
    private AndroidJavaObject _unityDotCommunicationInstance;

    // For quick visual debug in UI (optional)
    public TextMeshProUGUI sensorDataText;

    // Singleton instance
    private static RingIMUreceiver _instance;

    // Public property to access the singleton instance
    public static RingIMUreceiver Instance
    {
        get { return _instance; }
    }

    // Latest sensor data received from Android plugin
    private RingIMU _ringIMU;

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        // Ensure only one instance of the class exists and persist across scenes
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Instantiate the Android plugin class. The plugin is expected to create its base
        // and call back into Unity using the `ReceiveRingIMU` method to deliver sensor data.
        UnityDotCommunication = new AndroidJavaClass("com.example.xsensedot.UnityDotCommunication");
        _unityDotCommunicationInstance = UnityDotCommunication.CallStatic<AndroidJavaObject>("instnace");
    }

    // Called by the Android plugin to deliver IMU data as a JSON string
    public void ReceiveRingIMU(string jsonData)
    {
        Debug.Log("Received sensor data: " + jsonData);

        // Parse JSON into the RingIMU data structure. Adjust this if the plugin payload format changes.
        _ringIMU = JsonUtility.FromJson<RingIMU>(jsonData);

        if (sensorDataText != null)
        {
            sensorDataText.text = jsonData;
        }

        // TODO: add handling to convert arrays into Vector3, filter, or forward to other systems
    }

    // Public accessor for the latest IMU data
    public RingIMU ringIMU
    {
        get { return _ringIMU; }
    }

    // Serializable container matching expected JSON payload from the Android plugin
    [System.Serializable]
    public class RingIMU
    {
        public float[] acc; // accelerometer data (e.g., [x, y, z])
        public float[] gyr; // gyroscope data (e.g., [x, y, z])
    }
}