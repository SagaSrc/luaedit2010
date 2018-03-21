local x = 5
local t = { 1, 2.25, 3.5, { "test", "more test" } }

print("local variables t and x are worth:")
print(t, x)

function IncrementX(inc)
	x = x + inc
	return x
end

IncrementX(5)
print(x)
IncrementX(100)
print(x)
IncrementX(-20)
print(x)

print("*** End Of Script ***")