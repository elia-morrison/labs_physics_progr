using System.Collections.Generic;
using UnityEngine;

public static class InverseKinematics
{
    public static void UpdateAngles(Vector3 targetPosition, RoboticArm arm, float baseLearningRate = 10f)
    {
        float deltaAngle = 0.3f;

        int numberOfJoints = arm.AnglesInDegrees.Length;

        for (int i = 0; i < numberOfJoints; i++)
        {
            Vector3 initialTipPosition = InitialTipPosition(arm);
            float initialDistanceToTarget = Vector3.Distance(initialTipPosition, targetPosition);
            Vector3 perturbedTipPosition = PerturbedTipPosition(arm, i, deltaAngle);
            float perturbedDistanceToTarget = Vector3.Distance(perturbedTipPosition, targetPosition);
            float differential = (perturbedDistanceToTarget - initialDistanceToTarget) / deltaAngle;

            // Calculate an exponential scaling factor based on the joint's position in the chain
            float scalingFactor = (i + 1);
            float adjustedLearningRate = baseLearningRate * scalingFactor;

            arm.AnglesInDegrees[i] -= differential * adjustedLearningRate;
            arm.AnglesInDegrees[i] = Mathf.Clamp(arm.AnglesInDegrees[i], -90f, 90f);
        }
    }

    private static Vector3 InitialTipPosition(RoboticArm arm)
    {
        return ForwardKinematicsAlgorithm.CalculateTipPosition(arm.BasePosition, arm.Lengthes, arm.Axis, arm.AnglesInDegrees);
    }

    private static Vector3 PerturbedTipPosition(RoboticArm arm, int index, float deltaAngle)
    {
        float[] angles = (float[])arm.AnglesInDegrees.Clone();
        angles[index] += deltaAngle;

        return ForwardKinematicsAlgorithm.CalculateTipPosition(arm.BasePosition, arm.Lengthes, arm.Axis, angles);
    }
}
