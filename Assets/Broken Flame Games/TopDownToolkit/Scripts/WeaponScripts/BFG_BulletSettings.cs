// ================================================
// 
//            Script Name: BFG_BulletSettings
//            Creator: Burning Flame Games
//            Description: Put script onto a bullet prefab.
// 
// ================================================

using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using BrokenFlameGames;
namespace BrokenFlameGames
{
    public class BFG_BulletSettings : MonoBehaviour
    {
        //Speed At Which Bullet Travels
        public float speed = 25;

        // Damage The Projectile Does
        public float damage = 40;
        private Rigidbody rb;
        public LayerMask HitLayers = 1;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.linearVelocity = transform.forward * speed;
        }

        //Damages an object if it has the component Prop Health, and is on the correct layer
        void OnCollisionEnter(Collision other)
        {
            if ((HitLayers.value & (1 << other.gameObject.layer)) != 0)
            {
                //Value Hit Correct Target
                if (other.gameObject.GetComponent<BFG_PropHealth>())
                {
                    //If object contains prop health component, then it damages it.
                    other.gameObject.GetComponent<BFG_PropHealth>().DamageProp(damage);
                    Destroy(gameObject);
                }
            }

            Destroy(gameObject);
        }

    }


#if UNITY_EDITOR
    [CustomEditor(typeof(BFG_BulletSettings)), InitializeOnLoadAttribute]
    public class BFG_BulletSettingsEditor : Editor
    {
        BFG_BulletSettings shootMan;
        SerializedObject SerFPC;

        SerializedProperty speed;

        SerializedProperty damage;
        SerializedProperty HitLayers;
        private void OnEnable()
        {
            shootMan = (BFG_BulletSettings)target;
            SerFPC = new SerializedObject(target);

            // Get all serialized properties
            speed = SerFPC.FindProperty("speed");
            damage = SerFPC.FindProperty("damage");
            HitLayers = SerFPC.FindProperty("HitLayers");
        }

        public override void OnInspectorGUI()
        {
            // Update the serialized object
            SerFPC.Update();

            GUILayout.Label("Bullet Settings", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 16 }, GUILayout.ExpandWidth(true));
            GUILayout.Label("By Broken Flame Games", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = 11 }, GUILayout.ExpandWidth(true));
            GUILayout.Label("V1.0", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = 11 }, GUILayout.ExpandWidth(true));
            EditorGUILayout.HelpBox("Hover Over Anything to See What it Does!", MessageType.Warning);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            EditorGUILayout.Space();
            GUILayout.Label("Bullet Settings", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
            EditorGUILayout.Space();
            EditorGUILayout.Slider(speed, 1.0f, 85.0f, new GUIContent("Bullet Speed", "Speed at which the bullet Travels"));
            EditorGUILayout.Slider(damage, 1.0f, 100.0f, new GUIContent("Bullet Damage", "The Damage the bullet deals."));
            EditorGUILayout.PropertyField(HitLayers, new GUIContent("Hit Layers", "The Layer the bullet will hit."));


            // Apply changes
            SerFPC.ApplyModifiedProperties();
        }
    }
#endif
}