using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Face
{
    /// <summary>
    /// 人脸库
    /// </summary>
    public class FaceLib
    {
        public List<Item> Items { get; set; } = new List<Item>();
        public class Item
        {
            /// <summary>
            /// 用于排序
            /// </summary>
            public long OrderId { get; set; }
            /// <summary>
            /// 文件名作为ID
            /// </summary>
            public string ID { get; set; }
            /// <summary>
            /// 人脸模型
            /// </summary>
            public FaceModel FaceModel { get; set; }
        }
    }
    /// <summary>
    /// 人脸识别结果
    /// </summary>
    public class FaceResult
    {
        //public int NotMatchedCount { get; set; }
        public string ID { get; set; }
        public float Score { get; set; }
        public System.Drawing.Rectangle Rectangle { get; set; }
        public int Age { get; set; }
        /// <summary>
        /// 0：男，1：女，其他：未知
        /// </summary>
        public int Gender { get; set; }
        public override string ToString()
        {

            string ret = "";
            if (!string.IsNullOrEmpty(ID))
                ret = ID + "，";
            ret += Age + "岁";
            if (Gender == 0)
                ret += "，男";
            else if (Gender == 1)
                ret += "，女";

            return ret + "," + Score;
        }
    }

    /// <summary>
    /// 人脸跟踪、检测、性别年龄评估和获取人脸信息的输入参数
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ImageData
    {
        public uint u32PixelArrayFormat;
        public int i32Width;
        public int i32Height;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public IntPtr[] ppu8Plane;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I4)]
        public int[] pi32Pitch;
    }
    /// <summary>
    /// 人脸跟踪的结果
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct TraceResult
    {
        [MarshalAs(UnmanagedType.I4)]
        public int nFace;
        [MarshalAs(UnmanagedType.I4)]
        public int lfaceOrient;
        public IntPtr rcFace;
    }
    /// <summary>
    /// 人脸检测的结果
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct DetectResult
    {
        [MarshalAs(UnmanagedType.I4)]
        public int nFace;
        public IntPtr rcFace;
        public IntPtr lfaceOrient;
    }

    /// <summary>
    /// 人脸在图片中的位置
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct FaceRect
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }
    /// <summary>
    /// 获取人脸特征的输入参数
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct FaceFeatureInput
    {
        public FaceRect rcFace;
        public int lOrient;
    }
    /// <summary>
    /// 人脸特征
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct FaceModel
    {
        public IntPtr pbFeature;
        [MarshalAs(UnmanagedType.I4)]
        public int lFeatureSize;
    }
    /// <summary>
    /// 性别和年龄评估的输入参数
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct EstimationInput
    {
        public IntPtr pFaceRectArray;
        public IntPtr pFaceOrientArray;
        public int lFaceNumber;
    }
    /// <summary>
    /// 性别和年龄评估的结果
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct EstimationResult
    {
        public IntPtr pResult;
        public int lFaceNumber;
    }

    /// <summary>
    /// 错误代码
    /// </summary>
    public enum ErrorCode
    {
        /// <summary>
        /// 正确
        /// </summary>
        Ok = 0,

        /// <summary>
        /// 通用错误类型
        /// </summary>
        BasicBase = 0x0001,

        /// <summary>
        /// 错误原因不明
        /// </summary>
        Unknown = BasicBase,

        /// <summary>
        /// 无效的参数
        /// </summary>
        InvalidParam = BasicBase + 1,

        /// <summary>
        /// 引擎不支持
        /// </summary>
        Unsupported = BasicBase + 2,

        /// <summary>
        /// 内存不足
        /// </summary>
        NoMemory = BasicBase + 3,

        /// <summary>
        /// 状态错误
        /// </summary>
        BadState = BasicBase + 4,

        /// <summary>
        /// 用户取消相关操作
        /// </summary>
        UserCancel = BasicBase + 5,

        /// <summary>
        /// 操作时间过期
        /// </summary>
        Expired = BasicBase + 6,

        /// <summary>
        /// 用户暂停操作
        /// </summary>
        UserPause = BasicBase + 7,

        /// <summary>
        /// 缓冲上溢
        /// </summary>
        BufferOverflow = BasicBase + 8,

        /// <summary>
        /// 缓冲下溢
        /// </summary>
        BufferUnderflow = BasicBase + 9,

        /// <summary>
        /// 存贮空间不足
        /// </summary>
        NoDiskspace = BasicBase + 10,

        /// <summary>
        /// 组件不存在
        /// </summary>
        ComponentNotExist = BasicBase + 11,

        /// <summary>
        /// 全局数据不存在
        /// </summary>
        GlobalDataNotExist = BasicBase + 12,

        /// <summary>
        /// Free SDK通用错误类型
        /// </summary>
        SdkBase = 0x7000,

        /// <summary>
        /// 无效的App Id
        /// </summary>
        InvalidAppId = SdkBase + 1,

        /// <summary>
        /// 无效的SDK key
        /// </summary>
        InvalidSdkId = SdkBase + 2,

        /// <summary>
        /// AppId和SDKKey不匹配
        /// </summary>
        InvalidIdPair = SdkBase + 3,

        /// <summary>
        /// SDKKey 和使用的SDK 不匹配
        /// </summary>
        MismatchIdAndSdk = SdkBase + 4,

        /// <summary>
        /// 系统版本不被当前SDK所支持
        /// </summary>
        SystemVersionUnsupported = SdkBase + 5,

        /// <summary>
        /// SDK有效期过期，需要重新下载更新
        /// </summary>
        LicenceExpired = SdkBase + 6,

        /// <summary>
        /// Face Recognition错误类型
        /// </summary>
        FaceRecognitionBase = 0x12000,

        /// <summary>
        /// 无效的输入内存
        /// </summary>
        InvalidMemoryInfo = FaceRecognitionBase + 1,

        /// <summary>
        /// 无效的输入图像参数
        /// </summary>
        InvalidImageInfo = FaceRecognitionBase + 2,

        /// <summary>
        /// 无效的脸部信息
        /// </summary>
        InvalidFaceInfo = FaceRecognitionBase + 3,

        /// <summary>
        /// 当前设备无GPU可用
        /// </summary>
        NoGpuAvailable = FaceRecognitionBase + 4,

        /// <summary>
        /// 待比较的两个人脸特征的版本不一致
        /// </summary>
        MismatchedFeatureLevel = FaceRecognitionBase + 5
    }
    /// <summary>
    /// 脸部角度的检测范围
    /// </summary>
    public enum OrientPriority
    {
        /// <summary>
        /// 检测 0 度（±45 度）方向
        /// </summary>
        Only0 = 0x1,

        /// <summary>
        /// 检测 90 度（±45 度）方向
        /// </summary>
        Only90 = 0x2,

        /// <summary>
        /// 检测 270 度（±45 度）方向
        /// </summary>
        Only270 = 0x3,

        /// <summary>
        /// 检测 180 度（±45 度）方向
        /// </summary>
        Only180 = 0x4,

        /// <summary>
        /// 检测 0， 90， 180， 270 四个方向,0 度更优先
        /// </summary>
        Ext0 = 0x5
    }
}
