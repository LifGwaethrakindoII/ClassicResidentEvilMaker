using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voidless.EditorNodes
{
[System.Serializable]
public abstract class BaseNode<T> : ScriptableClass<T>, IEventProcessor where T : BaseNode<T>
{
	public const string NO_TEXT = ""; 								/// <summary>No Text.</summary>
	public const string CONTEXT_MENU_REMOVE_NODE = "Remove Node"; 	/// <summary>Remove Node Context Menu option.</summary>
	public const string CONTEXT_MENU_COPY_NODE = "Copy Node"; 		/// <summary>Copy Node Context Menu option.</summary>
	public const string LABEL_FIELD_ID = "ID:";
	public const float DEFAULT_FIELD_HORIZONTAL_OFFSET = 8f; 		/// <summary>Node's default Field Horizontal Offset.</summary>
	public const float DEFAULT_FIELD_HEIGHT = 15f; 					/// <summary>Node's default Field height.</summary>

	[HideInInspector] private List<T> _childNodes; 					/// <summary>Node's Child Nodes.</summary>
	private Rect _rect; 											/// <summary>Node's Rect.</summary>
	private GUIStyle _nodeStyle; 									/// <summary>Node's GUI Style.</summary>
	private GUIStyle _defaultNodeStyle; 							/// <summary>Node's default GUI Style.</summary>
	private GUIStyle _selectedNodeStyle; 							/// <summary>Node's GUIS Style when selected.</summary>
	[SerializeField] private ConnectionPoint _inPoint; 				/// <summary>Node's Input Point.</summary>
	[SerializeField] private ConnectionPoint _outPoint;	 			/// <summary>Node's Output Poit.</summary>
	private bool _isDragged; 										/// <summary>Is the Node being dragged?.</summary>
	private bool _isSelected; 										/// <summary>Is the Node being selected?.</summary>
	private float _originalHeight; 									/// <summary>Stores the default rect's height property for default height adjustment.</summary>
	private float _fieldHorizontalOffset; 							/// <summary>Node's Field horizontal offset on Layout.</summary>
	private float _fieldHeight; 									/// <summary>Node's Field height.</summary>
	private float _currentLayoutY; 									/// <summary>Reference to the current element's position on the Node's GUI box in the vertical Layout.</summary>
	private int _nodeID; 											/// <summary>Node's ID.</summary>
	private string _nodeTitle; 										/// <summary>Node's Title.</summary>
	private string _nodeName;

	/// <summary>Action called when the Node has to be removed.</summary>
	/// <param name="_node">Reference to this Node.</param>
	private Action<T> _onRemoveNode;

	/// <summary>Action called when the Node has to be copied.</summary>
	/// <param name="_node">Reference to this Node.</param>
	/// <param name="_mousePosition">Mouse's position when the copy request was made.</param>
	private Action<T, Vector2> _onCopyNode;

#region Getters/Setters:
	/// <summary>Gets and Sets childNodes property.</summary>
	public List<T> childNodes
	{
		get { return _childNodes; }
		set { _childNodes = value; }
	}

	/// <summary>Gets and Sets rect property.</summary>
	public Rect rect
	{
		get { return _rect; }
		set { _rect = value; }
	}

	/// <summary>Gets and Sets nodeStyle property.</summary>
	public GUIStyle nodeStyle
	{
		get { return _nodeStyle; }
		set { _nodeStyle = value; }
	}

	/// <summary>Gets and Sets defaultNodeStyle property.</summary>
	public GUIStyle defaultNodeStyle
	{
		get { return _defaultNodeStyle; }
		set { _defaultNodeStyle = value; }
	}

	/// <summary>Gets and Sets selectedNodeStyle property.</summary>
	public GUIStyle selectedNodeStyle
	{
		get { return _selectedNodeStyle; }
		set { _selectedNodeStyle = value; }
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

	/// <summary>Gets and Sets isDragged property.</summary>
	public bool isDragged
	{
		get { return _isDragged; }
		set { _isDragged = value; }
	}

	/// <summary>Gets and Sets isSelected property.</summary>
	public bool isSelected
	{
		get { return _isSelected; }
		set { _isSelected = value; }
	}

	/// <summary>Gets and Sets originalHeight property.</summary>
	public float originalHeight
	{
		get { return _originalHeight; }
		set { _originalHeight = value; }
	}

	/// <summary>Gets and Sets fieldHorizontalOffset property.</summary>
	public float fieldHorizontalOffset
	{
		get { return _fieldHorizontalOffset; }
		set { _fieldHorizontalOffset = value; }
	}

	/// <summary>Gets and Sets fieldHeight property.</summary>
	public float fieldHeight
	{
		get { return _fieldHeight; }
		set { _fieldHeight = value; }
	}

	/// <summary>Gets and Sets currentLayoutY property.</summary>
	public float currentLayoutY
	{
		get { return _currentLayoutY; }
		set { _currentLayoutY = value; }
	}

	/// <summary>Gets and Sets nodeID property.</summary>
	public int nodeID
	{
		get { return _nodeID; }
		set { _nodeID = value; }
	}

	/// <summary>Gets and Sets nodeTitle property.</summary>
	public string nodeTitle
	{
		get
		{
			if(_nodeTitle == string.Empty)
			{
				_nodeTitle = typeof(T).Name;
			}
			return _nodeTitle;
		}
	}

	/// <summary>Gets and Sets nodeName property.</summary>
	public abstract string nodeName { get; }

	/// <summary>Gets and Sets OnRemoveNode property.</summary>
	public Action<T> OnRemoveNode
	{
		get { return _onRemoveNode; }
		set { _onRemoveNode = value; }
	}

	/// <summary>Gets and Sets OnCopyNode property.</summary>
	public Action<T, Vector2> OnCopyNode
	{
		get { return _onCopyNode; }
		set { _onCopyNode = value; }
	}
#endregion

	void OnEnable()
	{
		hideFlags = HideFlags.HideAndDontSave;
	}

	/// <summary>Default BaseNode constructor.</summary>
	public BaseNode()
	{
		//...
	}

	/// <summary>BaseNode's constructor.</summary>
	/// <param name="_position">BaseNode's Starting Position.</param>
	/// <param name="_width">BaseNode's rect width.</param>
	/// <param name="_height">BaseNode's rect height.</param>
	/// <param name="_fieldHorizontalOffset">BaseNode's Field horizontal offset.</param>
    /// <param name="_fieldHeight">BaseNode's Field height.</param>
	/// <param name="_nodeStyle">BaseNode's GUIStyle.</param>
    /// <param name="_selectedBaseNodeStyle">Selected GUIStyle.</param>
    /// <param name="onRemoveBaseNode">Action called when the BaseNode has to be removed.</param>
    /// <param name="onCopyBaseNode">Action called when the BaseNode has to be copied.</param>
	public BaseNode(Vector2 _position, float _width, float _height, float _fieldHorizontalOffset, float _fieldHeight, GUIStyle _nodeStyle, GUIStyle _selectedNodeStyle, Action<T> onRemoveNode, Action<T, Vector2> onCopyNode)
	{
		rect = new Rect(_position.x, _position.y, _width, _height);
		fieldHorizontalOffset = _fieldHorizontalOffset;
		fieldHeight = _fieldHeight;
		originalHeight = _height;
		nodeStyle = _nodeStyle;
		defaultNodeStyle = _nodeStyle;
		selectedNodeStyle = _selectedNodeStyle;
		OnRemoveNode = onRemoveNode;
        OnCopyNode = onCopyNode;
	}

	/// <summary>BaseNode ScriptableObject's constructor.</summary>
	/// <param name="_position">BaseNode's Starting Position.</param>
	/// <param name="_width">BaseNode's rect width.</param>
	/// <param name="_height">BaseNode's rect height.</param>
	/// <param name="_fieldHorizontalOffset">BaseNode's Field horizontal offset.</param>
    /// <param name="_fieldHeight">BaseNode's Field height.</param>
	/// <param name="_nodeStyle">BaseNode's GUIStyle.</param>
    /// <param name="_selectedBaseNodeStyle">Selected GUIStyle.</param>
    /// <param name="onRemoveBaseNode">Action called when the BaseNode has to be removed.</param>
    /// <param name="onCopyBaseNode">Action called when the BaseNode has to be copied.</param>
	/// <returns>Instance of this ScriptableClass.</returns>
	public static T NewInstance(Vector2 _position, float _width, float _height, float _fieldHorizontalOffset, float _fieldHeight, GUIStyle _nodeStyle, GUIStyle _selectedNodeStyle, Action<T> onRemoveNode, Action<T, Vector2> onCopyNode)
	{
		T newNode = CreateInstance<T>();

		newNode.rect = new Rect(_position.x, _position.y, _width, _height);
		newNode.fieldHorizontalOffset = _fieldHorizontalOffset;
		newNode.fieldHeight = _fieldHeight;
		newNode.originalHeight = _height;
		newNode.nodeStyle = _nodeStyle;
		newNode.defaultNodeStyle = _nodeStyle;
		newNode.selectedNodeStyle = _selectedNodeStyle;
		newNode.OnRemoveNode = onRemoveNode;
        newNode.OnCopyNode = onCopyNode;

        return newNode;
	}

#region Events:
	/// <summary>Process EventSytem events.</summary>
	/// <param name="_event">Current Event.</param>
	/// <returns>State of the Event Processing.</returns>
	public virtual bool ProcessEvents(Event _event)
	{
		switch (_event.type)
        {
            case EventType.MouseDown:
                if (_event.button == 0)
                {
                    if (rect.Contains(_event.mousePosition))
                    {
                        isDragged = true;
                        GUI.changed = true;
                        isSelected = true;
                        nodeStyle = selectedNodeStyle;
                    }
                    else
                    {
                        GUI.changed = true;
                        isSelected = false;
                        nodeStyle = defaultNodeStyle;
                    }
                }

                if (_event.button == 1 && isSelected && rect.Contains(_event.mousePosition))
                {
                    ProcessContextMenu(_event.mousePosition);
                    _event.Use();
                }
            break;

            case EventType.MouseUp:
                isDragged = false;
            break;

            case EventType.MouseDrag:
                if (_event.button == 0 && isDragged)
                {
                    Drag(_event.delta);
                    _event.Use();
                    return true;
                }
            break;
        }

        return (_event != null);
	}

	/// <summary>Draws Node.</summary>
	public virtual void Draw()
	{
		if(inPoint != null) inPoint.Draw();
        if(outPoint != null) outPoint.Draw();
		GUI.Box(rect, NO_TEXT, nodeStyle);
		DrawNodeFields();
	}

	/// <summary>Drags Node.</summary>
	/// <param name="_delta">Delta position between the last position and the actual position that represents the drag movement vector.</param>
	public virtual void Drag(Vector2 _delta)
	{
		Vector2 movePosition = rect.position + _delta;
		Rect updatedRect = new Rect(movePosition.x, movePosition.y, rect.width, rect.height);
		rect = updatedRect;
	}
#endregion

#region GUILayoutMethods:
	/// <summary>Gets Layout Y's position updated each time it is called.</summary>
	/// <returns>Updated value of the Layout's Y for the new GUI element.</returns>
	public virtual float DisplaceYLayout()
	{
		return (rect.y + (currentLayoutY += fieldHeight));
	}

	/// <summary>Checks Node's rect size with current content to update height.</summary>
	public virtual void CheckForLayoutUpdate()
	{
		DisplaceYLayout();
		float displacedLayoutY = DisplaceYLayout();

		if((rect.y + rect.height) <= displacedLayoutY)
		{
			Rect newRect = new Rect(rect.x, rect.y, rect.width, Mathf.Abs((rect.y) - displacedLayoutY));
			rect = newRect;
		}
		else
		{
			Rect originalRect = new Rect(rect.x, rect.y, rect.width, originalHeight);
			rect = originalRect;
		}
	}

	/// <summary>Generates a new Layout position according to the current Layout status.</summary>
	/// <returns>New Layout position [Rect].</returns>
	protected virtual Rect GetNewLayoutPosition()
	{
		return new Rect
		(
			(rect.x + fieldHorizontalOffset),
			DisplaceYLayout(),
			(rect.width - (fieldHorizontalOffset * 2)),
			fieldHeight

		);
	}

	/// <summary>Generates a new Layout indented position according to the current Layout status.</summary>
	/// <returns>New Layout position indented [Rect].</returns>
	protected virtual Rect GetNewIndentedLayoutPosition()
	{
		Rect indentedRect = GetNewLayoutPosition();
		indentedRect.x += fieldHorizontalOffset;
		indentedRect.width -= fieldHorizontalOffset;

		return indentedRect;
	}
#endregion

#region AbstractMethods:
	/// <summary>Shows Tree Node creation Menu Context.</summary>
	/// <param name="_mousePosition">Position of the mouse when the right click took place.</param>
	public abstract void ProcessContextMenu(Vector2 _mousePosition);

	/// <summary>Removes thid Node from Window Editor's Node List.</summary>
	public abstract void OnRemoveNodeSelected();

	/// <summary>Clones Node into new Node.</summary>
	/// <returns>Clone of this Node.</returns>
	public abstract T CloneNode();

	/// <summary>Draws Node's Fields.</summary>
	public abstract void DrawNodeFields();
#endregion

	public virtual bool HasChildNode(T _node)
	{
		if(childNodes == null) childNodes = new List<T>();
		return childNodes.Contains(_node);
	}

	public virtual void AddChildNode(T _node)
	{
		if(childNodes == null) childNodes = new List<T>();
		childNodes.Add(_node);
	}

	public virtual void RemoveChildNode(T _node)
	{
		if(childNodes != null) childNodes.Remove(_node);
	}
}
}