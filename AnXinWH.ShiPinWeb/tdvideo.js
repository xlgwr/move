//-------JS for ie5.0，2013-12

//-----语言类型定义
var LANG_CH = 0;
var LANG_EN = 1;
var LANG_CH_TW = 2;
var LANG_CH_KOREA = 3;
var LANG_CH_SPAIN = 4;
var LANG_CH_ITALY = 5;
var LANG_CH_RUSSIAN = 6;
var LANG_CH_TURKISH = 7;
var LANG_CH_THAI = 8;
//-----End
var jsLanguage = LANG_CH;      //0-中文,1-English
var sServerHost;
var sProxyIP;
var WPort;
var npPlugin;

var AXCOLOR = 0;     //界面颜色:0-绿色,1-蓝色；0-TD,1-OEM
var vOCXVersion = "5.0.0.44";

//	JS_cShow5暂时没什么用处
var JS_cShow1 = "网络视频浏览器";
var JS_cShow2 = "预&nbsp;&nbsp;览";
var JS_cShow3 = "回&nbsp;&nbsp;放";
var JS_cShow4 = "配&nbsp;&nbsp;置";
var JS_cShow5 = "中&nbsp;&nbsp;文";
var JS_cShow6 = "日&nbsp;&nbsp;志";

//标题字体类型
var JS_cFont = "宋体,Arial,Helvetica,sans-serif";

var npInitStatus = 0;
var minWidth = 1350;
var minHigth = 550;
var midWidth = 0;	//滚动条居中的宽度
var midHeight = 0;	//滚动条居中的高度

var myWidth = 0; //设置OCX的宽度
var myHeight = 0;//设置OCX的高度
var LogonStaus = 1;//1已登录 0未登录
var loginFlag = false;//判断当前是否已经等了
var IE10 = 10;
var bClickBTNLogon = false;//判断是否点登录按钮,解决ie10和11登录后界面显示问题

//火狐插件事件类型
var NP_EVENTID_OCXINIT = 0;
var NP_EVENTID_LOGON = 1;
var NP_EVENTID_CHANGELANGUAGE = 2;
var NP_EVENTID_CHECKUSER = 3;
var NP_EVENTID_KEYBOARD_MSG = 100;

//键盘消息
var VK_F5 = 0x74;

var Sys = {};
var ua = navigator.userAgent.toLowerCase();
var s;
(s = ua.match(/msie ([\d.]+)/)) ? Sys.ie = s[1] :
(s = ua.match(/firefox\/([\d.]+)/)) ? Sys.firefox = s[1] :
(s = ua.match(/chrome\/([\d.]+)/)) ? Sys.chrome = s[1] :
(s = ua.match(/opera.([\d.]+)/)) ? Sys.opera = s[1] :
(s = ua.match(/version\/([\d.]+).*safari/)) ? Sys.safari = s[1] : 0;

//	兼容IE11的判断
if (document.documentMode && document.documentMode == 11 || document.documentMode == 10) {
    Sys.ie = IE10;
}

function isNeedResize(h, w) {
    if (h > minWidth || w > minHigth) {
        return true;
    }
}

function displayScroll() {
    document.body.focus();
    document.getElementById("NvsVideo").focus();
    document.body.scrollTop = 0;
    //window.scrollTo(midWidth,midHeight);
}

function ChangeTitleLanguage(_iLanguage) {
    switch (_iLanguage) {
        case LANG_EN: JS_cShow1 = "Net Video Browser";
            break;
        case LANG_CH_TW: JS_cShow1 = "網路視頻流覽器";
            break;
        case LANG_CH_KOREA: JS_cShow1 = "네트워크 비디오 브라우저";
            break;
        case LANG_CH_SPAIN: JS_cShow1 = "Red Navegador de Vídeo";
            break;
        case LANG_CH_ITALY: JS_cShow1 = "Navigatore di rete video";
            break;
        case LANG_CH_RUSSIAN: JS_cShow1 = "Сеть браузера Видео";
            break;
        case LANG_CH_TURKISH: JS_cShow1 = "Net Video Browser";//待翻译
            break;
        case LANG_CH_THAI: JS_cShow1 = "Net Video Browser";
            break;
        default: JS_cShow1 = "网络视频浏览器";
    }

    document.title = JS_cShow1;
}

function CheckOCX() {
    var video = document.getElementById("NvsVideo");
    var retval = -1;
    try {
        video.ReSizeWindow(1, 1);
        retval = 0;
    }
    catch (e) {
        document.location = 'index.html';
        retval = -1;
    }
    return retval;
}

function LoadHtml() {
    if (CheckOCX() < 0) {
        return;
    }
    if (Sys.ie) {
        IE_Event_OCXInit(0);

        //	LoadHtml时仅改变title语言即可
        jsLanguage = NvsVideo.Language;
        ChangeTitleLanguage(jsLanguage);
    }
    else {
        npPlugin = document.getElementById("NvsVideo");
        Firefox_Event_OCXInit(npInitStatus);
        Firefox_ResizeWindow(0);//F5刷新时重汇窗口

        //	LoadHtml时仅改变title语言即可
        jsLanguage = npPlugin.GetLanguage();
        ChangeTitleLanguage(jsLanguage);
    }
}

function ResizeScreen(flag) {
    if (Sys.ie) {
        IE_ResizeWindow(flag);
    }
    else {
        Firefox_ResizeWindow(flag);
    }
}

function IE_ResizeWindow(flag) {
    myWidth = document.body.clientWidth;
    myHeight = document.body.clientHeight;
    midWidth = (document.body.scrollWidth - document.body.offsetWidth) / 2;
    midHeight = (document.body.scrollHeight - document.body.offsetHeight) / 2;
    if (loginFlag == true)//登录成功后
    {
        if (flag == LogonStaus) {
            myWidth = myWidth - 10;
            myHeight = myHeight - 110;
            if (myWidth < minWidth) {
                if (Sys.ie == IE10) {
                    document.getElementById("main_title").style.width = "1350px";
                    document.getElementById("Lybiaoti").style.width = "1350px";
                }
                else {
                    document.getElementById("main_title").style.width = "1360px";
                    document.getElementById("Lybiaoti").style.width = "1360px";
                }
            }
            else {
                document.getElementById("main_title").style.width = "100%";
                document.getElementById("Lybiaoti").style.width = "100%";
            }
            if (bClickBTNLogon && (Sys.ie == IE10)) {
                myHeight = myHeight + 17;
                myWidth = myWidth + 25;
                bClickBTNLogon = false;
            } else if (Sys.ie == IE10) {
                myWidth = myWidth + 8;
                document.getElementById("div_obj").style.height = myHeight;
                document.getElementById("div_obj").style.width = myWidth;
            }
        }
    }
    else//解决ie10和11登陆界面有滚动条问题
    {
        if (Sys.ie == IE10) {
            myHeight = myHeight - 4;
            document.getElementById("div_obj").style.height = myHeight;
            document.getElementById("div_obj").style.width = myWidth;
        }
    }
    NvsVideo.ReSizeWindow(myWidth, myHeight);
    //if(!flag && !loginFlag)
    //{
    //	window.scrollTo((document.body.scrollWidth-document.body.offsetWidth)/2,(document.body.scrollHeight-document.body.offsetHeight)/2);
    //}
    if (flag) {
        setTimeout(displayScroll, 100);//解决IE滚动条被OCX挡住的问题
    }
}

function IE_Event_OCXInit(_lInitStatus) {
    sServerHost = GetArgsFromHref(document.location.href, "encoder");
    WPort = "3000";// GetArgsFromHref(document.location.href, "wport");
    suid = "admin";// GetArgsFromHref(document.location.href, "uid");
    spwd = "admin";//GetArgsFromHref(document.location.href, "pwd");

    var webport = "80";
    var href = "http://192.168.1.110/index_ch.html";//document.location.href.split("/");
    var host = "192.168.1.110";//href[2];
    var indexIPV6 = host.indexOf("]");
    var indexWebPort = host.lastIndexOf(":");
    if (indexIPV6 < 0)   //IPV4
    {
        if (indexWebPort < 0) {
            sProxyIP = host;
        }
        else {
            sProxyIP = host.substring(0, indexWebPort);
            webport = host.substring(indexWebPort + 1, host.length);
        }
    }
    else    //IPV6
    {
        if ("undefined" != typeof ipaddr1) {
            sProxyIP = ipaddr1;
        }
        if (indexWebPort > indexIPV6) {
            webport = host.substring(indexWebPort + 1, host.length);
        }
    }

    if (sServerHost == "") {
        sServerHost = sProxyIP;
    }

    if (webport != "") {
        NvsVideo.webport = parseInt(webport);
    }

    //if((sProxyIP!="")&&(sProxyIP!="localhost"))
    //{
    //	NvsVideo.proxyhost=sProxyIP;
    //}
    if (sServerHost != "") {
        NvsVideo.encoder = sServerHost;
    }

    if (NvsVideo !== "defined") {
        NvsVideo.CustStyle = AXCOLOR;
    }

    if (WPort != "") {
        NvsVideo.wport = parseInt(WPort);
    }

    if (suid != "") {
        NvsVideo.suid = suid;
    }

    if (spwd != "") {
        NvsVideo.spwd = spwd;
    }
    //页面上显示当前时间
    //setInterval("ShowTime()",1000);	
}

function IE_Event_Logon(_lLogonStatus, _lLogonID) {
    if (_lLogonStatus == 1) {
        var width = document.getElementById("div_obj").offsetWidth;
        myWidth = document.body.clientWidth - 10; //OCX的宽度
        myHeight = document.body.clientHeight - 110;//OCX的高度
        //显示登录的tilte
        document.getElementById("main_title").style.display = "none";
        document.getElementById("main_title").style.width = width;
        //显示菜单
        document.getElementById("Lybiaoti").style.display = "none";//block
        document.getElementById("Lybiaoti").style.width = width;
        //显示按钮的状态
        CtlButtonclose();
        lyCtl01.style.display = "block";
        //显示时间
        //document.getElementById("lyShowTime").style.display ="block";
        //登录成功之后 显示窗口
        document.getElementById("div_obj").style.padding = "5px 5px 0px 5px;"; //设置OCX显示的样式，登录成功之后让OCX上左右各有5px的高度
        document.getElementById("div_obj_font").style.display = "none"; //设置底部的样式，高度为 10px
        loginFlag = true; //设置当前登录的状态
        bClickBTNLogon = true;
        if (Sys.ie == IE10) {
            if (myWidth < minWidth) {
                document.getElementById("main_title").style.width = "1350px";
                document.getElementById("Lybiaoti").style.width = "1350px";
                document.getElementById("div_obj").style.width = minWidth;
            }
            else {
                document.getElementById("main_title").style.width = "100%";
                document.getElementById("Lybiaoti").style.width = "100%";
                document.getElementById("div_obj").style.width = myWidth;
            }

            if (myHeight < minHigth) {
                document.getElementById("div_obj").style.height = minHigth;
            }
            else {
                document.getElementById("div_obj").style.height = myHeight;
            }

            IE_ResizeWindow(1);
        }
        else {
            if (myWidth < minWidth) {
                document.getElementById("main_title").style.width = "1360px";
                document.getElementById("Lybiaoti").style.width = "1360px";
            }
            else {
                document.getElementById("main_title").style.width = "100%";
                document.getElementById("Lybiaoti").style.width = "100%";
            }
            NvsVideo.ReSizeWindow(myWidth, myHeight);
        }
        //	改变语言
        jsLanguage = NvsVideo.Language;
        ChangeLanguage();
        ChangeFont();

        setTimeout(displayScroll, 100);//解决IE滚动条被OCX挡住的问题
        NvsVideo.CheckOCXVersion(vOCXVersion);
        //imglogID.src="v-013-2.gif";
        if (AXCOLOR == 0) {
            //tdlog1.style.backgroundColor ="#029900";
            //lyMain.style.backgroundColor ="#029900";
        } else {
            //tdlog1.style.backgroundColor ="#007D99";
            //lyMain.style.backgroundColor ="#007D99";
        }
        //tdlog2.backgroundColor ="#029900";
    }
}

function Event_ChangeLanguage(_lLanguage) {
    jsLanguage = _lLanguage;
    //	语言改变事件只能在登陆界面触发，仅改变title语言即可
    ChangeTitleLanguage(jsLanguage);
}

function Event_CheckUser(_lParam) {
    ShowConfigWindow();
}

function Firefox_ResizeWindow(flag) {
    myWidth = document.body.clientWidth;
    myHeight = document.body.clientHeight;
    if (loginFlag == false) {
        if (myWidth < minWidth) {
            document.getElementById("div_obj").style.width = minWidth;
        }
        else {
            document.getElementById("div_obj").style.width = myWidth - 2;
        }
        if (myHeight < minHigth) {
            document.getElementById("div_obj").style.height = minHigth;
        }
        else {
            document.getElementById("div_obj").style.height = myHeight - 2;
        }
    }
    else if (loginFlag == true) {
        if (flag == 1) {
            myWidth = myWidth - 2;
            myHeight = myHeight - 110;
            if (myWidth < minWidth) {
                document.getElementById("main_title").style.width = "1360px";
                document.getElementById("Lybiaoti").style.width = "1360px";
                document.getElementById("div_obj").style.width = minWidth;
            }
            else {
                document.getElementById("main_title").style.width = "100%";
                document.getElementById("Lybiaoti").style.width = "100%";
                document.getElementById("div_obj").style.width = myWidth;
            }

            if (myHeight < minHigth) {
                document.getElementById("div_obj").style.height = minHigth;
            }
            else {
                document.getElementById("div_obj").style.height = myHeight;
            }
        }
    }
    npPlugin.ReSizeWindow(myWidth, myHeight);
}

function npEvent(type, param1, param2) {
    if (NP_EVENTID_OCXINIT == type) {
        npInitStatus = param1;
    }
    else if (NP_EVENTID_LOGON == type) {
        var npLogonStatus = param1;
        var npLogonID = param2;
        if (1 == npLogonStatus) {
            Firefox_Event_Logon();
        }
    }
    else if (NP_EVENTID_CHANGELANGUAGE == type) {
        Event_ChangeLanguage(param1);
    }
    else if (NP_EVENTID_KEYBOARD_MSG == type) {
        if (param1 == VK_F5) {
            location.reload();//解决火狐浏览器不能使用F5刷新的问题
        }
    }
    else if (NP_EVENTID_CHECKUSER == type) {
        Event_CheckUser(param1);
    }
}

function Firefox_Event_OCXInit(_iInitStatus) {
    sServerHost = GetArgsFromHref(document.location.href, "encoder");
    WPort = "3000";// GetArgsFromHref(document.location.href, "wport");
    suid = "admin";// GetArgsFromHref(document.location.href, "uid");
    spwd = "admin";//GetArgsFromHref(document.location.href, "pwd");

    var webport = "";
    var href = document.location.href.split("/");
    var host = href[2];
    var indexIPV6 = host.indexOf("]");
    var indexWebPort = host.lastIndexOf(":");
    if (indexIPV6 < 0)   //IPV4
    {
        if (indexWebPort < 0) {
            sProxyIP = host;
        }
        else {
            sProxyIP = host.substring(0, indexWebPort);
            webport = host.substring(indexWebPort + 1, host.length);
        }
    }
    else    //IPV6
    {
        if ("undefined" != typeof ipaddr1) {
            sProxyIP = ipaddr1;
        }
        if (indexWebPort > indexIPV6) {
            webport = host.substring(indexWebPort + 1, host.length);
        }
    }

    if (sServerHost == "") {
        sServerHost = sProxyIP;
    }

    if (webport != "") {
        npPlugin.SetWebPort(parseInt(webport));
    }

    //if((sProxyIP!="")&&(sProxyIP!="localhost"))
    //{
    //	npPlugin.SetProxyHost(sProxyIP);
    //}
    if (sServerHost != "") {
        npPlugin.SetHostIp(sServerHost);
    }

    npPlugin.SetCustStyle(AXCOLOR);

    if (WPort != "") {
        npPlugin.SetHostPort(WPort);
    }

    if (suid != "") {
        npPlugin.SetUserName(suid);
    }

    if (spwd != "") {
        npPlugin.SetUserPsw(spwd);
    }
    //setInterval("ShowTime()",1000);
}

function Firefox_Event_Logon() {
    //显示登录的tilte
    document.getElementById("main_title").style.display = "block";
    //显示菜单
    document.getElementById("Lybiaoti").style.display = "block";
    //显示按钮的状态
    CtlButtonclose();
    lyCtl01.style.display = "block";
    //显示时间
    //document.getElementById("lyShowTime").style.display ="block";
    //登陆后显示窗口
    document.getElementById("div_obj").style.padding = "5px 5px 0px 5px;"; //设置OCX显示的样式，登录成功之后让OCX上左右各有5px的高度
    document.getElementById("div_obj_font").style.display = "block"; //设置底部的样式，高度为 10px
    myWidth = document.documentElement.clientWidth;
    myHeight = document.documentElement.clientHeight;
    loginFlag = true; //设置当前登录的状态
    myWidth = myWidth - 2;
    myHeight = myHeight - 110;
    if (myWidth < minWidth) {
        document.getElementById("main_title").style.width = "1350px";
        document.getElementById("Lybiaoti").style.width = "1350px";
        document.getElementById("div_obj").style.width = minWidth;
    }
    else {
        document.getElementById("main_title").style.width = "100%";
        document.getElementById("Lybiaoti").style.width = "100%";
        document.getElementById("div_obj").style.width = myWidth;
    }

    if (myHeight < minHigth) {
        document.getElementById("div_obj").style.height = minHigth;
    }
    else {
        document.getElementById("div_obj").style.height = myHeight;
    }
    //登录成功之后				
    jsLanguage = npPlugin.GetLanguage();
    ChangeLanguage();
    ChangeFont();

    setTimeout(function () { Firefox_ResizeWindow(1); }, 100);
    npPlugin.CheckVersion(vOCXVersion);
}

function UnLoadHtml() {
}

function OutPrint(vText) {
    document.write(vText);
}
function CtlButtonclose() {
    lyCtl01.style.display = "none";
    lyCtl02.style.display = "none";
    lyCtl03.style.display = "none";
    lyCtl04.style.display = "none";
}
function ShowPreviewWindow() {
    var iResult = -1;
    if (Sys.ie) {
        iResult = NvsVideo.ShowPreviewWindow();
    }
    else {
        iResult = npPlugin.ShowPreviewWindow();
    }
    if (iResult == 0) {
        CtlButtonclose();
        lyCtl01.style.display = "block";
    }
}
function ShowPlaybackWindow() {
    var iResult = -1;
    if (Sys.ie) {
        iResult = NvsVideo.ShowPlaybackWindow();
    }
    else {
        iResult = npPlugin.ShowPlaybackWindow();
    }
    if (iResult == 0) {
        CtlButtonclose();
        lyCtl02.style.display = "block";
    }
}
function ShowConfigWindow() {
    var iResult = -1;
    if (Sys.ie) {
        iResult = NvsVideo.ShowConfigWindow();
    }
    else {
        iResult = npPlugin.ShowConfigWindow();
    }
    if (iResult == 0) {
        CtlButtonclose();
        lyCtl03.style.display = "block";
    }
}
function ShowLogWindow() {
    var iResult = -1;
    if (Sys.ie) {
        iResult = NvsVideo.ShowLogWindow();
    }
    else {
        iResult = npPlugin.ShowLogWindow();
    }
    if (iResult == 0) {
        CtlButtonclose();
        lyCtl04.style.display = "block";
    }
}
function ChangeFont() {
    switch (jsLanguage) {
        case LANG_CH:
        case LANG_EN:
        case LANG_CH_TW:
        case LANG_CH_KOREA:
        case LANG_CH_SPAIN:
        case LANG_CH_ITALY:
        case LANG_CH_TURKISH:
        case LANG_CH_THAI:
            {
                JS_cFont = "宋体,Arial,Helvetica,sans-serif";
            }
            break;
        case LANG_CH_RUSSIAN:
            {
                JS_cFont = "Arial,Helvetica,sans-serif";
            }
            break;
        default:
            {
                JS_cFont = "Arial,Helvetica,sans-serif";
            }
    }

    //document.getElementById("lyVideo").style.fontFamily = JS_cFont;
    //document.getElementById("lyPlayback").style.fontFamily = JS_cFont;
    //document.getElementById("lyConfig").style.fontFamily = JS_cFont;
    //document.getElementById("lyLog").style.fontFamily = JS_cFont;

}
function ChangeLanguage() {
    switch (jsLanguage) {
        case LANG_EN:
            {
                JS_cShow1 = "Net Video Browser";
                JS_cShow2 = "Live View";
                JS_cShow3 = "Playback";
                JS_cShow4 = "Configuration";
                JS_cShow5 = "Chinese";
                JS_cShow6 = "Log";
            }
            break;
        case LANG_CH_TW:
            {
                JS_cShow1 = "網路視頻流覽器";
                JS_cShow2 = "預&nbsp;&nbsp;覽";
                JS_cShow3 = "重&nbsp;&nbsp;播";
                JS_cShow4 = "配&nbsp;&nbsp;置";
                JS_cShow5 = "Chinese";
                JS_cShow6 = "日&nbsp;&nbsp;誌";
            }
            break;
        case LANG_CH_KOREA:
            {
                JS_cShow1 = "네트워크 비디오 브라우저";
                JS_cShow2 = "미리보기";
                JS_cShow3 = "재생";
                JS_cShow4 = "구성";
                JS_cShow5 = "Chinese";
                JS_cShow6 = "일지";
            }
            break;
        case LANG_CH_SPAIN:
            {
                JS_cShow1 = "Red Navegador de Vídeo";
                JS_cShow2 = "Avance";
                JS_cShow3 = "Reproducción";
                JS_cShow4 = "Configuración";
                JS_cShow5 = "Chinese";
                JS_cShow6 = "Revista";
            }
            break;
        case LANG_CH_ITALY:
            {
                JS_cShow1 = "Navigatore di rete video";
                JS_cShow2 = "Anteprima";
                JS_cShow3 = "Riproduzione";
                JS_cShow4 = "Configurazione";
                JS_cShow5 = "Chinese";
                JS_cShow6 = "Giornale";
            }
            break;
        case LANG_CH_RUSSIAN:
            {
                JS_cShow1 = "Сеть браузера Видео";
                JS_cShow2 = "Просмотр";
                JS_cShow3 = "воспроизведение";
                JS_cShow4 = "конфигурация";
                JS_cShow5 = "китайский";
                JS_cShow6 = "журнал";
            }
            break;
        case LANG_CH_TURKISH:
            {
                JS_cShow1 = "Net Video Browser";
                JS_cShow2 = "Canlı İzle";
                JS_cShow3 = "Kayıt İzle";
                JS_cShow4 = "Ayarlar";
                JS_cShow5 = "Chinese";
                JS_cShow6 = "Log";
            }
            break;
        case LANG_CH_THAI:
            {
                JS_cShow1 = "Net Video Browser";
                JS_cShow2 = "แสดงภาพสด";
                JS_cShow3 = "เล่นย้อนหลัง";
                JS_cShow4 = "ตั้งค่า";
                JS_cShow5 = "Chinese";
                JS_cShow6 = "ข้อมูลการใช้งาน";
            }
            break;
        default:
            {
                JS_cShow1 = "网络视频浏览器";
                var JS_cShow2 = "预&nbsp;&nbsp;览";
                var JS_cShow3 = "回&nbsp;&nbsp;放";
                var JS_cShow4 = "配&nbsp;&nbsp;置";
                var JS_cShow5 = "中&nbsp;&nbsp;文";
                var JS_cShow6 = "日&nbsp;&nbsp;志";
            }
    }

    document.getElementById("lyVideo").innerHTML = JS_cShow2;
    //document.getElementById("lyPlayback").innerHTML = JS_cShow3;
    //document.getElementById("lyConfig").innerHTML = JS_cShow4;
    //document.getElementById("lyLog").innerHTML = JS_cShow6;
    document.title = JS_cShow1;
}
function SelLanguage() {
    document.location = 'index_ch.html';
}
function ShowTime() {
    //date 
    var now = new Date();
    var yA = now.getFullYear();
    var mA = now.getMonth() + 1;
    var dA = now.getDate();
    //time 
    var hh = now.getHours();
    var mm = now.getMinutes();
    var ss = now.getTime() % 60000;
    ss = (ss - (ss % 1000)) / 1000;
    var clock = '   ' + yA + '-' + mA + '-' + dA + '    ' + hh + ':';
    if (mm < 10) clock += '0';
    clock += mm + ':';
    if (ss < 10) clock += '0';
    clock += ss;
    //lyShowTime.innerHTML = clock; 
}

function GetArgsFromHref(sHref, sArgName) {
    var args = sHref.split("?");
    var retval = "";

    //null param
    if (args[0] == sHref) {
        return retval;
    }
    var str = args[1];
    args = str.split("&");
    for (var i = 0; i < args.length; i++) {
        str = args[i];
        var arg = str.split("=");
        if (arg.length <= 1) continue;
        if (arg[0] == sArgName) retval = arg[1];
    }
    return retval;
}
/*
function MM_reloadPage(init) {  //reloads the window if Nav4 resized
  if (init==true) 
  	with (navigator) {
		if ((appName=="Netscape")&&(parseInt(appVersion)==4)) {
    		document.MM_pgW=innerWidth; 
			document.MM_pgH=innerHeight; 
			onresize=MM_reloadPage; 
		}
	}
  else if (innerWidth!=document.MM_pgW || innerHeight!=document.MM_pgH) 
  	location.reload();
}
MM_reloadPage(true);

function MM_preloadImages() { //v3.0
	var d=document; 
	if(d.images){ 
		if(!d.MM_p) 
			d.MM_p=new Array();
    		var i,j=d.MM_p.length,a=MM_preloadImages.arguments; 
			for(i=0; i<a.length; i++)
    			if (a[i].indexOf("#")!=0){ 
					d.MM_p[j]=new Image; 
					d.MM_p[j++].src=a[i];
				}
			}
}
*/

function MM_swapImgRestore() { //v3.0
    var i, x, a = document.MM_sr;
    for (i = 0; a && i < a.length && (x = a[i]) && x.oSrc; i++)
        x.src = x.oSrc;
}

function MM_findObj(n, d) { //v4.01
    var p, i, x;
    if (!d)
        d = document;
    if ((p = n.indexOf("?")) > 0 && parent.frames.length) {
        d = parent.frames[n.substring(p + 1)].document;
        n = n.substring(0, p);
    }
    if (!(x = d[n]) && d.all)
        x = d.all[n];
    for (i = 0; !x && i < d.forms.length; i++)
        x = d.forms[i][n];
    for (i = 0; !x && d.layers && i < d.layers.length; i++)
        x = MM_findObj(n, d.layers[i].document);
    if (!x && d.getElementById)
        x = d.getElementById(n);
    return x;
}

function MM_swapImage() { //v3.0
    var i, j = 0, x, a = MM_swapImage.arguments;
    document.MM_sr = new Array;
    for (i = 0; i < (a.length - 2) ; i += 3)
        if ((x = MM_findObj(a[i])) != null) {
            document.MM_sr[j++] = x;
            if (!x.oSrc) x.oSrc = x.src;
            x.src = a[i + 2];
        }
}
