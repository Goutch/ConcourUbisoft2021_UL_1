using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TechSupport.Surveillance
{
    public enum SurveillanceMode
    {
        Grid,
        Focused
    }
    public class SurveillanceSystem : MonoBehaviour
    {
        [SerializeField] private SurveillanceMode mode = SurveillanceMode.Grid; // Default mode : grid
        private readonly GridSystem _gridSystem = new GridSystem();
        private readonly FullScreenSystem _fullScreenSystem = new FullScreenSystem();

        private IEnumerable<SurveillanceCamera> _cameras = new List<SurveillanceCamera>();

        #region Callbacks

        public Action OnModeSwitched;

        public readonly IDictionary<SurveillanceMode, Action> _onSwitchMethods;
        public readonly IDictionary<SurveillanceMode, Action> _exitMethods;

        public SurveillanceSystem()
        {
            _onSwitchMethods
                = new Dictionary<SurveillanceMode, Action>()
                {
                    { SurveillanceMode.Focused, OnFullScreen },
                    { SurveillanceMode.Grid, OnGrid },
                };
            _exitMethods
                = new Dictionary<SurveillanceMode, Action>()
                {
                    { SurveillanceMode.Focused, ExitFullScreen },
                    { SurveillanceMode.Grid, ExitGrid },
                };
        }

        #endregion

        private void Awake()
        {
            _cameras = FindObjectsOfType<SurveillanceCamera>();
            foreach (SurveillanceCamera camera in _cameras)
            {
                camera.Init(); 
            }
            _gridSystem.SearchGridSize(_cameras.Count());
            _fullScreenSystem.SetTarget(_cameras.First().GetCamera());
            SystemSwitch(mode);
        }

        // TODO: Improve this basic input system
        private void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (mode == SurveillanceMode.Grid)
                {
                    SystemSwitch(SurveillanceMode.Focused);
                }
            } else if (Input.GetButtonUp("Cancel"))
            {
                if (mode == SurveillanceMode.Focused)
                {
                    SystemSwitch(SurveillanceMode.Grid);
                }
            }
        }

        #region Camera

        private void EnableAll(bool enabledCamera)
        {
            foreach (SurveillanceCamera cam in _cameras)
            {
                cam.Enable(enabledCamera);
            }
        }

        #endregion

        #region General System

        private void SystemSwitch(SurveillanceMode newMode)
        {
            if (mode != newMode)
            {
                _exitMethods[mode]?.Invoke();
            }
            _onSwitchMethods[mode = newMode]?.Invoke();
            OnModeSwitched?.Invoke();
        }

        private void ExitFullScreen()
        {
            _fullScreenSystem.EscapeFullScreen();
        }

        private void ExitGrid()
        {
            SurveillanceCamera selected =
                _cameras.First(surveillanceCamera => surveillanceCamera.Contains(Input.mousePosition));
            if (selected != null)
            {
                _fullScreenSystem.SetTarget(selected.GetCamera());
            }
        }

        private void OnGrid()
        {
            EnableAll(true);
            _gridSystem.Grid(_cameras.Select(input => input.GetCamera()),
                _gridSystem.GetGridSize());
        }

        private void OnFullScreen()
        {
            EnableAll(false);
            _fullScreenSystem.RenderFullScreen();
        }

        #endregion
    }
}