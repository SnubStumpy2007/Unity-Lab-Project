// ================================================
// 
//            Script Name: BFG_ShootingManager
//            Creator: Burning Flame Games
//            Description: Put script onto player to allow them to shoot
// 
// ================================================
using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using BrokenFlameGames;

namespace BrokenFlameGames
{
    public class BFG_ShootingManager : MonoBehaviour
    {

        [SerializeField] public Transform bulletPrefab;
        [SerializeField] public Transform muzzleFlash;
        [SerializeField] public GameObject muzzle;
        [SerializeField] public float damage = 25;
        [SerializeField] public float speed = 40;

        [SerializeField] public int BulletCount = 90;
        [SerializeField] public int MagSize = 30;
        [SerializeField] public int CurrentMagCapacity = 30;
        [SerializeField] public float reloadTime = 1.5f;
        [SerializeField] public float timeBetweenShots = 0.1f;
        public bool infiniteAmmo = false;
        public bool hasMuzzleFlash = false;
        public bool infiniteStockpile = false;

        private bool reloading = false;
        private bool currentlyShooting = false;
        private bool mouseDown = false;
        private float nextShotTime = 0f;
        [SerializeField] KeyCode keyCode = KeyCode.R;


        void Update()
        {
            GunManager();
            mousePressFunction();
            //If Reload key press, will start reload function.
            if (Input.GetKeyDown(keyCode) && !reloading)
            {
                Debug.Log("Reloading?");
                StartCoroutine(Reload());
            }
        }

        void GunManager()
        {

            if (infiniteAmmo)
            {
                Shoot();
            }
            else
            {
                //Checks if mag is greater than 0, if so then player will shoot, if not they are forced to reload.
                if (CurrentMagCapacity <= 0 && !reloading)
                {
                    StopCoroutine(Shooting());
                    if (BulletCount > 0)
                    {
                        StartCoroutine(Reload());
                    }
                }
                else if (!reloading)
                {
                    Shoot();
                }
            }

        }

        //Sees if mosue is being currently pressed or not
        void mousePressFunction()
        {
            if (Input.GetMouseButtonDown(0))
            {
                mouseDown = true;
            }
            if (Input.GetMouseButtonUp(0))
            {
                mouseDown = false;
            }
        }

        //If mouse is held down, then the player can shoot!
        void Shoot()
        {
            if (mouseDown && Time.time >= nextShotTime)
            {
                if (!currentlyShooting) { }
                StartCoroutine(Shooting());
            }
        }

        private IEnumerator Shooting()
        {
            currentlyShooting = true;

            //Spawns Bullet with proper settings
            Quaternion BulletRotation = Quaternion.Euler(0, muzzle.transform.eulerAngles.y, 0);
            Transform myObj = Instantiate(bulletPrefab, muzzle.transform.position, BulletRotation);
            BFG_BulletSettings bulletSettings = myObj.GetComponent<BFG_BulletSettings>();
            bulletSettings.damage = damage;
            bulletSettings.speed = speed;

            //If there is a muzzle flash then it will instantiate/clone one
            if (hasMuzzleFlash)
            {
                if (muzzleFlash != null)
                {
                    Transform muzzleGo = Instantiate(muzzleFlash, muzzle.transform.position, BulletRotation);
                }
                else
                {
                    Debug.LogWarning("Missing Particle for Muzzle Flash");
                }
            }

            if (!infiniteAmmo)
                CurrentMagCapacity--;
            nextShotTime = Time.time + timeBetweenShots;
            yield return new WaitForSeconds(timeBetweenShots);

            currentlyShooting = false;
        }

        private IEnumerator Reload()
        {
            reloading = true;
            yield return new WaitForSeconds(reloadTime);

            if (!infiniteStockpile)
            {

                if (BulletCount > 0)
                {
                    int tempCount = MagSize - CurrentMagCapacity;
                    CurrentMagCapacity = MagSize;
                    BulletCount -= tempCount;
                }
                else if (BulletCount < MagSize)
                {
                    CurrentMagCapacity = BulletCount;
                    BulletCount = 0;
                }
                else
                {
                    CurrentMagCapacity = MagSize;
                    BulletCount -= MagSize;
                }

            }
            else
            {
                CurrentMagCapacity = MagSize;
            }
            reloading = false;
        }


        void OnTriggerEnter(Collider other)
        {
            //If player is in a Trigger with the BFG_AmmoPickup, then the player will gain the ammo from it.
            if (other.GetComponent<BFG_AmmoPickup>())
            {
                Tuple<int, bool> AmmoCountPickUp = other.GetComponent<BFG_AmmoPickup>().pickupAmmo();
                if (AmmoCountPickUp.Item2)
                {
                    BulletCount += (MagSize * AmmoCountPickUp.Item1);
                }
                else
                {
                    BulletCount += AmmoCountPickUp.Item1;
                }
            }
        }
    }



#if UNITY_EDITOR
    [CustomEditor(typeof(BFG_ShootingManager)), InitializeOnLoadAttribute]
    public class ShootingManagerEditor : Editor
    {
        BFG_ShootingManager shootMan;
        SerializedObject SerFPC;

        SerializedProperty bulletPrefab;
        SerializedProperty damage;
        SerializedProperty speed;
        SerializedProperty bulletCount;
        SerializedProperty magSize;
        SerializedProperty currentMagCapacity;
        SerializedProperty reloadTime;
        SerializedProperty timeBetweenShots;
        SerializedProperty infiniteAmmo;
        SerializedProperty infiniteStockpile;
        SerializedProperty muzzle;
        SerializedProperty keyCode;

        SerializedProperty muzzleFlash;
        SerializedProperty hasMuzzleFlash;
        private void OnEnable()
        {
            shootMan = (BFG_ShootingManager)target;
            SerFPC = new SerializedObject(target);

            // Get all serialized properties
            bulletPrefab = SerFPC.FindProperty("bulletPrefab");
            damage = SerFPC.FindProperty("damage");
            speed = SerFPC.FindProperty("speed");
            bulletCount = SerFPC.FindProperty("BulletCount");
            magSize = SerFPC.FindProperty("MagSize");
            currentMagCapacity = SerFPC.FindProperty("CurrentMagCapacity");
            reloadTime = SerFPC.FindProperty("reloadTime");
            timeBetweenShots = SerFPC.FindProperty("timeBetweenShots");
            infiniteAmmo = SerFPC.FindProperty("infiniteAmmo");
            infiniteStockpile = SerFPC.FindProperty("infiniteStockpile");
            muzzle = SerFPC.FindProperty("muzzle");
            keyCode = SerFPC.FindProperty("keyCode");
            muzzleFlash = SerFPC.FindProperty("muzzleFlash");
            hasMuzzleFlash = SerFPC.FindProperty("hasMuzzleFlash");
        }

        public override void OnInspectorGUI()
        {
            // Update the serialized object
            SerFPC.Update();

            GUILayout.Label("Weapon Manager", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 16 }, GUILayout.ExpandWidth(true));
            GUILayout.Label("By Broken Flame Games", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = 11 }, GUILayout.ExpandWidth(true));
            GUILayout.Label("V1.0", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = 11 }, GUILayout.ExpandWidth(true));
            EditorGUILayout.HelpBox("Hover Over Anything to See What it Does!", MessageType.Warning);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            GUILayout.Label("Bullet Settings", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(bulletPrefab, new GUIContent("Bullet Prefab", "The Bullet the player will shoot out."));
            EditorGUILayout.PropertyField(muzzle, new GUIContent("Muzzle Position", "This is the position the bullet prefab comes out of."));
            EditorGUILayout.PropertyField(hasMuzzleFlash, new GUIContent("Muzzle Flash?", "Enables if there is a weapon flash."));
            if (hasMuzzleFlash.boolValue)
            {
                EditorGUILayout.PropertyField(muzzleFlash, new GUIContent("Muzzle Flash", "The Muzzle Flash Prefab That Comes Out of The Muzzle."));
            }
            EditorGUILayout.Slider(damage, 0.0f, 100.0f, new GUIContent("Bullet Damage", "The damage that the bullet has when spawned in."));
            EditorGUILayout.Slider(speed, 10.0f, 100.0f, new GUIContent("Bullet Speed", "The speed at which the bullet travels."));

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            GUILayout.Label("Gun Settings", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(keyCode, new GUIContent("Reload Key", "The Button used to reload."));
            EditorGUILayout.IntSlider(bulletCount, 0, 1000, new GUIContent("Total Bullets", "The amount of bullets the player has"));
            EditorGUILayout.IntSlider(magSize, 0, 1000, new GUIContent("Max Magazine Capacity", "The max amount of bullets that can be in the magazine"));
            EditorGUILayout.IntSlider(currentMagCapacity, 0, 1000, new GUIContent("Current Magazine Capacity", "The current amount of bullets in your magazine"));
            EditorGUILayout.Slider(reloadTime, 0f, 10f, new GUIContent("Reload Time", "Time needed to reload."));
            EditorGUILayout.Slider(timeBetweenShots, 0f, 3f, new GUIContent("Time Between Shots", "Time taken between shots."));

            if (timeBetweenShots.floatValue == 0f)
            {
                EditorGUILayout.HelpBox("Warning: A value of 0 may cause issues (e.g., instant continuous firing).", MessageType.Warning);
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            GUILayout.Label("Debug Gun Settings", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 12 }, GUILayout.ExpandWidth(true));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(infiniteAmmo, new GUIContent("Infinite Ammo", "Players never have to reload their weapon."));
            EditorGUILayout.PropertyField(infiniteStockpile, new GUIContent("Infinite Stockpile", "Players have an unlimited amount of ammo in their stockpile"));

            // Apply changes
            SerFPC.ApplyModifiedProperties();
        }
    }
#endif
}