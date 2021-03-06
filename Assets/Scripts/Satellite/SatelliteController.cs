﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Satellite
{
    public class SatelliteController : MonoBehaviour
    {
        public float speed;
        public float angularSpeed;

        public Satellite satellite;
        private List<SatelliteEngine>[] _satelliteEngineStageLists = new List<SatelliteEngine>[5];
        private int curEngineStage = 0;
        private int engineStages = 0;
        

        private void Start()
        {
            GenerateEngineStageList();
        }

        private void FixedUpdate()
        {
            Rotate();
            Push();
        }


        private void GenerateEngineStageList()
        {
            foreach (SatellitePart satellitePart in satellite.satelliteParts)
            {
                if (satellitePart.PartType == SatelliteType.Engine)
                {
                    SatelliteEngine engine = (SatelliteEngine) satellitePart;
                    Debug.Log("engine stage:" + engine.engineStage);
                    if (_satelliteEngineStageLists[engine.engineStage] == null)
                    {
                        _satelliteEngineStageLists[engine.engineStage] = new List<SatelliteEngine>();
                        engineStages++;
                    }
                    _satelliteEngineStageLists[engine.engineStage].Add(engine);
                    
                }
            }

            Debug.Log("Rocket stages:" + engineStages);
            _satelliteEngineStageLists[engineStages]= new List<SatelliteEngine>();
            _satelliteEngineStageLists[engineStages].Add(satellite.satelliteCore);
        }

        private void Rotate()
        {
            if (Input.GetKey(KeyCode.A))
            {
                _satelliteEngineStageLists[curEngineStage].ForEach((engine => engine.Rotate(-engine.transform.right * angularSpeed)));

                // this._satellitePart.Rotate(-this.transform.up * angularSpeed);
            }

            if (Input.GetKey(KeyCode.D))
            {
                _satelliteEngineStageLists[curEngineStage].ForEach((engine => engine.Rotate(engine.transform.right  * angularSpeed)));

                // this._satellitePart.Rotate(this.transform.up  * angularSpeed);
            }
            
        }

        private void Push()
        {
            if (Input.GetKey(KeyCode.W))
            {
                _satelliteEngineStageLists[curEngineStage].ForEach((engine => engine.Push(engine.transform.up * speed)));
            }

            // if (Input.GetKey(KeyCode.S))
            // {
            //     this._satellitePart.Push(-this.transform.forward * speed);
            // }
        }

        public void SeparateControl()
        {
            if(curEngineStage < engineStages)
                Separate();
        }
        private void Separate()
        {
            //是否到达核心层
            if (curEngineStage == engineStages - 1)
            {
                _satelliteEngineStageLists[curEngineStage++].ForEach((engine => engine.Separate(true)));
                _satelliteEngineStageLists[curEngineStage].ForEach((engine => engine.Separate(false)));
            }
            else
            {
                _satelliteEngineStageLists[curEngineStage++].ForEach((engine => engine.Separate(true)));
                _satelliteEngineStageLists[curEngineStage].ForEach((engine => engine.Separate(true)));
            }

            Debug.Log("Now in stage:" + curEngineStage);
        }
    }
}
