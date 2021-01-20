using UnityEngine;

namespace WorldEditorServer
{
    public class Utils
    {
        public static string Vector3ToString(Vector3 vector3)
        {
            return vector3.x + "," + vector3.y + "," + vector3.z;
        }

        public static Vector3 StringToVector3(string vector3String)
        {
            var splitted = vector3String.Split(',');
            return new Vector3(float.Parse(splitted[0]), float.Parse(splitted[1]), float.Parse(splitted[2]));
        }

        public static string QuaternionToString(Quaternion quaternion)
        {
            return quaternion.x + "," + quaternion.y + "," + quaternion.z + "," + quaternion.w;
        }

        public static Quaternion StringToQuaternion(string quaternionString)
        {
            var splitted = quaternionString.Split(',');
            return new Quaternion(float.Parse(splitted[0]), float.Parse(splitted[1]), float.Parse(splitted[2]), float.Parse(splitted[3]));
        }
    }
}