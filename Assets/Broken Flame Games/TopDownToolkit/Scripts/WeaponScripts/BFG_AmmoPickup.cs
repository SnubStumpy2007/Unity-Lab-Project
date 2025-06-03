// ================================================
// 
//            Script Name: BFG_AmmoPickup
//            Creator: Burning Flame Games
//            Description: Put script onto a object to allow the player to get ammo from it
// 
// ================================================
using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using BrokenFlameGames;
namespace BrokenFlameGames
{
    public class BFG_AmmoPickup : MonoBehaviour
    {
        [SerializeField] private bool isClips;
        [SerializeField] private bool hasPickupParticle;
        [SerializeField] private Transform pickupParticle;
        [SerializeField] private int bulletAmount = 1;
        [SerializeField] private int clipAmount = 1;


        //Passes if the ammo picked up is a magazine or just bullets
        public Tuple<int, bool> pickupAmmo()
        {
            if (isClips)
            {
                Destroy(gameObject);
                if (hasPickupParticle)
                {
                    if (pickupParticle != null)
                    {
                        Transform myObj = Instantiate(pickupParticle, transform.position, Quaternion.identity);
                    }
                    else
                    {
                        Debug.LogWarning("MISSING PARTICLE");
                    }

                }
                return Tuple.Create(clipAmount, true);
            }
            else
            {
                Destroy(gameObject);
                if (pickupParticle != null)
                {
                    Transform myObj = Instantiate(pickupParticle, transform.position, Quaternion.identity);
                }
                else
                {
                    Debug.LogWarning("MISSING PARTICLE");
                }
                return Tuple.Create(bulletAmount, false);
            }
        }
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(BFG_AmmoPickup)), InitializeOnLoadAttribute]
    public class BFG_AmmoPickupEditor : Editor
    {
        BFG_AmmoPickup shootMan;
        SerializedObject SerFPC;

        SerializedProperty isClips;

        SerializedProperty bulletAmount;
        SerializedProperty clipAmount;
        SerializedProperty hasPickupParticle;
        SerializedProperty pickupParticle;
        private void OnEnable()
        {
            shootMan = (BFG_AmmoPickup)target;
            SerFPC = new SerializedObject(target);

            // Get all serialized properties
            isClips = SerFPC.FindProperty("isClips");
            bulletAmount = SerFPC.FindProperty("bulletAmount");
            clipAmount = SerFPC.FindProperty("clipAmount");
            hasPickupParticle = SerFPC.FindProperty("hasPickupParticle");
            pickupParticle = SerFPC.FindProperty("pickupParticle");
        }

        public override void OnInspectorGUI()
        {
            // Update the serialized object
            SerFPC.Update();

            GUILayout.Label("Ammo Pickup", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 16 }, GUILayout.ExpandWidth(true));
            GUILayout.Label("By Broken Flame Games", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = 11 }, GUILayout.ExpandWidth(true));
            GUILayout.Label("V1.0", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = 11 }, GUILayout.ExpandWidth(true));
            EditorGUILayout.HelpBox("Hover Over Anything to See What it Does!", MessageType.Warning);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            EditorGUILayout.Space();
            GUILayout.Label("Ammo Pickup Settings", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(isClips, new GUIContent("Magazines?", "true = gives full magazines\n false = gives a bullet count instead of full magazines"));
            if (isClips.boolValue)
            {
                EditorGUILayout.IntSlider(clipAmount, 1, 10, new GUIContent("Magazine Amount", "The amount of clips/magazines the player will get."));

            }
            else
            {
                EditorGUILayout.IntSlider(bulletAmount, 1, 240, new GUIContent("Bullet Amount", "The amount of bullets that the player will get."));
            }

            EditorGUILayout.PropertyField(hasPickupParticle, new GUIContent("Pickup Particle?", "Do you want a particle to play when you pickup the ammo?"));
            if (hasPickupParticle.boolValue)
            {
                EditorGUILayout.PropertyField(pickupParticle, new GUIContent("Pickup Particle Prefab", "Drag and drop a prefab of the particle you want to spawn!"));
            }

            // Apply changes
            SerFPC.ApplyModifiedProperties();
        }
    }
#endif
}