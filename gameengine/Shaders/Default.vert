#version 330 core 

layout (location = 0) in vec3 aPosition; // vertex coordinates
layout (location = 1) in vec2 aTexCoord; // texture coordinates

out vec2 texCoord; // declare texCoord as an output variable

void main()
{
    gl_Position = vec4(aPosition, 1.0); // coordinates
    texCoord = aTexCoord; // assign the texture coordinates to the output variable
}
