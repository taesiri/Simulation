import sys
import clr
clr.AddReference("IronPython")
clr.AddReference("IronPython.Modules")
sys.path.append(r'C:\Program Files (x86)\IronPython 2.7\Lib')

import random

def function(min,max):
	if min < max:
		return random.randint(min,max)
	else:
		return random.randint(max,min)

def myMethod(x):
	return random.randint(0,x)
