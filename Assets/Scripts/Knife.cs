using UnityEngine;
using EzySlice;

/// <summary>
/// To keep track of what the knife is doing
/// UP - knife is all the way up
/// CUTTING - Knife is going down
/// </summary>
public enum KnifeState
{
    UP = 0,
    MOVING_UP = 1,
    MOVING_DOWN = 2,
    DOWN = 3
}
public class Knife : MonoBehaviour
{
    public KnifeState state = KnifeState.UP;
    public float knifeDownSpeed;
    public float knifeUpSpeed;
    public float cutForce;
    public Transform cutObjectHullParent;

    [SerializeField] private GameObject cutPlaneObject;
    [SerializeField] private Material crossSectionMaterial;
    private InputManager inputManager;
    private GameObject objectToCut;

    private void Awake()
    {
        inputManager = InputManager.Instance;
    }
    private void Update()
    {
        if (inputManager.IsMousePressed || inputManager.IsTouchPressed)
            MoveKnifeDown();
        
        else if(state != KnifeState.UP)
            MoveKnifeUp();
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision Happened");
        if (other.gameObject.CompareTag("CuttableObject"))
        {
            objectToCut = other.gameObject;
            Vibrate();
            Debug.Log("Hit Cuttable Object: " + objectToCut.name);
        }
        else if (other.gameObject.CompareTag("Table"))
        {
            Debug.Log("Hit TABLE");
            state = KnifeState.DOWN;
            // fully cut the object when hit the table
            if (objectToCut != null)
            {
                Debug.Log("Slicing Object: " + objectToCut.name);
                SlicedObject slicedObject = SliceInstantiate(objectToCut);
                objectToCut = null;
            }

            EventManager.KnifeAllTheWayDown();
        }
        else if (other.gameObject.CompareTag("Ceiling"))
        {
            Debug.Log("Hit CEILING");
            state = KnifeState.UP;
            EventManager.KnifeAllTheWayUp();
        }
    }

    private SlicedObject SliceInstantiate(GameObject objectToSlice)
    {
        SlicedObject slicedObject = new SlicedObject();
        TextureRegion cutRegion = new TextureRegion(0.0f, 0.0f, 1.0f, 1.0f);
        EzySlice.Plane cutPlane = ComputeCutPlane(objectToSlice, cutPlaneObject.transform.position, Vector3.left);
        SlicedHull slice = Slicer.Slice(objectToSlice, cutPlane, cutRegion, crossSectionMaterial);
        slicedObject.slicedHull = slice;

        if (slice == null)
        {
            Debug.LogError("Couldn't slice object");
            return slicedObject;
        }

        // Instantiate Slices
        GameObject upperHull = slice.CreateUpperHull(objectToSlice, crossSectionMaterial);
        GameObject lowerHull = slice.CreateLowerHull(objectToSlice, crossSectionMaterial);

        if (upperHull != null)
        {
            upperHull.transform.SetParent(cutObjectHullParent);
            AddComponentsForUpperHull(upperHull);
            slicedObject.upperHullObject = upperHull;
        }

        if (lowerHull != null)
        {
            lowerHull.transform.SetParent(cutObjectHullParent);
            AddComponentsForLowerHull(lowerHull);
            slicedObject.lowerHullObject = lowerHull;
            Destroy(objectToSlice);
        }

        // nothing to return, so return nothing!
        return slicedObject;
    }

    public void MoveKnifeDown()
    {
        if(state != KnifeState.DOWN)
        {
            state = KnifeState.MOVING_DOWN;
            Vector3 knifeDownVector = Vector3.down * knifeDownSpeed * Time.deltaTime;
            transform.position += knifeDownVector;
        }
    }

    public void MoveKnifeUp()
    {
        if(state != KnifeState.UP)
        {
            state = KnifeState.MOVING_UP;
            Vector3 knifeUpVector = Vector3.up * knifeUpSpeed * Time.deltaTime;
            transform.position += knifeUpVector;
        }
    }

    private GameObject AddComponentsForUpperHull(GameObject slicedObject)
    {
        slicedObject.tag = "CuttableObject";
        slicedObject.AddComponent<CuttableObject>();
        slicedObject.AddComponent<BoxCollider>();
        Rigidbody rigidbody = slicedObject.AddComponent<Rigidbody>();
        rigidbody.isKinematic = false;
        rigidbody.useGravity = true;
        rigidbody.AddForceAtPosition(Vector3.left * cutForce, slicedObject.transform.position, ForceMode.Impulse);
        return slicedObject;
    }

    private GameObject AddComponentsForLowerHull(GameObject slicedObject)
    {
        slicedObject.tag = "CuttableObject";
        slicedObject.AddComponent<CuttableObject>();
        slicedObject.AddComponent<SphereCollider>();
        Rigidbody rigidbody = slicedObject.AddComponent<Rigidbody>();
        rigidbody.isKinematic = true;
        rigidbody.useGravity = false;
        
        return slicedObject;
    }

    private EzySlice.Plane ComputeCutPlane(GameObject objectToCut, Vector3 planePosition, Vector3 planeDirection)
    {
        EzySlice.Plane cuttingPlane = new EzySlice.Plane();

        Matrix4x4 mat = objectToCut.transform.worldToLocalMatrix;
        Matrix4x4 transpose = mat.transpose;
        Matrix4x4 inv = transpose.inverse;

        Vector3 refUp = inv.MultiplyVector(planeDirection).normalized;
        Vector3 refPt = objectToCut.transform.InverseTransformPoint(planePosition);

        cuttingPlane.Compute(refPt, refUp);
        return cuttingPlane;
    }

    private void Vibrate()
    {
        Debug.Log("Vibrating!");
        Handheld.Vibrate();
    } 
}
