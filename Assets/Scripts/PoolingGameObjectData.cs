using UnityEngine;

public class PoolingGameObjectData
{
    public GameObject m_gameObject
    {
        get;
        set;
    }
    public bool m_isUsing;

    public enum PoolKey {snowman};

}
