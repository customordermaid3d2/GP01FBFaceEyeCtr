using COM3D2.LillyUtill;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM3D2.GP01FBFaceEyeCtr
{
    class UtillMPN
    {
        static Dictionary<folder, MPN[]> folderMPNs = new Dictionary<folder, MPN[]>();
        static Dictionary<folder, List<MPN>> folderMPNl = new Dictionary<folder, List<MPN>>();
        static Dictionary<folder, float[]> folderMPNv = new Dictionary<folder, float[]>();
        static Dictionary<folder, float[]> folderMPNb = new Dictionary<folder, float[]>();
        static Dictionary<folder, float[]> folderMPNmin = new Dictionary<folder, float[]>();
        static Dictionary<folder, float[]> folderMPNmax = new Dictionary<folder, float[]>();
        static Dictionary<folder, string[]> folderMPNnm = new Dictionary<folder, string[]>();
        static Dictionary<folder, bool[]> folderBool = new Dictionary<folder, bool[]>();

        public static Dictionary<MPN, folder> mpnFolder = new Dictionary<MPN, folder>();

        public static folder nowfolder;

        public static MPN[] nowMPNs;
        public static List<MPN> nowMPNl;
        public static float[] nowMPNv, nowMPNvb, nowMPNmin, nowMPNmax;
        public static string[] nowMPNnm;
        public static bool[] nowBools;

        public static string[] foldersNm;
        //public static folder[] folders;

        public enum folder
        {
            head,
            Mayu,
            Eye,
            ear,
            Mabuta,
            body,
            mune,
            bodyUp,
            bodyDown,
        }

        internal static void init()
        {
            folderMPNs.Add(folder.head, new MPN[]
            {
                 MPN.FaceShape,
                 MPN.FaceShapeSlim,

                 MPN.NosePos,
                 MPN.NoseScl,

                 MPN.HeadX,
                 MPN.HeadY
            });

            folderMPNs.Add(folder.Mayu, new MPN[]
            {
                 MPN.MayuX,
                 MPN.MayuY,
                 MPN.MayuRot,
                 MPN.MayuShapeIn,
                 MPN.MayuShapeOut
            });

            folderMPNs.Add(folder.Eye, new MPN[]
            {
                 MPN.EyeSclX,
                 MPN.EyeSclY,
                 MPN.EyePosX,
                 MPN.EyePosY,
                 MPN.EyeClose,

                 MPN.EyeBallPosY,
                 MPN.EyeBallSclX,
                 MPN.EyeBallSclY
            });

            folderMPNs.Add(folder.ear, new MPN[]
            {
                 MPN.EarElf ,
                 MPN.EarScl ,
                 MPN.EarRot
            });

            folderMPNs.Add(folder.Mabuta, new MPN[]
            {
                // 속눈썹 위
                MPN.MabutaUpIn,
                MPN.MabutaUpIn2,
                MPN.MabutaUpMiddle,
                MPN.MabutaUpOut,
                MPN.MabutaUpOut2,
                // 속눈썹 아래
                MPN.MabutaLowIn,
                MPN.MabutaLowUpMiddle,
                MPN.MabutaLowUpOut
            });

            folderMPNs.Add(folder.body, new MPN[]
            {
                MPN.DouPer,
                MPN.sintyou
            });

            folderMPNs.Add(folder.mune, new MPN[]
            {
                MPN.MuneL,
                MPN.MuneTare,
                MPN.MuneUpDown,
                MPN.MuneYori,
                MPN.MuneYawaraka
            });

            folderMPNs.Add(folder.bodyUp, new MPN[]
            {
                MPN.west,
                MPN.Hara,
                MPN.kata,
                MPN.ArmL,
                MPN.UdeScl,
                MPN.KubiScl
            });

            folderMPNs.Add(folder.bodyDown, new MPN[]
            {
                MPN.koshi,
                MPN.RegFat,
                MPN.RegMeet
            });

            foreach (var item in folderMPNs)
            {
                folderMPNl.Add(item.Key, item.Value.ToList());
                folderMPNv[item.Key] = new float[item.Value.Length];
                folderMPNb[item.Key] = new float[item.Value.Length];
                folderMPNmax[item.Key] = new float[item.Value.Length];
                folderMPNmin[item.Key] = new float[item.Value.Length];
                folderMPNnm[item.Key] = new string[item.Value.Length];
                folderBool[item.Key] = new bool[item.Value.Length];
                for (int i = 0; i < item.Value.Length; i++)
                {
                    folderMPNnm[item.Key][i] = item.Value[i].ToString();
                    mpnFolder.Add(item.Value[i], item.Key);
                }
            }

            foldersNm = Enum.GetNames(typeof(folder));

            SetFolderMPNs(folder.Mabuta);
            GP01FBFaceEyeCtrGUI.seletedfolderbak = GP01FBFaceEyeCtrGUI.seletedfolder = (int)folder.Mabuta;
        }

        internal static void SetFolderMPNs(folder nm)
        {
            nowfolder = nm;
            nowMPNs = folderMPNs[nm];
            nowMPNl = folderMPNl[nm];
            nowMPNv = folderMPNv[nm];
            nowMPNvb = folderMPNb[nm];
            nowMPNmin = folderMPNmin[nm];
            nowMPNmax = folderMPNmax[nm];
            nowMPNnm = folderMPNnm[nm];
            nowBools = folderBool[nm];
        }

        public static void UpdateMPNs(MPN mpn)
        {
            if (mpnFolder.ContainsKey(mpn))
            {
                if (mpnFolder[mpn] == nowfolder)
                {
                    SetNowMPNv(folderMPNl[nowfolder].IndexOf(mpn));
                }
            }
            else
            {
                GP01FBFaceEyeCtr.myLog.LogWarning("UpdateMPNs", mpn);
            }
        }

        public static void UpdateMPNs()
        {
            UpdateMPNs(GP01FBFaceEyeCtrGUI.seleted);
        }


        public static void UpdateMPNs(int seleted)
        {
            if (GP01FBFaceEyeCtrGUI.seleted== seleted)
            {
                Maid maid = MaidActivePatch.GetMaid(seleted);
                if (maid != null)
                    //if (maid == MaidActivePatch.GetMaid(GP01FBFaceEyeCtrGUI.seleted))
                        for (int i = 0; i < UtillMPN.nowMPNs.Length; i++)
                        {
                            SetNowMPNv(i);
                        }
            }
        }

        public static void SetNowMPNv(int i)
        {
            if (i < 0)
            {
                GP01FBFaceEyeCtr.myLog.LogWarning("SetNowMPNv", i);
                return;
            }
            var mp = MaidActivePatch.GetMaid(GP01FBFaceEyeCtrGUI.seleted).GetProp(UtillMPN.nowMPNs[i]);
            UtillMPN.nowMPNvb[i] = UtillMPN.nowMPNv[i] = mp.value;
            UtillMPN.nowMPNmin[i] = mp.min;
            UtillMPN.nowMPNmax[i] = mp.max;
        }
    }
}
