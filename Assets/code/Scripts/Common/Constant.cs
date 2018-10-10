namespace Assets.code.Scripts.Common
{
    public static class Constant
    {
        public static class EventName
        {
            public static readonly string HERO_HPUPDATE = "HERO_HPUPDATE";
            public static readonly string UI_HPUPDATE = "UI_HPUPDATE";
            public static readonly string SWITCH_WEAPON = "SWITCH_WEAPON";
            public static readonly string CREATER_ZOMBIE = "CREATER_ZOMBIE";
            public static readonly string SCENE_SWITCH = "SCENE_SWITCH";
            public static readonly string HERO_ATTACK = "HERO_ATTACK";
        }

        public static class ZombieAniTime
        {
            public static readonly float ATTACK_TIME = 4.0f;
            public static readonly float HIT_TIME = 2.0f;
            public static readonly float FLY_TIME = 2.0f;
            public static readonly float DEATH_TIME = 2.25f;
            public static readonly float MIN_HARM_TIME = 1.0f;
            public static readonly float MAX_HARM_TIME = 3.0f;
        }

        public static class HeroAniTime
        {
            public static readonly float MIN_ATTACK_TIME = 0.15f;
            public static readonly float MAX_ATTACK_TIME = 1.07f;
            public static readonly float WEAPON_SWITCH_TIME = 0.7f;
        }

        public static class ScenesName
        {
            public static readonly string LOGIN = "Login";
            public static readonly string MENU = "Menu";
            public static readonly string LEVEL_1 = "Level1";
            public static readonly string LEVEL_2 = "Level2";
            public static readonly string GAME_OVER = "GameOver";
        }
    }
}
