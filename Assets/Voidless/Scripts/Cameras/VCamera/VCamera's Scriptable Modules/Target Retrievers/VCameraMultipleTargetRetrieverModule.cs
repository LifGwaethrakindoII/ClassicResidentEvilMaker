using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[CreateAssetMenu(menuName = PATH_TARGETRETRIEVER + "Default Multiple")]
public class VCameraMultipleTargetRetrieverModule : VCameraTargetRetrieverModule
{
    [SerializeField] private VCameraTargetHashSet _targets;
    [Space(5f)]
    [SerializeField] private bool _weightedBounds;
#if UNITY_EDITOR
    [Space(5f)]
    [Header("Gizmos' Attributes:")]
    [SerializeField] private Color gizmosColor;
#endif
    private List<Bounds> _boundsList;

    /// <summary>Gets and Sets targets property.</summary>
    public VCameraTargetHashSet targets
    {
        get
        {
#if UNITY_EDITOR
            if(_targets == null) _targets = new VCameraTargetHashSet(); 
#endif
            return _targets;
        }
        private set { _targets = value; }
    }

    /// <summary>Gets and Sets weightedBounds property.</summary>
    public bool weightedBounds
    {
        get { return _weightedBounds; }
        set { _weightedBounds = value; }
    }

    /// <summary>Gets and Sets target property.</summary>
    public override VCameraTarget target
    {
        get { return _target; }
        set
        {
            _target = value;
            
            if(targets == null) targets = new VCameraTargetHashSet();

            bool contains = targets.Contains(_target);

            if(_target != null && !contains)
            {
                targets.Add(_target);
            
            } else if(_target == null && contains)
            {
                targets.Remove(_target);
            }
        }
    }

    /// <summary>Gets and Sets boundsList property.</summary>
    public List<Bounds> boundsList
    {
        get { return _boundsList; }
        private set { _boundsList = value; }
    }

    /// <summary>Draws Gizmos taking the VCamera's argument into account.</summary>
    /// <param name="_camera">VCamera's requesting to Draw Gizmos.</param>
    public override void DrawGizmos(VCamera _camera)
    {
#if UNITY_EDITOR
        if(!Application.isPlaying) return;
        
        Gizmos.color = gizmosColor;
        Bounds bounds = GetTargetBounds(_camera);
        VGizmos.DrawBounds(bounds);

        Vector3 p1 = new Vector3(
            bounds.min.x,
            bounds.min.y + bounds.extents.y,
            bounds.center.z
        );
        Vector3 p2 = new Vector3(
            bounds.max.x,
            bounds.min.y + bounds.extents.y,
            bounds.center.z
        );
        Vector3 p3 = new Vector3(
            bounds.min.x + bounds.extents.x,
            bounds.min.y,
            bounds.center.z
        );
        Vector3 p4 = new Vector3(
            bounds.min.x + bounds.extents.x,
            bounds.max.y,
            bounds.center.z
        );
        Gizmos.DrawLine(p1, p2);
        Gizmos.DrawLine(p3, p4);
#endif
    }

    /// <summary>Initializes this component taking the VCamera's argument into account.</summary>
    /// <param name="_camera">VCamera requesting for initialization.</param>
    public override void Initialize(VCamera _camera)
    {
        if(targets == null) targets = new VCameraTargetHashSet();
        if(boundsList == null) boundsList = new List<Bounds>();

        if(target == null) return;

        if(!targets.Contains(target)) targets.Add(target);
    }

    /// <summary>Clears all targets' references.</summary>
    public override void ClearTargets()
    {
        targets.Clear();
    }

    /// <summary>Adds Target's Transform into the internal dictionary.</summary>
    /// <param name="_target">Target Transform to add.</param>
    public override void AddTarget(VCameraTarget _target)
    {
        if(_target == null) return;

        if(!targets.Contains(_target))
        targets.Add(_target);
    }

    /// <summary>Removes Target's Transform into the internal dictionary.</summary>
    /// <param name="_target">Target Transform to add.</param>
    public override void RemoveTarget(VCameraTarget _target)
    {
        if(_target == null) return;

        if(targets.Contains(_target))
        targets.Remove(_target);
    }

    /// <returns>True if has any active targets.</returns>
    public override bool HasActiveTargets()
    {
        if(targets == null || targets.Count == 0) return false;

        foreach(VCameraTarget target in targets)
        {
            if(targets != null && target.gameObject.activeSelf)
            return true;
        }

        return false;
    }

    /// <summary>Gets Target's Position.</summary>
    /// <param name="_camera">VCamera's requesting Target's Position.</param>
    /// <returns>Camera's Target.</returns>
    public override Vector3 GetTargetPosition(VCamera _camera)
    {
        if(targets == null) return Vector3.zero;

        Vector3 position = _camera.transform.position;
        Vector3 positions = Vector3.zero;
        float w = 0.0f;
        float count = 0.0f;

        foreach(VCameraTarget target in targets)
        {
            if(target == null || !target.gameObject.activeSelf) continue;

            w = target.weight;

            if(w <= 0.0f) continue;

            positions += Vector3.Lerp(position, (position +  (target.GetPosition() - position)), w);
            count++;
        }

        if(count == 0.0f) positions = position;

        return count > 1.0f ? positions / count : positions;
    }

    /// <summary>Gets Target's Rotation.</summary>
    /// <param name="_camera">VCamera's requesting Target's Rotation.</param>
    /// <returns>Target's Rotation.</returns>
    public override Quaternion GetTargetRotation(VCamera _camera)
    {
        return Quaternion.identity;
    }

    /// <summary>Gets Target's Bounds.</summary>
    /// <param name="_camera">VCamera's requesting Target's Bounds.</param>
    /// <returns>Target's Bounds.</returns>
    public override Bounds GetTargetBounds(VCamera _camera)
    {
        if(targets == null) return target != null ? target.GetBounds() : default(Bounds);

        Vector3 p = _camera.transform.position;
        boundsList.Clear();
        /*Bounds[] targetsBounds = new Bounds[targets.Count];
        int i = 0;*/

        foreach(VCameraTarget target in targets)
        {
            Bounds bounds = target.GetBounds();

            if(weightedBounds)
            {
                float w = target.weight;

                if(w <= 0.0f) continue;

                bounds.center = Vector3.Lerp(p, (p + (target.GetBoundsCenter() - p)), w);
                boundsList.Add(bounds);
            }
            else boundsList.Add(bounds);

            /*targetsBounds[i] = bounds;
            i++;*/
        }

        return VBounds.GetBoundsToFitSet(boundsList.ToArray());
        //return VBounds.GetBoundsToFitSet(targetsBounds);
    }
}
}
