﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace BookReading.Properties {
    using System;
    
    
    /// <summary>
    ///   一个强类型的资源类，用于查找本地化的字符串等。
    /// </summary>
    // 此类是由 StronglyTypedResourceBuilder
    // 类通过类似于 ResGen 或 Visual Studio 的工具自动生成的。
    // 若要添加或移除成员，请编辑 .ResX 文件，然后重新运行 ResGen
    // (以 /str 作为命令选项)，或重新生成 VS 项目。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   返回此类使用的缓存的 ResourceManager 实例。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("BookReading.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   重写当前线程的 CurrentUICulture 属性
        ///   重写当前线程的 CurrentUICulture 属性。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   查找类似 var currentpos, timer;
        ///function initialize() {
        ///    timer = setInterval(&quot;scrollwindow()&quot;, 10);
        ///}
        ///function clr() {
        ///    clearInterval(timer);
        ///}
        ///function scrollwindow() {
        ///    currentpos = document.body.scrollTop;
        ///    window.scroll(0, ++currentpos);
        ///    if (currentpos != document.body.scrollTop)
        ///        clr();
        ///}
        ///document.onmousedown = clr
        ///document.ondblclick = initialize  的本地化字符串。
        /// </summary>
        internal static string head {
            get {
                return ResourceManager.GetString("head", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 &lt;html&gt;
        ///
        ///&lt;head&gt;
        ///    &lt;meta http-equiv=&quot;Content-Type&quot; content=&quot;text/html; charset=gb2312&quot;&gt;
        ///    &lt;title&gt;[BookName]&lt;/title&gt;
        ///    &lt;link href=&quot;../style.css&quot; rel=&quot;stylesheet&quot; type=&quot;text/css&quot;&gt;
        ///    &lt;script src=&quot;../head.js&quot;&gt;&lt;/script&gt;
        ///    
        ///    &lt;script language=&quot;javascript&quot;&gt;
        ///        window.onload = function () {
        ///            if (document.getElementsByTagName(&quot;img&quot;)[0].width &gt; 210) {
        ///                document.getElementsByTagName(&quot;img&quot;)[0].width = 210;
        ///            }
        ///        }
        ///    &lt;/script&gt;
        ///&lt;/head&gt;
        ///
        ///&lt;body tex [字符串的其余部分被截断]&quot;; 的本地化字符串。
        /// </summary>
        internal static string List {
            get {
                return ResourceManager.GetString("List", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 body{font-size: 12pt; 
        ///    line-height: 17pt;
        ///}
        ///.body{
        ///    background:#E7F4FE;
        ///}
        ///td{font-FAMILY:宋体;font-size: 9pt; line-height: 160%}
        ///.article_title{font-FAMILY:宋体;font-size: 19pt;  letter-spacing: 2px; line-height: 130%; margin: 6; font-weight: bold;}
        ///.newfont2{font-FAMILY:宋体;font-size: 9pt; line-height: 12pt}
        ///.newfont3{font-FAMILY:宋体;font-size: 9pt; line-height: 12pt}
        ///.thetd{font-FAMILY:宋体;font-size: 12pt; color:#000000;line-height: 160%;font-weight:None;font-style: None}
        ///INPUT
        ///{
        ///    BORDER-B [字符串的其余部分被截断]&quot;; 的本地化字符串。
        /// </summary>
        internal static string style {
            get {
                return ResourceManager.GetString("style", resourceCulture);
            }
        }
    }
}
