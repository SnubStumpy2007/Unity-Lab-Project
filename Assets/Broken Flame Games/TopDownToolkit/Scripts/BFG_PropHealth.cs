// ================================================
// 
//            Script Name: BFG_PropHealth
//            Creator: Burning Flame Games
//            Description: Put script onto a Prop to allow it to be hit with the bullet projectiles.
// 
// ================================================
using UnityEngine;
using System.Collections;
using UnityEditor;
using BrokenFlameGames;
namespace BrokenFlameGames
{
    public class BFG_PropHealth : MonoBehaviour
    {
        [Header("Health Settings")]
        [SerializeField] private float health = 10.0f;

        [Header("Death Effects")]
        [SerializeField] private bool hasPickupParticle;
        [SerializeField] private Transform pickupParticle;

        [Header("Damage Flash")]
        [SerializeField] public bool enableDamageFlash = true;
        [SerializeField] public GameObject flashTarget; // Object that will flash (defaults to this object if null)
        [SerializeField] private Color flashColor = Color.red;
        [SerializeField] private float flashDuration = 0.1f;

        private Material originalMaterial;
        private Color originalColor;
        private Renderer targetRenderer;
        private bool isFlashing = false;

        private void Start()
        {
            // If no flash target specified, use this object
            if (flashTarget == null)
            {
                flashTarget = gameObject;
            }

            // Get the renderer of the target object
            targetRenderer = flashTarget.GetComponent<Renderer>();
            if (targetRenderer != null)
            {
                originalMaterial = targetRenderer.material;
                originalColor = targetRenderer.material.color;
            }
            else
            {
                Debug.LogWarning("No Renderer found on flash target: " + flashTarget.name);
            }
        }

        // Damages prop based on (float/damage).
        public void DamageProp(float damage)
        {
            health -= damage;
            if (enableDamageFlash && targetRenderer != null && !isFlashing)
            {
                StartCoroutine(FlashObject());
            }
            CheckHealth();
        }

        private IEnumerator FlashObject()
        {
            isFlashing = true;
            targetRenderer.material.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            targetRenderer.material.color = originalColor;
            isFlashing = false;
        }

        // Checks the health of the object once damaged
        public void CheckHealth()
        {
            if (health <= 0)
            {
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

                Destroy(gameObject);
            }
        }

        // Instantly Destroy the Game Object
        public void InstantKill()
        {
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
            Destroy(gameObject);
        }
    }



#if UNITY_EDITOR
    [CustomEditor(typeof(BFG_PropHealth)), InitializeOnLoadAttribute]
    public class BFG_PropHealthEditor : Editor
    {
        SerializedObject SerFPC;

        SerializedProperty health;
        SerializedProperty hasPickupParticle;
        SerializedProperty pickupParticle;
        SerializedProperty enableDamageFlash;
        SerializedProperty flashTarget;
        SerializedProperty flashColor;
        SerializedProperty flashDuration;

        private void OnEnable()
        {
            SerFPC = new SerializedObject(target);

            // Get all serialized properties
            health = SerFPC.FindProperty("health");
            hasPickupParticle = SerFPC.FindProperty("hasPickupParticle");
            pickupParticle = SerFPC.FindProperty("pickupParticle");
            enableDamageFlash = SerFPC.FindProperty("enableDamageFlash");
            flashTarget = SerFPC.FindProperty("flashTarget");
            flashColor = SerFPC.FindProperty("flashColor");
            flashDuration = SerFPC.FindProperty("flashDuration");
        }

        public override void OnInspectorGUI()
        {
            // Update the serialized object
            SerFPC.Update();

            GUILayout.Label("Prop Health", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 16 }, GUILayout.ExpandWidth(true));
            GUILayout.Label("By Broken Flame Games", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = 11 }, GUILayout.ExpandWidth(true));
            GUILayout.Label("V1.0", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = 11 }, GUILayout.ExpandWidth(true));
            EditorGUILayout.HelpBox("Hover Over Anything to See What it Does!", MessageType.Warning);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            GUILayout.Label("Prop Settings", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
            EditorGUILayout.Space();

            EditorGUILayout.Slider(health, 1.0f, 250.0f, new GUIContent("Prop Health", "This is the max Health the prop has."));
            EditorGUILayout.PropertyField(hasPickupParticle, new GUIContent("Pickup Particle?", "Do you want a particle to play when you pickup the ammo?"));
            if (hasPickupParticle.boolValue)
            {
                EditorGUILayout.PropertyField(pickupParticle, new GUIContent("Pickup Particle Prefab", "Drag and drop a prefab of the particle you want to spawn!"));
            }

            EditorGUILayout.Space();
            GUILayout.Label("Damage Effects", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
            EditorGUILayout.PropertyField(enableDamageFlash, new GUIContent("Enable Damage Flash", "Should the object flash when taking damage?"));
            if (enableDamageFlash.boolValue)
            {
                EditorGUILayout.PropertyField(flashTarget, new GUIContent("Flash Target", "Which object should flash when damaged (leave empty for this object)"));
                EditorGUILayout.PropertyField(flashColor, new GUIContent("Flash Color", "Color the object will flash when damaged"));
                EditorGUILayout.PropertyField(flashDuration, new GUIContent("Flash Duration", "How long the flash lasts in seconds"));

                // Show warning if flash target doesn't have a renderer
                var script = (BFG_PropHealth)target;
                if (script != null && script.enableDamageFlash)
                {
                    GameObject targetObj = script.flashTarget != null ? script.flashTarget : script.gameObject;
                    if (targetObj.GetComponent<Renderer>() == null)
                    {
                        EditorGUILayout.HelpBox("Warning: The flash target needs a Renderer component to flash!", MessageType.Warning);
                    }
                }
            }

            // Apply changes
            SerFPC.ApplyModifiedProperties();
        }
    }
#endif
}