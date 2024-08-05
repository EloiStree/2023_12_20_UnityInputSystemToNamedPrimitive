using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class InputActionMapAutoGenerateUnityEventChangedMono : MonoBehaviour
{


    public Dictionary<string, LabelEventOnChangedBool>  m_dicoBool = new Dictionary<string, LabelEventOnChangedBool>();
    public Dictionary<string, LabelEventOnChangedFloat> m_dicoFloat = new Dictionary<string, LabelEventOnChangedFloat>();


    public List<LabelEventOnChangedBool> m_labelBool = new List<LabelEventOnChangedBool>();
    public List<LabelEventOnChangedFloat> m_labelFloat = new List<LabelEventOnChangedFloat>();


    private void Awake()
    {
        foreach (var item in m_labelBool)
        {
            m_dicoBool.Add(item.m_labelId, item);
        }
        foreach (var item in m_labelFloat)
        {
            m_dicoFloat.Add(item.m_labelId, item);
        }
    }

    public void PushLabelValue(string label, bool value)
    {

        if (!m_dicoBool.ContainsKey(label)) { 
            m_dicoBool.Add(label, new LabelEventOnChangedBool(label));
            m_labelBool = m_dicoBool.Values.ToList();
        }
        m_dicoBool[label].PushValue(value);

    }
    public void PushLabelValue(string label, float value)
    {
        if (!m_dicoFloat.ContainsKey(label)) { 
            m_dicoFloat.Add(label, new LabelEventOnChangedFloat(label));
            m_labelFloat = m_dicoFloat.Values.ToList();
        }
        m_dicoFloat[label].PushValue(value);


    }


    [System.Serializable]
    public class LabelEventOnChangedBool : LabelEventOnChangedT<bool>
    {
        public LabelEventOnChangedBool()
        {
        }

        public LabelEventOnChangedBool(string labelId) : base(labelId)
        {
        }
    }

    [System.Serializable]
    public class LabelEventOnChangedFloat : LabelEventOnChangedT<float> {
        public LabelEventOnChangedFloat()
        {
        }

        public LabelEventOnChangedFloat(string labelId) : base(labelId)
        {
        }
    }


    [System.Serializable]
    public class LabelEventOnChangedT<T> {

        public string m_labelId;
        public T m_previousValue;
        public T m_currentValue;
        public LabelEventOnChangedT()
        {
        }

        public LabelEventOnChangedT(string labelId)
        {
            m_labelId = labelId;
        }

        public void PushValue(T value)
        {

            m_previousValue = m_currentValue;
            m_currentValue = value;
            if (!m_previousValue.Equals(m_currentValue)) {
                //Debug.Log("Changed: " + m_currentValue+ " "+m_labelId);
                m_onChanged.Invoke(value);
            }
        }
        public void SetValue(T value, bool sendNotification=false)
        {

            m_previousValue = value;
            m_currentValue = value;
            if (sendNotification)
                m_onChanged.Invoke(value);
        }
        public UnityEvent<T> m_onChanged;
    }

   
}
