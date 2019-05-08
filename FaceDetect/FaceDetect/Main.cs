using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FaceDetect
{
    public partial class Main : Form
    {
        #region Property
        /// <summary>
        /// 保存人脸数据的文件夹
        /// </summary>
        const string FeaturePath = "d:\\FeatureData";
        /// <summary>
        /// 虹软SDK的AppId
        /// </summary>
        const string FaceAppId = "Cgxu8ByM9ymDvGuPRGrP5keHV7wfkBcqGhFkAfQaawQ2";
        /// <summary>
        /// 虹软SDK人脸跟踪的Key
        /// </summary>
        const string FaceTraceKey = "5wJkr5RViTwYJjqpmtXC4ikC5bKK75NRCrwpq8P94z3U";
        /// <summary>
        /// 虹软SDK人脸检测的Key
        /// </summary>
        const string FaceDetectKey = "5wJkr5RViTwYJjqpmtXC4ikKEzaTZrdPs4HXQk8M4nRC";
        /// <summary>
        /// 虹软SDK人脸比对的Key
        /// </summary>
        const string FaceMatchKey = "5wJkr5RViTwYJjqpmtXC4ikotbdBt9vjwrAnXCsr6m9h";
        /// <summary>
        /// 虹软SDK年龄识别的Key
        /// </summary>
        const string FaceAgeKey = "5wJkr5RViTwYJjqpmtXC4im4DQ9U2rSUfYPkGeLxyMHZ";
        /// <summary>
        /// 虹软SDK性别识别的Key
        /// </summary>
        const string FaceGenderKey = "5wJkr5RViTwYJjqpmtXC4imBNoQffqmX7i6eUC1mvGMR";
        /// <summary>
        /// 缓存大小
        /// </summary>
        const int BufferSize = 40 * 1024 * 1024;
        /// <summary>
        /// 人脸跟踪的缓存
        /// </summary>
        //byte[] _FaceTraceBuffer = new byte[BufferSize];
        /// <summary>
        /// 人脸检测的缓存
        /// </summary>
        byte[] _FaceDetectBuffer = new byte[BufferSize];
        /// <summary>
        /// 人脸比对的缓存
        /// </summary>
        byte[] _FaceMatchBuffer = new byte[BufferSize];
        /// <summary>
        /// 年龄识别的缓存
        /// </summary>
        //byte[] _FaceAgeBuffer = new byte[BufferSize];
        /// <summary>
        /// 性别识别的缓存
        /// </summary>
        //byte[] _FaceGenderBuffer = new byte[BufferSize];
        /// <summary>
        /// 人脸跟踪的引擎
        /// </summary>
        //IntPtr _FaceTraceEnginer = IntPtr.Zero;
        /// <summary>
        /// 人脸检测的引擎
        /// </summary>
        IntPtr _FaceDetectEnginer = IntPtr.Zero;
        /// <summary>
        /// 人脸比对的引擎
        /// </summary>
        IntPtr _FaceMatchEngine = IntPtr.Zero;
        /// <summary>
        /// 年龄识别的引擎
        /// </summary>
        //IntPtr _FaceAgeEngine = IntPtr.Zero;
        /// <summary>
        /// 性别识别的引擎
        /// </summary>
        //IntPtr _FaceGenderEngine = IntPtr.Zero;
        /// <summary>
        /// 人脸库字典
        /// </summary>
        Face.FaceLib _FaceLib = new Face.FaceLib();
        /// <summary>
        /// 摄像头参数
        /// </summary>
        Common.CameraPara _CameraPara = null;
        double _RateW, _RateH;
        private readonly ReaderWriterLockSlim _CacheLock = new ReaderWriterLockSlim();
        Face.FaceResult _FaceResult = new Face.FaceResult();
        System.Threading.CancellationTokenSource _CancellationTokenSource = new System.Threading.CancellationTokenSource();
        bool _RegisterClicked = false;
        byte[] _RegisterFeatureData = null;
        #endregion
        public Main()
        {
            InitializeComponent();
        }


        private void Main_Load(object sender, EventArgs e)
        {

            if (!Directory.Exists(FeaturePath))
                Directory.CreateDirectory(FeaturePath);

            foreach (var file in Directory.GetFiles(FeaturePath))
            {
                var info = new FileInfo(file);
                var data = File.ReadAllBytes(file);
                var faceModel = new Face.FaceModel
                {
                    lFeatureSize = data.Length,
                    pbFeature = Marshal.AllocHGlobal(data.Length)
                };

                Marshal.Copy(data, 0, faceModel.pbFeature, data.Length);
                _FaceLib.Items.Add(new Face.FaceLib.Item() { OrderId = 0, ID = info.Name.Replace(info.Extension, ""), FaceModel = faceModel });
            }
            _CameraPara = new Common.CameraPara();
            if (!_CameraPara.HasVideoDevice)
            {
                MessageBox.Show("没有检测到摄像头");
                this.Close();
                return;
            }

            this.VideoPlayer.VideoSource = _CameraPara.VideoSource;
            this.VideoPlayer.Start();

            _RateH = 1.0 * this.VideoPlayer.Height / this._CameraPara.FrameHeight;
            _RateW = 1.0 * this.VideoPlayer.Width / this._CameraPara.FrameWidth;

            //var initResult = (Face.ErrorCode)Face.Trace.Init(FaceAppId, FaceTraceKey, _FaceTraceBuffer, BufferSize, out _FaceTraceEnginer, (int)Face.OrientPriority.Only0, 16, 1);
            //if (initResult != Face.ErrorCode.Ok)
            //{
            //    MessageBox.Show("初始化人脸跟踪引擎失败，错误代码为：" + initResult);
            //    this.Close();
            //    return;
            //}

            var initResult = (Face.ErrorCode)Face.Detect.Init(FaceAppId, FaceDetectKey, _FaceDetectBuffer, BufferSize, out _FaceDetectEnginer, (int)Face.OrientPriority.Only0, 16, 1);
            if (initResult != Face.ErrorCode.Ok)
            {
                MessageBox.Show("初始化人脸检测引擎失败，错误代码为：" + initResult);
                this.Close();
                return;
            }

            initResult = (Face.ErrorCode)Face.Match.Init(FaceAppId, FaceMatchKey, _FaceMatchBuffer, BufferSize, out _FaceMatchEngine);
            if (initResult != Face.ErrorCode.Ok)
            {
                MessageBox.Show("初始化人脸比对引擎失败，错误代码为：" + initResult);
                this.Close();
                return;
            }

            //initResult = (Face.ErrorCode)Face.Age.Init(FaceAppId, FaceAgeKey, _FaceAgeBuffer, BufferSize, out _FaceAgeEngine);
            //if (initResult != Face.ErrorCode.Ok)
            //{
            //    MessageBox.Show("初始化年龄识别引擎失败，错误代码为：" + initResult);
            //    this.Close();
            //    return;
            //}

            //initResult = (Face.ErrorCode)Face.Gender.Init(FaceAppId, FaceGenderKey, _FaceGenderBuffer, BufferSize, out _FaceGenderEngine);
            //if (initResult != Face.ErrorCode.Ok)
            //{
            //    MessageBox.Show("初始化性别识别引擎失败，错误代码为：" + initResult);
            //    this.Close();
            //    return;
            //}

            //Task.Factory.StartNew(() =>
            //{
            //    Task.Delay(1000).Wait();
            //    while (!_CancellationTokenSource.IsCancellationRequested)
            //    {
            //        #region 200毫秒左右
            //        if(_RegisterClicked)
            //        {
            //            MatchFrame();
            //        }
            //        #endregion
            //    }
            //}, _CancellationTokenSource.Token);

        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_CameraPara.HasVideoDevice)
            {
                _CancellationTokenSource.Cancel();
                Thread.Sleep(500);
                this.VideoPlayer.Stop();

                if (_FaceMatchEngine != IntPtr.Zero)
                {
                    Face.Match.Close(_FaceMatchEngine);
                }
                if (_FaceDetectEnginer != IntPtr.Zero)
                {
                    Face.Detect.Close(_FaceDetectEnginer);
                }
            }

        }

        private void MatchFrame()
        {
            #region 获取图片 1毫秒
            var bitmap = this.VideoPlayer.GetCurrentVideoFrame();
            #endregion

            Stopwatch sw = new Stopwatch();
            sw.Start();
            #region 图片转换 0.7-2微妙
            var bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            var imageData = new Face.ImageData
            {
                u32PixelArrayFormat = 513,//Rgb24,
                i32Width = bitmap.Width,
                i32Height = bitmap.Height,
                pi32Pitch = new int[4],
                ppu8Plane = new IntPtr[4]
            };
            imageData.pi32Pitch[0] = bmpData.Stride;
            imageData.ppu8Plane[0] = bmpData.Scan0;

            sw.Stop();
            _FaceResult.Score = sw.ElapsedTicks;


            #endregion
            try
            {
                #region 人脸检测 5-8毫秒
                var ret = (Face.ErrorCode)Face.Detect.Detection(_FaceDetectEnginer, ref imageData, out var pDetectResult);
                if (ret != Face.ErrorCode.Ok)
                    return;
                var detectResult = Marshal.PtrToStructure<Face.DetectResult>(pDetectResult);
                if (detectResult.nFace == 0)
                    return;
                var faceRect = Marshal.PtrToStructure<Face.FaceRect>(detectResult.rcFace);
                _FaceResult.Rectangle = new Rectangle((int)(faceRect.left * _RateW), (int)(faceRect.top * _RateH), (int)((faceRect.right - faceRect.left) * _RateW), (int)((faceRect.bottom - faceRect.top) * _RateH));
                var faceOrient = Marshal.PtrToStructure<int>(detectResult.lfaceOrient);
                #endregion


                #region 性别识别基本准确 年龄识别误差太大，没什么应用场景
                //Face.ExtraFaceInput faceInput = new Face.ExtraFaceInput()
                //{
                //    lFaceNumber = facesDetect.nFace,
                //    pFaceRectArray = Marshal.AllocHGlobal(Marshal.SizeOf(faceRect)),
                //    pFaceOrientArray = Marshal.AllocHGlobal(Marshal.SizeOf(faceOrient))
                //};
                //Marshal.StructureToPtr(faceRect, faceInput.pFaceRectArray, false);
                //Marshal.StructureToPtr(faceOrient, faceInput.pFaceOrientArray, false);

                //var ageResult = Face.Age.ASAE_FSDK_AgeEstimation_Preview(_FaceAgeEngine, ref imageData, ref faceInput, out var pAgeRes);
                //var ages = pAgeRes.pResult.ToStructArray<int>(pAgeRes.lFaceNumber);
                //var genderResult = Face.Gender.ASGE_FSDK_GenderEstimation_Preview(_FaceGenderEngine, ref imageData, ref faceInput, out var pGenderRes);
                //var genders = pGenderRes.pResult.ToStructArray<int>(pGenderRes.lFaceNumber);
                //_FaceResult.Age = ages[0];
                //_FaceResult.Gender = genders[0];

                //Marshal.FreeHGlobal(faceInput.pFaceOrientArray);
                //Marshal.FreeHGlobal(faceInput.pFaceRectArray);
                #endregion

                #region 获取人脸特征 160-180毫秒
                var faceFeatureInput = new Face.FaceFeatureInput
                {
                    rcFace = faceRect,
                    lOrient = faceOrient
                };

                ret = (Face.ErrorCode)Face.Match.ExtractFeature(_FaceMatchEngine, ref imageData, ref faceFeatureInput, out var faceModel);
                #endregion

                if (ret == Face.ErrorCode.Ok)
                {

                    if (_RegisterClicked)
                    {
                        _RegisterFeatureData = new byte[faceModel.lFeatureSize];
                        Marshal.Copy(faceModel.pbFeature, _RegisterFeatureData, 0, faceModel.lFeatureSize);
                    }

                    #region 人脸识别（100张人脸） 17-20毫秒
                    foreach (var item in _FaceLib.Items.OrderByDescending(ii => ii.OrderId))
                    {
                        var fm = item.FaceModel;
                        Face.Match.FacePairMatch(_FaceMatchEngine, ref fm, ref faceModel, out float score);
                        if (score > 0.5)
                        {
                            item.OrderId = DateTime.Now.Ticks;
                            _FaceResult.ID = item.ID;
                            break;
                        }
                    }
                    #endregion

                }

            }catch(Exception ex)
            {
                bitmap = null;
            }
            finally
            {
                bitmap.UnlockBits(bmpData);
                if (_RegisterClicked)
                {
                    this.pictureBox1.Invoke(new Action(() =>
                    {
                        this.pictureBox1.Image = bitmap;
                    }));
                    _RegisterClicked = false;
                }
                else
                {
                    bitmap.Dispose();
                }
            }

        }

        private void VideoPlayer_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(Pens.White, _FaceResult.Rectangle);
            e.Graphics.DrawString(_FaceResult.ID, this.Font, Brushes.White, _FaceResult.Rectangle.Left, _FaceResult.Rectangle.Top - 20);
        }

        private void VideoPlayer_Click(object sender, EventArgs e)
        {
            _RegisterFeatureData = null;
            _RegisterClicked = true;
            MatchFrame();
            this.TextBoxID.Text = _FaceResult.ID;
        }
        private void ButtonRegister_Click(object sender, EventArgs e)
        {
            if (_RegisterFeatureData == null)
            {
                MessageBox.Show("没有人脸数据，请面对摄像头并点击视频");
                return;
            }
            if (string.IsNullOrEmpty(this.TextBoxID.Text))
            {
                MessageBox.Show("请输入Id");
                this.TextBoxID.Focus();
                return;
            }
            var fileName = FeaturePath + "\\" + this.TextBoxID.Text + ".dat";
            if (System.IO.File.Exists(fileName))
            {
                if (MessageBox.Show($"您要替换[{this.TextBoxID.Text}]的人脸数据吗？", "咨询", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.No)
                    return;
            }
            System.IO.File.WriteAllBytes(fileName, _RegisterFeatureData);
            var faceModel = new Face.FaceModel
            {
                lFeatureSize = _RegisterFeatureData.Length,
                pbFeature = Marshal.AllocHGlobal(_RegisterFeatureData.Length)
            };

            Marshal.Copy(_RegisterFeatureData, 0, faceModel.pbFeature, _RegisterFeatureData.Length);
            _FaceLib.Items.Add(new Face.FaceLib.Item() { OrderId = DateTime.Now.Ticks, ID = this.TextBoxID.Text, FaceModel = faceModel });

        }




    }
}
