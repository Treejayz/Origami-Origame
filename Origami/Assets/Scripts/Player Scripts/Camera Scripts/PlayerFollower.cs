﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollower : MonoBehaviour {

    public Transform target;

    // Update is called once per frame
    void Update() {
        transform.position = Vector3.Lerp(transform.position, target.position, 0.5f);
    }
}
