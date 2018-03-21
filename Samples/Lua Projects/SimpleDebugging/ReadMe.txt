***********************************************
** Simple Debugging
***********************************************

This project is used to demonstrate how to debug locally using
projects and scripts.

To fully test this sample, set the "SimpleDebugging" project
as the startup project by right-clicking on it in the Solution Explorer
window and selecting the "Set as StartUp Project" menu. Then, open the
script "ExternalProgramDebug.lua" and add breakpoints to it. Start
debugging by using the "Debug/Start Debugging" menu or hit F5. This
should intiate a local debug session and automatically run the script
"SimpleDebug.lua" since it is specified to do so in the project properties.

LuaEdit should break on the lines where breakpoints you added
are located.

REMARK: When starting a local debug session, you do not need to specify
		the port number since the debugging is done locally.