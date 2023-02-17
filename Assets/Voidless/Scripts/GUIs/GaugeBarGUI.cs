using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Voidless
{
public class GaugeBarGUI : BaseGUI<IConstrainedValue<float>>
{
	[SerializeField] private RectTransform _mainGauge; 			/// <summary>Main's gauge.</summary>
	[SerializeField] private RectTransform _secondaryGauge; 	/// <summary>Secondary's gauge.</summary>
	[SerializeField] private float gaugeDisplacementSpeed; 		/// <summary>Secondary Gauge's displacement speed when the player takes damage.</summary>
	[SerializeField] private Axes3D _scaleAxes; 				/// <summary>Axes to scale the HP Bar.</summary>
	[SerializeField] private bool _followTarget; 				/// <summary>Follow target on each frame?.</summary>
	private Image _mainGaugeImage; 								/// <summary>Main Gauge's Image.</summary>
	private Image _secondaryGaugeImage; 						/// <summary>Secondary Gauge's Image.</summary>
	private Transform _target; 									/// <summary>GUI's target to follow.</summary>
	private float _normalizedTime; 								/// <summary>current's Normalized Time.</summary>
	private float _priorValue; 									/// <summary>Cached prior value.</summary>
	private Vector3 _offset; 									/// <summary>Additional Offset from target's position.</summary>
	private Behavior displaceMainGaugeBar;

	/// <summary>Gets and Sets mainGauge property.</summary>
	public RectTransform mainGauge
	{
		get { return _mainGauge; }
		set { _mainGauge = value; }
	}

	/// <summary>Gets and Sets secondaryGauge property.</summary>
	public RectTransform secondaryGauge
	{
		get { return _secondaryGauge; }
		set { _secondaryGauge = value; }
	}

	/// <summary>Gets and Sets mainGaugeImage property.</summary>
	public Image mainGaugeImage
	{
		get { return _mainGaugeImage; }
		set { _mainGaugeImage = value; }
	}

	/// <summary>Gets and Sets secondaryGaugeImage property.</summary>
	public Image secondaryGaugeImage
	{
		get { return _secondaryGaugeImage; }
		set { _secondaryGaugeImage = value; }
	}

	/// <summary>Gets and Sets offset property.</summary>
	public Vector3 offset
	{
		get { return _offset; }
		set { _offset = value; }
	}

	/// <summary>Gets and Sets normalizedTime property.</summary>
	public float normalizedTime
	{
		get { return _normalizedTime; }
		set { _normalizedTime = value; }
	}

	/// <summary>Gets and Sets priorValue property.</summary>
	public float priorValue
	{
		get { return _priorValue; }
		set { _priorValue = value; }
	}

	/// <summary>Gets and Sets target property.</summary>
	public Transform target
	{
		get { return _target; }
		set
		{
			_target = value;
			enabled = target != null;
		}
	}

	/// <summary>Gets and Sets scaleAxes property.</summary>
	public Axes3D scaleAxes
	{
		get { return _scaleAxes; }
		set { _scaleAxes = value; }
	}

	/// <summary>Gets and Sets followTarget property.</summary>
	public bool followTarget
	{
		get { return _followTarget; }
		set { enabled = _followTarget = value; }
	}

	/// <summary>Method invoked when GUI's Data is assigned.</summary>
	protected override void OnDataAssigned()
	{
        if(Data is MonoBehaviour)
        {
            MonoBehaviour mono = Data as MonoBehaviour;
            target = mono.gameObject.transform;
        }

	}

	void OnDisable()
    {
    	if(displaceMainGaugeBar != null)
    	{
    		VCoroutines.DispatchBehavior(ref displaceMainGaugeBar);
    	}  
    }

	void Update()
	{
		if(target != null) transform.position = (target.position + offset);
        transform.LookAt(Camera.main.transform);
	}

	void Awake()
	{
		mainGaugeImage = mainGauge.GetComponent<Image>();
		secondaryGaugeImage = secondaryGauge.GetComponent<Image>();
		enabled = followTarget;
	}

	/// <summary>Updates the GUI's Feedback to the User.</summary>
	/// <param name="_value">Vitaluty Data that the GUI will recieve.</param>
	public override void UpdateGUI(IConstrainedValue<float> _value)
	{
		float normalizedScale = _value.max <= 0.0f ? 1.0f : Mathf.Clamp((_value.current / _value.max), 0.0f, 1.0f);
		float difference = _value.current - priorValue;
		// If the gauge was reduced, reduce the main gauge, else the secondary one.
		RectTransform targetRectTransform = difference < 0.0f ? mainGauge : secondaryGauge;
		Vector3 newScale = new Vector3
		(
			scaleAxes.HasFlag(Axes3D.X) ? normalizedScale : 1.0f,
			scaleAxes.HasFlag(Axes3D.Y) ? normalizedScale : 1.0f,
			scaleAxes.HasFlag(Axes3D.Z) ? normalizedScale : 1.0f
		);

		targetRectTransform.localScale = newScale;
		priorValue = _value.current;

		if(displaceMainGaugeBar != null) displaceMainGaugeBar.EndBehavior();
		displaceMainGaugeBar = new Behavior(this, DisplaceMainGaugeBar((difference < 0.0f)));
	}

	/// <summary>Displaces Secondary gauge to current MainGauge's gauge on a displacement speed.</summary>
	/// <param name="_gaugeReduced">Was the gauge reduced? To determine whether to displace the main or secondary bar.</param>
	private IEnumerator DisplaceMainGaugeBar(bool _gaugeReduced)
	{
		float normalizedSpeed = /*(mainGauge.localScale.x / 1f)*/1f;
		RectTransform targetRectTransform = _gaugeReduced ? secondaryGauge : mainGauge;
		RectTransform destinyRectTransform = _gaugeReduced ? mainGauge : secondaryGauge;
		Vector3 originalGaugeScale = targetRectTransform.localScale;

		while(normalizedTime < (1.0f + Mathf.Epsilon))
		{
			targetRectTransform.localScale = Vector3.Lerp(originalGaugeScale, destinyRectTransform.localScale, normalizedTime);
			normalizedTime += ((Time.deltaTime / gaugeDisplacementSpeed) * normalizedSpeed);
			yield return null;
		}		

		normalizedTime = 0.0f;
	}
}
}