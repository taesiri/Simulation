#IronRuby Scripts Goes Here!

def GeneratorFunc(min,max)
	if min < max then
		return rand(min..max)
	else
		return rand(max..min)
	end
end

def userDefinedFunc(x)
	return random.randint(0,x)
end

