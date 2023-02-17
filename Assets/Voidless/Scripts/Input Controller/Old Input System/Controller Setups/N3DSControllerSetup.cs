using UnityEngine;
using System.Collections;
using System;
#if UNITY_N3DS
using UnityEngine.N3DS;
using N3DS = UnityEngine.N3DS;
#endif

using Unity = UnityEngine;

namespace Voidless
{
[Serializable]
public class N3DSControllerSetup : BaseControllerSetup
<
#if UNITY_N3DS
	N3dsButton
#else
	int
#endif
>
{
	/// <summary>Gets leftAxisX property.</summary>
	public override float leftAxisX
	{
		get
		{
#if UNITY_N3DS
			return GamePad.CirclePad.x;
#else
			return 0f;
#endif
		}
	}

	/// <summary>Gets leftAxisY property.</summary>
	public override float leftAxisY
	{
		get
		{
#if UNITY_N3DS
			return GamePad.CirclePad.y;
#else
			return 0f;
#endif
		}
	}

	/// <summary>Gets rightAxisX property.</summary>
	public override float rightAxisX
	{
		get
		{
#if UNITY_N3DS
			if(GamePad.GetButtonTrigger(N3dsButton.Right))
			{
				return 0.5f;
			}else if(GamePad.GetButtonTrigger(N3dsButton.Left))
			{
				return -0.5f;
			}else if(GamePad.GetButtonHold(N3dsButton.Right))
			{
				return 1f;
			}else if(GamePad.GetButtonHold(N3dsButton.Left))
			{
				return -1f;
			}else if(GamePad.GetButtonRelease(N3dsButton.Right))
			{
				return 0.1f;
			}else if(GamePad.GetButtonRelease(N3dsButton.Left))
			{
				return -0.1f;
			}
			else return 0f;
#else
			return 0f;
#endif
			//return GamePad.CirclePadPro.x;
		}
	}

	/// <summary>Gets rightAxisY property.</summary>
	public override float rightAxisY
	{
		get
		{
#if UNITY_N3DS
			if(GamePad.GetButtonTrigger(N3dsButton.Up))
			{
				return 0.5f;
			}else if(GamePad.GetButtonTrigger(N3dsButton.Down))
			{
				return -0.5f;
			}else if(GamePad.GetButtonHold(N3dsButton.Up))
			{
				return 1f;
			}else if(GamePad.GetButtonHold(N3dsButton.Down))
			{
				return -1f;
			}else if(GamePad.GetButtonRelease(N3dsButton.Up))
			{
				return 0.1f;
			}else if(GamePad.GetButtonRelease(N3dsButton.Down))
			{
				return -0.1f;
			}
			else return 0f;
#else
			return 0f;
#endif
			//return GamePad.CirclePadPro.y;
		}
	}

	/// <summary>Gets leftTrigger property.</summary>
	public override float leftTrigger { get { return 0f; } }

	/// <summary>Gets rightTrigger property.</summary>
	public override float rightTrigger { get { return 0f; } }

	/// <summary>Gets dPadAxisX property.</summary>
	public override float dPadAxisX
	{
		get
		{
#if UNITY_N3DS
			if(GamePad.GetButtonTrigger(N3dsButton.Right))
			{
				return 0.5f;
			}else if(GamePad.GetButtonTrigger(N3dsButton.Left))
			{
				return -0.5f;
			}else if(GamePad.GetButtonHold(N3dsButton.Right))
			{
				return 1f;
			}else if(GamePad.GetButtonHold(N3dsButton.Left))
			{
				return -1f;
			}else if(GamePad.GetButtonRelease(N3dsButton.Right))
			{
				return 0.1f;
			}else if(GamePad.GetButtonRelease(N3dsButton.Left))
			{
				return -0.1f;
			}
			else return 0f;
#else
			return 0f;
#endif
		}
	}

	/// <summary>Gets dPadAxisY property.</summary>
	public override float dPadAxisY
	{
		get
		{
#if UNITY_N3DS
			if(GamePad.GetButtonTrigger(N3dsButton.Up))
			{
				return 0.5f;
			}else if(GamePad.GetButtonTrigger(N3dsButton.Down))
			{
				return -0.5f;
			}else if(GamePad.GetButtonHold(N3dsButton.Up))
			{
				return 1f;
			}else if(GamePad.GetButtonHold(N3dsButton.Down))
			{
				return -1f;
			}else if(GamePad.GetButtonRelease(N3dsButton.Up))
			{
				return 0.1f;
			}else if(GamePad.GetButtonRelease(N3dsButton.Down))
			{
				return -0.1f;
			}
			else return 0f;
#else
			return 0f;
#endif
		}
	}

	/// <summary>N3DSControllerSetup's Constructor.</summary>
	public N3DSControllerSetup() : base() {}
}
}