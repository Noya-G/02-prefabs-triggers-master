using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/**
 * This component spawns the given object whenever the player clicks a given key.
 */
public class ClickSpawner: MonoBehaviour
{
    private DateTime last;
    private DateTime now;
    private bool first;
    private void Start()
    {
        // Initialize the start time when the component starts.
        last = DateTime.Now;
        first = true;
        Debug.Log("Start ClickSpawner");
    }
    [SerializeField] protected InputAction spawnAction = new InputAction(type: InputActionType.Button);
    [SerializeField] protected GameObject prefabToSpawn;
    [SerializeField] protected Vector3 velocityOfSpawnedObject;
    
    void OnEnable()  {
        spawnAction.Enable();
    }

    void OnDisable()  {
        spawnAction.Disable();
    }

    protected virtual GameObject spawnObject() {
        Debug.Log("Clicked last: "+last+", now:"+now);
        // Check if 3 seconds or more have passed since the start time.
        now = DateTime.Now;
        
        TimeSpan timePassed = now-last;
        Debug.Log(timePassed.TotalSeconds);
        
        if (timePassed.TotalSeconds >= 2 || first==true)
        {
            last = DateTime.Now;
            first = false;
            // Step 1: spawn the new object.
            Vector3 positionOfSpawnedObject = transform.position;  // span at the containing object position.
            Quaternion rotationOfSpawnedObject = Quaternion.identity;  // no rotation.
            GameObject newObject = Instantiate(prefabToSpawn, positionOfSpawnedObject, rotationOfSpawnedObject);

            // Step 2: modify the velocity of the new object.
            Mover newObjectMover = newObject.GetComponent<Mover>();
            if (newObjectMover)
            {
                newObjectMover.SetVelocity(velocityOfSpawnedObject);
            }

            return newObject;
        }
        else
        {
            // Do something else if less than 3 seconds have passed.
            return null;
        }
    }
    
    private void Update() {
        if (spawnAction.WasPressedThisFrame()) {
            spawnObject();
        }
    }
}
