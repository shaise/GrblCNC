using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using GrblCNC.Properties;

namespace GrblCNC.Glutils
{
    // A simple class meant to help create shaders.
    public class Shader
    {
        public enum ShadingType
        {
            Flat2D = 0,
            MultiColor2D,
            Textured2D,
            Flat3D,
            FlatNorm3D,
            Textured3D,
            Wire3D,
            Line3D
        }

        public readonly int Handle;
        static Dictionary<ShadingType, Shader> ShaderDict = null;

        private readonly Dictionary<string, int> _uniformLocations;

        public static Shader GetShader(ShadingType stype)
        {
            if (ShaderDict == null)
            {
                ShaderDict = new Dictionary<ShadingType, Shader>();
                ShaderDict[ShadingType.Flat2D] = new Shader(Resources.VertShader2DFlat, Resources.FragShaderFlat);
                ShaderDict[ShadingType.MultiColor2D] = new Shader(Resources.VertShader2DColor, Resources.FragShaderColor);
                ShaderDict[ShadingType.Textured3D] = new Shader(Resources.VertShader3DText, Resources.FragShaderText);
                ShaderDict[ShadingType.Flat3D] = new Shader(Resources.VertShader3DFlat, Resources.FragShaderFlat);
                //ShaderDict[ShadingType.Wire3D] = new Shader(Resources.VertShader3DWire, Resources.FragShaderWire);
                ShaderDict[ShadingType.Wire3D] = new Shader(Resources.VertShader3DWire, Resources.FragShaderWire);
                ShaderDict[ShadingType.FlatNorm3D] = new Shader(Resources.VertShader3DNorm, Resources.FragShaderNorm);
                ShaderDict[ShadingType.Line3D] = new Shader(Resources.VertShader3DLine, Resources.FragShaderLine);
            }
            return ShaderDict[stype];
        }

        public static void SetMatrix4All(string name, Matrix4 data)
        {
            foreach (var pair in ShaderDict)
            {
                Shader s = pair.Value;
                s.SetMatrix4(name, data);
            }
        }

        // This is how you create a simple shader.
        // Shaders are written in GLSL, which is a language very similar to C in its semantics.
        // The GLSL source is compiled *at runtime*, so it can optimize itself for the graphics card it's currently being used on.
        // A commented example of GLSL can be found in shader.vert
        public Shader(string vertSource, string fragSource)
        {
            // There are several different types of shaders, but the only two you need for basic rendering are the vertex and fragment shaders.
            // The vertex shader is responsible for moving around vertices, and uploading that data to the fragment shader.
            //   The vertex shader won't be too important here, but they'll be more important later.
            // The fragment shader is responsible for then converting the vertices to "fragments", which represent all the data OpenGL needs to draw a pixel.
            //   The fragment shader is what we'll be using the most here.

            // Load vertex shader and compile
            // LoadSource is a simple function that just loads all text from the file whose path is given.
            //var shaderSource = LoadSource(vertPath);

            // GL.CreateShader will create an empty shader (obviously). The ShaderType enum denotes which type of shader will be created.
            var vertexShader = CompileShader(vertSource, ShaderType.VertexShader);
            var fragmentShader = CompileShader(fragSource, ShaderType.FragmentShader);

            // These two shaders must then be merged into a shader program, which can then be used by OpenGL.
            // To do this, create a program...
            Handle = GL.CreateProgram();

            // Attach both shaders...
            GL.AttachShader(Handle, vertexShader);
            GL.AttachShader(Handle, fragmentShader);

            // And then link them together.
            LinkProgram(Handle);

            // When the shader program is linked, it no longer needs the individual shaders attacked to it; the compiled code is copied into the shader program.
            // Detach them, and then delete them.
            GL.DetachShader(Handle, vertexShader);
            GL.DetachShader(Handle, fragmentShader);
            GL.DeleteShader(fragmentShader);
            GL.DeleteShader(vertexShader);

            // The shader is now ready to go, but first, we're going to cache all the shader uniform locations.
            // Querying this from the shader is very slow, so we do it once on initialization and reuse those values
            // later.

            // First, we have to get the number of active uniforms in the shader.
            int numberOfUniforms;
            GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out numberOfUniforms);
            //GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms,out int @nn);

 
            // Next, allocate the dictionary to hold the locations.
            _uniformLocations = new Dictionary<string, int>();

            // Loop over all the uniforms,
            for (var i = 0; i < numberOfUniforms; i++)
            {
                // get the name of this uniform,
                int sz;
                ActiveUniformType tph;
                var key = GL.GetActiveUniform(Handle, i, out sz, out tph);

                // get the location,
                var location = GL.GetUniformLocation(Handle, key);

                // and then add it to the dictionary.
                _uniformLocations.Add(key, location);
            }
        }

        private static int CompileShader(string shaderSource, ShaderType stype)
        {
            // GL.CreateShader will create an empty shader (obviously). The ShaderType enum denotes which type of shader will be created.
            var shader = GL.CreateShader(stype);

            // Now, bind the GLSL source code
            GL.ShaderSource(shader, shaderSource);
            
            // Try to compile the shader
            GL.CompileShader(shader);

            // Check for compilation errors
            int code;
            GL.GetShader(shader, ShaderParameter.CompileStatus, out code);
            if (code != (int)All.True)
            {
                // We can use `GL.GetShaderInfoLog(shader)` to get information about the error.
                var infoLog = GL.GetShaderInfoLog(shader);
                throw new Exception(string.Format("Error occurred whilst compiling Shader({0}).\n\n{1}", shader, infoLog));
            }
            
            return shader;
        }

        private static void LinkProgram(int program)
        {
            // We link the program
            GL.LinkProgram(program);

            // Check for linking errors
            int code;
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out code);
            if (code != (int)All.True)
            {
                // We can use `GL.GetProgramInfoLog(program)` to get information about the error.
                string err = GL.GetProgramInfoLog(program);
                throw new Exception(string.Format("Error occurred whilst linking Program({0}): {1}", program, err));
            }
        }

        // A wrapper function that enables the shader program.
        public void Use()
        {
            GL.UseProgram(Handle);
        }

        // The shader sources provided with this project use hardcoded layout(location)-s. If you want to do it dynamically,
        // you can omit the layout(location=X) lines in the vertex shader, and use this in VertexAttribPointer instead of the hardcoded values.
        public int GetAttribLocation(string attribName)
        {
            return GL.GetAttribLocation(Handle, attribName);
        }

        // Just loads the entire file into a string.
        private static string LoadSource(string path)
        {
            using (var sr = new StreamReader(path, Encoding.UTF8))
            {
                return sr.ReadToEnd();
            }
        }


        public bool UniformExists(string name)
        {
            return (_uniformLocations.ContainsKey(name));
        }

        // Uniform setters
        // Uniforms are variables that can be set by user code, instead of reading them from the VBO.
        // You use VBOs for vertex-related data, and uniforms for almost everything else.

        // Setting a uniform is almost always the exact same, so I'll explain it here once, instead of in every method:
        //     1. Bind the program you want to set the uniform on
        //     2. Get a handle to the location of the uniform with GL.GetUniformLocation.
        //     3. Use the appropriate GL.Uniform* function to set the uniform.

        /// <summary>
        /// Set a uniform int on this shader.
        /// </summary>
        /// <param name="name">The name of the uniform</param>
        /// <param name="data">The data to set</param>
        public void SetInt(string name, int data)
        {
            if (!UniformExists(name))
                return;
            GL.UseProgram(Handle);
            GL.Uniform1(_uniformLocations[name], data);
        }

        /// <summary>
        /// Set a uniform uint on this shader.
        /// </summary>
        /// <param name="name">The name of the uniform</param>
        /// <param name="data">The data to set</param>
        public void SetUint(string name, uint data)
        {
            if (!UniformExists(name))
                return;
            GL.UseProgram(Handle);
            GL.Uniform1(_uniformLocations[name], data);
        }

        /// <summary>
        /// Set a uniform float on this shader.
        /// </summary>
        /// <param name="name">The name of the uniform</param>
        /// <param name="data">The data to set</param>
        public void SetFloat(string name, float data)
        {
            if (!UniformExists(name))
                return;
            GL.UseProgram(Handle);
            GL.Uniform1(_uniformLocations[name], data);
        }

        /// <summary>
        /// Set a uniform Matrix4 on this shader
        /// </summary>
        /// <param name="name">The name of the uniform</param>
        /// <param name="data">The data to set</param>
        /// <remarks>
        ///   <para>
        ///   The matrix is transposed before being sent to the shader.
        ///   </para>
        /// </remarks>
        public void SetMatrix4(string name, Matrix4 data)
        {
            if (!UniformExists(name))
                return;
            GL.UseProgram(Handle);
            GL.UniformMatrix4(_uniformLocations[name], true, ref data);
        }

        /// <summary>
        /// Set a uniform Vector3 on this shader.
        /// </summary>
        /// <param name="name">The name of the uniform</param>
        /// <param name="data">The data to set</param>
        public void SetVector3(string name, Vector3 data)
        {
            if (!UniformExists(name))
                return;
            GL.UseProgram(Handle);
            GL.Uniform3(_uniformLocations[name], data);
        }

        /// <summary>
        /// Set a uniform Vector4 on this shader.
        /// </summary>
        /// <param name="name">The name of the uniform</param>
        /// <param name="data">The data to set</param>
        public void SetVector4(string name, Vector4 data)
        {
            if (!UniformExists(name))
                return;
            GL.UseProgram(Handle);
            GL.Uniform4(_uniformLocations[name], data);
        }
    }
}
