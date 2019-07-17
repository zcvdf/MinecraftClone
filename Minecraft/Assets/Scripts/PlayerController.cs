﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController characterController;
    public ChunkManager chunkManager;
    public Transform feet;
    [Range(0.0f, 100.0f)] public float baseSpeed = 10.0f;
    [Range(0.0f, 2.0f)] public float waterSpeedModifier = 0.5f;
    public GameObject splashEffect;

    private float _speed;

    private Vector3Int _prevPosition;
    private BlockType _prevType;
    //private List<Collider> _waterColliders;

    void Start()
    {
        _speed = baseSpeed;
        //_waterColliders = new List<Collider>();

        if (characterController == null)
        {
            characterController = GetComponent<CharacterController>();
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            characterController.SimpleMove(this.transform.forward * _speed);
        } else if (Input.GetKey(KeyCode.S))
        {
            characterController.SimpleMove(-this.transform.forward * _speed);
        }

        if (Input.GetKey(KeyCode.D))
        {
            this.transform.Rotate(0f,90f*Time.deltaTime,0f);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            this.transform.Rotate(0f, -90f*Time.deltaTime, 0f);
        }

        Vector3Int currPosition = new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);

        if (currPosition != _prevPosition)
        {
            chunkManager.UpdateLoadedChunks();
            _prevPosition = currPosition;

            // Check if in water
            BlockType currentType = WorldChunk.GetBlockType(feet.position, chunkManager.chunkHeight);
            if (_prevType != currentType)
            {
                string blockTypeName = currentType == null ? "Air" : currentType.displayName;
                Debug.Log(blockTypeName);

                if (currentType != null && currentType.displayName == "Water")
                {
                    _speed = baseSpeed * waterSpeedModifier; // Entering water
                    GameObject effect = Instantiate(splashEffect, null) as GameObject;
                    effect.transform.position = feet.position;
                    Destroy(effect, 1.5f);
                }
                else if (_prevType != null && _prevType.displayName == "Water")
                {
                    _speed = baseSpeed; // Exiting water
                }

                _prevType = currentType;
            }

        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Water"))
    //    {
    //        _waterColliders.Add(other);
    //        _speed = baseSpeed * waterSpeedModifier;
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Water"))
    //    {
    //        _waterColliders.Remove(other);
    //        if (_waterColliders.Count == 0)
    //        {
    //            _speed = baseSpeed; // Left all water, return to normal speed
    //        }
       
    //    }
    //}
}
