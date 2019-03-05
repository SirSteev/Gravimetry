using UnityEngine;

public static class IsNull
{
    public static bool IsTrueNull(this Object obj)
    {
        return (object)obj == null;
    }

    public static bool HasComponent<T>(this GameObject flag) where T : Component
    {
        return flag.GetComponent<T>() != null;
    }
}
