using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voidless
{
[CanEditMultipleObjects]
[CustomEditor(typeof(ParticleEffect))]
public class ParticleEffectInspector : Editor
{
	protected ParticleEffect particleEffect; 	/// <summary>Inspector's Target.</summary>
	private float time; 						/// <summary>simulation's time.</summary>

	/// <summary>Sets target property.</summary>
	private void OnEnable()
	{
		particleEffect = target as ParticleEffect;
		if(particleEffect.systems == null || particleEffect.systems.Length == 0) particleEffect.GetParticleSystems();
		time = 1.0f;
		EditorUtility.SetDirty(particleEffect);
	}

	/// <summary>OnInspectorGUI override.</summary>
	public override void OnInspectorGUI()
	{	
		DrawDefaultInspector();

		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();

		time = EditorGUILayout.FloatField("Simulation Time: ", time);

		if(!particleEffect.isPlaying && GUILayout.Button("Simulate")) particleEffect.Simulate(true, time);
		if(!particleEffect.isPlaying && GUILayout.Button("Play")) particleEffect.Play();
		if(particleEffect.isPlaying && GUILayout.Button("Pause")) particleEffect.Pause();
		if(!particleEffect.isStopped && GUILayout.Button("Stop")) particleEffect.Stop();
		if(particleEffect.isPlaying && GUILayout.Button("Clear")) particleEffect.Clear();
		if(GUILayout.Button(" Get Children Particle Systems"))
		{
			particleEffect.GetParticleSystems();

#if UNITY_EDITOR
			if(!Application.isPlaying)
			{
				EditorUtility.SetDirty(this);
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
			}
#endif
		}

   		serializedObject.ApplyModifiedProperties();
	}
}
}