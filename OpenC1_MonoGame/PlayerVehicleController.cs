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

                if (OneAmEngine.Engine.Input.IsKeyDown(Keys.Up))
                    return 1.0f;

                return OneAmEngine.Engine.Input.GamePadState.Triggers.Right;
            }
        }

        public static float Brake
        {
            get
            {
                if (ForceBrake)
                    return 0f;

                if (OneAmEngine.Engine.Input.IsKeyDown(Keys.Down))
                    return 1.0f;

                return OneAmEngine.Engine.Input.GamePadState.Triggers.Left;
            }
        }

        public static float Turn
        {
            get
            {
                if (OneAmEngine.Engine.Input.IsKeyDown(Keys.Left))
                    return -1;
                else if (OneAmEngine.Engine.Input.IsKeyDown(Keys.Right))
                    return 1;

                return OneAmEngine.Engine.Input.GamePadState.ThumbSticks.Left.X;
            }
        }

        public static bool ChangeView
        {
            get
            {
                if (OneAmEngine.Engine.Input.WasPressed(Keys.C))
                    return true;
                if (OneAmEngine.Engine.Input.WasPressed(Buttons.RightShoulder))
                    return true;
                return false;
            }
        }

        public static bool GearUp
        {
            get
            {
                if (OneAmEngine.Engine.Input.WasPressed(Keys.A))
                    return true;
                if (OneAmEngine.Engine.Input.WasPressed(Buttons.B))
                    return true;
                return false;
            }
        }

        public static bool GearDown
        {
            get
            {
                if (OneAmEngine.Engine.Input.WasPressed(Keys.Z))
                    return true;
                if (OneAmEngine.Engine.Input.WasPressed(Buttons.X))
                    return true;
                return false;
            }
        }

        public static bool Handbrake
        {
            get
            {
                if (ForceBrake) return true;
                return OneAmEngine.Engine.Input.IsKeyDown(Keys.Space);
            }
        }
    }
}
