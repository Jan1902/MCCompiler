<div id="top"></div>

<!-- PROJECT LOGO -->
<br />
<div align="center">
  <!--
  <a href="https://github.com/othneildrew/Best-README-Template">
    <img src="images/logo.png" alt="Logo" width="80" height="80">
  </a>
  -->

  <h2 align="center">MC Compiler</h2>

  <p align="center">
    A simple compiler to make programming Minecraft CPUs easier!
    <br />
</a>
    <br />
    ·
    <a href="https://github.com/Jan1902/MCCompiler/issues">Report Bug</a>
    ·
    <a href="https://github.com/Jan1902/MCCompiler/issues">Request Feature</a>
  </p>
</div>

<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
    <li><a href="#acknowledgments">Acknowledgments</a></li>
  </ol>
</details>

<!-- ABOUT THE PROJECT -->
## About The Project

Over time Minecraft CPUs have grown bigger and more powerful - to a point where you're able to play games like Pong and Tetris on them. Unfortunately inputting such big programs by hand is a pain. This project is supposed to make this process a lot easier by providing an easy-to-use programming language aswell as a program for generating customizable schematics from your code.

### Built With

_The project is pretty much made from scratch besides some ease-of-life libraries._

* [.NET](https://dotnet.microsoft.com/en-us/)
* [fNBT](https://github.com/mstefarov/fNbt)
* [CommandLineParser](https://github.com/commandlineparser/commandline)

<!-- GETTING STARTED -->
## Getting Started

_To get the MC Compiler up and running follow these simple steps._

### Prerequisites

* Visual Studio 2022
* .NET 6

### Installation

_Installing the project only requires you to clone the repo and open it up in VS 2022_

1. Clone the repo

		git clone https://github.com/Jan1902/MCCompiler.git

2. Open it up in VS 2022

<!-- USAGE EXAMPLES -->
## Usage

_Both the compiler and the Schematic Generator are operated through the command line making it easy to use._

### Compiler
#### Syntax
	CompilerTest.exe -c <CODE-FILE> -i <INSTRUCTION-SET-FILE> -o <OUTPUT-FILE>
#### Example
    CompilerTest.exe -c code.txt -i is.txt -o result.txt
    
### Schematic Generator
#### Syntax
	SchematicGenerator.exe <BINARY-FILE>
#### Example
    SchematicGenerator.exe result.txt

<!-- ROADMAP -->
## Roadmap

- [x] Compiler
	- [x] Loading Instruction Sets
	- [x] Tokenizer
    - [x] Parser
        - [ ] Better Syntax Error Recognition
    - [ ] Support different CPU architectures
    - [x] Transformer
    - [x] Translator
        - [ ] Handling nested calculations
    - [x] Proper CLI
	- [ ] Automatically move variables between ram and registers
    - [ ] Automatically create temporary variables when needed
- [ ] Schematic Generator
	- [ ] Configuration
	- [ ] Command Line Parser

See the [open issues](https://github.com/Jan1902/MCCompiler/issues) for a full list of proposed features (and known issues).

<!-- CONTRIBUTING -->
## Contributing

Contributions are what make the open source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

If you have a suggestion that would make this better, please fork the repo and create a pull request. You can also simply open an issue with the tag "enhancement".
Don't forget to give the project a star! Thanks again!

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

<!-- LICENSE -->
## License

Distributed under the MIT License. See `LICENSE` for more information.

<!-- CONTACT -->
## Contact

Jan-Hendrik Heinbokel - jan.heinbokel@gmx.de

Project Link: [https://github.com/Jan1902/MCCompiler](https://github.com/Jan1902/MCCompiler)
