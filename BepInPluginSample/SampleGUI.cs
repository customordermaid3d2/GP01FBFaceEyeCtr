using BepInEx;
using BepInEx.Configuration;
using COM3D2.LillyUtill;
using COM3D2API;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GP01FBFaceEyeCtr
{
    class SampleGUI : MonoBehaviour
    {
        public static SampleGUI instance;

        private static ConfigFile config;

        private static ConfigEntry<BepInEx.Configuration.KeyboardShortcut> ShowCounter;

        private static int windowId = new System.Random().Next();

        private static Vector2 scrollPosition;

        // 위치 저장용 테스트 json
        public static MyWindowRect myWindowRect;

        public static bool IsOpen
        {
            get => myWindowRect.IsOpen;
            set => myWindowRect.IsOpen = value;
        }

        // GUI ON OFF 설정파일로 저장
        private static ConfigEntry<bool> IsGUIOn;

        public static bool isGUIOn
        {
            get => IsGUIOn.Value;
            set => IsGUIOn.Value = value;
        }

        private static ConfigEntry<bool> IsAllMaid;

        public static bool isAllMaid
        {
            get => IsAllMaid.Value;
            set => IsAllMaid.Value = value;
        }

        Harmony harmony;


        internal static SampleGUI Install(GameObject parent, ConfigFile config)
        {
            SampleGUI.config = config;
            instance = parent.GetComponent<SampleGUI>();
            if (instance == null)
            {
                instance = parent.AddComponent<SampleGUI>();
                Sample.myLog.LogMessage("GameObjectMgr.Install", instance.name);
            }
            return instance;
        }

        public void Awake()
        {
            myWindowRect = new MyWindowRect(config, MyAttribute.PLAGIN_FULL_NAME, MyAttribute.PLAGIN_NAME, "GP01FB");
            IsGUIOn = config.Bind("GUI", "isGUIOn", false);
            IsAllMaid = config.Bind("GUI", "IsAllMaid", false);
            ShowCounter = config.Bind("GUI", "isGUIOnKey", new BepInEx.Configuration.KeyboardShortcut(KeyCode.Alpha9, KeyCode.LeftControl));
            //SystemShortcutAPI.AddButton(MyAttribute.PLAGIN_FULL_NAME, new Action(delegate () { SampleGUI.isGUIOn = !SampleGUI.isGUIOn; }), MyAttribute.PLAGIN_NAME + " : " + ShowCounter.Value.ToString(), MyUtill.ExtractResource(GP01FBFaceEyeCtr.Properties.Resources.icon));
            SystemShortcutAPI.AddButton(MyAttribute.PLAGIN_FULL_NAME, new Action(delegate () { enabled = !enabled; }), MyAttribute.PLAGIN_NAME + " : " + ShowCounter.Value.ToString(), MyUtill.ExtractResource(GP01FBFaceEyeCtr.Properties.Resources.icon));
            MaidActivePatch.selectionGrid+= UtillMPN.UpdateMPNs;
        }

        public void OnEnable()
        {
            Sample.myLog.LogMessage("OnEnable");

            SampleGUI.myWindowRect.load();
            SceneManager.sceneLoaded += this.OnSceneLoaded;
            harmony = Harmony.CreateAndPatchAll(typeof(SamplePatch));           
        }

        public void Start()
        {
            Sample.myLog.LogMessage("Start");
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SampleGUI.myWindowRect.save();
        }

        private void Update()
        {
            //if (ShowCounter.Value.IsDown())
            //{
            //    MyLog.LogMessage("IsDown", ShowCounter.Value.Modifiers, ShowCounter.Value.MainKey);
            //}
            //if (ShowCounter.Value.IsPressed())
            //{
            //    MyLog.LogMessage("IsPressed", ShowCounter.Value.Modifiers, ShowCounter.Value.MainKey);
            //}
            if (ShowCounter.Value.IsUp())
            {
                isGUIOn = !isGUIOn;
                Sample.myLog.LogMessage("IsUp", ShowCounter.Value.Modifiers, ShowCounter.Value.MainKey);
            }
        }

        public void OnGUI()
        {
            if (!isGUIOn)
                return;

            //GUI.skin.window = ;

            //myWindowRect.WindowRect = GUILayout.Window(windowId, myWindowRect.WindowRect, WindowFunction, MyAttribute.PLAGIN_NAME + " " + ShowCounter.Value.ToString(), GUI.skin.box);
            //GUI.skin.box.
            myWindowRect.WindowRect = GUILayout.Window(windowId, myWindowRect.WindowRect, WindowFunction, "", GUI.skin.box);
        }



        public void WindowFunction(int id)
        {
            GUI.enabled = true;

            GUILayout.BeginHorizontal();
            GUILayout.Label(myWindowRect.windowName, GUILayout.Height(20));
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("-", GUILayout.Width(20), GUILayout.Height(20))) { IsOpen = !IsOpen; }
            if (GUILayout.Button("x", GUILayout.Width(20), GUILayout.Height(20))) { isGUIOn = false; }
            GUILayout.EndHorizontal();

            if (!IsOpen)
            {

            }
            else
            {
                scrollPosition = GUILayout.BeginScrollView(scrollPosition);




                GUILayout.Label("folder select");
                seletedfolderbak = GUILayout.SelectionGrid(seletedfolder, UtillMPN.foldersNm, 3);

                if (GUI.changed)
                {
                    if (seletedfolder != seletedfolderbak)
                    {
                        seletedfolder = seletedfolderbak;
                        UtillMPN.SetFolderMPNs((UtillMPN.folder)seletedfolder);
                        UtillMPN.UpdateMPNs();
                    }
                    GUI.changed = false;
                }

                #region 슬라이드

                GUI.enabled = MaidActivePatch.GetMaid(SampleGUI.seleted) != null;

                for (int i = 0; i < UtillMPN.nowMPNs.Length; i++)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(UtillMPN.nowMPNnm[i]);
                    GUILayout.FlexibleSpace();
                    UtillMPN.nowBools[i] = GUILayout.Toggle(UtillMPN.nowBools[i], "All Maid Aplly");
                    GUILayout.EndHorizontal();
                    UtillMPN.nowMPNvb[i] = GUILayout.HorizontalSlider(UtillMPN.nowMPNv[i], UtillMPN.nowMPNmin[i], UtillMPN.nowMPNmax[i]);
                }

                if (GUI.changed)
                {
                    //if (SamplePatch.GetMaid(SampleGUI.seleted) != null)
                    {

                        for (int i = 0; i < UtillMPN.nowMPNs.Length; i++)
                        {
                            if (UtillMPN.nowMPNv[i] == UtillMPN.nowMPNvb[i])
                            {
                                continue;
                            }
                            if (isAllMaid)
                            {
                                foreach (var item in MaidActivePatch.Maids2.Values)
                                {
                                    item?.SetProp(UtillMPN.nowMPNs[i], (int)(UtillMPN.nowMPNv[i] = (int)UtillMPN.nowMPNvb[i]));
                                }
                            }
                            else
                            {
                                MaidActivePatch.GetMaid(SampleGUI.seleted)?.SetProp(UtillMPN.nowMPNs[i], (int)(UtillMPN.nowMPNv[i] = (int)UtillMPN.nowMPNvb[i]));
                            }
                            //MyLog.LogMessage("changed", mpns[i], mpni[i]);
                        }
                        if (isAllMaid)
                        {
                            foreach (var item in MaidActivePatch.Maids2.Values)
                            {
                                item?.AllProcProp();
                            }
                        }
                        else
                        {
                            MaidActivePatch.GetMaid(SampleGUI.seleted)?.AllProcProp();
                        }
                        SceneEdit.Instance?.UpdateSliders();
                    }
                    GUI.changed = false;
                }

                GUI.enabled = true;

                #endregion

                #region 메이드

                GUILayout.BeginHorizontal();

                isAllMaid = GUILayout.Toggle(isAllMaid, "All Maid Aplly");
                if (GUILayout.Button("Copy All") && MaidActivePatch.GetMaid(SampleGUI.seleted) != null)
                {
                    var ms = MaidActivePatch.Maids2.Values;
                    for (int i = 0; i < UtillMPN.nowMPNs.Length; i++)
                    {
                        if (UtillMPN.nowBools[i])
                        {
                            var m = MaidActivePatch.GetMaid(SampleGUI.seleted).GetProp(UtillMPN.nowMPNs[i]);
                            foreach (var item in ms)
                            {
                                item?.SetProp(UtillMPN.nowMPNs[i], m.value);
                            }
                        }
                        foreach (var item in ms)
                        {
                            item?.AllProcProp();
                        }
                    }
                }

                GUILayout.EndHorizontal();


                seleted = MaidActivePatch.SelectionGrid(seleted,3, 265 ,false);
                /*
                GUILayout.Label("maid select");
                // 여기는 출력된 메이드들 이름만 가져옴
                // seleted 가 이름 위치 번호만 가져온건데
                seleted = GUILayout.SelectionGrid(seleted, MaidActivePatch.maidNames, 1);

                if (GUI.changed)
                {
                    UtillMPN.UpdateMPNs();
                    GUI.changed = false;
                }
                */
                #endregion

                GUILayout.EndScrollView();



            }
            GUI.enabled = true;
            GUI.DragWindow(); // 창 드레그 가능하게 해줌. 마지막에만 넣어야함
        }



        public void OnDisable()
        {

            SampleGUI.myWindowRect?.save();
            SceneManager.sceneLoaded -= this.OnSceneLoaded;
            harmony.UnpatchSelf();
        }


        public static int seleted;
        public static int seletedfolder;
        public static int seletedfolderbak;



    }
}
