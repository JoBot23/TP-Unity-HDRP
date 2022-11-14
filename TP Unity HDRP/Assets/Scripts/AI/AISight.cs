using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AISight : MonoBehaviour
{

    [SerializeField] float sightRange;
    [SerializeField] float sightAngle;
    [SerializeField] float sightHeight;
    [SerializeField] Color sightGizmoColor = Color.red;

    [SerializeField] int scanFrequency = 30;
    [SerializeField] LayerMask layers;
    [SerializeField] LayerMask blockingLayers;
    [HideInInspector] public List<GameObject> objects  = new List<GameObject>();

    Mesh mesh;
    Collider[] colliders = new Collider[50];
    int count;
    float scanInterval;
    float scanTimer;

    void Start()
    {
        scanInterval = 1f / scanFrequency;
    }

    void Update()
    {
        scanTimer -= Time.deltaTime;
        if(scanTimer < 0)
        {
            scanTimer += scanInterval;
            Scan();
        }
    }

    private void Scan()
    {
        count = Physics.OverlapSphereNonAlloc(transform.position, sightRange, colliders, layers, QueryTriggerInteraction.Collide);
    
        objects.Clear();
        for(int i = 0; i < count; i++)
        {
            GameObject obj = colliders[i].gameObject;
            if(IsInSight(obj))
            {
                objects.Add(obj);
            }
        }
    }

    public bool IsInSight(GameObject obj)
    {
        Vector3 origin = transform.position;
        Vector3 dest = obj.transform.position;
        Vector3 direction = dest - origin;

        if(direction.y < 0 || direction.y > sightHeight)
        {
            return false;
        }

        direction.y = 0;
        float deltaAngle = Vector3.Angle(direction, transform.forward);
        if(deltaAngle > sightAngle)
        {
            return false;
        }

        origin.y += sightHeight / 2;
        dest.y = origin.y;
        if(Physics.Linecast(origin, dest, blockingLayers))
        {
            return false;
        }

        return true;
    }

    private Mesh CreateSightGizmo()
    {
        Mesh mesh = new Mesh();

        int nbSegments = 10;
        int nbTriangles = (nbSegments * 4) + 2 + 2;
        int nbVertices = nbTriangles * 3;

        Vector3[] vertices = new Vector3[nbVertices];
        int[] triangles = new int[nbVertices];

        Vector3 bottomCenter = Vector3.zero;
        Vector3 bottomLeft = Quaternion.Euler(0, -sightAngle, 0) * Vector3.forward * sightRange;
        Vector3 bottomRight = Quaternion.Euler(0, sightAngle, 0) * Vector3.forward * sightRange;

        Vector3 topCenter = bottomCenter + Vector3.up * sightHeight;
        Vector3 topLeft = bottomLeft + Vector3.up * sightHeight;
        Vector3 topRight = bottomRight + Vector3.up * sightHeight;

        int vert = 0;

        //Left
        vertices[vert++] = bottomCenter;
        vertices[vert++] = bottomLeft;
        vertices[vert++] = topLeft;

        vertices[vert++] = topLeft;
        vertices[vert++] = topCenter;
        vertices[vert++] = bottomCenter;

        //Right
        vertices[vert++] = bottomCenter;
        vertices[vert++] = topCenter;
        vertices[vert++] = topRight;

        vertices[vert++] = topRight;
        vertices[vert++] = bottomRight;
        vertices[vert++] = bottomCenter;

        float currentAngle= -sightAngle;
        float deltaAngle = (sightAngle * 2) / nbSegments;
        for(int i = 0; i < nbSegments; i++)
        {
            bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * sightRange;
            bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * sightRange;

            topLeft = bottomLeft + Vector3.up * sightHeight;
            topRight = bottomRight + Vector3.up * sightHeight;

            //Front
            vertices[vert++] = bottomLeft;
            vertices[vert++] = bottomRight;
            vertices[vert++] = topRight;

            vertices[vert++] = topRight;
            vertices[vert++] = topLeft;
            vertices[vert++] = bottomLeft;

            //Top

            vertices[vert++] = topCenter;
            vertices[vert++] = topLeft;
            vertices[vert++] = topRight;

            //Bottom
            vertices[vert++] = bottomCenter;
            vertices[vert++] = bottomRight;
            vertices[vert++] = bottomLeft;

            currentAngle += deltaAngle;
        }

        for(int i = 0; i < nbVertices; i++)
        {
            triangles[i] = i;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }

    private void OnValidate() 
    {
        mesh = CreateSightGizmo();
        scanInterval = 1f / scanFrequency;
    }

    private void OnDrawGizmos()
    {
        if(mesh)
        {
            Gizmos.color = sightGizmoColor;
            Gizmos.DrawMesh(mesh, transform.position, transform.rotation);
        }

        // Gizmos.DrawWireSphere(transform.position, sightRange);
        // for(int i = 0; i < count; i++)
        // {
        //     Gizmos.DrawSphere(colliders[i].transform.position, 0.2f);
        // }

        Gizmos.color = Color.green;
        foreach(var obj in objects)
        {
            Gizmos.DrawSphere(obj.transform.position, 0.2f);
        }
    }
}
