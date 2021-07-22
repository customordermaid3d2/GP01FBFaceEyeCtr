using BepInEx;
using BepInEx.Configuration;
using COM3D2.LillyUtill;
using COM3D2API;
using HarmonyLib;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GP01FBFaceEyeCtr
{
    [BepInPlugin(MyAttribute.PLAGIN_FULL_NAME, MyAttribute.PLAGIN_FULL_NAME, MyAttribute.PLAGIN_VERSION)]// 버전 규칙 잇음. 반드시 2~4개의 숫자구성으로 해야함. 미준수시 못읽어들임
    //[BepInPlugin("COM3D2.Sample.Plugin", "COM3D2.Sample.Plugin", "21.6.6")]// 버전 규칙 잇음. 반드시 2~4개의 숫자구성으로 해야함. 미준수시 못읽어들임
    [BepInProcess("COM3D2x64.exe")]
    public class Sample : BaseUnityPlugin
    {
        // 단축키 설정파일로 연동
        //private ConfigEntry<BepInEx.Configuration.KeyboardShortcut> ShowCounter;

        Harmony harmony;

        public static Sample sample;

        public Sample()
        {
            sample = this;
            MyLog.log = BepInEx.Logging.Logger.CreateLogSource(MyAttribute.PLAGIN_NAME);

            UtillMPN.init();
        }

        /// <summary>
        ///  게임 실행시 한번만 실행됨
        /// </summary>
        public void Awake()
        {
            MyLog.LogMessage("Awake");

            // 단축키 기본값 설정
            //ShowCounter = Config.Bind("KeyboardShortcut", "KeyboardShortcut0", new BepInEx.Configuration.KeyboardShortcut(KeyCode.Alpha9, KeyCode.LeftControl));

            //SampleConfig.Install(MyLog.log);

            // 기어 메뉴 추가. 이 플러그인 기능 자체를 멈추려면 enabled 를 꺽어야함. 그러면 OnEnable(), OnDisable() 이 작동함
        }



        public void OnEnable()
        {
            MyLog.LogMessage("OnEnable");

            SceneManager.sceneLoaded += this.OnSceneLoaded;

            // 하모니 패치
            harmony = Harmony.CreateAndPatchAll(typeof(SamplePatch));

        }

        /// <summary>
        /// 게임 실행시 한번만 실행됨
        /// </summary>
        public void Start()
        {
            MyLog.LogMessage("Start");

            SampleGUI.Install(gameObject, Config);

            //SystemShortcutAPI.AddButton(MyAttribute.PLAGIN_FULL_NAME, new Action(delegate () { enabled = !enabled; }), MyAttribute.PLAGIN_NAME, MyUtill.ExtractResource(GP01FBFaceEyeCtr.Properties.Resources.icon));
        }

        public string scene_name = string.Empty;

        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            MyLog.LogMessage("OnSceneLoaded", scene.name, scene.buildIndex);
            //  scene.buildIndex 는 쓰지 말자 제발
            scene_name = scene.name;
        }

        public void FixedUpdate()
        {

        }

        public void Update()
        {
            //if (ShowCounter.Value.IsDown())
            //{
            //    MyLog.LogMessage("IsDown", ShowCounter.Value.Modifiers, ShowCounter.Value.MainKey);
            //}
            //if (ShowCounter.Value.IsPressed())
            //{
            //    MyLog.LogMessage("IsPressed", ShowCounter.Value.Modifiers, ShowCounter.Value.MainKey);
            //}
            //if (ShowCounter.Value.IsUp())
            //{
            //    MyLog.LogMessage("IsUp", ShowCounter.Value.Modifiers, ShowCounter.Value.MainKey);
            //}
        }

        public void LateUpdate()
        {

        }

        

        public void OnGUI()
        {
          
        }



        public void OnDisable()
        {
            MyLog.LogMessage("OnDisable");

            SceneManager.sceneLoaded -= this.OnSceneLoaded;

            harmony.UnpatchSelf();// ==harmony.UnpatchAll(harmony.Id);
            //harmony.UnpatchAll(); // 정대 사용 금지. 다름 플러그인이 패치한것까지 다 풀려버림
        }

        public void Pause()
        {
            MyLog.LogMessage("Pause");
        }

        public void Resume()
        {
            MyLog.LogMessage("Resume");
        }

        /// <summary>
        /// 게임 X 버튼 눌렀을때 반응
        /// </summary>
        public void OnApplicationQuit()
        {
            MyLog.LogMessage("OnApplicationQuit");
        }

    }
}
