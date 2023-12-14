# OpenTK Project 1

This is a simple OpenTK project that demonstrates a basic 3D rendering setup using OpenGL. The project includes a rotating textured cube and utilizes the OpenTK library for window creation and OpenGL bindings.

## Table of Contents
- [Features](#features)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
- [Usage](#usage)
- [Project Structure](#project-structure)
- [Contributing](#contributing)
- [License](#license)

## Features

- 3D rendering of a textured cube.
- Basic camera and transformation controls.
- Loading of textures using StbImageSharp.
- Simple shader setup with vertex and fragment shaders.
- Resource cleanup on application exit.

## Getting Started

### Prerequisites

- .NET SDK (version X.X.X or later) [Installation Guide](https://dotnet.microsoft.com/download)
- OpenTK library

### Installation

1. Clone the repository:

    ```bash
    git clone https://github.com/your-username/OpenTK-Project1.git
    cd OpenTK-Project1
    ```

2. Build and run the project:

    ```bash
    dotnet build
    dotnet run
    ```

## Usage

- Upon running the application, a window will open displaying a rotating textured cube.
- Use keyboard and mouse inputs for camera and interaction controls (specifics can be added here).

## Project Structure

- `Game.cs`: Main class that inherits from OpenTK's `GameWindow` and contains the game logic, rendering, and setup.
- `Shaders/`: Directory containing vertex and fragment shader source files.
- `Textures/`: Directory containing texture files used in the project.

## Contributing

Contributions are welcome! If you find a bug or have suggestions for improvement, please open an issue or submit a pull request.

## License

This project is licensed under the [MIT License](LICENSE).
