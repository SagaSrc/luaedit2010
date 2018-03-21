print("Simple Debugging Script Start!")

local v = { 1, 'test', { 2, 3.3333 }}
local z = _G
local x = 0

function CallDepthTest()
	x = x + 1
	if x <= 10 then
		print(x)
		CallDepthTest()
	end
end

print(v, z)
CallDepthTest()
print("*** End Of Script ***")