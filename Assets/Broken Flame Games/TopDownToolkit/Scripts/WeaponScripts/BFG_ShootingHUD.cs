using UnityEngine;
using System.Collections;
using UnityEditor;
using TMPro;
using BrokenFlameGames;
namespace BrokenFlameGames
{
    public class BFG_ShootingHUD : MonoBehaviour
    {
        [SerializeField] public BFG_ShootingManager shootMan;
        [SerializeField] public TextMeshProUGUI bulletText;
        void Update()
        {
            UpdateBulletText();
        }

        void UpdateBulletText()
        {
            string firstPart = shootMan.infiniteAmmo ? "\u221E" : shootMan.CurrentMagCapacity.ToString();
            string secondPart = shootMan.infiniteStockpile ? "\u221E" : shootMan.BulletCount.ToString();
            bulletText.text = firstPart + "/" + secondPart;
        }
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(BFG_ShootingHUD)), InitializeOnLoadAttribute]
    public class BFG_ShootingHUDEditor : Editor
    {
        BFG_ShootingHUD shootMan;
        SerializedObject SerFPC;

        SerializedProperty ShootingManager;

        SerializedProperty bulletText;

        private void OnEnable()
        {
            shootMan = (BFG_ShootingHUD)target;
            SerFPC = new SerializedObject(target);

            // Get all serialized properties
            ShootingManager = SerFPC.FindProperty("shootMan");
            bulletText = SerFPC.FindProperty("bulletText");
        }

        public override void OnInspectorGUI()
        {
            // Update the serialized object
            SerFPC.Update();

            GUILayout.Label("Shooting HUD", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 16 }, GUILayout.ExpandWidth(true));
            GUILayout.Label("By Broken Flame Games", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = 11 }, GUILayout.ExpandWidth(true));
            GUILayout.Label("V1.0", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = 11 }, GUILayout.ExpandWidth(true));
            EditorGUILayout.HelpBox("Hover Over Anything to See What it Does!", MessageType.Warning);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            GUILayout.Label("Bullet Settings", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(ShootingManager, new GUIContent("Shooting Manager", "Drag and drop your shooting manager script here."));
            EditorGUILayout.PropertyField(bulletText, new GUIContent("Bullet's Text", "Drag and drop your Bullet Text here."));
            // Apply changes
            SerFPC.ApplyModifiedProperties();
        }
    }
#endif
}