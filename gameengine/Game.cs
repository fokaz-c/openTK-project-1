﻿using StbImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;


namespace gameengine;

internal class Game : GameWindow
{

    float[] vertices =
        {
            -0.5f, 0.5f, 0f, // top left vertex - 0
            0.5f, 0.5f, 0f, // top right vertex - 1
            0.5f, -0.5f, 0f, // bottom right - 2
            -0.5f, -0.5f, 0f // bottom left - 3
        };

    float[] texCoords =
    {
            0f, 1f,
            1f, 1f,
            1f, 0f,
            0f, 0f
        };

    uint[] indices =
    {
            // top triangle
            0, 1, 2,
            // bottom triangle
            2, 3, 0
        };


    //render pipeline variables
    int vao;
    int shaderProgram;
    int vbo;
    int ebo;
    int textureID;
    int textureVBO;



    //Constants
    int width, height;

    public Game(int width, int height) : base(GameWindowSettings.Default, NativeWindowSettings.Default)
    {
        //center the window on monitor

        this.CenterWindow(new Vector2i(width, height));
        this.width = width;
        this.height = height;


    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        GL.Viewport(0, 0, e.Width, e.Height);
        this.width = e.Width;
        this.height = e.Height;
    }


    protected override void OnLoad()
    {
        base.OnLoad();

        // generate the vbo
        vao = GL.GenVertexArray();

        // bind the vao
        GL.BindVertexArray(vao);

        // --- Vertices VBO ---

        // generate a buffer
        vbo = GL.GenBuffer();
        // bind the buffer as an array buffer
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        // Store data in the vbo
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);


        // put the vertex VBO in slot 0 of our VAO

        // point slot (0) of the VAO to the currently bound VBO (vbo)
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
        // enable the slot
        GL.EnableVertexArrayAttrib(vao, 0);

        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

        // --- Texture VBO ---

        textureVBO = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, textureVBO);
        GL.BufferData(BufferTarget.ArrayBuffer, texCoords.Length * sizeof(float), texCoords, BufferUsageHint.StaticDraw);


        // put the texture VBO in slot 1 of our VAO

        // point slot (1) of the VAO to the currently bound VBO (vbo)
        GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
        // enable the slot
        GL.EnableVertexArrayAttrib(vao, 1);

        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

        // unbind the vbo and vao respectively

        GL.BindVertexArray(0);


        ebo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);


        // create the shader program
        shaderProgram = GL.CreateProgram();

        // create the vertex shader
        int vertexShader = GL.CreateShader(ShaderType.VertexShader);
        // add the source code from "Default.vert" in the Shaders file
        GL.ShaderSource(vertexShader, LoadShaderSource("Default.vert"));
        // Compile the Shader
        GL.CompileShader(vertexShader);

        // Same as vertex shader
        int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentShader, LoadShaderSource("Default.frag"));
        GL.CompileShader(fragmentShader);

        // Attach the shaders to the shader program
        GL.AttachShader(shaderProgram, vertexShader);
        GL.AttachShader(shaderProgram, fragmentShader);

        // Link the program to OpenGL
        GL.LinkProgram(shaderProgram);

        // delete the shaders
        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragmentShader);

        // --- TEXTURES ---
        textureID = GL.GenTexture();
        // activate the texture in the unit
        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2D, textureID);

        // texture parameters
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

        // load image
       
        StbImage.stbi_set_flip_vertically_on_load(1);
        try
        {
            ImageResult dirtTexture = ImageResult.FromStream(File.OpenRead("../../../Textures/dirt.png"), ColorComponents.RedGreenBlueAlpha);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, dirtTexture.Width, dirtTexture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, dirtTexture.Data);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to load texture: " + ex.Message);
        }

    }

    protected override void OnUnload()
    {
        base.OnUnload();

        GL.DeleteVertexArray(vao);
        GL.DeleteBuffer(vbo);
        GL.DeleteBuffer(ebo);
        GL.DeleteProgram(shaderProgram);
        GL.DeleteTexture(textureID);
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        GL.ClearColor(0.98f, 0.67f, 0.52f, 1.0f);
        GL.Clear(ClearBufferMask.ColorBufferBit);

        //draw triangle
        GL.UseProgram(shaderProgram);

        GL.BindTexture(TextureTarget.Texture2D, textureID);
        GL.BindVertexArray(vao);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
        GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);

        //GL.DrawArrays(PrimitiveType.Triangles, 0, 4);

        Context.SwapBuffers();

        base.OnRenderFrame(args);
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);
    }

    // Function to load a text file and return its contents as a string
    public static string LoadShaderSource(string filePath)
    {
        string shaderSource = "";

        try
        {
            using (StreamReader reader = new StreamReader("../../../Shaders/" + filePath))
            {
                shaderSource = reader.ReadToEnd();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to load shader source file: " + e.Message);
        }

        return shaderSource;
    }


}
