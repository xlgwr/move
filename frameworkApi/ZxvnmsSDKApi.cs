using System;
using System.Collections.Generic;
//using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace AnXinWH.ShiPin
{
    public class ZxvnmsSDKApi
    {
        #region 常量
        const int MAX_VIDEO_PLAY_COUNT = 20;	// 最大可同时实时浏览的路数
        const int MAX_FILE_PLAY_COUNT = 20;	// 最大可支持的文件回放路数
        const int MAX_FILE_DOWNLOAD_COUNT = 20;	// 最大可支持的文件下载路数
        string EMPTY_STRING = "";	// 空字符串

        const int MAX_LEN_21 = 21;				// 
        const int MAX_LEN_51 = 51;				// 
        const int MAX_LEN_16 = 16;				// 
        const int MAX_LEN_255 = 255;
        #endregion
        #region error no
        const UInt32 ERROR_NULL_POINTER = 0xE0002001; // 空指针错误
        const UInt32 ERROR_INVALID_ARGUMENT = 0xE0002002; // 非法参数错误
        const UInt32 ERROR_PROTOCOL_PARSE = 0xE0002003;  // 协议解析错误
        const UInt32 REQUEST_VIDEO_ROUTER_FAIL = 0xE0002103; // 请求视频路由失败
        const UInt32 REQUEST_VIDEO_ROUTER_ERROR = 0xE0002104; // 服务器端获取路由错误
        const UInt32 NO_AVALIABLE_VIDEO_ROUTER = 0xE0002105; // 无可用路由
        const UInt32 PU_NOT_REGISTER = 0xE0002112; // 前端PU未注册
        const UInt32 PU_REQUEST_FAULT = 0xE0002113; // 前端PU请求出错
        const UInt32 PU_AUDIO_CHANNEL_USED = 0xE0002114; // 前端PU语音通道已占用
        const UInt32 REQUEST_TIME_OUT = 0xE0002211; // 请求超时
        const UInt32 ERROR_SERVER_NOT_CONNECT = 0xE0002216; // 服务器未连接
        const UInt32 ERROR_ACCESS_DATABASE = 0xE0002301; // 访问数据库失败
        const UInt32 ERROR_NO_RECORD = 0xE0002302; // 数据库中无相关纪录
        const UInt32 ERROR_USER_SUSPEND = 0xE0002310; // 用户已停用
        const UInt32 ERROR_NO_AUTHORITY = 0xE0002311; // 没有执行操作的权限
        const UInt32 ERROR_NO_USER = 0xE0002312; // 用户不存在
        const UInt32 ERROR_INVALID_PWD = 0xE0002313; // 密码错
        const UInt32 ERROR_MAX_LOGINED = 0xE0002314; // 同时登陆用户数已达到最大
        const UInt32 ERROR_USERINFO_NULL = 0xE0002317; // 返回用户信息有误
        const UInt32 ERROR_USER_LOGINING = 0xE0002318; // 客户端已经有用户登陆
        const UInt32 ERROR_MAX_GROUP_LOGIN = 0xE0002319; // 所在大客户登陆用户数达到最大
        const UInt32 ERROR_USER_NAME_REPEATED = 0xE0002320; // 用户名重复
        const UInt32 ERROR_MAX_USER_COUNT = 0xE0002321; // 创建用户数已达到最大
        const UInt32 ERROR_GROUP_NAME_REPEQTED = 0xE0002322; // 创建的用户组名称重复
        const UInt32 ERROR_BUFFER_NOT_ENOUGH = 0xE0002401; // 缓冲区不足
        const UInt32 ERROR_SESSION_NOT_INIT = 0xE0002420; // 客户端session没有初始化，或者初始
        #endregion
        #region help dll C++ to C#
        //        C++            C#
        //=====================================
        //WORD              ushort
        //DWORD             uint
        //UCHAR             int/byte   大部分情况都可以使用int代替,而如果需要严格对齐的话则应该用bytebyte 
        //UCHAR*            string/IntPtr
        //unsigned char*    [MarshalAs(UnmanagedType.LPArray)]byte[]/？（Intptr）
        //char*             string
        //LPCTSTR           string
        //LPTSTR            [MarshalAs(UnmanagedType.LPTStr)] string
        //long              int
        //ulong             uint
        //Handle            IntPtr
        //HWND              IntPtr
        //void*             IntPtr
        //int               int
        //int*              ref int
        //*int              IntPtr
        //unsigned int      uint
        //COLORREF          uint 
        //Marshal.PtrToStringAnsi(IntPtr result) 来得到返回字符串
        #endregion

        #region 当前窗口句柄
        //        示例:
        //InPtr awin = GetForegroundWindow(); //获取当前窗口句柄
        //RECT rect = new RECT();
        //GetWindowRect(awin, ref rect);
        //int width = rc.Right - rc.Left; //窗口的宽度
        //int height = rc.Bottom - rc.Top; //窗口的高度
        //int x = rc.Left;
        //int y = rc.Top;
        //获取当前窗口句柄:GetForegroundWindow()
        /// <summary>
        /// 返回值类型是IntPtr,即为当前获得焦点窗口的句柄
        //使用方法 : IntPtr myPtr=GetForegroundWindow();
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindow();


        /// <summary>
        ///  //获取到该窗口句柄后,可以对该窗口进行操作.比如,关闭该窗口或在该窗口隐藏后,使其显示
        ///  其中ShowWindow(IntPtr hwnd, int nCmdShow);
        //nCmdShow的含义
        //0 关闭窗口
        //1 正常大小显示窗口
        //2 最小化窗口
        //3 最大化窗口
        //使用实例: ShowWindow(myPtr, 0);
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="nCmdShow"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left; //最左坐标
            public int Top; //最上坐标
            public int Right; //最右坐标
            public int Bottom; //最下坐标
        }
        #endregion

        /// <summary>
        /// 初始化接口
        /// </summary>
        /// <returns>返回值：0 成功 错误时返回错误号</returns>
        [DllImport("ZxvnmsSDK.dll")]
        public static extern int ZXVNMS_Init();

        /// <summary>
        /// 资源释放接口，关闭所有的连接
        /// </summary>
        /// <returns>返回值：0 成功 错误时返回错误号</returns>
        [DllImport("ZxvnmsSDK.dll")]
        public static extern int ZXVNMS_Free();



        /// <summary>
        /// 登录到指定平台，当创建多个控件实例时，对于目标平台只需要登录一次
        /// </summary>
        /// <param name="cmsip"></param>
        /// <param name="cmsPort"></param>
        /// <param name="userName"></param>
        /// <param name="pswd"></param>
        /// <param name="ValidateType"></param>
        /// <param name="UserMacAddr"></param>
        /// <param name="UserUsbKey"></param>
        /// <param name="Bound"></param>
        /// <returns>返回值：>0 成功，返回一个会话句柄，错误时返回错误号</returns>
        [DllImport("ZxvnmsSDK.dll")]
        public static extern int ZXVNMS_InitSession(
           string cmsip,
           int cmsPort,
           string userName,
           string pswd,
           int ValidateType,
           string UserMacAddr,
           string UserUsbKey,
           int Bound);

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="handle">handle ZXVNMS_InitSession 返回的句柄</param>
        /// <returns>返回值：0 成功 错误时返回错误号</returns>
        [DllImport("ZxvnmsSDK.dll")]
        public static extern int ZXVNMS_TerminateSession(int handle);

        /// <summary>
        /// 获取当前会话用户ID
        /// </summary>
        /// <returns>返回当前会话的用户ID，失败返回空字符串（””）</returns>
        [DllImport("ZxvnmsSDK.dll")]
        public static extern IntPtr ZXVNMS_GetUserID();

        /// <summary>
        /// 在多个已登录的会话之间切换，把handle切换为当前会话
        /// </summary>
        /// <param name="handle">handle ZXVNMS_InitSession 返回的句柄</param>
        /// <returns>0 成功 错误时返回错误号</returns>
        [DllImport("ZxvnmsSDK.dll")]
        public static extern int ZXVNMS_SwitchCMS(int handle);

        /// <summary>
        /// 2.2.5.	获取用户类型
        /// </summary>
        /// <returns>>=0成功，详见具体用户类型，<0失败</returns>
        [DllImport("ZxvnmsSDK.dll")]
        public static extern int ZXVNMS_GetUserType();

        /// <summary>
        /// 2.2.6.	获取会话ID
        /// </summary>
        /// <returns>返回当前会话ID，失败返回空字符串（””）</returns>
        [DllImport("ZxvnmsSDK.dll")]
        public static extern IntPtr ZXVNMS_GetSessionID();

        /// <summary>
        /// 2.2.7.	获取中心服务器ID 获取当前会话中心服务器IP地址
        /// </summary>
        /// <returns>返回当前会话中心服务器IP地址，失败返回空字符串（””）</returns>
        [DllImport("ZxvnmsSDK.dll")]
        public static extern IntPtr ZXVNMS_GetCMSIP();

        /// <summary>
        /// 2.2.8.	获取中心服务器端口
        /// </summary>
        /// <returns>返回当前会话中心服务器端口，失败返回0</returns>
        [DllImport("ZxvnmsSDK.dll")]
        public static extern int ZXVNMS_GetCMSPort();

        //2.3.	数据集查询、遍历（设备列表、预置位、录像文件等


        /// <summary>
        /// 2.3.1.	设备查询 查询用户有权限的所有指定类型的设备信息
        /// </summary>
        /// <param name="devType">devType 设备类型 
        ///    OFFICE = 0x00,			局站	
        ///    PLATFORM_DEVICE = 0x01,	平台设备
        ///    ENCODER  = 0x02,       	编码器 
        ///    DECODER  = 0x03,       	解码器
        ///    CAMERA   = 0x04,       	摄像头
        ///    DI_DEVICE = 0x05,    	   	告警输入设备DI
        ///    DO_DEVICE = 0x06,       	告警输出设备
        ///</param>
        /// <returns>
        /// 0 查询成功 错误时返回错误号，查询成功后使用ZXVNMS_MoveNext()，
        /// ZXVNMS_GetValueStr()，ZXVNMS_GetValueInt()逐条获取设备信息
        /// </returns>
        [DllImport("ZxvnmsSDK.dll")]
        public static extern int ZXVNMS_QueryDevices(int devType);


        /// <summary>
        /// 2.3.2.	预置位查询 查询指定摄像头的预置位信息
        /// </summary>
        /// <param name="cameraID">cameraID 摄像头ID</param>
        /// <returns>
        /// 0 查询成功 错误时返回错误号，查询成功后使用ZXVNMS_MoveNext()，
        /// ZXVNMS_GetValueStr()，ZXVNMS_GetValueInt()逐条获取预置位信息
        /// </returns>
        [DllImport("ZxvnmsSDK.dll")]
        public static extern int ZXVNMS_QueryPreset(IntPtr cameraID);


        /// <summary>
        /// 2.3.3.	录像文件查询
        /// </summary>
        /// <param name="pszPUID">编码器ID</param>
        /// <param name="nChannel">通道号</param>
        /// <param name="startTime">查询录像的开始时间 YYYY-MM-DD HH:mm:ss</param>
        /// <param name="endTime">查询录像的结束时间 YYYY-MM-DD HH:mm:ss</param>
        /// <param name="isForword">录像所在位置 0—中心存储 1—前端存储</param>
        /// <returns>
        /// 0 查询成功 错误时返回错误号，查询成功后使用ZXVNMS_MoveNext()，
        /// ZXVNMS_GetValueStr()，ZXVNMS_GetValueInt()逐条获取录像信息
        /// </returns>
        [DllImport("ZxvnmsSDK.dll")]
        public static extern int ZXVNMS_QueryRecordFile(
            IntPtr pszPUID,
            int nChannel,
            string startTime,
            string endTime,
            int isForword
            );

        /// <summary>
        /// 查询指定摄像头某一时间段内的录像文件信息
        /// </summary>
        /// <param name="cameraID">摄像头ID</param>
        /// <param name="startTime">查询录像的开始时间 YYYY-MM-DD HH:mm:ss</param>
        /// <param name="endTime">查询录像的结束时间 YYYY-MM-DD HH:mm:ss</param>
        /// <param name="isForword">locate		录像所在位置 0—中心存储 1—前端存储</param>
        /// <returns>
        /// 返回值：0 查询成功 错误时返回错误号，查询成功后使用ZXVNMS_MoveNext()，
        /// ZXVNMS_GetValueStr()，ZXVNMS_GetValueInt()逐条获取录像信息
        /// 
        /// </returns>
        [DllImport("ZxvnmsSDK.dll")]
        public static extern int ZXVNMS_QueryRecordFileEx(
            string cameraID,
            string startTime,
            string endTime,
            int isForword,
            int locate
            );

        /// <summary>
        /// 2.3.4.	游标向后移动 向后移动数据访问游标
        /// </summary>
        /// <returns>0 数据未读取完， -1 数据读取完成，</returns>
        [DllImport("ZxvnmsSDK.dll")]
        public static extern int ZXVNMS_MoveNext();


        /// <summary>
        /// 2.3.5.	获取设备字符串属性
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns>
        /// propertyName 属性名称，各种设备的属性名称接口头文件中均有定义
        ///返回值：字符串属性值，如果没有返回””
        /// </returns>
        [DllImport("ZxvnmsSDK.dll")]
        public static extern IntPtr ZXVNMS_GetValueStr(string propertyName);

        /// <summary>
        /// 获取设备的整形属性
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns>
        /// propertyName 属性名称，各种设备的属性名称接口头文件中均有定义
        /// 返回值：设备整形类型属性，获取属性失败返回INT_MIN 
        /// </returns>
        [DllImport("ZxvnmsSDK.dll")]
        public static extern int ZXVNMS_GetValueInt(string propertyName);

        //*******************2.4.	浏览实时视频

        /// <summary>
        /// 2.4.1.	播放实时视频 开始实时浏览视频
        /// </summary>
        /// <param name="cameraID">摄像头ID</param>
        /// <param name="hWnd">播放窗口句柄</param>
        /// <returns>返回值：>=0成功，返回实时视频句柄，<0失败</returns>
        [DllImport("ZxvnmsSDK.dll")]
        public static extern int ZXVNMS_PlayVideo(string cameraID, IntPtr hWnd);


        /// <summary>
        /// 2.4.2.	停止实时视频 停止实时视频浏览
        /// </summary>
        /// <param name="handle">handle ZXVNMS_PlayVideo返回的句柄</param>
        /// <returns>0 成功 错误时返回错误号</returns>
        [DllImport("ZxvnmsSDK.dll")]
        public static extern int ZXVNMS_StopVideo(int handle);

        /// <summary>
        /// 2.4.3.	获取实时视频、文件回放码率
        /// </summary>
        /// <param name="handle">ZXVNMS_PlayVideo,ZXVNMS_PlayFile返回的句柄</param>
        /// <param name="pVal">实时视频、文件回放码率（kbps）指针</param>
        /// <returns></returns>
        [DllImport("ZxvnmsSDK.dll")]
        public static extern int ZXVNMS_GetVideoDataRate(int handle, IntPtr pVal);

        [DllImport("ZxvnmsSDK.dll")]
        public static extern int ZXVNMS_GetDownloadRemainTime(int handle, IntPtr pVal);

        //**************2.5.	请求视频码流数据

        /// <summary>
        /// 视频码流接收回调函数指针 delete
        /// </summary>
        /// <param name="handle">ZXVNMS_StartVideoStream返回的句柄</param>
        /// <param name="data">接收视频码流数据缓冲区指针</param>
        /// <param name="size">接收视频码流数据字节数</param>
        /// <param name="pUser">用户数据</param>
        public delegate void fStreamCallback(int handle, int dataType, IntPtr data, int size, IntPtr pUser);

        /// <summary>
        /// 2.5.6.	设置YUV数据回调函数
        /// </summary>
        /// <param name="handle"> ZXVNMS_PlayVideo或ZXVNMS_PlayFile返回的handle</param>
        /// <param name="data">接收数据缓冲区地址</param>
        /// <param name="size">接收数据字节数</param>
        /// <param name="width">图片宽度</param>
        /// <param name="height">图片高度</param>
        /// <param name="pUser">用户参数</param>
        public delegate void fYUVCallback(int handle, IntPtr data, int size, int width, int height, IntPtr pUser);



        /// <summary>
        /// 2.5.1.	设置视频码流接收回调函数
        /// </summary>
        /// <param name="fStreamCallback"></param>
        /// <param name="pUser"></param>
        /// <returns>0 成功 错误时返回错误号</returns>
        [DllImport("ZxvnmsSDK.dll")]
        public static extern int ZXVNMS_SetVideoStreamCallback(fStreamCallback fStreamCallback, IntPtr pUser);

        /// <summary>
        /// 2.5.6.	设置YUV数据回调函数
        /// </summary>
        /// <param name="fYUVCallback"></param>
        /// <param name="pUser"></param>
        /// <returns>0 成功 错误时返回错误号</returns>
        [DllImport("ZxvnmsSDK.dll")]
        public static extern int ZXVNMS_SetVideoStreamCallback(fYUVCallback fYUVCallback, IntPtr pUser);


        /// <summary>
        /// 2.5.2.	开始实时视频码流
        /// 开始实时视频码流数据，数据通过ZXVNMS_SetVideoStreamCallback设置的回调	函数返回
        /// </summary>
        /// <param name="cameraID"></param>
        /// <returns>>=0 成功,表示开始实时视频流句柄，<0 错误，表示错误号</returns>
        [DllImport("ZxvnmsSDK.dll")]
        public static extern int ZXVNMS_StartVideoStream(string cameraID);


        /// <summary>
        /// 停止实时视频码流数据
        /// </summary>
        /// <param name="handle">handle ZXVNMS_StartVideoStream返回的句柄</param>
        /// <returns>0 成功 错误时返回错误号</returns>
        [DllImport("ZxvnmsSDK.dll")]
        public static extern int ZXVNMS_StopVideoStream(int handle);


        /// <summary>
        /// 2.5.4.	开始视频文件流数据 
        /// 开始视频文件流数据，
        ///  数据通过ZXVNMS_SetVideoStreamCallback设置的回调	函数返回
        /// </summary>
        /// <param name="cameraID"></param>
        /// <param name="filename"></param>
        /// <param name="filesize"></param>
        /// <param name="locate">文件位置 0:中心存储 1:前端存储</param>
        /// <returns>>=0 成功,表示开始视频文件流句柄，<0 错误，表示错误号</returns>
        [DllImport("ZxvnmsSDK.dll")]
        public static extern int ZXVNMS_StartVideoFileStream(
            string cameraID,
            string filename,
            int filesize,
            int locate);


        /// <summary>
        /// 2.5.5.	停止视频文件流数据
        /// </summary>
        /// <param name="handle">handle ZXVNMS_StartVideoFileStream返回的句柄</param>
        /// <returns>0 成功 错误时返回错误号</returns>
        [DllImport("ZxvnmsSDK.dll")]
        public static extern int ZXVNMS_StopVideoFileStream(int handle);


        //*******************2.6.	云镜控制
        //

        /// <summary>
        /// 云镜控制
        /// </summary>
        ///// <param name="cameraID"></param>
        /// <param name="dwPTZCommand">控制命令
        /// :  设置预置位
        ///9:  删除预置位
        ///11: 镜头拉近
        ///12: 镜头移远
        ///13: 焦点前调
        ///14: 焦点后调
        ///15: 光圈扩大
        ///16: 光圈缩小
        ///21: 镜头向上
        ///22: 镜头向下
        ///23: 镜头向左
        ///24: 镜头向右
        ///39: 到达预置位
        ///</param>
        /// <param name="param1">开始动作 0 停止动作，
        /// 当dwPTZCommand为8,9,39时，param1为预置位序	
        /// 号，最多255,具体和球机有关</param>
        /// <param name="param2">预留</param>
        /// <param name="param3">预留</param>
        /// <param name="param4">预留</param>
        /// <returns></returns>
        [DllImport("ZxvnmsSDK.dll")]
        public static extern int ZXVNMS_ControlCamera(string cameraID, int dwPTZCommand, int param1, int param2, int param3, int param4);


        /// <summary>
        /// 具有争控功能，且不支持万能控制协议的平台，
        /// 用此接口控制云台。调用此接口时，如没有控制权限，会自动请求控制权限
        /// </summary>
        /// <param name="cameraID"></param>
        /// <param name="dwPTZCommand"></param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        /// <param name="param3"></param>
        /// <param name="param4"></param>
        /// <returns></returns>
        [DllImport("ZxvnmsSDK.dll")]
        public static extern int ZXVNMS_ControlCameraAutoReq(
            string cameraID,
            int dwPTZCommand,
            int param1,
            int param2,
            int param3,
            int param4);

        /// <summary>
        /// 2.6.3.	云镜控制（通过编码器id和通道号）
        /// </summary>
        /// <param name="pszPUID"></param>
        /// <param name="nChannel"></param>
        /// <param name="dwPTZCommand"></param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        /// <param name="param3"></param>
        /// <param name="param4"></param>
        /// <returns></returns>
        [DllImport("ZxvnmsSDK.dll")]
        public static extern int ZXVNMS_ControlCameraEx(
            string pszPUID,
            int nChannel,
            int dwPTZCommand,
            int param1,
            int param2,
            int param3,
            int param4);


        //**********************2.7.	录像回放、下载


        /// <summary>
        /// 2.7.1.	开始文件回放
        /// </summary>
        /// <param name="cameraID"></param>
        /// <param name="filename"></param>
        /// <param name="locate">录像所在位置 0—中心存储 1—前端存储</param>
        /// <param name="hWnd">文件播放窗口句柄</param>
        /// <returns>>=0成功，返回文件播放句柄，<0失败</returns>
        [DllImport("ZxvnmsSDK.dll")]
        public static extern int ZXVNMS_PlayFile(
                    string cameraID,
                    string filename,
                    int filesize,
                    int locate,
                    IntPtr hWnd);

        /// <summary>
        /// 2.7.2.	停止文件回放
        /// </summary>
        /// <param name="handle">handle ZXVNMS_PlayFile返回的句柄</param>
        /// <returns>0 成功 错误时返回错误号</returns>
        [DllImport("ZxvnmsSDK.dll")]
        public static extern int ZXVNMS_StopFilePlay(int handle);


        /// <summary>
        /// 2.7.3.	设置文件回放偏移量
        /// </summary>
        /// <param name="handle"> ZXVNMS_PlayFile返回的句柄</param>
        /// <param name="offset">文件偏移百分比 0~100</param>
        /// <returns></returns>
        [DllImport("ZxvnmsSDK.dll")]
        public static extern int ZXVNMS_SetFilePlayOffset(int handle, int offset);


        /// <summary>
        /// 2.7.4.	暂停文件回放
        /// </summary>
        /// <param name="handle"> ZXVNMS_PlayFile返回的句柄</param>
        /// <param name="bPause"> true：暂停	false：取消暂停</param>
        /// <returns></returns>
        [DllImport("ZxvnmsSDK.dll")]
        public static extern int ZXVNMS_PauseFilePlay(int handle, bool bPause);

        /// <summary>
        /// 2.7.5.	慢速播放 慢速播放，播放速度减半
        /// </summary>
        /// <param name="handle">handle ZXVNMS_PlayFile返回的句柄</param>
        /// <returns></returns>
        [DllImport("ZxvnmsSDK.dll")]
        public static extern int ZXVNMS_SlowFilePlay(int handle);

        /// <summary>
        /// 2.7.6.	快速播放 快速播放，播放速度加倍
        /// </summary>
        /// <param name="handle">handle ZXVNMS_PlayFile返回的句柄</param>
        /// <returns></returns>
        [DllImport("ZxvnmsSDK.dll")]
        public static extern int ZXVNMS_FastFilePlay(int handle);


        /// <summary>
        /// 2.7.7.	开始文件下载
        /// </summary>
        /// <param name="cameraID">摄像头ID</param>
        /// <param name="filename">录像文件名</param>
        /// <param name="locate">录像所在位置 0—中心存储 1—前端存储</param>
        /// <param name="savefilepath">本地文件保存全路径</param>
        /// <returns>>=0成功，返回文件下载句柄，<0失败</returns>
        [DllImport("ZxvnmsSDK.dll")]
        public static extern int ZXVNMS_DownloadFile(
             string cameraID,
             string filename,
             int size,
             int locate,
             string savefilepath);


        /// <summary>
        /// 2.7.8.	停止文件下载
        /// </summary>
        /// <param name="handle">handle ZXVNMS_DownloadFile返回的句柄</param>
        /// <returns>：0 成功 错误时返回错误号</returns>
        [DllImport("ZxvnmsSDK.dll")]
        public static extern int ZXVNMS_StopFileDownload(int handle);


        /// <summary>
        /// 2.7.9.	获取文件回放、下载百分比
        /// </summary>
        /// <param name="handle"> ZXVNMS_DownloadFile返回的句柄</param>
        /// <param name="pVal">文件回放、下载百分比指针</param>
        /// <returns>0 成功 错误时返回错误号</returns>
        [DllImport("ZxvnmsSDK.dll")]
        public static extern int ZXVNMS_GetFilePercent(int handle, IntPtr pVal);


        /// <summary>
        /// 2.8.1.	视频抓图
        /// </summary>
        /// <param name="handle">ZXVNMS_PlayVideo,ZXVNMS_PlayFile返回的句柄</param>
        /// <param name="savefilepath">本地文件保存全路径</param>
        /// <returns>0 成功 错误时返回错误号</returns>
        [DllImport("ZxvnmsSDK.dll")]
        public static extern int ZXVNMS_CapPic(int handle, string savefilepath);


        /// <summary>
        /// 2.8.2.	视频抓图到内存
        /// </summary>
        /// <param name="handle">视频句柄</param>
        /// <param name="pBuf">上层开的缓冲，用来保存YUV数据</param>
        /// <param name="nBufSize">缓冲大小</param>
        /// <param name="pWidth">图片宽度指针，函数调用成功后将YUV图片的宽度通过该指针上传给上层</param>
        /// <param name="pHeight">图片高度指针，函数调用成功后将YUV图片的高度通过该指针上传给上层</param>
        /// <returns>0 成功 错误时返回错误号</returns>
        [DllImport("ZxvnmsSDK.dll")]
        public static extern int ZXVNMS_SnapPicYUV(int handle, ref byte[] pBuf, int nBufSize, ref int pWidth, ref int pHeight);

        /// <summary>
        /// 2.8.3.	开始本地录像
        /// </summary>
        /// <param name="handle"> ZXVNMS_PlayVideo,ZXVNMS_PlayFile返回的句柄</param>
        /// <param name="savefilepath">本地文件保存全路径</param>
        /// <returns></returns>
        [DllImport("ZxvnmsSDK.dll")]
        public static extern int ZXVNMS_StartRecordFile(int handle, string savefilepath);

        /// <summary>
        /// 2.8.4.	停止本地录像
        /// </summary>
        /// <param name="handle">ZXVNMS_PlayVideo,ZXVNMS_PlayFile返回的句柄</param>
        /// <returns></returns>
        [DllImport("ZxvnmsSDK.dll")]
        public static extern int ZXVNMS_StopRecordFile(int handle);

        //**************2.12.	智能分析


        /// <summary>
        /// 2.12.1.	查询平台支持的智能分析算法类型
        /// </summary>
        /// <returns>
        /// 0 成功，失败时返回错误号，查询成功后使用ZXVNMS_MoveNext()，
        /// ZXVNMS_GetValueStr()，ZXVNMS_GetValueInt()逐条获取查询结果信息 
        /// </returns>
        [DllImport("ZxvnmsSDK.dll")]
        public static extern int ZXVNMS_IA_QueryAlgType();


        /// <summary>
        /// 2.12.2.	查询平台支持的智能分析事件类型
        /// </summary>
        /// <returns>
        ///  0 成功，失败时返回错误号，查询成功后使用ZXVNMS_MoveNext()，
        ///  ZXVNMS_GetValueStr()，ZXVNMS_GetValueInt()逐条获取查询结果信息
        /// 
        /// </returns>
        [DllImport("ZxvnmsSDK.dll")]
        public static extern int ZXVNMS_IA_QueryEventType();


        /// <summary>
        /// 2.12.3.	查询摄像头视频智能分析任务
        /// </summary>
        /// <param name="cameraID"></param>
        /// <returns>
        /// 
        /// 0 成功，失败时返回错误号，查询成功后使用ZXVNMS_MoveNext()，
        /// ZXVNMS_GetValueStr()，ZXVNMS_GetValueInt()逐条获取查询结果信息
        /// </returns>
        [DllImport("ZxvnmsSDK.dll")]
        public static extern int ZXVNMS_IA_QueryTask(string cameraID);

        /// <summary>
        /// 2.13.1.	摄像头状态查询
        /// </summary>
        /// <param name="cameraID"></param>
        /// <param name="currState">当前状态（输出参数），详见ZXVNMS_CameraState
        ///返回值： 0 成功，失败时返回错误号
        ///</param>
        /// <returns></returns>
        [DllImport("ZxvnmsSDK.dll")]
        public static extern int ZXVNMS_GetState(string cameraID, ref int currState);

        /// <summary>
        /// 根据错误号获取错误信息
        /// </summary>
        /// <param name="errcode">错误号</param>
        /// <param name="pBuf">错误信息缓冲区（输出参数）</param>
        /// <param name="bufSize">错误信息缓冲区字节数（输出参数）</param>
        /// <returns>0 成功，失败时返回错误号</returns>
        [DllImport("ZxvnmsSDK.dll")]
        public static extern int ZXVNMS_GetErrorInfo(int errcode, IntPtr pBuf, IntPtr bufSize);


        /// <summary>
        /// 2.13.3.	获取服务器运行状态
        /// </summary>
        /// <param name="serverID">服务器ID</param>
        /// <param name="serverType">服务器类型
        /// 0x0001	中心服务器
        /// 0x0002	接入服务器
        /// 0x0004	视频转发服务器
        /// 0x0008	存储服务器
        /// 0x1004	物理中心服务器
        /// </param>
        /// <param name="cpu">cpu占用率(%)（输出参数）</param>
        /// <param name="mem">内存占用率(%)（输出参数）</param>
        /// <param name="netsend">网络收(Bytes/s) （输出参数）</param>
        /// <param name="netrecv">网络发(Bytes/s) （输出参数）</param>
        /// <returns></returns>
        [DllImport("ZxvnmsSDK.dll")]
        public static extern int ZXVNMS_GetServerRunState(
                string serverID,
                    int serverType,
                    ref int cpu,
                    ref int mem,
                    ref int netsend,
                    ref int netrecv);

    }


    #region model


    public class configHost
    {
        public string cmsip { get; set; }
        public int cmsPort { get; set; }
        public string userName { get; set; }
        public string pswd { get; set; }
        public int ValidateType { get; set; }
        public string UserMacAddr { get; set; }
        public string UserUsbKey { get; set; }
        public int Bound { get; set; }
    }

    /// <summary>
    /// // 查询结果类型
    /// </summary>
    public class ZXVNMS_DevType
    {
        public static int OFFICE = 0x00;			/**< 局站 */
        public static int PLATFORM_DEVICE = 0x01;	/**< 平台设备 */
        public static int ENCODER = 0x02;			/**< 编码器 */
        public static int DECODER = 0x03;			/**< 解码器 */
        public static int CAMERA = 0x04;			/**< 摄像头 */
        public static int DI_DEVICE = 0x05;			/**< 告警设备DI */
        public static int DO_DEVICE = 0x06;			/**< DO设备 */
        public static int TV_WALL = 0x07;           /**< 电视墙 */

        public static int PRESET = 0x10;			/**< 预置位 */
        public static int RECORDFILE = 0x11;		/**< 录像文件 */
        public static int ALARMLINKAGE = 0x12;		/**< 告警联动 */

        public static int PLATE_DOMAIN = 0x20;		/**< 平台域 */
    }
    // 平台域的属性名
    public class ZXVNMS_Domain
    {
        public static string Domain = "domain";
        public static string Id = "id";
        public static string Name = "name";
        public static string Ip = "ip";
        public static string Port = "port";
        public static string Link_mode = "link_mode";
        public static string CmsType = "cmsType";
        public static string Mduip = "mduip";
        public static string Mduport = "mduport";
        public static string State = "state";
        public static string Domain_type = "domain_type";
        public static string User_name = "user_name";
        public static string User_password = "user_password";
        public static string Domain_dll = "domain_dll";
    }
    // 局站的属性名
    public class ZXVNMS_Office
    {
        public static string Map = "Map";
        public static string OfficeID = "OfficeID";
        public static string OfficeName = "OfficeName";
        public static string UpOfficeID = "UpOfficeID";
        public static string city_id = "city_id";
        public static string province_id = "province_id";
    }

    // 平台设备的属性名
    public class ZXVNMS_Platform
    {
        public static string device_id = "device_id";
        public static string device_name = "device_name";
        public static string device_type = "device_type";
        public static string login_state = "login_state";
        public static string office_id = "office_id";
        public static string privilege_flag = "privilege_flag";
        public static string service_addr = "service_addr";
        public static string service_port = "service_port";
        public static string web_page = "web_page";
    }

    // 编码器设备的属性名
    public class ZXVNMS_Encoder
    {
        public static string connect_server_id = "connect_server_id";
        public static string device_id = "device_id";
        public static string device_name = "device_name";
        public static string device_type = "device_type";
        public static string dispatch_server_id = "dispatch_server_id";
        public static string encoder_state = "encoder_state";
        public static string encoder_type = "encoder_type";
        public static string ip_address = "ip_address";
        public static string office_id = "office_id";
        public static string privilege_flag = "privilege_flag";
        public static string web_page = "web_page";
    }

    // 解码码器设备的属性名
    public class ZXVNMS_Decoder
    {
        public static string connect_server_id = "connect_server_id";
        public static string device_id = "device_id";
        public static string device_name = "device_name";
        public static string device_type = "device_type";
        public static string ip_address = "ip_address";
        public static string office_id = "office_id";
        public static string privilege_flag = "privilege_flag";
        public static string web_page = "web_page";
    }

    // 摄像头设备的属性名
    public class ZXVNMS_Camera
    {
        public static string address = "address";
        public static string category_id = "category_id";
        public static string control_port = "control_port";
        public static string device_id = "device_id";
        public static string device_name = "device_name";
        public static string device_type = "device_type";
        public static string inport = "inport";
        public static string is_controlable = "is_controlable";
        public static string office_id = "office_id";
        public static string parent_device_id = "parent_device_id";
        public static string port_param = "port_param";
        public static string pos_control = "pos_control";
        public static string privilege_flag = "privilege_flag";
        public static string ptz_protocol = "ptz_protocol";
    }
    public class ZXVNMS_Camera2
    {
        public string address { get; set; }
        public string category_id { get; set; }
        public string control_port { get; set; }
        public string device_id { get; set; }
        public string device_name { get; set; }
        public string device_type { get; set; }
        public string inport { get; set; }
        public string is_controlable { get; set; }
        public string office_id { get; set; }
        public string parent_device_id { get; set; }
        public string port_param { get; set; }
        public string pos_control { get; set; }
        public string privilege_flag { get; set; }
        public string ptz_protocol { get; set; }
    }
    // DI设备的属性名
    public class ZXVNMS_DI
    {
        public static string alarm_level = "alarm_level";
        public static string alarm_type = "alarm_type";
        public static string category_id = "category_id";
        public static string description = "description";
        public static string device_id = "device_id";
        public static string device_name = "device_name";
        public static string device_type = "device_type";
        public static string di_port = "di_port";
        public static string di_state = "di_state";
        public static string encoder_deviceid = "encoder_deviceid";
        public static string office_id = "office_id";
        public static string privilege_flag = "privilege_flag";
    }

    // DO设备的属性名
    public class ZXVNMS_DO
    {
        public static string category_id = "category_id";
        public static string device_id = "device_id";
        public static string device_name = "device_name";
        public static string device_type = "device_type";
        public static string do_port = "do_port";
        public static string do_state = "do_state";
        public static string encoder_deviceid = "encoder_deviceid";
        public static string office_id = "office_id";
        public static string privilege_flag = "privilege_flag";
    }

    // 预置位的相关属性名
    public class ZXVNMS_Preset
    {
        public static string preset = "preset";
        public static string description = "description";
    }

    // 告警联动的相关属性名
    public class ZXVNMS_AlarmLinkage
    {
        public static string alarm_device_id = "alarm_device_id";
        public static string linkage_device_id = "linkage_device_id";
        public static string alarm_type = "alarm_type";
        public static string linkage_type = "linkage_type";
        public static string trigger_state = "trigger_state";
        public static string step = "step";
        public static string param1 = "param1";
    }

    // 电视墙设备的属性名
    public class ZXVNMS_TVWALL
    {
        public static string device_id = "device_id";
        public static string device_name = "device_name";
        public static string device_type = "device_type";
        public static string office_id = "office_id";
        public static string privilege_flag = "privilege_flag";
    }

    // 录像文件的属性名
    public class ZXVNMS_RecordFile
    {
        public static string encoder_id = "encoder_id";
        public static string service_addr = "service_addr";
        public static string service_port = "service_port";
        public static string outport = "outport";
        public static string starttime = "starttime";
        public static string endtime = "endtime";
        public static string type = "type";
        public static string filename = "filename";
        public static string size = "size";
    }
    public class ZXVNMS_RecordFile2
    {

        public int id { get; set; }
        public string locale { get; set; }


        public string encoder_id { get; set; }
        public string service_addr { get; set; }
        public string service_port { get; set; }
        public string outport { get; set; }
        public string starttime { get; set; }
        public string endtime { get; set; }
        public string type { get; set; }
        public string filename { get; set; }
        public string size { get; set; }
    }

    // 摄像头状态
    public class ZXVNMS_CameraState
    {
        public static int ONLINE = 0;				// 在线
        public static int OFFLINE = 1;				// 断线
    }
    #endregion
}
