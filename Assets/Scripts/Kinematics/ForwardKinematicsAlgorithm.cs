using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ForwardKinematicsAlgorithm
{
    public static Vector3[] CalculateJointPositions(Vector3 basePosition, float[] lengths, Vector3[] axis, float[] anglesInDegrees)
    {
        Vector3[] points = InitialJointPositions(basePosition, lengths);
        Vector3[] rotatedAxis = ComputeAngles(axis, anglesInDegrees);
        Quaternion angle = Quaternion.identity;

        for (int i = 1; i < points.Length; i++)
        {
            var diff = Vector3.up * lengths[i - 1];
            angle = Quaternion.AngleAxis(anglesInDegrees[i - 1], rotatedAxis[i - 1]) * angle;
            diff = angle * diff;
            points[i] = points[i - 1] + diff;
        }

        return points;
    }

    public static Vector3 CalculateTipPosition(Vector3 basePosition, float[] lengths, Vector3[] axis, float[] anglesInDegrees)
    {
        return CalculateJointPositions(basePosition, lengths, axis, anglesInDegrees).Last();
    }

    private static Vector3[] InitialJointPositions(Vector3 basePosition, float[] lengths)
    {
        Vector3[] points = new Vector3[lengths.Length + 1];
        points[0] = basePosition;
        for (int i = 1; i < points.Length; i++)
        {
            points[i] = points[i - 1] + Vector3.up * lengths[i - 1];
        }
        return points;
    }

    private static Vector3[] ComputeAngles(Vector3[] axis, float[] angles)
    {
        Quaternion angle = Quaternion.identity;
        Vector3[] rotatedAxis = new Vector3[axis.Length];
        rotatedAxis[0] = axis[0];

        for (int i = 1; i < axis.Length; i++)
        {
            angle = Quaternion.AngleAxis(angles[i - 1], axis[i - 1]) * angle;
            rotatedAxis[i] = angle * axis[i];
        }

        return rotatedAxis;
    }
}
