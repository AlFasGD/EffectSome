namespace EffectSome
{
    class Objects
    {
        public enum Trigger
        {
            None = 0,
            BG = 29,
            GRND = 30,
            EnableTrail = 32,
            DisableTrail = 33,
            Obj = 105,
            ThreeDL = 744,
            Color = 899,
            GRND2 = 900,
            Move = 901,
            Line = 915,
            Pulse = 1006,
            Alpha = 1007,
            Toggle = 1049,
            Spawn = 1268,
            Rotate = 1346,
            Follow = 1347,
            Shake = 1520,
            Animate = 1585,
            Touch = 1595,
            Count = 1611,
            HidePlayer = 1612,
            ShowPlayer = 1613,
            Stop = 1616,
            InstantCount = 1811,
            OnDeath = 1812,
            FollowPlayerY = 1814,
            Collision = 1815,
            Pickup = 1817,
            BGEffectOn = 1818,
            BGEffectOff = 1819,
            // Not implemented yet - Wait for 2.2 to bring all that shit
            Zoom = -1,
            StaticCamera = -1,
            CameraOffset = -1,
            Scale = -1,
            Random = -1,
            End = -1,
        }
        // TOOD: Work on that shit?
        public enum Object
        {
            StandardBlock = 1,
            GridBlock1 = 2,
            GridBlock2 = 3,
            StandardBlockSlope45 = 371,
            StandardBlockSlope45Cornerpiece = 372,
            StandardBlockSlope26 = 373,
            StandardBlockSlope26Cornerpiece = 374
        }
    }
}
