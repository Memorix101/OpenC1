using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using OneAmEngine;

namespace OpenC1.CameraViews
{
    class FlyView : ICameraView
    {
        FPSCamera _camera;
        Vehicle _vehicle;

        public FlyView(Vehicle vehicle)
        {
            _vehicle = vehicle;
            _camera = new FPSCamera();
            _camera.SetPerspective(55.55f, GameEngine.AspectRatio, 1, 500);
        }

        #region ICameraView Members

        public bool Selectable
        {
            get { return true; }
        }

        public void Update()
        {
            
        }

        public void Render()
        {
            //_vehicle.Render();
            GameEngine.Camera = _camera;
        }

        public void Activate()
        {
            GameEngine.Camera = _camera;
            _camera.Position = new Vector3(-968.4f, -17.04f, 197.64f); // _vehicle.Position;
        }

        public void Deactivate()
        {
            
        }

        #endregion
    }
}
