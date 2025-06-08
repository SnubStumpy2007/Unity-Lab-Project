// ================================================
// 
//            Script Name: BFG_PlayerController
//            Creator: Burning Flame Games
//            Description: Put script onto player to allow them to walk around
// 
// ================================================
using UnityEngine;
using System.Collections;
using UnityEditor;
using BrokenFlameGames;
namespace BrokenFlameGames
{
    public class BFG_PlayerController : MonoBehaviour
    {
        [SerializeField] private float Speed = 5;
        [SerializeField] private float DistanceStop = 0.5f;
        [SerializeField] private bool FixedMovement = true;

        [SerializeField] private Transform body;
        [SerializeField] private bool smoothRotate = true;
        [SerializeField] private float smoothRotateSpeed = 7.5f;

        [SerializeField] private Camera MainCamera;

        [SerializeField] private Vector3 cameraOffset = new Vector3(0, 5.8f, -10.27f);

        [SerializeField] private Vector3 cameraRotation = new Vector3(31.24f, 0, 0);

        [SerializeField] private bool followPlayer = true;

        [SerializeField] private float cameraFollowSpeed = 5f;

        [SerializeField] private LayerMask IgnoreMask = 1;

        private Rigidbody PlayerRB;

        void Start()
        {
            PlayerRB = GetComponent<Rigidbody>();
            MainCamera = Camera.main;
            if (MainCamera != null)
            {
                // Apply initial camera position/rotation
                UpdateCameraPosition(true);
            }
        }

        void Update()
        {
            PlayerMovement();
            if (MainCamera != null)
            {
                RotationMan();
            }
            else
            {
                MainCamera = Camera.main;
                Debug.Log("Finding Camera");
            }

            if (followPlayer && MainCamera != null)
            {
                UpdateCameraPosition();
            }
        }

        void UpdateCameraPosition(bool instant = false)
        {
            Vector3 desiredPosition = transform.position +
                             (Vector3.forward * cameraOffset.z) +
                             (Vector3.right * cameraOffset.x) +
                             (Vector3.up * cameraOffset.y);

            // Apply position
            if (instant)
            {
                MainCamera.transform.position = desiredPosition;
            }
            else
            {
                MainCamera.transform.position = Vector3.Lerp(
                    MainCamera.transform.position,
                    desiredPosition,
                    Time.deltaTime * cameraFollowSpeed
                );
            }

            // Apply rotation
            if (instant)
            {
                MainCamera.transform.rotation = Quaternion.Euler(cameraRotation);
            }
            else
            {
                MainCamera.transform.rotation = Quaternion.Lerp(
                    MainCamera.transform.rotation,
                    Quaternion.Euler(cameraRotation),
                    Time.deltaTime * cameraFollowSpeed
                );
            }
        }

        void RotationMan()
        {
            //Gets point of where mouse is on screen
            Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, IgnoreMask))
            {
                //Grabs Player position
                Vector2 playerPos = new Vector2(body.position.x, body.position.z);
                //Grabs mouse position
                Vector2 hitPos = new Vector2(raycastHit.point.x, raycastHit.point.z);
                float distance = Vector2.Distance(playerPos, hitPos);
                //If not in stopping distance, then player will look on Y axis at mouse pointer
                if (distance > DistanceStop)
                {
                    Vector3 direction = raycastHit.point - body.position;
                    direction.y = 0;

                    if (direction != Vector3.zero)
                    {
                        Quaternion targetRotation = Quaternion.LookRotation(direction);
                        float targetYRotation = targetRotation.eulerAngles.y;
                        Quaternion yAxisOnlyRotation = Quaternion.Euler(0, targetYRotation, 0);

                        if (smoothRotate)
                        {
                            body.rotation = Quaternion.Lerp(
                                body.rotation,
                                yAxisOnlyRotation,
                                Time.deltaTime * smoothRotateSpeed
                            );
                        }
                        else
                        {
                            body.rotation = yAxisOnlyRotation;
                        }
                    }
                }
            }
        }

        void PlayerMovement()
        {
            //Player Moves on X & Y Axis
            if (FixedMovement)
            {
                Vector3 playerInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
                transform.position += playerInput.normalized * Time.deltaTime * Speed;
            }
            //Player Moves based on direction faced
            else
            {
                //Gets mouse position
                Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, IgnoreMask))
                {
                    //Gets player position
                    Vector2 playerPos = new Vector2(transform.position.x, transform.position.z);

                    //Gets mouse x and z
                    Vector2 hitPos = new Vector2(raycastHit.point.x, raycastHit.point.z);
                    float distance = Vector2.Distance(playerPos, hitPos);

                    //If player is in stoping distance, they cant move
                    if (distance > DistanceStop)
                    {
                        Vector3 playerInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
                        Vector3 moveDirection = (transform.forward * playerInput.z + transform.right * playerInput.x).normalized;
                        transform.position += moveDirection * Time.deltaTime * Speed;
                    }
                }
            }
        }
    }



#if UNITY_EDITOR
    [CustomEditor(typeof(BFG_PlayerController)), InitializeOnLoadAttribute]
    public class playerControllerEditor : Editor
    {
        SerializedObject SerFPC;
        SerializedProperty speed;
        SerializedProperty MainCamera;
        SerializedProperty FixedMovement;
        SerializedProperty IgnoreMask;
        SerializedProperty DistanceStop;
        SerializedProperty smoothRotate;
        SerializedProperty smoothRotateSpeed;
        SerializedProperty body;
        SerializedProperty cameraOffset;
        SerializedProperty cameraRotation;
        SerializedProperty followPlayer;
        SerializedProperty cameraFollowSpeed;

        private void OnEnable()
        {
            SerFPC = new SerializedObject(target);

            speed = SerFPC.FindProperty("Speed");
            DistanceStop = SerFPC.FindProperty("DistanceStop");
            MainCamera = SerFPC.FindProperty("MainCamera");
            FixedMovement = SerFPC.FindProperty("FixedMovement");
            IgnoreMask = SerFPC.FindProperty("IgnoreMask");
            smoothRotate = SerFPC.FindProperty("smoothRotate");
            smoothRotateSpeed = SerFPC.FindProperty("smoothRotateSpeed");
            body = SerFPC.FindProperty("body");
            cameraOffset = SerFPC.FindProperty("cameraOffset");
            cameraRotation = SerFPC.FindProperty("cameraRotation");
            followPlayer = SerFPC.FindProperty("followPlayer");
            cameraFollowSpeed = SerFPC.FindProperty("cameraFollowSpeed");
        }

        public override void OnInspectorGUI()
        {
            SerFPC.Update();

            GUILayout.Label("Top Down Player Controller", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 16 }, GUILayout.ExpandWidth(true));
            GUILayout.Label("By Broken Flame Games", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = 11 }, GUILayout.ExpandWidth(true));
            GUILayout.Label("V1.0", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = 11 }, GUILayout.ExpandWidth(true));
            EditorGUILayout.HelpBox("Hover Over Anything to See What it Does!", MessageType.Warning);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            GUILayout.Label("Player Settings", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
            EditorGUILayout.Space();

            EditorGUILayout.Slider(speed, 0.0f, 25.0f, new GUIContent("Player Speed", "The speed at which the player moves."));
            EditorGUILayout.PropertyField(body, new GUIContent("Player Body", "This is the object that rotates towards the mouse pointer"));

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Mouse Settings", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(MainCamera, new GUIContent("Main Camera", "This is the main camera you are using in your scene."));
            EditorGUILayout.Slider(DistanceStop, 0.0f, 5.0f, new GUIContent("Stopping Distance", "A offset to prevent player from spinning infinitly"));
            if (DistanceStop.floatValue == 0f)
            {
                EditorGUILayout.HelpBox("Warning: A value of 0 may cause issues (e.g., Constant Spinning and Flip Flopping).", MessageType.Warning);
            }
            IgnoreMask.intValue = EditorGUILayout.MaskField(
                new GUIContent("Mouse Pointer Layers", "Layers that pick up mouse pointer position."),
                IgnoreMask.intValue,
                UnityEditorInternal.InternalEditorUtility.layers
            );

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Camera Settings", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(followPlayer, new GUIContent("Follow Player", "Should the camera track the player?"));
            if (followPlayer.boolValue)
            {
                EditorGUILayout.PropertyField(cameraOffset, new GUIContent("Position Offset", "X/Y/Z offset from player"));
                EditorGUILayout.PropertyField(cameraRotation, new GUIContent("Rotation", "Camera angle (Euler)"));
                EditorGUILayout.Slider(cameraFollowSpeed, 0.1f, 15f, new GUIContent("Follow Speed", "How quickly camera adjusts"));
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Movement Settings", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(FixedMovement, new GUIContent("Fixed Movement", "true = Player moves on x & z axis (Not based on players facing direction.)"));
            EditorGUILayout.PropertyField(smoothRotate, new GUIContent("Smooth Rotate", "true = Player has a slight delay when rotating."));
            if (smoothRotate.boolValue)
            {
                EditorGUILayout.Slider(smoothRotateSpeed, 0.0f, 15.0f, new GUIContent("Smooth Rotation Speed", "The speed at which player rotates.\n(Lower Value = Slower)\n(Higher Value = Faster)"));
                if (smoothRotateSpeed.floatValue == 0f)
                {
                    EditorGUILayout.HelpBox("Warning: The Player Will NOT be Able to Rotate", MessageType.Warning);
                }
            }

            SerFPC.ApplyModifiedProperties();
        }
    }
#endif
}