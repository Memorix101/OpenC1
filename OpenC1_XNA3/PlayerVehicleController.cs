using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;
using OneAmEngine;

namespace OpenC1
{
    class PlayerVehicleController
    {
        public static bool ForceBrake { get; set; }

        public static float Acceleration
        {
            get
            {
                if (ForceBrake)
                    return 0;

                if (GameEngine.Input.IsKeyDown(Keys.Up))
                    return 1.0f;

                return GameEngine.Input.GamePadState.Triggers.Right;
            }
        }

        public static float Brake
        {
            get
            {
                if (ForceBrake)
                    return 0f;

                if (GameEngine.Input.IsKeyDown(Keys.Down))
                    return 1.0f;

                return GameEngine.Input.GamePadState.Triggers.Left;
            }
        }

        public static float Turn
        {
            get
            {
                if (GameEngine.Input.IsKeyDown(Keys.Left))
                    return -1;
                else if (GameEngine.Input.IsKeyDown(Keys.Right))
                    return 1;

                return GameEngine.Input.GamePadState.ThumbSticks.Left.X;
            }
        }

        /*public static bool ChangeView
        {
            get
            {
                if (GameEngine.Input.WasPressed(Keys.C))
                    return true;
                if (GameEngine.Input.WasPressed(Buttons.DPadUp))
                    return true;
                return false;
            }
        }*/

        public static bool GearUp
        {
            get
            {
                if (GameEngine.Input.WasPressed(Keys.A))
                    return true;
                if (GameEngine.Input.WasPressed(Buttons.RightShoulder))
                    return true;
                return false;
            }
        }

        public static bool GearDown
        {
            get
            {
                if (GameEngine.Input.WasPressed(Keys.Z) || GameEngine.Input.WasPressed(Keys.Y)) //QWERTZ
                    return true;
                if (GameEngine.Input.WasPressed(Buttons.LeftShoulder))
                    return true;
                return false;
            }
        }

        public static bool Handbrake
        {
            get
            {
                if (ForceBrake) return true;
                return GameEngine.Input.IsKeyDown(Keys.Space) || GameEngine.Input.IsButtonDown(Buttons.X);
            }
        }
    }
}