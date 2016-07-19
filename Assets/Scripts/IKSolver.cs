using UnityEngine;

/*!
 * Implemented a modified version of IK solver from
 *
 * [ FABRIK: A fast, iterative solver for the Inverse Kinematics problem ]
 *
 * Basically a bidirection iteration solver, limited the maximum iteration
 * time to full-fill realtime requirement.
 */

public class IKSolver {

    // error tolerance
    private const float k_tol = 0.01f;
    // maximum iteration times
    private const int k_max_iter = 40;

    /*!
     * LimitedSolveIK - take initial position and target position in world 
     * space as input, direction as constraint vector. When inverse is true,
     * last joint in the list would be the root.
     */
    public Vector3[] LimitedSolveIK (Vector3[] init_p,
        Vector3 target,
        Vector3 direction,
        bool inverse = false) {

        int chain_l = init_p.Length;
        Vector3[] p = new Vector3[chain_l];

        if (inverse)
            for (int i = 0; i < chain_l; ++i)
                p[i] = init_p[chain_l - i - 1];
        else
            for (int i = 0; i < chain_l; ++i)
                p[i] = init_p[i];

        // calculate the distance between joints
        float[] d = new float[chain_l - 1];
        float dsum = 0, dist = Vector3.Distance(p[0], target);

        for (int i = 0; i < chain_l - 1; ++i) {
            d[i] = Vector3.Distance(p[i + 1], p[i]);
            dsum += d[i];
        }

        float[] r = new float[chain_l - 1];
        float[] lamda = new float[chain_l - 1];

        // if target is unreachable
        if (dist > dsum)
            target = (target - p[0]) * (dsum - k_tol) / dist + p[0];

        // iteration
        Vector3 b = p[0];
        float dif = Vector3.Distance(p[chain_l - 1], target);
        int iter = 0;
        while (dif > k_tol && iter < k_max_iter) {
            // forward reaching
            p[chain_l - 1] = target;
            for (int i = chain_l - 2; i >= 0; --i) {
                r[i] = Vector3.Distance(p[i + 1], p[i]);
                lamda[i] = d[i] / r[i];
                p[i] = (1 - lamda[i]) * p[i + 1] + lamda[i] * p[i];
            }

            // backward reaching
            p[0] = b;
            for (int i = 0; i < chain_l - 1; ++i) {
                r[i] = Vector3.Distance(p[i + 1], p[i]);
                lamda[i] = d[i] / r[i];
                p[i + 1] = (1 - lamda[i]) * p[i] + lamda[i] * p[i + 1];
            }

            dif = Vector3.Distance(p[chain_l - 1], target);
            ++iter;
        }

        // orientation constraint
        Vector3 pivot = inverse ?
            (target - init_p[chain_l - 1]).normalized :
            (target - init_p[0]).normalized;
        Vector3 ptr = Vector3.Cross(Vector3.Cross(pivot, direction), pivot).normalized;

        for (int i = 1; i < chain_l - 1; ++i) {
            Vector3 pver = Vector3.Dot(p[i] - p[0], pivot) * pivot;
            Vector3 phor = Vector3.Magnitude(p[i] - p[0] - pver) * ptr;
            p[i] = p[0] + pver + phor;
        }

        // reverse to origin alignment
        if (inverse) {
            int p1 = 0, p2 = chain_l - 1;
            while(p1 < p2) {
                Vector3 temp = p[p1];
                p[p1++] = p[p2];
                p[p2--] = temp;
            }

        }

        return p;
    }

}
