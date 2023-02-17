using System;
using UnityEditor;
using UnityEngine;

namespace Voidless.EditorNodes
{
[System.Serializable]
public class Connection : ScriptableObject
{
	private Color _beizerLineColor; 					/// <summary>Beizer Line's color.</summary>
	private float _beizerLineTangent; 					/// <summary>Beizer line's tangent, determines the smoothness of the line between Input and Output points.</summary>
	private float _beizerLineWidth; 					/// <summary>Beizer line's width.</summary>
	private float _handlesButtonSize; 					/// <summary>Handles's button size.</summary>
	private float _handlesButtonPickSize; 				/// <summary>Handles's button pick size.</summary>
	[HideInInspector] private ConnectionPoint _inPoint; 					/// <summary>Connection's Input Point.</summary>
	[HideInInspector] private ConnectionPoint _outPoint; 					/// <summary>Connection's Output Point.</summary>
	private PointAllignmentTypes _pointAllignmentType; 	/// <summary>How the connection draws the Beizer relative to the Node's allingment type.</summary>

	/// <summary>Action called when this Connection has to be removed.</summary>
	/// <param name="_connection">Connection [this] to be removed.</param>
	[HideInInspector] private Action<Connection> _OnClickRemoveConnection;

#region Getters/Setters:
	/// <summary>Gets and Sets beizerLineColor property.</summary>
	public Color beizerLineColor
	{
		get { return _beizerLineColor; }
		set { _beizerLineColor = value; }
	}

	/// <summary>Gets and Sets beizerLineTangent property.</summary>
	public float beizerLineTangent
	{
		get { return _beizerLineTangent; }
		set { _beizerLineTangent = value; }
	}

	/// <summary>Gets and Sets beizerLineWidth property.</summary>
	public float beizerLineWidth
	{
		get { return _beizerLineWidth; }
		set { _beizerLineWidth = value; }
	}

	/// <summary>Gets and Sets handlesButtonSize property.</summary>
	public float handlesButtonSize
	{
		get { return _handlesButtonSize; }
		set { _handlesButtonSize = value; }
	}

	/// <summary>Gets and Sets handlesButtonPickSize property.</summary>
	public float handlesButtonPickSize
	{
		get { return _handlesButtonPickSize; }
		set { _handlesButtonPickSize = value; }
	}

	/// <summary>Gets and Sets inPoint property.</summary>
	public ConnectionPoint inPoint
	{
		get { return _inPoint; }
		set { _inPoint = value; }
	}

	/// <summary>Gets and Sets outPoint property.</summary>
	public ConnectionPoint outPoint
	{
		get { return _outPoint; }
		set { _outPoint = value; }
	}

	/// <summary>Gets and Sets pointAllignmentType property.</summary>
	public PointAllignmentTypes pointAllignmentType
	{
		get { return _pointAllignmentType; }
		set { _pointAllignmentType = value; }
	}

	/// <summary>Gets and Sets OnClickRemoveConnection property.</summary>
	public Action<Connection> OnClickRemoveConnection
	{
		get { return _OnClickRemoveConnection; }
		set { _OnClickRemoveConnection = value; }
	}
#endregion

	void OnEnable()
	{
		hideFlags = HideFlags.HideAndDontSave;
	}

	/// <summary>Connection's constructor.</summary>
	/// <param name="_inPoint">Connection;s Input Point.</param>
	/// <param name="_outPoint">Connection's Output Point.</param>
	/// <param name="onClickRemoveConnection">Action called when this Connection has to be removed.</param>
	public Connection(ConnectionPoint _inPoint, ConnectionPoint _outPoint, Action<Connection> onClickRemoveConnection)
	{
		inPoint = _inPoint;
		outPoint = _outPoint;
		beizerLineColor = Color.white;
		OnClickRemoveConnection = onClickRemoveConnection;
	}

	/// <summary>Connection's constructor.</summary>
	/// <param name="_inPoint">Connection;s Input Point.</param>
	/// <param name="_outPoint">Connection's Output Point.</param>
	/// <param name="_pointAllignmentType">How the Beizer will be oriented.</param>
	/// <param name="_beizerLineColor">Color of the Beizer Line.</param>
	/// <param name="_beizerLineTangent">Beizer line's tangent.</param>
	/// <param name="_beizerLineWidth">Beizerline's width.</param>
	/// <param name="_handlesButtonSize">Handles's button size.</param>
	/// <param name="_handlesButtonPickSize">Handles's button pick size.</param>
	/// <param name="onClickRemoveConnection">Action called when this Connection has to be removed.</param>
	public Connection(ConnectionPoint _inPoint, ConnectionPoint _outPoint, PointAllignmentTypes _pointAllignmentType, Color _beizerLineColor, float _beizerLineTangent, float _beizerLineWidth, float _handlesButtonSize, float _handlesButtonPickSize,Action<Connection> onClickRemoveConnection)
	{
		inPoint = _inPoint;
		outPoint = _outPoint;
		pointAllignmentType = _pointAllignmentType;
		beizerLineColor = _beizerLineColor;
		beizerLineTangent = _beizerLineTangent;
		beizerLineWidth = _beizerLineWidth;
		handlesButtonSize = _handlesButtonSize;
		handlesButtonPickSize = _handlesButtonPickSize;
		OnClickRemoveConnection = onClickRemoveConnection;
	}

	/// <summary>Connection's ScriptableObject constructor.</summary>
	/// <param name="_inPoint">Connection;s Input Point.</param>
	/// <param name="_outPoint">Connection's Output Point.</param>
	/// <param name="_pointAllignmentType">How the Beizer will be oriented.</param>
	/// <param name="_beizerLineColor">Color of the Beizer Line.</param>
	/// <param name="_beizerLineTangent">Beizer line's tangent.</param>
	/// <param name="_beizerLineWidth">Beizerline's width.</param>
	/// <param name="_handlesButtonSize">Handles's button size.</param>
	/// <param name="_handlesButtonPickSize">Handles's button pick size.</param>
	/// <param name="onClickRemoveConnection">Action called when this Connection has to be removed.</param>
	public static Connection NewInstance(ConnectionPoint _inPoint, ConnectionPoint _outPoint, PointAllignmentTypes _pointAllignmentType, Color _beizerLineColor, float _beizerLineTangent, float _beizerLineWidth, float _handlesButtonSize, float _handlesButtonPickSize,Action<Connection> onClickRemoveConnection)
	{
		Connection newConnection = CreateInstance<Connection>();

		newConnection.inPoint = _inPoint;
		newConnection.outPoint = _outPoint;
		newConnection.pointAllignmentType = _pointAllignmentType;
		newConnection.beizerLineColor = _beizerLineColor;
		newConnection.beizerLineTangent = _beizerLineTangent;
		newConnection.beizerLineWidth = _beizerLineWidth;
		newConnection.handlesButtonSize = _handlesButtonSize;
		newConnection.handlesButtonPickSize = _handlesButtonPickSize;
		newConnection.OnClickRemoveConnection = onClickRemoveConnection;

		return newConnection;
	}

	/// <summary>Draws Connection between Input Point and Output Point.</summary>
	public virtual void Draw()
	{
		switch(pointAllignmentType)
		{
			case PointAllignmentTypes.Vertical:
				Handles.DrawBezier
				(
					inPoint.rect.center,
					outPoint.rect.center,
					(inPoint.rect.center + (Vector2.up * beizerLineTangent)),
					(outPoint.rect.center - (Vector2.up * beizerLineTangent)),
					beizerLineColor,
					null,
					beizerLineWidth
				);
			break;

			case PointAllignmentTypes.Horizontal:
				Handles.DrawBezier
				(
					inPoint.rect.center,
					outPoint.rect.center,
					(inPoint.rect.center + (Vector2.left * beizerLineTangent)),
					(outPoint.rect.center - (Vector2.left * beizerLineTangent)),
					beizerLineColor,
					null,
					beizerLineWidth
				);
			break;
		}
		

		if(Handles.Button( ((inPoint.rect.center + outPoint.rect.center) * 0.5f), Quaternion.identity, handlesButtonSize, handlesButtonPickSize, Handles.RectangleHandleCap ))
		{
			if(OnClickRemoveConnection != null) OnClickRemoveConnection(this);
		}
	}
}
}