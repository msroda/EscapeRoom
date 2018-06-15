﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WardrobePickup : MonoBehaviour {

    public float m_maxHeight;
    public float m_minHeight;

    [SerializeField] private float m_minInteractHeight;
    [SerializeField] private GameObject m_objectUnder;


    void Update ()
    {
        this.transform.position = new Vector3(this.transform.position.x, Mathf.Max(m_minHeight, Mathf.Min(m_maxHeight, this.transform.position.y)), this.transform.position.z);

        m_objectUnder.SetActive(this.transform.position.y >= m_minInteractHeight);
	}

    public void SetMinHeight(float height)
    {
        m_minHeight = height;
    }

    public void SetMaxHeight(float height)
    {
        m_maxHeight = height;
    }
}