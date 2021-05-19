using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Numerics;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;
using System.Windows.Forms;
using System.Threading.Tasks;
enum SStatus
{
    NotInit = 0,
    OK = 1,
    NotFound = -1,
    Unknown = -2,
}
class ShaderView
{
    private SStatus Status = SStatus.NotInit;
    private int Width;
    private int Height;
    private int WWidth;
    private int HHeight;
    private int BasicProgramID;
    public const int CUBE_MCOUNT = 10;
    public const int CUBE_MSIDES = 6;
    public const int RAYTRACING_MDEPTH = 10;
    public const int CUBE_TRIANGLES_COUNT = 12;
    public const float TOTAL_VIEW_WIDTH = 9.99f;
    private int BasicVertexShader;
    private int BasicFragmentShader;
    private int CUBE_COUNT = 0;
    public int[] CubeMaterials = new int[CUBE_MCOUNT];
    public float[] CubeSizes = new float[CUBE_MCOUNT];
    public int RayTracingDepth = RAYTRACING_MDEPTH;
    public OpenTK.Vector3 CameraPosition = new OpenTK.Vector3(0.0F, 0.0F, -9.9f);
    public OpenTK.Vector3 LightSourcePosition = new OpenTK.Vector3(4.0f, 4.0f, 0.0f);
    public OpenTK.Vector3[] CubePositions = new OpenTK.Vector3[CUBE_MCOUNT];
    public ShaderView(int width, int height, int wwidth, int hheight)
    {
        Width = Math.Abs(width);
        Height = Math.Abs(height);
        WWidth = Math.Abs(wwidth);
        HHeight = Math.Abs(hheight);
        InitShader();
        InitView(wwidth, hheight);
    }
    public void LoadShader(string filename, ShaderType type, int program, out int address)
    {
        address = GL.CreateShader(type);
        if (File.Exists(filename))
        {
            StreamReader Reader = new StreamReader(filename);
            GL.ShaderSource(address, Reader.ReadToEnd());
            GL.CompileShader(address);
            GL.AttachShader(program, address);
            Console.WriteLine(GL.GetShaderInfoLog(address));
            Reader.Close();
            Status = SStatus.OK;
        }
        else
        {
            Status = SStatus.NotFound;
        }
    }
    private void InitShader()
    {
        BasicProgramID = GL.CreateProgram();
        string repositoryPath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"..\..\"));
        LoadShader(repositoryPath + "Shaders\\raytracing.vert", BasicProgramID, out BasicVertexShader);
        LoadShader(repositoryPath + "Shaders\\raytracing.frag", BasicProgramID, out BasicFragmentShader);
        GL.LinkProgram(BasicProgramID);
        int status = 0;
        GL.GetProgram(BasicProgramID, GetProgramParameterName.LinkStatus, out status);
        Console.WriteLine(GL.GetProgramInfoLog(BasicProgramID));
    }
    public void LoadShader(string filename, int program, out int address)
    {
        address = -1;
        if (File.Exists(filename))
        {
            ShaderType type;
            switch (Path.GetExtension(filename))
            {
                case ".vert":
                    {
                        type = ShaderType.VertexShader;
                    };
                    break;
                case ".frag":
                    {
                        type = ShaderType.FragmentShader;
                    };
                    break;
                default:
                    {
                        Status = SStatus.Unknown;
                        return;
                    };
            }
            LoadShader(filename, type, program, out address);
        }
        else
        {
            Status = SStatus.NotFound;
        }
    }
    public void InitView(int ViewWidth = 800, int ViewHeight = 600)
    {
        GL.ShadeModel(ShadingModel.Smooth);
        GL.MatrixMode(MatrixMode.Projection);
        GL.LoadIdentity();
        GL.Ortho(0, Width, 0, Height, -1, 1);
        GL.Viewport(0, 0, WWidth, HHeight);
    }
    public void AddCube(float x, float y, float z, int materialIndex, float size)
    {
        if (CUBE_COUNT < CUBE_MCOUNT)
        {
            int index = CUBE_COUNT;
            CubeSizes[index] = size;
            CubeMaterials[index] = materialIndex;
            CubePositions[index] = new OpenTK.Vector3(x, y, z);
            CUBE_COUNT += 1;
        }
    }   
    public struct STriangle
    {
        public OpenTK.Vector3 v1;
        public OpenTK.Vector3 v2;
        public OpenTK.Vector3 v3;
    }
    public struct Base
    {
        public OpenTK.Vector3 top;
        public OpenTK.Vector3 left;
        public OpenTK.Vector3 right;
        public OpenTK.Vector3 bottom;
        public OpenTK.Vector3 align;
    }
    private STriangle[] CreateCube(OpenTK.Vector3 position, float size)
    {
        STriangle[] Triangles = new STriangle[2 * CUBE_MSIDES];
        Base[] basis = new Base[6];

        basis[0].top = new OpenTK.Vector3(+0, +1, +0);
        basis[0].left = new OpenTK.Vector3(-1, +0, +0);
        basis[0].right = new OpenTK.Vector3(+1, +0, +0);
        basis[0].bottom = new OpenTK.Vector3(+0, -1, +0);
        basis[0].align = new OpenTK.Vector3(+0, +0, +1);

        basis[1].top = new OpenTK.Vector3(+0, +1, +0);
        basis[1].left = new OpenTK.Vector3(-1, +0, +0);
        basis[1].right = new OpenTK.Vector3(+1, +0, +0);
        basis[1].bottom = new OpenTK.Vector3(+0, -1, +0);
        basis[1].align = new OpenTK.Vector3(+0, +0, -1);

        basis[2].top = new OpenTK.Vector3(+0, +1, +0);
        basis[2].left = new OpenTK.Vector3(+0, +0, +1);
        basis[2].right = new OpenTK.Vector3(+0, +0, -1);
        basis[2].bottom = new OpenTK.Vector3(+0, -1, +0);
        basis[2].align = new OpenTK.Vector3(-1, +0, +0);

        basis[3].top = new OpenTK.Vector3(+0, -1, +0);
        basis[3].left = new OpenTK.Vector3(+0, +0, +1);
        basis[3].right = new OpenTK.Vector3(+0, +0, -1);
        basis[3].bottom = new OpenTK.Vector3(+0, +1, +0);
        basis[3].align = new OpenTK.Vector3(+1, +0, +0);

        basis[4].top = new OpenTK.Vector3(+0, +0, +1);
        basis[4].left = new OpenTK.Vector3(-1, +0, +0);
        basis[4].right = new OpenTK.Vector3(+1, +0, +0);
        basis[4].bottom = new OpenTK.Vector3(+0, +0, -1);
        basis[4].align = new OpenTK.Vector3(+0, +1, +0);

        basis[5].top = new OpenTK.Vector3(+0, +0, +1);
        basis[5].left = new OpenTK.Vector3(-1, +0, +0);
        basis[5].right = new OpenTK.Vector3(+1, +0, +0);
        basis[5].bottom = new OpenTK.Vector3(+0, +0, -1);

        for (int i = 0; i < CUBE_MSIDES; i++)
        {
            Triangles[2 * i + 0].v1 = position + size * (basis[i].align + basis[i].right + basis[i].top);
            Triangles[2 * i + 0].v2 = position + size * (basis[i].align + basis[i].left + basis[i].bottom);
            Triangles[2 * i + 0].v3 = position + size * (basis[i].align + basis[i].right + basis[i].bottom);
            Triangles[2 * i + 1].v1 = position + size * (basis[i].align + basis[i].right + basis[i].top);
            Triangles[2 * i + 1].v2 = position + size * (basis[i].align + basis[i].left + basis[i].top);
            Triangles[2 * i + 1].v3 = position + size * (basis[i].align + basis[i].left + basis[i].bottom);
        }
        return Triangles;
    }
    private void SetUni1(string name, int value)
    {
        GL.Uniform1(GL.GetUniformLocation(BasicProgramID, name), value);
    }
    private void SetUni2(string name, OpenTK.Vector2 value)
    {
        GL.Uniform2(GL.GetUniformLocation(BasicProgramID, name), value);
    }
    private void SetUni3(string name, OpenTK.Vector3 value)
    {
        GL.Uniform3(GL.GetUniformLocation(BasicProgramID, name), value);
    }   
    private void LoadCubes()
    {
        STriangle[] Triangles = new STriangle[0];
        for (int i = 0; i < CUBE_COUNT; i++)
        {
            STriangle[] NextTriangles = CreateCube(CubePositions[i], CubeSizes[i]);
            Triangles = Triangles.Union(NextTriangles).ToArray();
        }
        for (int i = 0, CubeIndex = 0; i < Triangles.Length; i++)
        {
            SetUni1("CubeTriangles[" + i + "].MaterialIdx", CubeMaterials[CubeIndex]);
            SetUni3("CubeTriangles[" + i + "].v1", Triangles[i].v1);
            SetUni3("CubeTriangles[" + i + "].v2", Triangles[i].v2);
            SetUni3("CubeTriangles[" + i + "].v3", Triangles[i].v3);
            
            CubeIndex = i / CUBE_TRIANGLES_COUNT;
        }
    }
    private void UpdateUniforms()
    {
        SetUni1("CubeLoadedCount", CUBE_COUNT);
        SetUni1("RayTracingDepth", Math.Max(1, Math.Min(RayTracingDepth, RAYTRACING_MDEPTH)));
        SetUni3("LIGHT_POSITION", LightSourcePosition);
        SetUni3("uCamera.Position", CameraPosition);
        SetUni3("uCamera.View", new OpenTK.Vector3(0.0f, 0.0f, 1.0f));
        SetUni3("uCamera.Up", new OpenTK.Vector3(0.0f, 1.0f, 0.0f));
        SetUni3("uCamera.Side", new OpenTK.Vector3(1.0f, 0.0f, 0.0f));
        SetUni2("uCamera.Scale", new OpenTK.Vector2(1.0f, 1.0f));
        LoadCubes();
    }
    public void DrawQuads()
    {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        GL.UseProgram(BasicProgramID);
        UpdateUniforms();
        GL.Begin(PrimitiveType.Quads);
        GL.Vertex2(-Width, -Height);
        GL.Vertex2(+Width, -Height);
        GL.Vertex2(+Width, +Height);
        GL.Vertex2(-Width, +Height);
        GL.End();
    }
}