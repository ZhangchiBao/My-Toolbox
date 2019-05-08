using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Face
{
    internal class Detect
    {
        private const string DllPath = "libarcsoft_fsdk_face_detection.dll";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="sdkKey"></param>
        /// <param name="memory"></param>
        /// <param name="memroySize"></param>
        /// <param name="engine"></param>
        /// <param name="orientPriority"></param>
        /// <param name="scale">最小人脸尺寸有效值范围[2,50] 推荐值 16。该尺寸是人脸相对于所在图片的长边的占比。例如，如果用户想检测到的最小人脸尺寸是图片长度的 1/8，那么这个 nScale 就应该设置为8</param>
        /// <param name="maxFaceNumber">用户期望引擎最多能检测出的人脸数有效值范围[1,100]</param>
        /// <returns></returns>
        [DllImport(DllPath, EntryPoint = "AFD_FSDK_InitialFaceEngine", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int Init(string appId, string sdkKey, byte[] memory, int memroySize, out IntPtr engine, int orientPriority, int scale, int maxFaceNumber);
        [DllImport(DllPath, EntryPoint = "AFD_FSDK_StillImageFaceDetection", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int Detection(IntPtr engine, ref ImageData imgData, out IntPtr pDetectResult);
        [DllImport(DllPath, EntryPoint = "AFD_FSDK_UninitialFaceEngine", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int Close(IntPtr engine);
    }
    internal class Trace
    {
        private const string DllPath = "libarcsoft_fsdk_face_tracking.dll";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="sdkKey"></param>
        /// <param name="buffer"></param>
        /// <param name="bufferSize"></param>
        /// <param name="engine"></param>
        /// <param name="orientPriority"></param>
        /// <param name="scale">最小人脸尺寸有效值范围[2,16] 推荐值 16。该尺寸是人脸相对于所在图片的长边的占比。例如，如果用户想检测到的最小人脸尺寸是图片长度的 1/8，那么这个 nScale 就应该设置为8</param>
        /// <param name="faceNumber">用户期望引擎最多能检测出的人脸数有效值范围[1,20]</param>
        /// <returns></returns>        
        [DllImport(DllPath, EntryPoint = "AFT_FSDK_InitialFaceEngine", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int Init(string appId, string sdkKey, byte[] buffer, int bufferSize, out IntPtr engine, int orientPriority, int scale, int faceNumber);
        [DllImport(DllPath, EntryPoint = "AFT_FSDK_FaceFeatureDetect", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int Detection(IntPtr engine, ref ImageData imgData, out IntPtr pTraceResult);
        [DllImport(DllPath, EntryPoint = "AFT_FSDK_UninitialFaceEngine", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int Close(IntPtr engine);
    }

    internal class Match
    {
        private const string DllPath = "libarcsoft_fsdk_face_recognition.dll";
        [DllImport(DllPath, EntryPoint = "AFR_FSDK_InitialEngine", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int Init(string appId, string sdkKey, byte[] buffer, int bufferSize, out IntPtr engine);
        [DllImport(DllPath, EntryPoint = "AFR_FSDK_ExtractFRFeature", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int ExtractFeature(IntPtr engine, ref ImageData imageData, ref FaceFeatureInput faceFeatureInput, out FaceModel pFaceModels);
        [DllImport(DllPath, EntryPoint = "AFR_FSDK_FacePairMatching", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int FacePairMatch(IntPtr engine, ref FaceModel faceModel1, ref FaceModel faceModel2, out float score);
        [DllImport(DllPath, EntryPoint = "AFR_FSDK_UninitialEngine", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int Close(IntPtr engine);
    }
    internal class Age
    {
        private const string DllPosition = "libarcsoft_fsdk_age_estimation.dll";

        [DllImport(DllPosition, EntryPoint = "ASAE_FSDK_InitAgeEngine", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int Init(string appId, string sdkKey, byte[] buffer, int bufferSize, out IntPtr engine);
        [DllImport(DllPosition, EntryPoint = "ASAE_FSDK_AgeEstimation_StaticImage", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int EstimationStatic(IntPtr engine, ref ImageData imgData, ref EstimationInput estimationInputInput, out EstimationResult pAgeResult);
        [DllImport(DllPosition, EntryPoint = "ASAE_FSDK_AgeEstimation_Preview", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int EstimationPreview(IntPtr engine, ref ImageData imgData, ref EstimationInput estimationInputInput, out EstimationResult pAgeResult);
        [DllImport(DllPosition, EntryPoint = "ASAE_FSDK_UninitAgeEngine", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int Close(IntPtr engine);
    }
    internal class Gender
    {
        private const string DllPosition = "libarcsoft_fsdk_gender_estimation.dll";

        [DllImport(DllPosition, EntryPoint = "ASGE_FSDK_InitGenderEngine", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int Init(string appId, string sdkKey, byte[] buffer, int bufferSize, out IntPtr engine);
        [DllImport(DllPosition, EntryPoint = "ASGE_FSDK_GenderEstimation_StaticImage", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int EstimationStatic(IntPtr engine, ref ImageData imgData, ref EstimationInput estimationInputInput, out EstimationResult pGenderResult);
        [DllImport(DllPosition, EntryPoint = "ASGE_FSDK_GenderEstimation_Preview", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int EstimationPreview(IntPtr engine, ref ImageData imgData, ref EstimationInput estimationInputInput, out EstimationResult pGenderResesult);
        [DllImport(DllPosition, EntryPoint = "ASGE_FSDK_UninitGenderEngine", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int Close(IntPtr engine);
    }
}
