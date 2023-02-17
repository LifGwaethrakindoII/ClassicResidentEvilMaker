using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Voidless
{
public class CharacterAIController<T> : MonoBehaviour where T : Character
{
    [SerializeField] private T _character;                      /// <summary>Character's reference.</summary>
    [SerializeField] private float _updateRate;                 /// <summary>AI's Main Update Rate.</summary>
    [Space(5f)]
    [Header("Pathfinding's Attributes:")]
    [SerializeField] private float _minWaypointDistance;        /// <summary>Minimum Waypoint's Distance.</summary>
    [SerializeField] private float _pathfindingSeed;            /// <summary>Pathfinding's Seed.</summary>
    [SerializeField] private float _maxPathWidth;               /// <summary>Path's Maximum Width.</summary>
    [SerializeField] private int _minLateralSegments;         /// <summary>Minimum lateral segments per Pathfinding line segment.</summary>
    private NavMeshPath _currentPath;                           /// <summary>Current Pathfinding path.</summary>
    private float _time;                                        /// <summary>AI-Loop's time.</summary>
    protected Coroutine pathTraversing;                         /// <summary>Path Traversing's coroutine.</summary>
    protected IEnumerator<Vector3> pathIteration;               /// <summary>Path's Iteration.</summary>
    protected Vector3 _previousTarget;                          /// <summary>Previous Target's Position.</summary>

    /// <summary>Gets and Sets character property.</summary>
    public T character
    {
        get { return _character; }
        set { _character = value; }
    }

    /// <summary>Gets and Sets updateRate property.</summary>
    public float updateRate
    {
        get { return _updateRate; }
        set { _updateRate = value; }
    }

    /// <summary>Gets and Sets minWaypointDistance property.</summary>
    public float minWaypointDistance
    {
        get { return _minWaypointDistance; }
        set { _minWaypointDistance = value; }
    }

    /// <summary>Gets and Sets pathfindingSeed property.</summary>
    public float pathfindingSeed
    {
        get { return _pathfindingSeed; }
        set { _pathfindingSeed = value; }
    }

    /// <summary>Gets and Sets maxPathWidth property.</summary>
    public float maxPathWidth
    {
        get { return _maxPathWidth; }
        set { _maxPathWidth = value; }
    }

    /// <summary>Gets and Sets minLateralSegments property.</summary>
    public int minLateralSegments
    {
        get { return _minLateralSegments; }
        set { _minLateralSegments = value; }
    }

    /// <summary>Gets and Sets previousTarget property.</summary>
    public Vector3 previousTarget
    {
        get { return _previousTarget; }
        protected set { _previousTarget = value; }
    }

    /// <summary>Gets and Sets time property.</summary>
    public float time
    {
        get { return _time; }
        protected set { _time = value; }
    }

    /// <summary>Gets and Sets currentPath property.</summary>
    public NavMeshPath currentPath
    {
        get { return _currentPath; }
        set { _currentPath = value; }
    }

    /// <summary>Draws Gizmos on Editor mode.</summary>
    protected virtual void OnDrawGizmos()
    {
        if(currentPath != null)
        {
            Gizmos.color = Color.cyan;
            Vector3 corner = Vector3.zero;
            Vector3? previousCorner = null;
            IEnumerator<Vector3> pathCornersIterator = GetPathCorners();

            while(pathCornersIterator.MoveNext())
            {
                corner = pathCornersIterator.Current;

                if(previousCorner.HasValue)
                {
                    Gizmos.DrawSphere(previousCorner.Value, 0.2f);
                    Gizmos.DrawSphere(corner, 0.2f);
                    Gizmos.DrawLine(previousCorner.Value, corner);
                }
                previousCorner = corner;
            }
        }
    }

    /// <summary>Updates CharacterAIController at each time step.</summary>
    protected virtual void Update()
    {
        if(time >= updateRate)
        {
            time = 0.0f;
            OnAIUpdate();
        }
        else time += Time.deltaTime;
    }

    /// <summary>Callback internally invoked when the AI ought to be updated.</summary>
    protected virtual void OnAIUpdate() { /*...*/ }

    /// <summary>Calculates NavMesh path towards target.</summary>
    /// <param name="target">Target.</param>
    /// <param name="areaMask">A bitfield mask specifying which NavMesh areas can be passed when calculating a path.</param>
    /// <returns>True if path could be calculated, false otherwise.</returns>
    protected virtual bool GetPath(Vector3 target, int areaMask = NavMesh.AllAreas)
    {
        if(currentPath == null) currentPath = new NavMeshPath();

        bool sameTarget = previousTarget == target;
        bool result = sameTarget ? true : NavMesh.CalculatePath(character.transform.position, target, areaMask, currentPath);
        
        if(!sameTarget)
        {
            pathIteration = PathTraversingRoutine();
        }

        previousTarget = target;
        return result;
    }

    /// <summary>Calculates lateral segments given a segment of 2 points A and B.</summary>
    /// <param name="a">Point A.</param>
    /// <param name="b">Point B.</param>
    /// <returns>Lateral segments from Line-segment AB.</returns>
    protected virtual ValueVTuple<Vector3, Vector3>[] GetLateralSegments(Vector3 a, Vector3 b)
    {
        Vector3 u = Vector3.up;
        Vector3 d = b - a;
        Vector3 p = Vector3.ProjectOnPlane(d, u);
        Quaternion r = Quaternion.LookRotation(p, u);
        Vector3 l = Quaternion.Inverse(r) * p;
        Vector3 c = Vector3.Cross(u, d).normalized;
        float lateralSegments = (float)minLateralSegments;
        float seed = Mathf.Abs(pathfindingSeed);
        float m = d.magnitude;
        float min = Mathf.Min(m, lateralSegments);
        float max = Mathf.Max(m * m, lateralSegments);
        float s = Mathf.Ceil(VMath.Rand(min, max, seed));
        ValueVTuple<Vector3, Vector3>[] tuples = new ValueVTuple<Vector3, Vector3>[(int)s];

        for(float i = 0.0f; i < s; i++)
        {
            float t = i / s;
            float x = ((t * l.x * maxPathWidth) / m) * maxPathWidth;
            float y = ((t * l.z * maxPathWidth) / m) * maxPathWidth;
            float n = Mathf.PerlinNoise(x, y) * maxPathWidth;
            float o = VMath.Rand(-1.0f, 1.0f, t * seed);

            ValueVTuple<Vector3, Vector3> tuple = default(ValueVTuple<Vector3, Vector3>);
            tuple.Item1 = Vector3.Lerp(a, b, t);
            tuple.Item2 = c * n * Mathf.Sign(o);
            tuples[(int)i] = tuple;
        }

        return tuples;
    }

    /// <summary>Iterates through Path's corners and lateral segments.</summary>
    protected virtual IEnumerator<Vector3> GetPathCorners()
    {
        if(currentPath == null) yield break;

        Vector3? previousCorner = null;

        foreach(Vector3 corner in currentPath.corners)
        {
            if(minLateralSegments > 0 && previousCorner.HasValue)
            {
                ValueVTuple<Vector3, Vector3>[] lateralSegments = GetLateralSegments(previousCorner.Value, corner);

                foreach(ValueVTuple<Vector3, Vector3> lateralSegment in lateralSegments)
                {
                    yield return lateralSegment.Item1 + lateralSegment.Item2;
                }
            }

            previousCorner = corner;

            yield return corner;
        }
    }

    /// <summary>Path-traversing Routine.</summary>
    protected IEnumerator<Vector3> PathTraversingRoutine()
    {
        if(currentPath == null) yield break;

        float distance = (minWaypointDistance * minWaypointDistance);
        IEnumerator<Vector3> iterator = GetPathCorners();

        Vector3 current = Vector3.zero;
        Vector3 direction = Vector3.zero;

        while(true)
        {
            if(!iterator.MoveNext()) yield break;

            current = iterator.Current;
            direction = character.transform.position - current;
            
            while(direction.sqrMagnitude > distance)
            {
                yield return current;
                direction = character.transform.position - current;
                Debug.DrawRay(character.transform.position, direction, Color.red);
                Debug.DrawRay(current, Vector3.up * 5.0f, Color.yellow);
            }
        }
    }
}
}