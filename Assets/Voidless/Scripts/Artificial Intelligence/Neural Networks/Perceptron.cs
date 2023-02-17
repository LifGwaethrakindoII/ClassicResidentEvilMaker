using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public enum ActivationFunction
{
	Sigmoid,
	ReLU
}

[Serializable]
public class Perceptron
{
	[HideInInspector] public PerceptronData[] inputs; 					/// <summary>Input Nodes.</summary>	
	[HideInInspector] public float[] weights; 							/// <summary>Input Weights.</summary>
	[HideInInspector] public PerceptronData output; 					/// <summary>Output Node.</summary>
	[HideInInspector] public float bias; 								/// <summary>Bias value.</summary>
	[Range(0.0f, 1.0f)] public float learningRate; 						/// <summary>Learning Rate.</summary>
	[HideInInspector] public ActivationFunction activationFunction; 	/// <summary>Activation's Function type.</summary>
	public Func<float, float> a; 										/// <summary>Activation function.</summary>
	[HideInInspector] public float state; 								/// <summary>Current's State.</summary>

	public Perceptron(){}

	/// <returns>Activation function given the value of activationFunction.</returns>
	public Func<float, float> GetActivationFunction()
	{
		switch(activationFunction)
		{
			case ActivationFunction.Sigmoid: return VMath.Sigmoid;
			case ActivationFunction.ReLU: return VMath.RectifiedLinearUnit;
			default: return VMath.DefaultNormalizedPropertyFunction;
		}
	}

	/// <returns>Dot product of the Inputs and Weights vector, plus the bias.</returns>
	public float Sum()
	{
		float sum = 0.0f;

		sum += bias;

		for(int i = 0; i < Mathf.Min(inputs.Length, weights.Length); i++)
		{
			sum += (inputs[i].value * weights[i]);
		}

		return sum;
	}

	/// <summary>Sets Input Value.</summary>
	/// <param name="i">Input's Index.</param>
	/// <param name="x">New Input Value.</param>
	public void SetInputValue(int i, float x)
	{
		inputs[i].value = x;
	}

	/// <returns>Result of Activation Function.</returns>
	public float Activation(float s)
	{
		if(a == null) a = GetActivationFunction();

		return a(s);
	}

	/// <summary>Calculates the output and updates the current state.</summary>
	/// <returns>Output result given the forward propagation step.</returns>
	public float ForwardPropagation()
	{
		return output.value = state = Activation(Sum());
	}

	/// <summary>Updates weights and bias by backward propagation.</summary>
	public void BackwardPropagation(float t)
	{
		float d = t - state;
		float x = learningRate * d;

		for(int i = 0; i < Mathf.Min(inputs.Length, weights.Length); i++)
		{
			weights[i] = weights[i] + x * inputs[i].value;
		}

		bias = bias + x;
	}

	/// <returns>String representing this Perceptron.</returns>
	public string ToString()
	{
		StringBuilder builder = new StringBuilder();

		for(int i = 0; i < Mathf.Min(inputs.Length, weights.Length); i++)
		{
			builder.Append("Input #");
			builder.Append(i.ToString());
			builder.Append(" = [ ");
			builder.Append(inputs[i].ToString());
			builder.Append(", Weight: ");
			builder.Append(weights[i].ToString());
			builder.AppendLine(" ]");
		}
		builder.Append("Bias = ");
		builder.AppendLine(bias.ToString());
		builder.AppendLine("\n");
		builder.Append("State: ");
		builder.AppendLine(state.ToString());

		return builder.ToString();
	}
}
}