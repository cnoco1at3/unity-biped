using UnityEngine;

/*!
 * Implemented a modified version of IK solver from
 *
 * [ FABRIK: A fast, iterative solver for the Inverse Kinematics problem ]
 *
 * Basically a bidirection iteration solver, limited the maximum iteration
 * time to full-fill realtime requirement.
 */

public static class FABRIKSolver {

    // error tolerance
    private const float kMaxError = 0.01f;
    // maximum iteration times
    private const int kMaxIteration = 40;

    /*!
     * LimitedSolveIK - take initial position and target position in world 
     * space as input, direction as constraint vector. When inverse is true,
     * last joint in the list would be the root.
     */
    public static void SolveIKWithVectorConstraint (ref Vector3[] position,
        Vector3 target,
        Vector3 direction,
        bool inverse = false) {

        // If the limb is in inverse index
        if (inverse) {
            int halflength = position.Length / 2;
            for (int i = 0; i < halflength; ++i) {
                Extension.Swap<Vector3>(ref position[i], ref position[position.Length - i - 1]);
            }
        }

        // calculate the distance between joints
        float[] d = new float[position.Length - 1];
        float dsum = 0, dist = Vector3.Distance(position[0], target);

        for (int i = 0; i < position.Length - 1; ++i) {
            d[i] = Vector3.Distance(position[i + 1], position[i]);
            dsum += d[i];
        }

        float[] r = new float[position.Length - 1];
        float[] lamda = new float[position.Length - 1];

        // if target is unreachable
        if (dist > dsum)
            target = (target - position[0]) * (dsum - kMaxError) / dist + position[0];

        // iteration
        Vector3 b = position[0];
        float err = Vector3.Distance(position[position.Length - 1], target);
        int iter = 0;
        while (err > kMaxError && iter < kMaxIteration) {
            // forward reaching
            position[position.Length - 1] = target;
            for (int i = position.Length - 2; i >= 0; --i) {
                r[i] = Vector3.Distance(position[i + 1], position[i]);
                lamda[i] = d[i] / r[i];
                position[i] = (1 - lamda[i]) * position[i + 1] + lamda[i] * position[i];
            }

            // backward reaching
            position[0] = b;
            for (int i = 0; i < position.Length - 1; ++i) {
                r[i] = Vector3.Distance(position[i + 1], position[i]);
                lamda[i] = d[i] / r[i];
                position[i + 1] = (1 - lamda[i]) * position[i] + lamda[i] * position[i + 1];
            }

            err = Vector3.Distance(position[position.Length - 1], target);
            ++iter;
        }

        // orientation constraint
        Vector3 pivot = inverse ?
            (target - position[position.Length - 1]).normalized :
            (target - position[0]).normalized;
        Vector3 ptr = Vector3.Cross(Vector3.Cross(pivot, direction), pivot).normalized;

        for (int i = 1; i < position.Length - 1; ++i) {
            Vector3 pver = Vector3.Dot(position[i] - position[0], pivot) * pivot;
            Vector3 phor = Vector3.Magnitude(position[i] - position[0] - pver) * ptr;
            position[i] = position[0] + pver + phor;
        }

        // reverse to origin alignment
        if (inverse) {
            int halflength = position.Length / 2;
            for(int i = 0; i < halflength; ++i) {
                Extension.Swap<Vector3>(ref position[i], ref position[position.Length - i - 1]);
            }
        }
    }
}
