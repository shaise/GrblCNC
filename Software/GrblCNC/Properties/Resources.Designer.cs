﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GrblCNC.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("GrblCNC.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
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
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap aboutButt {
            get {
                object obj = ResourceManager.GetObject("aboutButt", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap AboutPicSmall {
            get {
                object obj = ResourceManager.GetObject("AboutPicSmall", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap AlarmErrorIcon {
            get {
                object obj = ResourceManager.GetObject("AlarmErrorIcon", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap AlarmIcon {
            get {
                object obj = ResourceManager.GetObject("AlarmIcon", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap checkedButt {
            get {
                object obj = ResourceManager.GetObject("checkedButt", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap ConfGrblButt {
            get {
                object obj = ResourceManager.GetObject("ConfGrblButt", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap ConfSenderButt {
            get {
                object obj = ResourceManager.GetObject("ConfSenderButt", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap ErrorIcon {
            get {
                object obj = ResourceManager.GetObject("ErrorIcon", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap estop {
            get {
                object obj = ResourceManager.GetObject("estop", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #version 330 core
        ///out vec4 FragColor;  
        ///in vec3 vertColor;
        ///  
        ///void main()
        ///{
        ///    FragColor = vec4(vertColor, 1.0);
        ///}
        ///.
        /// </summary>
        internal static string FragShaderColor {
            get {
                return ResourceManager.GetString("FragShaderColor", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #version 330 core
        ///
        ///out vec4 FragColor;  
        ///uniform vec4 flatColor; // we set this variable in the OpenGL code.
        ///
        ///void main()
        ///{
        ///    FragColor = flatColor;
        ///} 
        ///  
        ///.
        /// </summary>
        internal static string FragShaderFlat {
            get {
                return ResourceManager.GetString("FragShaderFlat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #version 330 core
        ///out vec4 FragColor;  
        ///flat in float vColor; 
        ///  
        ///void main()
        ///{
        ///    uint wc = uint(vColor + 0.1);
        ///    float b = 0.0039215 * float(wc &amp; 0xFFu);
        ///    float g = 0.0039215 * float((wc &gt;&gt; 8) &amp; 0xFFu);
        ///    float r = 0.0039215 * float((wc &gt;&gt; 16) &amp; 0xFFu);
        ///    float a = 1.0;
        ///	FragColor = vec4(r, g, b, a);
        ///}
        ///.
        /// </summary>
        internal static string FragShaderLine {
            get {
                return ResourceManager.GetString("FragShaderLine", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to // Normals Color shader 
        ///#version 330
        ///
        ///out vec4 FragColor;
        ///
        ///in vec3 Normal;
        ///in vec3 FragPos;
        ///
        ///uniform vec3 lightPos;
        ///uniform vec3 lightColor;
        ///uniform vec3 objectColor;
        ///uniform vec3 ambient;
        ///
        ///void main()
        ///{
        ///	vec3 norm = normalize(Normal);
        ///	vec3 lightDir = normalize(lightPos - FragPos);  
        ///	float diff = max(dot(norm, lightDir), 0.0);
        ///	vec3 diffuse = diff * lightColor;
        ///	vec3 result = (ambient + diffuse) * objectColor;
        ///	FragColor = vec4(result, 1.0);
        ///}
        ///.
        /// </summary>
        internal static string FragShaderNorm {
            get {
                return ResourceManager.GetString("FragShaderNorm", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to // Texture Color shader 
        ///#version 330
        ///#extension GL_ARB_texture_query_lod : enable
        ///
        ///out vec4 outputColor;
        ///in vec2 texCoord;
        ///uniform sampler2D texture0;
        ///uniform vec4 tintColor;
        ///
        ///void main()
        ///{
        ///    //float mipmapLevel = textureQueryLod(texture0, texCoord).x;
        ///#ifdef GL_ARB_texture_query_lod
        ///    float mipmapLevel = textureQueryLOD(texture0, texCoord).x; // NOTE CAPITALIZATION
        ///    outputColor = tintColor * textureLod(texture0, texCoord, mipmapLevel * 0.7);
        ///#else
        ///    outputColor = tintColor * texture(texture0, tex [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string FragShaderText {
            get {
                return ResourceManager.GetString("FragShaderText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #version 330 core
        ///out vec4 FragColor;  
        ///flat in vec2 texCoord; // using the texture coord to pass color info
        ///
        ///uniform float changeIndex; // before this index add transparency
        ///uniform float hltIndex;    // highligt this segment
        ///  
        ///void main()
        ///{
        ///    uint wc = uint(texCoord[1] + 0.1);
        ///    float b = 0.0039215 * float(wc &amp; 0xFFu);
        ///    float g = 0.0039215 * float((wc &gt;&gt; 8) &amp; 0xFFu);
        ///    float r = 0.0039215 * float((wc &gt;&gt; 16) &amp; 0xFFu);
        ///    float a = 1.0;
        ///	if (hltIndex == texCoord[0]) // &amp;&amp; hltIndex &lt;= (texCoord[0] [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string FragShaderWire {
            get {
                return ResourceManager.GetString("FragShaderWire", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap GotoIcon {
            get {
                object obj = ResourceManager.GetObject("GotoIcon", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 1:Hard limit triggered. Machine position is likely lost due to sudden and immediate halt. Re-homing is highly recommended.
        ///2:G-code motion target exceeds machine travel. Machine position safely retained. Alarm may be unlocked.
        ///3:Reset while in motion. Grbl cannot guarantee position. Lost steps are likely. Re-homing is highly recommended.
        ///4:Probe fail. The probe is not in the expected initial state before starting probe cycle, where G38.2 and G38.3 is not triggered and G38.4 and G38.5 is triggered.
        ///5:Pro [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string GrblAlarmCodes {
            get {
                return ResourceManager.GetString("GrblAlarmCodes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 1:G-code words consist of a letter and a value. Letter was not found.
        ///2:Numeric value format is not valid or missing an expected value.
        ///3:Grbl &apos;$&apos; system command was not recognized or supported.
        ///4:Negative value received for an expected positive value.
        ///5:Homing cycle is not enabled via settings.
        ///6:Minimum step pulse time must be greater than 3usec
        ///7:EEPROM read failed. Reset and restored to default values.
        ///8:Grbl &apos;$&apos; command cannot be used unless Grbl is IDLE. Ensures smooth operation during a job.
        /// [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string GrblErrorCodes {
            get {
                return ResourceManager.GetString("GrblErrorCodes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap grblIcon {
            get {
                object obj = ResourceManager.GetObject("grblIcon", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to %
        ///(GRBLLOGO)
        ///(T2  D=1.588 CR=0. - ZMIN=-0.2 - FLAT END MILL)
        ///N10 G90 G94 G17 G91.1
        ///N15 G21
        ///N20 G53 G0 Z15.
        ///(2D CONTOUR1)
        ///N25 T2 M6
        ///N30 S10000 M3
        ///N35 G54
        ///N40 M8
        ///N45 G0 X45.207 Y14.875
        ///N50 G43 Z15. H2
        ///N55 G0 Z5.
        ///N60 G1 Z1. F150.
        ///N65 Z-0.2
        ///N70 X45.102 Y15.313 F300.
        ///N75 X45.005 Y15.882
        ///N80 X44.946 Y16.462
        ///N85 X44.931 Y16.756
        ///N90 X44.926 Y17.053
        ///N95 Y19.344
        ///N100 X44.931 Y19.641
        ///N105 X44.946 Y19.935
        ///N110 X45.005 Y20.515
        ///N115 X45.102 Y21.084
        ///N120 X45.235 Y21.639
        ///N125 X45.404 Y22.18
        ///N130 [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string GrblLogo_ngc {
            get {
                return ResourceManager.GetString("GrblLogo_ngc", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to # Code | Description | Units | Type | Group | Extra
        ///0|Step pulse time|us|Float|Hardware Timings
        ///1|Step idle delay|ms|Int|Hardware Timings
        ///2|Step pulse invert|mask|Mask|Polarity/Direction inverts|X,Y,Z,A,B,C
        ///3|Step direction invert|mask|Mask|Polarity/Direction inverts|X,Y,Z,A,B,C
        ///4|Invert step enable pin|bool|Bool|Polarity/Direction inverts
        ///5|Invert limit pins|mask|Mask|Polarity/Direction inverts|X,Y,Z,A,B,C
        ///6|Invert probe pin|bool|Bool|Polarity/Direction inverts
        ///10|Status report options|mask|Mask|Ge [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string GrblParamDescription {
            get {
                return ResourceManager.GetString("GrblParamDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap GridCenter {
            get {
                object obj = ResourceManager.GetObject("GridCenter", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap GridEmpty {
            get {
                object obj = ResourceManager.GetObject("GridEmpty", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap GridRuler {
            get {
                object obj = ResourceManager.GetObject("GridRuler", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap HomedIcon {
            get {
                object obj = ResourceManager.GetObject("HomedIcon", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap HomeIcon {
            get {
                object obj = ResourceManager.GetObject("HomeIcon", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap HomeIconFull {
            get {
                object obj = ResourceManager.GetObject("HomeIconFull", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Byte[].
        /// </summary>
        internal static byte[] JetBrainsMono_Medium {
            get {
                object obj = ResourceManager.GetObject("JetBrainsMono_Medium", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap JogButtHover {
            get {
                object obj = ResourceManager.GetObject("JogButtHover", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap JogButtNorm {
            get {
                object obj = ResourceManager.GetObject("JogButtNorm", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap JogButtPress {
            get {
                object obj = ResourceManager.GetObject("JogButtPress", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap MacroAddButt {
            get {
                object obj = ResourceManager.GetObject("MacroAddButt", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap MacroF1Buttt {
            get {
                object obj = ResourceManager.GetObject("MacroF1Buttt", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap MacroF2Buttt {
            get {
                object obj = ResourceManager.GetObject("MacroF2Buttt", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap MacroF3Buttt {
            get {
                object obj = ResourceManager.GetObject("MacroF3Buttt", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap MacroF4Buttt {
            get {
                object obj = ResourceManager.GetObject("MacroF4Buttt", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap MacroF5Buttt {
            get {
                object obj = ResourceManager.GetObject("MacroF5Buttt", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap MacroF6Buttt {
            get {
                object obj = ResourceManager.GetObject("MacroF6Buttt", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap MacroF7Buttt {
            get {
                object obj = ResourceManager.GetObject("MacroF7Buttt", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap MacroF8Buttt {
            get {
                object obj = ResourceManager.GetObject("MacroF8Buttt", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap MacroF9Buttt {
            get {
                object obj = ResourceManager.GetObject("MacroF9Buttt", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap OpenButt {
            get {
                object obj = ResourceManager.GetObject("OpenButt", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap pauseButt {
            get {
                object obj = ResourceManager.GetObject("pauseButt", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap playButt {
            get {
                object obj = ResourceManager.GetObject("playButt", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap powerButt {
            get {
                object obj = ResourceManager.GetObject("powerButt", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap ProbeHoleIcon {
            get {
                object obj = ResourceManager.GetObject("ProbeHoleIcon", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap ProbeIcon {
            get {
                object obj = ResourceManager.GetObject("ProbeIcon", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap ReloaButt {
            get {
                object obj = ResourceManager.GetObject("ReloaButt", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap SliderThumb {
            get {
                object obj = ResourceManager.GetObject("SliderThumb", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap SpinLeftIcon {
            get {
                object obj = ResourceManager.GetObject("SpinLeftIcon", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap SpinRightIcon {
            get {
                object obj = ResourceManager.GetObject("SpinRightIcon", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap StepButt {
            get {
                object obj = ResourceManager.GetObject("StepButt", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap stopButt {
            get {
                object obj = ResourceManager.GetObject("stopButt", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap StopIcon {
            get {
                object obj = ResourceManager.GetObject("StopIcon", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap ToolTableButt {
            get {
                object obj = ResourceManager.GetObject("ToolTableButt", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap TouchGIcon {
            get {
                object obj = ResourceManager.GetObject("TouchGIcon", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap TouchTIcon {
            get {
                object obj = ResourceManager.GetObject("TouchTIcon", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #version 330 core
        ///layout (location = 0) in vec3 aPos;   // the position variable has attribute position 0
        ///layout (location = 1) in vec3 aColor; // the color variable has attribute position 1
        ///  
        ///out vec3 vertColor; // output a color to the fragment shader
        ///
        ///void main()
        ///{
        ///    gl_Position = vec4(aPos, 1.0);
        ///    vertColor = aColor; // set textColor to the input color we got from the vertex data
        ///}
        ///.
        /// </summary>
        internal static string VertShader2DColor {
            get {
                return ResourceManager.GetString("VertShader2DColor", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #version 330 core
        ///layout(location = 0) in vec3 aPosition;
        ///void main(void)
        ///{
        ///    gl_Position = vec4(aPosition, 1.0);
        ///}.
        /// </summary>
        internal static string VertShader2DFlat {
            get {
                return ResourceManager.GetString("VertShader2DFlat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to // this is a 2D textured shader
        ///#version 330 core
        ///layout(location = 0) in vec3 aPosition;
        ///layout(location = 1) in vec2 aTexCoord;
        ///
        ///out vec2 texCoord;
        ///
        ///void main(void)
        ///{
        ///    texCoord = aTexCoord;
        ///    gl_Position = vec4(aPosition, 1.0);
        ///}
        ///.
        /// </summary>
        internal static string VertShader2DText {
            get {
                return ResourceManager.GetString("VertShader2DText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to // this is a 3D textured shader
        ///#version 330 core
        ///
        ///layout(location = 0) in vec3 aPosition;
        ///
        ///uniform mat4 model;
        ///uniform mat4 view;
        ///uniform mat4 projection;
        ///
        ///void main(void)
        ///{
        ///    gl_Position = vec4(aPosition, 1.0) * model * view * projection;
        ///}
        ///.
        /// </summary>
        internal static string VertShader3DFlat {
            get {
                return ResourceManager.GetString("VertShader3DFlat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to // this is a 3D line shader: 3 float vert, 1 float color
        ///#version 330 core
        ///
        ///layout(location = 0) in vec3 aPosition;
        ///layout(location = 1) in float aVertColor;
        ///
        ///flat out float vColor;
        ///
        ///uniform mat4 model;
        ///uniform mat4 view;
        ///uniform mat4 projection;
        ///
        ///void main(void)
        ///{
        ///    vColor = aVertColor;
        ///    gl_Position = vec4(aPosition, 1.0) * model * view * projection;
        ///}
        ///.
        /// </summary>
        internal static string VertShader3DLine {
            get {
                return ResourceManager.GetString("VertShader3DLine", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to // this is a 3D textured shader
        ///#version 330 core
        ///
        ///layout(location = 0) in vec3 aPosition;
        ///layout(location = 1) in vec3 aNormal;
        ///
        ///out vec3 Normal;
        ///out vec3 FragPos;
        ///
        ///uniform mat4 model;
        ///uniform mat4 view;
        ///uniform mat4 projection;
        ///
        ///void main(void)
        ///{
        ///    gl_Position = vec4(aPosition, 1.0) * model * view * projection;
        ///	FragPos = vec3(model * vec4(aPosition, 1.0));
        ///	Normal = aNormal;
        ///}.
        /// </summary>
        internal static string VertShader3DNorm {
            get {
                return ResourceManager.GetString("VertShader3DNorm", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to // this is a 3D textured shader
        ///#version 330 core
        ///
        ///layout(location = 0) in vec3 aPosition;
        ///
        ///layout(location = 1) in vec2 aTexCoord;
        ///
        ///out vec2 texCoord;
        ///
        ///uniform mat4 model;
        ///uniform mat4 view;
        ///uniform mat4 projection;
        ///
        ///void main(void)
        ///{
        ///    texCoord = aTexCoord;
        ///
        ///    gl_Position = vec4(aPosition, 1.0) * model * view * projection;
        ///}
        ///.
        /// </summary>
        internal static string VertShader3DText {
            get {
                return ResourceManager.GetString("VertShader3DText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to // this is a 3D textured shader
        ///#version 330 core
        ///
        ///layout(location = 0) in vec3 aPosition;
        ///
        ///layout(location = 1) in vec2 aTexCoord;
        ///
        ///flat out vec2 texCoord;
        ///
        ///uniform mat4 model;
        ///uniform mat4 view;
        ///uniform mat4 projection;
        ///
        ///void main(void)
        ///{
        ///    texCoord = aTexCoord;
        ///
        ///    gl_Position = vec4(aPosition, 1.0) * model * view * projection;
        ///}
        ///.
        /// </summary>
        internal static string VertShader3DWire {
            get {
                return ResourceManager.GetString("VertShader3DWire", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap WifiButt {
            get {
                object obj = ResourceManager.GetObject("WifiButt", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
    }
}
