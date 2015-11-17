using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
namespace AnXinWH.ShiPinNewVideoOCX
{
    public sealed class TMCC
    {
        public const int TMCC_ERR_SUCCESS = 0;
        public const int TMCC_MAJOR_CMD_SERVERCONTROL = 0x111;		/*远程服务器控制*/
        public const int TMCC_MINOR_CMD_MANUALCAPTURE = 0x10;		    /*远程手动抓图传到本地*/
        public const int TMCC_MAJOR_CMD_VIDEOINCFG = 0x116;		/*视频输入配置*/
        public const int TMCC_MINOR_CMD_VIDEOIN = 0x00;		    /*输入配置*/
        /*远程文件播放方式*/
        public const int REMOTEPLAY_MODE_BUFFILE = 0x00;	/*解码显示播放，需要本地缓冲，此方式占用网络带快大*/
        public const int REMOTEPLAY_MODE_NOBUFFILE = 0x01;	/*解码显示，不带本地缓冲，此方式占用网络带宽与码流大小一致*/
        public const int REMOTEPLAY_MODE_OLDFILE = 0x02;	/*老方式播放文件，主要是a2的摄像机*/
        public const int REMOTEPLAY_MODE_LOCALFILE = 0x03; /*本地文件*/
        public const int REMOTEPLAY_MODE_READFILE = 0x04;	/*不解码显示，为TMCC_ReadFile提供支持*/
        public const int REMOTEPLAY_MODE_CONTROLFILE = 0x05;	/*服务器控制视频的速率*/

        /*远程文件打开控制结构定义*/
        public const UInt32 PLAY_CONTROL_PLAY = 0;	//*播放,以iPlayData作为播放参数(0-保留当前设置,1-回复默认)*/
        public const UInt32 PLAY_CONTROL_STOP = 1;		//*停止*/
        public const UInt32 PLAY_CONTROL_PAUSE = 2;		//*暂停,注意停止直接调用相关关闭函数即可*/
        public const UInt32 PLAY_CONTROL_FAST = 3;	//*快放,以iSpeed作为速度*/
        public const UInt32 PLAY_CONTROL_SLOW = 4;	//*慢放,以iSpeed作为速度*/
        public const UInt32 PLAY_CONTROL_SEEKPOS = 5;	//*seek,以iCurrentPosition*/
        public const UInt32 PLAY_CONTROL_SEEKTIME = 6;	//*seek,以dwCurrentTime作为时间*/
        public const UInt32 PLAY_CONTROL_STEMP = 7;	//*stemp,单帧播放*/
        public const UInt32 PLAY_CONTROL_SWITCH = 8;	//*切换文件,以szFileName作为文件名/或struTime时间*/
        public const UInt32 PLAY_CONTROL_MUTE = 9;	//*音频开关,以iEnableAudio作为开关*/
        public const UInt32 PLAY_CONTROL_UPEND = 10;	//*倒放*/
        public const UInt32 PLAY_CONTROL_GETAVINDEX = 11;	//*得到本地文件的索引*/
        public const UInt32 PLAY_CONTROL_SETAVINDEX = 12;	//*设置播放文件的索引*/
        public const UInt32 PLAY_CONTROL_AUTORESETBUFTIME = 13;	//*设置是否自动调节缓冲时间*/
        public const UInt32 PLAY_CONTROL_SEEKTIME_NEW = 14;	//*seek,以struTime作为时间  绝对时间   jukin add for gb*/


        [DllImport("tmControlClient.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TMCC_SetTimeOut(IntPtr flag, int dwTime);


        [DllImport("tmControlClient.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TMCC_RegisterConnectCallBack(IntPtr flag, TMCC_CONNECT_CALLBACK CALLBACK, IntPtr contone);


        [DllImport("tmControlClient.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr TMCC_Init(UInt32 flag);

        [DllImport("tmControlClient.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TMCC_SetAutoReConnect(IntPtr ptr, bool bShow);

        [DllImport("tmControlClient.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TMCC_SetDisplayShow(IntPtr ptr, bool bShow);

        [DllImport("tmControlClient.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TMCC_SetStreamBufferTime(IntPtr ptr, UInt32 dwTime);

        [DllImport("tmControlClient.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TMCC_Connect(IntPtr ptr, ref tmConnectInfo_t connectInfo, bool bSync);

        [DllImport("tmControlClient.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TMCC_GetConfig(IntPtr ptr, ref tmCommandInfo_t cmdInfo);

        [DllImport("tmControlClient.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TMCC_SetConfig(IntPtr ptr, ref tmCommandInfo_t cmdInfo);

        [DllImport("tmControlClient.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TMCC_SaveConfig(IntPtr ptr);

        [DllImport("tmControlClient.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TMCC_ConnectStream(IntPtr ptr, ref tmPlayRealStreamCfg_t info, IntPtr playHandler);

        [DllImport("tmControlClient.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TMCC_CloseStream(IntPtr ptr);

        [DllImport("tmControlClient.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TMCC_ClearDisplay(IntPtr ptr);

        [DllImport("tmControlClient.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TMCC_DisConnect(IntPtr flag);

        [DllImport("tmControlClient.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TMCC_Done(IntPtr flag);

        [DllImport("tmControlClient.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TMCC_CapturePictureToFile(IntPtr hTmCC, string pFileName, string pFmt);

        [DllImport("tmControlClient.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TMCC_SetOtherParam(IntPtr hTmCC, uint dwFlags, IntPtr buf, ref int iLen);

        [DllImport("tmControlClient.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TMCC_RegisterStreamCallBack(IntPtr ptr, StreamCallback back, IntPtr context);

        [DllImport("tmControlClient.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TMCC_RegisterAVFrameCallBack(IntPtr ptr, AvFrameCallback back, IntPtr context);

        [DllImport("tmControlClient.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TMCC_SetImageOutFmt(IntPtr ptr, uint iOutFmt);

        [DllImport("tmControlClient.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TMCC_GetLastError();

        [DllImport("tmControlClient.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr TMCC_FindFirstFile(IntPtr ptr, IntPtr ConditionCfg, IntPtr FileCfg);


        [DllImport("tmControlClient.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TMCC_FindNextFile(IntPtr ptr, IntPtr FileCfg);

        //[DllImport("tmControlClient.dll", CallingConvention = CallingConvention.Cdecl)]
        //public static extern IntPtr TMCC_OpenFile(IntPtr ptr, ref  tmPlayConditionCfg_t pPlayInfo, IntPtr hPlayWnd);

        [DllImport("tmControlClient.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr TMCC_OpenFile(IntPtr ptr, IntPtr pPlayInfo, IntPtr hPlayWnd);


        [DllImport("tmControlClient.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TMCC_GetFilePlayState(IntPtr ptr, IntPtr pPlayInfo);


        [DllImport("tmControlClient.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TMCC_ControlFile(IntPtr ptr, IntPtr hPlayWnd);


        [DllImport("tmControlClient.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr TMCC_ReadFile(IntPtr ptr, ref byte[] buf, int iReadSize);


        [DllImport("tmControlClient.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TMCC_CloseFile(IntPtr ptr);

        [DllImport("tmControlClient.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TMCC_FindCloseFile(IntPtr hTmFile);

        public static int Avdec_PlayToDo(IntPtr p, UInt32 com, int iSpeed)
        {
            tmPlayControlCfg_t cfg = new tmPlayControlCfg_t();

            cfg.dwSize = (UInt32)Marshal.SizeOf(cfg);
            cfg.dwCommand = com;
            cfg.iSpeed = iSpeed;

            IntPtr p5cfg = Marshal.AllocHGlobal(Marshal.SizeOf(cfg));
            Marshal.StructureToPtr(cfg, p5cfg, true);

            return TMCC_ControlFile(p, p5cfg);

        }

        public static int Avdec_SetCurrentTime(IntPtr p, UInt32 com, UInt32 fCurrentTime)
        {
            tmPlayControlCfg_t cfg = new tmPlayControlCfg_t();

            cfg.dwSize = (UInt32)Marshal.SizeOf(cfg);
            cfg.dwCommand = com;
            cfg.dwCurrentTime = fCurrentTime;

            IntPtr p5cfg = Marshal.AllocHGlobal(Marshal.SizeOf(cfg));
            Marshal.StructureToPtr(cfg, p5cfg, true);

            return TMCC_ControlFile(p, p5cfg);

        }
        public static tmPlayStateCfg_t Avdec_GetTmPlayStateCfg_t(IntPtr p)
        {
            tmPlayStateCfg_t cfg = new tmPlayStateCfg_t();


            cfg.dwSize = (UInt32)Marshal.SizeOf(cfg);

            IntPtr p5cfg = Marshal.AllocHGlobal(Marshal.SizeOf(cfg));
            Marshal.StructureToPtr(cfg, p5cfg, true);

            TMCC_GetFilePlayState(p, p5cfg);

            TMCC.tmPlayStateCfg_t tmcfg = (TMCC.tmPlayStateCfg_t)Marshal.PtrToStructure(p5cfg, typeof(TMCC.tmPlayStateCfg_t));

            return tmcfg;

        }



        //实时流回调
        public delegate void StreamCallback(IntPtr hTmCC, ref tmRealStreamInfo_t pStreamInfo, IntPtr context);
        //解码输出回调
        public delegate void AvFrameCallback(IntPtr hTmCC, ref tmAvImageInfo_t pStreamInfo, IntPtr context);

        //连接消息回调函数，通过它可以得到异步方式和断开连接状态
        public delegate void TMCC_CONNECT_CALLBACK(IntPtr hTmCC, bool bConnect, uint dwResult, IntPtr context);


        //连接消息回调函数，通过它可以得到异步方式和断开连接状态
        public delegate int fnStreamReadCallBackDelegate(IntPtr hTmCC, IntPtr pStreamInfo, IntPtr context);

        public class tmFileAccessInterface_t
        {
            public delegate IntPtr OpenDelegate(string lpFileName, string lpMode, IntPtr context);
            public OpenDelegate Open;
            public delegate int CloseDelegate(IntPtr hFile);
            public CloseDelegate Close;
            public delegate uint SeekDelegate(IntPtr hFile, int offset, int origin);
            public SeekDelegate Seek;
            public delegate int ReadDelegate(IntPtr hFile, IntPtr lpBuffer, int nRead);
            public ReadDelegate Read;
            public delegate int WriteDelegate(IntPtr hFile, IntPtr lpBuffer, int nWrite);
            public WriteDelegate Write;
            public delegate uint SizeDelegate(IntPtr hFile);
            public SizeDelegate Size;
        }

       [StructLayout(LayoutKind.Explicit)]
        public struct tmPlayControlCfg_t
        {
            [FieldOffset(0)]
            public UInt32 dwSize;
            [FieldOffset(4)]
            public UInt32 dwCommand;
            [FieldOffset(8)]
            public int iPlayData;
            [FieldOffset(8)]
            public int iSpeed;
            [FieldOffset(8)]
            public int iEnableAudio;
            [FieldOffset(8)]
            public int iCurrentPosition;
            [FieldOffset(8)]
            public UInt32 dwCurrentTime;
            [FieldOffset(8)]
            public bool bForward;
            [FieldOffset(8)]
            public bool bClearDisplay;
            [FieldOffset(8)]
            public bool bAutoResetBufTime;
            [FieldOffset(8)]
            public tmTimeInfo_t struTime;

        }
        //播放文件的当前信息
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public class tmPlayStateCfg_t
        {
            public uint dwSize; //本结构大小
            public byte byCurrentState; //当前播放状态
            public byte byResetTime; //需要复位时间戳
            public byte byResetFile; //需要复位时间戳
            public byte byIndex; //当前文件下载数

            public uint dwTotalFrames; //总共帧数
            public uint dwCurrentFrame; //当前帧数

            public uint dwTotalTimes; //总时间(毫秒)
            public uint dwCurrentTimes; //当前时间(毫秒)

            public tmTimeInfo_t struStartTime; //当前播放文件的开始时间

            public uint dwTotalSize; //总文件大小
            public uint dwCurrentSize; //当前文件大小

        }

        //文件索引结构定义
        public class tmAvIndexEntry_t
        {
            public uint ckid;
            public uint dwFlags;
            public uint dwChunkOffset;
            public uint dwChunkLength;
        }

        [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Size = 128)]
        public struct _time
        {
            public tmTimeInfo_t struStartTime; //文件的开始时间
            public tmTimeInfo_t struStopTime; //文件的结束时间
            public byte byCheckStopTime; //是否检测结束时间
            public byte byAlarmType; //报警类型
            public byte byFileFormat; //0-JPEG,1-JPEG2000,2-RGB555,3-RGB565,4-RGB24,
            public byte byBackupData; //是否是备份文件
            public byte byDiskName; //所在磁盘
            public byte byConvertToJpeg; //非JPEG强制转换成JPEG

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 18)]
            public char[] byReserves;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public char[] sServerAddress;


            public uint dwServerPort; //服务器端口
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public char[] sUserName;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public char[] sUserPass;

            //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 18)]
            //public string byReserves;
            //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            //public string sServerAddress; //服务器地址
            //public uint dwServerPort; //服务器端口
            //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            //public string sUserName; //用户名
            //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            //public string sUserPass; //用户密码
        }

        [StructLayoutAttribute(LayoutKind.Sequential, Size = 128)]
        public struct _file
        {
            public byte byAutoCreateIndex; //是否自动生成索引
            public byte byAutoPlay; //打开后是否自动播放
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] byTemp;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string sFileName; //文件名
            public tmFileAccessInterface_t pFileCallBack; //文件访问回调函数
            public IntPtr pFileContext; //文件访问相关句柄
            public tmAvIndexEntry_t pAvIndex; //索引缓冲
            public int iAvIndexCount; //缓冲中的索引数
        }

        //文件播放条件定义
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct tmPlayConditionCfg_t
        {
            public uint dwSize;
            public ushort wFactoryId; //厂商ID
            public byte byChannel; //通道 

            public tmTimeInfo_t struStartTime; //文件的开始时间
            public tmTimeInfo_t struStopTime; //文件的结束时间

            public byte byBufferBeforePlay; //开始播放是否需要缓冲数据
            public uint dwBufferSizeBeforePlay; //缓冲大小

            public tmFileAccessInterface_t pFileCallBack;			/*文件访问回调函数*/
            public IntPtr pFileContext;			/*文件访问相关句柄*/
            public tmAvIndexEntry_t pAvIndex;				/*索引缓冲*/
            public int iAvIndexCount;			/*缓冲中的索引数*/

            public fnStreamReadCallBackDelegate fnStreamReadCallBack;
            public IntPtr fnStreamReadContext;
        }




        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct tmConnectInfo_t
        {
            public uint dwSize;				//该结构的大小，必须填写
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public char[] pIp;			//连接服务器的IP地址
            public int iPort;				//服务器连接的端口
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public char[] szUser;			//登录用户名
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public char[] szPass;			//登录用户口令
            public int iUserLevel;			//登录用户级别，主要用户DVS的一些互斥访问资源
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] pUserContext;//用户自定义数据

            public void Init()
            {
                pUserContext = new byte[32];
            }
        }

        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct tmPlayRealStreamCfg_t
        {
            public UInt32 dwSize;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public char[] szAddress;	//连接服务器的IP地址
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public char[] szTurnAddress;//转发器地址
            public int iPort;		//服务器连接的端口
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public char[] szUser;	//登录用户名
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public char[] szPass;	//登录用户口令
            public byte byChannel;	//通道
            public byte byStream;	//码流号
            public byte byTranstType;	//传输类型
            public byte byReConnectNum;	//从连次数	
            public int iTranstPackSize;//传输包大小
            public int iReConnectTime;	//重连的时间间隔
            public byte byTransProtocol;//传输协议0-内部自定,1-SONY,2-RTSP	
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
            public char[] ok;	//登录用户口令
            public void Init()
            {
                ok = new char[128];
            }
        }

        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct tmRealStreamInfo_t
        {
            public uint dwSize;		//本结构大小
            public byte byFrameType;	//帧类型0-视频，1-音频，2-数据流头
            public byte byNeedReset;	//是否需要复位解码器
            public byte byKeyFrame;	//是否关键帧
            public byte byTemp;
            public uint dwFactoryId;	//厂家ID	
            public uint dwStreamTag;	//流类型Tag
            public uint dwStreamId;	//流ID
            //union
            //{
            //int	iWidth;		//视频宽
            public int iSamplesPerSec;	//音频采样率
            //int	iHeight;	//视频高
            public int iBitsPerSample;	//音频采样位数
            //};
            //union
            //{
            //    int	iFrameRate;	//帧率*1000
            public int iChannels;	//音频的声道数
            //};
            //add by 2009-0429
            //union
            //{
            public uint nDisplayScale;	//显示比例*1000
            //};
            public uint dwTimeStamp;	//时间戳(单位毫秒)
            public uint dwPlayTime;		//此帧播放时间(单位毫秒)
            public uint dwBitRate;		//此数据流的码流大小	
            public IntPtr pBuffer;		//数据缓冲
            public int iBufferSize;		//数据大小
        }

        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct tmFindConditionCfg_t
        {
            public uint dwSize; //本结构大小
            public byte byChannel; //搜索的通道
            public byte byFileType; //搜索类型 0xFF-全部，'N'-定时，'M'-移动，'A'-报警，'H'-手动，'O'-其它
            public byte bySearchAllTime; //搜索所有时间文件
            public byte bySearchImage; //是否搜索图片
            public tmTimeInfo_t struStartTime; //搜索的开始时间
            public tmTimeInfo_t struStopTime; //搜索的结束时间
            public byte byEnableServer; //是否启用网络参数
            public byte byOldServer; //是否是老设备
            public byte byBackupData; //是否搜索备份文件
            public byte byTemp;
            //public string sServerAddress = new string(new char[32]); //服务器地址
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string sServerAddress;	//连接服务器的IP地址

            public uint dwServerPort; //服务器端口
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string sUserName;	//连接服务器的IP地址
            // public string sUserName = new string(new char[32]); //用户名
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string sUserPass;	//连接服务器的IP地址
            //public string sUserPass = new string(new char[32]); //用户密码
        }


        //[StructLayoutAttribute(LayoutKind.Sequential, CharSet=CharSet.Ansi, Pack=1)]
        [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 2)]
        public struct tmFindFileCfg_t
        {
            public uint dwSize;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string sFileName;	//连接服务器的IP地址
            public byte byChannel; //搜索的通道

            [StructLayout(LayoutKind.Explicit)]
            struct s
            {
                [FieldOffset(0)]
                public byte byAlarmType; //搜索类型 0xFF-全部，'N'-定时，'M'-移动，'A'-报警，'H'-手动，'O'-其它
                [FieldOffset(0)]
                public byte byFileType; //搜索类型 0xFF-全部，'N'-定时，'M'-移动，'A'-报警，'H'-手动，'O'-其它
            }


            public ushort wFactoryId; //厂商ID
            public tmTimeInfo_t struStartTime; //搜索的开始时间
            public tmTimeInfo_t struStopTime; //搜索的结束时间
            public uint dwFileTime; //文件时间，毫秒
            public uint dwFileSize; //文件的大小(字节表示，所以录像文件不能太大)
            public byte byImage; //文件是否为图片
            public byte byFileFormat; //文件格式
            public byte byBackupData; //是否是备份文件
            public byte byDiskName; //所在磁盘
        }

        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct tmTimeInfo_t
        {
            public ushort wYear;
            public byte byMonth;
            public byte byDay;
            public byte byHour;
            public byte byMinute;
            public byte bySecond;
            public byte byTemp;
            public uint dwMicroSecond;

        }

        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct tmAvImageInfo_t
        {
            public byte video;
            public byte face;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] temp;
            public IntPtr buffer0;
            public IntPtr buffer1;
            public IntPtr buffer2;
            public IntPtr buffer3;
            public int bufsize0;
            public int bufsize1;
            public int bufsize2;
            public int bufsize3;
            _stuVideo videoInfo;
            public int key_frame;
            public uint timestamp;
        }

        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct _stuVideo
        {
            public short width;
            public short height;
            public int framerate;
            public byte format;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public byte[] temp;
        }

        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct tmCommandInfo_t
        {
            public uint dwSize;		        //该结构的大小，必须填写为sizeof(tmCommandInfo_t)	
            public uint dwMajorCommand;		//主消息数据命令即数据类型
            public uint dwMinorCommand;		//次消息数据命令即数据类型	
            public ushort iChannel;         //通道号，该通道号要根据dwMajorCommand来判断是否有效
            public ushort iStream;          //子通道号，该通道号要根据dwMajorCommand来判断是否有效
            public IntPtr pCommandBuffer;   //消息数据缓冲
            public int iCommandBufferLen;   //消息数据缓冲大小
            public int iCommandDataLen;     //消息数据实际大小
            public uint dwResult;           //消息控制返回结果
        }

        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct tmToManagerImageInfo_t
        {
            public uint dwSize;
            public _stuImage image;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] byTemp;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public byte[] byMACAddr;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] byTemp2;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] szServerIP;
            public byte byImageFmt;

            public byte byCount;
            public byte byIndex;
            public byte byImageMode;
            public byte byAlarmId;
            public byte byChannelId;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] byOtherInfo;
            public _stuTime time;
            public uint dwImageSize;
        }

        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct _stuImage
        {
            public short nWidth;
            public short nHeight;
            public byte byBitCount;
            public byte byRevolving;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] byTemp;
        };

        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct _stuTime
        {
            public short nYear;
            public byte nMonth;
            public byte nDay;
            public byte nDayOfWeek;
            public byte nHour;
            public byte nMinute;
            public byte nSecond;
        }

        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct tmVideoInCfg_t
        {
            public uint dwSize;
            public byte byAntiFlickerMode;
            public byte byVideoColorStyle;
            public byte byRotaeAngle180;
            public byte byColorTransMode; /*彩转黑模式0-自动，1-彩色，2-黑白*/
            public byte byShutterSpeed;
            public byte byAgc;
            public byte byIRShutMode;
            public byte byExposure;
            public byte byIRStartHour;
            public byte byIRStartMin;
            public byte byIRStopHour;
            public byte byIRStopMin;
            public byte byModeSwitch;
            public byte byWhiteBalance;
            public byte byWdr;
            public byte byBlc;
            public ushort nWhiteBalanceR;
            public ushort nWhiteBalanceB;
            public byte byMctfStrength;
            public byte byIRType;
            public byte byIRCutTriggerAlarmOut;
            public byte byIRCutTime;
            public byte byExposureLevel;
            public byte byColorTransMin;
            public byte byColorTransMax;
            public byte byNoiseFilter;
            public byte byForceNoiseFilter;
            public byte byAeMeteringMode;
            public byte byWdrMode;
            public byte byIRShutAlarmIn;
            public byte byAutoContrast;
            public byte byLightInhibitionEn;
            public byte byVinFrameRate;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
            public byte[] byTemp;
            public byte byAgcTransMin;
            public byte byAgcTransMax;
            public ushort nMaxShutterSpeed;
            public ushort nMaxAgc;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 96)]
            public byte[] byAeMeteringData;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] byExposureLevelHdr;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public ushort[] nMinShutterSpeedHdr;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public ushort[] nMaxShutterSpeedHdr;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] byMaxAgcHdr;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4 * 96)]
            public byte[] byAeMeteringDataHdr;
        }
    }
}
