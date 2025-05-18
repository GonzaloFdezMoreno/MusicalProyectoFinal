﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
//using FMOD;

public class TimeManager : MonoBehaviour
{ 
    public bool IsFinite { get; private set; }
    public float TotalTime { get; private set; }
    public float TimeRemaining { get; private set; }
    public bool IsOver { get; private set; }

    private bool raceStarted;

    public static Action<float> OnAdjustTime;
    
    public static Action<int, bool, GameMode> OnSetTime;

    
    FMODUnity.StudioEventEmitter sound;
    [SerializeField]
    private GameObject refAud;
    [SerializeField]
    private bool mtra = false;
    [SerializeField]
    private float mtratime = 0;

    private void Awake()
    {
        IsFinite = false;
        TimeRemaining = TotalTime;
        Debug.Log(TotalTime);
    }

    private void Start()
    {
        if (refAud != null)
        {
            sound = refAud.GetComponent<FMODUnity.StudioEventEmitter>();
        }
    }


    void OnEnable()
    {
        OnAdjustTime += AdjustTime;
        OnSetTime += SetTime;
    }

    private void OnDisable()
    {
        OnAdjustTime -= AdjustTime;
        OnSetTime -= SetTime;
    }

    private void AdjustTime(float delta)
    {
        TimeRemaining += delta;
    }

    private void SetTime(int time, bool isFinite, GameMode gameMode)
    {
        TotalTime = time;
        IsFinite = isFinite;
        TimeRemaining = TotalTime;
    }

    void Update()
    {
        if (!raceStarted) return;
        
        if (IsFinite && !IsOver)
        {
            TimeRemaining -= Time.deltaTime;
            if (TimeRemaining <= 0)
            {
                TimeRemaining = 0;
                IsOver = true;
            }
        }
        if (mtra)
        {
            if(mtratime<30.0f)
                mtratime += Time.deltaTime/2;
            else
            {
                mtra = false;
            }
            
            if (sound != null)
            {
                sound.SetParameter("trantime", mtratime);
            }

        }
    }

    public void StartRace()
    {
        raceStarted = true;
    }

    public void StopRace() {
        raceStarted = false;
    }

    public void SetTransformation(bool state)
    {
        mtra = state;
    }
}

