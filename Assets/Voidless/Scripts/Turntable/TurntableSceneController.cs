using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor.Recorder;
using UnityEditor;
#endif

namespace Voidless
{
public enum CameraShot
{
    Free,
    IsometricLeft,
    IsometricRight,
    Top,
    Front,
    Left,
    Right
}

[ExecuteInEditMode]
public class TurntableSceneController : MonoBehaviour
{
    [SerializeField] private bool rotate;
    [SerializeField] private CameraShot cameraShot;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 pointOfInterest;
    [SerializeField] private float distance;
    [SerializeField] private float height;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private int rotations;
    [SerializeField] private int frameRate;
    private Coroutine rotation;

    /// <summary>Draws Gizmos on Editor mode when TurntableSceneController's instance is selected.</summary>
    private void OnDrawGizmosSelected()
    {
        if(target == null) return;
        Gizmos.DrawSphere(target.TransformPoint(pointOfInterest), 0.2f);
    }

    /// <summary>TurntableSceneController's instance initialization when loaded [Before scene loads].</summary>
    private void Awake()
    {
        Application.targetFrameRate = frameRate;
    }

    /// <summary>Callback invoked when scene loads, one frame before the first Update's tick.</summary>
    private void Start()
    {
        if(Application.isPlaying && rotate) this.StartCoroutine(RotationRoutine(), ref rotation);
    }

    /// <summary>Updates TurntableSceneController's instance at each frame.</summary>
    private void Update()
    {
        if(target == null) return;

        if(Application.isPlaying && rotate) target.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        AdjustCamera();
    }

    private void AdjustCamera()
    {
        if(target == null) return;

        switch(cameraShot)
        {
            case CameraShot.Free:
            break;

            case CameraShot.IsometricLeft:
                SetOffset(new Vector3(-1.0f, 0.0f, 1.0f));
            break;

            case CameraShot.IsometricRight:
                SetOffset(new Vector3(1.0f, 0.0f, 1.0f));
            break;

            case CameraShot.Top:
                SetOffset(new Vector3(0.0f, 1.0f, 0.0f));
            break;

            case CameraShot.Front:
                SetOffset(new Vector3(0.0f, 0.0f, 1.0f));
            break;

            case CameraShot.Left:
                SetOffset(new Vector3(-1.0f, 0.0f, 1.0f));
            break;

            case CameraShot.Right:
                SetOffset(new Vector3(1.0f, 0.0f, 1.0f));
            break;
        }

        Transform cameraTransform = Camera.main.transform;
        Vector3 direction = target.TransformPoint(pointOfInterest) - cameraTransform.position;
        
        cameraTransform.rotation = Quaternion.LookRotation(direction);
    }

    [Button("Take Screenshot")]
    private void TakeScreenshot(string name)
    {
        ScreenCapture.CaptureScreenshot(Application.dataPath + "/Screenshots/" + name + ".png");
        AssetDatabase.Refresh();
    }

    private void SetOffset(Vector3 offset)
    {
        SetOffset(offset, distance, height);
    }

    private void SetOffset(Vector3 offset, float distance, float height)
    {
        offset.Normalize();
        Camera.main.transform.position = target.position + (offset * distance) + (Vector3.up * height);
    }

    /// <summary>Turntable's Main Routine.</summary>
    private IEnumerator RotationRoutine()
    {
        float t = 0.0f;
        float s = (float)rotations;
        float c = (360.0f / rotationSpeed) * s;

        while(t < c)
        {
            t += Time.deltaTime;
            yield return null;
        }

#if UNITY_EDITOR
        RecorderWindow window = (RecorderWindow)EditorWindow.GetWindow(typeof(RecorderWindow));

        if(window == null) yield break;

        if(window.IsRecording()) window.StopRecording();
#endif
    }
}

[Serializable]
public struct CameraShotData
{
    //public Vector3 pointOfInterest;
    public float height;
    public float distance;
}
}