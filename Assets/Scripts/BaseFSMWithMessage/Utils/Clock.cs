using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    public static double m_currentTime;

    private void Start()
    {
        m_currentTime = 0;
    }

    private void Update()
    {
        m_currentTime += Time.deltaTime;
    }

    public static double GetCurrentTime()
    {
        return m_currentTime;
    }
}
