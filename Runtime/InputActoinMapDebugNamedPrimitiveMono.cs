using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputActoinMapDebugNamedPrimitiveMono : MonoBehaviour
{

    public NamedValueBool m_lastBoolean;
    public NamedValueFloat m_lastFloat;
    public NamedValueVector3 m_lastVector3;
    public NamedValueQuaternion m_lastQuaternion;
    [System.Serializable] public class NamedValueBool       : NamedValueDebug<bool> { }
    [System.Serializable] public class NamedValueFloat      : NamedValueDebug<float> { }
    [System.Serializable] public class NamedValueVector3    : NamedValueDebug<Vector3> { }
    [System.Serializable] public class NamedValueQuaternion : NamedValueDebug<Quaternion> { }
    [System.Serializable]
    public class NamedValueDebug<T> {
        public string m_named;
        public T m_value;

        public void Set(string name, T value) { m_named = name; m_value = value; }
    }

    public void Push(string name, bool value) => m_lastBoolean.Set(name, value);
    public void Push(string name, float value) => m_lastFloat.Set(name, value);
    public void Push(string name, Vector3 value) => m_lastVector3.Set(name, value);
    public void Push(string name, Quaternion value) => m_lastQuaternion.Set(name, value);
}

