# Majala (Managed Java Launcher)
Majala is a Java launcher (such as [Launch4J](http://launch4j.sourceforge.net/), [WinRun4J](http://winrun4j.sourceforge.net/), 
[JSmooth](http://jsmooth.sourceforge.net/) or [Janel](http://www.redskaper.com/)). Majala differs from these other launchers
in a little detail. Majala is written in C# and so it is launched as a managed process.

# Current state
Majala is not a ready to use product. I even don't know if it will ever become one. The idea of Majala came up when I've
looked around for a 64 bit Java launcher. I was prettey disappointed by the available solutions so I decided to write
my own. But as time goes by I noticed the Java launcher [Janel](http://www.redskaper.com/). I did some
small enhancements to it and was quite happy with [Janel](http://www.redskaper.com/).

# Goals
When I decided to write Majala I had the following goals in mind:
* **Bitness:** The Java launcher is available in a 32bit and 64bit flavour.
* **Application types:** Majala supports various application types, i. e. console applications, graphical applications and Windows services. For every application type majala provides a separate exe-wrapper. 
* **Configuration:** The runtime configuration for the Java JVM can be provided by a simple line oriented configuration file (like a well known .ini file) or with an XML file. This file can be read as a regular file from the file system or can be embedded in the majala exe-wrapper (so it may be hidden from the user). 
* **Logging:** In case of problems, a detailed log of all majala activities, like the selection of the JVM, can be logged and analyzed. By default majala does not print any log messages, but just some simple steps are required to let majala produce a verbose log at runtime. 
* **Self modifying:** When creating the exe-wrapper for the Java application, the Majala executable is able to modify itself. It is able to change its icon and to embed the runtime configuration file to its own executable. After the exe-wrapper is created, no changes are allowed anymore (the created exe is *locked*). An other idea is that there is a exe-wrapper creator that runs as a console application (so it can also run unter Linux using [Mono](http://www.mono-project.com/)). This tool can be used to create the exe-wrapper (so this tool has the different wrappers (Windows service, command line application, GUI application) embedded as resources and can extract and modify them).

# Biggest problem
The biggest problem is the requirement of the .NET Framework at runtime, because every .NET executable needs the .NET Framework
it was built for (so to get a Java application up and running you need the Java Runtime **AND** a .NET Runtime, which can irritate
the end user of the application). I hope that some day Microsoft will allow us to build native binaries from C# code that will run on x86 and x64
platforms.

# Licence
Majala has been dedicated to the public domain. But if you did some enhancements or bug fixes to Majala please provide me with the changes (pull requests) so the community can participate from your changes. This is how the community works...
