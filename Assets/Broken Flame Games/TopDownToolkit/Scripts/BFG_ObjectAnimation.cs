// ================================================
// 
//            Script Name: BFG_ObjectAnimation
//            Creator: Burning Flame Games
//            Description: Put script onto a object to animate it
// 
// ================================================
using UnityEngine;
using UnityEditor;
using System;
using BrokenFlameGames;
namespace BrokenFlameGames
{
    public class BFG_ObjectAnimation : MonoBehaviour
{

    [Tooltip("How high the object moves up and down")]
    public float bobbingHeight = 0.5f;
    [Tooltip("Speed of the bobbing animation")]
    public float bobbingSpeed = 1f;

    [Tooltip("Speed of the rotation animation")]
    public float rotationSpeed = 30f;
    [Tooltip("How much the object rotates on each axis (degrees per second)")]
    public Vector3 rotationAmount = new Vector3(0, 180, 0);

    private Vector3 startPosition;
    private float randomOffset;

    private void Start()
    {
        startPosition = transform.position;
        randomOffset = UnityEngine.Random.Range(0f, 2f * Mathf.PI); // Add some randomness to the bobbing
    }

    private void Update()
    {
        // Bobbing animation
        float newY = startPosition.y + Mathf.Sin((Time.time + randomOffset) * bobbingSpeed) * bobbingHeight;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);

        // Rotation animation
        Vector3 rotationThisFrame = rotationAmount * rotationSpeed * Time.deltaTime;
        transform.Rotate(rotationThisFrame);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(BFG_ObjectAnimation)), InitializeOnLoadAttribute]
public class BFG_ObjectAnimationEditor : Editor
{
    BFG_ObjectAnimation animationScript;
    SerializedObject SerObj;

    SerializedProperty bobbingHeight;
    SerializedProperty bobbingSpeed;
    SerializedProperty rotationSpeed;
    SerializedProperty rotationAmount;

    private void OnEnable()
    {
        animationScript = (BFG_ObjectAnimation)target;
        SerObj = new SerializedObject(target);

        // Get all serialized properties
        bobbingHeight = SerObj.FindProperty("bobbingHeight");
        bobbingSpeed = SerObj.FindProperty("bobbingSpeed");
        rotationSpeed = SerObj.FindProperty("rotationSpeed");
        rotationAmount = SerObj.FindProperty("rotationAmount");
    }

    public override void OnInspectorGUI()
    {
        // Update the serialized object
        SerObj.Update();
        
        GUILayout.Label("Object Animation", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 16 }, GUILayout.ExpandWidth(true));
        GUILayout.Label("By Broken Flame Games", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = 11 }, GUILayout.ExpandWidth(true));
        GUILayout.Label("V1.0", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = 11 }, GUILayout.ExpandWidth(true));
        EditorGUILayout.HelpBox("Hover Over Anything to See What it Does!", MessageType.Warning);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.Space();
        
        GUILayout.Label("Bobbing Settings", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(bobbingHeight, new GUIContent("Bobbing Height", "How high the object moves up and down"));
        EditorGUILayout.PropertyField(bobbingSpeed, new GUIContent("Bobbing Speed", "Speed of the bobbing animation"));
        EditorGUILayout.Space();
        
        GUILayout.Label("Rotation Settings", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(rotationSpeed, new GUIContent("Rotation Speed", "Speed multiplier for the rotation"));
        EditorGUILayout.PropertyField(rotationAmount, new GUIContent("Rotation Amount", "How much the object rotates on each axis (degrees per second)"));
        
        // Apply changes
        SerObj.ApplyModifiedProperties();
    }   
}
#endif
}
