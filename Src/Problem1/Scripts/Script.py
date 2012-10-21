import clr
clr.AddReference("IronPython")
clr.AddReference("IronPython.Modules")

import sys
#User must reference IronPython Installation Path below
sys.path.append(r'C:\Program Files (x86)\IronPython 2.7\Lib')

import random

def GeneratorFunc(min,max):
	if min < max:
		return random.randint(min,max)
	else:
		return random.randint(max,min)

def userDefinedFunc(x):
	return random.randint(0,x)

def DelayMapper(x):
	if x >= 0 and x < 10:
		return 1
	elif x >= 10 and x < 20:
		return 2
	elif x >= 20 and x<30:
		return 3
	elif x >= 30 and x < 40:
		return 4
	elif x >= 40 and x<50:
		return 5
	elif x >= 50 and x < 60:
		return 6
	elif x >= 60 and x < 70:
		return 7
	elif x >= 70 and x<80:
		return 8
	elif x >= 80 and x <= 90:
		return 9
	else:
		return 4;

def LifeSpanMapper(x):
	if x >= 0 and x < 10:
		return 1000
	elif x >= 10 and x < 20:
		return 1100
	elif x >= 20 and x<30:
		return 1200
	elif x >= 30 and x < 40:
		return 1300
	elif x >= 40 and x<50:
		return 1400
	elif x >= 50 and x < 60:
		return 1500
	elif x >= 60 and x < 70:
		return 1600
	elif x >= 70 and x<80:
		return 1700
	elif x >= 80 and x<90:
		return 1800
	elif x >= 90 and x <= 100:
		return 1900
	else:
		return 1500
