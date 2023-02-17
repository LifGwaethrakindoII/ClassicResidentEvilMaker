using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public enum GizmosDrawType 								/// <summary>Gizmos' Draw Types.</summary>
{
	Wired, 												/// <summary>Wired Draw Type.</summary>
	Solid 												/// <summary>Solid Draw Type.</summary>
}

public static class VGizmos
{
	/// <summary>Draws TransformData into the Gizmos' thread.</summary>
	/// <param name="_data">TransformData to draw.</param>
	/// <param name="r">Gizmos' Radius [0.5f as default].</param>
	public static void DrawTransformData(TransformData _data, float r = 0.5f)
	{
		Vector3 position = _data.position;
		Quaternion rotation = _data.rotation;

		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(position, r);
		Gizmos.color = Color.red;
		Gizmos.DrawRay(position, (rotation * Vector3.right * r));
		Gizmos.color = Color.green;
		Gizmos.DrawRay(position, (rotation * Vector3.up * r));
		Gizmos.color = Color.blue;
		Gizmos.DrawRay(position, (rotation * Vector3.forward * r));
	}

	/// \TODO Finish the damn method...
	public static void DrawWireCamera(Vector3 _center, float _baseDimension, Color _color)
	{
#if UNITY_EDITOR
        float rightX = (_center.x + (_baseDimension * 0.5f));
		float leftX = (_center.x - (_baseDimension * 0.5f));
		float upY = (_center.y + (_baseDimension * 0.5f));
		float downY = (_center.y - (_baseDimension * 0.5f));
		float frontZ = (_center.z + (_baseDimension * 0.5f));
		float backZ = (_center.z - (_baseDimension * 0.5f));

		Gizmos.color = _color;

		/// Back Face:
		Gizmos.DrawLine(new Vector3(leftX, downY, backZ), new Vector3(rightX, downY, backZ));
		Gizmos.DrawLine(new Vector3(leftX, upY, backZ), new Vector3(rightX, upY, backZ));
		Gizmos.DrawLine(new Vector3(leftX, downY, backZ), new Vector3(leftX, upY, backZ));
		Gizmos.DrawLine(new Vector3(rightX, downY, backZ), new Vector3(leftX, upY, backZ));
		/// Front Face:
		Gizmos.DrawLine(new Vector3(leftX, downY, frontZ), new Vector3(rightX, downY, frontZ));
		Gizmos.DrawLine(new Vector3(leftX, upY, frontZ), new Vector3(rightX, upY, frontZ));
		Gizmos.DrawLine(new Vector3(leftX, downY, frontZ), new Vector3(leftX, upY, frontZ));
		Gizmos.DrawLine(new Vector3(rightX, downY, frontZ), new Vector3(leftX, upY, frontZ));
		/// Between Lines:
		Gizmos.DrawLine(new Vector3(leftX, downY, backZ), new Vector3(leftX, downY, frontZ));
		Gizmos.DrawLine(new Vector3(leftX, upY, backZ), new Vector3(leftX, upY, frontZ));
		Gizmos.DrawLine(new Vector3(rightX, downY, backZ), new Vector3(rightX, upY, backZ));
		Gizmos.DrawLine(new Vector3(rightX, downY, frontZ), new Vector3(rightX, upY, frontZ));
#endif
    }


    /// <summary>Draws Capsule.</summary>
    /// <param name="_origin">Capsule's Origin.</param>
    /// <param name="_direction">Capsule's Direction.</param>
    /// <param name="_rotation">Capsule's Orientation represented by a quaternion.</param>
    /// <param name="_radius">Capsule's Radius.</param>
    /// <param name="_length">Capsule's Length.</param>
    public static void DrawCapsule(Vector3 _origin, Vector3 _direction, Quaternion _rotation,  float _radius, float _length)
    {
#if UNITY_EDITOR
    	float length = (_radius * 2.0f) < _length ? (_length - (_radius * 2.0f)) : 0.0f;
    	Vector3 normal = _direction.normalized;
    	Vector3 rightNormal = Vector3.right * _radius;
    	Vector3 upNormal = Vector3.up * _radius;
    	Vector3 forwardNormal = Vector3.forward * _radius;
    	Vector3 projectedPoint = _origin + (normal * length);
    	Matrix4x4 priorMatrix;
    	RotateGizmos(_rotation, out priorMatrix);

    	Gizmos.DrawWireSphere(_origin, _radius);
    	Gizmos.DrawWireSphere(projectedPoint, _radius);
    	Gizmos.DrawLine(_origin + rightNormal, projectedPoint + rightNormal);
    	Gizmos.DrawLine(_origin - rightNormal, projectedPoint - rightNormal);
    	Gizmos.DrawLine(_origin + upNormal, projectedPoint + upNormal);
    	Gizmos.DrawLine(_origin - upNormal, projectedPoint - upNormal);
    	Gizmos.DrawLine(_origin + forwardNormal, projectedPoint + forwardNormal);
    	Gizmos.DrawLine(_origin - forwardNormal, projectedPoint - forwardNormal);

    	Gizmos.matrix = priorMatrix;
#endif
    }

    /// <summary>Draws Capsule.</summary>
    /// <param name="_origin">Capsule's Origin.</param>
    /// <param name="_direction">Capsule's Direction.</param>
    /// <param name="_radius">Capsule's Radius.</param>
    /// <param name="_length">Capsule's Length.</param>
    public static void DrawCapsule(Vector3 _origin, Vector3 _direction,  float _radius, float _length)
    {
#if UNITY_EDITOR
    	float length = (_radius * 2.0f) < _length ? (_length - (_radius * 2.0f)) : 0.0f;
    	Vector3 normal = _direction.normalized;
    	Vector3 rightNormal = Vector3.right * _radius;
    	Vector3 upNormal = Vector3.up * _radius;
    	Vector3 forwardNormal = Vector3.forward * _radius;
    	Vector3 projectedPoint = _origin + (normal * length);

    	Gizmos.DrawWireSphere(_origin, _radius);
    	Gizmos.DrawWireSphere(projectedPoint, _radius);
    	Gizmos.DrawLine(_origin + rightNormal, projectedPoint + rightNormal);
    	Gizmos.DrawLine(_origin - rightNormal, projectedPoint - rightNormal);
    	Gizmos.DrawLine(_origin + upNormal, projectedPoint + upNormal);
    	Gizmos.DrawLine(_origin - upNormal, projectedPoint - upNormal);
    	Gizmos.DrawLine(_origin + forwardNormal, projectedPoint + forwardNormal);
    	Gizmos.DrawLine(_origin - forwardNormal, projectedPoint - forwardNormal);
#endif
    }

    /// <summary>Draws a Rotated Wired Box.</summary>
    /// <param name="_origin">Box's Origin.</param>
    /// <param name="_halfExtents">Box's Dimensions.</param>
    /// <param name="_rotation">Box's Rotation.</param>
    public static void DrawWireBox(Vector3 _origin, Vector3 _halfExtents, Quaternion _rotation)
    {
#if UNITY_EDITOR
    	Vector3 leftUpForwardVertex = _origin + (_rotation * Vector3.Scale((Vector3.left + Vector3.up + Vector3.forward).normalized, _halfExtents));
    	Vector3 leftUpBackVertex = _origin + (_rotation * Vector3.Scale((Vector3.left + Vector3.up + Vector3.back).normalized, _halfExtents));
    	Vector3 leftDownForwardVertex = _origin + (_rotation * Vector3.Scale((Vector3.left + Vector3.down + Vector3.forward).normalized, _halfExtents));
    	Vector3 leftDownBackVertex = _origin + (_rotation * Vector3.Scale((Vector3.left + Vector3.down + Vector3.back).normalized, _halfExtents));
    	Vector3 rightUpForwardVertex = _origin + (_rotation * Vector3.Scale((Vector3.right + Vector3.up + Vector3.forward).normalized, _halfExtents));
    	Vector3 rightUpBackVertex = _origin + (_rotation * Vector3.Scale((Vector3.right + Vector3.up + Vector3.back).normalized, _halfExtents));
    	Vector3 rightDownForwardVertex = _origin + (_rotation * Vector3.Scale((Vector3.right + Vector3.down + Vector3.forward).normalized, _halfExtents));
    	Vector3 rightDownBackVertex = _origin + (_rotation * Vector3.Scale((Vector3.right + Vector3.down + Vector3.back).normalized, _halfExtents));

    	Gizmos.DrawLine(leftUpForwardVertex, leftUpBackVertex);
    	Gizmos.DrawLine(leftUpForwardVertex, leftDownForwardVertex);
    	Gizmos.DrawLine(leftUpForwardVertex, rightUpForwardVertex);
    	Gizmos.DrawLine(leftUpBackVertex, leftDownBackVertex);
    	Gizmos.DrawLine(leftUpBackVertex, rightUpBackVertex);
    	Gizmos.DrawLine(rightUpBackVertex, rightUpForwardVertex);
    	Gizmos.DrawLine(rightUpBackVertex, rightDownBackVertex);
    	Gizmos.DrawLine(rightDownBackVertex, rightDownForwardVertex);
    	Gizmos.DrawLine(rightDownBackVertex, leftDownBackVertex);
    	Gizmos.DrawLine(rightDownForwardVertex, leftDownForwardVertex);
    	Gizmos.DrawLine(leftDownForwardVertex, leftDownBackVertex);
    	Gizmos.DrawLine(rightDownForwardVertex, rightUpForwardVertex);
#endif
    }

    /// <summary>Draws Gizmos' Sphere.</summary>
    /// <param name="_position">Sphere's Position.</param>
    /// <param name="_radius">Sphere's Radius.</param>
    /// <param name="_rotation">Sphere's Rotation.</param>
    /// <param name="_drawType">Draw Type [Wired by default].</param>
    public static void DrawSphere(Vector3 _position, float _radius, Quaternion _rotation, GizmosDrawType _drawType = GizmosDrawType.Wired)
    {
#if UNITY_EDITOR
    	Matrix4x4 priorMatrix;
    	RotateGizmos(_rotation, out priorMatrix);

    	switch(_drawType)
    	{
    		case GizmosDrawType.Wired: Gizmos.DrawWireSphere(_position, _radius); 	break;
    		case GizmosDrawType.Solid: Gizmos.DrawSphere(_position, _radius); 		break;
    	}

    	Gizmos.matrix = priorMatrix;
#endif
    }

    public static void DrawBounds(Bounds _bounds, GizmosDrawType _drawType = GizmosDrawType.Wired)
    {
#if UNITY_EDITOR
    	switch(_drawType)
    	{
    		case GizmosDrawType.Wired: Gizmos.DrawWireCube(_bounds.center, _bounds.size); 	break;
    		case GizmosDrawType.Solid: Gizmos.DrawCube(_bounds.center, _bounds.size); 		break;
    	}
#endif
    }

    /// <summary>Draws Wire Grid Cube.</summary>
    /// <param name="_center">Cube's Center.</param>
    /// <param name="_size">Cube's Size.</param>
    /// <param name="_division">Cell divisions on the 3 axes.</param>
    public static void DrawWireGridCube(Vector3 _center, Vector3 _size, Vector3 _divisions)
    {
#if UNITY_EDITOR
    	Bounds cubeBounds = new Bounds(_center, _size);
    	Bounds[] boundsSet = cubeBounds.GetCartesianGridBounds(_divisions);

    	if(boundsSet == null) return;

    	foreach(Bounds bounds in boundsSet)
    	{
    		Gizmos.DrawWireCube(bounds.center, bounds.size);
    	}
#endif
    }

    /// <summary>Rotates Gizmos' Matrix.</summary>
    /// <param name="_rotation">New Rotation.</param>
    /// <param name="_priorMatrix">Previous Matrix's reference.</param>
    public static void RotateGizmos(Quaternion _rotation, out Matrix4x4 _priorMatrix)
    {
#if UNITY_EDITOR
    	Matrix4x4 gizmoTransform = Matrix4x4.TRS(Vector3.zero, _rotation, Vector3.one);
		_priorMatrix = Gizmos.matrix;
		Gizmos.matrix *= gizmoTransform;
#else
		_priorMatrix = default(Matrix4x4);
#endif		
    }

	/// <summary>Draws Transform's normals when it is not selected.</summary>
	/// <param name="_transform">Transform to draw normals.</param>
	/// <param name="_normalProjection">Normal length projection.</param>
	public static void DrawNormals(this Transform _transform, float _normalProjection)
	{
#if UNITY_EDITOR
        Gizmos.color = Color.red;
		Gizmos.DrawLine(_transform.position, (_transform.position + (_transform.right * _normalProjection)));
		Gizmos.color = Color.green;
		Gizmos.DrawLine(_transform.position, (_transform.position + (_transform.up * _normalProjection)));
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(_transform.position, (_transform.position + (_transform.forward * _normalProjection)));
#endif
        }

	/// <summary>[Overload Method] Draws GameObject's normals when it is not selected.</summary>
	/// <param name="_gameObject">GameObject to draw normals.</param>
	/// <param name="_normalProjection">Normal length projection.</param>
	public static void DrawNormals(this GameObject _gameObject, float _normalProjection)
	{
#if UNITY_EDITOR
        _gameObject.transform.DrawNormals(_normalProjection);
#endif
        }

	/// <summary>[Overload Method] Draws MonoBehaviour's normals when it is not selected.</summary>
	/// <param name="_monoBehaviour">MonoBehaviour to draw normals.</param>
	/// <param name="_normalProjection">Normal length projection.</param>
	public static void DrawNormals(this MonoBehaviour _monoBehaviour, float _normalProjection)
	{
#if UNITY_EDITOR
        _monoBehaviour.gameObject.DrawNormals(_normalProjection);
#endif
    }

    /// \TODO DELETE!
	/// <summary>Draws a Gizmo Ray.</summary>
	/// <param name="_origin">Ray's initial position.</param>
	/// <param name="_direction">Ray's direction.</param>
	/// <param name="_distance">Ray's distance.</param>
	/// <param name="_color">Ray's color.</param>
	public static void DrawGizmoRay(Vector3 _origin, Vector3 _direction, float _distance, Color _color)
	{
#if UNITY_EDITOR
        Gizmos.color = _color;
		Gizmos.DrawLine(
			_origin,
			(_origin + (_direction.normalized * _distance))
		);
#endif
    }

        /// \TODO DELETE!
	/// <summary>Draws a Gizmo Ray.</summary>
	/// <param name="_origin">Ray's initial position.</param>
	/// <param name="_direction">Ray's direction.</param>
	/// <param name="_distance">Ray's distance.</param>
	/// <param name="_color">Ray's color.</param>
	public static void DrawGizmoRay(Ray _ray, float _distance, Color _color)
	{
#if UNITY_EDITOR
        Gizmos.color = _color;
		Gizmos.DrawLine(
			_ray.origin,
			(_ray.origin + (_ray.direction.normalized * _distance))
		);
#endif
        }

	/// <summary>Draws a Gizmo Ray.</summary>
	/// <param name="_origin">Ray's initial position.</param>
	/// <param name="_direction">Ray's direction.</param>
	/// <param name="_distance">Ray's distance.</param>
	/// <param name="_color">Ray's color.</param>
	public static void DrawGizmoRay2D(Ray2D _ray, float _distance, Color _color)
	{
#if UNITY_EDITOR
        Vector3 origin = _ray.origin.ToVector3();
		Vector3 direction = _ray.direction.ToVector3();
		
		Gizmos.color = _color;
		Gizmos.DrawLine(
			origin,
			(origin + (direction.normalized * _distance))
		);
#endif
     }

	/// Credits: Arakade, GIT on: https://gist.github.com/Arakade/9dd844c2f9c10e97e3d0
	/// <summary>Draws GUI Text on the scene [Editor Mode only].</summary>
	/// <param name="_text">Test to show.</param>
	/// <param name="_worldPosition">Position [based on world] to show the text on.</param>
	/// <param name="_offset">Additional offset.</param>
	/// <param name="_color">Text's color.</param>
	static public void DrawText(string _text, Vector2 _worldPosition, Vector2 _offset, Color _color)
	{
#if UNITY_EDITOR
        UnityEditor.Handles.BeginGUI();

	    var restoreColor = GUI.color;

	    GUI.color = _color;
	    var view = UnityEditor.SceneView.currentDrawingSceneView;
	    Vector3 screenPos = view.camera.WorldToScreenPoint(_worldPosition);

	    if (screenPos.y < 0 || screenPos.y > Screen.height || screenPos.x < 0 || screenPos.x > Screen.width || screenPos.z < 0)
	    {
	        GUI.color = restoreColor;
	        UnityEditor.Handles.EndGUI();
	        return;
	    }

	    Vector2 size = GUI.skin.label.CalcSize(new GUIContent(_text));
	    Rect labelPosition = new Rect
	    (
	    	(screenPos.x - (size.x * 0.5f)) + _offset.x,
	    	(-screenPos.y + view.position.height) - _offset.y,
	    	size.x,
	    	size.y
	    );
	    GUI.Label(labelPosition, _text);
	    GUI.color = restoreColor;
	    UnityEditor.Handles.EndGUI();
#endif
    }

	/// <summary>Draws a Cuadratic Beizer Curve Line between an initial point, a tangent and a final point on the scene.</summary>
	/// <param name="_initial">Curve's initial point [P0].</param>
	/// <param name="_finalPoint">Curve's final point [Pf].</param>
	/// <param name="_tangent">Curve's tangent [P1].</param>
	/// <param name="_timeSplit">Time split. The bigger the time split, the smoother the curve will look.</param>
	/// <param name="_debugPoints">Debug Points?.</param>
	/// <param name="_color">Line's color.</param>
	public static void DrawCuadraticBeizerCurve(Vector3 _initialPoint, Vector3 _finalPoint, Vector3 _tangent, int _timeSplit, Color _color/*, bool _debugPoints*/)
	{
#if UNITY_EDITOR
        float timeSplitInverseMultiplicative = (1f / (1f * _timeSplit));

		Gizmos.color = _color;

		/*if(_debugPoints)
		{
			Gizmos.DrawWireSphere(_initialPoint, 0.5f);
			Gizmos.DrawWireSphere(_finalPoint, 0.5f);
			Gizmos.DrawWireSphere(_tangent, 0.5f);
		}*/

		for(int i = 0; i < _timeSplit -1; i++)
		{
			Vector3 initialPoint = VMath.CuadraticBeizer(_initialPoint, _finalPoint, _tangent, (i * timeSplitInverseMultiplicative));
			Vector3 finalPoint = VMath.CuadraticBeizer(_initialPoint, _finalPoint, _tangent, ((i + 1) * timeSplitInverseMultiplicative));
			Gizmos.DrawLine(initialPoint, finalPoint);
		}
#endif
    }

	/// <summary>Draws a Cubic Beizer Curve Line between an initial point, a tangent and a final point on the scene.</summary>
	/// <param name="_initial">Curve's initial point [P0].</param>
	/// <param name="_finalPoint">Curve's final point [Pf].</param>
	/// <param name="_startTangent">Curve's start tangent [P1].</param>
	/// <param name="_endTangent">Curve's end tangent [P2].</param>
	/// <param name="_timeSplit">Time split. The bigger the time split, the smoother the curve will look.</param>
	/// <param name="_debugPoints">Debug Points?.</param>
	/// <param name="_color">Line's color.</param>
	public static void DrawCubicBeizerCurve(Vector3 _initialPoint, Vector3 _finalPoint, Vector3 _startTangent, Vector3 _endTangent, int _timeSplit, Color _color/*, bool _debugPoints*/)
	{
#if UNITY_EDITOR
        float timeSplitInverseMultiplicative = (1f / (1f * _timeSplit));

		Gizmos.color = _color;

		/*if(_debugPoints)
		{
			Gizmos.DrawWireSphere(_initialPoint, 0.5f);
			Gizmos.DrawWireSphere(_finalPoint, 0.5f);
			Gizmos.DrawWireSphere(_startTangent, 0.5f);
		}*/

		for(int i = 0; i < _timeSplit -1; i++)
		{
			Vector3 initialPoint = VMath.CubicBeizer(_initialPoint, _finalPoint, _startTangent, _endTangent, (i * timeSplitInverseMultiplicative));
			Vector3 finalPoint = VMath.CubicBeizer(_initialPoint, _finalPoint, _startTangent, _endTangent, ((i + 1) * timeSplitInverseMultiplicative));
			Gizmos.DrawLine(initialPoint, finalPoint);
		}
#endif
    }
}
}
