using UnityEngine;
using FMODUnity;

/// <summary>
/// This class inherits from TargetObject and represents a PickupObject.
/// </summary>
public class PickupObject : TargetObject
{
    FMODUnity.StudioEventEmitter sound;
    [SerializeField]
    private GameObject refAud;
    [SerializeField]
    private int val;
    [SerializeField]
    private int tranTo;
    [SerializeField]
    private bool transformation=false;
    [SerializeField]
    private GameObject GamFlowManag; //esto es para acceder al timeManager y me modifique algunos parametros en el update dado que el objeto que lleva esto se destruye 
    private TimeManager tman;

    [Header("PickupObject")]

    [Tooltip("New Gameobject (a VFX for example) to spawn when you trigger this PickupObject")]
    public GameObject spawnPrefabOnPickup;

    [Tooltip("Destroy the spawned spawnPrefabOnPickup gameobject after this delay time. Time is in seconds.")]
    public float destroySpawnPrefabDelay = 10;
    
    [Tooltip("Destroy this gameobject after collectDuration seconds")]
    public float collectDuration = 0f;

    

    void Start() {
        Register();
        sound=refAud.GetComponent<FMODUnity.StudioEventEmitter>();

        if(GamFlowManag!=null)
        {
            tman=GamFlowManag.GetComponent<TimeManager>();
        }

    }

    void OnCollect()
    {
        if (CollectSound)
        {
            //AudioUtility.CreateSFX(CollectSound, transform.position, AudioUtility.AudioGroups.Pickup, 0f);
            sound.SetParameter("JumpToSong", val);
            sound.SetParameter("OnTrack", tranTo);
        }
        if (val>0&&tman!=null)
        {
            transformation = true;
            tman.SetTransformation(transformation);
        }

        if (spawnPrefabOnPickup)
        {
            var vfx = Instantiate(spawnPrefabOnPickup, CollectVFXSpawnPoint.position, Quaternion.identity);
            Destroy(vfx, destroySpawnPrefabDelay);
        }
               
        Objective.OnUnregisterPickup(this);

        TimeManager.OnAdjustTime(TimeGained);

        Destroy(gameObject, collectDuration);
    }
    
    void OnTriggerEnter(Collider other)
    {
        if ((layerMask.value & 1 << other.gameObject.layer) > 0 && other.gameObject.CompareTag("Player"))
        {
            OnCollect();
        }
    }
}
