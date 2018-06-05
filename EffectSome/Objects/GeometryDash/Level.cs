using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EffectSome
{
    public class Level
    {
        #region Properties
        public string LevelNameWithRevision => $"{LevelName}{(LevelRevision > 0 ? $" (Rev. {LevelRevision})" : "")}";
        public string LevelName;
        public string LevelString;
        public string DecryptedLevelString;
        public string LevelGuidelinesString
        {
            get => levelGuidelinesString;
            set
            {
                levelGuidelines = null; // Reset and only analyze if requested
                levelGuidelinesString = value;
            }
        }
        public string LevelDescription = "";
        public string RawLevel;
        public int LevelRevision;
        public int LevelOfficialSongID;
        public int LevelCustomSongID;
        public int LevelObjectCount
        {
            get
            {
                if (levelObjectCount == -1)
                {
                    levelObjectCount = 0;
                    if (ObjectCounts != null)
                    {
                        foreach (var kvp in ObjectCounts)
                            levelObjectCount += kvp.Value;
                        levelObjectCount -= ObjectCounts.ValueOrDefault((int)LevelObject.Trigger.StartPos);
                    }
                }
                return levelObjectCount;
            }
        }
        public int LevelTriggerCount
        {
            get
            {
                if (levelTriggerCount == -1)
                {
                    levelTriggerCount = 0;
                    if (ObjectCounts != null)
                    {
                        foreach (var kvp in ObjectCounts)
                            if (Enum.GetValues(typeof(LevelObject.Trigger)).Cast<int>().Contains(kvp.Key))
                                levelTriggerCount += kvp.Value;
                        levelTriggerCount -= ObjectCounts.ValueOrDefault((int)LevelObject.Trigger.StartPos);
                    }
                }
                return levelTriggerCount;
            }
        }
        public int LevelAttempts;
        public int LevelID;
        public int LevelVersion;
        public int LevelLength;
        public int LevelFolder;
        public int BuildTime; // This is in seconds
        public TimeSpan TotalBuildTime
        {
            get => new TimeSpan(0, 0, BuildTime);
            set => BuildTime = (int)value.TotalSeconds;
        }
        public bool LevelVerifiedStatus;
        public bool LevelUploadedStatus;
        public List<LevelObject> LevelObjects
        {
            get => levelObjects;
            set
            {
                levelObjects = value;
                levelObjectCount = -1;
                levelTriggerCount = -1;
                colorTriggerCount = -1;
            }
        }
        public List<Guideline> LevelGuidelines
        {
            get
            {
                if (levelGuidelines == null)
                    levelGuidelines = Gamesave.GetGuidelines(LevelGuidelinesString);
                return levelGuidelines;
            }
            set
            {
                levelGuidelines = value;
                LevelGuidelinesString = GetGuidelineString(levelGuidelines);
            }
        }
        public int[] LevelDifferentObjectIDs = new int[0];
        public int[] LevelUsedGroupIDs = new int[0];
        public Dictionary<int, int> ObjectCounts;
        #region Trigger info
        public int MoveTriggersCount => ObjectCounts.ValueOrDefault((int)LevelObject.Trigger.Move);
        public int StopTriggersCount => ObjectCounts.ValueOrDefault((int)LevelObject.Trigger.Stop);
        public int PulseTriggersCount => ObjectCounts.ValueOrDefault((int)LevelObject.Trigger.Pulse);
        public int AlphaTriggersCount => ObjectCounts.ValueOrDefault((int)LevelObject.Trigger.Alpha);
        public int ToggleTriggersCount => ObjectCounts.ValueOrDefault((int)LevelObject.Trigger.Toggle);
        public int SpawnTriggersCount => ObjectCounts.ValueOrDefault((int)LevelObject.Trigger.Spawn);
        public int CountTriggersCount => ObjectCounts.ValueOrDefault((int)LevelObject.Trigger.Count);
        public int InstantCountTriggersCount => ObjectCounts.ValueOrDefault((int)LevelObject.Trigger.InstantCount);
        public int PickupTriggersCount => ObjectCounts.ValueOrDefault((int)LevelObject.Trigger.Pickup);
        public int FollowTriggersCount => ObjectCounts.ValueOrDefault((int)LevelObject.Trigger.Follow);
        public int FollowPlayerYTriggersCount => ObjectCounts.ValueOrDefault((int)LevelObject.Trigger.FollowPlayerY);
        public int TouchTriggersCount => ObjectCounts.ValueOrDefault((int)LevelObject.Trigger.Touch);
        public int AnimateTriggersCount => ObjectCounts.ValueOrDefault((int)LevelObject.Trigger.Animate);
        public int RotateTriggersCount => ObjectCounts.ValueOrDefault((int)LevelObject.Trigger.Rotate);
        public int ShakeTriggersCount => ObjectCounts.ValueOrDefault((int)LevelObject.Trigger.Shake);
        public int CollisionTriggersCount => ObjectCounts.ValueOrDefault((int)LevelObject.Trigger.Collision);
        public int OnDeathTriggersCount => ObjectCounts.ValueOrDefault((int)LevelObject.Trigger.OnDeath);
        public int ColorTriggersCount
        {
            get
            {
                if (colorTriggerCount == -1)
                {
                    colorTriggerCount = ObjectCounts.ValueOrDefault((int)LevelObject.Trigger.Color);
                    colorTriggerCount += ObjectCounts.ValueOrDefault((int)LevelObject.Trigger.BG);
                    colorTriggerCount += ObjectCounts.ValueOrDefault((int)LevelObject.Trigger.GRND);
                    colorTriggerCount += ObjectCounts.ValueOrDefault((int)LevelObject.Trigger.GRND2);
                    colorTriggerCount += ObjectCounts.ValueOrDefault((int)LevelObject.Trigger.Line);
                    colorTriggerCount += ObjectCounts.ValueOrDefault((int)LevelObject.Trigger.Obj);
                    colorTriggerCount += ObjectCounts.ValueOrDefault((int)LevelObject.Trigger.ThreeDL);
                    colorTriggerCount += ObjectCounts.ValueOrDefault((int)LevelObject.Trigger.Color1);
                    colorTriggerCount += ObjectCounts.ValueOrDefault((int)LevelObject.Trigger.Color2);
                    colorTriggerCount += ObjectCounts.ValueOrDefault((int)LevelObject.Trigger.Color3);
                    colorTriggerCount += ObjectCounts.ValueOrDefault((int)LevelObject.Trigger.Color4);
                }
                return colorTriggerCount;
            }
        }
        public int HidePlayerTriggersCount => ObjectCounts.ValueOrDefault((int)LevelObject.Trigger.HidePlayer);
        public int ShowPlayerTriggersCount => ObjectCounts.ValueOrDefault((int)LevelObject.Trigger.ShowPlayer);
        public int DisableTrailTriggersCount => ObjectCounts.ValueOrDefault((int)LevelObject.Trigger.DisableTrail);
        public int EnableTrailTriggersCount => ObjectCounts.ValueOrDefault((int)LevelObject.Trigger.EnableTrail);
        public int BGEffectOnTriggersCount => ObjectCounts.ValueOrDefault((int)LevelObject.Trigger.BGEffectOn);
        public int BGEffectOffTriggersCount => ObjectCounts.ValueOrDefault((int)LevelObject.Trigger.BGEffectOff);
        #endregion
        private int levelObjectCount = -1;
        private int levelTriggerCount = -1;
        private int colorTriggerCount = -1;
        private List<LevelObject> levelObjects;
        private List<Guideline> levelGuidelines;
        private string levelGuidelinesString;
        #endregion

        #region Constructors
        /// <summary>Creates a new empty instance of the <see cref="Level"/> class.</summary>
        public Level() { }
        /// <summary>Creates a new instance of the <see cref="Level"/> class from a raw string containing a level without getting its info.</summary>
        /// <param name="level">The raw string containing the level.</param>
        public Level(string level)
        {
            RawLevel = level;
        }
        #endregion

        #region Functions
        public static string GetGuidelineString(List<Guideline> guidelines)
        {
            StringBuilder result = new StringBuilder();
            foreach (var g in guidelines)
                result.Append(g.ToString() + "~");
            return result.ToString();
        }
        #endregion
    }
}