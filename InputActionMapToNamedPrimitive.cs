using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputActionMapToNamedPrimitive : MonoBehaviour
{

    public InputActionMap m_currentActionMap;

    [Tooltip("Input actions associated with the player.")]
    [SerializeField] public InputActionAsset m_actions;


    public NamedBoolEvent           m_onBoolEvent;
    public NamedFloatEvent          m_onFloatEvent;
    public NamedVector3Event        m_onVector3Event;
    public NamedQuaternionEvent     m_onQuaternionEvent;


    [System.Serializable]
    public class NamedBoolEvent : UnityEvent<string, bool> { }

    [System.Serializable]
    public class NamedFloatEvent : UnityEvent<string, float> { }

    [System.Serializable]
    public class NamedVector3Event : UnityEvent<string, Vector3> { }

    [System.Serializable]
    public class NamedQuaternionEvent : UnityEvent<string, Quaternion> { }


    private void OnEnable()
    {
        m_actions.Enable();
        foreach (var action in m_actions)
        {
            action.Enable();
            action.performed += context => OnActionPerformed(action, context);
            action.canceled += context => OnActionCanceled(action, context);
        }
    }
    public bool m_useMapNameInName;
    public string m_charToSplitMapAndName = "_";

    public string GetName(ref InputAction action) {
        if (!m_useMapNameInName)
            return action.name;
        return action.actionMap.name + m_charToSplitMapAndName + action.name;
    }
    private void OnActionPerformed(InputAction action, InputAction.CallbackContext context)
    {
        var inputValue = context.ReadValueAsObject();
        
        PushContext(GetName(ref action), context);
    }

    private void OnActionCanceled(InputAction action, InputAction.CallbackContext context)
    {
        PushContext(GetName(ref action), context);
    }

    private void PushContext(string name, InputAction.CallbackContext context)
    {

        //Debug.Log($"{name} performed. Value: {context.ReadValueAsObject()} Type: { context.valueType}");

       
        if (context.valueType == typeof(System.Single))
        {   float value = context.ReadValue<System.Single>();
            //Debug.Log($"{name} performed. Value: {value} Type: { context.valueType}");

            m_onBoolEvent.Invoke(name, value >= 1);
            if (value > 0.001 & value < 0.9999) { 
                if (!m_isConsiderFloat.ContainsKey(name))
                    m_isConsiderFloat.Add(name, value);
            }
            if (m_isConsiderFloat.ContainsKey(name)) {
                m_isConsiderFloat[name] = value;
                m_onFloatEvent.Invoke(name, value);
            }
        }

        else if (context.valueType == typeof(UnityEngine.Vector2))
        {
            Vector2 value = context.ReadValue<UnityEngine.Vector2>();
          //  Debug.Log($"{name} performed. Value: {value} Type: { context.valueType}");

            m_onVector3Event.Invoke(name, value);
        }
        else if (context.valueType == typeof(UnityEngine.Vector3))
        {
            Vector3 value = context.ReadValue<UnityEngine.Vector3>();
            //  Debug.Log($"{name} performed. Value: {value} Type: { context.valueType}");

            m_onVector3Event.Invoke(name, value);
        }
        else if (context.valueType == typeof(UnityEngine.Quaternion))
        {
            Quaternion value = context.ReadValue<UnityEngine.Quaternion>();
            // Debug.Log($"{name} performed. Value: {value} Type: { context.valueType}");

            m_onQuaternionEvent.Invoke(name, value);
        }


    }

    public Dictionary<string, float> m_isConsiderFloat = new Dictionary<string, float>();
    //public Dictionary<string, float> m_isConsiderVector = new Dictionary<string, float>();
    //public Dictionary<string, float> m_isConsiderQuaternion = new Dictionary<string, float>();
    //public Dictionary<string, float> m_isConsiderFloat = new Dictionary<string, float>();


    private void OnDisable()
    {
        m_actions.Disable();
        foreach (var action in m_actions)
        {
            action.Disable();
            action.performed -= context => OnActionPerformed(action, context);
            action.canceled -= context => OnActionCanceled(action, context);
        }
    }
}
