using UnityEngine;

public static class MathHelper{
    // covert local space rotation to joint space
    public static Quaternion LocalToJoint (ConfigurableJoint jnt,
        Quaternion init,
        Quaternion tar) {

        Vector3 right = jnt.axis;
        Vector3 forward = Vector3.Cross(jnt.axis, jnt.secondaryAxis).normalized;
        Vector3 up = Vector3.Cross(forward, right).normalized;

        Quaternion w2j = Quaternion.LookRotation(forward, up);

        Quaternion res = Quaternion.Inverse(w2j);
        res *= Quaternion.Inverse(tar) * init;
        res *= w2j;
        return res;
    }

    // convert world space rotation to joint space
    public static Quaternion WorldToJoint (ConfigurableJoint jnt,
        Quaternion init,
        Quaternion tar) {

        Vector3 right = jnt.axis;
        Vector3 forward = Vector3.Cross(jnt.axis, jnt.secondaryAxis).normalized;
        Vector3 up = Vector3.Cross(forward, right).normalized;

        Quaternion w2j = Quaternion.LookRotation(forward, up);

        Quaternion res = Quaternion.Inverse(w2j);
        res *= init * Quaternion.Inverse(tar);
        res *= w2j;
        return res;
    }
}
