using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public class HPBarGUI : BaseGUI<IVitality>
{
	[SerializeField] private RectTransform _healthGauge; 	/// <summary>Health's gauge.</summary>
	[SerializeField] private RectTransform _bloodGauge; 	/// <summary>Blood's gauge.</summary>
	[SerializeField] private float bloodDisplacementSpeed; 	/// <summary>Blood's displacement speed when the player takes damage.</summary>
	[SerializeField] private Axes3D _scaleAxes; 			/// <summary>Axes to scale the HP Bar.</summary>
	[SerializeField] private bool _followTarget; 			/// <summary>Follow target on each frame?.</summary>
	private Transform target; 								/// <summary>GUI's target to follow.</summary>
	private Vector3 _offset; 								/// <summary>Additional Offset from target's position.</summary>
	private Behavior displaceHealthBar;

	/// <summary>Gets and Sets healthGauge property.</summary>
	public RectTransform healthGauge
	{
		get { return _healthGauge; }
		set { _healthGauge = value; }
	}

	/// <summary>Gets and Sets bloodGauge property.</summary>
	public RectTransform bloodGauge
	{
		get { return _bloodGauge; }
		set { _bloodGauge = value; }
	}

	/// <summary>Gets and Sets offset property.</summary>
	public Vector3 offset
	{
		get { return _offset; }
		set { _offset = value; }
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
		//maxHP = Data.HP;
        //UpdateGUI(Data);

        if(Data is MonoBehaviour)
        {
            MonoBehaviour mono = Data as MonoBehaviour;
            target = mono.gameObject.transform;
        }

	}

	void OnDisable()
    {
    	if(displaceHealthBar != null)
    	{
    		displaceHealthBar.EndBehavior();
	        displaceHealthBar = null;
    	}  
    }

	void Update()
	{
		if(target != null) transform.position = (target.position + offset);
        transform.LookAt(Camera.main.transform);
	}

	void Awake()
	{
		enabled = followTarget;
	}

	/// <summary>Updates the GUI's Feedback to the User.</summary>
	/// <param name="_HPData">Vitaluty Data that the GUI will recieve.</param>
	public override void UpdateGUI(IVitality _HPData)
	{
		float normalizedScale = Mathf.Clamp((_HPData.HP / _HPData.maxHP), 0.0f, 1.0f);
		Vector3 newScale = new Vector3
		(
			scaleAxes.HasFlag(Axes3D.X) ? normalizedScale : 1.0f,
			scaleAxes.HasFlag(Axes3D.Y) ? normalizedScale : 1.0f,
			scaleAxes.HasFlag(Axes3D.Z) ? normalizedScale : 1.0f
		);

		if (healthGauge != null) healthGauge.localScale = newScale;

		if(displaceHealthBar != null) displaceHealthBar.EndBehavior();
		displaceHealthBar = new Behavior(this, DisplaceHealthBar(_HPData.HP));
	}

	/// <summary>Displaces Blood gauge to current Health's gauge on a displacement speed.</summary>
	private IEnumerator DisplaceHealthBar(float _health)
	{
		float normalizedTime = 0.0f;
		float normalizedSpeed = /*(healthGauge.localScale.x / 1f)*/1f;
		Vector3 originalBloodScale = bloodGauge.localScale;

		while(normalizedTime < 1.0f)
		{
			if (healthGauge != null) bloodGauge.localScale = Vector3.Lerp(originalBloodScale, healthGauge.localScale, normalizedTime);
			normalizedTime += ((Time.deltaTime / bloodDisplacementSpeed) * normalizedSpeed);
			yield return null;
		}
	}
}
}