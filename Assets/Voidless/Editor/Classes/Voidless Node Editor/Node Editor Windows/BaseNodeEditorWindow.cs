using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voidless.EditorNodes
{
public enum PointTypes 														/// <summary>Point Types.</summary>
{
	Unassigned, 															/// <summary>Unassigned Point Type.</summary>
	In, 																	/// <summary>Input Point Type.</summary>
	Out 																	/// <summary>Output Point Type.</summary>
}

public enum PointAllignmentTypes 											/// <summary>Point Allignment Types.</summary>
{
	Unassigned, 															/// <summary>Unassigned Point Allignment Type.</summary>
	Horizontal, 															/// <summary>Horizontal Point Allignment Type.</summary>
	Vertical 																/// <summary>Vertical Point Allignment Type.</summary>
}

#pragma warning disable 693
public abstract class BaseNodeEditorWindow<N, D, A, T, I> : EditorWindow
where N : Node
where D : BaseNodeEditorData<N, A>
where A : BaseNodeEditorAttributes
where T : BaseNodeData<I>
{
#region Constants:
	public const string NO_TEXT = ""; 										                                                                /// <summary>No Text.</summary>
	public const string WINDOW_LOADING_MESSAGE = "Loading..."; 				                                                                /// <summary>Loading in background message.</summary>
	public const string WINDOW_ROOT_PATH = "Window/Node Editors/"; 			                                                                /// <summary>Node Editor's options location on the menu bar.</summary>
	public const string WINDOW_PANEL_DATA_FIELD = "Node Editor Data: ";		                                                                /// <summary>Node Editor Data Field ID.</summary>
	public const string WINDOW_PANEL_GAME_DATA_FIELD = "Game Node Data: ";
    public const string WINDOW_PANEL_LOAD_BUTTON = "Load"; 					                                                                /// <summary>Load Button's Text.</summary>
	public const string WINDOW_PANEL_SAVE_BUTTON = "Save"; 					                                                                /// <summary>Save Button's Text.</summary>
    public const string WINDOW_PANEL_DELETE_BUTTON = "Delete";                                                                              /// <summary>Delete Button's Text.</summary>
    public const string WINDOW_DELETE_PROMPT_TITLE = "Delete Data?";                                                                        /// <summary>Delete prompt title.</summary>
    public const string WINDOW_DELETE_PROMPT_MESSAGE = "Are you sure you want to delete your data? it will not be recovered.";               /// <summary>Delete prompt message.</summary>
    public const string WINDOW_NO_DATA_MESSAGE = "There is no Data assigned yet. Be sure to attach a Data Asset on this Editor Window.";    /// <summary>No Data Assigned message.</summary>
	public const string WINDOW_NO_DATA_TITLE = "There is no Data assigned";                                                                 /// <summary>No Data assigned title.</summary>
    public const string WINDOW_NO_ATTRIBUTES_MESSAGE = "Please verify that the Data has Attributes attached.";                              /// <summary>No Attributes message.</summary>
    public const string WINDOW_NO_ATTRIBUTES_TITLE = "Data has no reference to Attributes";                                                 /// <summary>No Attributes attached message.</summary>
    public const string WINDOW_REPEATED_CONNECTION_TITLE =  "Connection already existing!";
    public const string WINDOW_REPEATED_CONNECTION_MESSAGE_0 = " parent has already a connection with ";
    public const string WINDOW_REPEATED_CONNECTION_MESSAGE_1 = " child. This new Connection will be canceled.";
    public const string WINDOW_ACCEPT_ANSWER = "OK";                                                                                        /// <summary>Window's Accept answer.</summary>
    public const string WINDOW_CANCEL_ANSWER = "Cancel";                                                                                    /// <summary>Window's Cancel answer.</summary>
    public const string LOAD_SUCCESS_MESSAGE = "Success Loading Data";                                                                      /// <summary>Load Success message.</summary>
	public const string LOAD_ERROR_MESSAGE = "Failure Loading Data";                                                                        /// <summary>Load Failure message.</summary>
	public const string SAVE_SUCCESS_MESSAGE = "Success Saving Data";                                                                       /// <summary>Save Success message.</summary>
	public const string SAVE_ERROR_MESSAGE = "Error Saving Data";                                                                           /// <summary>Save Failure message.</summary>
	public const float DEFAULT_DATA_PANEL_HEIGHT = 100F;                                                                                    /// <summary>Default Data Panel's height.</summary>
	public const float DEFAULT_DATA_FIELD_HEIGHT = 15f;                                                                                     /// <summary>Default Data Panel's field height.</summary>
	public const float DEFAULT_LOAD_BUTTON_WIDTH = 120F;                                                                                    /// <summary>Data Panel's default load button's width.</summary>
	public const float DEFAULT_LOAD_BUTTON_HEIGHT = 75F;                                                                                    /// <summary>Data Panel's default load button's height.</summary>
	public const float DEFAULT_BUTTON_Y_OFFSET = 25f;                                                                                       /// <summary>Default Data Panel's button Y-Axis offset.</summary>
#endregion

	private List<N> _nodes; 								                                                                                /// <summary>Window's Nodes.</summary>
	private D _data; 										                                                                                /// <summary>Window's Data.</summary>
	private A _attributes; 								                                                                                    /// <summary>Window's Attributes.</summary>
	private T _gameData;
    private List<Connection> _connections; 				                                                                                    /// <summary>Window's Nodes Connections.</summary>
    private List<ConnectionPoint> _connectionPoints;
    private ConnectionPoint _selectedInPoint; 				                                                                                /// <summary>Window's current selected Input ConnectionPoint.</summary>
	private ConnectionPoint _selectedOutPoint; 			                                                                                    /// <summary>Window's current selected Output ConnectionPoint.</summary>
	private Vector2 _offset; 								                                                                                /// <summary>Offset calculated from the delta drag, determines Grid's displacement.</summary>
	private Vector2 _drag; 								                                                                                    /// <summary>Current delta drag.</summary>
	private bool _loading = false; 						                                                                                    /// <summary>Is the Node Editor's Window loading?.</summary>
	private string _editorWindowTitle; 									                                                                    /// <summary>Editor Window's Title.</summary>
    private string _selectedCommand;                                                                                                        /// <summary>Context Menu's current selected command.</summary>
    private GenericMenu _nodeContextMenu;                                                                                                   /// <summary>Event Processor's context menu.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets nodes property.</summary>
	public List<N> nodes
	{
		get { return _nodes; }
		set { _nodes = value; }
	}

	/// <summary>Gets and Sets data property.</summary>
	public D data
	{
		get { return _data; }
		set { _data = value; }
	}

	/// <summary>Gets and Sets attributes property.</summary>
	public A attributes
	{
		get { return _attributes; }
		set { _attributes = value; }
	}

    /// <summary>Gets and Sets gameData property.</summary>
    public T gameData
    {
        get { return _gameData; }
        set { _gameData = value; }
    }

	/// <summary>Gets and Sets connections property.</summary>
	public List<Connection> connections
	{
		get { return _connections; }
		set { _connections = value; }
	}

    /// <summary>Gets and Sets connectionPoints property.</summary>
    public List<ConnectionPoint> connectionPoints
    {
        get { return _connectionPoints; }
        set { _connectionPoints = value; }
    }

	/// <summary>Gets and Sets selectedInPoint property.</summary>
	public ConnectionPoint selectedInPoint
	{
		get { return _selectedInPoint; }
		set { _selectedInPoint = value; }
	}

	/// <summary>Gets and Sets selectedOutPoint property.</summary>
	public ConnectionPoint selectedOutPoint
	{
		get { return _selectedOutPoint; }
		set { _selectedOutPoint = value; }
	}

	/// <summary>Gets and Sets offset property.</summary>
	public Vector2 offset
	{
		get { return _offset; }
		set { _offset = value; }
	}

	/// <summary>Gets and Sets drag property.</summary>
	public Vector2 drag
	{
		get { return _drag; }
		set { _drag = value; }
	}

	/// <summary>Gets and Sets loading property.</summary>
	public bool loading
	{
		get { return _loading; }
		set { _loading = value; }
	}

	/// <summary>Gets editorWindowTitle property.</summary>
	public virtual string editorWindowTitle
	{
		get
		{
			if(_editorWindowTitle != null)
			{
				_editorWindowTitle = "Base Editor Window";
			}

			return _editorWindowTitle;
		}
	}

    /// <summary>Gets and Sets selectedCommand property.</summary>
    public string selectedCommand
    {
        get { return _selectedCommand; }
        set { _selectedCommand = value; }
    }

    /// <summary>Gets and Sets nodeContextMenu property.</summary>
    public GenericMenu nodeContextMenu
    {
        get { return _nodeContextMenu; }
        set { _nodeContextMenu = value; }
    }
#endregion

	void OnEnable()
	{
		loading = false;
        hideFlags = HideFlags.HideAndDontSave;

        if(nodes == null) nodes = new List<N>();
        if(connections == null) connections = new List<Connection>();
        if(connectionPoints == null) connectionPoints = new List<ConnectionPoint>();

        if(data != null) LoadData(null);

        EditorUtility.SetDirty(data);
        //DeleteData(null);
	}

    /*void OnDisable()
    {
        DestroyImmediate(this);
    }*/

	void OnGUI()
	{
		if(loading)
		{ //If the data is loading, cover all Window plain and show a Loading message
			GUI.Box(new Rect(0, 0, position.width, position.height), WINDOW_LOADING_MESSAGE);
		}
		else
		{
            //if(!EditorApplication.isPlaying)
			if(data != null && attributes != null)
			{ //If this Editor Window has Data and Attributes.
				Event currentEvent = Event.current;

				DrawGrid(attributes.thinLineSpacing, attributes.thinLineAlpha, attributes.thinLineColor);
		        DrawGrid(attributes.grossLineSpacing, attributes.grossLineAlpha, attributes.grossLineColor);

				DrawNodes();
				DrawConnections();
		        DrawConnectionLine(currentEvent);
				ProcessEvents(currentEvent);
				ProcessNodeEvents(currentEvent);
				if(GUI.changed) Repaint();
			}
			else
			{
				Debug.LogWarning("[BaseNodeEditorWindow] You should Load Data first!");
			}	

			DrawDataPanel();
		}
	}

	/// \TODO Adjust values properly...
	/// <summary>Draws Data Panel that receives the Data ScriptableObject.</summary>
	public virtual void DrawDataPanel()
	{
        if(data != null && attributes != null)
        {
        	GUI.Box(new Rect(0, (position.height - attributes.dataPanelHeight), position.width, attributes.dataPanelHeight), NO_TEXT/*, attributes.dataPanelStyle*/);
        	data = EditorGUI.ObjectField(new Rect(0, (position.height - attributes.dataPanelHeight), position.width, attributes.dataFieldHeight), WINDOW_PANEL_DATA_FIELD, data, typeof(D), true) as D;
            gameData = EditorGUI.ObjectField(new Rect(0, (position.height - (attributes.dataPanelHeight - attributes.dataFieldHeight)), position.width, attributes.dataFieldHeight), WINDOW_PANEL_GAME_DATA_FIELD, gameData, typeof(T), true) as T;

            /// \TODO MAKE A VERTICAL / HORIZONAL LAYOUT FUNCTION FOR EDITORWINDOW.
            float centerPositionX = ((position.width - attributes.buttonWidth) * 0.5f);
            float leftPositionX = (centerPositionX - (attributes.buttonWidth * 0.5f) - attributes.buttonSpacing);
            float rightPositionX = (centerPositionX + (attributes.buttonWidth * 0.5f) + attributes.buttonSpacing);

        	if
            (GUI.Button(new Rect
                (
                    leftPositionX,
                    ((position.height - attributes.dataPanelHeight) + ((attributes.buttonHeight * 0.5f) + attributes.buttonYOffset)),
                    attributes.buttonWidth,
                    attributes.buttonHeight
                ),
                WINDOW_PANEL_LOAD_BUTTON)
            )
	    	{
	    		LoadData(()=> { /*...*/ });
	    	}
	        if
            (GUI.Button(new Rect
                (
                    centerPositionX,
                    ((position.height - attributes.dataPanelHeight) + ((attributes.buttonHeight * 0.5f) + attributes.buttonYOffset)),
                    attributes.buttonWidth,
                    attributes.buttonHeight
                ),
                WINDOW_PANEL_SAVE_BUTTON)
            )
	    	{
	    		SaveData(()=> { /*...*/ });
			}
            if
            (GUI.Button(new Rect
                (
                    rightPositionX,
                    ((position.height - attributes.dataPanelHeight) + ((attributes.buttonHeight * 0.5f) + attributes.buttonYOffset)),
                    attributes.buttonWidth,
                    attributes.buttonHeight 
                ),
                WINDOW_PANEL_DELETE_BUTTON)
            )
            {
                if(EditorUtility.DisplayDialog
                (
                    WINDOW_DELETE_PROMPT_TITLE,
                    WINDOW_DELETE_PROMPT_MESSAGE,
                    WINDOW_ACCEPT_ANSWER,
                    WINDOW_CANCEL_ANSWER
                ))
                {
                    DeleteData(()=> { LoadData(null); });
                } 
            }
        }
        else
        {
        	data = EditorGUI.ObjectField(new Rect(0, (position.height - (position.height * 0.5f)), position.width, DEFAULT_DATA_FIELD_HEIGHT), WINDOW_PANEL_DATA_FIELD, data, typeof(D), true) as D;
        	GUI.Box(new Rect(0, 0, position.width, position.height), NO_TEXT);
			if(GUI.Button(new Rect(((position.width - DEFAULT_LOAD_BUTTON_WIDTH) * 0.5f), position.height - ((DEFAULT_DATA_PANEL_HEIGHT * 0.5f) + DEFAULT_BUTTON_Y_OFFSET), DEFAULT_LOAD_BUTTON_WIDTH, DEFAULT_LOAD_BUTTON_HEIGHT), WINDOW_PANEL_LOAD_BUTTON))
	    	{
	    		LoadData(()=> { /*...*/ });
	    	}        
        }      	
	}

	/// <summary>Draws Nodes on Window.</summary>
	public virtual void DrawNodes()
	{
		if(nodes != null)
		{
			foreach(N node in nodes)
			{
				node.Draw();
			}
		}
	}

	/// <summary>Draws Connection son Window.</summary>
	public virtual void DrawConnections()
	{
		if(connections != null)
		{
			foreach(Connection connection in connections)
			{
				connection.Draw();
			}
		}
	}

	/// <summary>Processes EventSystem mouse's events.</summary>
	/// <param name="_event">Current EventSystem's Event.</param>
	/// <returns>State of the processing.</returns>
	public virtual bool ProcessEvents(Event _event)
	{
		drag = Vector2.zero;

        switch (_event.type)
        {
            case EventType.MouseDown: //When any mouse button is pressed
                if (_event.button == 0)
                { //If left-click button was pressed
                    ClearConnectionSelection();
                }

                if (_event.button == 1)
                { //If right-click button was pressed
                    ProcessContextMenu(_event.mousePosition);
                }
            break;

            case EventType.MouseDrag: //When the mouse is being dragged [had a delta-position]
                /*if (_event.button == 0)
                {
                    OnDrag(_event.delta);
                }*/
                if(_event.button == 2)
                { //Middle Button [Scroll]
                	OnDrag(_event.delta);
                }
            break;
        }

        return (_event != null);
	}

	/// <summary>Processes Nodes's Events.</summary>
	/// <param name="_event">Current EventSystem Event.</param>
	public virtual void ProcessNodeEvents(Event _event)
	{
		if (nodes != null)
        {
            foreach(N node in nodes)
            { 
                if(node.ProcessEvents(_event))
                {
                    GUI.changed = true;
                }
            }
        }
	}

	/// <summary>Draws Connection Line between two points [One has to be of type In, and other of type Out].</summary>
	/// <param name="_event">Current Event.</param>
	public virtual void DrawConnectionLine(Event _event)
    {
        if (selectedInPoint != null && selectedOutPoint == null)
        {
            Handles.DrawBezier
            (
                selectedInPoint.rect.center,
                _event.mousePosition,
                selectedInPoint.rect.center + Vector2.left * attributes.beizerLineTangent,
                _event.mousePosition - Vector2.left * attributes.beizerLineTangent,
                Color.white,
                null,
                2f
            );

            GUI.changed = true;
        }

        if (selectedOutPoint != null && selectedInPoint == null)
        {
            Handles.DrawBezier
            (
                selectedOutPoint.rect.center,
                _event.mousePosition,
                selectedOutPoint.rect.center - Vector2.left * attributes.beizerLineTangent,
                _event.mousePosition + Vector2.left * attributes.beizerLineTangent,
                Color.white,
                null,
                2f
            );

            GUI.changed = true;
        }
    }

    /// <summary>Callback invoked when there is a Drag.</summary>
    /// <param name="_delta">Delta of mouse position between the last frame and the actual frame.</param>
    public virtual void OnDrag(Vector2 _delta)
    {
        drag = _delta;

        if (nodes != null)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].Drag(_delta);
            }
        }

        GUI.changed = true;
    }

    /// <summary>Callback invoked when a Node is being clicked.</summary>
    public virtual void OnClickAddNode(Vector2 mousePosition)
    {
        if (nodes == null)
        {
            nodes = new List<N>();
        }

        //nodes.Add(new Node(mousePosition, 200, 50, nodeStyle, selectedNodeStyle, inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode));
    }

    /// <summary>Selects the Input Point.</summary>
    /// <param name="_inPoint">Input Point that invoked this method.</param>
    public virtual void OnClickInPoint(ConnectionPoint _inPoint)
    {
        selectedInPoint = _inPoint;

        if (selectedOutPoint != null)
        {
            if (selectedOutPoint.node != selectedInPoint.node)
            {
                CheckIfCanCreateConnection();
                ClearConnectionSelection(); 
            }
            else
            {
                ClearConnectionSelection();
            }
        }
    }

     /// <summary>Selects the Output Point.</summary>
    /// <param name="_outPoint">Output Point that invoked this method.</param>
    public virtual void OnClickOutPoint(ConnectionPoint _outPoint)
    {
        selectedOutPoint = _outPoint;

        if (selectedInPoint != null)
        {
            if (selectedOutPoint.node != selectedInPoint.node)
            {
                CheckIfCanCreateConnection();
                ClearConnectionSelection();
            }
            else
            {
                ClearConnectionSelection();
            }
        }
    }

    /// <summary>Removes Node from the Window and all connections attached to it.</summary>
    /// <param name="_node">Node to remove.</param>
    public virtual void OnClickRemoveNode(Node _node)
    {
        if (connections != null)
        {
            List<Connection> connectionsToRemove = new List<Connection>();

            foreach(Connection connection in connections)
            {
                if (connection.inPoint == _node.inPoint || connection.outPoint == _node.outPoint)
                {
                    connectionsToRemove.Add(connection);
                }
            }

            foreach(Connection connectionToRemove in connectionsToRemove)
            {
                connections.Remove(connectionToRemove);
            }

            connectionsToRemove = null;
        }

        nodes.Remove((N)_node);
    }

    /// <summary>Copies T Node.</summary>
    /// <param name="_node">Node Reference to copy.</param>
    /// <param name="_mousePosition">Position of the mouse when the copy request was made.</param>
    public virtual void OnClickCopyNode(Node _node, Vector2 _mousePosition)
    {
    	/// \TODO Implement copy functionallity.
    }

    /// <summary>Removes Connection from Editor Window.</summary>
    /// <param name="_connection">Connection to remove.</param>
    public virtual void OnClickRemoveConnection(Connection _connection)
    {
        //Remove child from connection's parent node.
        _connection.outPoint.node.RemoveChildNode(_connection.inPoint.node);

        _connection.inPoint.connections.Remove(_connection);
        _connection.outPoint.connections.Remove(_connection);
        connections.Remove(_connection);
    }

    /// <summary>Creates Node Connection [creates collection list if null].</summary>
    public virtual void CheckIfCanCreateConnection()
    {
        if (connections == null)
        {
            connections = new List<Connection>();
        }

        //connections.Add(new C(selectedInPoint, selectedOutPoint, OnClickRemoveConnection));
        if(!selectedOutPoint.node.HasChildNode(selectedInPoint.node))
        {
            CreateConnection(selectedInPoint, selectedOutPoint);
            selectedOutPoint.node.AddChildNode(selectedInPoint.node);
        }
        else
        {
            EditorUtility.DisplayDialog
            (
                WINDOW_REPEATED_CONNECTION_TITLE,
                (selectedOutPoint.node.nodeName +
                WINDOW_REPEATED_CONNECTION_MESSAGE_0 +
                selectedInPoint.node.nodeName +
                WINDOW_REPEATED_CONNECTION_MESSAGE_1),
                WINDOW_ACCEPT_ANSWER
            );
        }  
    }

    public virtual void CreateConnection(ConnectionPoint _input, ConnectionPoint _output)
    {
        Connection newConnection = Connection.NewInstance
        (
            _input,
            _output,
            attributes.pointAllingmentType,
            attributes.beizerLineColor,
            attributes.beizerLineTangent,
            attributes.beizerLineWidth,
            attributes.handlesButtonSize,
            attributes.handlesButtonPickSize,
            OnClickRemoveConnection
        );


        newConnection.inPoint.connections.Add(newConnection);
        newConnection.outPoint.connections.Add(newConnection);
        connections.Add(newConnection);
    }

    /// <summary>Sets selectedInPoint and selectedOutPoint to default [null].</summary>
    public virtual void ClearConnectionSelection()
    {
        selectedInPoint = null;
        selectedOutPoint = null;
    }

    /// <summary>Draws Grid as background.</summary>
    public virtual void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
    {
        int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
        int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

        Handles.BeginGUI();
        Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

        offset += drag * 0.5f;
        Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

        for (int i = 0; i < widthDivs; i++)
        {
            Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
        }

        for (int j = 0; j < heightDivs; j++)
        {
            Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(position.width, gridSpacing * j, 0f) + newOffset);
        }

        Handles.color = Color.white;
        Handles.EndGUI();
    }

	/// <summary>Processes Context Menu.</summary>
	/// <param name="_mousePosition">Mouse Position when the Context Menu invocation took place.</param>
	public abstract void ProcessContextMenu(Vector2 _mousePosition);

	/// <summary>Creates Node.</summary>
	/// <param name="_mousePosition">Event's Mouse Position.</param>
	public abstract void CreateNode(Vector2 _mousePosition);

    /// <summary>Converts Editor Nodes to a readable format on Play mode.</summary>
    /// <returns>Readable format Node [preferibaly inheriting INode].</returns>
    public abstract I GetConvertedNodes();

	/// <summary>Saves Editor Window's Data into assignated ScriptableObject data container.</summary>
    /// <param name="onSaveEnds">Action called when the Save process ends.</param>
	public virtual void SaveData(Action onSaveEnds)
	{
		loading = true;

		if(data != null)
		{
            data.ResetTemporalMemory();
            if(nodes != null)
            {
                for(int i = 0; i < nodes.Count; i++)
                {
                    data.SaveNode<N>(nodes[i]);
                }
            }

			data.nodes = nodes;
			data.connections = connections;
            data.connectionPoints = connectionPoints;
		}
		else
		{
            EditorUtility.DisplayDialog
            (
                WINDOW_NO_DATA_TITLE,
                WINDOW_NO_DATA_MESSAGE,
                WINDOW_ACCEPT_ANSWER
            );
		}

        AssetDatabase.SaveAssets();
		Debug.LogWarning("[BaseNodeEditorWindow] SaveData Result: " + data != null ? SAVE_SUCCESS_MESSAGE : SAVE_ERROR_MESSAGE);
		loading = false;
		if(onSaveEnds != null) onSaveEnds();
	}

	/// <summary>Loads Editor Window's Data from ScriptableObject data container.</summary>
    /// <param name="onLoadEnds">Action called when the Load process ends.</param>
	public virtual void LoadData(Action onLoadEnds)
	{
		loading = true;

		if(data != null)
		{ //If there is Data attached to the Node Editor:
            if(data.attributes != null)
            { //If there is an Attributes Assets attached to the Data Asset:
                attributes = data.attributes;
                //nodes = data.GetNodes();
                nodes = data.nodes;
                connections = data.connections;
                connectionPoints = data.connectionPoints;

                if(nodes != null)
                {
                    for(int i = 0; i < nodes.Count; i++)
                    {
                    	if(nodes[i] != null)
                    	{
                    		nodes[i].OnRemoveNode = OnClickRemoveNode;
    	                    nodes[i].OnCopyNode = OnClickCopyNode;

                            if(nodes[i].inPoint != null)
                            {
                                ConnectionPoint currentInputPoint = nodes[i].inPoint;

                                currentInputPoint.node = nodes[i];
                                currentInputPoint.OnClickConnectionPoint = OnClickInPoint;
                            } 
                            if(nodes[i].outPoint != null)
                            {
                                ConnectionPoint currentOutputPoint = nodes[i].outPoint;

                                currentOutputPoint.node = nodes[i];
                                currentOutputPoint.OnClickConnectionPoint = OnClickOutPoint;
                            }
                    	}
    	                    
                    }

                    if(connectionPoints != null)
                    for(int i = 0; i < connectionPoints.Count; i++)
                    {
                        for(int j = 0; j < nodes.Count; j++)
                        {
                            if(connectionPoints[i] != null)
                            if(connectionPoints[i].node == nodes[j])
                            {
                                connectionPoints[i].node = nodes[j];
                                switch(connectionPoints[i].pointType)
                                {
                                    case PointTypes.In:
                                    nodes[j].inPoint = connectionPoints[i];
                                    break;

                                    case PointTypes.Out:
                                    nodes[j].outPoint = connectionPoints[i];
                                    break;
                                }
                                break;
                            }   
                        }
                    }

                    if(connections != null)
                    {
                        for(int i = 0; i < connections.Count; i++)
                        {
                            connections[i].OnClickRemoveConnection = OnClickRemoveConnection;   
                        }
                    }
                }
            }
            else
            { //If there is no Attribute Asset attached:
                EditorUtility.DisplayDialog
                (
                    WINDOW_NO_ATTRIBUTES_TITLE,
                    WINDOW_NO_ATTRIBUTES_MESSAGE,
                    WINDOW_ACCEPT_ANSWER
                );
            }
    			
		}
		else
		{ //If there is no Data attached to this Node Editor:
            EditorUtility.DisplayDialog
            (
                WINDOW_NO_DATA_TITLE,
                WINDOW_NO_DATA_MESSAGE,
                WINDOW_ACCEPT_ANSWER
            );
		}

		Debug.LogWarning("[BaseNodeEditorWindow] LoadData Result: " + data != null ? LOAD_SUCCESS_MESSAGE : LOAD_ERROR_MESSAGE);
		loading = false;
		if(onLoadEnds != null) onLoadEnds();
	}

    /// <summary>Deletes Data from ScriptableObject data container.</summary>
    /// <param name="onDeleteEnds">Action called when the Delete process ends.</param>
    public virtual void DeleteData(Action onDeleteEnds)
    {
        if(data != null)
        {
            data.DeleteData();
        }
        else
        {
             EditorUtility.DisplayDialog
            (
                WINDOW_NO_DATA_TITLE,
                WINDOW_NO_DATA_MESSAGE,
                WINDOW_ACCEPT_ANSWER
            );
        }

        if(onDeleteEnds != null) onDeleteEnds();
    }

	/// <summary>Creates Node Editor Window [Needs to be overriden].</summary>
	public static BaseNodeEditorWindow<N, D, A, T, I> CreateNodeEditorWindow()
	{
		Debug.Log("[BaseNodeEditorWindow] You are Implementing Base Class's CreateNodeEditorWindow Method. Consider override this method");
		return null;
	}

	/// <summary>Opens Node Editor Window with its respective required Data.</summary>
	/// <param name="_data">Required Node Editor Data.</param>
	public static void OpenNodeEditorWindowWithData(D _data)
	{
		BaseNodeEditorWindow<N, D, A, T, I> nodeEditorWindow = CreateNodeEditorWindow();
		nodeEditorWindow.data = _data;
		nodeEditorWindow.LoadData(null);
	}

    /// <summary>Creates Node of type N.</summary>
    /// <param name="_mousePosition">Position where the mouse is going to be created.</param>
    /// <param name="createInputConnectionPoint">Create Input Connection Point on this Node?.</param>
    /// <param name="createOutputConnectionPoint">Create Output Connection Point on this Node?.</param>
    /// <returns>New N Node with the basic Node properties.</returns>
    protected N CreateNodeOfType<N>(Vector2 _mousePosition, bool createInputConnectionPoint, bool createOutputConnectionPoint) where N : Node, new()
    {
        Node node = Node.NewInstance
        (
            _mousePosition,
            attributes.nodeWidth,
            attributes.nodeHeight,
            attributes.nodeFieldHorizontalOffset,
            attributes.nodeFieldHeight,
            attributes.nodeStyle,
            attributes.selectedNodeStyle,
            OnClickRemoveNode,
            OnClickCopyNode
        );

        /// \TODO Implicit/Explicit up casting from Node/N to N/Node
        //N n = new N();
        N n = CreateInstance<N>();

        n.rect = node.rect;
        n.fieldHorizontalOffset = node.fieldHorizontalOffset;
        n.fieldHeight = node.fieldHeight;
        n.originalHeight = node.originalHeight;
        n.nodeStyle = node.nodeStyle;
        n.defaultNodeStyle = node.defaultNodeStyle;
        n.selectedNodeStyle = node.selectedNodeStyle;
        n.OnRemoveNode = node.OnRemoveNode;
        n.OnCopyNode = node.OnCopyNode;

        if(createInputConnectionPoint) CreateInputConnectionPoint(n);
        if(createOutputConnectionPoint) CreateOutputConnectionPoint(n);

        node = null;

        return n;
    }

    /// <summary>Creates Node's Input Connection Point.</summary>
    /// <param name="_node">Node that owns the Connection Point.</param>
    protected void CreateInputConnectionPoint(Node _node)
    {
        ConnectionPoint newConnectionPoint = ConnectionPoint.NewInstance
        (
            _node,
            attributes.pointWidth,
            attributes.pointHeight,
            attributes.rectWidthOffset,
            attributes.rectHeightOffset,
            attributes.pointAllingmentType,
            PointTypes.In,
            attributes.inPointStyle,
            OnClickInPoint
        );

         _node.inPoint = newConnectionPoint;
         if(connectionPoints == null) connectionPoints = new List<ConnectionPoint>();
         connectionPoints.Add(newConnectionPoint);
    }

    /// <summary>Creates Node's Output Connection Point.</summary>
    /// <param name="_node">Node that owns the Connection Point.</param>
    protected void CreateOutputConnectionPoint(Node _node)
    {
        ConnectionPoint newConnectionPoint = ConnectionPoint.NewInstance
        (
            _node,
            attributes.pointWidth,
            attributes.pointHeight,
            attributes.rectWidthOffset,
            attributes.rectHeightOffset,
            attributes.pointAllingmentType,
            PointTypes.Out,
            attributes.outPointStyle,
            OnClickOutPoint
        );
        
         _node.outPoint = newConnectionPoint;
         if(connectionPoints == null) connectionPoints = new List<ConnectionPoint>();
         connectionPoints.Add(newConnectionPoint);

    }
}
}