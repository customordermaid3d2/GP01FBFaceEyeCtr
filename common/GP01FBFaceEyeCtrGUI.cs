using BepInEx.Configuration;
using COM3D2API;
using HarmonyLib;
using LillyUtill.MyMaidActive;
using LillyUtill.MyWindowRect;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using UnityEngine;

namespace COM3D2.GP01FBFaceEyeCtr
{
    class GP01FBFaceEyeCtrGUI : MonoBehaviour
    {
        public static ConfigEntry<bool> isEnabled;

        public static GP01FBFaceEyeCtrGUI instance;

        private static ConfigFile config;

        public static ConfigEntry<BepInEx.Configuration.KeyboardShortcut> ShowCounter;

        private static int windowId = new System.Random().Next();

        private static Vector2 scrollPosition;

        // 위치 저장용 테스트 json
        public static WindowRectUtill myWindowRect;
        /*
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
        */

        private static ConfigEntry<bool> IsAllMaid;

        public static bool isAllMaid
        {
            get => IsAllMaid.Value;
            set => IsAllMaid.Value = value;
        }
        Harmony harmony;


        internal static GP01FBFaceEyeCtrGUI Install(GameObject parent, ConfigFile config)
        {
            GP01FBFaceEyeCtrGUI.config = config;
            instance = parent.GetComponent<GP01FBFaceEyeCtrGUI>();
            if (instance == null)
            {
                instance = parent.AddComponent<GP01FBFaceEyeCtrGUI>();
                //GP01FBFaceEyeCtr.myLog.LogMessage("GP01FBFaceEyeCtrGUI.Install", instance.name);
            }
            return instance;
        }

        public void Awake()
        {
            myWindowRect = new WindowRectUtill(config, MyAttribute.PLAGIN_FULL_NAME, MyAttribute.PLAGIN_NAME, "GP");
            isEnabled = config.Bind("Plugin", "isEnabled", true);
            isEnabled.SettingChanged += isEnabledChg;
            //IsGUIOn = config.Bind("GUI", "isGUIOn", false);
            IsAllMaid = config.Bind("GUI", "IsAllMaid", false);
            ShowCounter = config.Bind("GUI", "isGUIOnKey", new BepInEx.Configuration.KeyboardShortcut(KeyCode.Alpha9, KeyCode.LeftControl));

            MaidActiveUtill.setActiveMaidNum += MaidActivePatch_setActiveMaid;
            //MaidActivePatch.selectionGrid2+= UtillMPN.UpdateMPNs;
        }

        private void MaidActivePatch_setActiveMaid(int maid)
        {
            UtillMPN.UpdateMPNs(maid);
        }

        private void isEnabledChg(object sender, EventArgs e)
        {
            enabled = (bool)((SettingChangedEventArgs)e).ChangedSetting.BoxedValue ;
            //GP01FBFaceEyeCtr.myLog.LogMessage("isEnabledChg"
            //    ,((SettingChangedEventArgs)e).ChangedSetting.DefaultValue                
            //    ,((SettingChangedEventArgs)e).ChangedSetting.BoxedValue                
            //    ,((SettingChangedEventArgs)e).ChangedSetting.SettingType                
            //    );
        }

        public void OnEnable()
        {
            GP01FBFaceEyeCtr.myLog.LogMessage("OnEnable");
                       
            
            harmony = Harmony.CreateAndPatchAll(typeof(GP01FBFaceEyeCtrPatch));           
        }

        public void Start()
        {
            GP01FBFaceEyeCtr.myLog.LogMessage("Start");
            SystemShortcutAPI.AddButton(MyAttribute.PLAGIN_FULL_NAME, new Action(delegate () { myWindowRect.IsGUIOn = !myWindowRect.IsGUIOn; }), MyAttribute.PLAGIN_NAME + " : " + GP01FBFaceEyeCtrGUI.ShowCounter.Value.ToString(), ExtractResource(COM3D2.GP01FBFaceEyeCtr.Properties.Resources.icon));

        }

        public static byte[] ExtractResource(Bitmap image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }

        /*
        */

        /*
        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            GP01FBFaceEyeCtrGUI.myWindowRect.save();
        }
        */
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
                myWindowRect.IsGUIOn = !myWindowRect.IsGUIOn;
                //GP01FBFaceEyeCtr.myLog.LogMessage("IsUp", ShowCounter.Value.Modifiers, ShowCounter.Value.MainKey);
            }
        }

        public void OnGUI()
        {
            if (!myWindowRect.IsGUIOn)
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
            if (GUILayout.Button("-", GUILayout.Width(20), GUILayout.Height(20))) { myWindowRect.IsOpen = !myWindowRect.IsOpen; }
            if (GUILayout.Button("x", GUILayout.Width(20), GUILayout.Height(20))) { myWindowRect.IsGUIOn = false; }
            GUILayout.EndHorizontal();

            if (!myWindowRect.IsOpen)
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

                GUI.enabled = MaidActiveUtill.GetMaid(GP01FBFaceEyeCtrGUI.seleted) != null;

                for (int i = 0; i < UtillMPN.nowMPNs.Length; i++)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(UtillMPN.nowMPNnm[i]);
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Rnd"))
                    {
                        MaidActiveUtill.GetMaid(GP01FBFaceEyeCtrGUI.seleted)?.SetProp(UtillMPN.nowMPNs[i], (int)(  UtillMPN.nowMPNvb[i] = UtillMPN.nowMPNv[i]=UnityEngine.Random.Range(UtillMPN.nowMPNmin[i], UtillMPN.nowMPNmax[i])));
                        UtillMPN.SetNowMPNv(i);
                    }
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
                                foreach (var item in MaidActiveUtill.GetMaidAll())
                                {
                                    item?.SetProp(UtillMPN.nowMPNs[i], (int)(UtillMPN.nowMPNv[i] = (int)UtillMPN.nowMPNvb[i]));
                                }
                            }
                            else
                            {
                                MaidActiveUtill.GetMaid(GP01FBFaceEyeCtrGUI.seleted)?.SetProp(UtillMPN.nowMPNs[i], (int)(UtillMPN.nowMPNv[i] = (int)UtillMPN.nowMPNvb[i]));
                            }
                            //MyLog.LogMessage("changed", mpns[i], mpni[i]);
                        }
                        if (isAllMaid)
                        {
                            foreach (var item in MaidActiveUtill.GetMaidAll())
                            {
                                item?.AllProcProp();
                            }
                        }
                        else
                        {
                            MaidActiveUtill.GetMaid(GP01FBFaceEyeCtrGUI.seleted)?.AllProcProp();
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
                if (GUILayout.Button("Copy All") && MaidActiveUtill.GetMaid(GP01FBFaceEyeCtrGUI.seleted) != null)
                {
                    var ms = MaidActiveUtill.maids;
                    for (int i = 0; i < UtillMPN.nowMPNs.Length; i++)
                    {
                        if (UtillMPN.nowBools[i])
                        {
                            var m = MaidActiveUtill.GetMaid(GP01FBFaceEyeCtrGUI.seleted).GetProp(UtillMPN.nowMPNs[i]);
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


                //seleted = MaidActivePatch.SelectionGrid(seleted,3, 265 ,false);
                GUILayout.Label("maid select");
                // 여기는 출력된 메이드들 이름만 가져옴
                // seleted 가 이름 위치 번호만 가져온건데
                seleted = MaidActiveUtill.SelectionGrid(seleted);

                if (GUI.changed)
                {
                    UtillMPN.UpdateMPNs();
                    GUI.changed = false;
                }
                /*
                */
                #endregion

                GUILayout.EndScrollView();



            }
            GUI.enabled = true;
            GUI.DragWindow(); // 창 드레그 가능하게 해줌. 마지막에만 넣어야함
        }



        public void OnDisable()
        {

            //GP01FBFaceEyeCtrGUI.myWindowRect?.save();
            //SceneManager.sceneLoaded -= this.OnSceneLoaded;
            harmony.UnpatchSelf();
        }


        public static int seleted;
        public static int seletedfolder;
        public static int seletedfolderbak;



    }
}
