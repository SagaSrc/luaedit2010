***********************************************
** External Program Debugging
***********************************************

This project is used to demonstrate how to debug an external
program specified in the project properties under the field
"Start external program". When such option is specified in
the project settings, LuaEdit will start the program then
attach to it and initiate a debug session.

To fully test this sample, set the "ExternalProgramDebugging" project
as the startup project by right-clicking on it in the Solution Explorer
window and selecting the "Set as StartUp Project" menu. Then, open the
script "ExternalProgramDebug.lua" and add breakpoints to it. Start
debugging by using the "Debug/Start Debugging" menu or hit F5. This
should launch the currently specified program lua_rdbg.exe which is
basically lua.exe using the LuaEdit remote debugger. In the program's
command prompt, type-in the following line:

dofile("../../../Lua Projects/ExternalProgramDebugging/ExernalProgramDebug.lua")

LuaEdit should break on the lines where breakpoints you added
are located. LuaEdit should also recongnize the "./ExernalProgramDebug.lua"
since the working directory in the project settings is defined
as being the directory representing "./" in "./ExernalProgramDebug.lua".