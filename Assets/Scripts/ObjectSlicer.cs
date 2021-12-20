using UnityEngine;
using EzySlice;
using System.Collections.Generic;

public class ObjectSlicer : MonoBehaviour
{
    public GameObject objectToSlice; // non-null
    public GameObject cutPlane;
    public GameObject knife;
    public List<GameObject> allSlicedObjects;
    public float knifeDownSpeed;
    public float knifeUpSpeed;

    private InputManager inputManager;

    private void Awake()
    {
        // cache input manager
        inputManager = InputManager.Instance;
    }

    private void Update()
    {
       
    }
    public SlicedHull Slice(Vector3 planeWorldPosition, Vector3 planeWorldDirection)
    {
        return objectToSlice.Slice(planeWorldPosition, planeWorldDirection);
    }

    public GameObject[] SliceInstantiate(Vector3 planeWorldPosition, Vector3 planeWorldDirection)
    {
        return objectToSlice.SliceInstantiate(planeWorldPosition, planeWorldDirection);
    }

    public void MoveKnifeDown()
    {
        Vector3 knifeDownVector = Vector3.down * knifeDownSpeed * Time.deltaTime;
        knife.transform.position += knifeDownVector;
    }

    public void MoveKnifeUp()
    {
        Vector3 knifeUpVector = Vector3.up * knifeUpSpeed * Time.deltaTime;
        knife.transform.position += knifeUpVector;
    }
}
