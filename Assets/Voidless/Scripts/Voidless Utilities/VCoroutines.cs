using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System;

/*============================================================
**
** Class:  VCoroutines
**
** Purpose: This static class contains utility properties and methods for Coroutines:
** 
** 	- General-purpose IEnumerators
**  - Static YieldInstructions. The following are the reasons:
**		- Unity's Coroutine Manager does reflection on what the IEnumerators returns.
**		- So, the same Manager does all the logic just by interpreting the YieldInstructions' Types.
**		- With this, we don't need to be initializing some YieldInstructions, such as the ones being declared.
** 	- Methods for safe startups and dispatchments of Coroutines
**
**
** Author: Lîf Gwaethrakindo
**
==============================================================*/

namespace Voidless
{
public enum TransformRelativeness
{
	World,
	Local
}

public enum InterpolationType
{
	Lerp,
	Slerp
}

public static class VCoroutines
{
	public static readonly WaitForEndOfFrame WAIT_MAIN_THREAD; 		/// <summary>Wait for end of main thread's Yield Instruction.</summary>
	public static readonly WaitForFixedUpdate WAIT_PHYSICS_THREAD; 	/// <summary>Wait for end of physics thread's Yield Instruction.</summary>

	/// <summary>VoidlessCoroutine's Static Constructor.</summary>
	static VCoroutines()
	{
		WAIT_MAIN_THREAD = new WaitForEndOfFrame(); 	
		WAIT_PHYSICS_THREAD = new WaitForFixedUpdate();
	}

	/// <summary>Stops reference's Coroutine and then  sets it to null [if the Coroutine is different than null]. Starts Coroutine.</summary>
	/// <param name="_monoBehaviour">Extension MonoBehaviour, used to call StopCoroutine and StartCoroutine.</param>
	/// <param name="_iterator">Coroutine's Iterator.</param>
	/// <param name="_coroutine">Coroutine to dispatch and to initialize.</param>
	public static Coroutine StartCoroutine(this MonoBehaviour _monoBehaviour, IEnumerator _iterator, ref Coroutine _coroutine)
	{
		if(!_monoBehaviour.enabled || !_monoBehaviour.gameObject.activeSelf)
		{
			_coroutine = null;
			return null;
		}

		if(_coroutine != null)
		{
			_monoBehaviour.StopCoroutine(_coroutine);
			_coroutine = null;
		}
		return _coroutine = _monoBehaviour.StartCoroutine(_iterator);
	}

	/// <summary>Stops reference's Coroutine and then  sets it to null [if the Coroutine is different than null].</summary>
	/// <param name="_monoBehaviour">Extension MonoBehaviour, used to call StopCoroutine.</param>
	/// <param name="_coroutine">Coroutine to dispatch.</param>
	public static void DispatchCoroutine(this MonoBehaviour _monoBehaviour, ref Coroutine _coroutine)
	{
		if(_coroutine != null)
		{
			_monoBehaviour.StopCoroutine(_coroutine);
			_coroutine = null;
		}
	}

	/// <summary>Creates new Asynchronous Behavior as a Coroutine.</summary>
	/// <param name="_monoBehavior">MonoBehavior that will start the coroutine.</param>
	/// <param name="_iterator">Coroutine's Iterator.</param>
	/// <param name="_startAutomagically">Start the coroutine as soon as the Behavior is initialized [true by default].</param>
	/// <returns>Created Behavior.</returns>
	public static Behavior StartBehaviorCoroutine(this MonoBehaviour _monoBehaviour, IEnumerator _iterator, bool _startAutomagically = true)
	{
		return new Behavior(_monoBehaviour, _iterator, _startAutomagically);
	}

	/// <summary>Ends Behaviour's reference, and then sets it to null.</summary>
	/// <param name="_behavior">Behavior to dispatch.</param>
	public static void DispatchBehavior(ref Behavior _behavior)
	{
		if(_behavior != null)
		{
			_behavior.EndBehavior();
			_behavior = null;
		}
	}

#region IEnumerators:
	/// <summary>Waits for a certain ILoadable instance to be loaded.</summary>
	/// <param name="_monoBehaviour">Requesting MonoBehaviour.</param>
	/// <param name="_loadable">Expected ILoadable instance to load.</param>
	/// <param name="onObjectLoaded">Optional callback invoked when the object is loaded.</param>
	public static IEnumerator WaitForLoadable<T>(this MonoBehaviour _monoBehaviour, T _loadable, Action onObjectLoaded = null) where T : MonoBehaviour, ILoadable
	{
		while(!_loadable.Loaded) { yield return null; }
		if(onObjectLoaded != null) onObjectLoaded();
	}

	/// <summary>Wait for some seconds, and then invoke a callback.</summary>
	/// <param name="_monoBehaviour">Requester MonoBehaviour.</param>
	/// <param name="_seconds">Wait duration.</param>
	/// <param name="onIEnumeratorEnds">Callback invoked when IEnumerator ends.</param>
	public static IEnumerator WaitSeconds(this MonoBehaviour _monoBehaviour, float _seconds, Action onWaitEnds = null)
	{
		SecondsDelayWait wait = new SecondsDelayWait(_seconds);
		while(wait.MoveNext()) yield return null;
		if(onWaitEnds != null) onWaitEnds();
	}

	/// <summary>Wait for some random seconds, and then invoke a callback.</summary>
	/// <param name="_monoBehaviour">Requester MonoBehaviour.</param>
	/// <param name="_secondsRange">Random range of seconds.</param>
	/// <param name="onIEnumeratorEnds">Callback invoked when IEnumerator ends.</param>
	public static IEnumerator WaitRandomSeconds(this MonoBehaviour _monoBehaviour, FloatRange _secondsRange, Action onWaitEnds = null)
	{
		float randomDuration = UnityEngine.Random.Range(_secondsRange.min, _secondsRange.max);
		SecondsDelayWait wait = new SecondsDelayWait(randomDuration);
		while(wait.MoveNext()) yield return null;
		if(onWaitEnds != null) onWaitEnds();
	}

	/// <summary>Does action while waiting some seconds.</summary>
	/// <param name="_monoBehaviour">Requester MonoBehaviour.</param>
	/// <param name="_seconds">Seconds to wait.</param>
	/// <param name="doWhileAction">Action to do while waiting seconds.</param>
	/// <param name="onWaitEnds">Optional callback invoked when the wait ends.</param>
	public static IEnumerator DoWhileWaitingSeconds(this MonoBehaviour _monoBehaviour, float _seconds, Action doWhileAction, Action onWaitEnds = null)
	{
		SecondsDelayWait wait = new SecondsDelayWait(_seconds);

		while(wait.MoveNext())
		{
			doWhileAction();
			yield return null;
		}

		if(onWaitEnds != null) onWaitEnds();
	}

	/// <summary>Does action while waiting some random seconds.</summary>
	/// <param name="_monoBehaviour">Requester MonoBehaviour.</param>
	/// <param name="_secondsRange">Random range of seconds.</param>
	/// <param name="doWhileAction">Action to do while waiting seconds.</param>
	/// <param name="onWaitEnds">Optional callback invoked when the wait ends.</param>
	public static IEnumerator DoWhileWaitingSeconds(this MonoBehaviour _monoBehaviour, FloatRange _secondsRange, Action doWhileAction, Action onWaitEnds = null)
	{
		float randomDuration = UnityEngine.Random.Range(_secondsRange.min, _secondsRange.max);
		SecondsDelayWait wait = new SecondsDelayWait(randomDuration);

		while(wait.MoveNext())
		{
			doWhileAction();
			yield return null;
		}

		if(onWaitEnds != null) onWaitEnds();
	}

	/// <summary>Waits until a condition is false, to then invoke a callbakc when it is done.</summary>
	/// <param name="_monoBehaviour">Requester MonoBehaviour.</param>
	/// <param name="_condition">Condition iterator.</param>
	/// <param name="onConditionEnds">Callback invoked when the condition ends.</param>
	public static IEnumerator WaitUntilCondition(this MonoBehaviour _monoBehaviour, Func<bool> _condition, Action onWaitEnds = null)
	{
		while(!_condition()) { yield return null; }
		if(onWaitEnds != null) onWaitEnds();
	}

	/// <summary>Waits until a condition is false, to then invoke a callbakc when it is done.</summary>
	/// <param name="_monoBehaviour">Requester MonoBehaviour.</param>
	/// <param name="_condition">Condition iterator.</param>
	/// <param name="onConditionEnds">Callback invoked when the condition ends.</param>
	public static IEnumerator WaitUntilCondition(this MonoBehaviour _monoBehaviour, IEnumerator _condition, Action onConditionEnds)
	{
		while(_monoBehaviour.enabled && _condition.MoveNext()) { yield return null; }
		onConditionEnds();
	}

	/// <summary>Moves Transform towards position.</summary>
	/// <param name="_transform">Transform to move.</param>
	/// <param name="_position">Position to move the transform to.</param>
	/// <param name="_duration">Displacement's duration.</param>
	/// <param name="onMoveEnds">Optional Callback invoked when the displacement ends.</param>
	public static IEnumerator DisplaceToPosition(this Transform _transform, Vector3 _position, float _duration, Action onMoveEnds = null, Func<float, float> f = null)
	{
		Vector3 originalPosition = _transform.localPosition;
		float inverseDuration = 1.0f / _duration;
		float t = 0.0f;

		if(f == null) f = VMath.DefaultNormalizedPropertyFunction;

		while(t < (1.0f + Mathf.Epsilon))
		{
			_transform.localPosition = Vector3.Lerp(originalPosition, _position, f(t));
			t += (Time.deltaTime * inverseDuration);
			yield return null;
		}

		_transform.localPosition = _position;

		if(onMoveEnds != null) onMoveEnds();
	}

	/// <summary>Moves Rigidbody towards given target at given speed.</summary>
	/// <param name="_rigidbody">RTigidbody to move.</param>
	/// <param name="_target">Destination target.</param>
	/// <param name="_speed">Movement's Speed.</param>
	/// <param name="_distance">Distance's threshold [0.0f by default].</param>
	/// <param name="onMovementEnds">Optional callback to invoke when the movement is finished.</param>
	public static IEnumerator MoveRigidbodyTowards(this Rigidbody _rigidbody, Vector3 _target, float _speed, float _distance = 0.0f, Action onMovementEnds = null)
	{
		Vector3 direction = _target - _rigidbody.position;
		float squareDistance = _distance * _distance;

		while(direction.sqrMagnitude > squareDistance)
		{
			_rigidbody.MovePosition(_rigidbody.position + (direction.normalized * _speed * Time.fixedDeltaTime));
			direction = _target - _rigidbody.position;
			yield return WAIT_PHYSICS_THREAD;
		}

		_rigidbody.MovePosition(_target);
		if(onMovementEnds != null) onMovementEnds();
	}

	/// <summary>Interpolates Transform's Rotation in to desired rotation.</summary>
	/// <param name="_transform">Transform to rotate.</param>
	/// <param name="_rotation">Desired rotation.</param>
	/// <param name="_duration">Interpolation's duration.</param>
	/// <param name="onRotationEnds">Optional callback invoked when the rotation ends.</param>
	public static IEnumerator PivotToRotation(this Transform _transform, Quaternion _rotation, float _duration, TransformRelativeness _relativeness = TransformRelativeness.World, Action onRotationEnds = null)
	{
		Quaternion originalRotation = _relativeness == TransformRelativeness.World ? _transform.rotation : _transform.localRotation;
		float inverseDuration = 1.0f / _duration;
		float t = 0.0f;

		while(t < (1.0f + Mathf.Epsilon))
		{
			switch(_relativeness)
			{
				case TransformRelativeness.World:
				_transform.rotation = Quaternion.Lerp(originalRotation, _rotation, t);
				break;

				case TransformRelativeness.Local:
				_transform.localRotation = Quaternion.Lerp(originalRotation, _rotation, t);
				break;
			}

			t += (Time.deltaTime * inverseDuration);
			yield return null;
		}

		switch(_relativeness)
		{
			case TransformRelativeness.World:
			_transform.rotation = _rotation;
			break;

			case TransformRelativeness.Local:
			_transform.localRotation = _rotation;
			break;
		}

		if(onRotationEnds != null) onRotationEnds();
	}

	/// \TODO I'll Later improve it [I guess...]
	/// <summary>Rotates a set of Components [containing transforms] around another transform while a condition is met.</summary>
	/// <param name="_components">Set of Components to rotate around.</param>
	/// <param name="_around">Transform to rotate the Components around.</param>
	/// <param name="axes">Rotation's axes.</param>
	/// <param name="o">Additional offset.</param>
	/// <param name="a">Angular speed.</param>
	/// <param name="r">Separation's Radius.</param>
	/// <param name="condition">Condition that defines whether the rotation keeps being done.</param>
	public static IEnumerator RotateAroundTransform(Component[] _components, Transform _around, Vector3 axes, Vector3 o, float a, float r, Func<bool> condition)
	{
		int length = _components.Length;
		Quaternion currentRotation = Quaternion.identity;
		Vector3 rotationAxes = axes.normalized * a;
		Vector3[] localPositions = new Vector3[length];

		for(int i = 0; i < length; i++)
		{
			localPositions[i] = (_components[i].transform.position - (_around.position + o)).normalized * r;
		}

		while(condition())
		{
			for(int i = 0; i < length; i++)
			{
				currentRotation *= Quaternion.Euler(rotationAxes * Time.deltaTime);
				_components[i].transform.position = (_around.position + o) +  ((_around.rotation * currentRotation) * localPositions[i]);
			}

			yield return null;
		}
	}

	/// <summary>Rotates Transform around given axis, at a fixed time step.</summary>
	/// <param name="_transform">Transform to rotate.</param>
	/// <param name="_axis">Relative to what axis to rotate.</param>
	/// <param name="_rotation">Rotation step given at each frame.</param>
	/// <param name="_space">Relative to which space to rotate.</param>
	public static IEnumerator RotateOnAxis(this Transform _transform, Vector3 _axis, float _rotation, Space _space = Space.Self)
	{
		while(true)
		{
			_transform.Rotate(_axis, _rotation * Time.deltaTime, _space);
			yield return null;
		}
	}

	/// <summary>Rotates Transform around given axis, at a fixed time step.</summary>
	/// <param name="_transform">Transform to rotate.</param>
	/// <param name="_axis">Relative to what axis to rotate.</param>
	/// <param name="_rotation">Rotation step given at each frame.</param>
	/// <param name="_duration">Rotation's duration.</param>
	/// <param name="_space">Relative to which space to rotate.</param>
	/// <param name="onRotationEnds">Optional Callback invoked when the rotation ends.</param>
	public static IEnumerator RotateOnAxis(this Transform _transform, Vector3 _axis, float _rotation, float _duration, Space _space = Space.Self, Action onRotationEnds = null)
	{
		float rotationSplit = (_rotation / _duration);
		float n = 0.0f;

		while(n < (1.0f + Mathf.Epsilon))
		{
			_transform.Rotate(_axis, rotationSplit * Time.deltaTime, _space);
			n += Time.deltaTime;
			yield return null;
		}

		if(onRotationEnds != null) onRotationEnds();
	}

	/// <summary>Rotates Tranform by given Vector, at a fixed time step.</summary>
	/// <param name="_transform">Transform to rotate.</param>
	/// <param name="_rotationVector">Rotation vector to add to this Transform's rotation each frame.</param>
	/// <param name="_duration">Rotation's duration.</param>
	/// <param name="_space">Relative to which space to rotate.</param>
	/// <param name="onRotationEnds">Optional Callback invoked when the rotation ends.</param>
	public static IEnumerator RotateVector3(this Transform _transform, Vector3 _rotationVector, float _duration, Space _space = Space.Self, Action onRotationEnds = null)
	{
		Vector3 rotationSplit = ((_rotationVector * Time.deltaTime) / _duration);
		float n = 0.0f;

		while(n < (1.0f + Mathf.Epsilon))
		{
			_transform.Rotate(rotationSplit, _space);
			n += Time.deltaTime;
			yield return null;
		}

		if(onRotationEnds != null) onRotationEnds();
	}

	/// <summary>Rotates Transform so that given relative direction points towards another direction.</summary>
	/// <param name="_transform">Transform that will be rotated.</param>
	/// <param name="_rotationAxis">Rotation's Axis.</param>
	/// <param name="_directionAxis">Direction relative to the transform that must point towards given direction.</param>
	/// <param name="_direction">Direction to point towards to.</param>
	/// <param name="_speed">Rotation's Speed.</param>
	/// <param name="_dotTolerance">Dot Product's Tolerance.</param>
	/// <param name="_space">Space relativeness [Space.Self by default].</param>
	/// <param name="onRotationEnds">Optional callback invoked when the rotation ends.</param>
	public static IEnumerator RotateOnAxisTowardsDirection(this Transform _transform, Vector3 _rotationAxis, Vector3 _directionAxis, Vector3 _direction, float _speed, float _dotTolerance = 0.005f, Space _space = Space.Self, Action onRotationEnds = null)
	{
		/// Normalize the arguments' axes:
		_rotationAxis = _rotationAxis.normalized;
		_directionAxis = _directionAxis.normalized;
		_direction = _direction.normalized;

		if(_space == Space.Self) _rotationAxis = _transform.rotation * _rotationAxis;

		float dot = Vector3.Dot(_transform.rotation * _directionAxis, _direction);

		while(dot < (1.0f - _dotTolerance))
		{
			_transform.rotation *= Quaternion.Euler(_rotationAxis * _speed * Time.deltaTime);
			dot = Vector3.Dot(_transform.rotation * _directionAxis, _direction);
			yield return null;
		}

		if(onRotationEnds != null) onRotationEnds();
	}

	/// <summary>Scales given Transform by a regular Vector of the given value at a duration, invokes an optional callback when finished scaling.</summary>
	/// <param name="_transform">Transform to scale.</param>
	/// <param name="_scaleNormal">Value that will define the regular vector this transform will be scaled to.</param>
	/// <param name="_duration">Scaling's duration.</param>
	/// <param name="onScaleEnds">Optional Callback invoked when the scaling ends.</param>
	/// <param name="f">Optional Normalized Property Function for the scale.</param>
	public static IEnumerator RegularScale(this Transform _transform, float _scaleNormal, float _duration, Action onScaleEnds = null, Func<float, float> f = null)
	{
		if(f == null) f = VMath.DefaultNormalizedPropertyFunction;

		Vector3 originalScale = _transform.localScale;
		Vector3 destinyScale = VVector3.Regular(_scaleNormal);
		float inverseDuration = 1.0f / _duration;
		float t = 0.0f;

		while(t < (1.0f + Mathf.Epsilon))
		{
			_transform.localScale = Vector3.Lerp(originalScale, destinyScale, f(t));
			t += (Time.deltaTime * inverseDuration);
			yield return null;
		}

		_transform.localScale = destinyScale;

		if(onScaleEnds != null) onScaleEnds();
	}

	/// <summary>Scales given Transform by given Vector3 at a duration, invokes an optional callback when finished scaling.</summary>
	/// <param name="_transform">Transform to scale.</param>
	/// <param name="_scaleVector">Vector this transform will be scaled to.</param>
	/// <param name="_duration">Scaling's duration.</param>
	/// <param name="onScaleEnds">Optional Callback invoked when the scaling ends.</param>
	/// <param name="f">Optional Normalized Property Function for the scale.</param>
	public static IEnumerator IrregularScale(this Transform _transform, Vector3 _scaleVector, float _duration, Action onScaleEnds = null, Func<float, float> f = null)
	{
		if(f == null) f = VMath.DefaultNormalizedPropertyFunction;

		Vector3 originalScale = _transform.localScale;
		float inverseDuration = 1.0f / _duration;
		float t = 0.0f;

		while(t < (1.0f + Mathf.Epsilon))
		{
			_transform.localScale = Vector3.Lerp(originalScale, _scaleVector, f(t));
			t += (Time.deltaTime * inverseDuration);
			yield return null;
		}

		_transform.localScale = _scaleVector;

		if(onScaleEnds != null) onScaleEnds();
	}

	/// <summary>Interpolates selected Transform's properties into given Transform.</summary>
	/// <param name="_transform">Transform to interpolate.</param>
	/// <param name="_data">Destination Transform.</param>
	/// <param name="_duration">Interpolation's Duration.</param>
	/// <param name="_properties">Transform's properties to interpolate [PositionAndRotation by default].</param>
	/// <param name="onLerpEnds">Optional callback invoked when the interpolation ends.</param>
	public static IEnumerator LerpTowardsTransform(this Transform _transform, Transform _data, float _duration, TransformProperties _properties = TransformProperties.PositionAndRotation, Space _space = Space.World, InterpolationType _lerpType = InterpolationType.Lerp, Action onLerpEnds = null, Func<float, float> f = null)
	{
		TransformData originalData = _transform;
		float inverseDuration = 1.0f / _duration;
		float t = 0.0f;
		Vector3 positionA = _space == Space.World ? originalData.position : originalData.localPosition;
		Vector3 positionB = _space == Space.World ? _data.position : _data.localPosition;
		Quaternion rotationA = _space == Space.World ? originalData.rotation : originalData.localRotation;
		Quaternion rotationB = _space == Space.World ? _data.rotation : _data.localRotation;
		Func<Vector3, Vector3, float, Vector3> vectorLerp = null;
		Func<Quaternion, Quaternion, float, Quaternion> rotationLerp = null;

		if(f == null) f = VMath.DefaultNormalizedPropertyFunction;

		switch(_lerpType)
		{
			case InterpolationType.Lerp:
			vectorLerp = Vector3.Lerp;
			rotationLerp = Quaternion.Lerp;
			break;

			case InterpolationType.Slerp:
			vectorLerp = Vector3.Slerp;
			rotationLerp = Quaternion.Slerp;
			break;
		}

		while(t < 1.0f)
		{
			switch(_space)
			{
				case Space.World:
				if((_properties | TransformProperties.Position) == _properties) _transform.position = vectorLerp(positionA, positionB, t);
				if((_properties | TransformProperties.Rotation) == _properties) _transform.rotation = rotationLerp(rotationA, rotationB, t);
				break;

				case Space.Self:
				if((_properties | TransformProperties.Position) == _properties) _transform.localPosition = vectorLerp(positionA, positionB, t);
				if((_properties | TransformProperties.Rotation) == _properties) _transform.localRotation = rotationLerp(rotationA, rotationB, t);
				break;
			}

			if((_properties | TransformProperties.Scale) == _properties) _transform.localScale = vectorLerp(originalData.scale, _data.localScale, t);

			t += (Time.deltaTime * inverseDuration);
			yield return null;
		}

		switch(_space)
		{
			case Space.World:
			if((_properties | TransformProperties.Position) == _properties) _transform.position = positionB;
			if((_properties | TransformProperties.Rotation) == _properties) _transform.rotation = rotationB;
			break;

			case Space.Self:
			if((_properties | TransformProperties.Position) == _properties) _transform.localPosition = positionB;
			if((_properties | TransformProperties.Rotation) == _properties) _transform.localRotation = rotationB;
			break;
		}

		if(onLerpEnds != null) onLerpEnds();
	}

	/// <summary>Interpolates selected Transform's properties into given TransformData.</summary>
	/// <param name="_transform">Transform to interpolate.</param>
	/// <param name="_data">Destination TransformData.</param>
	/// <param name="_duration">Interpolation's Duration.</param>
	/// <param name="_properties">Transform's properties to interpolate [PositionAndRotation by default].</param>
	/// <param name="onLerpEnds">Optional callback invoked when the interpolation ends.</param>
	public static IEnumerator LerpTowardsData(this Transform _transform, TransformData _data, float _duration, TransformProperties _properties = TransformProperties.PositionAndRotation, Space _space = Space.World, Action onLerpEnds = null, Func<float, float> f = null)
	{
		TransformData originalData = _transform;
		float inverseDuration = 1.0f / _duration;
		if(f == null) f = VMath.DefaultNormalizedPropertyFunction;
		float t = 0.0f;
		Vector3 positionA = _space == Space.World ? originalData.position : originalData.localPosition;
		Vector3 positionB = _space == Space.World ? _data.position : _data.localPosition;
		Quaternion rotationA = _space == Space.World ? originalData.rotation : originalData.localRotation;
		Quaternion rotationB = _space == Space.World ? _data.rotation : _data.localRotation;
		/*Action<Transform, Vector3> setPosition = null;
		Action<Transform, Quaternion> setRotation = null;

		switch(_space)
		{
			case Space.World:
			setPosition = VTransform.SetPosition;
			setRotation = VTransform.SetRotation;
			break;

			case Space.Self:
			setPosition = VTransform.SetLocalPosition;
			setRotation = VTransform.SetLocalRotation;
			break;
		}*/

		while(t < 1.0f)
		{
			/*if((_properties | TransformProperties.Position) == _properties) setPosition(_transform, Vector3.Lerp(positionA, positionB, t));
			if((_properties | TransformProperties.Rotation) == _properties) setRotation(_transform, Quaternion.Lerp(rotationA, rotationB, t));
			if((_properties | TransformProperties.Scale) == _properties) _transform.localScale = Vector3.Lerp(originalData.scale, _data.scale, t);*/

			switch(_space)
			{
				case Space.World:
				if((_properties | TransformProperties.Position) == _properties) _transform.position = Vector3.Lerp(positionA, positionB, f(t));
				if((_properties | TransformProperties.Rotation) == _properties) _transform.rotation = Quaternion.Lerp(rotationA, rotationB, f(t));
				break;

				case Space.Self:
				if((_properties | TransformProperties.Position) == _properties) _transform.localPosition = Vector3.Lerp(positionA, positionB, f(t));
				if((_properties | TransformProperties.Rotation) == _properties) _transform.localRotation = Quaternion.Lerp(rotationA, rotationB, f(t));
				break;
			}

			if((_properties | TransformProperties.Scale) == _properties) _transform.localScale = Vector3.Lerp(originalData.scale, _data.scale, f(t));

			t += (Time.deltaTime * inverseDuration);
			yield return null;
		}

		/*if((_properties | TransformProperties.Position) == _properties) setPosition(_transform, positionB);
		if((_properties | TransformProperties.Rotation) == _properties) setRotation(_transform, rotationB);
		if((_properties | TransformProperties.Scale) == _properties) _transform.localScale = _data.scale;*/

		switch(_space)
		{
			case Space.World:
			if((_properties | TransformProperties.Position) == _properties) _transform.position = positionB;
			if((_properties | TransformProperties.Rotation) == _properties) _transform.rotation = rotationB;
			break;

			case Space.Self:
			if((_properties | TransformProperties.Position) == _properties) _transform.localPosition = positionB;
			if((_properties | TransformProperties.Rotation) == _properties) _transform.localRotation = rotationB;
			break;
		}

		if(onLerpEnds != null) onLerpEnds();
	}

	/// <summary>Shakes Transform's position.</summary>
	/// <param name="_monoBehaviour">Requester MonoBehaviour.</param>
	/// <param name="_duration">Shake's Duration.</param>
	/// <param name="_speed">Sake's Speed.</param>
	/// <param name="_magnitude">Shake's Magnitude.</param>
	/// <param name="onShakeEnds">Action invoked when the shaking ends.</param>
	public static IEnumerator ShakePosition(this Transform _transform, float _duration, float _speed, float _magnitude, Action onShakeEnds = null)
	{
		Vector3 originalPosition = _transform.localPosition;
		float elapsedTime = 0.0f;
		float scaledSpeed = 0.0f;

		while((elapsedTime < (_duration + Mathf.Epsilon)) && (_transform != null))
		{
			scaledSpeed = (Time.time * _speed);

			_transform.localPosition = originalPosition.WithAddedXAndY
			(
				((Mathf.PerlinNoise(scaledSpeed, 0.0f) * _magnitude) - (_magnitude * 0.5f)),
				((Mathf.PerlinNoise(0.0f, scaledSpeed) * _magnitude) - (_magnitude * 0.5f))
			);
			elapsedTime += Time.deltaTime;
			yield return null;
		}

		_transform.localPosition = originalPosition;
		if(onShakeEnds != null) onShakeEnds();
	}

	/// <summary>Shakes Transform's rotation.</summary>
	/// <param name="_monoBehaviour">Requester MonoBehaviour.</param>
	/// <param name="_duration">Shake's Duration.</param>
	/// <param name="_speed">Sake's Speed.</param>
	/// <param name="_magnitude">Shake's Magnitude.</param>
	/// <param name="onShakeEnds">Action invoked when the shaking ends.</param>
	public static IEnumerator ShakeRotation(this Transform _transform, float _duration, float _speed, float _magnitude, Action onShakeEnds = null)
	{
		Vector3 originalEulerRotation = _transform.localRotation.eulerAngles;
		float elapsedTime = 0.0f;
		float scaledSpeed = 0.0f;

		while((elapsedTime < (_duration + Mathf.Epsilon)) && (_transform != null))
		{
			scaledSpeed = (Time.time * _speed);

			_transform.localRotation = Quaternion.Euler(originalEulerRotation + new Vector3(
				((Mathf.PerlinNoise(scaledSpeed, 0.0f) * _magnitude) - (_magnitude * 0.5f)),
				((Mathf.PerlinNoise(0.0f, scaledSpeed) * _magnitude) - (_magnitude * 0.5f)),
				((Mathf.PerlinNoise(0.5f, (scaledSpeed * 0.5f)) * _magnitude) - (_magnitude * 0.5f))));
			elapsedTime += Time.deltaTime;
			yield return null;
		}

		_transform.localRotation = Quaternion.Euler(originalEulerRotation);
		if(onShakeEnds != null) onShakeEnds();
	}

	/// <summary>Does actions taking a normalized time t.</summary>
	/// <param name="_monoBehaviour">Requester MonoBehaviour.</param>
	/// <param name="_duration">Normalized Time's duration.</param>
	/// <param name="action">Action taking the normalized time t each frame.</param>
	/// <param name="onActionEnds">Optional callbakc invoked when the normalized time reaches 1.0f.</param>
	public static IEnumerator DoOnNormalizedTime(this MonoBehaviour _monoBehaviour, float _duration, Action<float> action, Action onActionEnds = null)
	{
		float inverseDuration = 1.0f / _duration;
		float t = 0.0f;

		while(t < (1.0f + Mathf.Epsilon))
		{
			action(t);
			t += (Time.deltaTime * inverseDuration);
			yield return null;
		}

		if(t != 1.0f) action(1.0f);
		if(onActionEnds != null) onActionEnds();
	}

	/// <summary>Changes multiuple Material colors simultaneously at a given duration.</summary>
	/// <param name="_material">Target Material.</param>
	/// <param name="_duration">Color change duration.</param>
	/// <param name="onChangeEnds">Optional callback to invoke when the color changing ends.</param>
	/// <param name="_colorTuples">Tuples that contains both the Material tag and its new destiny color.</param>
	public static IEnumerator ChangeColors(this Material _material, float _duration, Action onChangeEnds = null, params ValueVTuple<MaterialTag, Color>[] _colorTuples)
	{
		float t = 0.0f;
		float inverseDuration = 1.0f / _duration;
		Color[] colors = new Color[_colorTuples.Length];

		for(int i = 0; i < colors.Length; i++)
		{
			colors[i] = _material.GetColor(_colorTuples[i].Item1);
		}

		while(t < 1.0f)
		{
			for(int i = 0; i < colors.Length; i++)
			{
				ValueVTuple<MaterialTag, Color> tuple = _colorTuples[i];
				Color color = Color.Lerp(colors[i], tuple.Item2, t);

				_material.SetColor(tuple.Item1, color);
			}

			t += (Time.deltaTime * inverseDuration);
			yield return null;
		}

		for(int i = 0; i < colors.Length; i++)
		{
			ValueVTuple<MaterialTag, Color> tuple = _colorTuples[i];
			_material.SetColor(tuple.Item1, tuple.Item2);
		}

		if(onChangeEnds != null) onChangeEnds();
	}

	/// <summary>Color Oscillation's Routine with Sin function.</summary>
	/// <param name="a">Color A.</param>
	/// <param name="b">Color B.</param>
	/// <param name="_speed">Oscillation's Speed.</param>
	/// <param name="onColorChange">Callback called each frame when the color changes.</param>
	public static IEnumerator ColorOscillation(Color a, Color b, float _speed, Action<Color> onColorChange)
	{
		if(onColorChange == null) yield break;

		float t = 0.0f;
		float  time = 0.0f;

		while(true)
		{
			t = VMath.RemapValueToNormalizedRange(Mathf.Sin(time), -1.0f, 1.0f);
			onColorChange(Color.Lerp(a, b, t));
			time += (Time.unscaledDeltaTime * _speed);

			yield return null;
		}
	}

	/// <summary>Oscillates habilitation of Renderer.</summary>
	/// <param name="_renderer">Renderer's reference.</param>
	/// <param name="_duration">Oscillation duration.</param>
	/// <param name="_cycles">Oscillation Cycles.</param>
	/// <param name="onOscillationEnds">Callback optionally invoked when oscillation ends.</param>
	public static IEnumerator OscillateRendererActivation(this Renderer _renderer, float _duration, float _cycles, Action onOscillationEnds = null)
	{
		_cycles = Mathf.Floor(_cycles);

		float time = _duration / (_cycles * 2.0f);
		float inverseDuration = 1.0f / time;
		float n = 0.0f;
		float t = 0.0f;

		while(n < _cycles)
		{
			while(t < 1.0f)
			{
				t += (Time.deltaTime * inverseDuration);
				yield return null;
			}

			n++;
			t = 0.0f;
			_renderer.enabled = !_renderer.enabled;
		}

		_renderer.enabled = true;
	}

	/// <summary>Oscilates Material's Material Main Color between its original and a desired color, interpolating back and forth.</summary>
	/// <param name="_material">Material to apply the Colro oscillation effect.</param>
	/// <param name="_color">Desired Color.</param>
	/// <param name="_duration">Oscillation process's duration.</param>
	/// <param name="_cycles">Number of back and forth cycles during the oscillation.</param>
	/// <param name="_propertyTag">Property tag referrinf to the color ["_Color" as default].</param>
	/// <param name="onColorOscillation">Optional callback invoked when the effect ends.</param>
	public static IEnumerator OscillateMaterialColor(this Material _material, Color _color, float _duration, float _cycles, string _propertyTag = "_Color", Action onColorOscillationEnds = null)
	{
		FloatRange sinRange = new FloatRange(-1.0f, 1.0f);
		int propertyID = Shader.PropertyToID(_propertyTag);
		Color originalColor = _material.GetColor(propertyID);
		Color newColor = new Color(0f, 0f, 0f, 0f);
		float inverseDuration = 1.0f / _duration;
		float t = 0.0f;
		float x = (360f * _cycles * Mathf.Deg2Rad);

		while(t < (1.0f + Mathf.Epsilon))
		{
			newColor = Color.Lerp(originalColor, _color, VMath.RemapValueToNormalizedRange(Mathf.Sin(t * x), sinRange));
			_material.SetColor(propertyID, newColor);
			t += (Time.deltaTime * inverseDuration);
			yield return null;
		}

		_material.SetColor(propertyID, originalColor);
		if(onColorOscillationEnds != null) onColorOscillationEnds();
	}

	/// <summary>Oscilates Renderer's Material Main Color between its original and a desired color, interpolating back and forth.</summary>
	/// <param name="_renderer">Renderer to apply the Colro oscillation effect.</param>
	/// <param name="_color">Desired Color.</param>
	/// <param name="_duration">Oscillation process's duration.</param>
	/// <param name="_cycles">Number of back and forth cycles during the oscillation.</param>
	/// <param name="_propertyTag">Property tag referrinf to the color ["_Color" as default].</param>
	/// <param name="onColorOscillation">Optional callback invoked when the effect ends.</param>
	public static IEnumerator OscillateRendererMainColor(this Renderer _renderer, Color _color, float _duration, float _cycles, string _propertyTag = "_Color", Action onColorOscillationEnds = null)
	{
		FloatRange sinRange = new FloatRange(-1.0f, 1.0f);
		int propertyID = Shader.PropertyToID(_propertyTag);
		Color originalColor = _renderer.material.GetColor(propertyID);
		Color newColor = new Color(0f, 0f, 0f, 0f);
		float inverseDuration = 1.0f / _duration;
		float t = 0.0f;
		float x = (360f * _cycles * Mathf.Deg2Rad);

		while(t < (1.0f + Mathf.Epsilon))
		{
			newColor = Color.Lerp(originalColor, _color, VMath.RemapValueToNormalizedRange(Mathf.Sin(t * x), sinRange));
			_renderer.material.SetColor(propertyID, newColor);
			t += (Time.deltaTime * inverseDuration);
			yield return null;
		}

		_renderer.material.SetColor(propertyID, originalColor);
		if(onColorOscillationEnds != null) onColorOscillationEnds();
	}

	/// <summary>Sets weight of Blend Shape of SkinnedMeshRenderer of a given index.</summary>
	/// <param name="_renderer">Renderer to set blend shape weight to.</param>
	/// <param name="_index">Index of Blend Shape to modify.</param>
	/// <param name="_weight">Desired weight value.</param>
	/// <param name="_duration">Duration of the blend shape setting.</param>
	/// <param name="onSetEnds">Optional callback to invoke when the setting is done.</param>
	public static IEnumerator SetBlendShapeWeight(this SkinnedMeshRenderer _renderer, int _index, float _weight, float _duration, Action onSetEnds = null)
	{
		/// Clamp both the index and the weight for good measure:
		_index = Mathf.Clamp(_index, 0, _renderer.sharedMesh.blendShapeCount);
		_weight = Mathf.Clamp(_weight, 0.0f, 100.0f);

		float t = 0.0f;
		float inverseDuration = 1.0f / _duration;
		float originalWeight = _renderer.GetBlendShapeWeight(_index);

		if(_duration > 0.0f) while(t < 1.0f)
		{
			_renderer.SetBlendShapeWeight(_index, Mathf.Lerp(originalWeight, _weight, t));
			t += (Time.deltaTime * inverseDuration);
			yield return null;
		}
		else yield return null;

		_renderer.SetBlendShapeWeight(_index, _weight);
		if(onSetEnds != null) onSetEnds();
	}

	/// <summary>Begins an Input Mashing Sequence.</summary>
	/// <param name="key">KeyCode to press during the sequence.</param>
	/// <param name="acceleration">Acceleration rate when the key is pressed in a frame (dividedd by the frame rate).</param>
	/// <param name="decceleration">Decceleration rate when the key in not pressed in a frame (divided by the frame rate).</param>
	/// <param name="minLimit">Minimum tolerance value.</param>
	/// <param name="maxLimit">Max limit where the sequence is considered a success.</param>
	/// <param name="onFailed">Optional Callback invoked when the sequence has failed [when the value passes the minimum limit].</param>
	/// <param name="onSucceeded">Optional Callback invoked when the sequence has been successfully done.</param>
	public static IEnumerator<float> InputMashingSequence(KeyCode key, float acceleration, float decceleration, float minLimit, float maxLimit, Action onFailed = null, Action onSucceeded = null)
	{
		float current = 0.0f;
		float progress = 0.0f;
		bool keyPressed = false;
		bool keyPressedLastFrame = false;

		while(current > minLimit && current < maxLimit)
		{
			keyPressed = Input.GetKeyDown(key);

			if(!keyPressedLastFrame)
			{
				current += (keyPressed ? acceleration : -decceleration) * Time.deltaTime;
				progress = Mathf.Clamp(VMath.RemapValueToNormalizedRange(current, minLimit, maxLimit), 0.0f, 1.0f);
				keyPressedLastFrame = keyPressed;
			}
			else keyPressedLastFrame = false;

			yield return progress;
		}

		if(current <= minLimit && onFailed != null)
		{
			progress = 0.0f;
			onFailed();

		} else if(current >= maxLimit && onSucceeded != null)
		{
			progress = 1.0f;
			onSucceeded();
		}
	}

	/// <summary>Begins an Input Mashing Sequence [With InputController's API].</summary>
	/// <param name="inputID">KeyCode to press during the sequence.</param>
	/// <param name="acceleration">Acceleration rate when the Input's ID is pressed in a frame (dividedd by the frame rate).</param>
	/// <param name="decceleration">Decceleration rate when the Input's ID in not pressed in a frame (divided by the frame rate).</param>
	/// <param name="minLimit">Minimum tolerance value.</param>
	/// <param name="maxLimit">Max limit where the sequence is considered a success.</param>
	/// <param name="onFailed">Optional Callback invoked when the sequence has failed [when the value passes the minimum limit].</param>
	/// <param name="onSucceeded">Optional Callback invoked when the sequence has been successfully done.</param>
	public static IEnumerator<float> InputMashingSequence(int inputID, float acceleration, float decceleration, float minLimit, float maxLimit, Action onFailed = null, Action onSucceeded = null)
	{
		if(InputController.Instance == null) yield break;

		float current = 0.0f;
		float progress = 0.0f;
		bool inputEntered = false;
		bool inputEnteredLastFrame = false;

		while(current > minLimit && current < maxLimit)
		{
			inputEntered = InputController.InputBegin(inputID);

			if(!inputEnteredLastFrame)
			{
				current += (inputEntered ? acceleration : -decceleration) * Time.deltaTime;
				progress = Mathf.Clamp(VMath.RemapValueToNormalizedRange(current, minLimit, maxLimit), 0.0f, 1.0f);
				inputEnteredLastFrame = inputEntered;
			}
			else inputEnteredLastFrame = false;

			yield return progress;
		}

		if(current <= minLimit && onFailed != null)
		{
			progress = 0.0f;
			onFailed();

		} else if(current >= maxLimit && onSucceeded != null)
		{
			progress = 1.0f;
			onSucceeded();
		}
	}

	/// <summary>Runs multiple IEnumerators.</summary>
	/// <param name="onFinished">Callback optionally invoked after all the iterations are finished.</param>
	/// <param name="_iterators">Set of Iterators.</param>
	public static IEnumerator RunMultipleIterators(Action onFinished = null, params IEnumerator[] _iterators)
	{
		if(_iterators == null) yield break;

		int interatorsLength = _iterators.Length;
		HashSet<int> finishedIterators = new HashSet<int>();

		while(finishedIterators.Count < interatorsLength)
		{
			for(int i = 0; i < interatorsLength; i++)
			{
				if(finishedIterators.Contains(i)) continue;

				if(!_iterators[i].MoveNext()) finishedIterators.Add(i);
			}

			yield return null;
		}

		if(onFinished != null) onFinished();
	}

	/// <summary>Plays Animator's State and waits for that state to end.</summary>
	/// <param name="_animator">Animator's Reference.</param>
	/// <param name="_animationHash">State's Hash.</param>
	/// <param name="_layer">AnimatorController's Layer.</param>
	/// <param name="_normalizedTime">Normalized Time Offset [where does the Animation start].</param>
	/// <param name="_additionalWait">AdditionalWait [0.0f by default].</param>
	/// <param name="onWaitEnds">Callback invoked when the wait ends [null by default].</param>
	public static IEnumerator PlayAndWait(this Animator _animator, int _animationHash, int _layer, float _normalizedTime = Mathf.NegativeInfinity, float _additionalWait = 0.0f, Action onWaitEnds = null)
	{
		_animator.Play(_animationHash, _layer, _normalizedTime);

		yield return null;

		AnimatorStateInfo state = _animator.GetCurrentAnimatorStateInfo(_layer);
		SecondsDelayWait wait = new SecondsDelayWait(state.length + _additionalWait);

		while(wait.MoveNext()) yield return null;

		if(onWaitEnds != null) onWaitEnds();
	}

	/// <summary>Cross-Fades towards Animation and waits till that Cross-Fade is finished.</summary>
	/// <param name="_animator">Animator's Reference.</param>
	/// <param name="_animationHash">AnimationState's Hash.</param>
	/// <param name="_fadeDuration">Cross-Fade's Duration.</param>
	/// <param name="_layerIndex">Layer's Index [0 by default].</param>
	/// <param name="_normalizedTime">Normalized Time Offset [where does the Animation start].</param>
	/// <param name="onAnimationEnds">Callback invoked when the animation ends.</param>
	public static IEnumerator WaitForCrossFade(this Animator _animator, int _animationHash, float _fadeDuration = 0.3f, int _layerIndex = -1, float _normalizedTime = Mathf.NegativeInfinity, Action onCrossFadeEnds = null)
	{
		_animator.CrossFade(_animationHash, _fadeDuration,_layerIndex, _normalizedTime);

		yield return null;

		/*
			When it cross-fades, it first begins from the actual state.
			We need to wait the actual states's duration * fadeDuration.
			Then after that wait, we can finally wait for the next animation.
		*/

		AnimatorStateInfo info = _animator.GetCurrentAnimatorStateInfo(_layerIndex);
		AnimatorTransitionInfo transitionInfo = _animator.GetAnimatorTransitionInfo(_layerIndex);

		SecondsDelayWait wait = new SecondsDelayWait(transitionInfo.duration * info.length);

		while(wait.MoveNext()) yield return null;

		if(onCrossFadeEnds != null) onCrossFadeEnds();
	}

	/// <summary>CrossFades LayerWeights on amount of time.</summary>
	/// <param name="_animator">Animator's Component.</param>
	/// <param name="_duration">Cross-Fade's Duration [in seconds].</param>
	/// <param name="onCrossFadeEnds">Callback invoked when the crossfade ends [null by default].</param>
	/// <param name="_layerWeightTupples">Tupples containing a layer with its respective desired new weight.</param>
	public static IEnumerator CrossFadeLayerWeights(this Animator _animator, float _duration, Action onCrossFadeEnds = null, params ValueVTuple<int, float>[] _layerWeightTuples)
	{
		if(_layerWeightTuples == null) yield break;

		int length = _layerWeightTuples.Length;
		float[] originalWeights = new float[length];
		ValueVTuple<int, float> tuple = default(ValueVTuple<int, float>);
		float t = 0.0f;
		float iD = 1.0f / _duration;

		for(int i = 0; i < length; i++)
		{
			originalWeights[i] = _animator.GetLayerWeight(_layerWeightTuples[i].Item1);
		}

		while(t < 1.0f)
		{
			for(int i = 0; i < length; i++)
			{
				tuple = _layerWeightTuples[i];
				_animator.SetLayerWeight(tuple.Item1, Mathf.Lerp(originalWeights[i], tuple.Item2, t));
				t += (Time.deltaTime * iD);
			}

			yield return null;
		}

		for(int i = 0; i < length; i++)
		{
			tuple = _layerWeightTuples[i];
			_animator.SetLayerWeight(tuple.Item1, tuple.Item2);
		}

		if(onCrossFadeEnds != null) onCrossFadeEnds();
	}

	/// <summary>Sets layer on given amount of time.</summary>
	/// <param name="_animator">Animator's Component.</param>
	/// <param name="_layer">Layer to set.</param>
	/// <param name="_weight">New weight for layer. Internally clamped.</param>
	/// <param name="_duration">How much it lasts the weight to be setted.</param>
	/// <param name="onLayerSet">Optional callback invoked then the layer finished being set.</param>
	public static IEnumerator SetLayerWeight(this Animator _animator, int _layer, float _weight, float _duration, Action onLayerSet = null)
	{
		float w = _animator.GetLayerWeight(_layer);
		float t = 0.0f;
		float iD = 1.0f / _duration;

		_weight = Mathf.Clamp(_weight, 0.0f, 1.0f);

		while(t < 1.0f)
		{
			_animator.SetLayerWeight(_layer, Mathf.Lerp(w, _weight, t));
			t += (Time.deltaTime * iD);
			yield return null;
		}

		_animator.SetLayerWeight(_layer, _weight);

		if(onLayerSet != null) onLayerSet();
	}

	/// <summary>Cross-Fades towards Animation and waits till that next animation is finished.</summary>
	/// <param name="_animator">Animator's Reference.</param>
	/// <param name="_animationHash">AnimationState's Hash.</param>
	/// <param name="_fadeDuration">Cross-Fade's Duration.</param>
	/// <param name="_layerIndex">Layer's Index [0 by default].</param>
	/// <param name="_normalizedTime">Normalized Time Offset [where does the Animation start].</param>
	/// <param name="_additionalWait">Optional Additional Wait [0.0f by default].</param>
	/// <param name="onAnimationEnds">Callback invoked when the animation ends.</param>
	public static IEnumerator CrossFadeAndWait(this Animator _animator, int _animationHash, float _fadeDuration = 0.3f, int _layerIndex = -1, float _normalizedTime = Mathf.NegativeInfinity, float _additionalWait = 0.0f, Action onAnimationEnds = null)
	{
		_animator.CrossFade(_animationHash, _fadeDuration,_layerIndex, _normalizedTime);

		yield return null;

		/*
			When it cross-fades, it first begins from the actual state.
			We need to wait the actual states's duration * fadeDuration.
			Then after that wait, we can finally wait for the next animation.
		*/

		AnimatorStateInfo info = _animator.GetCurrentAnimatorStateInfo(_layerIndex);
		AnimatorTransitionInfo transitionInfo = _animator.GetAnimatorTransitionInfo(_layerIndex);

		SecondsDelayWait wait = new SecondsDelayWait(transitionInfo.duration * info.length);

		while(wait.MoveNext()) yield return null;

		info = _animator.GetCurrentAnimatorStateInfo(_layerIndex);
		wait.ChangeDurationAndReset(info.length);


		while(wait.MoveNext()) yield return null;

		if(onAnimationEnds != null) onAnimationEnds();
	}

	/// \TODO Well....do it...
	/* Sources:
		- https://answers.unity.com/questions/628200/get-length-of-animator-statetransition.html
		- https://docs.unity3d.com/ScriptReference/Animator.GetCurrentAnimatorClipInfo.html
		- https://docs.unity3d.com/ScriptReference/AnimatorClipInfo.html
	*/
	
	public static IEnumerator WaitForAnimatorState(this Animator _animator, int _layerIndex = -1, float _additionalWait = 0.0f, Action onAnimatorStateEnds = null)
	{
		/// Wait one frame after changing the Animator's parameters.
		yield return null;

		AnimatorStateInfo info = _animator.GetCurrentAnimatorStateInfo(_layerIndex);
		SecondsDelayWait wait = new SecondsDelayWait(info.length);

		while(wait.MoveNext()) yield return null;

		if(onAnimatorStateEnds != null) onAnimatorStateEnds();
	}

	/// <summary>Plays Animation and waits until it ends.</summary>
	/// <param name="_animation">Animation Component.</param>
	/// <param name="_clip">AnimationClip to play.</param>
	/// <param name="_mode">PlayMode [PlayMode.StopSameLayer bu default].</param>
	/// <param name="_additionalWait">Additional Wait [0.0f by default].</param>
	/// <param name="onAnimationEnds">Callback invoked when animation ends.</param>
	public static IEnumerator PlayAnimationAndWait(this Animation _animation, AnimationClip _clip, PlayMode _mode = PlayMode.StopSameLayer, float _additionalWait = 0.0f, Action onAnimationEnds = null)
	{
		_animation.Play(_clip, _mode);

		AnimationState animationState = _animation.GetAnimationState(_clip);
		SecondsDelayWait wait = new SecondsDelayWait(animationState.clip.length + _additionalWait);

		while(wait.MoveNext()) yield return null;

		if(onAnimationEnds != null) onAnimationEnds();
	}

	/// <summary>Plays Animation and waits until it ends.</summary>
	/// <param name="_animation">Animation Component.</param>
	/// <param name="_clip">AnimationClip to play.</param>
	/// <param name="_fadeDuration">fade's Duration.</param>
	/// <param name="_mode">PlayMode [PlayMode.StopSameLayer bu default].</param>
	/// <param name="_additionalWait">Additional Wait [0.0f by default].</param>
	/// <param name="onAnimationEnds">Callback invoked when animation ends.</param>
	public static IEnumerator CrossFadeAndWait(this Animation _animation, AnimationClip _clip, float _fadeDuration = 0.3f, PlayMode _mode = PlayMode.StopSameLayer, float _additionalWait = 0.0f, Action onAnimationEnds = null)
	{
		_animation.CrossFade(_clip, _fadeDuration,_mode);

		AnimationState animationState = _animation.GetAnimationState(_clip);
		SecondsDelayWait wait = new SecondsDelayWait(animationState.clip.length + _additionalWait);

		while(wait.MoveNext()) yield return null;

		if(onAnimationEnds != null) onAnimationEnds();
	}

	/// <summary>Waits for Animation to end.</summary>
	/// <param name="_animation">Animation's Component.</param>
	/// <param name="_clip">AnimationClip that it is expected to end.</param>
	/// <param name="onAnimationEnds">Optional callback invoked when AnimationClip ends.</param>
	public static IEnumerator WaitForAnimationToEnd(this Animation _animation, AnimationClip _clip, Action onAnimationEnds = null)
	{
		AnimationState animationState = _animation.GetAnimationState(_clip);

		if(animationState == null || !animationState.enabled)
		{
			if(onAnimationEnds != null) onAnimationEnds();
			yield break;
		}

		while(animationState.normalizedTime < 1.0f)
		{
			yield return null;
		}

		if(onAnimationEnds != null) onAnimationEnds();
	}

	/// <summary>Sincronizes animation so certain frame of the animation matches a time.</summary>
	/// <param name="_animation">Animation Component.</param>
	/// <param name="_clip">AnimationClip to match with given time.</param>
	/// <param name="_frame">Desired animation's frame that must match the time.</param>
	/// <param name="_duration">Time's Duration.</param>
	public static IEnumerator PlayAndSincronizeAnimationWithTime(this Animation _animation, AnimationClip _clip, int _frame, float _duration, Action onAnimationEnds = null)
	{
		//_frame = Mathf.Max(_frame - 1, 0);
		_duration -= Time.deltaTime;
		AnimationState state = _animation.GetAnimationState(_clip);

		state.speed = 1.0f;

		SecondsDelayWait wait = new SecondsDelayWait(0.0f);
		float f = (float)_frame;
		float dt = 1.0f / _clip.frameRate;						/// Ideal Delta Time.
		float time = (f * dt) / state.speed; 					/// Time in frames it will take to reach the desired frame.
		float difference = _duration - time;

		if(difference > 0.0f)
		{ /// Duration lasts more than the desired frame.
			wait.ChangeDurationAndReset(difference - Time.deltaTime);
			while(wait.MoveNext()) yield return null;

		} else if(difference < 0.0f)
		{ /// Time towards desired frame lasts more than duration.
			state.speed = time / _duration; 					/// Make speed faster so it synchs with time.
		}

		_animation.Play(_clip);
		wait.ChangeDurationAndReset(state.length * state.speed);
		while(wait.MoveNext()) yield return null;

		if(onAnimationEnds != null) onAnimationEnds();
	}

	/// <summary>Tests Connection.</summary>
	/// <param name="onResponse">Callback invoked when there is a response.</param>
	public static IEnumerator TestInternetConnection(Action<bool> onResponse)
	{
		if(onResponse == null) yield break;

		string server = "https://google.com";
		bool result = false;

		using(var request = UnityWebRequest.Head(server))
		{
			request.timeout = 5;
			yield return request.SendWebRequest();
			result = !request.isNetworkError && !request.isHttpError && request.responseCode == 200;
		}

		onResponse(result);
	}

#endregion

}
}